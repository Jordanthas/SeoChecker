using SeoChecker.Interfaces;
using SeoChecker.Utilities;

namespace SeoChecker.Services
{
    public class CachedSearchService : ISearchEngine
    {
        public ISearchEngine InnerSearchEngine { get; }

        private readonly IDataRepository _dataRepository;

        public CachedSearchService(ISearchEngine innerSearchEngine, IDataRepository dataRepository)
        {
            InnerSearchEngine = innerSearchEngine;
            _dataRepository = dataRepository;
        }

        public async Task<List<int>> SearchAsync(string keywords, string url, bool forceRefresh = false)
        {
            var result = forceRefresh ? null : GetCachedResult(keywords, url);
            if (result != null) return result;

            result = await InnerSearchEngine.SearchAsync(keywords, url, forceRefresh);
            SaveToCache(keywords, url, result);
            return result;
        }

        private List<int> GetCachedResult(string keywords, string url)
        {
            var cachedResult = _dataRepository.GetCachedResult(keywords, url);
            if (cachedResult != null)
            {
                Logger.Log(LogLevel.CacheHit, $"{keywords} {url}");
            }
            return cachedResult;
        }

        private void SaveToCache(string keywords, string url, List<int> result)
        {
            _dataRepository.SaveSearchResult(keywords, url, result);
        }

    }
}
