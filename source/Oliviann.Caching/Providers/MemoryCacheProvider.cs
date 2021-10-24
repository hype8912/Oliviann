#if !NET35

namespace Oliviann.Caching.Providers
{
    #region Usings

    using System.Collections.Generic;
    using System.Linq;
#if NETSTANDARD2_0 || NETCOREAPP2_0 || NETCOREAPP2_1
    using System;
    using Microsoft.Extensions.Caching.Memory;
    using Oliviann.Extensions.Caching.Memory;
#else
    using System.Runtime.Caching;
    using Oliviann.Collections.Generic;
#endif

    #endregion Usings

    /// <summary>
    /// Represents a <see cref="MemoryCache"/> cache provider.
    /// </summary>
    public class MemoryCacheProvider : CacheProvider
    {
        #region Fields

        /// <summary>
        /// Defines the default name of this instance.
        /// </summary>
        private const string DefaultName = "MemoryCache";

        /// <summary>
        /// The memory cache instance.
        /// </summary>
        private readonly MemoryCache instance;

        #endregion Fields

        #region Constructor/Destructor

        /// <summary>
        /// Initializes a new instance of the <see cref="MemoryCacheProvider"/>
        /// class.
        /// </summary>
        public MemoryCacheProvider() : this(DefaultName)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MemoryCacheProvider"/>
        /// class.
        /// </summary>
        /// <param name="name">The unique name for the provider.</param>
        public MemoryCacheProvider(string name) : base(name)
        {
#if NETSTANDARD2_0 || NETCOREAPP2_0
            var options = new MemoryCacheOptions();
            this.instance = new MemoryCache(options);
#else
            this.instance = name == DefaultName ? MemoryCache.Default : new MemoryCache(name);
#endif
        }

        #endregion Constructor/Destructor

        #region Methods

        /// <inheritdoc />
        public override bool Add(CacheEntry entry)
        {
#if NETSTANDARD2_0 || NETCOREAPP2_0
            this.Set(entry);
            return true;
#else
            return this.instance.Add(
                entry.Key,
                entry.Value,
                new CacheItemPolicy
                {
                    AbsoluteExpiration = entry.AbsoluteExpiration,
                    SlidingExpiration = entry.SlidingExpiration
                });
#endif
        }

        /// <inheritdoc />
        public override bool Contains(CacheEntry entry)
        {
#if NETSTANDARD2_0 || NETCOREAPP2_0
            return this.instance.TryGetValue(entry.Key, out object _);
#else
            return this.instance.Contains(entry.Key);
#endif
        }

        /// <inheritdoc />
        public override object Get(CacheEntry entry)
        {
            return this.instance.Get(entry.Key);
        }

        /// <inheritdoc />
        public override IReadOnlyCollection<string> GetKeys()
        {
#if NETSTANDARD2_0 || NETCOREAPP2_0 || NETCOREAPP2_1
            return this.instance.GetKeys();
#else
            return this.instance.Select(entry => entry.Key).ToListHelper();
#endif
        }

        /// <inheritdoc />
        public override void Set(CacheEntry entry)
        {
#if NETSTANDARD2_0 || NETCOREAPP2_0
            var options = new MemoryCacheEntryOptions();
            if (entry.SlidingExpiration != TimeSpan.MinValue)
            {
                options.SlidingExpiration = entry.SlidingExpiration;
            }
            else
            {
                options.AbsoluteExpiration = entry.AbsoluteExpiration;
            }

            this.instance.Set(entry.Key, entry.Value, options);
#else
            this.instance.Set(
                entry.Key,
                entry.Value,
                new CacheItemPolicy
                {
                    AbsoluteExpiration = entry.AbsoluteExpiration,
                    SlidingExpiration = entry.SlidingExpiration
                });
#endif
        }

        /// <inheritdoc />
        public override object Remove(CacheEntry entry)
        {
#if NETSTANDARD2_0 || NETCOREAPP2_0
            object result = this.Get(entry);
            this.instance.Remove(entry.Key);

            return result;
#else
            return this.instance.Remove(entry.Key);
#endif
        }

        #endregion Methods
    }
}

#endif