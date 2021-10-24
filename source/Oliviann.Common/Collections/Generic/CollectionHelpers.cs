namespace Oliviann.Collections.Generic
{
    #region Usings

    using System.Collections.Generic;
    using System.Linq;

    #endregion

    /// <summary>
    /// Represents a collection of helper methods for working with collections.
    /// </summary>
    internal static class CollectionHelpers
    {
        /// <summary>
        /// Provides the correct implementation of a new IReadOnlyCollection
        /// instance.
        /// </summary>
        /// <typeparam name="T">The type of elements.</typeparam>
        /// <returns>A new implementation of IReadOnlyCollection.</returns>
        public static IReadOnlyCollection<T> CreateReadOnlyCollection<T>()
        {
#if NET35 || NET40
            return new ReadOnlyListWrapper<T>();
#else
            return new List<T>();
#endif
        }

        /// <summary>
        /// Provides the correct implementation of a new IReadOnlyCollection
        /// instance for ToList.
        /// </summary>
        /// <typeparam name="T">The type of elements.</typeparam>
        /// <param name="collection">The enumerable collection.</param>
        /// <returns>A new implementation of IReadOnlyCollection for the
        /// specified collection.</returns>
        public static IReadOnlyCollection<T> ToListHelper<T>(this IEnumerable<T> collection)
        {
#if NET35 || NET40
            return new ReadOnlyListWrapper<T>(collection);
#else
            return collection.ToList();
#endif
        }
    }
}