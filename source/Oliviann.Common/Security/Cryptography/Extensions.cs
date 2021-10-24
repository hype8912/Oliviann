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
        /// Rotates the specified value to the left by performing the <c>hokie
        /// pokie</c>.
        /// </summary>
        /// <param name="originalData">The original data to be rotated.</param>
        /// <param name="count">The amount of bits to rotate.</param>
        /// <returns>A rotated hash value.</returns>
#if !NET35 && !NET40
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif

        internal static uint RotateLeft(this uint originalData, int count) => (originalData << count) | (originalData >> (32 - count));

        /// <summary>
        /// Performs the final hash of the data for MurmurHash2 hashing. Casts
        /// magic spells and performs chants on final hash.
        /// </summary>
        /// <param name="hash">The hash before the final fixes.</param>
        /// <returns>A hash with the final fixes completed.</returns>
#if !NET35 && !NET40
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif

        internal static uint MixFinalMurmur2(this uint hash)
        {
            hash ^= hash >> 13;
            hash *= 0x5bd1e995;
            hash ^= hash >> 15;
            return hash;
        }

        /// <summary>
        /// Performs the final hash of the data for MurmurHash3 hashing. Casts
        /// magic spells and performs chants on final hash.
        /// </summary>
        /// <param name="hash">The hash before the final fixes.</param>
        /// <returns>A hash with the final fixes completed.</returns>
#if !NET35 && !NET40
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif

        internal static uint MixFinalMurmur3(this uint hash)
        {
            // Pipelining friendly algorithm.
            hash = (hash ^ (hash >> 16)) * 0x85ebca6b;
            hash = (hash ^ (hash >> 13)) * 0xc2b2ae35;
            return hash ^ (hash >> 16);
        }
    }
}