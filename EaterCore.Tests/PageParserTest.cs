using System.IO;
using Xunit;

namespace EaterCore.Tests
{
    public class PageParserTest
    {
        private PageParser parser;

        public PageParserTest()
        {
            parser = new PageParser();
        }

        [Fact]
        public void ReturnsEmptyStringWhenNoRecipeDataExists()
        {
            var recipe = parser.ParseRecipeFromMarkup(ExtractMarkupFromFile("NoRecipe.html"));

            Assert.Empty(recipe);
        }

        [Fact]
        public void ReturnsEmptyStringWhenStructuredRecipeDataIsMissingClosingBrace()
        {
            var originalRecipe = ExtractMarkupFromFile("Recipe1.json");
            var alteredRecipe = originalRecipe.Substring(0, originalRecipe.Length - 1);
            var webPageWithRecipe = CreateWebpageWithRecipe("WebpageWithRecipe.html", alteredRecipe);
            var recipe = parser.ParseRecipeFromMarkup(webPageWithRecipe);
            
            Assert.Empty(recipe);
        }

        [Fact]
        public void ReturnsRawRecipeTextWhenStructuredRecipeDataExists()
        {
            var originalRecipe = ExtractMarkupFromFile("Recipe1.json");
            var webPageWithRecipe = CreateWebpageWithRecipe("WebpageWithRecipe.html", originalRecipe);
            var recipe = parser.ParseRecipeFromMarkup(webPageWithRecipe);
            
            Assert.Equal(originalRecipe, recipe);
        }

        private string ExtractMarkupFromFile(string fileName)
        {
            var currentDir = Directory.GetCurrentDirectory();
            var path = Path.Combine(currentDir, "fixtures", fileName);

            return File.ReadAllText(path);
        }

        private string CreateWebpageWithRecipe(string webpageFilePath, string recipeRawFile)
        {
            var webpage = ExtractMarkupFromFile(webpageFilePath);

            return webpage.Replace("{{recipe_structured_data}}", recipeRawFile);
        }
    }
}
