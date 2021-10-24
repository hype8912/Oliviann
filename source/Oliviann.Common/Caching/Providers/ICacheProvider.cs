namespace Oliviann.Caching.Providers
{
    #region Usings

    using System.Collections.Generic;
    using Oliviann.Collections.Generic;

    #endregion

    /// <summary>
    /// Represents a single cache provider implementation.
    /// </summary>
    public interface ICacheProvider
    {
        #region Properties

        /// <summary>
        /// Gets the name of the cache instance.
        /// </summary>
        /// <value>
        /// The name of the cache instance.
        /// </value>
        string Name { get; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Inserts a cache entry into the cache without overwriting any
        /// existing cache entry.
        /// </summary>
        /// <param name="entry">The cache entry to be inserted.</param>
        /// <returns>
        /// True if insertion succeeded, or false if there is already
        /// an entry in the cache that has the same key.
        /// </returns>
        bool Add(CacheEntry entry);

        /// <summary>
        /// Determines whether a cache entry exists in the cache.
        /// </summary>
        /// <param name="entry">The cache entry to check.</param>
        /// <returns>
        /// True if cache contains a matching key; otherwise, false.
        /// </returns>
        bool Contains(CacheEntry entry);

        /// <summary>
        /// Returns an entry from the cache.
        /// </summary>
        /// <param name="entry">The entry to retrieve.</param>
        /// <returns>
        /// A reference to the cache entry that is identified by key, if the
        /// entry exists; otherwise, null.
        /// </returns>
        object Get(CacheEntry entry);

        /// <summary>
        /// Gets a collection of all the available keys.
        /// </summary>
        /// <returns>A collection of available keys.</returns>
        IReadOnlyCollection<string> GetKeys();

        /// <summary>
        /// Inserts a cache entry into the cache or updates an already existing
        /// entry.
        /// </summary>
        /// <param name="entry">The cache entry to be inserted.</param>
        void Set(CacheEntry entry);

        /// <summary>
        /// Removes a cache entry from the cache.
        /// </summary>
        /// <param name="entry">The cache entry to remove.</param>
        /// <returns>
        /// If the entry is found in the cache, the removed cache entry;
        /// otherwise, null.
        /// </returns>
        object Remove(CacheEntry entry);

        #endregion Methods
    }
}