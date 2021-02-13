using SevenWest.Core.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SevenWest.Core.Services
{
    public interface IPersonDataSource
    {
        Task<List<Person>> Get();
    }
}