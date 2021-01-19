using System;
using System.Net.Http;
using System.Threading.Tasks;
using EaterCore;

namespace menueater.cli
{
    class Program
    {
        static readonly HttpClient client = new HttpClient();

        static async Task<int> Main(string[] args)
        {
            Console.WriteLine("Please enter a web page address of a food recipe:");

            var address = Console.ReadLine();

            try
            {
                var webpage = new Uri(address);

                var parser = new PageParser();
                var pageEater = new EaterMain(client, parser);

                var recipe = await pageEater.ScrapeWebPage(webpage);

                Console.WriteLine(recipe);

                return 0;
            }
            catch (UriFormatException e)
            {
                Console.WriteLine($"Sorry, {address} is not a valid url.");
                return 1;
            }
        }
    }
}
