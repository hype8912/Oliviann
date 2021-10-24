namespace Oliviann.Collections
{
    #region Usings

    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Linq;
    using Oliviann.Collections.Generic;

    #endregion Usings

    /// <summary>
    /// Represents a collection of commonly used extension methods for extending
    /// collections.
    /// </summary>
    public static class CollectionExtensions
    {
        /// <summary>
        /// Adds the elements of the specified parameters to the this
        /// collection.
        /// </summary>
        /// <typeparam name="T">The type of items in
        /// <paramref name="collection"/>.</typeparam>
        /// <param name="collection">The collection for the specified items to
        /// be added to the end of.</param>
        /// <param name="values">The values that should be added to the end of
        /// <paramref name="collection"/>.</param>
        public static void AddRange<T>(this ICollection<T> collection, params T[] values)
        {
            ADP.CheckArgumentNull(collection, nameof(collection));
            if (values != null && values.Length > 0)
            {
                values.Execute(collection.Add);
            }
        }

        /// <summary>
        /// Adds the elements of the specified parameters to the this
        /// collection.
        /// </summary>
        /// <typeparam name="T">The type of items in
        /// <paramref name="collection"/>.</typeparam>
        /// <param name="collection">The collection for the specified items to
        /// be added to the end of.</param>
        /// <param name="items">The items that should be added to the end of
        /// <paramref name="collection"/>.</param>
        /// <remarks>This method does not do any type of filtering for duplicate
        /// entries.</remarks>
        public static void AddRange<T>(this ICollection<T> collection, IEnumerable<T> items) => collection.AddRange(items, false);

        /// <summary>
        /// Adds the elements of the specified parameters to the this collection
        /// with the option to ignore duplicates.
        /// </summary>
        /// <typeparam name="T">The type of items in
        /// <paramref name="collection"/>.</typeparam>
        /// <param name="collection">The collection for the specified items to
        /// be added to the end of.</param>
        /// <param name="items">The items that should be added to the end of
        /// <paramref name="collection"/>.</param>
        /// <param name="ignoreDuplicates">If true, does not add entries from
        /// the items collection to the source collection; otherwise, duplicate
        /// entries will be added.</param>
        /// <remarks>If the source collection already has duplicate entries
        /// before being passed, those entries will not be removed.</remarks>
        public static void AddRange<T>(this ICollection<T> collection, IEnumerable<T> items, bool ignoreDuplicates)
        {
            ADP.CheckArgumentNull(collection, nameof(collection));
            if (items.IsNullOrEmpty())
            {
                return;
            }

            Action<T> act = item =>
                {
                    bool addItem = true;
                    if (ignoreDuplicates)
                    {
                        addItem = !collection.Contains(item);
                    }

                    if (addItem)
                    {
                        collection.Add(item);
                    }
                };

            items.Execute(act);
        }

        /// <summary>
        /// Copies all items of the current <paramref name="sourceCollection"/>
        /// to the specified <paramref name="targetCollection"/>.
        /// </summary>
        /// <typeparam name="T">The type of items in
        /// <paramref name="sourceCollection"/>.</typeparam>
        /// <param name="sourceCollection">The source collection to copy from.
        /// </param>
        /// <param name="targetCollection">The target collection to copy to.
        /// </param>
        /// <remarks>
        /// The <paramref name="targetCollection"/> will be cleared before any
        /// items from the <paramref name="sourceCollection"/> are copied to it.
        /// </remarks>
        public static void CopyTo<T>(this ICollection<T> sourceCollection, ICollection<T> targetCollection)
        {
            ADP.CheckArgumentNull(targetCollection, nameof(targetCollection));

            targetCollection.Clear();
            targetCollection.AddRange(sourceCollection);
        }

        /// <summary>
        /// Creates a new <see cref="KeyValuePair{TKey, TValue}"/> object with a
        /// new value from another
        /// <see cref="KeyValuePair{TKey, TValue}"/> object.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="originalPair">The original
        /// <see cref="KeyValuePair{TKey, TValue}"/>.</param>
        /// <param name="newValue">The new value.</param>
        /// <returns>A newly created
        /// <see cref="KeyValuePair{TKey, TValue}"/> with the new value and the
        /// same key value.</returns>
        public static KeyValuePair<TKey, TValue> New<TKey, TValue>(this KeyValuePair<TKey, TValue> originalPair, TValue newValue)
        {
            return new KeyValuePair<TKey, TValue>(originalPair.Key, newValue);
        }

        /// <summary>
        /// Removes all items from the collection where a match is specified by
        /// the delegate function.
        /// </summary>
        /// <typeparam name="T">The type of items in
        /// <paramref name="collection"/>.</typeparam>
        /// <param name="collection">The collection for the specified items to
        /// be removed from.</param>
        /// <param name="match">The delegate function for matching items to be
        /// removed.</param>
        /// <returns>The number of items affected by the matching delegate
        /// function.</returns>
        public static int RemoveAll<T>(this ICollection<T> collection, Func<T, bool> match)
        {
            ADP.CheckArgumentNull(collection, nameof(collection));
            if (match == null)
            {
                return 0;
            }

            IList<T> toRemove = collection.Where(match).ToList();
            toRemove.Execute(r => collection.Remove(r));
            return toRemove.Count;
        }

        /// <summary>
        /// Attempts to add the specified value to the collection. If the value
        /// already exists then the current value is not over written.
        /// </summary>
        /// <typeparam name="T">The type of items in
        /// <paramref name="collection"/>.</typeparam>
        /// <param name="collection">The collection to add an item to.</param>
        /// <param name="value">The value to be added.</param>
        /// <returns>True if the specified value was added to the collection;
        /// otherwise, false.</returns>
        public static bool TryAdd<T>(this ICollection<T> collection, T value)
        {
            if (collection == null || collection.Contains(value))
            {
                return false;
            }

            collection.Add(value);
            return true;
        }

        #region Converts

        /// <summary>
        /// Copies the elements of the <see cref="StringCollection"/> to a new
        /// array.
        /// </summary>
        /// <param name="collection">A string collection to be copied from.
        /// </param>
        /// <returns>An array containing copies of the elements of the
        /// <see cref="StringCollection"/>.</returns>
        /// <remarks>
        /// The elements are copied using
        /// <see cref="Array.CopyTo(Array, int)"/>, which is an O(n)
        /// operation, where n is <see cref="StringCollection.Count"/>.
        /// </remarks>
        public static string[] ToArray(this StringCollection collection)
        {
            ADP.CheckArgumentNull(collection, nameof(collection));
            var destinationArray = new string[collection.Count];
            collection.CopyTo(destinationArray, 0);
            return destinationArray;
        }

        #endregion Converts
    }
}