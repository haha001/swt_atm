using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
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
                    "A4;29045;12932;15000;20151006213456789"}));
        }

        [Test]
        public void wat()
        {
            Assert.Throws<NullReferenceException>(() => _receiver.TransponderDataReady += Raise.EventWith(new object(),
                new RawTransponderDataEventArgs(new List<string>() {null})));
        }

        [Test]
        public void wat2()
        {
            _receiver.TransponderDataReady += Raise.EventWith(new object(),
                new RawTransponderDataEventArgs(new List<string>() {null, ""}));

            Assert.That(_atm._planes.Count, Is.EqualTo(2));
        }

        [Test]
        public void wat3()
        {
            Assert.Throws<NullReferenceException>(() => _receiver.TransponderDataReady += Raise.EventWith(new object(),
                new RawTransponderDataEventArgs(new List<string>() { "uwotm8" })));
        }

        [Test]
        public void DetectorOnNoSeperationEvent_ExpectedResult_True()
        {
            _detector.NoSeperationEvent += Raise.EventWith(_detector, new CollisionEventArgs(_atm._planes[0], _atm._planes[1]));
            Assert.That(!_atm._planes[0].Separation && !_atm._planes[1].Separation);
        }

        [Test]
        public void DetectorOnSeparationEvent_ExpectedResult_True()
        {
            _detector.SeparationEvent += Raise.EventWith(_detector, new CollisionEventArgs(_atm._planes[0], _atm._planes[1]));
            Assert.That(_atm._planes[0].Separation && _atm._planes[1].Separation);
        }

        [Test]
        public void HandleData_PlaneOutsideAirspace_ExpectedResult_True()
        {
            _receiver.TransponderDataReady += Raise.EventWith(new object(), 
            new RawTransponderDataEventArgs(new List<string>() { "A3;100000;12932;14000;20151006213456900" ,
                "A4;29045;12932;15000;20151006213456900"}));
            Assert.That(_atm._planes.Exists(p => p.Tag == "A3"), Is.False);
        }

        [Test]
        public void HandleData_PlaneInsideAirspace_ExpectedResult_True()
        {
            _receiver.TransponderDataReady += Raise.EventWith(new object(),
            new RawTransponderDataEventArgs(new List<string>() { "A3;30000;12932;14000;20151006213456900" ,
                "A4;29045;12932;15000;20151006213456900"}));
            Assert.That(_atm._planes.Exists(p => p.Tag == "A3"), Is.True);
        }

        [Test]
        public void HandleData_DataParser_ThrowExection_True()
        {
            string wrongFormatInput = "A4;29045;129321500020151006213456900";
            Assert.DoesNotThrow(() => _atm.HandleData(wrongFormatInput));
        }
    }
}
