namespace Oliviann.Security.Cryptography
{
    #region Usings

    using System.Runtime.CompilerServices;

    #endregion Usings

    /// <summary>
    /// Represents a C# implementation of the MurmurHash3 originally written in
    /// C++. This version is about 50% slower than the MurmurHash2
    /// implementation but has less chances of collisions.
    /// </summary>
    /// <remarks>I was able to make this code as fast as it will probably go but
    /// MurmurHash3 was found to run slower on some CPUs and faster on some
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
        public uint Hash(byte[] data)
        {
            return this.Hash(data, 0xc58f1a7b);
        }

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
                        h1 = (h1 ^ ((*b++ * C1).RotateLeft(15) * C2)).RotateLeft(13) * 5 + 0xe6546b64;
                    }

                    if (remainder > 0)
                    {
                        h1 ^= Tail(d + (length - remainder), remainder);
                    }
                }
            }

            h1 = (h1 ^ (uint)length).MixFinalMurmur3();
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
#if !NET40 && !NET35
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif

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

            return (k1 * 0xcc9e2d51).RotateLeft(15) * 0x1b873593;
        }

        #endregion Helper Methods
    }
}