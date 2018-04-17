using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransponderReceiver;
using TransponderLib;
using ATM = TransponderLib.ATM;

namespace ATM_Application
{
	class Program
	{
        
		static void Main(string[] args)
		{

			ATM atm = new ATM(TransponderReceiverFactory.CreateTransponderDataReceiver(),new TransponderDataParser(), new ConsoleOutputter(), new CollisionDetector());
			Console.ReadLine();
        }
        
        /*
        /// Test of ConsoleOutputter Main
	    static void Main(string[] args)
	    {

	        var uut = new ConsoleOutputter();

            uut.Print("this is cleared printed to console");

            uut.Reset();

	        uut.Print("this is printed after Reset()");

	        Console.ReadLine();
	    }
        */
}
}
