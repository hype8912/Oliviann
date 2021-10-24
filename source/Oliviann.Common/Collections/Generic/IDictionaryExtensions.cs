namespace Oliviann.Collections.Generic
{
    #region Usings

    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Diagnostics;

    #endregion Usings

    /// <summary>
    /// Represents a collection of commonly used extension methods for extending
    /// dictionaries.
    /// </summary>
    public static class IDictionaryExtensions
    {
        /// <summary>
        /// Adds the specified <paramref name="key"/> and
        /// <paramref name="value"/> to the dictionary if it doesn't already
        /// exist in the specified <paramref name="collection"/>. If the
        /// specified <paramref name="key"/> already exists in the
        /// <paramref name="collection"/> then the current value associated with
        /// the <paramref name="key"/> will be updated with specified
        /// <paramref name="value"/>.
        /// </summary>
        /// <typeparam name="TKey">The type of the keys in the dictionary.
        /// </typeparam>
        /// <typeparam name="TValue">The type of the values in the dictionary.
        /// </typeparam>
        /// <param name="collection">The generic collection of key/value pairs.
        /// </param>
        /// <param name="key">The key of the element to add.</param>
        /// <param name="value">The value of the element to add. The value can
        /// be <c>null</c> for reference types.</param>
        public static void AddOrUpdate<TKey, TValue>(this IDictionary<TKey, TValue> collection, TKey key, TValue value)
            where TKey : class
        {
            if (collection == null || key == null)
            {
                Trace.TraceWarning("Oliviann.Collections.Generic.AddOrUpdate: The dictionary collection and/or key cannot be null.");
                return;
            }

            if (collection.ContainsKey(key))
            {
                collection[key] = value;
            }
            else
            {
                collection.Add(key, value);
            }
        }

        /// <summary>
        /// Attempts to add the specified key and value to the dictionary. If
        /// the key already exists then the current key will not be overwritten.
        /// </summary>
        /// <typeparam name="TKey">The type of the keys in the dictionary.
        /// </typeparam>
        /// <typeparam name="TValue">The type of the values in the dictionary.
        /// </typeparam>
        /// <param name="collection">The generic collection of key/value pairs.
        /// </param>
        /// <param name="key">The key of the element to add.</param>
        /// <param name="value">The value of the element to add. It can be null.
        /// </param>
        /// <returns>True if the key/value pair was added to the dictionary
        /// successfully; otherwise, false.</returns>
        public static bool TryAdd<TKey, TValue>(this IDictionary<TKey, TValue> collection, TKey key, TValue value)
        {
            if (collection == null || key == null || collection.ContainsKey(key))
            {
                return false;
            }

            collection.Add(key, value);
            return true;
        }

        /// <summary>
        /// Gets the value associated with the specified <paramref name="key"/>
        /// or the specified <paramref name="defaultValue"/>.
        /// </summary>
        /// <typeparam name="TKey">The type of the keys in the dictionary.
        /// </typeparam>
        /// <typeparam name="TValue">The type of the values in the dictionary.
        /// </typeparam>
        /// <param name="collection">The generic collection of key/value pairs.
        /// </param>
        /// <param name="key">The key of the value to get.</param>
        /// <param name="defaultValue">The value to be returned if the
        /// <paramref name="key"/> is not found in the collection.</param>
        /// <returns>
        /// When this method returns containing the value associated with the
        /// specified <paramref name="key"/> if the <paramref name="key"/> is
        /// found; otherwise, the <paramref name="defaultValue"/> is returned.
        /// </returns>
        public static TValue TryGetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> collection, TKey key, TValue defaultValue)
            where TKey : class
        {
            if (collection == null || key == null)
            {
                Trace.TraceWarning("Oliviann.Collections.Generic.TryGetValue: The dictionary collection and/or key cannot be null.");
                return defaultValue;
            }

            return collection.TryGetValue(key, out TValue value) ? value : defaultValue;
        }

        /// <summary>
        /// Creates a <see cref="NameValueCollection"/> from an
        /// <see cref="IDictionary{TKey,TValue}"/>.
        /// </summary>
        /// <param name="source">The source dictionary to be converted.</param>
        /// <returns>A new <see cref="NameValueCollection"/> with the same
        /// contents as the source dictionary.</returns>
        public static NameValueCollection ToNameValueCollection(this IDictionary<string, string> source)
        {
            ADP.CheckArgumentNull(source, nameof(source));

            var collection = new NameValueCollection();
            foreach (KeyValuePair<string, string> pair in source)
            {
                collection.Add(pair.Key, pair.Value);
            }

            return collection;
        }
    }
}