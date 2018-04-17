using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NSubstitute;
using NUnit.Framework;
using TransponderLib;

namespace TestUnit
{
    [TestFixture]
    class UT_IOutput
    {
        private IOutput _uut;
        [SetUp]
        public void Setup()
        {
            _uut = Substitute.For<IOutput>();
        }

        [Test]
        public void Test_Reset_Expected_Called_Once()
        {
            _uut.Reset();

            _uut.Received(1).Reset();
        }

        [Test]
        public void Test_Print_Expected_Called_Once_With_Args()
        {
            _uut.Print("hello world");

            _uut.Received().Print(Arg.Is<string>(s => s == "hello world"));    
        }
    }
}
