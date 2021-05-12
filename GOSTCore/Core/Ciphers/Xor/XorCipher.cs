using System;
using System.Collections.Generic;
using GOSTCore.Core.Ciphers.Substitution;
using GOSTCore.Core.SBlocks.IBlock;

namespace GOSTCore.Core.Ciphers.Xor
{
    internal class XorCipher : SubstitutionCipher, ICipher
    {
        private uint n3;
        private uint n4;

        public XorCipher(byte[] iv, List<uint> subKeys, ISBlock sBlock) : base(sBlock)
        {
            SetIV(iv, subKeys);
        }

        /// <summary>
        /// Encode.
        /// </summary>
        /// <param name="data">Decoded data</param>
        /// <param name="subKeys">Subkeys</param>
        /// <returns>Encoded data</returns>
        public new byte[] Encode(byte[] data, List<uint> subKeys)
        {
            n3 += 16843009 % 4294967295;
            n4 += 16843012 % 4294967294;

            var n1 = n3;
            var n2 = n4;

            var gamma = new byte[8];
            Array.Copy(BitConverter.GetBytes(n1), 0, gamma, 0, 4);
            Array.Copy(BitConverter.GetBytes(n2), 0, gamma, 4, 4);
            gamma = base.Encode(gamma, subKeys);

            return XOR(gamma, data);
        }

        /// <summary>
        /// Decode.
        /// </summary>
        /// <param name="data">Encoded data</param>
        /// <param name="subKeys">Subkeys</param>
        /// <returns>Decoded data</returns>
        public new byte[] Decode(byte[] data, List<uint> subKeys)
        {
            return Encode(data, subKeys);
        }

        /// <summary>
        /// Set cipher init state.
        /// </summary>
        /// <param name="iv">init vector</param>
        /// <param name="subKeys">Subkeys</param>
        private void SetIV(byte[] iv, List<uint> subKeys)
        {
            var encodedIV = base.Encode(iv, subKeys);

            n3 = BitConverter.ToUInt32(encodedIV, 0);
            n4 = BitConverter.ToUInt32(encodedIV, 4);
        }

        /// <summary>
        /// Apply XOR on gamma and data block.
        /// </summary>
        /// <param name="gamma">Gamma</param>
        /// <param name="data">Data block</param>
        /// <returns>Result</returns>
        private byte[] XOR(byte[] gamma, byte[] data)
        {
            var len = data.Length;
            var res = new byte[len];

            for (var i = 0; i != len; i++)
            {
                res[i] = (byte) (gamma[i] ^ data[i]);
            }

            return res;
        }
    }
}