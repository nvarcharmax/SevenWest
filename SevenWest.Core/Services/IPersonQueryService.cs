using System.Collections.Generic;

namespace SevenWest.Core.Services
{
    public interface IPersonQueryService
    {
        List<string> GetFullNamesById(int id);
        List<string> GetCommaSeparatedFirstNamesByAge(int age);
        List<string> GetGenderDistributionByAge();
    }
}