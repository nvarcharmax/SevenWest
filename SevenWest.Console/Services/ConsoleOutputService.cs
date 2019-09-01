using System.Collections.Generic;
using SevenWest.Core.Services;

namespace SevenWest.Console.Services
{
    public class ConsoleOutputService : IOutputService
    {
        public void Write(List<string> items)
        {
            foreach (var i in items)
            {
                System.Console.WriteLine(i);
            }
        }
    }
}
