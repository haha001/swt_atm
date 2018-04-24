using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NUnit.Framework;
using NUnit.Framework.Internal;
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
        public void TransponderDataReady_Input_Null_ExpectedResult_2planes()
        {
            _receiver.TransponderDataReady += Raise.EventWith(new object(),
                new RawTransponderDataEventArgs(new List<string>() {null}));

            Assert.That(_atm._planes.Count, Is.EqualTo(2));
        }

        [Test]
        public void TransponderDataReady_Input_NullAndEmptyString_ExpectedResult_2planes()
        {
            _receiver.TransponderDataReady += Raise.EventWith(new object(),
                new RawTransponderDataEventArgs(new List<string>() {null, ""}));

            Assert.That(_atm._planes.Count, Is.EqualTo(2));
        }

        [Test]
        public void TransponderDataReady_Input_WrongFormatString_ExpectedResult_2planes()
        {
            _receiver.TransponderDataReady += Raise.EventWith(new object(),
                new RawTransponderDataEventArgs(new List<string>() {"uwotm8"}));

            Assert.That(_atm._planes.Count, Is.EqualTo(2));
        }

        [Test]
        public void TransponderDataReady_Input_StringWithSpaces_ExpectedResult_4planes()
        {
            _receiver.TransponderDataReady += Raise.EventWith(new object(),
                new RawTransponderDataEventArgs(new List<string>() { "A1; 39045; 12932; 14000; 20151006213456789" ,
                    "A2;29045;12932;15000;20151006213456789"}));

            _receiver.TransponderDataReady += Raise.EventWith(new object(),
                new RawTransponderDataEventArgs(new List<string>() { "A1; 40000; 13000; 14000; 20151006213459999" ,
                    "A2;30000;13000;15000;20151006213459999"}));

            Assert.That(_atm._planes.Count, Is.EqualTo(4));
        }

        [TestCase("A1; 10000; 90000; 14000; 20151006213456789", "A2;90000;10000;15000;20151006213456789", 4, TestName = "Input_RightAirspace_ExpectedResult_4")]
        [TestCase("A1; 1000; 90000; 14000; 20151006213456789", "A2;90000;10000;15000;20151006213456789", 3, TestName = "Input_WrongAirspace_ExpectedResult_3")]
        [TestCase("A1; 10000; 91000; 14000; 20151006213456789", "A2;90000;10000;15000;20151006213456789", 3, TestName = "Input_WrongAirspace_ExpectedResult_4")]
        public void TransponderDataReady_CheckAirSpace(string a1, string a2, int expectedResult)
        {
            _receiver.TransponderDataReady += Raise.EventWith(new object(),
                new RawTransponderDataEventArgs(new List<string>() {a1, a2}));
            
            Assert.That(_atm._planes.Count, Is.EqualTo(expectedResult));
        }

        [Test]
        public void TransponderDataReady_Input_WrongTimeDirection_ExpectedResult_3planes()
        {
            _receiver.TransponderDataReady += Raise.EventWith(new object(),
                new RawTransponderDataEventArgs(new List<string>() { "A1; 39045; 12932; 14000; 20151006213456789" ,
                    "A2;29045;12932;15000;20151006213456789"}));

            _receiver.TransponderDataReady += Raise.EventWith(new object(),
                new RawTransponderDataEventArgs(new List<string>() { "A1; 40000; 13000; 14000; 20151006213459999" ,
                    "A2;30000;13000;15000;20141006213459999"}));

            Assert.That(_atm._planes.Count, Is.EqualTo(3));
        }

        [Test]
        public void TransponderDataReady_Input_DirectlyDown_ExpectedResult_PlaneCrash_ItDidnt()
        {
            _receiver.TransponderDataReady += Raise.EventWith(new object(),
                new RawTransponderDataEventArgs(new List<string>() { "A1; 39045; 12932; 14000; 20151006213456789"}));

            _receiver.TransponderDataReady += Raise.EventWith(new object(),
                new RawTransponderDataEventArgs(new List<string>() { "A1; 39045; 12932; 10000; 20151006213456789" }));

            _receiver.TransponderDataReady += Raise.EventWith(new object(),
                new RawTransponderDataEventArgs(new List<string>() { "A1; 39045; 12932; 5000; 20151006213456789" }));

            _receiver.TransponderDataReady += Raise.EventWith(new object(),
                new RawTransponderDataEventArgs(new List<string>() { "A1; 39045; 12932; 0; 20151006213456789" }));

            Assert.That(_atm._planes.Count, Is.EqualTo(2));
        }

        [Test]
        public void TransponderDataReady_Input_0Height_ExpectedResult_3Planes()
        {
            _receiver.TransponderDataReady += Raise.EventWith(new object(),
                new RawTransponderDataEventArgs(new List<string>() { "A1; 39045; 12932; 0; 20151006213456789" }));
        }

        [Test]
        public void TransponderDataReady_UpdatePlane_Input_PlaneMovingOutAndIntoAirspace_ExpectedResult_3planes()
        {
            _receiver.TransponderDataReady += Raise.EventWith(new object(),
                new RawTransponderDataEventArgs(new List<string>() { "A1; 39045; 12932; 14000; 20151006213456789" }));

            _receiver.TransponderDataReady += Raise.EventWith(new object(),
                new RawTransponderDataEventArgs(new List<string>() { "A1; 139045; 112932; 14000; 20151006213456789" }));

            _receiver.TransponderDataReady += Raise.EventWith(new object(),
                new RawTransponderDataEventArgs(new List<string>() { "A1; 39045; 12932; 14000; 20151006213456789" }));

            Assert.That(_atm._planes.Count, Is.EqualTo(3));
        }

        [Test]
        public void TransponderDataReady_UpdatePlane_Input_OnlyDate_ExpectedResult_2PlanesCauseFaultyData()
        {
            _receiver.TransponderDataReady += Raise.EventWith(new object(),
                new RawTransponderDataEventArgs(new List<string>() { "A1; 39045; 12932; 14000; 20151006" }));

            Assert.That(_atm._planes.Count, Is.EqualTo(2));
        }

        [Test]
        public void TransponderDataReady_UpdatePlane_Input_CorrectDataAndOnlyDate_ExpectedResult_3planes()
        {
            _receiver.TransponderDataReady += Raise.EventWith(new object(),
                new RawTransponderDataEventArgs(new List<string>() { "A1; 39045; 12932; 14000; 20151006213456789" }));

            _receiver.TransponderDataReady += Raise.EventWith(new object(),
                new RawTransponderDataEventArgs(new List<string>() { "A1; 39045; 12932; 14000; 20151006" }));

            Assert.That(_atm._planes.Count, Is.EqualTo(3));
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
        public void HandleData_PlaneOutsideAirspace_ExpectedResult_False()
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
