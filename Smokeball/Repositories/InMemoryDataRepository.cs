using SeoChecker.Interfaces;

namespace SeoChecker.Repositories
{
    public class InMemoryDataRepository : IDataRepository
    {
        private readonly Dictionary<(string, string), List<int>> _cache = new();

        public void SaveSearchResult(string keywords, string url, List<int> positions)
        {
            _cache[(keywords, url)] = positions;
        }

        public List<int> GetCachedResult(string keywords, string url)
        {
            return _cache.TryGetValue((keywords, url), out var positions) ? positions : null;
        }
    }
}
