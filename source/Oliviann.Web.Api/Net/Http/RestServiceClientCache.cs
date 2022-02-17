namespace Oliviann.Net.Http
{
    #region Usings

    using System;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using Oliviann.Caching;

    #endregion Usings

    /// <summary>
    /// Represents a REST API client side caching implementation.
    /// </summary>
    internal class RestServiceClientCache : IRestServiceClientCache
    {
        #region Fields

        /// <summary>
        /// the current cache instance.
        /// </summary>
        private readonly ICache cache;

        #endregion Fields

        #region Constructor/Destructor

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="RestServiceClientCache"/> class.
        /// </summary>
        internal RestServiceClientCache() : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="RestServiceClientCache"/> class.
        /// </summary>
        /// <param name="cacheInstance">The cache instance.</param>
        internal RestServiceClientCache(ICache cacheInstance)
        {
            if (cacheInstance == null)
            {
                var builder = new CacheBuilder();
                builder.AddMemoryCache();
                cacheInstance = builder;
            }

            this.cache = cacheInstance;
        }

        #endregion Constructor/Destructor

        #region Methods

        /// <summary>
        /// Validates and adds the specified result message to the cache
        /// </summary>
        /// <param name="response">The current response message.</param>
        /// <param name="result">The response message content result.</param>
        public void ClientCacheSet(HttpResponseMessage response, object result)
        {
            if (!this.ShouldCacheResponse(response.Headers.CacheControl))
            {
                return;
            }

            string key = response.RequestMessage.RequestUri.AbsoluteUri;
            this.SetCache(key, result, response.Headers.CacheControl.MaxAge);
        }

        /// <summary>
        /// Gets the associated item from the cache if available.
        /// </summary>
        /// <typeparam name="T">The type of object to be returned.</typeparam>
        /// <param name="key">A unique identifier for the cache entry to get.
        /// </param>
        /// <returns>
        /// A reference to the cache entry that is identified by key, if the
        /// entry exists; otherwise, null.
        /// </returns>
        public T GetResponseFromCache<T>(string key) => this.cache.Get<T>(key);

        #endregion Methods

        #region Helper Methods

        /// <summary>
        /// Determines whether the response content should be cached.
        /// </summary>
        /// <param name="cacheControl">The cache control header.</param>
        /// <returns>
        /// True if the response should be cached; otherwise, false.
        /// </returns>
        protected bool ShouldCacheResponse(CacheControlHeaderValue cacheControl)
        {
            if (cacheControl == null)
            {
                return false;
            }

            return !cacheControl.NoCache;
        }

        /// <summary>
        /// Adds the specified key and result to the cache based on the cache
        /// control age.
        /// </summary>
        /// <param name="key">A unique identifier for the cache entry.</param>
        /// <param name="result">The object to be stored in the cache.</param>
        /// <param name="cacheControlMaxAge">The cache control maximum age.
        /// </param>
        protected void SetCache(string key, object result, TimeSpan? cacheControlMaxAge)
        {
            ADP.CheckArgumentNullOrEmpty(key, nameof(key));
            if (cacheControlMaxAge == null)
            {
                return;
            }

            var entry = new CacheEntry(key, result) { AbsoluteExpiration = DateTimeOffset.UtcNow.AddSeconds(cacheControlMaxAge.Value.TotalSeconds) };
            this.cache.Set(entry);
        }

        #endregion Helper Methods
    }
}