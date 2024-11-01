using System.Text.Json;

namespace SeoChecker.Helpers
{
    public static class HttpHelper
    {
        public static async Task<T> SendRequestAsync<T>(HttpClient httpClient, string requestUri)
        {
            var response = await httpClient.GetAsync(requestUri);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Error: {response.ReasonPhrase}");
                return default;
            }

            var jsonResponse = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(jsonResponse);
        }
    }
}
