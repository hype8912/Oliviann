namespace Oliviann.Caching.NCache
{
    #region Usings

    using System;
    using System.Collections.Generic;
    using Alachisoft.NCache.Web.Caching;
    using Oliviann.Caching.Providers;
    using Oliviann.Collections.Generic;
    using CacheEntry = Oliviann.Caching.CacheEntry;

    #endregion Usings

    /// <summary>
    /// Represents a NCache cache provider.
    /// </summary>
    public class NCacheProvider : CacheProvider
    {
        #region Fields

        /// <summary>
        /// The NCache cache instance.
        /// </summary>
        private Cache cacheInstance;

        #endregion Fields

        #region Constructor/Destructor

        /// <summary>
        /// Initializes a new instance of the <see cref="NCacheProvider"/>
        /// class.
        /// </summary>
        public NCacheProvider() : this("NCache")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NCacheProvider"/>
        /// class.
        /// </summary>
        /// <param name="name">The unique name for the provider.</param>
        public NCacheProvider(string name) : base(name)
        {
            this.CacheOptions = new CacheOptions();
        }

        #endregion Constructor/Destructor

        #region Properties

        /// <summary>
        /// Gets the cache options instance.
        /// </summary>
        /// <value>
        /// The cache options instance.
        /// </value>
        public CacheOptions CacheOptions { get; internal set; }

        #endregion Properties

        #region Methods

        /// <inheritdoc />
        public override bool Add(CacheEntry entry)
        {
            if (entry == null || StringExtensions.IsNullOrEmpty(entry.Key))
            {
                return false;
            }

            var item = new CacheItem(entry.Value);
            if (entry.SlidingExpiration != TimeSpan.MinValue)
            {
                item.SlidingExpiration = entry.SlidingExpiration;
            }
            else
            {
                item.AbsoluteExpiration = entry.AbsoluteExpiration.UtcDateTime;
            }

            this.cacheInstance.Add(entry.Key, item);
            return true;
        }

        /// <inheritdoc />
        public override bool Contains(CacheEntry entry)
        {
            return this.GetInstance().Contains(entry.Key);
        }

        /// <inheritdoc />
        public override object Get(CacheEntry entry) => this.GetInstance().Get(entry.Key);

        /// <inheritdoc />
        public override IReadOnlyCollection<string> GetKeys()
        {
#if NET40
            var keys = new ReadOnlyListWrapper<string>();
#else
            var keys = new List<string>();
#endif
            foreach (string key in this.GetInstance())
            {
                keys.Add(key);
            }

            return keys;
        }

        /// <inheritdoc />
        public override void Set(CacheEntry entry)
        {
            if (entry == null || StringExtensions.IsNullOrEmpty(entry.Key))
            {
                return;
            }

            var item = new CacheItem(entry.Value);
            if (entry.SlidingExpiration != TimeSpan.MinValue)
            {
                item.SlidingExpiration = entry.SlidingExpiration;
            }
            else
            {
                item.AbsoluteExpiration = entry.AbsoluteExpiration.UtcDateTime;
            }

            this.GetInstance().Insert(entry.Key, item);
        }

        /// <inheritdoc />
        public override object Remove(CacheEntry entry) => this.GetInstance().Remove(entry.Key);

        #endregion Methods

        #region Helper Methods

        /// <summary>
        /// Gets the available cache instance.
        /// </summary>
        /// <returns>An instance for communicating with the cache.</returns>
        private Cache GetInstance()
        {
            if (this.cacheInstance != null)
            {
                return this.cacheInstance;
            }

            var serverinfo = new List<CacheServerInfo>();
            foreach (CacheServerEntry serverEntry in this.CacheOptions.Servers)
            {
                serverinfo.Add(
                    serverEntry.Port.HasValue
                        ? new CacheServerInfo(serverEntry.Name, serverEntry.Port.Value)
                        : new CacheServerInfo(serverEntry.Name));
            }

            var parms = new CacheInitParams { ServerList = serverinfo.ToArray() };
            this.cacheInstance = NCache.InitializeCache(this.Name, parms);
            return this.cacheInstance;
        }

        #endregion Helper Methods
    }
}