using System;
using System.Text;
using GOSTCore.Gost.Types;
using NUnit.Framework;

namespace GOSTCore.Tests
{
    public class XorTest
    {
        [Test]
        public void PassWithBytes()
        {
            byte[] key = Encoding.UTF8.GetBytes("12345678901234567890123456789012");
            byte[] iv = Encoding.Default.GetBytes("12345678");
            byte[] message = Encoding.Default.GetBytes("message");

            var encoded = Xor.Encode(key, iv, message, SBlockTypes.GOST);
            var decoded = Xor.Decode(key, iv, message, SBlockTypes.GOST);

            Assert.IsNotNull(encoded);
            Assert.IsNotNull(decoded);
            Assert.IsNotEmpty(encoded);
            Assert.IsNotEmpty(decoded);
            Assert.AreEqual(decoded, encoded);
        }

        [Test]
        public void PassWithStrings()
        {
            var encoded = Xor.Encode("12345678901234567890123456789012", "12345678", "message", SBlockTypes.GOST);
            var decoded = Xor.Decode("12345678901234567890123456789012", "12345678", "message", SBlockTypes.GOST);

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
            byte[] iv = Encoding.Default.GetBytes("12345678");
            byte[] message = Encoding.UTF8.GetBytes("message");

            Assert.Throws<ArgumentNullException>(() => Xor.Encode(null, iv, message, SBlockTypes.GOST));
            Assert.Throws<ArgumentNullException>(() => Xor.Encode(key, null, null, SBlockTypes.GOST));
            Assert.Throws<ArgumentNullException>(() => Xor.Encode(key, null, message, SBlockTypes.GOST));
            Assert.Throws<ArgumentNullException>(() => Xor.Encode((byte[])null, null, null, SBlockTypes.GOST));
        }

        [Test]
        public void FailWithEmptyData()
        {
            byte[] key = Encoding.UTF8.GetBytes("12345678901234567890123456789012");
            byte[] iv = Encoding.Default.GetBytes("12345678");
            byte[] message = Encoding.UTF8.GetBytes("message");

            Assert.Throws<ArgumentException>(() => Xor.Encode(new byte[] { }, iv, message, SBlockTypes.GOST));
            Assert.Throws<ArgumentException>(() => Xor.Encode(key, new byte[] { }, new byte[] { }, SBlockTypes.GOST));
            Assert.Throws<ArgumentException>(() => Xor.Encode(key, new byte[] { }, message, SBlockTypes.GOST));
            Assert.Throws<ArgumentException>(() => Xor.Encode(new byte[] { }, new byte[] { }, new byte[] { }, SBlockTypes.GOST));
        }

        [Test]
        public void FailWithNotEqualsKeys()
        {
            var encoded = Xor.Encode("12345678901234567890123456789012", "12345678", "message", SBlockTypes.GOST);
            var decoded = Xor.Decode("00000000000000000000000000000000", "12345678", "message", SBlockTypes.GOST);

            Assert.IsNotNull(encoded);
            Assert.IsNotNull(decoded);
            Assert.IsNotEmpty(encoded);
            Assert.IsNotEmpty(decoded);
            Assert.AreNotEqual(decoded, encoded);
        }

        [Test]
        public void FailWithNotEqualsIvs()
        {
            var encoded = Xor.Encode("12345678901234567890123456789012", "12345678", "message", SBlockTypes.GOST);
            var decoded = Xor.Decode("12345678901234567890123456789012", "00000000", "message", SBlockTypes.GOST);

            Assert.IsNotNull(encoded);
            Assert.IsNotNull(decoded);
            Assert.IsNotEmpty(encoded);
            Assert.IsNotEmpty(decoded);
            Assert.AreNotEqual(decoded, encoded);
        }

        [Test]
        public void FailWithShortKey()
        {
            Assert.Throws<ArgumentException>(() => Xor.Encode("12345678901234567890123456789012", "1234678", "message", SBlockTypes.GOST));
        }

        [Test]
        public void FailWithShortIv()
        {
            Assert.Throws<ArgumentException>(() => Xor.Encode("12345678901234567890123456789", "1245678", "message", SBlockTypes.GOST));
        }

        [Test]
        public void FailWithNotEqualsSBlocks()
        {
            var encoded = Xor.Encode("12345678901234567890123456789012", "12345678", "message", SBlockTypes.CryptoProB);
            var decoded = Xor.Decode("12345678901234567890123456789012", "12345678", "message", SBlockTypes.GOST);

            Assert.IsNotNull(encoded);
            Assert.IsNotNull(decoded);
            Assert.IsNotEmpty(encoded);
            Assert.IsNotEmpty(decoded);
            Assert.AreNotEqual(decoded, encoded);
        }
    }
}
