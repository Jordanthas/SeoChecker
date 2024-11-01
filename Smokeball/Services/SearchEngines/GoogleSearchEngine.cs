using SeoChecker.Helpers;
using SeoChecker.Models;

namespace SeoChecker.Services.SearchEngines
{
    public class GoogleSearchEngine : BaseSearchEngine
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly string _searchEngineId;

        public GoogleSearchEngine(HttpClient httpClient, string apiKey, string searchEngineId)
        {
            _httpClient = httpClient;
            _apiKey = apiKey;
            _searchEngineId = searchEngineId;
        }

        public override async Task<List<int>> SearchAsync(string keywords, string url, bool forceRefresh = false)
        {
            List<int> positions = new List<int>();
            int startIndex = 1;

            while (positions.Count < MaxResults && startIndex <= MaxResults)
            {
                var results = await FetchSearchResults(keywords, startIndex);
                if (results?.Items == null || results.Items.Count == 0) break;

                AddMatchingPositions(positions, results.Items, url, startIndex, item => item.Link);

                if (results.Items.Count < PageSize) break;
                startIndex += PageSize;
            }

            return positions.Count > 0 ? positions : new List<int> { 0 };
        }

        private async Task<GoogleSearchResult> FetchSearchResults(string keywords, int startIndex)
        {
            var requestUri = $"https://www.googleapis.com/customsearch/v1?key={_apiKey}&cx={_searchEngineId}&q={System.Uri.EscapeDataString(keywords)}&start={startIndex}&cr=countryAU";
            return await HttpHelper.SendRequestAsync<GoogleSearchResult>(_httpClient, requestUri);
        }
    }
}
