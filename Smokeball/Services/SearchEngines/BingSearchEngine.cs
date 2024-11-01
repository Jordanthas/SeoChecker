using SeoChecker.Helpers;
using SeoChecker.Models;

namespace SeoChecker.Services.SearchEngines
{
    public class BingSearchEngine : BaseSearchEngine
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly string _endpoint;

        public BingSearchEngine(HttpClient httpClient, string apiKey, string endpoint)
        {
            _httpClient = httpClient;
            _apiKey = apiKey;
            _endpoint = endpoint;
        }

        public override async Task<List<int>> SearchAsync(string keywords, string url, bool forceRefresh = false)
        {
            List<int> positions = new List<int>();
            int startIndex = 1;

            while (positions.Count < MaxResults && startIndex < MaxResults)
            {
                var results = await FetchSearchResults(keywords, startIndex);
                if (results?.WebPages?.Items == null || results.WebPages.Items.Count == 0) break;

                AddMatchingPositions(positions, results.WebPages.Items, url, startIndex, item => item.Url);

                if (results.WebPages.Items.Count < PageSize) break;
                startIndex += PageSize;
            }

            return positions.Count > 0 ? positions : new List<int> { 0 };
        }

        private async Task<BingSearchResult> FetchSearchResults(string keywords, int offset)
        {
            var requestUri = $"{_endpoint}?q={System.Uri.EscapeDataString(keywords)}&offset={offset}&count={PageSize}";
            return await HttpHelper.SendRequestAsync<BingSearchResult>(_httpClient, requestUri);
        }
    }
}
