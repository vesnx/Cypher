// ***********************************************************************
// Assembly         : Walter.Web.CypherTests
// Author           : Walter Verhoeven
// Created          : Fri 01-Mar-2024
//
// Last Modified By : Walter Verhoeven
// Last Modified On : Fri 01-Mar-2024
// ***********************************************************************
// <copyright file="ChecksumTest.cs" company="Walter.Web.CypherTests">
//     Copyright (c) VESNX SA. All rights reserved.
// </copyright>
// <summary>
// use tests to show case some features
// </summary>
// ***********************************************************************
using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;

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
            Assert.AreEqual(1468089490, cs.GetHashCode());
        }
        [TestMethod]
        public void HaschodeEqualIntTest()
        {
            var cs = new Checksum(123);
            Assert.AreEqual(1078348337, cs.GetHashCode());
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
