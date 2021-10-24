#if NET35 || NET40

namespace Oliviann.Collections.Generic
{
    /// <summary>
    /// Represents a read-only collection of elements that can be accessed by
    /// index.
    /// </summary>
#if NET35

    public interface IReadOnlyList<T> : IReadOnlyCollection<T>
#else

    public interface IReadOnlyList<out T> : IReadOnlyCollection<T>
#endif
    {
        /// <summary>
        /// Gets the element at the specified index in the read-only list.
        /// </summary>
        /// <param name="index">The zero-based index of the element to get.
        /// </param>
        /// <returns>The element at the specified index in the read-only list.
        /// </returns>
        T this[int index] { get; }
    }
}

#endif