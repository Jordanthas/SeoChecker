using Moq;
using Moq.Protected;
using SeoChecker.Services;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using SeoChecker.Models;
using Xunit;
using SeoChecker.Services.SearchEngines;

namespace SeoChecker.Tests.Services
{
    public class BingSearchEngineTests
    {
        private readonly BingSearchEngine _bingSearchEngine;
        private readonly Mock<HttpMessageHandler> _httpMessageHandler;

        public BingSearchEngineTests()
        {
            _httpMessageHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            var httpClient = new HttpClient(_httpMessageHandler.Object);
            _bingSearchEngine = new BingSearchEngine(httpClient, "fakeApiKey", "https://api.bing.microsoft.com/v7.0/search");
        }

        [Fact]
        public async Task SearchAsync_ReturnsPositions_WhenUrlIsFound()
        {
            // Arrange
            var expectedPositions = new List<int> { 1, 5, 10 };
            var keywords = "test keywords";
            var url = "example.com";

            // Page 1: First 10 results, 3 of which match the target URL
            var firstPageResults = new BingSearchResult
            {
                WebPages = new WebPages
                {
                    Items = new List<BingSearchResultItem>
                    {
                        new BingSearchResultItem { Url = "http://example.com" },  // Position 1
                        new BingSearchResultItem { Url = "http://another.com" },
                        new BingSearchResultItem { Url = "http://another.com" },
                        new BingSearchResultItem { Url = "http://another.com" },
                        new BingSearchResultItem { Url = "http://example.com" },  // Position 5
                        new BingSearchResultItem { Url = "http://another.com" },
                        new BingSearchResultItem { Url = "http://another.com" },
                        new BingSearchResultItem { Url = "http://another.com" },
                        new BingSearchResultItem { Url = "http://another.com" },
                        new BingSearchResultItem { Url = "http://example.com" }   // Position 10
                    }
                }
            };

            // Page 2: Next 10 results, all non-matching
            var secondPageResults = new BingSearchResult
            {
                WebPages = new WebPages
                {
                    Items = new List<BingSearchResultItem>
                    {
                        new BingSearchResultItem { Url = "http://differentdomain.com/1" },
                        new BingSearchResultItem { Url = "http://differentdomain.com/2" },
                        new BingSearchResultItem { Url = "http://differentdomain.com/3" },
                        new BingSearchResultItem { Url = "http://differentdomain.com/4" },
                        new BingSearchResultItem { Url = "http://differentdomain.com/5" },
                        new BingSearchResultItem { Url = "http://differentdomain.com/6" },
                        new BingSearchResultItem { Url = "http://differentdomain.com/7" },
                        new BingSearchResultItem { Url = "http://differentdomain.com/8" },
                        new BingSearchResultItem { Url = "http://differentdomain.com/9" },
                        new BingSearchResultItem { Url = "http://differentdomain.com/10" }
                    }
                }
            };

            // Serialize each page to JSON for mock response
            var firstPageJson = JsonSerializer.Serialize(firstPageResults);
            var secondPageJson = JsonSerializer.Serialize(secondPageResults);

            // Set up the mock to return first page results on the first call,
            // second page results on the second call, and keep returning second page results for subsequent calls.
            _httpMessageHandler.Protected()
                .SetupSequence<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(firstPageJson)
                })
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(secondPageJson)
                })
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(secondPageJson)
                })
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(secondPageJson)
                })
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(secondPageJson)
                })
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(secondPageJson)
                })
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(secondPageJson)
                })
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(secondPageJson)
                })
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(secondPageJson)
                })
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(secondPageJson)
                });

            // Act
            var positions = await _bingSearchEngine.SearchAsync(keywords, url);

            // Assert
            Assert.Equal(expectedPositions, positions);

            // Verify that SendAsync was called at least twice
            _httpMessageHandler.Protected().Verify(
                "SendAsync",
                Times.AtLeast(2),
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>());
        }

        [Fact]
        public async Task SearchAsync_ReturnsZero_WhenUrlIsNotFound()
        {
            // Arrange
            var expectedPositions = new List<int> { 0 };
            var keywords = "test keywords";
            var url = "http://notfound.com";

            // Set up a BingSearchResult with no matching URLs
            var searchResult = new BingSearchResult
            {
                WebPages = new WebPages
                {
                    Items = new List<BingSearchResultItem>
                    {
                        new BingSearchResultItem { Url = "http://example.com" },
                        new BingSearchResultItem { Url = "http://another.com" }
                    }
                }
            };

            // Serialize the BingSearchResult object to JSON
            var jsonResponse = JsonSerializer.Serialize(searchResult);

            // Setup the mock to return the JSON response
            _httpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(jsonResponse)
                });

            // Act
            var positions = await _bingSearchEngine.SearchAsync(keywords, url);

            // Assert
            Assert.Equal(expectedPositions, positions);
        }
    }
}
