using SeoChecker.Interfaces;

namespace SeoChecker.Services
{
    public class SearchFacade
    {
        private readonly ISearchEngine _searchEngine;

        public SearchFacade(ISearchEngine searchEngine)
        {
            _searchEngine = searchEngine;
        }

        public async Task<List<int>> PerformSearchAsync(string keywords, string url, bool forceRefresh = false)
        {
            // Call the search engine's search method and return the results
            return await _searchEngine.SearchAsync(keywords, url, forceRefresh);
        }
    }
}
