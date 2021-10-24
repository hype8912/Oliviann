namespace Oliviann.Caching
{
    #region Usings

    using System;

    #endregion Usings

    /// <summary>
    /// Represents an individual cache entry in the cache.
    /// </summary>
    [Serializable]
    public class CacheEntry
    {
        #region Constructor/Destructor

        /// <summary>
        /// Initializes a new instance of the <see cref="CacheEntry" /> class.
        /// </summary>
        /// <param name="key">The unique identifier for a cache entry.</param>
        public CacheEntry(string key)
        {
            this.Key = key;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CacheEntry" /> class.
        /// </summary>
        /// <param name="key">The unique identifier for a cache entry.</param>
        /// <param name="value">The data for a cache entry.</param>
        public CacheEntry(string key, object value)
        {
            this.Key = key;
            this.Value = value;
        }

        #endregion Constructor/Destructor

        #region Properties

        /// <summary>
        /// Gets or sets the fixed date and time at which the cache entry will
        /// expire.
        /// </summary>
        /// <value>
        /// The fixed date and time at which the cache entry will expire.
        /// </value>
        public DateTimeOffset AbsoluteExpiration { get; set; } = DateTimeOffset.MaxValue;

        /// <summary>
        /// Gets or sets a unique identifier for a CacheEntry instance.
        /// </summary>
        /// <value>
        /// A unique identifier for a CacheEntry instance.
        /// </value>
        public string Key { get; set; }

        /// <summary>
        /// Gets or sets the interval between the time the added object was last
        /// accessed and the time at which that object expires.
        /// </summary>
        /// <value>
        /// The interval between the time the added object was last accessed and
        /// the time at which that object expires.
        /// </value>
        public TimeSpan SlidingExpiration { get; set; } = TimeSpan.Zero;

        /// <summary>
        /// Gets or sets the target scope of the cache entry.
        /// </summary>
        /// <value>
        /// The target scope of the cache entry.
        /// </value>
        public CacheTarget Target { get; set; } = CacheTarget.Default;

        /// <summary>
        /// Gets or sets the data for a CacheEntry instance.
        /// </summary>
        /// <value>
        /// The data for a CacheEntry instance.
        /// </value>
        public object Value { get; set; }

        /// <summary>
        /// Gets or sets the version object.
        /// </summary>
        /// <value>
        /// The version object.
        /// </value>
        public object Version { get; set; }

        #endregion Properties
    }
}