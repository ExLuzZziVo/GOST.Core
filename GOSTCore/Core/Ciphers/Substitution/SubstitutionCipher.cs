using System;
using System.Collections.Generic;
using GOSTCore.Core.SBlocks.IBlock;

namespace GOSTCore.Core.Ciphers.Substitution
{
    internal class SubstitutionCipher : ICipher
    {
        private readonly ISBlock _sBlock;

        public SubstitutionCipher(ISBlock sBlock)
        {
            _sBlock = sBlock;
        }

        /// <summary>
        /// Substitution encode.
        /// </summary>
        /// <param name="data">Opened message multiple of 64 bits.</param>
        /// <param name="subKeys">Subkeys.</param>
        /// <returns>Encoded message multiple of 64 bits.</returns>
        public byte[] Encode(byte[] data, List<uint> subKeys)
        {
            var little = BitConverter.ToUInt32(data, 0);
            var big = BitConverter.ToUInt32(data, 4);

            for (int i = 0; i != 32; i++)
            {
                var round = big ^ Function(little, subKeys[i]);

                if (i < 31)
                {
                    big = little;
                    little = round;
                }
                else
                {
                    big = round;
                }
            }

            byte[] result = new byte[8];
            Array.Copy(BitConverter.GetBytes(little), 0, result, 0, 4);
            Array.Copy(BitConverter.GetBytes(big), 0, result, 4, 4);
            return result;
        }

        /// <summary>
        /// Substitution decode.
        /// </summary>
        /// <param name="data">Encoded message multiple of 64 bits.</param>
        /// <param name="subKeys">Subkeys.</param>
        /// <returns>Opened message multiple of 64 bits.</returns>
        public byte[] Decode(byte[] data, List<uint> subKeys) => Encode(data, subKeys);

        /// <summary>
        /// Main func.
        /// </summary>
        /// <param name="block">Little bits.</param>
        /// <param name="subKey">Subkeys.</param>
        /// <returns>Result.</returns>
        public uint Function(uint block, uint subKey)
        {
            block = (block + subKey) % 4294967295;
            block = Substitute(block);
            block = (block << 11) | (block >> 21);
            return block;
        }

        /// <summary>
        /// Substitution.
        /// </summary>
        /// <param name="value">Block for substitution.</param>
        /// <returns>Result.</returns>
        private uint Substitute(uint value)
        {
            uint res = 0;

            for (int i = 0; i != 8; i++)
            {
                byte index = (byte)(value >> (4 * i) & 0x0f);
                byte block = _sBlock.SBlockTable[i][index];
                res |= (uint)block << (4 * i);
            }

            return res;
        }
    }
}
