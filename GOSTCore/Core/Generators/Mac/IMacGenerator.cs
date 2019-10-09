using System.Collections.Generic;

namespace GOSTCore.Core.Generators.Mac
{
    internal interface IMacGenerator
    {
        /// <summary>
        /// MAC generator.
        /// </summary>
        /// <param name="data">Message.</param>
        /// <param name="subKeys">Subkeys.</param>
        /// <returns>MAC.</returns>
        public byte[] Generate(byte[] data, List<uint> subKeys);
    }
}
