using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using TransponderLib;

namespace TestUnit
{
    [TestFixture]
    public class UT_TransponderDataParser
    {

        private ITransponderDataParser _uut;

        [SetUp]
        public void SetUp()
        {
            _uut = new TransponderDataParser();
        }

        [Test]
        public void ParseTime_AllowedNumbers_ExpectedResult_True()
        {
            var date = new DateTime(1010,10,10,10,10,10,111);
            var time = "10101010101010111";
            Assert.That(_uut.ParseTime(time),Is.EqualTo(date));
        }

        [TestCase("20180101246060", TestName = "Min/Sec too high")]
        [TestCase("20180230444444444", TestName = "30th february")]
        [TestCase("99999999999999999", TestName = "AllNines")]
        [TestCase("00000000000000000", TestName = "AllZeroes")]
        public void ParseTime_NoneligibleInput_ExpectedResult_Exception(string time)
        {
            Assert.That(() => _uut.ParseTime(time),Throws.Exception.TypeOf<FormatException>());
        }

        [TestCase("abc11111111111111", TestName = "Letters as input")]
        public void ParseTime_NonNumberInput_ExpectedResult_Exception(string time)
        {
            Assert.That(() => _uut.ParseTime(time), Throws.Exception.TypeOf<FormatException>());
        }

        [Test]
        public void ParseData_InputStringNull_ExpectedResult_ExceptionThrown()
        {
            string data = null;

            Assert.That(() => _uut.ParseData(data,
                out string tag,
                out int xCoord,
                out int yCoord,
                out int altitude,
                out DateTime time), Throws.Exception.TypeOf<NullReferenceException>());
        }

        [Test]
        public void ParseData_MissingData_ExpectedResult_Exception()
        {
            string data = "A3";
            Assert.That(() => _uut.ParseData(data,
                out string tag,
                out int xCoord,
                out int yCoord,
                out int altitude,
                out DateTime time), Throws.Exception.TypeOf<ArgumentException>());
        }

        [TestCase("A3;39045;12932;14000;20151006213456789","A3",TestName = "Shorter Name")]
        [TestCase("ATR423;39045;12932;14000;20151006213456789", "ATR423", TestName = "Legal input")]
        public void ParseData_TagTest_ExpectedResult_True(string data, string expectedTag)
        {
            _uut.ParseData(data, 
                out string tag, 
                out int xCoord, 
                out int yCoord, 
                out int altitude, 
                out DateTime time);
            
            Assert.That(tag, Is.EqualTo(expectedTag));
        }

        [TestCase("ATR423;-23;12932;14000;20151006213456789", -23, TestName = "xCoord_-23")]
        [TestCase("ATR423;0;12932;14000;20151006213456789",0, TestName = "xCoord_0")]
        [TestCase("ATR423;39045;12932;14000;20151006213456789",39045, TestName = "xCoord_39045")]
        public void ParseData_xCoordTest_ExpectedResult_True(string data, int xCor)
        {
            _uut.ParseData(data,
                out string tag,
                out int xCoord,
                out int yCoord,
                out int altitude,
                out DateTime time);

            Assert.That(xCoord, Is.EqualTo(xCor));
        }

        [TestCase("ATR423;39045;-23;14000;20151006213456789", -23, TestName = "yCoord_-23")]
        [TestCase("ATR423;39045;0;14000;20151006213456789",0,TestName = "yCoord_0")]
        [TestCase("ATR423;39045;12932;14000;20151006213456789",12932,TestName = "yCoord_12932")]
        public void ParseData_yCoordTest_ExpectedResult_True(string data, int yCor)
        {
            _uut.ParseData(data,
                out string tag,
                out int xCoord,
                out int yCoord,
                out int altitude,
                out DateTime time);

            Assert.That(yCoord, Is.EqualTo(yCor));
        }

        [TestCase("ATR423;39045;12932;-32;20151006213456789",-32,TestName = "Altitude_-32")]
        [TestCase("ATR423;39045;12932;0;20151006213456789",0,TestName = "Altitude_0")]
        [TestCase("ATR423;39045;12932;14000;20151006213456789",14000,TestName = "Altitude_14k")]
        public void ParseData_AltitudeTest_ExpectedResult_True(string data, int alt)
        {
            _uut.ParseData(data,
                out string tag,
                out int xCoord,
                out int yCoord,
                out int altitude,
                out DateTime time);

            Assert.That(altitude, Is.EqualTo(alt));
        }

        [Test]
        public void ParseData_ParseTimeTest_ExpectedResult_True()
        {
            string data = "ATR423;39045;12932;14000;20151006213456789";
            var date = DateTime.Parse("2015-10-06 21:34:56.789");
            _uut.ParseData(data,
                out string tag,
                out int xCoord,
                out int yCoord,
                out int altitude,
                out DateTime time);

            Assert.That(time, Is.EqualTo(date));
        }
    }
}
