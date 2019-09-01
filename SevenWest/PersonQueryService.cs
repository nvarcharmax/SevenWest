using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Logging;

namespace SevenWest
{
    public interface IPersonQueryService
    {
        List<string> GetFullNamesById(int id);
        List<string> GetCommaSeparatedFirstNamesByAge(int age);
        List<string> GetGenderDistributionByAge();
    }

    public class PersonQueryService : IPersonQueryService
    {
        public ILogger<PersonQueryService> _logger;
        public IDataSource _dataSource;

        public PersonQueryService(ILogger<PersonQueryService> logger, IDataSource dataSource)
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
            var distinctGenders = _dataSource.Data.Select(x => x.Gender).Distinct();

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
