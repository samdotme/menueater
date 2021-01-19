namespace EaterCore
{
    /// <inheritdoc cref="IPageParser"/>
    public class PageParser : IPageParser
    {
        /// <inheritdoc cref="IPageParser.ParseRecipeFromMarkup(string)"/>
        public string ParseRecipeFromMarkup(string rawMarkup)
        {
            var searchString = @"@type"": ""Recipe";

            var matchLoc = rawMarkup.IndexOf(searchString);

            if (matchLoc < 0)
            {
                return "";
            }
            
            var beginningOfRecipe = FindIndexOfRecipeStart(matchLoc, rawMarkup);

            if (beginningOfRecipe == -1)
                return "";

            var endOfRecipe = FindIndexOfRecipeEnd(matchLoc, rawMarkup);

            if (endOfRecipe == -1)
                return "";

            return rawMarkup.Substring(beginningOfRecipe, endOfRecipe - beginningOfRecipe);
        }

        private int FindIndexOfRecipeStart(int lookBehindStart, string rawMarkup)
        {
            var currentIndex = lookBehindStart;

            while (currentIndex >= 0 && rawMarkup[currentIndex] != '{') 
            {
                currentIndex -= 1;
            }

            if (rawMarkup[currentIndex] == '{')
                return currentIndex;

            return -1;
        }

        private int FindIndexOfRecipeEnd(int lookAheadStart, string rawMarkup)
        {
            var curIndex = lookAheadStart;
            var curLevel = 1;

            while (curIndex < rawMarkup.Length && curLevel > 0)
            {
                if (rawMarkup[curIndex] == '{')
                    curLevel += 1;
                else if (rawMarkup[curIndex] == '}')
                    curLevel -= 1;

                curIndex += 1;
            }

            if (curLevel == 0)
                return curIndex;

            return -1;
        }
    }
}
