using System;
using System.Collections.Generic;
using GOSTCore.Core.Ciphers;
using GOSTCore.Core.Ciphers.Cfb;
using GOSTCore.Core.Ciphers.Substitution;
using GOSTCore.Core.Ciphers.Xor;
using GOSTCore.Core.SBlocks.IBlock;
using GOSTCore.Gost.Types;

namespace GOSTCore.Gost.Factories
{
    internal static class CipherFactory
    {
        public static ICipher Cipher(byte[] iv, List<uint> subKeys, CipherTypes cipher, ISBlock sBlock)
        {
            return cipher switch
            {
                CipherTypes.Substitution => new SubstitutionCipher(sBlock),
                CipherTypes.Cfb => new CbfCipher(iv, sBlock),
                CipherTypes.Xor => new XorCipher(iv, subKeys, sBlock),
                _ => throw new ArgumentException(nameof(cipher)),
            };
        }
    }
}
