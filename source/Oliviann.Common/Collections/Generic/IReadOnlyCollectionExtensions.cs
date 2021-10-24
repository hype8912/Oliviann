namespace Oliviann.Collections.Generic
{
    #region Usings

    using System.Collections.Generic;
    using Oliviann.Collections.Generic;

    #endregion Usings

    /// <summary>
    /// Represents a collection of commonly used extension methods for extending
    /// <see cref="IReadOnlyCollection{T}"/>s.
    /// </summary>
    public static class IReadOnlyCollectionExtensions
    {
        /// <summary>
        /// Determines whether the specified collection is null or empty.
        /// </summary>
        /// <typeparam name="T">The type pf collection.</typeparam>
        /// <param name="collection">The collection to be checked.</param>
        /// <returns>
        /// True if the specified collection is null or empty; otherwise, false.
        /// </returns>
        public static bool IsNullOrEmpty<T>(this IReadOnlyCollection<T> collection) => collection == null || collection.Count < 1;
    }
}