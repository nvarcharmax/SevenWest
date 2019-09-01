using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace SevenWest
{
    public static class HostBuilderExtensions
    {
        public static IHost Configure(this HostBuilder hostBuilder, string[] args)
        {
            return hostBuilder
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddLogging();
                    services.AddSingleton<IConfiguration>(
                        new ConfigurationBuilder()
                            .AddJsonFile("appsettings.json")
                            .Build()
                    );
                    services.AddScoped<IDataSource, JsonDataSource>();
                    services.AddScoped<IPersonQueryService, PersonQueryService>();
                    services.AddScoped<IOutputService, ConsoleOutputService>();
                })
                .Build();
        }
    }
}
