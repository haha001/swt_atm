using System;
using NUnit.Framework;
using TransponderLib;

namespace TestUnit
{
    [TestFixture]
    class UT_Plane
    {
        private Plane _uut;
        [SetUp]
        public void SetUp()
        {
            _uut = new Plane();
        }

        [Test]
        public void Attribute_TagTest_ExpectedResult_True()
        {
            string tag = "wat";
            _uut.Tag = tag;
            Assert.That(_uut.Tag, Is.EqualTo(tag));
        }

        [Test]
        public void Attribute_XCoordTest_ExpectedResult_True()
        {
            int xcoord = 230;
            _uut.XCoord = xcoord;
            Assert.That(_uut.XCoord, Is.EqualTo(xcoord));
        }

        [Test]
        public void Attribute_YCoordTest_ExpectedResult_True()
        {
            int ycoord = 230;
            _uut.YCoord = ycoord;
            Assert.That(_uut.YCoord, Is.EqualTo(ycoord));
        }

        [Test]
        public void Attribute_Altitude_ExpectedResult_True()
        {
            int altitude = 3000;
            _uut.Altitude = altitude;
            Assert.That(_uut.Altitude, Is.EqualTo(altitude));
        }

        [Test]
        public void Attribute_AltitudeNegative_ExpectedResult_True()
        {
            int altitude = -300;
            _uut.Altitude = altitude;
            Assert.That(_uut.Altitude, Is.EqualTo(0));
        }

        [Test]
        public void Attribute_Speed_ExpectedResult_True()
        {
            double speed = 223.3;
            _uut.Speed = speed;
            Assert.That(_uut.Speed, Is.EqualTo(speed));
        }

        [Test]
        public void Attribute_Course_ExpectedResult_True()
        {
            double course = 32.3;
            _uut.Course = course;
            Assert.That(_uut.Course, Is.EqualTo(course));
        }

        [Test]
        public void Attribute_LastUpdated_ExpectedResult_True()
        {
            DateTime dt = DateTime.Parse("2018-03-22 22:03:46.321");
            _uut.LastUpdated = dt;
            Assert.That(_uut.LastUpdated, Is.EqualTo(dt));
        }

        [Test]
        public void Attribute_Separation_ExpectedResult_True()
        {
            bool sep = true;
            _uut.Separation = sep;
            Assert.That(_uut.Separation, Is.EqualTo(sep));
        }

        [Test]
        public void Attribute_SeparationTime_ExpectedResult_True()
        {
            DateTime dt = DateTime.Parse("2018-03-22 22:03:46.321");
            _uut.SeparationTime = dt;
            Assert.That(_uut.SeparationTime, Is.EqualTo(dt));
        }
    }
}
