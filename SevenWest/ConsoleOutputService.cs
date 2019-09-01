using System;
using System.Collections.Generic;
using System.Text;

namespace SevenWest
{
    public interface IOutputService
    {
        void Write(List<string> items);
    }

    public class ConsoleOutputService : IOutputService
    {
        public void Write(List<string> items)
        {
            foreach (var i in items)
            {
                Console.WriteLine(i);
            }
        }
    }
}
