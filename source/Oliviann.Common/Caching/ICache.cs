namespace Oliviann.Caching
{
    #region Usings

    using System.Collections.Generic;
    using Oliviann.Collections.Generic;

    #endregion

    /// <summary>
    /// Represents an interface for implementing a generic cache handler.
    /// </summary>
    public interface ICache
    {
        /// <summary>
        /// Inserts a cache entry into the cache without overwriting any
        /// existing cache entry. If no provider name is provided, then the
        /// entry will be added to the first available cache provider.
        /// </summary>
        /// <param name="entry">The cache entry to be inserted.</param>
        /// <param name="providerName">Optional. Name of the provider.</param>
        /// <returns>
        /// True if insertion succeeded, or false if there is already an entry
        /// in the cache that has the same key.
        /// </returns>
        bool Add(CacheEntry entry, string providerName = null);

        /// <summary>
        /// Determines whether a cache entry exists in the cache.
        /// </summary>
        /// <param name="entry">The cache entry to search for.</param>
        /// <param name="providerName">Optional. Name of the provider.</param>
        /// <returns>
        /// True if cache contains a matching key; otherwise, false.
        /// </returns>
        bool Contains(CacheEntry entry, string providerName = null);

        /// <summary>
        /// Returns an entry from the cache.
        /// </summary>
        /// <param name="entry">The cache entry to retrieve.</param>
        /// <param name="providerName">Optional. Name of the provider.</param>
        /// <returns>
        /// A reference to the cache entry that is identified by key, if the
        /// entry exists; otherwise, null.
        /// </returns>
        object Get(CacheEntry entry, string providerName = null);

        /// <summary>
        /// Gets all the available keys.
        /// </summary>
        /// <param name="providerName">Optional. Name of the provider.</param>
        /// <returns>A collection of all the available keys. If a providerName
        /// is provided then only the available keys from that provider are
        /// returned.</returns>
        IReadOnlyCollection<string> GetKeys(string providerName = null);

        /// <summary>
        /// Inserts a cache entry into the cache or updates an already existing
        /// entry. If no provider name is provided, then the entry will be added
        /// to the first available cache provider.
        /// </summary>
        /// <param name="entry">The cache entry to be inserted.</param>
        /// <param name="providerName">Optional. Name of the provider.</param>
        void Set(CacheEntry entry, string providerName = null);

        /// <summary>
        /// Removes a cache entry from the cache.
        /// </summary>
        /// <param name="entry">The cache entry to remove.</param>
        /// <param name="providerName">Optional. Name of the provider.</param>
        /// <returns>
        /// If the entry is found in the cache, the removed cache entry;
        /// otherwise, null.
        /// </returns>
        object Remove(CacheEntry entry, string providerName = null);
    }
}