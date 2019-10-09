using System;
using GOSTCore.Core.SBlocks;
using GOSTCore.Core.SBlocks.IBlock;
using GOSTCore.Gost.Types;

namespace GOSTCore.Gost.Factories
{
    internal static class SBlockFactory
    {
        public static ISBlock SBlock(SBlockTypes sBlock)
        {
            return sBlock switch
            {
                SBlockTypes.CryptoProA => new CryptoProABlock(),
                SBlockTypes.CryptoProB => new CryptoProBBlock(),
                SBlockTypes.CryptoProC => new CryptoProCBlock(),
                SBlockTypes.CryptoProD => new CryptoProDBlock(),
                SBlockTypes.TC26 => new Tc26Block(),
                SBlockTypes.GOST => new GostBlock(),
                _ => throw new ArgumentException(nameof(sBlock)),
            };
        }
    }
}
