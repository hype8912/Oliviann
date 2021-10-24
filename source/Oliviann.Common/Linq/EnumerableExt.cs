namespace Oliviann.Linq
{
    #region Usings

    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using Oliviann.Linq.Expressions;

    #endregion Usings

    /// <summary>
    /// Represents a collection of extension methods for extending the built-in
    /// capabilities of Linq.
    /// </summary>
    public static class EnumerableExt
    {
        /// <summary>
        /// Returns the number of elements in a sequence.
        /// </summary>
        /// <param name="source">A sequence that contains elements to be
        /// counted.</param>
        /// <returns>The number of elements in the input sequence.</returns>
        public static int Count(this IEnumerable source)
        {
            ADP.CheckArgumentNull(source, nameof(source));

            if (source is ICollection collect)
            {
                return collect.Count;
            }

            checked
            {
                int num = 0;
                IEnumerator enumerator = source.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    num += 1;
                }

                return num;
            }
        }

        /// <summary>
        /// Returns all distinct elements of the given source, where
        /// "distinctness" is determined via a projection and the default
        /// equality comparer for the projected type.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of
        /// <paramref name="source"/>.</typeparam>
        /// <typeparam name="TKey">The type of the projected element.
        /// </typeparam>
        /// <param name="source">The sequence to remove duplicate elements from.
        /// </param>
        /// <param name="selector">Projection for determining "distinctness".
        /// </param>
        /// <returns>
        /// A sequence consisting of distinct elements from the source sequence,
        /// comparing them by the specified key projection.
        /// </returns>
        /// <remarks>
        /// Performance is better using a <c>foreach</c> loop with a generic
        /// <see cref="HashSet{T}"/> versus using a linq query to perform the
        /// action.
        /// </remarks>
        public static IEnumerable<TSource> Distinct<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector)
        {
            ADP.CheckArgumentNull(source, nameof(source));
            ADP.CheckArgumentNull(selector, nameof(selector));

            // ReSharper disable LoopCanBeConvertedToQuery
            var knownKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (knownKeys.Add(selector(element)))
                {
                    yield return element;
                }
            }

            // ReSharper restore LoopCanBeConvertedToQuery
        }

        /// <summary>
        /// Returns all distinct keys of the given source, where
        /// "distinctness" is determined via a projection and the default
        /// equality comparer for the projected type.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of
        /// <paramref name="source"/>.</typeparam>
        /// <typeparam name="TKey">The type of the projected element.
        /// </typeparam>
        /// <param name="source">The sequence to remove duplicate elements from.
        /// </param>
        /// <param name="selector">Projection for determining "distinctness".
        /// </param>
        /// <returns>
        /// A sequence consisting of distinct keys from the source sequence,
        /// comparing them by the specified key projection.
        /// </returns>
        /// <remarks>
        /// Performance is better using a <c>foreach</c> loop with a generic
        /// <see cref="HashSet{T}"/> versus using a linq query to perform the
        /// action.
        /// </remarks>
        public static IEnumerable<TKey> DistinctKey<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector)
        {
            ADP.CheckArgumentNull(source, nameof(source));
            ADP.CheckArgumentNull(selector, nameof(selector));

            // ReSharper disable LoopCanBeConvertedToQuery
            var knownKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                TKey selectorResult = selector(element);
                if (knownKeys.Add(selectorResult))
                {
                    yield return selectorResult;
                }
            }

            // ReSharper restore LoopCanBeConvertedToQuery
        }

        /// <summary>
        /// Returns all the duplicate elements for a given source.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of
        /// <paramref name="source"/>.</typeparam>
        /// <param name="source">The sequence to find duplicate elements from.
        /// </param>
        /// <returns>A sequence consisting of duplicate keys from the source
        /// sequence.</returns>
        public static IEnumerable<TSource> Duplicates<TSource>(this IEnumerable<TSource> source)
        {
            ADP.CheckArgumentNull(source, nameof(source));
            return source.GroupBy(x => x).Where(group => group.Count() > 1).Select(group => group.Key).ToList();
        }

        /// <summary>
        /// Produces the set difference of two sequences by using the specified
        /// comparison function.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of the input
        /// sequences.</typeparam>
        /// <param name="first">An <see cref="IEnumerable{T}"/> whose elements
        /// that are not also in second will be returned.</param>
        /// <param name="second">An <see cref="IEnumerable{T}"/> whose elements
        /// that also occur in the first sequence will cause those elements to
        /// be removed from the returned sequence.</param>
        /// <param name="comparer">A function to test each element for a
        /// condition.</param>
        /// <returns>A sequence that contains the set difference of the elements
        /// of two sequences.</returns>
        public static IEnumerable<TSource> Except<TSource>(
                                                           this IEnumerable<TSource> first,
                                                           IEnumerable<TSource> second,
                                                           Func<TSource, TSource, bool> comparer)
        {
            return first.Except(second, new LambdaComparer<TSource>(comparer));
        }

        /// <summary>
        /// Filters the elements of an <see cref="IEnumerable"/> based on a
        /// specified type. If the source is <c>null</c>, then an empty
        /// Enumerable is return with type <typeparamref name="TResult"/>.
        /// </summary>
        /// <typeparam name="TResult">The type to filter the elements of the
        /// sequence on.</typeparam>
        /// <param name="source">The <see cref="IEnumerable"/> whose elements to
        /// filter.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> that contains elements from
        /// the input sequence of type <typeparamref name="TResult"/>.</returns>
        public static IEnumerable<TResult> OfTypeSafe<TResult>(this IEnumerable source)
        {
            return source == null ? Enumerable.Empty<TResult>() : source.OfType<TResult>();
        }

        /// <summary>
        /// Sorts the elements of a sequence in the specified order according to
        /// a key.
        /// </summary>
        /// <typeparam name="T">The type of the elements of
        /// <paramref name="source" />.</typeparam>
        /// <typeparam name="TKey">The type of the key returned by
        /// <paramref name="selector" />.</typeparam>
        /// <param name="source">TA sequence of values to order.</param>
        /// <param name="selector">A function to extract a key from an element.
        /// </param>
        /// <param name="ascending">True for ascending order; otherwise, false
        /// for descending order.</param>
        /// <returns>A collection whose elements are sorted according to a key.
        /// </returns>
        public static IEnumerable<T> OrderBy<T, TKey>(this IEnumerable<T> source, Func<T, TKey> selector, bool ascending)
        {
            ADP.CheckArgumentNull(source, nameof(source));
            return ascending ? source.OrderBy(selector) : source.OrderByDescending(selector);
        }

        /// <summary>
        /// Converts the specified enumerable to a read-only collection.
        /// </summary>
        /// <typeparam name="T">The type of items in <paramref name="source"/>.
        /// </typeparam>
        /// <param name="source">The collection to be converted.</param>
        /// <returns>A read-only collection that represents the current
        /// <paramref name="source"/> collection.</returns>
        public static IList<T> ToReadOnlyCollection<T>(this IEnumerable<T> source) => new ReadOnlyCollection<T>(source.ToList());

        /// <summary>
        /// Produces the set union of two sequences by using a specified
        /// comparison function.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of the input
        /// sequences.</typeparam>
        /// <param name="first">An <see cref="IEnumerable{T}"/> whose distinct
        /// elements form the first set for the union.</param>
        /// <param name="second">An <see cref="IEnumerable{T}"/> whose distinct
        /// elements form the second set for the union.</param>
        /// <param name="comparer">A function to test each element for a
        /// condition.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> that contains the elements
        /// from both input sequences, excluding duplicates.</returns>
        public static IEnumerable<TSource> Union<TSource>(
                                                          this IEnumerable<TSource> first,
                                                          IEnumerable<TSource> second,
                                                          Func<TSource, TSource, bool> comparer)
        {
            return first.Union(second, new LambdaComparer<TSource>(comparer));
        }
    }
}