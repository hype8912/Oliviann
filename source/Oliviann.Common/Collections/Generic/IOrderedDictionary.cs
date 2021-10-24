namespace Oliviann.Collections.Generic
{
    #region Usings

    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;

    #endregion Usings

    /// <summary>
    /// Represents a generic collection of key/value pairs that are ordered
    /// independently of the key and value.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys in the dictionary
    /// </typeparam>
    /// <typeparam name="TValue">The type of the values in the dictionary
    /// </typeparam>
    public interface IOrderedDictionary<TKey, TValue> : IOrderedDictionary, IDictionary<TKey, TValue>
    {
        /// <summary>
        /// Gets or sets the value at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the value to get or set.
        /// </param>
        /// <value>The value of the item at the specified index.</value>
        /// <returns>The element at the specified index.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="index"/> is less than 0.<br/>
        /// -or-<br/>
        /// <paramref name="index"/> is equal to or greater than
        /// <see cref="System.Collections.ICollection.Count"/>.</exception>
        new TValue this[int index] { get; set; }

        /// <summary>
        /// Adds an entry with the specified key and value into the
        /// <see cref="IOrderedDictionary{TKey,TValue}"/> collection with the
        /// lowest available index.
        /// </summary>
        /// <param name="key">The key of the entry to add.</param>
        /// <param name="value">The value of the entry to add.</param>
        /// <returns>The index of the newly added entry</returns>
        /// <remarks>
        /// <para>You can also use the
        /// <see cref="M:IDictionary{TKey, TValue}.Item(TKey)"/>
        /// property to add new elements by setting the value of a key that does
        /// not exist in the <see cref="IOrderedDictionary{TKey,TValue}"/>
        /// collection; however, if the specified key already exists in the
        /// <see cref="IOrderedDictionary{TKey,TValue}"/>, setting the
        /// <see cref="M:Item(int)"/> property overwrites the old value. In
        /// contrast, the <see cref="Add"/> method does not modify existing
        /// elements.</para></remarks>
        /// <exception cref="ArgumentException">An element with the same key
        /// already exists in the <see cref="IOrderedDictionary{TKey,TValue}"/>
        /// </exception>
        /// <exception cref="NotSupportedException">The
        /// <see cref="IOrderedDictionary{TKey,TValue}"/> is read-only.<br/>
        /// -or-<br/>
        /// The <see cref="IOrderedDictionary{TKey,TValue}"/> has a fixed size.
        /// </exception>
        new int Add(TKey key, TValue value);
    }
}