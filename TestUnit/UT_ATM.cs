using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NSubstitute;
using NUnit.Framework;
using TransponderLib;
using TransponderReceiver;


namespace TestUnit
{
    [TestFixture]
    class UT_ATM
    {
        private ATM _atm;
        private Plane _plane1;
        private Plane _plane2;

        private ITransponderReceiver _receiver;
        private ITransponderDataParser _dataParser;
        private IOutput _output;
        private ICollisionDetector _detector;
        private CollisionEventArgs _collisionEventArgs;

        [SetUp]
        public void SetUp()
        {
            _receiver = Substitute.For<ITransponderReceiver>();
            _dataParser = new TransponderDataParser();
            _output = Substitute.For<IOutput>();
            _detector = Substitute.For<ICollisionDetector>();

            _plane1 = new Plane() { Tag = "A3", XCoord = 39045, YCoord = 12932, Altitude = 14000, LastUpdated = _dataParser.ParseTime("20151006213456789") };
            _plane2 = new Plane() { Tag = "A4", XCoord = 29045, YCoord = 13932, Altitude = 15000, LastUpdated = _dataParser.ParseTime("20151006213456789") };

            _atm = new ATM(_receiver, _dataParser, _output, _detector);
        }

        [Test]
        public void DetectorOnNoSeperationEvent_ExpectedResult_True()
        {

            
        }

        // 

        [Test]
		[Ignore ("Not done")]
        public void wat()
        {
            _plane1.Course = 33.3;
            _plane1.Speed = 200.42;

            var date = DateTime.Parse("2018-11-22 01:13:37.323");

            _atm.UpdatePlane(_plane1, 23800, 21000, 11500, date);

            Assert.That(_plane1.XCoord, Is.EqualTo(23800));
        }
    }
}
