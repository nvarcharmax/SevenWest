using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;
using SevenWest;

namespace SevenWest
{
    public interface IDataSource
    {
        List<Person> Data { get; set; }
        void Initialise(string jsonFilePath);
    }

    public class JsonDataSource : IDataSource
    {
        private ILogger<JsonDataSource> _logger;

        public JsonDataSource(ILogger<JsonDataSource> logger)
        {
            _logger = logger;
        }

        public List<Person> Data { get; set; }
        public void Initialise(string jsonFilePath)
        {
            try
            {
                var json = System.IO.File.ReadAllText(jsonFilePath);
                Data = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Person>>(json);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error reading {jsonFilePath}");
                throw;
            }
        }
    }
}
