using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SevenWest.Core.Entities;

namespace SevenWest.Core.Services
{
    public class ApiPersonDataSource : IPersonDataSource
    {
        private ILogger<ApiPersonDataSource> _logger;
        private IMemoryCache _memoryCache;
        private IHttpClientFactory _clientFactory;
        private IConfiguration _configuration;
        private const string CacheKey = nameof(ApiPersonDataSource);

        public ApiPersonDataSource(ILogger<ApiPersonDataSource> logger, IConfiguration configuration, IMemoryCache memoryCache, IHttpClientFactory clientFactory)
        {
            _logger = logger;
            _configuration = configuration;
            _memoryCache = memoryCache;
            _clientFactory = clientFactory;
        }

        public async Task<List<Person>> Get()
        {
            // Check cache
            var results = await _memoryCache.GetOrCreateAsync<List<Person>>(CacheKey, async (entry) =>
            {
                _logger.LogInformation("Cache missed, fetching from remote source");
                entry.SlidingExpiration = TimeSpan.FromSeconds(60);
                return await Fetch<List<Person>>();
            });

            return results;
        }

        public async Task<T> Fetch<T>()
        {
            var url = _configuration["PersonApi"];

            _logger.LogInformation("Fetching data from {url}", url);

            var request = new HttpRequestMessage(HttpMethod.Get, url);
            var client = _clientFactory.CreateClient(); 
            
            try
            {
                var response = await client.SendAsync(request);

                _logger.LogInformation("Fetched data from {url} with status code {code}", url, response.StatusCode);

                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                var list = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(content);
                return list;
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error reading response from {url}");
                throw;
            }
        }

    }
}
