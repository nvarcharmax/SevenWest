using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using SevenWest.Core.Entities;

namespace SevenWest.Core.Services
{
    public class PersonQueryService : IPersonQueryService
    {
        public ILogger<PersonQueryService> _logger;
        public IDataSource<Person> _dataSource;

        public PersonQueryService(ILogger<PersonQueryService> logger, IDataSource<Person> dataSource)
        {
            _logger = logger;
            _dataSource = dataSource;
        }

        public List<string> GetFullNamesById(int id)
        {
            return _dataSource.Data.Where(x => x.Id == id).Select(x => x.FullName).ToList();
        }

        public List<string> GetCommaSeparatedFirstNamesByAge(int age)
        {
            return new List<string>()
            {
                string.Join(",",
                    _dataSource.Data.Where(x => x.Age == age).Select(x => x.First)
                )
            };
        }
        public List<string> GetGenderDistributionByAge()
        {
            var groups = _dataSource.Data.GroupBy(x => x.Age).OrderBy(x => x.Key);
            var distinctGenders = _dataSource.Data.Select(x => x.Gender).Distinct().ToArray();

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
