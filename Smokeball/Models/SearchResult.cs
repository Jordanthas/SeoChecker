namespace SeoChecker.Models
{
    public class SearchResult
    {
        public string Keywords { get; set; }
        public string Url { get; set; }
        public List<int> Positions { get; set; } = new List<int>();
        public DateTime Timestamp { get; set; }
        public bool IsCachedResult { get; set; }

        public SearchResult(string keywords, string url, List<int> positions, bool isCachedResult)
        {
            Keywords = keywords;
            Url = url;
            Positions = positions;
            Timestamp = DateTime.Now;
            IsCachedResult = isCachedResult;
        }

        public override string ToString()
        {
            string positionsStr = Positions.Count > 0 ? string.Join(", ", Positions) : "0 (not found)";
            string cacheStatus = IsCachedResult ? "Cached result" : "Live result";
            return $"Keywords: '{Keywords}', URL: '{Url}', Positions: [{positionsStr}], {cacheStatus}, Searched at: {Timestamp}";
        }
    }
}
