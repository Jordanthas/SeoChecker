using Microsoft.Extensions.Configuration;
using SeoChecker.Interfaces;
using SeoChecker.Models;
using SeoChecker.Services;
using SeoChecker.Services.SearchEngines;

namespace SeoChecker.Factories
{
    public class SearchEngineFactory
    {
        private static IConfiguration _configuration;

        public static void Initialize(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public static ISearchEngine Create(SearchEngineType engineType, IDataRepository dataRepository)
        {
            if (_configuration == null)
                throw new InvalidOperationException("Facotry not initialized with configuration");

            return engineType switch
            {
                SearchEngineType.Google => CreateGoogleSearchEngine(dataRepository),
                SearchEngineType.Bing => CreateBingSearchEngine(dataRepository),
                _ => throw new ArgumentException("Unsupported search engine")
            };
        }

        private static ISearchEngine CreateGoogleSearchEngine(IDataRepository dataRepository)
        {
            var googleSettings = _configuration.GetSection("SearchEngines:Google");
            var apiKey = googleSettings["ApiKey"];
            var searchEngineId = googleSettings["SearchEngineId"];

            var googleEngine = new GoogleSearchEngine(new HttpClient(), apiKey, searchEngineId);
            return new CachedSearchService(googleEngine, dataRepository);
        }

        private static ISearchEngine CreateBingSearchEngine(IDataRepository dataRepository)
        {
            var bingSettings = _configuration.GetSection("SearchEngines:Bing");
            var apiKey = bingSettings["ApiKey"];
            var endPoint = bingSettings["Endpoint"];

            var bingEngine = new BingSearchEngine(new HttpClient(), apiKey, endPoint);
            return new CachedSearchService(bingEngine, dataRepository);
        }
    }
}
