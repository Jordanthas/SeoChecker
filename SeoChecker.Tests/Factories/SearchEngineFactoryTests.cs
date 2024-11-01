using Microsoft.Extensions.Configuration;
using Moq;
using SeoChecker.Factories;
using SeoChecker.Repositories;
using SeoChecker.Services;
using SeoChecker.Interfaces;
using Xunit;
using SeoChecker.Models;
using SeoChecker.Services.SearchEngines;

namespace SeoChecker.Tests.Factories
{
    public class SearchEngineFactoryTests
    {
        private readonly Mock<IDataRepository> _dataRepositoryMock;
        private readonly IConfiguration _configuration;

        public SearchEngineFactoryTests()
        {
            _dataRepositoryMock = new Mock<IDataRepository>();

            // Set up in-memory configuration for Google and Bing search engines
            var inMemorySettings = new Dictionary<string, string>
            {
                {"SearchEngines:Google:ApiKey", "fake-google-api-key"},
                {"SearchEngines:Google:SearchEngineId", "fake-google-search-engine-id"},
                {"SearchEngines:Bing:ApiKey", "fake-bing-api-key"},
                {"SearchEngines:Bing:Endpoint", "https://api.bing.microsoft.com/v7.0/search"}
            };

            _configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            // Initialize the factory with the configuration
            SearchEngineFactory.Initialize(_configuration);
        }

        [Fact]
        public void Create_GoogleSearchEngine_ReturnsGoogleSearchEngineInstance()
        {
            // Act
            var searchEngine = SearchEngineFactory.Create(SearchEngineType.Google, _dataRepositoryMock.Object);

            // Assert
            Assert.NotNull(searchEngine);
            Assert.IsType<CachedSearchService>(searchEngine);

            // Verify the inner search engine is GoogleSearchEngine
            var cachedSearchService = searchEngine as CachedSearchService;
            Assert.IsType<GoogleSearchEngine>(cachedSearchService.InnerSearchEngine);
        }

        [Fact]
        public void Create_BingSearchEngine_ReturnsBingSearchEngineInstance()
        {
            // Act
            var searchEngine = SearchEngineFactory.Create(SearchEngineType.Bing, _dataRepositoryMock.Object);

            // Assert
            Assert.NotNull(searchEngine);
            Assert.IsType<CachedSearchService>(searchEngine);

            // Verify the inner search engine is BingSearchEngine
            var cachedSearchService = searchEngine as CachedSearchService;
            Assert.IsType<BingSearchEngine>(cachedSearchService.InnerSearchEngine);
        }

        [Fact]
        public void Create_InvalidSearchEngineType_ThrowsArgumentException()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() =>
                SearchEngineFactory.Create((SearchEngineType)999, _dataRepositoryMock.Object));
        }
    }
}
