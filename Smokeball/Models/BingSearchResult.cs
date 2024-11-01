using System.Text.Json.Serialization;


namespace SeoChecker.Models
{
    public class BingSearchResult
    {
        [JsonPropertyName("webPages")]
        public WebPages WebPages { get; set; }
    }

    public class WebPages
    {
        [JsonPropertyName("value")]
        public List<BingSearchResultItem> Items { get; set; }
    }

    public class BingSearchResultItem
    {
        [JsonPropertyName("url")]
        public string Url { get; set; }
    }
}
