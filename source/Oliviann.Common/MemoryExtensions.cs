#if NET45 || NET46 || NET47 || NET48 || NETSTANDARD1_3 || NETSTANDARD2_0

namespace Oliviann
{
    #region Usings

    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    #endregion

    /// <summary>
    /// Represents a collection of commonly used extension methods for extending
    /// memory and span related types.
    /// </summary>
    public static class MemoryExtensions
    {
        /// <summary>
        /// Reverses the sequence of the elements in the entire span.
        /// </summary>
        /// <typeparam name="T">The type of elements in the span.</typeparam>
        /// <param name="span">The span to reverse.</param>
        /// <remarks>Method was added to .NET Standard in version 2.1.</remarks>
        public static void Reverse<T>(this Span<T> span)
        {
            if (span.Length < 2)
            {
                return;
            }

            ref T spanReference = ref MemoryMarshal.GetReference(span);
            int lowerIndex = 0;
            int upperIndex = span.Length - 1;
            while (lowerIndex < upperIndex)
            {
                T temp = Unsafe.Add(ref spanReference, lowerIndex);
                Unsafe.Add(ref spanReference, lowerIndex) = Unsafe.Add(ref spanReference, upperIndex);
                Unsafe.Add(ref spanReference, upperIndex) = temp;
                lowerIndex += 1;
                upperIndex -= 1;
            }
        }
    }
}

#endif