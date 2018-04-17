using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using TransponderReceiver;
using TransponderLib;


namespace ATM.Test.Integration
{
	[TestFixture]
	public class IT1_ATMPlaneParser
	{
		private ITransponderReceiver _receiver;
		private TransponderLib.ATM _atm;
		private Plane _plane1;

		[SetUp]
		public void SetUp()
		{
			_receiver = TransponderReceiverFactory.CreateTransponderDataReceiver();
		}
		[TestCase]
		public void TryingToTest()
		{
			Assert.That(true);
		}
	}
}
