namespace Oliviann.Security.Cryptography
{
    #region Usings

    using System.Runtime.CompilerServices;

    #endregion Usings

    /// <summary>
    /// Represents a collection of commonly used helper methods for making
    /// generating hash codes easier.
    /// </summary>
    public static class HashCode
    {
        #region Fields

        /// <summary>
        /// The collection of prime numbers to use for generating the hash
        /// codes.
        /// </summary>
        private const uint Prime1 = 2654435761U;

        private const uint Prime2 = 2246822519U;
        private const uint Prime3 = 3266489917U;
        private const uint Prime4 = 668265263U;
        private const uint Prime5 = 374761393U;

        #endregion Fields

        #region Methods

        /// <summary>
        /// Diffuses the hash code returned by the specified value.
        /// </summary>
        /// <typeparam name="T1">The type of the value to add the the hash code.
        /// </typeparam>
        /// <param name="value1">The value to add to the hash code.</param>
        /// <param name="seed">The value to seed the hashing algorithm.</param>
        /// <returns>
        /// The hash code that represents the single value.
        /// </returns>
        public static int Combine<T1>(T1 value1, uint seed = 0)
        {
            // Provide a way of diffusing bits from something with a limited
            // input hash space. For example, many enums only have a few
            // possible hashes, only using the bottom few bits of the code. Some
            // collections are built on the assumption that hashes are spread
            // over a larger space, so diffusing the bits may help the
            // collection work more efficiently.
            var hc1 = (uint)(value1?.GetHashCode() ?? 0);

            uint hash = MixEmptyState(seed);
            hash += 4;

            hash = QueueRound(hash, hc1);
            hash = MixFinal(hash);
            return (int)hash;
        }

        /// <summary>
        /// Diffuses the hash code returned by the specified values.
        /// </summary>
        /// <typeparam name="T1">The type of the first value to add the the hash
        /// code.
        /// </typeparam>
        /// <typeparam name="T2">The type of the second value to add the the
        /// hash code.
        /// </typeparam>
        /// <param name="value1">The first value to add to the hash code.
        /// </param>
        /// <param name="value2">The second value to add to the hash code.
        /// </param>
        /// <param name="seed">The value to seed the hashing algorithm.</param>
        /// <returns>
        /// The hash code that represents the specified values.
        /// </returns>
        public static int Combine<T1, T2>(T1 value1, T2 value2, uint seed = 0)
        {
            var hc1 = (uint)(value1?.GetHashCode() ?? 0);
            var hc2 = (uint)(value2?.GetHashCode() ?? 0);

            uint hash = MixEmptyState(seed);
            hash += 8;

            hash = QueueRound(hash, hc1);
            hash = QueueRound(hash, hc2);

            hash = MixFinal(hash);
            return (int)hash;
        }

        /// <summary>
        /// Diffuses the hash code returned by the specified values.
        /// </summary>
        /// <typeparam name="T1">The type of the first value to add the the hash
        /// code.
        /// </typeparam>
        /// <typeparam name="T2">The type of the second value to add the the
        /// hash code.
        /// </typeparam>
        /// <typeparam name="T3">The type of the third value to add the the
        /// hash code.
        /// </typeparam>
        /// <param name="value1">The first value to add to the hash code.
        /// </param>
        /// <param name="value2">The second value to add to the hash code.
        /// </param>
        /// <param name="value3">The third value to add to the hash code.
        /// </param>
        /// <param name="seed">The value to seed the hashing algorithm.</param>
        /// <returns>
        /// The hash code that represents the specified values.
        /// </returns>
        public static int Combine<T1, T2, T3>(T1 value1, T2 value2, T3 value3, uint seed = 0)
        {
            var hc1 = (uint)(value1?.GetHashCode() ?? 0);
            var hc2 = (uint)(value2?.GetHashCode() ?? 0);
            var hc3 = (uint)(value3?.GetHashCode() ?? 0);

            uint hash = MixEmptyState(seed);
            hash += 12;

            hash = QueueRound(hash, hc1);
            hash = QueueRound(hash, hc2);
            hash = QueueRound(hash, hc3);

            hash = MixFinal(hash);
            return (int)hash;
        }

        /// <summary>
        /// Diffuses the hash code returned by the specified values.
        /// </summary>
        /// <typeparam name="T1">The type of the first value to add the the hash
        /// code.
        /// </typeparam>
        /// <typeparam name="T2">The type of the second value to add the the
        /// hash code.
        /// </typeparam>
        /// <typeparam name="T3">The type of the third value to add the the
        /// hash code.
        /// </typeparam>
        /// <typeparam name="T4">The type of the fourth value to add the the
        /// hash code.
        /// </typeparam>
        /// <param name="value1">The first value to add to the hash code.
        /// </param>
        /// <param name="value2">The second value to add to the hash code.
        /// </param>
        /// <param name="value3">The third value to add to the hash code.
        /// </param>
        /// <param name="value4">The fourth value to add to the hash code.
        /// </param>
        /// <param name="seed">The value to seed the hashing algorithm.</param>
        /// <returns>
        /// The hash code that represents the specified values.
        /// </returns>
        public static int Combine<T1, T2, T3, T4>(T1 value1, T2 value2, T3 value3, T4 value4, uint seed = 0)
        {
            var hc1 = (uint)(value1?.GetHashCode() ?? 0);
            var hc2 = (uint)(value2?.GetHashCode() ?? 0);
            var hc3 = (uint)(value3?.GetHashCode() ?? 0);
            var hc4 = (uint)(value4?.GetHashCode() ?? 0);

            Initialize(seed, out uint v1, out uint v2, out uint v3, out uint v4);

            v1 = Round(v1, hc1);
            v2 = Round(v2, hc2);
            v3 = Round(v3, hc3);
            v4 = Round(v4, hc4);

            uint hash = MixState(v1, v2, v3, v4);
            hash += 16;

            hash = MixFinal(hash);
            return (int)hash;
        }

        /// <summary>
        /// Diffuses the hash code returned by the specified values.
        /// </summary>
        /// <typeparam name="T1">The type of the first value to add the the hash
        /// code.
        /// </typeparam>
        /// <typeparam name="T2">The type of the second value to add the the
        /// hash code.
        /// </typeparam>
        /// <typeparam name="T3">The type of the third value to add the the
        /// hash code.
        /// </typeparam>
        /// <typeparam name="T4">The type of the fourth value to add the the
        /// hash code.
        /// </typeparam>
        /// <typeparam name="T5">The type of the fifth value to add the the
        /// hash code.
        /// </typeparam>
        /// <param name="value1">The first value to add to the hash code.
        /// </param>
        /// <param name="value2">The second value to add to the hash code.
        /// </param>
        /// <param name="value3">The third value to add to the hash code.
        /// </param>
        /// <param name="value4">The fourth value to add to the hash code.
        /// </param>
        /// <param name="value5">The fifth value to add to the hash code.
        /// </param>
        /// <param name="seed">The value to seed the hashing algorithm.</param>
        /// <returns>
        /// The hash code that represents the specified values.
        /// </returns>
        public static int Combine<T1, T2, T3, T4, T5>(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, uint seed = 0)
        {
            var hc1 = (uint)(value1?.GetHashCode() ?? 0);
            var hc2 = (uint)(value2?.GetHashCode() ?? 0);
            var hc3 = (uint)(value3?.GetHashCode() ?? 0);
            var hc4 = (uint)(value4?.GetHashCode() ?? 0);
            var hc5 = (uint)(value5?.GetHashCode() ?? 0);

            Initialize(seed, out uint v1, out uint v2, out uint v3, out uint v4);

            v1 = Round(v1, hc1);
            v2 = Round(v2, hc2);
            v3 = Round(v3, hc3);
            v4 = Round(v4, hc4);

            uint hash = MixState(v1, v2, v3, v4);
            hash += 20;

            hash = QueueRound(hash, hc5);

            hash = MixFinal(hash);
            return (int)hash;
        }

        /// <summary>
        /// Diffuses the hash code returned by the specified values.
        /// </summary>
        /// <typeparam name="T1">The type of the first value to add the the hash
        /// code.
        /// </typeparam>
        /// <typeparam name="T2">The type of the second value to add the the
        /// hash code.
        /// </typeparam>
        /// <typeparam name="T3">The type of the third value to add the the
        /// hash code.
        /// </typeparam>
        /// <typeparam name="T4">The type of the fourth value to add the the
        /// hash code.
        /// </typeparam>
        /// <typeparam name="T5">The type of the fifth value to add the the
        /// hash code.
        /// </typeparam>
        /// <typeparam name="T6">The type of the sixth value to add the the
        /// hash code.
        /// </typeparam>
        /// <param name="value1">The first value to add to the hash code.
        /// </param>
        /// <param name="value2">The second value to add to the hash code.
        /// </param>
        /// <param name="value3">The third value to add to the hash code.
        /// </param>
        /// <param name="value4">The fourth value to add to the hash code.
        /// </param>
        /// <param name="value5">The fifth value to add to the hash code.
        /// </param>
        /// <param name="value6">The sixth value to add to the hash code.
        /// </param>
        /// <param name="seed">The value to seed the hashing algorithm.</param>
        /// <returns>
        /// The hash code that represents the specified values.
        /// </returns>
        public static int Combine<T1, T2, T3, T4, T5, T6>(
            T1 value1,
            T2 value2,
            T3 value3,
            T4 value4,
            T5 value5,
            T6 value6,
            uint seed = 0)
        {
            var hc1 = (uint)(value1?.GetHashCode() ?? 0);
            var hc2 = (uint)(value2?.GetHashCode() ?? 0);
            var hc3 = (uint)(value3?.GetHashCode() ?? 0);
            var hc4 = (uint)(value4?.GetHashCode() ?? 0);
            var hc5 = (uint)(value5?.GetHashCode() ?? 0);
            var hc6 = (uint)(value6?.GetHashCode() ?? 0);

            Initialize(seed, out uint v1, out uint v2, out uint v3, out uint v4);

            v1 = Round(v1, hc1);
            v2 = Round(v2, hc2);
            v3 = Round(v3, hc3);
            v4 = Round(v4, hc4);

            uint hash = MixState(v1, v2, v3, v4);
            hash += 24;

            hash = QueueRound(hash, hc5);
            hash = QueueRound(hash, hc6);

            hash = MixFinal(hash);
            return (int)hash;
        }

        /// <summary>
        /// Diffuses the hash code returned by the specified values.
        /// </summary>
        /// <typeparam name="T1">The type of the first value to add the the hash
        /// code.
        /// </typeparam>
        /// <typeparam name="T2">The type of the second value to add the the
        /// hash code.
        /// </typeparam>
        /// <typeparam name="T3">The type of the third value to add the the
        /// hash code.
        /// </typeparam>
        /// <typeparam name="T4">The type of the fourth value to add the the
        /// hash code.
        /// </typeparam>
        /// <typeparam name="T5">The type of the fifth value to add the the
        /// hash code.
        /// </typeparam>
        /// <typeparam name="T6">The type of the sixth value to add the the
        /// hash code.
        /// </typeparam>
        /// <typeparam name="T7">The type of the seventh value to add the the
        /// hash code.
        /// </typeparam>
        /// <param name="value1">The first value to add to the hash code.
        /// </param>
        /// <param name="value2">The second value to add to the hash code.
        /// </param>
        /// <param name="value3">The third value to add to the hash code.
        /// </param>
        /// <param name="value4">The fourth value to add to the hash code.
        /// </param>
        /// <param name="value5">The fifth value to add to the hash code.
        /// </param>
        /// <param name="value6">The sixth value to add to the hash code.
        /// </param>
        /// <param name="value7">The seventh value to add to the hash code.
        /// </param>
        /// <param name="seed">The value to seed the hashing algorithm.</param>
        /// <returns>
        /// The hash code that represents the specified values.
        /// </returns>
        public static int Combine<T1, T2, T3, T4, T5, T6, T7>(
            T1 value1,
            T2 value2,
            T3 value3,
            T4 value4,
            T5 value5,
            T6 value6,
            T7 value7,
            uint seed = 0)
        {
            var hc1 = (uint)(value1?.GetHashCode() ?? 0);
            var hc2 = (uint)(value2?.GetHashCode() ?? 0);
            var hc3 = (uint)(value3?.GetHashCode() ?? 0);
            var hc4 = (uint)(value4?.GetHashCode() ?? 0);
            var hc5 = (uint)(value5?.GetHashCode() ?? 0);
            var hc6 = (uint)(value6?.GetHashCode() ?? 0);
            var hc7 = (uint)(value7?.GetHashCode() ?? 0);

            Initialize(seed, out uint v1, out uint v2, out uint v3, out uint v4);

            v1 = Round(v1, hc1);
            v2 = Round(v2, hc2);
            v3 = Round(v3, hc3);
            v4 = Round(v4, hc4);

            uint hash = MixState(v1, v2, v3, v4);
            hash += 28;

            hash = QueueRound(hash, hc5);
            hash = QueueRound(hash, hc6);
            hash = QueueRound(hash, hc7);

            hash = MixFinal(hash);
            return (int)hash;
        }

        /// <summary>
        /// Diffuses the hash code returned by the specified values.
        /// </summary>
        /// <typeparam name="T1">The type of the first value to add the the hash
        /// code.
        /// </typeparam>
        /// <typeparam name="T2">The type of the second value to add the the
        /// hash code.
        /// </typeparam>
        /// <typeparam name="T3">The type of the third value to add the the
        /// hash code.
        /// </typeparam>
        /// <typeparam name="T4">The type of the fourth value to add the the
        /// hash code.
        /// </typeparam>
        /// <typeparam name="T5">The type of the fifth value to add the the
        /// hash code.
        /// </typeparam>
        /// <typeparam name="T6">The type of the sixth value to add the the
        /// hash code.
        /// </typeparam>
        /// <typeparam name="T7">The type of the seventh value to add the the
        /// hash code.
        /// </typeparam>
        /// <typeparam name="T8">The type of the eighth value to add the the
        /// hash code.
        /// </typeparam>
        /// <param name="value1">The first value to add to the hash code.
        /// </param>
        /// <param name="value2">The second value to add to the hash code.
        /// </param>
        /// <param name="value3">The third value to add to the hash code.
        /// </param>
        /// <param name="value4">The fourth value to add to the hash code.
        /// </param>
        /// <param name="value5">The fifth value to add to the hash code.
        /// </param>
        /// <param name="value6">The sixth value to add to the hash code.
        /// </param>
        /// <param name="value7">The seventh value to add to the hash code.
        /// </param>
        /// <param name="value8">The eighth value to add to the hash code.
        /// </param>
        /// <param name="seed">The value to seed the hashing algorithm.</param>
        /// <returns>
        /// The hash code that represents the specified values.
        /// </returns>
        public static int Combine<T1, T2, T3, T4, T5, T6, T7, T8>(
            T1 value1,
            T2 value2,
            T3 value3,
            T4 value4,
            T5 value5,
            T6 value6,
            T7 value7,
            T8 value8,
            uint seed = 0)
        {
            var hc1 = (uint)(value1?.GetHashCode() ?? 0);
            var hc2 = (uint)(value2?.GetHashCode() ?? 0);
            var hc3 = (uint)(value3?.GetHashCode() ?? 0);
            var hc4 = (uint)(value4?.GetHashCode() ?? 0);
            var hc5 = (uint)(value5?.GetHashCode() ?? 0);
            var hc6 = (uint)(value6?.GetHashCode() ?? 0);
            var hc7 = (uint)(value7?.GetHashCode() ?? 0);
            var hc8 = (uint)(value8?.GetHashCode() ?? 0);

            Initialize(seed, out uint v1, out uint v2, out uint v3, out uint v4);

            v1 = Round(v1, hc1);
            v2 = Round(v2, hc2);
            v3 = Round(v3, hc3);
            v4 = Round(v4, hc4);

            v1 = Round(v1, hc5);
            v2 = Round(v2, hc6);
            v3 = Round(v3, hc7);
            v4 = Round(v4, hc8);

            uint hash = MixState(v1, v2, v3, v4);
            hash += 32;

            hash = MixFinal(hash);
            return (int)hash;
        }

        #endregion Methods

        #region Helper Methods

#if !NET35 && !NET40
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif

        private static void Initialize(uint seed, out uint v1, out uint v2, out uint v3, out uint v4)
        {
            v1 = seed + Prime1 + Prime2;
            v2 = seed + Prime2;
            v3 = seed;
            v4 = seed - Prime1;
        }

#if !NET35 && !NET40
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif

        private static uint Round(uint hash, uint input)
        {
            hash += input * Prime2;
            hash = hash.RotateLeft(13);
            hash *= Prime1;
            return hash;
        }

#if !NET35 && !NET40
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif

        private static uint QueueRound(uint hash, uint queuedValue)
        {
            hash += queuedValue * Prime3;
            return hash.RotateLeft(17) * Prime4;
        }

#if !NET35 && !NET40
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif

        private static uint MixState(uint v1, uint v2, uint v3, uint v4)
        {
            return v1.RotateLeft(1) + v2.RotateLeft(7) + v3.RotateLeft(12) + v4.RotateLeft(18);
        }

        private static uint MixEmptyState(uint seed) => seed + Prime5;

        /// <summary>
        /// Performs the final hash of the data. Casts magic spells and performs
        /// chants on final hash.
        /// </summary>
        /// <param name="hash">The hash before the final fixes.</param>
        /// <returns>A hash with the final fixes completed.</returns>
#if !NET35 && !NET40
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif

        private static uint MixFinal(uint hash)
        {
            hash ^= hash >> 15;
            hash *= Prime2;
            hash ^= hash >> 13;
            hash *= Prime3;
            hash ^= hash >> 16;
            return hash;
        }

        #endregion Helper Methods
    }
}