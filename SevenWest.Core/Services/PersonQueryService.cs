using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SevenWest.Core.Entities;

namespace SevenWest.Core.Services
{
    public class PersonQueryService : IPersonQueryService
    {
        public ILogger<PersonQueryService> _logger;
        public IPersonDataSource _dataSource;

        public PersonQueryService(ILogger<PersonQueryService> logger, IPersonDataSource dataSource)
        {
            _logger = logger;
            _dataSource = dataSource;
        }

        public async Task<List<string>> GetFullNamesById(int id)
        {
            var data = await _dataSource.Get();
            return data.Where(x => x.Id == id).Select(x => x.FullName).ToList();
        }

        public async Task<List<string>> GetCommaSeparatedFirstNamesByAge(int age)
        {
            var data = await _dataSource.Get();
            return new List<string>()
            {
                string.Join(",",
                    data.Where(x => x.Age == age).Select(x => x.First)
                )
            };
        }
        public async Task<List<string>> GetGenderDistributionByAge()
        {
            var data = await _dataSource.Get();
            var groups = data.GroupBy(x => x.Age).OrderBy(x => x.Key);
            var distinctGenders = data.Select(x => x.Gender).Distinct().ToArray();

            var results = new List<string>();

            foreach (var g in groups)
            {
                // Aggregate by distinct genders
                var genderCounts = distinctGenders.Select(x => new
                {
                    Key = x,
                    Count = g.Count(y => y.Gender == x)
                });

                results.Add($"Age: {g.Key} {string.Join(" ", genderCounts.Select(x => $"{x.Key}: {x.Count}"))}");
            }

            return results;
        }
    }
}
