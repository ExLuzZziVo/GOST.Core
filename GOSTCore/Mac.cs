using System;
using System.Text;
using GOSTCore.Gost;

namespace GOSTCore
{
    public static class Mac
    {
        /// <summary>
        /// MAC generator.
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="message">Message</param>
        /// <returns>Generated message</returns>
        public static byte[] Generate(byte[] key, byte[] message)
        {
            CheckData(key, message);

            using var gost = new GostManager(key, null, message);
            return gost.Generate();
        }

        /// <summary>
        /// MAC generator.
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="message">Message</param>
        /// <returns>Generated message</returns>
        public static byte[] Generate(string key, string message)
        {
            CheckData(key, message);

            var byteKey = Encoding.Default.GetBytes(key);
            var byteMessage = Encoding.Default.GetBytes(message);

            return Generate(byteKey, byteMessage);
        }

        private static void CheckData(object key, object iv)
        {
            if (key is null || iv is null)
            {
                throw new ArgumentNullException();
            }
        }
    }
}
