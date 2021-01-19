using System;
using Xunit;
using Moq;
using Moq.Protected;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using EaterCore.Exceptions;

namespace EaterCore.Tests
{
    public class EaterMainTest
    {
        private Uri testWebpageAddress = new Uri("http://example.com/testaddress");

        private HttpClient client;

        private Mock<HttpMessageHandler> mockMessageHandler;

        private Mock<IPageParser> pageParser;

        public EaterMainTest()
        {
            mockMessageHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            pageParser = new Mock<IPageParser>();

            client = new HttpClient(mockMessageHandler.Object);
        }

        [Fact]
        public async Task ThrowsWhenWebAddressNotFound()
        {
            mockMessageHandler.Protected().Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.NotFound,
                Content = new StringContent("")
            });

            var main = new EaterMain(client, pageParser.Object);

            await Assert.ThrowsAsync<NoRecipePresentException>(
                async () => await main.ScrapeWebPage(testWebpageAddress)
            );
        }

        [Fact]
        public async Task ThrowsWhenNoRecipeMarkupExistsInPage()
        {
            mockMessageHandler.Protected().Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent("Some markup but no recipe")
            });

            pageParser.Setup(p => p.ParseRecipeFromMarkup(It.IsAny<string>()))
                .Returns(string.Empty);

            var main = new EaterMain(client, pageParser.Object);

            await Assert.ThrowsAsync<NoRecipePresentException>(
                async () => await main.ScrapeWebPage(testWebpageAddress)
            );
        }

        [Fact]
        public async Task ReturnsRecipeMarkupWhenPageContainsIt()
        {
            mockMessageHandler.Protected().Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent("<some_markup_that_contains>\"@type\": \"Recipe\"</some_markup_that_contains>")
            });

            pageParser.Setup(p => p.ParseRecipeFromMarkup(It.IsAny<string>()))
                .Returns("\"@type\": \"Recipe\"");

            var main = new EaterMain(client, pageParser.Object);

            var recipeMarkup = await main.ScrapeWebPage(new Uri("http://example.com/recipe1"));

            Assert.Contains(("\"@type\": \"Recipe\""), recipeMarkup);
        }
    }
}
