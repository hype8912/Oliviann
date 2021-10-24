#if NET40_OR_GREATER

namespace Oliviann.Runtime.Caching
{
    #region Usings

    using System;
    using System.Runtime.Caching;

    #endregion Usings

    /// <summary>
    /// Represents a collection of commonly used extension methods for extending
    /// <see cref="MemoryCache"/>.
    /// </summary>
    public static class MemoryCacheExtensions
    {
        #region Adds

        /// <summary>
        /// Returns an entry from the cache for the specified
        /// <paramref name="key" /> if the key exists in the specified
        /// <paramref name="cache" />. If no entry exists then the specified
        /// <paramref name="getDelegate" /> is called for retrieving the value
        /// in inserting into the cache.
        /// </summary>
        /// <typeparam name="T">The type of object being cached.</typeparam>
        /// <param name="cache">The object cache instance.</param>
        /// <param name="key">A unique identifier for the cache entry to get.
        /// </param>
        /// <param name="getDelegate">A delegate method for retrieving the cache
        /// object from.</param>
        /// <param name="slidingExpiration">Optional. A span of time within
        /// which a cache entry must be accessed before the cache entry is
        /// evicted from the cache.</param>
        /// <returns>
        /// A reference to the cache entry that is identified by key, if the
        /// entry exists; otherwise, null.
        /// </returns>
        public static T AddOrGetExisting<T>(
                                            this ObjectCache cache,
                                            string key,
                                            Func<T> getDelegate,
                                            TimeSpan? slidingExpiration = null)
        {
            var policy = slidingExpiration == null
                ? new CacheItemPolicy()
                : new CacheItemPolicy { SlidingExpiration = slidingExpiration.Value };
            return cache.AddOrGetExisting<T>(key, getDelegate, policy);
        }

        /// <summary>
        /// Returns an entry from the cache for the specified
        /// <paramref name="key" /> if the key exists in the specified
        /// <paramref name="cache" />. If no entry exists then the specified
        /// <paramref name="getDelegate" /> is called for retrieving the value
        /// in inserting into the cache.
        /// </summary>
        /// <typeparam name="T">The type of object being cached.</typeparam>
        /// <param name="cache">The object cache instance.</param>
        /// <param name="key">A unique identifier for the cache entry to get.
        /// </param>
        /// <param name="getDelegate">A delegate method for retrieving the cache
        /// object from.</param>
        /// <param name="policy">The cache policy for the object being cached.
        /// </param>
        /// <returns>
        /// A reference to the cache entry that is identified by key, if the
        /// entry exists; otherwise, null.
        /// </returns>
        public static T AddOrGetExisting<T>(this ObjectCache cache, string key, Func<T> getDelegate, CacheItemPolicy policy)
        {
            ADP.CheckArgumentNull(cache, nameof(cache));
            ADP.CheckArgumentNull(key, nameof(key));

            if (cache.Contains(key))
            {
                return (T)cache[key];
            }

            ADP.CheckArgumentNull(getDelegate, nameof(getDelegate));
            T result = getDelegate();
            if (result != null)
            {
                cache.Add(key, result, policy ?? new CacheItemPolicy());
            }

            return result;
        }

        #endregion Adds

        #region Gets

        /// <summary>Wrapper method for the
        /// <see cref="MemoryCache.Get(string, string)"/> method for returning
        /// the object as a specific type.</summary>
        /// <typeparam name="TOut">The type of the object to be outputted.
        /// </typeparam>
        /// <param name="cache">The object cache instance.</param>
        /// <param name="key">A unique identifier for the cache entry to get.
        /// </param>
        /// <returns>A reference to the cache entry that is identified by key,
        /// if the entry exists; otherwise, null.</returns>
        public static TOut Get<TOut>(this ObjectCache cache, string key)
        {
            if (cache == null)
            {
                return default(TOut);
            }

            object result = cache.Get(key);
            return result == null ? default(TOut) : (TOut)result;
        }

        /// <summary>
        /// Determines if the cache contains an entry for the specified key and
        /// tries to retrieve it.
        /// </summary>
        /// <typeparam name="TOut">The type of the object to be outputted.
        /// </typeparam>
        /// <param name="cache">The object cache instance.</param>
        /// <param name="key">>A unique identifier for the cache entry to get.
        /// </param>
        /// <param name="value">A reference to the cache entry that is
        /// identified by key, if the entry exists; otherwise, null.</param>
        /// <returns>True if the cache contains the specified key; otherwise,
        /// false.</returns>
        public static bool TryGetValue<TOut>(this ObjectCache cache, string key, out TOut value)
        {
            if (cache != null && cache.Contains(key))
            {
                value = cache.Get<TOut>(key);
                return true;
            }

            value = default(TOut);
            return false;
        }

        #endregion Gets

        #region Inserts

        /// <summary>
        /// Inserts an item in the <paramref name="cache"/> object with a cache
        /// key to reference its location.
        /// </summary>
        /// <param name="cache">The object cache instance.</param>
        /// <param name="key">The cache key used to reference the item.</param>
        /// <param name="value">The object to be inserted into the cache.
        /// </param>
        /// <param name="slidingExpiration">A span of time within which a cache
        /// entry must be accessed before the cache entry is evicted from the
        /// cache.</param>
        /// <returns>
        /// True if the insertion succeeded; otherwise, false.
        /// </returns>
        /// <example>
        /// The following example demonstrates how to insert an item into an
        /// application's cache.
        ///   <code>
        /// MemoryCache.Default.Insert("DSN", connectionString);
        ///   </code></example>
        /// <remarks>
        /// This method will overwrite an existing cache item whose key matches
        /// the <paramref name="key"/> parameter. The object added to the cache
        /// using this extension method is inserted with no file or cache
        /// dependencies, a priority of Default, a sliding expiration value of
        /// <see cref="ObjectCache.NoSlidingExpiration"/>, and an absolute
        /// expiration of <see cref="ObjectCache.InfiniteAbsoluteExpiration"/>.
        /// </remarks>
        public static bool Insert(this ObjectCache cache, string key, object value, TimeSpan? slidingExpiration = null)
        {
            ADP.CheckArgumentNull(cache, nameof(cache));
            ADP.CheckArgumentNull(key, nameof(key));

            if (cache.Contains(key))
            {
                cache[key] = value;
            }
            else if (slidingExpiration != null)
            {
                cache.Set(key, value, new CacheItemPolicy { SlidingExpiration = slidingExpiration.Value });
            }
            else
            {
                cache.Set(key, value, ObjectCache.InfiniteAbsoluteExpiration);
            }

            return cache.Contains(key);
        }

        #endregion Inserts
    }
}

#endif