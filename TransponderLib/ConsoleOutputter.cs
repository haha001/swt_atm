using System;
using System.Collections.Generic;
using System.Text;

namespace TransponderLib
{
    public interface IOutput
    {
        void Print(string str);
        void Reset();
    }

    public class ConsoleOutputter : IOutput
    {
        public void Print(string str)
        {
            Console.WriteLine(str);
        }

        public void Reset()
        {
            Console.Clear();
        }
    }
}
