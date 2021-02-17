using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Walter.Cypher.Tests
{
    [TestClass()]
    public class PasswordGeneratorTests
    {
        [TestMethod()]
        public void GeneratePasswordTest()
        {
            const string actual = "Pa$$w0rd123";
            var cipher = PasswordGenerator.GeneratePassword(actual);
            var expect = PasswordGenerator.TryValidateGeneratedPassword(cipher, actual, out var tampered);
            Assert.IsTrue(expect);
            Assert.IsFalse(tampered);
        }

        [TestMethod()]
        public void GeneratePasswordTest2()
        {
            const string actual = "this is a test";
            var cipher = PasswordGenerator.GeneratePassword(actual);
            cipher = cipher.Replace(cipher[6], cipher[5]);
            var expect = PasswordGenerator.TryValidateGeneratedPassword(cipher, actual, out var _);
            Assert.IsFalse(expect);
        }
    }
}