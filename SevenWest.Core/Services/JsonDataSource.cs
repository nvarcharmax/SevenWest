using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace SevenWest.Core.Services
{
    public class JsonDataSource<T> : IDataSource<T>
    {
        private ILogger<JsonDataSource<T>> _logger;

        public JsonDataSource(ILogger<JsonDataSource<T>> logger)
        {
            _logger = logger;
        }

        public List<T> Data { get; set; }

        public void Initialise(string path)
        {
            try
            {
                var json = System.IO.File.ReadAllText(path);
                Data = Newtonsoft.Json.JsonConvert.DeserializeObject<List<T>>(json).ToList();
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error reading {path}");
                throw;
            }
        }
    }
}
