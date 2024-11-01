using Moq;
using Moq.Protected;
using System.Net;
using System.Text.Json;
using SeoChecker.Models;
using SeoChecker.Services.SearchEngines;

namespace SeoChecker.Tests.Services
{
    public class GoogleSearchEngineTests
    {
        private readonly GoogleSearchEngine _googleSearchEngine;
        private readonly Mock<HttpMessageHandler> _httpMessageHandler;

        public GoogleSearchEngineTests()
        {
            _httpMessageHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            var httpClient = new HttpClient(_httpMessageHandler.Object);
            _googleSearchEngine = new GoogleSearchEngine(httpClient, "fakeApiKey", "fakeSearchEngineId");
        }

        [Fact]
        public async Task SearchAsync_ReturnsPositions_WhenUrlIsFound()
        {
            // Arrange
            var expectedPositions = new List<int> { 1, 5, 10 };
            var keywords = "test keywords";
            var url = "example.com";

            // Page 1: First 10 results, 3 of which match the target URL
            var firstPageResults = new GoogleSearchResult
            {
                Items = new List<GoogleSearchResultItem>
                {
                    new GoogleSearchResultItem { Link = "http://example.com" },  // Position 1
                    new GoogleSearchResultItem { Link = "http://another.com" },
                    new GoogleSearchResultItem { Link = "http://another.com" },
                    new GoogleSearchResultItem { Link = "http://another.com" },
                    new GoogleSearchResultItem { Link = "http://example.com" },  // Position 5
                    new GoogleSearchResultItem { Link = "http://another.com" },
                    new GoogleSearchResultItem { Link = "http://another.com" },
                    new GoogleSearchResultItem { Link = "http://another.com" },
                    new GoogleSearchResultItem { Link = "http://another.com" },
                    new GoogleSearchResultItem { Link = "http://example.com" }   // Position 10
                }
            };

            // Page 2: Next 10 results, all non-matching
            var secondPageResults = new GoogleSearchResult
            {
                Items = new List<GoogleSearchResultItem>
                {
                    new GoogleSearchResultItem { Link = "http://anotherdomain.com/1" },
                    new GoogleSearchResultItem { Link = "http://anotherdomain.com/2" },
                    new GoogleSearchResultItem { Link = "http://anotherdomain.com/3" },
                    new GoogleSearchResultItem { Link = "http://anotherdomain.com/4" },
                    new GoogleSearchResultItem { Link = "http://anotherdomain.com/5" },
                    new GoogleSearchResultItem { Link = "http://anotherdomain.com/6" },
                    new GoogleSearchResultItem { Link = "http://anotherdomain.com/7" },
                    new GoogleSearchResultItem { Link = "http://anotherdomain.com/8" },
                    new GoogleSearchResultItem { Link = "http://anotherdomain.com/9" },
                    new GoogleSearchResultItem { Link = "http://anotherdomain.com/10" }
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
            var positions = await _googleSearchEngine.SearchAsync(keywords, url);

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

            // Set up a GoogleSearchResult with no matching URLs
            var searchResult = new GoogleSearchResult
            {
                Items = new List<GoogleSearchResultItem>
                {
                    new GoogleSearchResultItem { Link = "http://example.com" },
                    new GoogleSearchResultItem { Link = "http://another.com" }
                }
            };

            // Serialize the GoogleSearchResult object to JSON
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
            var positions = await _googleSearchEngine.SearchAsync(keywords, url);

            // Assert
            Assert.Equal(expectedPositions, positions);
        }
    }
}
