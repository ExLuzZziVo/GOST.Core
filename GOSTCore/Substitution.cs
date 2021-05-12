using System;
using System.Text;
using GOSTCore.Gost;
using GOSTCore.Gost.Types;

namespace GOSTCore
{
    public static class Substitution
    {
        /// <summary>
        /// Substitution encode.
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="message">Message</param>
        /// <param name="sBlockType">SBlock type</param>
        /// <returns>Encoded</returns>
        public static byte[] Encode(byte[] key, byte[] message, SBlockTypes sBlockType)
        {
            CheckData(key, message);

            if (message.Length % 8 != 0)
            {
                throw new ArgumentException("Block must have 64 bit length");
            }

            using var gost = new GostManager(key, null, message, CipherTypes.Substitution, sBlockType);

            return gost.Encode();
        }

        /// <summary>
        /// Substitution encode.
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="message">Message</param>
        /// <param name="sBlockType">SBlock type</param>
        /// <returns>Encoded</returns>
        public static byte[] Encode(string key, string message, SBlockTypes sBlockType)
        {
            CheckData(key, message);

            var byteKey = Encoding.Default.GetBytes(key);
            var byteMessage = Encoding.Default.GetBytes(message);

            return Encode(byteKey, byteMessage, sBlockType);
        }

        /// <summary>
        /// Substitution decode.
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="message">Message</param>
        /// <param name="sBlockType">SBlock type</param>
        /// <returns>Decoded</returns>
        public static byte[] Decode(byte[] key, byte[] message, SBlockTypes sBlockType)
        {
            CheckData(key, message);

            if (message.Length % 8 != 0)
            {
                throw new ArgumentException("Block must have 64 bit length");
            }

            using var gost = new GostManager(key, null, message, CipherTypes.Substitution, sBlockType);

            return gost.Decode();
        }

        /// <summary>
        /// Substitution decode.
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="message">Message</param>
        /// <param name="sBlockType">SBlock type</param>
        /// <returns>Decoded</returns>
        public static byte[] Decode(string key, string message, SBlockTypes sBlockType)
        {
            CheckData(key, message);

            var byteKey = Encoding.Default.GetBytes(key);
            var byteMessage = Encoding.Default.GetBytes(message);

            return Decode(byteKey, byteMessage, sBlockType);
        }

        private static void CheckData(object key, object message)
        {
            if (key is null || message is null)
            {
                throw new ArgumentNullException();
            }
        }
    }
}