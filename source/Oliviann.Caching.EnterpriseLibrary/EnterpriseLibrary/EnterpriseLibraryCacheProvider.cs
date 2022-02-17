namespace Oliviann.Caching.EnterpriseLibrary
{
    #region Usings

    using System;
    using System.Collections.Generic;
    using Oliviann.Caching.Providers;
    using Oliviann.Collections.Generic;
    using Microsoft.Practices.EnterpriseLibrary.Caching;
    using Microsoft.Practices.EnterpriseLibrary.Caching.Expirations;

    #endregion Usings

    /// <summary>
    /// Represents a Enterprise Library Cache provider.
    /// </summary>
    public class EnterpriseLibraryCacheProvider : CacheProvider
    {
        #region Fields

        /// <summary>
        /// The cache manager instance.
        /// </summary>
        private readonly ICacheManager instance;

        #endregion Fields

        #region Constructor/Destructor

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="EnterpriseLibraryCacheProvider"/> class.
        /// </summary>
        public EnterpriseLibraryCacheProvider() : base("EnterpriseLibrary.Cache")
        {
            this.instance = CacheFactory.GetCacheManager();
        }

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="EnterpriseLibraryCacheProvider"/> class.
        /// </summary>
        /// <param name="name">The unique name for the provider.</param>
        public EnterpriseLibraryCacheProvider(string name) : base(name)
        {
            this.instance = CacheFactory.GetCacheManager(name);
        }

        #endregion Constructor/Destructor

        #region Methods

        /// <inheritdoc />
        public override bool Add(CacheEntry entry)
        {
            if (entry == null || StringExtensions.IsNullOrEmpty(entry.Key))
            {
                return false;
            }

            var expirations = new List<ICacheItemExpiration>();
            if (entry.SlidingExpiration != TimeSpan.MinValue)
            {
                expirations.Add(new SlidingTime(entry.SlidingExpiration));
            }
            else if (entry.AbsoluteExpiration != DateTimeOffset.MaxValue)
            {
                expirations.Add(new AbsoluteTime(entry.AbsoluteExpiration.UtcDateTime));
            }
            else
            {
                expirations.Add(new NeverExpired());
            }

            this.instance.Add(entry.Key, entry.Value, CacheItemPriority.Normal, null, expirations.ToArray());
            return true;
        }

        /// <inheritdoc />
        public override bool Contains(CacheEntry entry) => this.instance.Contains(entry.Key);

        /// <inheritdoc />
        public override object Get(CacheEntry entry) => this.instance[entry.Key];

        /// <inheritdoc />
        public override IReadOnlyCollection<string> GetKeys() => CollectionHelpers.CreateReadOnlyCollection<string>();

        /// <inheritdoc />
        public override void Set(CacheEntry entry) => this.Add(entry);

        /// <inheritdoc />
        public override object Remove(CacheEntry entry)
        {
            if (!this.Contains(entry))
            {
                return null;
            }

            object value = this.Get(entry);
            this.instance.Remove(entry.Key);
            return value;
        }

        #endregion Methods
    }
}