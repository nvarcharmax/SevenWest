using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SevenWest.Core.Entities;
using SevenWest.Core.Services;

namespace SevenWest.Console
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var host = new HostBuilder()
                .Configure(args);
            
            var personQueryService = host.Services.GetService<IPersonQueryService>();
            var outputService = host.Services.GetService<IOutputService>();
            
            // The users full name for id = 42
            var results = await personQueryService.GetFullNamesById(42);
            outputService.Write(results);

            // All the users first names(comma separated) who are 23
            results = await personQueryService.GetCommaSeparatedFirstNamesByAge(23);
            outputService.Write(results);

            //The number of genders per Age, displayed from youngest to oldest
            results = await personQueryService.GetGenderDistributionByAge();
            outputService.Write(results);
        }

    }
}
