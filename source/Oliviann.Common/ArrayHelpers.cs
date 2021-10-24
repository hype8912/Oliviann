#if NET35 || NET40 || NET45

namespace Oliviann
{
    #region Usings

    using System;

    #endregion

    /// <summary>
    /// Represents a collection of commonly used helper methods for making
    /// working with <see cref="Array"/>s easier.
    /// </summary>
    /// <remarks>This was added in .NET Framework 4.6.</remarks>
    public static class ArrayHelpers
    {
        #region Methods

        /// <summary>
        /// Returns an empty array.
        /// </summary>
        /// <typeparam name="T">The type of the elements of the array.
        /// </typeparam>
        /// <returns>An empty array.</returns>
        public static T[] Empty<T>() => EmptyArray<T>.Value;

        #endregion

        #region Classes

        private static class EmptyArray<T>
        {
#pragma warning disable CA1825 // this is the implementation of Array.Empty<T>()
            internal static readonly T[] Value = new T[0];
#pragma warning restore CA1825
        }

        #endregion
    }
}

#endif