using SeoChecker.Interfaces;

namespace SeoChecker.Services.SearchEngines
{
    public abstract class BaseSearchEngine : ISearchEngine
    {
        protected const int MaxResults = 100;
        protected const int PageSize = 10;

        public abstract Task<List<int>> SearchAsync(string keywords, string url, bool forceRefresh = false);

        protected void AddMatchingPositions<T>(List<int> positions, List<T> items, string url, int startIndex, Func<T, string> getUrlFunc)
        {
            for (int i = 0; i < items.Count; i++)
            {
                string resultUrl = getUrlFunc(items[i]);
                if (resultUrl != null && resultUrl.Contains(url, StringComparison.OrdinalIgnoreCase))
                {
                    positions.Add(startIndex + i);
                }
            }
        }
    }
}
