// ***********************************************************************
// Assembly         : Walter.Web.CypherTests
// Author           : Walter Verhoeven
// Created          : Fri 01-Mar-2024
//
// Last Modified By : Walter Verhoeven
// Last Modified On : Fri 01-Mar-2024
// ***********************************************************************
// <copyright file="StringExtensionsTests.cs" company="Walter.Web.CypherTests">
//     Copyright (c) VESNX SA. All rights reserved.
// </copyright>
// <summary>
// use tests to show case some features
// </summary>
// ***********************************************************************
using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Collections.Generic;
using System.Text;

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