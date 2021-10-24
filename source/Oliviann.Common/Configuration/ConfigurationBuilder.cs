namespace Oliviann.Configuration
{
    #region Usings

    using System;
    using System.Collections.Generic;
    using Oliviann.Collections.Generic;
    using Oliviann.Configuration.Providers;
    using Oliviann.Properties;

    #endregion Usings

    /// <summary>
    /// Represents a configuration builder for adding providers.
    /// </summary>
    public class ConfigurationBuilder : IConfiguration
    {
        #region Constructor/Destructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationBuilder"/>
        /// class.
        /// </summary>
        public ConfigurationBuilder()
        {
#if NETSTANDARD1_3
            this.Providers = new Dictionary<string, IConfigurationProvider>(StringComparer.CurrentCultureIgnoreCase);
#else
            this.Providers = new Dictionary<string, IConfigurationProvider>(StringComparer.InvariantCultureIgnoreCase);
#endif
        }

        #endregion Constructor/Destructor

        #region Properties

        /// <summary>
        /// Gets the collection of configuration providers.
        /// </summary>
        /// <value>
        /// The collection of configuration providers.
        /// </value>
        private Dictionary<string, IConfigurationProvider> Providers { get; }

        #endregion Properties

        #region Methods

        // --INI Provider
        // Steeltoe Configuration Provider
        // --Environment Variables
        // --ConfigurationManager
        // --Memory
        // --Registry
        // --Custom Provider
        // --Session
        // --Cookies

        /// <summary>
        /// Adds the provider to the collection of configuration providers.
        /// </summary>
        /// <param name="provider">The provider instance to be added.</param>
        public void AddProvider(IConfigurationProvider provider)
        {
            ADP.CheckArgumentNull(provider, nameof(provider));

            if (provider.Name.IsNullOrWhiteSpace())
            {
                throw ADP.ArgumentEx("provider.Name", Resources.ERR_NullOrEmpty.FormatWith("Provider unique name"));
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

        /// <summary>
        /// Builds this instance after all the configuration providers have been
        /// added to the builder.
        /// </summary>
        /// <remarks>This must be called if any providers were added that need
        /// to load resources into memory.</remarks>
        public void Build()
        {
            this.Providers.ForEach(provider => provider.Value.Load());
        }

        /// <summary>
        /// Gets the value for the specified key.
        /// </summary>
        /// <typeparam name="T">The type of instance to be returned.</typeparam>
        /// <param name="key">The unique key.</param>
        /// <param name="providerName">Optional. Name of the provider.</param>
        /// <returns>
        /// The value for the specified key; otherwise; default value of the T.
        /// </returns>
        public T Get<T>(string key, string providerName = null) where T : class
        {
            ADP.CheckArgumentNullOrEmpty(key, nameof(key));

            T value;
            if (!providerName.IsNullOrWhiteSpace())
            {
                if (this.Providers.TryGetValue(providerName, out IConfigurationProvider provider) &&
                    provider.TryGet(key, out value))
                {
                    return value;
                }
            }

            // Loops through the providers trying to find the first provider
            // that matches the key.
            foreach (KeyValuePair<string, IConfigurationProvider> provider in this.Providers)
            {
                if (provider.Value.TryGet(key, out value))
                {
                    return value;
                }
            }

            return default(T);
        }

        #endregion Methods
    }
}