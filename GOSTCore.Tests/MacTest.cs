using System;
using System.Text;
using NUnit.Framework;

namespace GOSTCore.Tests
{
    public class MacTest
    {
        [Test]
        public void PassWithBytes()
        {
            byte[] key = Encoding.UTF8.GetBytes("12345678901234567890123456789012");
            byte[] message1 = Encoding.Default.GetBytes("1234567887654321");
            byte[] message2 = Encoding.Default.GetBytes("test");
            byte[] message3 = Encoding.Default.GetBytes("message");
            byte[] message4 = Encoding.Default.GetBytes("message");

            var mac1 = Mac.Generate(key, message1);
            var mac2 = Mac.Generate(key, message2);
            var mac3 = Mac.Generate(key, message3);
            var mac4 = Mac.Generate(key, message4);

            Assert.IsNotNull(mac1);
            Assert.IsNotNull(mac2);
            Assert.IsNotNull(mac3);
            Assert.IsNotNull(mac4);
            Assert.IsNotEmpty(mac1);
            Assert.IsNotEmpty(mac2);
            Assert.IsNotEmpty(mac3);
            Assert.IsNotEmpty(mac4);
            Assert.AreNotEqual(mac1, mac2);
            Assert.AreNotEqual(mac2, mac3);
            Assert.AreEqual(mac4, mac3);
        }

        [Test]
        public void PassWithStrings()
        {
            var mac1 = Mac.Generate("12345678901234567890123456789012", "1234567887654321");
            var mac2 = Mac.Generate("12345678901234567890123456789012", "test");
            var mac3 = Mac.Generate("12345678901234567890123456789012", "message");
            var mac4 = Mac.Generate("12345678901234567890123456789012", "message");

            Assert.IsNotNull(mac1);
            Assert.IsNotNull(mac2);
            Assert.IsNotNull(mac3);
            Assert.IsNotNull(mac4);
            Assert.IsNotEmpty(mac1);
            Assert.IsNotEmpty(mac2);
            Assert.IsNotEmpty(mac3);
            Assert.IsNotEmpty(mac4);
            Assert.AreNotEqual(mac1, mac2);
            Assert.AreNotEqual(mac2, mac3);
            Assert.AreEqual(mac4, mac3);
        }

        [Test]
        public void FailWithShortKey()
        {
            Assert.Throws<ArgumentException>(() => Mac.Generate("12345678901234567890123456789", "message"));
        }

        [Test]
        public void FailWithNullData()
        {
            Assert.Throws<ArgumentNullException>(() => Mac.Generate(null, "1234567887654321"));
            Assert.Throws<ArgumentNullException>(() => Mac.Generate("12345678901234567890123456789012", null));
            Assert.Throws<ArgumentNullException>(() => Mac.Generate((byte[])null, null));
        }

        [Test]
        public void FailWithEmptyData()
        {
            byte[] key = Encoding.UTF8.GetBytes("12345678901234567890123456789012");
            byte[] message = Encoding.Default.GetBytes("1234567887654321");

            Assert.Throws<ArgumentException>(() => Mac.Generate(new byte[] { }, message));
            Assert.Throws<ArgumentException>(() => Mac.Generate(key, new byte[] { }));
            Assert.Throws<ArgumentException>(() => Mac.Generate(new byte[] { }, new byte[] { }));
        }
    }
}
