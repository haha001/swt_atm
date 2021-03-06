﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using TransponderReceiver;
using TransponderLib;
using NSubstitute;
using NSubstitute.Extensions;
using ATM = TransponderLib.ATM;

namespace Test.Integration
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
		private RawTransponderDataEventArgs _eventArgs;

		[SetUp]
		public void SetUp()
		{
			_receiver = Substitute.For<ITransponderReceiver>();
			_eventArgs = new RawTransponderDataEventArgs(new List<string>(){ "A3;39045;12932;14000;20151006213456789" });
			_parser = new TransponderDataParser();
			_output = Substitute.For<IOutput>();
			_plane1 = new Plane(){Tag = "A3", XCoord = 39045, YCoord = 12932, Altitude = 14000, LastUpdated = _parser.ParseTime("20151006213456789") };
			_detector = Substitute.For<ICollisionDetector>();
			_atm = new TransponderLib.ATM(_receiver, _parser, _output, _detector);
		}

		//Integration with DataParser
		[TestCase]
		public void HandleData_PlaneIsAdded_DataParsedCorrectly()
		{
			_receiver.TransponderDataReady += Raise.EventWith(new object(), _eventArgs); //Insert values equal to _plane1 into 

			//Check that plane exists in list now
			Assert.That(_atm._planes.Exists(plane => plane.Tag == "A3" && plane.XCoord == 39045 && plane.YCoord == 12932
													 && plane.Altitude == 14000 && plane.LastUpdated == _parser.ParseTime("20151006213456789")));

		}

		[TestCase]
		public void HandleData_IncorrectStringToDataParser_PlaneNotAddedToATM()
		{
			//Raise event with wrong data
			_receiver.TransponderDataReady +=
				Raise.EventWith(new object(), new RawTransponderDataEventArgs(new List<string>() {"23;23;23;23"}));

			//Make sure plane 
			Assert.That(_atm._planes.Count == 0);

		}

		[TestCase]
		public void HandleData_TwoSameStringsGiven_PlaneAddedOnceToATMWithoutModification()
		{
			_receiver.TransponderDataReady += Raise.EventWith(new object(), _eventArgs); //Insert values equal to _plane1 into 
			_receiver.TransponderDataReady += Raise.EventWith(new object(), _eventArgs); //Insert values equal to _plane1 into 

			//Make sure plane is only added once
			Assert.That(_atm._planes.Count == 1 && (_atm._planes.Exists(plane => plane.Tag == "A3" && plane.XCoord == 39045 && plane.YCoord == 12932
			                                                                     && plane.Altitude == 14000 && plane.LastUpdated == _parser.ParseTime("20151006213456789"))));
		}

		[TestCase]
		public void UpdatePlane_SamePlaneUpdated_PlaneInATMUpdated()
		{
			_receiver.TransponderDataReady += Raise.EventWith(new object(), _eventArgs); //Insert values equal to _plane1 into 
			_receiver.TransponderDataReady += Raise.EventWith(new object(), new RawTransponderDataEventArgs(new List<string>()
			{
				"A3;10001;10023;5000;20151006213457789"
			}));  

			Console.Write(_atm._planes.Count);

			//Make sure plane is only added once
			Assert.That(_atm._planes.Count == 1 && (_atm._planes.Exists(plane => plane.Tag == "A3" && plane.XCoord == 10001 && plane.YCoord == 10023
																				 && plane.Altitude == 5000 && plane.LastUpdated == _parser.ParseTime("20151006213457789"))));
		}
	}


}
