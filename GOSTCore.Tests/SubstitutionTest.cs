using System;
using System.Text;
using GOSTCore.Gost.Types;
using NUnit.Framework;

namespace GOSTCore.Tests
{
    public class SubstitutionTest
    {
        [Test]
        public void PassWithBytes()
        {
            byte[] key = Encoding.UTF8.GetBytes("12345678901234567890123456789012");
            byte[] message = Encoding.Default.GetBytes("1234567887654321");

            var encoded = Substitution.Encode(key, message, SBlockTypes.GOST);
            var decoded = Substitution.Decode(key, message, SBlockTypes.GOST);

            Assert.IsNotNull(encoded);
            Assert.IsNotNull(decoded);
            Assert.IsNotEmpty(encoded);
            Assert.IsNotEmpty(decoded);
            Assert.AreEqual(decoded, encoded);
        }

        [Test]
        public void PassWithStrings()
        {
            var encoded = Substitution.Encode("12345678901234567890123456789012", "1234567887654321", SBlockTypes.GOST);
            var decoded = Substitution.Decode("12345678901234567890123456789012", "1234567887654321", SBlockTypes.GOST);

            Assert.IsNotNull(encoded);
            Assert.IsNotNull(decoded);
            Assert.IsNotEmpty(encoded);
            Assert.IsNotEmpty(decoded);
            Assert.AreEqual(decoded, encoded);
        }

        [Test]
        public void FailWithNullData()
        {
            byte[] key = Encoding.UTF8.GetBytes("12345678901234567890123456789012");
            byte[] message = Encoding.UTF8.GetBytes("1234567887654321");

            Assert.Throws<ArgumentNullException>(() => Substitution.Encode(null, message, SBlockTypes.GOST));
            Assert.Throws<ArgumentNullException>(() => Substitution.Encode(key, null, SBlockTypes.GOST));
            Assert.Throws<ArgumentNullException>(() => Substitution.Encode((byte[])null, null, SBlockTypes.GOST));
        }

        [Test]
        public void FailWithEmptyData()
        {
            byte[] key = Encoding.UTF8.GetBytes("12345678901234567890123456789012");
            byte[] message = Encoding.UTF8.GetBytes("1234567887654321");

            Assert.Throws<ArgumentException>(() => Substitution.Encode(new byte[] { }, message, SBlockTypes.GOST));
            Assert.Throws<ArgumentException>(() => Substitution.Encode(key, new byte[] { }, SBlockTypes.GOST));
            Assert.Throws<ArgumentException>(() => Substitution.Encode(new byte[] { }, new byte[] { }, SBlockTypes.GOST));
        }

        [Test]
        public void FailWithNotEqualsKeys()
        {
            var encoded = Substitution.Encode("12345678901234567890123456789012", "1234567887654321", SBlockTypes.GOST);
            var decoded = Substitution.Decode("00000000000000000000000000000000", "1234567887654321", SBlockTypes.GOST);

            Assert.IsNotNull(encoded);
            Assert.IsNotNull(decoded);
            Assert.IsNotEmpty(encoded);
            Assert.IsNotEmpty(decoded);
            Assert.AreNotEqual(decoded, encoded);
        }

        [Test]
        public void FailWithShortKey()
        {
            Assert.Throws<ArgumentException>(() => Substitution.Encode("12345678901234567890123456789", "1234567887654321", SBlockTypes.GOST));
        }

        [Test]
        public void FailWithShortMessage()
        {
            Assert.Throws<ArgumentException>(() => Substitution.Encode("12345678901234567890123456789", "message", SBlockTypes.GOST));
        }

        [Test]
        public void FailWithNotEqualsSBlocks()
        {
            var encoded = Substitution.Encode("12345678901234567890123456789012", "1234567887654321", SBlockTypes.CryptoProB);
            var decoded = Substitution.Decode("12345678901234567890123456789012", "1234567887654321", SBlockTypes.GOST);

            Assert.IsNotNull(encoded);
            Assert.IsNotNull(decoded);
            Assert.IsNotEmpty(encoded);
            Assert.IsNotEmpty(decoded);
            Assert.AreNotEqual(decoded, encoded);
        }
    }
}