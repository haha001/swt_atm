using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using TransponderReceiver;
using TransponderLib;
using NSubstitute;
using ATM = TransponderLib.ATM;

namespace ATM.Test.Integration
{
	[TestFixture]
	public class IT1_ATMPlaneParser
	{
		private ITransponderReceiver _receiver;
		private ITransponderDataParser _parser;
		private ICollisionDetector _detector;
		private TransponderLib.ATM _atm;
		private IOutput _output;
		private Plane _plane1;

		[SetUp]
		public void SetUp()
		{
			_receiver = TransponderReceiverFactory.CreateTransponderDataReceiver();
			_parser = new TransponderDataParser();
			_output = Substitute.For<IOutput>();
			_plane1 = new Plane();
			_detector = Substitute.For<ICollisionDetector>();
			_atm = new TransponderLib.ATM(_receiver, _parser, _output, _detector);
		}

		//
		[TestCase]
		public void TryingToTest()
		{
			Assert.That(true);
		}
	}
}
