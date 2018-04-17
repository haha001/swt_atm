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
	}
}
