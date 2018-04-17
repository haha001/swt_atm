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

        private ITransponderReceiver _receiver;
        private ITransponderDataParser _dataParser;
        private IOutput _output;
        private ICollisionDetector _detector;
        //private CollisionEventArgs _collisionEventArgs;
        //private RawTransponderDataEventArgs _eventArgs;

        [SetUp]
        public void SetUp()
        {
            _receiver = Substitute.For<ITransponderReceiver>();
            _dataParser = new TransponderDataParser();
            _output = Substitute.For<IOutput>();
            _detector = Substitute.For<ICollisionDetector>();

            _atm = new ATM(_receiver, _dataParser, _output, _detector);

            _receiver.TransponderDataReady += Raise.EventWith(new object(), 
                new RawTransponderDataEventArgs(new List<string>() { "A3;39045;12932;14000;20151006213456789" ,
                    "A4;29045;12932;15000;20151006213456789", "A5;29050;12940;15000;20151006213456789" }));
        }

        [Test]
        public void DetectorOnNoSeperationEvent_ExpectedResult_False()
        {
            _detector.NoSeperationEvent += Raise.EventWith(_detector, new CollisionEventArgs(_atm._planes[0], _atm._planes[1]));
            Assert.That(!_atm._planes[0].Separation && !_atm._planes[1].Separation);
        }

        //[Test]
        //public void DetectorOnNoSeperationEvent_ExpectedResult_True()
        //{
        //    _detector.NoSeperationEvent += Raise.EventWith(_detector, new CollisionEventArgs(_atm._planes[1], _atm._planes[2]));
        //    Assert.That(_atm._planes[1].Separation && _atm._planes[2].Separation);
        //}

        //public void DetectorOnSeparationEvent_ExpectedResult_True()
        //{

        //}
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
