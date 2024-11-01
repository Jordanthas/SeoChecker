using SeoChecker.Interfaces;
using SeoChecker.Services;
using SeoChecker.Utilities;
using System;
using System.Threading.Tasks;

namespace SeoChecker.UI
{
    public class AutomaticCommandLineInterface : ICommandLineInterface
    {
        private readonly SearchFacade _searchFacade;

        // Hardcoded values
        private const string DefaultKeywords = "conveyancing software";
        private const string DefaultUrl = "www.smokeball.com.au";

        public AutomaticCommandLineInterface(SearchFacade searchFacade)
        {
            _searchFacade = searchFacade;
        }

        public async Task StartAsync()
        {
            Console.WriteLine("=== SEO Checker ===");
            Console.WriteLine($"Keywords: {DefaultKeywords}");
            Console.WriteLine($"URL to Search: {DefaultUrl}");

            // Prompt for cache refresh
            bool forceRefresh = PromptForCacheRefresh();

            Console.WriteLine("\nStarting search...");

            // Perform the search
            var positions = await _searchFacade.PerformSearchAsync(DefaultKeywords, DefaultUrl, forceRefresh);

            // Display results
            if (positions.Count > 0 && positions[0] != 0)
            {
                Console.WriteLine("\nResults:");
                Console.WriteLine($"The URL '{DefaultUrl}' was found at positions: {string.Join(", ", positions)}\n");
            }
            else
            {
                Console.WriteLine("\nNo results found for the given URL within the top search results.\n");
            }

            Console.WriteLine("=== End of Search ===\nPress any key to exit.");
            Console.ReadKey();
        }

        private bool PromptForCacheRefresh()
        {
            while (true)
            {
                Console.WriteLine("Would you like to force a cache refresh? (Y/N)");

                if (!char.TryParse(Console.ReadLine()?.Trim().ToUpper(), out char input))
                {
                    Logger.Log(LogLevel.Error, "Invalid input. Please enter 'Y' for Yes or 'N' for No.");
                    throw new Exception();
                }

                if (input == 'Y')
                    return true;
                else if (input == 'N')
                    return false;
                else 
                    Logger.Log(LogLevel.Error, "Invalid input. Please enter 'Y' for Yes or 'N' for No.");
            }
        }
    }
}
