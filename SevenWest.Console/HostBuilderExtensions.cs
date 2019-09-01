using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Extensions.Logging.File;
using SevenWest.Console.Services;
using SevenWest.Core.Services;

namespace SevenWest.Console
{
    public static class HostBuilderExtensions
    {
        public static IHost Configure(this HostBuilder hostBuilder, string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.RollingFile(configuration["Logging:PathFormat"])
                .CreateLogger();

            return hostBuilder
                .ConfigureLogging((hostingContext, builder) =>
                {
                    builder.AddSerilog(Log.Logger);
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddSingleton<IConfiguration>(configuration);
                    services.AddScoped(typeof(IDataSource<>), typeof(JsonDataSource<>));
                    services.AddScoped<IPersonQueryService, PersonQueryService>();
                    services.AddScoped<IOutputService, ConsoleOutputService>();
                })
                .Build();
        }
    }
}
