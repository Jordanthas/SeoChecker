namespace SeoChecker.Interfaces
{
    public interface ISearchEngine
    {
        Task<List<int>> SearchAsync(string keywords, string url, bool forceRefresh = false);
    }
}
