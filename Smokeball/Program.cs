using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SeoChecker.Factories;
using SeoChecker.Interfaces;
using SeoChecker.Models;
using SeoChecker.Repositories;
using SeoChecker.Services;
using SeoChecker.UI;

using CustomConfigurationManager = SeoChecker.Configuration.ConfigurationManager;

namespace SeoChecker
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // Initialize Dependency Injection
            var serviceProvider = ConfigureServices(CustomConfigurationManager.GetConfiguration());

            // Run the application
            await RunApplication(serviceProvider);
        }

        private static ServiceProvider ConfigureServices(IConfiguration configuration)
        {
            var services = new ServiceCollection();

            // Register configuration
            services.AddSingleton(configuration);

            // Initialize the SearchEngineFactory with configuration
            SearchEngineFactory.Initialize(configuration);

            // Register the data repository
            services.AddSingleton<IDataRepository, InMemoryDataRepository>();

            // Register the ISearchEngine with SearchEngineFactory
            services.AddSingleton<ISearchEngine>(provider =>
            {
                var dataRepository = provider.GetRequiredService<IDataRepository>();
                return SearchEngineFactory.Create(SearchEngineType.Google, dataRepository);
            });

            // Register SearchFacade, which depends on ISearchEngine
            services.AddSingleton<SearchFacade>();

            // Register the command line interfaces
            services.AddSingleton<AutomaticCommandLineInterface>();
            services.AddSingleton<ManualCommandLineInterface>();

            // Register ICommandLineInterface based on config setting
            services.AddSingleton<ICommandLineInterface>(provider =>
            {
                bool useAutoInterface = configuration.GetValue<bool>("UseAutoInterface");
                return useAutoInterface
                    ? provider.GetRequiredService<AutomaticCommandLineInterface>()
                    : provider.GetRequiredService<ManualCommandLineInterface>();
            });

            // Return the configured service provider
            return services.BuildServiceProvider();
        }

        private static async Task RunApplication(ServiceProvider serviceProvider)
        {
            // Get the selected Command Line Interface and run it
            var cli = serviceProvider.GetRequiredService<ICommandLineInterface>();
            await cli.StartAsync();
        }
    }
}
