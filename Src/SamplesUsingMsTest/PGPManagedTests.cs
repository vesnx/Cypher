using Microsoft.VisualStudio.TestTools.UnitTesting;
using Walter.Cypher.PGP;
using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using System.IO.Compression;
using System.IO;
using System.Security.Cryptography;

namespace Walter.Cypher.PGP.Tests
{
    [TestClass()]
    public class PGPManagedTests
    {
        [TestMethod()]
        public void EncryptTest()
        {
            var keyGen = new PGPKeyGenerator();

            var keyRing = keyGen.GenerateKeys(PGPKeySize.Key2048);
            var pgp = new PGPManaged();
            var largeTextFile = JsonConvert.SerializeObject(keyRing);
            var secure = pgp.Encrypt(largeTextFile, keyRing.PublicKey);

            var copy = pgp.Decrypt(secure, keyRing.PrivateKey);
            Assert.AreEqual(largeTextFile, copy);
        }

        [TestMethod()]
        public void EncryptSmallTest()
        {
            var keyGen = new PGPKeyGenerator();

            var keyRing = keyGen.GenerateKeys(PGPKeySize.Key2048);
            var pgp = new PGPManaged();
            var smallTextFile = "ABC";
            var secure = pgp.Encrypt(smallTextFile, keyRing.PublicKey);

            var copy = pgp.Decrypt(secure, keyRing.PrivateKey);
            Assert.AreEqual(smallTextFile, copy);
        }

        [TestMethod()]
        [ExpectedException(typeof(CryptographicException))]
        public void EncryptFailTest()
        {
            var keyGen = new PGPKeyGenerator();

            var keyRing = keyGen.GenerateKeys(PGPKeySize.Key2048);
            var wrongKey = keyGen.GenerateKeys(PGPKeySize.Key2048);
            var pgp = new PGPManaged();
            var smallTextFile = "ABC";
            var secure = pgp.Encrypt(smallTextFile, keyRing.PublicKey);

            var copy = pgp.Decrypt(secure, wrongKey.PrivateKey);
            Assert.AreEqual(smallTextFile, copy);
        }

        [TestMethod()]
        public void EncryptKeysetTest()
        {
            var keyGen = new PGPKeyGenerator();

            var keyRing = keyGen.GenerateKeys(PGPKeySize.Key2048);
            var wrongKey = keyGen.GenerateKeys(PGPKeySize.Key2048);
            var pgp = new PGPManaged();
            var smallTextFile = "ABC";
            var secure = pgp.Encrypt(smallTextFile, keyRing.PublicKey);

            if (pgp.TryDecrypt(secure, wrongKey.PrivateKey, out var copy) || pgp.TryDecrypt(secure, keyRing.PrivateKey, out copy))
                Assert.AreEqual(smallTextFile, copy);
            else
                Assert.Inconclusive("Decipher failed");
        }

        [TestMethod()]
        public void KeyringTest()
        {
            var keyGen = new PGPKeyGenerator();
            var keyRing2048 = keyGen.GenerateKeys(PGPKeySize.Key2048);
            var keyRing4096 = keyGen.GenerateKeys(PGPKeySize.Key4096);
            var pgp = new PGPManaged();
            var v2048 = pgp.Validate(keyRing2048);
            var v4096 = pgp.Validate(keyRing4096);

            Assert.IsTrue(v2048);
            Assert.AreEqual(v2048, v4096);
        }

        [TestMethod()]
        public void VerifySignatureTest()
        {
            var keyGen = new PGPKeyGenerator();

            var keyRing2048 = keyGen.GenerateKeys(PGPKeySize.Key2048);
            var pgp = new PGPManaged();
            var signature = pgp.Sign("sample", keyRing2048.PrivateKey);
            var actual = pgp.VerifySignature("sample", signature, keyRing2048.PrivateKey);
            Assert.IsTrue(actual);
        }

        [TestMethod()]
        public void VerifySignatureLargeTextTest()
        {
            var keyGen = new PGPKeyGenerator();

            var keyRing512 = keyGen.GenerateKeys(PGPKeySize.Key512);
            var pgp = new PGPManaged();
            var largeText = JsonConvert.SerializeObject(keyRing512);
            var signature = pgp.Sign(largeText, keyRing512.PrivateKey);
            var actual = pgp.VerifySignature(largeText, signature, keyRing512.PrivateKey);
            Assert.IsTrue(actual);
        }

        [TestMethod()]
        public void GenRecoveryTest()
        {
            var key = string.Concat("21A66D4CD5BD51099E4E2C3DAF54D2129668E519A8A3CA02CB7009A8BCDDA981", "-Jh$-", "123456Seven");
            var hash = new HashSet<string>();

            for (int i = 0; i < 10; i++)
            {
                var recovery = new PGPManaged().Encrypt(key, "MjA0OCE8UlNBS2V5VmFsdWU+PE1vZHVsdXM+djYvT29QUjFVU0luSUZ0TkNBbDE3aWltN2RZM2FEbkdRNUlIS0NJQ3dEdEJFcnFqeWVUbllmeldNeUc4YWFNL0Jza0l6cmN6SGs4cUdYSzBIbXNxK1hiRk44d09hZXFIRWN6S0JaRGRIaHhpaTlrVUxWQ0RJdTYxRFJvdXNuam02ZW9MaGs5Y1pRNjhvS2VqTGk0ZnBocW5YOElMRzhTUXNaS2Rnd2NISE52RGtHTC9CR0lwT0UrODhRNWViZUw0dGp2Z3FNRVEzdEYvTytLS1BRcEZWSnBkN0xYbU1SeTFNUXlFY0VheDhEY2VnQjczdkswdlYvdVhZYVV5UUlDSDMzOEljZDNkck52OTBvand2MkRhRWpQbUxxMUtnK1VLVndEdUVtMHEvd2hCMDNBS05mYmZBZEhGLzVJUEQ1N3RGVlNWMDVFaUo1NU9lWk10S04wRmFRPT08L01vZHVsdXM+PEV4cG9uZW50PkFRQUI8L0V4cG9uZW50PjwvUlNBS2V5VmFsdWU+");
                hash.Add(recovery);
            }
            var pgp = new PGP.PGPManaged();
            foreach (var recovery in hash)
            {
                var expect = pgp.Decrypt(recovery, "MjA0OCE8UlNBS2V5VmFsdWU+PE1vZHVsdXM+djYvT29QUjFVU0luSUZ0TkNBbDE3aWltN2RZM2FEbkdRNUlIS0NJQ3dEdEJFcnFqeWVUbllmeldNeUc4YWFNL0Jza0l6cmN6SGs4cUdYSzBIbXNxK1hiRk44d09hZXFIRWN6S0JaRGRIaHhpaTlrVUxWQ0RJdTYxRFJvdXNuam02ZW9MaGs5Y1pRNjhvS2VqTGk0ZnBocW5YOElMRzhTUXNaS2Rnd2NISE52RGtHTC9CR0lwT0UrODhRNWViZUw0dGp2Z3FNRVEzdEYvTytLS1BRcEZWSnBkN0xYbU1SeTFNUXlFY0VheDhEY2VnQjczdkswdlYvdVhZYVV5UUlDSDMzOEljZDNkck52OTBvand2MkRhRWpQbUxxMUtnK1VLVndEdUVtMHEvd2hCMDNBS05mYmZBZEhGLzVJUEQ1N3RGVlNWMDVFaUo1NU9lWk10S04wRmFRPT08L01vZHVsdXM+PEV4cG9uZW50PkFRQUI8L0V4cG9uZW50PjxQPjJRYnJsbFlpK0V6OEluTCtURFpWbmRWVHVLRnJSRDRTTUZ0bXk5RXlNSWRWamtPU1VDbEtubmRrTUx4SlZLRDd6UWpnQ3dIa1lsMnhydnQveTh2enVSeVpRN3ZDOVBKMUszT0RUUjlENUhuQnBNbGoyWEtFUWNhMk9UWVYyeTdzbExKRm1tL2pUVGtzOVZWa1AwUTRPd2JiT2VKUWhFVDVFdjBmKzNXYkpqYz08L1A+PFE+NGh2MEU4OEV0TzFMbjFsVzJPek9ZSU9IVEVDYkx4L1VEVjJXa0hvWTRwKzFZM0VwMzhwRkR1WXBNeHVoMmUrcFMzdFhrNHdzMFV6ODZqUnIwYzJwSXhKb0dDMXFaQ2VNdXFkb2FqWVBpU3N5VjRmL2ZMVStiL1V2UW1XYjJUQ2lPdDg2UE5UODhIRkM4T1doQzZqZWxSa09namZPZlBDb3psR3ltMFFwWVY4PTwvUT48RFA+QUc4T0czL1NsQko3VW9wT1RkS3greDNKREE1dWkzVmdUZTV3MXlsMDR6bnlCdFhGdmhsSEIrZ1BNRFhBSThZcW1xOVEyMVRHb3hleTJqbXdlTFRRcU9jUXUxenR0OVdnUEZUL3h6Q3Z6dzRiUC93VEVnVXpsSG9VTlNzUmdoMm01V2t4MEd3MjFSbXZLRkppWCtuLy9zWTF1L2ppMWxEWStwdTg2NkRCamFNPTwvRFA+PERRPnpkRFNHT0hkRCsvY1lUMUtQNFUyeWtXUGQ0cHJkN2JLS0N4amJEbW53MG5MSzZ5TGlFaXZHY1BLQnRxRk5rTGdZSFB5b0x6ODRydm9rQ3VOWlZtR053Vi9PVnJGdUVzRDM3ZVk1TzAyc1E1ZjhhczZsVUxKaGRHeDVnZGxtSXFiY1dsV3NwN1JhdkErRmRlQUE0UUFyOGt5R3JnL3ZoeTJHNi9rRzA4dE41TT08L0RRPjxJbnZlcnNlUT5HSHlzUzg5K1ByVFU1Q1RLaThpRnRjY1RGbXJlYm5ob3pNRkFvVUdGVUlOcFlpTjVORkhTWHdsdEx0Y096b3ZnL2dzdEllYnB2VVp3czNXYmFPdnVkWlpSYWtXNVZTR0I2eDd5MW1YM01KZkZISDk2NFQ0TWJXc2lnTmFHU25BSTI5ZTRxWTh6Vkx6TjZiWHRIQjZBbjhZY0RMbDFGVmpIN0w0eWp6bUZDK009PC9JbnZlcnNlUT48RD5rU3p5ZnA2R2w2c1pIUDFwQ3ZRM1YrZDcvZGRtNFU0WVphYWNPdjMvUFYzak8rOFZDMXlORFg1ek1BaVY3Ui9SSlM5dXR6aXl3M3JMZExpVnlBLzhYVEZoem8rQ1B4OTdxclNDTU5MMVZNL0Vwd3dDcHdzNk5tTzV4YkdWdWwrczYyM3h5b2dpZnZzNVN5ZUxnL2MweXhXV3ZBUjNhMUZsRU1mcytZYUNFWXprSXlUN1M1Rzh4TG1Bc09DRGpxT3pkTE5kNEZZSDRkTExLL3FzNEZHaEREWjZqSzVZSXFmRVRKOFVyYTlwZ1NGVDNPUHJmRXh3d1dEMWgrMTRzeWp0S0Y4V0kvUWtoQ0FmSkZZMFhLakwvTFlTUmlHenEyL3IwWlZwREptUUR0bm8raDJVUjFtUFdtSHJHcTVPQWFVNjRYaEthNVFQZFpacmk1eU9rcGRtL1E9PTwvRD48L1JTQUtleVZhbHVlPg==");
                Assert.AreEqual(expect, key);
            }
        }
    }
}