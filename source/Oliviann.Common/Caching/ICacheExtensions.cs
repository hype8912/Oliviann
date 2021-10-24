namespace Oliviann.Caching
{
    #region Usings

    using System;

    #endregion Usings

    /// <summary>
    /// Represents a collection of extension methods to make working with
    /// <see cref="ICache"/> easier.
    /// </summary>
    public static class ICacheExtensions
    {
        #region Add

        /// <summary>
        /// Inserts a cache entry into the cache without overwriting any
        /// existing cache entry.
        /// </summary>
        /// <param name="cache">The cache instance.</param>
        /// <param name="key">A unique identifier for the cache entry.</param>
        /// <param name="value">The object to insert.</param>
        /// <param name="providerName">Optional. Name of the provider.</param>
        /// <returns>True if insertion succeeded, or false if there is already
        /// an entry in the cache that has the same key.</returns>
        public static bool Add(this ICache cache, string key, object value, string providerName = null) =>
            cache.Add(new CacheEntry(key, value), providerName);

        /// <summary>
        /// Inserts a cache entry into the cache without overwriting any
        /// existing cache entry.
        /// </summary>
        /// <param name="cache">The cache instance.</param>
        /// <param name="key">A unique identifier for the cache entry.</param>
        /// <param name="value">The object to insert.</param>
        /// <param name="absoluteExpiration">The fixed date and time at which
        /// the cache entry will expire.</param>
        /// <param name="providerName">Optional. Name of the provider.</param>
        /// <returns>True if insertion succeeded, or false if there is already
        /// an entry in the cache that has the same key.</returns>
        public static bool Add(
            this ICache cache,
            string key,
            object value,
            DateTimeOffset absoluteExpiration,
            string providerName = null)
        {
            return cache.Add(new CacheEntry(key, value) { AbsoluteExpiration = absoluteExpiration }, providerName);
        }

        /// <summary>
        /// Inserts a cache entry into the cache without overwriting any
        /// existing cache entry.
        /// </summary>
        /// <param name="cache">The cache instance.</param>
        /// <param name="key">A unique identifier for the cache entry.</param>
        /// <param name="value">The object to insert.</param>
        /// <param name="slidingExpiration">The interval between the time the
        /// added object was last accessed and the time at which that object
        /// expires.</param>
        /// <param name="providerName">Optional. Name of the provider.</param>
        /// <returns>True if insertion succeeded, or false if there is already
        /// an entry in the cache that has the same key.</returns>
        public static bool Add(
            this ICache cache,
            string key,
            object value,
            TimeSpan slidingExpiration,
            string providerName = null)
        {
            return cache.Add(new CacheEntry(key, value) { SlidingExpiration = slidingExpiration }, providerName);
        }

        /// <summary>
        /// Gets an existing cache entry with the specified key or inserts a new
        /// cache entry and returns the cache entry.
        /// </summary>
        /// <typeparam name="TOut">The type of the cache entry value.
        /// </typeparam>
        /// <param name="cache">The cache instance.</param>
        /// <param name="key">A unique identifier for the cache entry to add or
        /// get.</param>
        /// <param name="getDelegate">A delegate method for retrieving the cache
        /// object from. For example, this may be a method that retrieves the
        /// object to be cached from a database or other source.</param>
        /// <param name="providerName">Optional. Name of the provider.</param>
        /// <returns>A reference to the cache entry that is identified by key,
        /// if the entry exists; otherwise, the value returned from the
        /// delegate.</returns>
        public static TOut AddOrGetExisting<TOut>(
            this ICache cache,
            string key,
            Func<TOut> getDelegate,
            string providerName = null) =>
            AddOrGetExisting(cache, new CacheEntry(key), getDelegate, providerName);

        /// <summary>
        /// Gets an existing cache entry with the specified key or inserts a new
        /// cache entry and returns the cache entry.
        /// </summary>
        /// <typeparam name="TOut">The type of the cache entry value.
        /// </typeparam>
        /// <param name="cache">The cache instance.</param>
        /// <param name="key">A unique identifier for the cache entry to add or
        /// get.</param>
        /// <param name="getDelegate">A delegate method for retrieving the cache
        /// object from. For example, this may be a method that retrieves the
        /// object to be cached from a database or other source.</param>
        /// <param name="absoluteExpirationUtc">The fixed date and time at which
        /// the cache entry will expire.</param>
        /// <param name="providerName">Optional. Name of the provider.</param>
        /// <returns>A reference to the cache entry that is identified by key,
        /// if the entry exists; otherwise, the value returned from the
        /// delegate.</returns>
        public static TOut AddOrGetExisting<TOut>(
            this ICache cache,
            string key,
            Func<TOut> getDelegate,
            DateTime absoluteExpirationUtc,
            string providerName = null)
        {
            return AddOrGetExisting(
                cache,
                new CacheEntry(key) { AbsoluteExpiration = DateTime.SpecifyKind(absoluteExpirationUtc, DateTimeKind.Utc) },
                getDelegate,
                providerName);
        }

        /// <summary>
        /// Gets an existing cache entry with the specified key or inserts a new
        /// cache entry and returns the cache entry.
        /// </summary>
        /// <typeparam name="TOut">The type of the cache entry value.
        /// </typeparam>
        /// <param name="cache">The cache instance.</param>
        /// <param name="entry">The cache entry configuration.</param>
        /// <param name="getDelegate">A delegate method for retrieving the cache
        /// object from. For example, this may be a method that retrieves the
        /// object to be cached from a database or other source.</param>
        /// <param name="providerName">Optional. Name of the provider.</param>
        /// <returns>A reference to the cache entry that is identified by key,
        /// if the entry exists; otherwise, the value returned from the
        /// delegate.</returns>
        public static TOut AddOrGetExisting<TOut>(
            this ICache cache,
            CacheEntry entry,
            Func<TOut> getDelegate,
            string providerName = null)
        {
            ADP.CheckArgumentNull(cache, nameof(cache));
            ADP.CheckArgumentNull(entry, nameof(entry));

            if (cache.Contains(entry, providerName))
            {
                return (TOut)cache.Get(entry, providerName);
            }

            ADP.CheckArgumentNull(getDelegate, nameof(getDelegate));
            entry.Value = getDelegate();
            if (entry.Value != null)
            {
                cache.Add(entry, providerName);
            }

            return (TOut)entry.Value;
        }

        #endregion Add

        #region Contains

        /// <summary>
        /// Determines of the cache contains the specified key.
        /// </summary>
        /// <param name="cache">The cache instance.</param>
        /// <param name="key">A unique identifier for the cache entry.</param>
        /// <param name="providerName">Optional. Name of the provider.</param>
        /// <returns>True if cache contains a matching key; otherwise, false.
        /// </returns>
        public static bool Contains(this ICache cache, string key, string providerName = null)
        {
            return cache != null && cache.Contains(new CacheEntry(key), providerName);
        }

        #endregion Contains

        #region Get

        /// <summary>
        /// Wrapper method for the
        /// <see cref="M:ICache.Get(string)" /> method for returning the object
        /// as a specific type.
        /// </summary>
        /// <typeparam name="TOut">The type of the object to be outputted.
        /// </typeparam>
        /// <param name="cache">The cache instance.</param>
        /// <param name="key">A unique identifier for the cache entry to get.
        /// </param>
        /// <param name="target">Optional. The target scope of the cache entry.
        /// </param>
        /// <param name="providerName">Optional. Name of the provider.</param>
        /// <returns>
        /// A reference to the cache entry that is identified by key, if the
        /// entry exists; otherwise, default value.
        /// </returns>
        public static TOut Get<TOut>(
            this ICache cache,
            string key,
            CacheTarget target = CacheTarget.Default,
            string providerName = null)
        {
            if (cache == null)
            {
                return default(TOut);
            }

            var entry = new CacheEntry(key) { Target = target };
            object result = cache.Get(entry, providerName);
            return result == null ? default(TOut) : (TOut)result;
        }

        #endregion Get

        #region Set

        /// <summary>
        /// Inserts a cache entry into the cache or updates an already existing
        /// entry.
        /// </summary>
        /// <param name="cache">The cache instance.</param>
        /// <param name="key">A unique identifier for the cache entry.</param>
        /// <param name="value">The object to insert.</param>
        /// <param name="providerName">Optional. Name of the provider.</param>
        public static void Set(this ICache cache, string key, object value, string providerName = null) =>
            cache.Set(new CacheEntry(key, value), providerName);

        /// <summary>
        /// Inserts a cache entry into the cache or updates an already existing
        /// entry.
        /// </summary>
        /// <param name="cache">The cache instance.</param>
        /// <param name="key">A unique identifier for the cache entry.</param>
        /// <param name="value">The object to insert.</param>
        /// <param name="absoluteExpiration">The fixed date and time at which
        /// the cache entry will expire. </param>
        /// <param name="providerName">Optional. Name of the provider.</param>
        public static void Set(
            this ICache cache,
            string key,
            object value,
            DateTimeOffset absoluteExpiration,
            string providerName = null)
        {
            cache.Set(new CacheEntry(key, value) { AbsoluteExpiration = absoluteExpiration }, providerName);
        }

        /// <summary>
        /// Inserts a cache entry into the cache or updates an already existing
        /// entry.
        /// </summary>
        /// <param name="cache">The cache instance.</param>
        /// <param name="key">A unique identifier for the cache entry.</param>
        /// <param name="value">The object to insert.</param>
        /// <param name="slidingExpiration">The interval between the time the
        /// added object was last accessed and the time at which that object
        /// expires.</param>
        /// <param name="providerName">Optional. Name of the provider.</param>
        public static void Set(
            this ICache cache,
            string key,
            object value,
            TimeSpan slidingExpiration,
            string providerName = null)
        {
            cache.Set(new CacheEntry(key, value) { SlidingExpiration = slidingExpiration });
        }

        #endregion Set

        #region Remove

        /// <summary>
        /// Removes the specified cache entry key.
        /// </summary>
        /// <param name="cache">The cache instance.</param>
        /// <param name="key">The unique identifier for the cache entry.</param>
        /// <param name="providerName">Optional. Name of the provider.</param>
        /// <returns>If the entry is found in the cache, the removed cache
        /// entry; otherwise, null.</returns>
        public static object Remove(this ICache cache, string key, string providerName = null) =>
            cache?.Remove(new CacheEntry(key), providerName);

        #endregion Remove
    }
}