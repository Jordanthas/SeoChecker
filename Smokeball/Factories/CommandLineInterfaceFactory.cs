using Microsoft.Extensions.DependencyInjection;
using SeoChecker.Interfaces;
using SeoChecker.UI;

namespace SeoChecker.Factories
{
    public static class CommandLineInterfaceFactory
    {
        public static ICommandLineInterface CreateInterface(bool useAutoInterface, IServiceProvider serviceProvider)
        {
            return useAutoInterface
                ? serviceProvider.GetRequiredService<AutomaticCommandLineInterface>()
                : serviceProvider.GetRequiredService<ManualCommandLineInterface>();
        }
    }
}
