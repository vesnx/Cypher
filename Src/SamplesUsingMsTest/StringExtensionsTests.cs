using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text;
using System.Collections.Generic;
using System;

namespace Walter.Cypher.Tests
{
    [TestClass()]
    public class StringExtensionsTests
    {
        [TestMethod()]
        public void AsNumericTest()
        {
            var str = "Test string";
            var result = str.AsNumeric();
            Assert.IsNotNull(result);
        }

        [TestMethod()]
        public void AsNumericLengthTest()
        {
            var str = "T";
            var result = str.AsNumeric();
            Assert.IsTrue(result.Length > str.Length);
        }

        [TestMethod()]
        public void AsFromNumericTest()
        {
            var str = "Test string";
            var result = str.AsNumeric();
            Assert.IsNotNull(result);
            var actual = result.FromNumeric();
            Assert.AreEqual(str, actual);
        }

        [TestMethod()]
        public void AsFromNumericNeverSameTest()
        {
            var str = "Test string";
            var result = str.AsNumeric();
            var other = str.AsNumeric();
            var actual = result.FromNumeric();
            Assert.AreNotEqual(other.Length, actual.Length);
            Assert.AreNotEqual(other, actual);
            Assert.AreEqual(result.FromNumeric(), other.FromNumeric());
            Assert.AreEqual(result.FromNumeric(), str);
        }

        [TestMethod()]
        public void AsFromNumericArrayTest()
        {
            var str = "Test string 123 is really quite long and one can't save it in a single long value";

            str.TryAsNumeric(out IEnumerable<ulong> values);
            var actual = values.FromNumeric();

            Assert.AreEqual(str, actual);
        }
    }
}