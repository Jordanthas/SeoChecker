using System.Text.Json.Serialization;

namespace SeoChecker.Models
{
    public class GoogleSearchResult
    {
        [JsonPropertyName("items")]
        public List<GoogleSearchResultItem> Items { get; set; }
    }

    public class GoogleSearchResultItem
    {
        [JsonPropertyName("link")]
        public string Link { get; set; }
    }
}
