using System.Collections.Generic;
using System.Linq;
using SevenWest.Core.Entities;

namespace SevenWest.Core.Services
{
    public interface IDataSource<T>
    {
        List<T> Data { get; set; }
        void Initialise(string path);
    }
}