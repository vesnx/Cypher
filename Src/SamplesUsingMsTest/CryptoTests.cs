// ***********************************************************************
// Assembly         : Walter.Web.CypherTests
// Author           : Walter Verhoeven
// Created          : Fri 01-Mar-2024
//
// Last Modified By : Walter Verhoeven
// Last Modified On : Fri 01-Mar-2024
// ***********************************************************************
// <copyright file="CryptoTests.cs" company="Walter.Web.CypherTests">
//     Copyright (c) VESNX SA. All rights reserved.
// </copyright>
// <summary>
// use tests to show case some features
// </summary>
// ***********************************************************************
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace Walter.Cypher.Tests
{
    using System.Text;
    using Walter.Cypher;

    [TestClass()]
    public class CryptoTests
    {
        [TestMethod()]
        public void PasswordRoundTripTest()
        {

            var bytes= Crypto.Cipher("Test this string", password: "sxWSxogHXTetqTzrOijNX7kivWOfMkiUB5kH5apsTEdmZIlYVSQPavgqeDhNhgNd==", method: CipherMethod.Fast);

            var value= Crypto.Decipher(bytes, password: "sxWSxogHXTetqTzrOijNX7kivWOfMkiUB5kH5apsTEdmZIlYVSQPavgqeDhNhgNd==", method: CipherMethod.Fast);
            Assert.AreEqual(value, "Test this string");
        }

        [TestMethod()]
        public void CypherTestCertificate()
        {
            var file = new FileInfo("Walter.Cypher.pfx");
            if (!file.Exists)
                CreateCertificate(file, "123456789");

            var cypkered = Crypto.Cipher("Test", file.FullName, "123456789");
            Assert.IsTrue(cypkered != null);
            Crypto.Free();
        }

        void CreateCertificate(FileInfo file, string password) 
        {
            PfxCertificateGenerator.GeneratePfxCertificate(file.FullName, password);
        }


        [TestMethod()]
        public void CypherTestDefault()
        {
            var cypkered = Crypto.Cipher("Test");
            Assert.IsTrue(cypkered != null);
        }

        [TestMethod()]
        public void CypherTestSalt()
        {
            var cSalt = Crypto.Cipher("Test", "Test");
            var c = Crypto.Cipher("Test");
            Assert.AreNotEqual(c, cSalt);
        }

        [TestMethod()]
        public void CypherDecipherSalt()
        {
            var cSalt = Crypto.Cipher("Test");
            var c = Crypto.Decipher(cSalt);
            Assert.AreEqual(c, "Test");
        }

        [TestMethod()]
        public void CypherDecipher()
        {
            var cSalt = Crypto.Cipher("Test", "pw");
            var c = Crypto.Decipher(cSalt, "pw");
            Assert.AreEqual("Test", c);
        }

        [TestMethod()]
        public void CypherTestCertificateRoundTrip()
        {
            var test = Guid.NewGuid().ToString();
            var file = new FileInfo("Walter.Cypher.pfx");

            if (!file.Exists)
                CreateCertificate(file, "123456789");

            var cypkered = Crypto.Cipher(test, file.FullName, "123456789");
            var expect = Crypto.Decrypt(cypkered, file.FullName, "123456789");
            Assert.AreEqual(test, expect);
            Crypto.Free();
        }

        [TestMethod()]
        public void ZipRoundTrip()
        {
            var test = Guid.NewGuid().ToString();

            var cypkered = Crypto.Zip(test);
            var expect = Crypto.UnZip(cypkered);
            Assert.AreEqual(test, expect);
        }

        [TestMethod()]
        public void CypherZipRoundTrip()
        {
            var test = Guid.NewGuid().ToString();

            var cypkered = Crypto.Zip(test, "123456789");
            var expect = Crypto.UnZip(cypkered, "123456789");
            Assert.AreEqual(test, expect);
        }

        [TestMethod()]
        public void CypherByteArrrayTest()
        {
            var test = Guid.NewGuid().ToString();

            var cypkered = Crypto.Cipher(test, "123456789", method: CipherMethod.Defauilt);
            var expect = Crypto.Decipher(cypkered, "123456789", CipherMethod.Defauilt);
            Assert.AreEqual(test, expect);
        }

        [TestMethod()]
        public void CypherByteArrrayLongPasswordTest()
        {
            var sb = new StringBuilder();
            for (var i = 0; i < 1024; i++)
            {
                sb.Append(DateTime.Now.ToString());
            }
            var test = sb.ToString();

            var cypkered = Walter.Cypher.Crypto.Cipher(test, "12345678988888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888", method: CipherMethod.Defauilt);
            var expect = Crypto.Decipher(cypkered, "12345678988888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888", CipherMethod.Defauilt);
            Assert.AreEqual(test, expect);
        }

        [TestMethod()]
        public void CypherByteArrrayLongPasswordTestFast()
        {
            var sb = new StringBuilder();
            for (var i = 0; i < 1024; i++)
            {
                sb.Append(DateTime.Now.ToString());
            }
            var test = sb.ToString();

            var cypkered = Walter.Cypher.Crypto.Cipher(test, "12345678988888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888", method: CipherMethod.Fast);
            var expect = Crypto.Decipher(cypkered, "12345678988888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888", CipherMethod.Fast);
            Assert.AreEqual(test, expect);
        }

        [TestMethod()]
        public void CypherByteArrrayLongPasswordTestSecure()
        {
            var sb = new StringBuilder();
            for (var i = 0; i < 1024; i++)
            {
                sb.Append(DateTime.Now.ToString());
            }
            var test = sb.ToString();

            var cypkered = Walter.Cypher.Crypto.Cipher(test, "12345678988888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888", method: CipherMethod.Secure);
            var expect = Crypto.Decipher(cypkered, "12345678988888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888", CipherMethod.Secure);
            Assert.AreEqual(test, expect);
        }

        [TestMethod()]
        public void CipherUserAndPassword()
        {
            var sb = new StringBuilder();
            for (var i = 0; i < 1024; i++)
            {
                sb.Append(DateTime.Now.ToString());
            }
            var test = sb.ToString();

            var cypkered = Walter.Cypher.Crypto.Cipher(test, "12345678988888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888");
            var expect = Crypto.Decipher(cypkered, "12345678988888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888");
            Assert.AreEqual(test, expect);
        }

        [TestMethod()]
        public void CipherUserNoPassword()
        {
            var sb = new StringBuilder();
            for (var i = 0; i < 1024; i++)
            {
                sb.Append(DateTime.Now.ToString());
            }
            var test = sb.ToString();

            var cypkered = Crypto.Cipher(test);
            var expect = Crypto.Decipher(cypkered);
            Assert.AreEqual(test, expect);
        }

        [TestMethod()]
        public void CipherZipCipher()
        {
            var sb = new StringBuilder();
            for (var i = 0; i < 1024; i++)
            {
                sb.Append(DateTime.Now.ToString());
            }
            var test = sb.ToString();

            var cypkered = Crypto.Zip(test, "Password");
            var expect = Crypto.UnZip(cypkered, "Password");
            Assert.AreEqual(test, expect);
        }

        [TestMethod()]
        public void CipherZipCipherNumerics()
        {
            var sb = new StringBuilder();
            for (var i = 0; i < 1024; i++)
            {
                sb.Append(DateTime.Now.ToString());
            }
            var test = sb.ToString();

            var cypkered = Crypto.Zip(test);
            var expect = Crypto.UnZip(cypkered);
            Assert.AreEqual(test, expect);

        }

        [TestMethod()]
        public void CipherZip()
        {
            var sb = new StringBuilder();
            for (var i = 0; i < 1024; i++)
            {
                sb.Append(DateTime.Now.ToString());
            }
            var test = sb.ToString();

            var cypkered = Crypto.Zip(test);
            var expect = Crypto.UnZip(cypkered);
            Assert.AreEqual(test, expect);
        }
    }
}