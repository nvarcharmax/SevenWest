using System.Collections.Generic;

namespace SevenWest.Core.Services
{
    public interface IOutputService
    {
        void Write(List<string> items);
    }
}