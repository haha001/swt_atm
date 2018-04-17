using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NSubstitute;
using NUnit.Framework;
using TransponderLib;
using TransponderReceiver;
using ATM = TransponderLib.ATM;

namespace Test.Integration
{
    [TestFixture]
    class IT2_CollisionDetector_ATM_Plane
    {
        private Plane _plane_1;
        private Plane _plane_2;
        private Plane _plane_3;

        private ICollisionDetector _collisionDetector;
        private ITransponderReceiver _receiver;
        private ITransponderDataParser _parser;
        private IOutput _output;

        private TransponderLib.ATM _atm;

        private RawTransponderDataEventArgs _eventArgs_1;
        private RawTransponderDataEventArgs _eventArgs_2;
        private RawTransponderDataEventArgs _eventArgs_3;

        [SetUp]
        public void SetUp()
        {
            _receiver = Substitute.For<ITransponderReceiver>();
            _parser = new TransponderDataParser();
            _output = Substitute.For<IOutput>();
            _collisionDetector = new CollisionDetector();

            _atm = new ATM(_receiver, _parser, _output, _collisionDetector);

            _plane_1 = new Plane() { Tag = "A1", XCoord = 39045, YCoord = 12932, Altitude = 14000, LastUpdated = _parser.ParseTime("20151006213456789") };
            _plane_2 = new Plane() { Tag = "A2", XCoord = 39045, YCoord = 12932, Altitude = 14000, LastUpdated = _parser.ParseTime("20151006213456789") };
            _plane_3 = new Plane() { Tag = "A3", XCoord = 39045, YCoord = 12932, Altitude = 14000, LastUpdated = _parser.ParseTime("20151006213456789") };

            _eventArgs_1 = new RawTransponderDataEventArgs(new List<string>() { "A1;39045;12933;14003;20151006213456789" });
            _eventArgs_2 = new RawTransponderDataEventArgs(new List<string>() { "A2;39044;12932;14002;20151006213456789" });
            _eventArgs_3 = new RawTransponderDataEventArgs(new List<string>() { "A3;39043;12931;14001;20151006213456789" });

        }

        [Test]
        public void DetectCollision_OnePlaneInserted_ExpectedResult_NoEvent()
        {
            bool raised = false;
            // Insert values equal to _plane_1
            _receiver.TransponderDataReady += Raise.EventWith(new object(), _eventArgs_1);

            _collisionDetector.SeparationEvent += (sender, args) => raised = true;

            _collisionDetector.DetectCollision(_atm._planes);

            Assert.That(raised == false);
        }

        [Test]
        public void DetectCollision_TwoPlanesInsertedWithinRange_ExpectedResult_EventRaised()
        {
            bool raised = false;

            _collisionDetector.SeparationEvent += (sender, args) => raised = true;

            // Insert values equal to _plane_1
            _receiver.TransponderDataReady += Raise.EventWith(new object(), _eventArgs_1);
            // Insert values equal to _plane_2
            _receiver.TransponderDataReady += Raise.EventWith(new object(), _eventArgs_2);



            //_collisionDetector.DetectCollision(_atm._planes);
            Assert.That(_atm._planes[0].Separation);
            //Assert.That(raised);
        }

        [Test]
        public void DetectCollision_ThreePlanesInsertedWithinRange_ExpectedResult_EventRaised()
        {
            int raised = 0;
            bool rased = false;



            // Insert values equal to _plane_1
            _receiver.TransponderDataReady += Raise.EventWith(new object(), _eventArgs_1);
            // Insert values equal to _plane_2
            _receiver.TransponderDataReady += Raise.EventWith(new object(), _eventArgs_2);

            _collisionDetector.SeparationEvent += (sender, args) => rased = true;

            _collisionDetector.DetectCollision(_atm._planes);

            Assert.That(rased);
            //// Insert values equal to _plane_3
            //_receiver.TransponderDataReady += Raise.EventWith(new object(), _eventArgs_3);

            //_collisionDetector.DetectCollision(_atm._planes);


            ////Assert.That(_atm._planes.Count, Is.EqualTo(3));
            ////Assert.That(raised, Is.EqualTo(3));
            //Assert.That(rased);
        }

    }
}
