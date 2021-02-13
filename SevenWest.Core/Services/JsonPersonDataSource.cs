using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SevenWest.Core.Entities;

namespace SevenWest.Core.Services
{
    public class JsonPersonDataSource: IPersonDataSource
    {
        private ILogger<JsonPersonDataSource> _logger;
        private IConfiguration _configuration;

        public JsonPersonDataSource(ILogger<JsonPersonDataSource> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<List<Person>> Get()
        {
            var path = _configuration["PersonFileLocation"];

            if (string.IsNullOrEmpty(path))
            {
                throw new InvalidOperationException($"Error reading {path}");
            }

            try
            {
                var json = System.IO.File.ReadAllText(path);
                return Newtonsoft.Json.JsonConvert.DeserializeObject<List<Person>>(json);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error reading {path}");
                throw;
            }
        }
    }
}
