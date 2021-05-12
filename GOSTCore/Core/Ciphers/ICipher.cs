using System.Collections.Generic;

namespace GOSTCore.Core.Ciphers
{
    internal interface ICipher
    {
        public byte[] Encode(byte[] data, List<uint> subKeys);

        public byte[] Decode(byte[] data, List<uint> subKeys);
    }
}