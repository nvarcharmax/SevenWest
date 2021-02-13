using System.Collections.Generic;
using System.Threading.Tasks;

namespace SevenWest.Core.Services
{
    public interface IPersonQueryService
    {
        Task<List<string>> GetFullNamesById(int id);
        Task<List<string>> GetCommaSeparatedFirstNamesByAge(int age);
        Task<List<string>> GetGenderDistributionByAge();
    }
}