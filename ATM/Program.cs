using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using TransponderLib;
using TransponderReceiver;

namespace ATM_ns
{
    class Program
    {
        static void Main(string[] args)
        {
            var atm = new ATM();
            while (Console.Read() != 'q') ;
        }
    }
}
