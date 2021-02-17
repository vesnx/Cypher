using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Walter.Web.CypherTests
{
    [TestClass()]
    public class ChecksumTest
    {
        [TestMethod]
        public void TestConstructot()
        {
            var cs = new Checksum("123");
            
            Assert.AreNotEqual(123, cs.GetHashCode());            
        }

        [TestMethod]
        public void HaschodeTest()
        {
            var cs = new Checksum("123");
            
            Assert.AreNotEqual("123".GetHashCode(), cs.GetHashCode());            
        }

        [TestMethod]
        public void HaschodeEqualTest()
        {
            var cs = new Checksum("123");            
            Assert.AreEqual(468528679, cs.GetHashCode());            
        }
        [TestMethod]
        public void HaschodeEqualIntTest()
        {
            var cs = new Checksum(123);            
            Assert.AreEqual(1410565602, cs.GetHashCode());            
        }
        [TestMethod]
        public void HaschodeEqualIntTest2()
        {
            var cs = new Checksum(12);
            var value = 3;
            cs.Add(value);
            Assert.AreNotEqual(1410565602, cs.GetHashCode());            
        }

    }
}
