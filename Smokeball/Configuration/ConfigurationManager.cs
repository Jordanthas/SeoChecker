using Microsoft.Extensions.Configuration;

namespace SeoChecker.Configuration
{
    public static class ConfigurationManager
    {
        private static IConfiguration _configuration;

        public static IConfiguration GetConfiguration()
        {
            if (_configuration == null)
            {
                _configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .Build();
            }

            return _configuration;
        }

        public static string GetGoogleApiKey() => GetConfiguration()["SearchEngines:Google:ApiKey"];
        public static string GetGoogleSearchEngineId() => GetConfiguration()["SearchEngines:Google:SearchEngineId"];
        public static string GetBingApiKey() => GetConfiguration()["SearchEngines:Bing:ApiKey"];
        public static string GetBingEndpoint() => GetConfiguration()["SearchEngines:Bing:Endpoint"];
        public static bool UseAutoInterface() => GetConfiguration().GetValue<bool>("UseAutoInterface");
    }
}
