using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignalAcquisitionDemo.Helper;
using System;

namespace DemoTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var s1 = ParsingData.ParseFloat(new byte[] { 0x42, 0xC8, 0x00, 0x00 });
            var s2 = ParsingData.ParseFloat(new byte[] { 0x41, 0xA3, 0x0A, 0x3D });
            var s3 = ParsingData.ParseFloat(new byte[] { 0x42, 0x20, 0x00, 0x00 });
            var s4 = ParsingData.ParseFloat(new byte[] { 0x41, 0xF7, 0xAE, 0x14 });
        }
    }
}
