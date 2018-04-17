using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using TransponderLib;
using TransponderReceiver;

namespace ATM_program
{
    class Program
    {
        static void Main(string[] args)
        {
            var atm = new ATM(TransponderReceiverFactory.CreateTransponderDataReceiver(), new TransponderDataParser(), new ConsoleOutputter(), new CollisionDetector());
            while (Console.Read() != 'q') ;
        }
    }
}
