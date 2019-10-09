using System;
using System.Text;
using GOSTCore.Gost;
using GOSTCore.Gost.Types;

namespace GOSTCore
{
    public static class Cfb
    {
        /// <summary>
        /// CFB encode.
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="iv">Init vector</param>
        /// <param name="message">Message</param>
        /// <param name="sBlockType">SBlock type</param>
        /// <returns>Encoded</returns>
        public static byte[] Encode(byte[] key, byte[] iv, byte[] message, SBlockTypes sBlockType)
        {
            CheckData(key, iv, message);

            using var gost = new GostManager(key, iv, message, CipherTypes.Cfb, sBlockType);
            return gost.Encode();
        }

        /// <summary>
        /// CFB encode.
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="iv">Init vector</param>
        /// <param name="message">Message</param>
        /// <param name="sBlockType">SBlock type</param>
        /// <returns>Encoded</returns>
        public static byte[] Encode(string key, string iv, string message, SBlockTypes sBlockType)
        {
            CheckData(key, iv, message);

            var byteKey = Encoding.Default.GetBytes(key);
            var byteMessage = Encoding.Default.GetBytes(message);
            var byteIv = Encoding.Default.GetBytes(iv);

            return Encode(byteKey, byteIv, byteMessage, sBlockType);
        }

        /// <summary>
        /// CFB decode.
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="iv">Init vector</param>
        /// <param name="message">Message</param>
        /// <param name="sBlockType">SBlock type</param>
        /// <returns>Decoded</returns>
        public static byte[] Decode(byte[] key, byte[] iv, byte[] message, SBlockTypes sBlockType)
        {
            CheckData(key, iv, message);

            using var gost = new GostManager(key, iv, message, CipherTypes.Cfb, sBlockType);
            return gost.Decode();
        }

        /// <summary>
        /// CFB decode.
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="iv">Init vector</param>
        /// <param name="message">Message</param>
        /// <param name="sBlockType">SBlock type</param>
        /// <returns>Decoded</returns>
        public static byte[] Decode(string key, string iv, string message, SBlockTypes sBlockType)
        {
            CheckData(key, iv, message);

            var byteKey = Encoding.Default.GetBytes(key);
            var byteMessage = Encoding.Default.GetBytes(message);
            var byteIv = Encoding.Default.GetBytes(iv);

            return Encode(byteKey, byteIv, byteMessage, sBlockType);
        }

        private static void CheckData(object key, object iv, object message)
        {
            if (key is null || iv is null || message is null)
            {
                throw new ArgumentNullException();
            }
        }
    }
}
