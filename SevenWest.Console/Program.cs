using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SevenWest.Core.Entities;
using SevenWest.Core.Services;

namespace SevenWest.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var host = new HostBuilder()
                .Configure(args);
            
            var dataSource = host.Services.GetService<IDataSource<Person>>();
            var personQueryService = host.Services.GetService<IPersonQueryService>();
            var outputService = host.Services.GetService<IOutputService>();

            // Return if no file paths in arguments
            if (!args.Any())
            {
                return;
            }
            
            // Load Data source from file
            dataSource.Initialise(args.FirstOrDefault());

            // The users full name for id = 42
            var results = personQueryService.GetFullNamesById(42);
            outputService.Write(results);

            // All the users first names(comma separated) who are 23
            results = personQueryService.GetCommaSeparatedFirstNamesByAge(23);
            outputService.Write(results);

            //The number of genders per Age, displayed from youngest to oldest
            results = personQueryService.GetGenderDistributionByAge();
            outputService.Write(results);

        }

    }
}
