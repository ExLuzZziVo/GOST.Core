using System;
using System.Collections.Generic;
using System.Text;
using GOSTCore.Core.Ciphers.Substitution;
using GOSTCore.Core.SBlocks.IBlock;

namespace GOSTCore.Core.Generators.Mac
{
    internal class MacGenerator : IMacGenerator
    {
        private uint n1;
        private uint n2;

        private byte[] round;

        private readonly SubstitutionCipher substitution;

        public MacGenerator(ISBlock sBlock)
        {
            substitution = new SubstitutionCipher(sBlock);
        }

        /// <summary>
        /// MAC generator.
        /// </summary>
        /// <param name="data">Message.</param>
        /// <param name="subKeys">Subkeys.</param>
        /// <returns>MAC.</returns>
        public byte[] Generate(byte[] data, List<uint> subKeys)
        {
            if (data.Length != 8)
            {
                byte[] temp = new byte[8];
                Array.Copy(data, 0, temp, 0, data.Length);
                for (int i = data.Length - 1; i != 8; i++)
                {
                    temp[i] = 0;
                }
                data = temp;
            }

            if (round is null)
            {
                n1 = BitConverter.ToUInt32(data, 0);
                n2 = BitConverter.ToUInt32(data, 4);

                round = ShortSubstitute(n1, n2, subKeys);
            }
            else if (round != null)
            {
                for (int i = 0; i != 8; i++)
                {
                    round[i] = (byte)(round[i] ^ data[i]);
                }

                n1 = BitConverter.ToUInt32(round, 0);
                n2 = BitConverter.ToUInt32(round, 4);

                round = ShortSubstitute(n1, n2, subKeys);
            }

            return round;
        }

        /// <summary>
        /// 16-round version of substitution cipher.
        /// </summary>
        /// <param name="little">Little bits.</param>
        /// <param name="big">Big bits.</param>
        /// <param name="subKeys">Subkeys.</param>
        /// <returns>Result.</returns>
        private byte[] ShortSubstitute(uint little, uint big, List<uint> subKeys)
        {
            for (int i = 0; i != 16; i++)
            {
                var round = big ^ substitution.Function(little, subKeys[i]);

                big = little;
                little = round;
            }

            byte[] result = new byte[8];
            Array.Copy(BitConverter.GetBytes(little), 0, result, 0, 4);
            Array.Copy(BitConverter.GetBytes(big), 0, result, 4, 4);
            return result;
        }
    }
}
