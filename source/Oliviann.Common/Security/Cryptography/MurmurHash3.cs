namespace Oliviann.Security.Cryptography
{
    #region Usings

    using System.Runtime.CompilerServices;
    using Oliviann.Numerics;

    #endregion Usings

    /// <summary>
    /// Represents a C# implementation of the MurmurHash3 originally written in
    /// C++. This version is about 50% slower than the MurmurHash2
    /// implementation but has less chances of collisions.
    /// </summary>
    /// <remarks>I was able to make this code as fast as it will probably go but
    /// MurmurHash3 was found to run slower on some CPUs and faster on other
    /// CPUs.</remarks>
    public class MurmurHash3 : ISeededHashAlgorithm
    {
        #region Methods

        /// <summary>
        /// Hashes the specified data.
        /// </summary>
        /// <param name="data">The byte array to be hashed.</param>
        /// <returns>
        /// The hash code value for the specified data.
        /// </returns>
        public uint Hash(byte[] data) => this.Hash(data, 0xc58f1a7b);

        /// <summary>
        /// Hashes the specified data using the specified hash
        /// <paramref name="seed"/> value.
        /// </summary>
        /// <param name="data">The byte array to be hashed.</param>
        /// <param name="seed">The value to seed the hashing algorithm.</param>
        /// <returns>
        /// The hash code value for the specified data.
        /// </returns>
        public uint Hash(byte[] data, uint seed)
        {
            ADP.CheckArgumentNull(data, nameof(data));
            const uint C1 = 0xcc9e2d51;
            const uint C2 = 0x1b873593;

            int length = data.Length;
            int remainder = length & 3;
            int blocks = length >> 2;
            uint h1 = 0;

            unsafe
            {
                fixed (byte* d = &data[0])
                {
                    var b = (uint*)d;
                    while (blocks-- > 0)
                    {
                        h1 = BitOperations.RotateLeft(h1 ^ (BitOperations.RotateLeft(*b++ * C1, 15) * C2), 13) * 5 + 0xe6546b64;
                    }

                    if (remainder > 0)
                    {
                        h1 ^= Tail(d + (length - remainder), remainder);
                    }
                }
            }

            h1 = MixFinal(h1 ^ (uint)length);
            return h1;
        }

        #endregion Methods

        #region Helper Methods

        /// <summary>
        /// Performs hashing for the remaining bytes at the end of the array.
        /// </summary>
        /// <param name="tail">The data to be hashed.</param>
        /// <param name="remainder">The amount of remaining bytes.</param>
        /// <returns>A hash for the trailing bytes of data.</returns>
        [MethodImpl(256)]
        private static unsafe uint Tail(byte* tail, int remainder)
        {
            // Create our keys and initialize to 0.
            uint k1 = 0;

            // Determine how many bytes we have left to work with based on length.
            switch (remainder)
            {
                case 3:
                    k1 ^= (uint)tail[2] << 16;
                    goto case 2;
                case 2:
                    k1 ^= (uint)tail[1] << 8;
                    goto case 1;
                case 1:
                    k1 ^= tail[0];
                    break;
            }

            return BitOperations.RotateLeft(k1 * 0xcc9e2d51, 15) * 0x1b873593;
        }

        /// <summary>
        /// Performs the final hash of the data for MurmurHash3 hashing. Casts
        /// magic spells and performs chants on final hash.
        /// </summary>
        /// <param name="hash">The hash before the final fixes.</param>
        /// <returns>A hash with the final fixes completed.</returns>
        [MethodImpl(256)]
        private static uint MixFinal(uint hash)
        {
            // Pipelining friendly algorithm.
            hash = (hash ^ (hash >> 16)) * 0x85ebca6b;
            hash = (hash ^ (hash >> 13)) * 0xc2b2ae35;
            return hash ^ (hash >> 16);
        }

        #endregion Helper Methods
    }
}