namespace Oliviann.Security.Cryptography
{
    #region Usings

    using System.Runtime.CompilerServices;

    #endregion Usings

    /// <summary>
    /// Represents a collection of commonly used extension methods for to make
    /// working with cryptography easier.
    /// </summary>
    internal static class Extensions
    {
        /// <summary>
        /// Performs the final hash of the data for MurmurHash2 hashing. Casts
        /// magic spells and performs chants on final hash.
        /// </summary>
        /// <param name="hash">The hash before the final fixes.</param>
        /// <returns>A hash with the final fixes completed.</returns>
        [MethodImpl(256)]
        internal static uint MixFinalMurmur2(this uint hash)
        {
            hash ^= hash >> 13;
            hash *= 0x5bd1e995;
            hash ^= hash >> 15;
            return hash;
        }
    }
}