#if NETFRAMEWORK

namespace Oliviann.Web.Caching
{
    #region Usings

    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Caching;
    using Oliviann.Caching;
    using Oliviann.Collections.Generic;

    #endregion

    /// <summary>
    /// Represents a proxy for calling the Cache instance.
    /// </summary>
    public class CacheProxy : ICache
    {
        #region Fields

        private readonly Cache instance;

        #endregion

        #region Constructor/Destructor

        /// <summary>
        /// Initializes a new instance of the <see cref="CacheProxy" /> class.
        /// </summary>
        /// <param name="cache">The cache instance.</param>
        public CacheProxy(Cache cache)
        {
            ADP.CheckArgumentNull(cache, nameof(cache));
            this.instance = cache;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        public bool Add(CacheEntry entry, string providerName = null)
        {
            if (entry == null || entry.Key.IsNullOrWhiteSpace())
            {
                return false;
            }

            this.instance.Add(
                entry.Key,
                entry.Value,
                null,
                this.ConvertAbsoluteExpiration(entry.AbsoluteExpiration),
                entry.SlidingExpiration,
                CacheItemPriority.Default,
                null);

            return true;
        }

        /// <inheritdoc />
        public bool Contains(CacheEntry entry, string providerName = null) => this.Get(entry) != null;

        /// <inheritdoc />
        public object Get(CacheEntry entry, string providerName = null)
        {
            if (entry?.Key == null)
            {
                return null;
            }

            return this.instance.Get(entry.Key);
        }

        /// <inheritdoc />
        public IReadOnlyCollection<string> GetKeys(string providerName = null)
        {
            return this.instance.Cast<DictionaryEntry>().Select(entry => (string)entry.Key).ToListHelper();
        }

        /// <inheritdoc />
        public void Set(CacheEntry entry, string providerName = null)
        {
            if (entry == null || entry.Key.IsNullOrWhiteSpace())
            {
                return;
            }

            this.instance.Insert(
                entry.Key,
                entry.Value,
                null,
                this.ConvertAbsoluteExpiration(entry.AbsoluteExpiration),
                entry.SlidingExpiration);
        }

        /// <inheritdoc />
        public object Remove(CacheEntry entry, string providerName = null)
        {
            if (entry?.Key == null)
            {
                return null;
            }

            return this.instance.Remove(entry.Key);
        }

        /// <summary>
        /// Converts the DateTimeOffset absolute expiration to the correct
        /// DateTime.
        /// </summary>
        /// <param name="offset">The current date time offset.</param>
        /// <returns>The matching DateTime instance of the absolute expiration.
        /// </returns>
        private DateTime ConvertAbsoluteExpiration(DateTimeOffset offset)
        {
            return offset == DateTimeOffset.MaxValue ? Cache.NoAbsoluteExpiration : offset.UtcDateTime;
        }

        #endregion
    }
}

#endif