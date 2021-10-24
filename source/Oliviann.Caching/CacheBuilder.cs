namespace Oliviann.Caching
{
    #region Usings

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Oliviann.Caching.Providers;
    using Oliviann.Properties;
    using Oliviann.Collections.Generic;

    #endregion Usings

    /// <summary>
    /// Represents a cache builder for adding cache providers.
    /// </summary>
    public class CacheBuilder : ICache
    {
        #region Constructor/Destructor

        /// <summary>
        /// Initializes a new instance of the <see cref="CacheBuilder"/> class.
        /// </summary>
        public CacheBuilder()
        {
            this.Providers = new Dictionary<string, ICacheProvider>(StringComparer.InvariantCultureIgnoreCase);
        }

        #endregion Constructor/Destructor

        #region Properties

        /// <summary>
        /// Gets the collection of cache providers.
        /// </summary>
        /// <value>
        /// The collection of cache providers.
        /// </value>
        private Dictionary<string, ICacheProvider> Providers { get; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Adds the provider to the collection of cache providers.
        /// </summary>
        /// <param name="provider">The provider instance to be added.</param>
        public void AddProvider(ICacheProvider provider)
        {
            ADP.CheckArgumentNull(provider, nameof(provider));

            if (provider.Name.IsNullOrWhiteSpace())
            {
                throw new ArgumentException(Resources.ERR_NullOrEmpty.FormatWith("Provider unique name"));
            }

            if (this.Providers.ContainsKey(provider.Name))
            {
                this.Providers[provider.Name] = provider;
            }
            else
            {
                this.Providers.Add(provider.Name, provider);
            }
        }

        /// <inheritdoc />
        public bool Add(CacheEntry entry, string providerName = null)
        {
            if (entry == null || entry.Key.IsNullOrWhiteSpace() || this.Providers.Count == 0)
            {
                return false;
            }

            if (this.TryGetProvider(providerName, out ICacheProvider foundProvider))
            {
                return foundProvider.Add(entry);
            }

            return this.Providers.First().Value.Add(entry);
        }

        /// <inheritdoc />
        public bool Contains(CacheEntry entry, string providerName = null)
        {
            if (entry == null || entry.Key.IsNullOrWhiteSpace())
            {
                return false;
            }

            if (this.TryGetProvider(providerName, out ICacheProvider foundProvider))
            {
                return foundProvider.Contains(entry);
            }

            foreach (KeyValuePair<string, ICacheProvider> provider in this.Providers)
            {
                if (provider.Value.Contains(entry))
                {
                    return true;
                }
            }

            return false;
        }

        /// <inheritdoc />
        public object Get(CacheEntry entry, string providerName = null)
        {
            if (entry == null || entry.Key.IsNullOrWhiteSpace())
            {
                return null;
            }

            if (this.TryGetProvider(providerName, out ICacheProvider foundProvider))
            {
                return foundProvider.Get(entry);
            }

            foreach (KeyValuePair<string, ICacheProvider> provider in this.Providers)
            {
                if (provider.Value.Contains(entry))
                {
                    return provider.Value.Get(entry);
                }
            }

            return null;
        }

        /// <inheritdoc />
        public IReadOnlyCollection<string> GetKeys(string providerName = null)
        {
            if (this.TryGetProvider(providerName, out ICacheProvider foundProvider))
            {
                return foundProvider.GetKeys();
            }
#if NET35 || NET40
            var keys = new ReadOnlyListWrapper<string>();
#else
            var keys = new List<string>();
#endif

            foreach (KeyValuePair<string, ICacheProvider> provider in this.Providers)
            {
                IEnumerable<string> providerKeys = provider.Value.GetKeys();
                keys.AddRange(providerKeys);
            }

            return keys;
        }

        /// <inheritdoc />
        public void Set(CacheEntry entry, string providerName = null)
        {
            if (entry == null || entry.Key.IsNullOrWhiteSpace() || this.Providers.Count == 0)
            {
                return;
            }

            if (this.TryGetProvider(providerName, out ICacheProvider foundProvider))
            {
                foundProvider.Set(entry);
            }
            else
            {
                this.Providers.First().Value.Set(entry);
            }
        }

        /// <inheritdoc />
        public object Remove(CacheEntry entry, string providerName = null)
        {
            if (entry == null || entry.Key.IsNullOrWhiteSpace())
            {
                return null;
            }

            if (this.TryGetProvider(providerName, out ICacheProvider foundProvider))
            {
                return foundProvider.Remove(entry);
            }

            object result = null;
            foreach (KeyValuePair<string, ICacheProvider> provider in this.Providers)
            {
                object tempResult = provider.Value.Remove(entry);
                if (tempResult != null)
                {
                    result = tempResult;
                }
            }

            return result;
        }

        #endregion Methods

        #region Helper Methods

        /// <summary>
        /// Gets the cache provider if only 1 provider is available or if a
        /// provider name is provided that matches an available provider.
        /// </summary>
        /// <param name="providerName">Name of the provider.</param>
        /// <param name="provider">The provider instance if found.</param>
        /// <returns>
        /// The if a single cache provider is found.
        /// </returns>
        private bool TryGetProvider(string providerName, out ICacheProvider provider)
        {
            if (this.Providers.Count == 1)
            {
                provider = this.Providers.First().Value;
                return true;
            }

            if (!providerName.IsNullOrWhiteSpace() &&
                this.Providers.TryGetValue(providerName, out ICacheProvider matchingProvider))
            {
                provider = matchingProvider;
                return true;
            }

            provider = null;
            return false;
        }

        #endregion Helper Methods
    }
}