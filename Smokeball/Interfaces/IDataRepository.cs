namespace SeoChecker.Interfaces
{
    public interface IDataRepository
    {
        void SaveSearchResult(string keywords, string url, List<int> positions);
        List<int> GetCachedResult(string keywords, string url);
    }
}
