using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using EaterCore.Exceptions;

namespace EaterCore
{
    /// <summary>
    /// Main logic class for obtaining recipe data and parsing it.
    /// </summary>
    public class EaterMain
    {
        private HttpClient client;

        private IPageParser parser;

        /// <summary>
        /// Constructor for <see cref="EaterMain"/>.
        /// </summary>
        public EaterMain(HttpClient client, IPageParser parser)
        {
            this.client = client;
            this.parser = parser;
        }

        /// <summary>
        /// Scrapes a web page by address and return the raw recipe markup, if it exists.
        /// </summary>
        public async Task<string> ScrapeWebPage(Uri address)
        {
            var response = await client.GetAsync(address.ToString());

            if (response.StatusCode != HttpStatusCode.OK)
                throw new NoRecipePresentException("Sorry, this webpage can't be found.");

            var rawContent = await response.Content.ReadAsStringAsync();
            var recipe = parser.ParseRecipeFromMarkup(rawContent);

            if (recipe == string.Empty)
                throw new NoRecipePresentException("Sorry, this webpage doesn't contain a recipe we can read.");
            
            return recipe;
        }
    }
}
