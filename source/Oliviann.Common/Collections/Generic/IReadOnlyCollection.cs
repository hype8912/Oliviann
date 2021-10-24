#if NET35 || NET40

namespace Oliviann.Collections.Generic
{
    #region Usings

    using System;
    using System.Collections;
    using System.Collections.Generic;

    #endregion

    /// <summary>
    /// Represents a strongly-typed, read-only collection of elements.
    /// </summary>
    /// <typeparam name="T">The type of the elements.</typeparam>
#if NET35

    public interface IReadOnlyCollection<T> : IEnumerable<T>, IEnumerable
#else

    public interface IReadOnlyCollection<out T> : IEnumerable<T>, IEnumerable
#endif
    {
        /// <summary>Gets the number of elements in the collection.</summary>
        /// <returns>The number of elements in the collection. </returns>
        int Count { get; }
    }
}

#endif