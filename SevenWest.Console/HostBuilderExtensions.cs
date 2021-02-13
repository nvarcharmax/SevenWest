using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using System.Net.Http;
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
                    services.AddScoped(typeof(IPersonDataSource), typeof(ApiPersonDataSource));
                    services.AddScoped<IPersonQueryService, PersonQueryService>();
                    services.AddScoped<IOutputService, ConsoleOutputService>();
                    services.AddMemoryCache();
                    services.AddHttpClient();
                })
                .Build();
        }
    }
}
