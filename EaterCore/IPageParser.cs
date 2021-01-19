namespace EaterCore
{
    /// <summary>
    /// Interface for page parsing functionality.
    /// </summary>
    public interface IPageParser
    {
        /// <summary>
        /// Parses a recipe from raw page markup.
        /// </summary>
        /// <param name="rawMarkup">The raw page markup.</param>
        /// <returns>The raw recipe markup if it exists. Otherwise returns an empty string.</returns>
        string ParseRecipeFromMarkup(string rawMarkup);
    }
}