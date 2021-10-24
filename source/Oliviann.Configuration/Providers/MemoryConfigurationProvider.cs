namespace Oliviann.Configuration.Providers
{
    #region Usings

    using System.Collections.Generic;

    #endregion Usings

    /// <summary>
    /// Represents an in memory configuration provider.
    /// </summary>
    public class MemoryConfigurationProvider : ConfigurationProvider
    {
        #region Fields

        /// <summary>
        /// The memory settings instance.
        /// </summary>
        private readonly IDictionary<string, string> settings;

        #endregion Fields

        #region Constructor/Destructor

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="MemoryConfigurationProvider" /> class.
        /// </summary>
        /// <param name="name">The unique provider name.</param>
        /// <param name="providerSettings">The provider settings.</param>
        public MemoryConfigurationProvider(string name, IDictionary<string, string> providerSettings) : base(name)
        {
            ADP.CheckArgumentNullOrEmpty(name, nameof(name));
            ADP.CheckArgumentNull(providerSettings, nameof(providerSettings));

            this.settings = providerSettings;
        }

        #endregion Constructor/Destructor

        #region Methods

        /// <inheritdoc />
        public override IEnumerable<string> GetChildKeys() => this.settings.Keys;

        /// <inheritdoc />
        public override void Load()
        {
            // Not used. No need for this since the dictionary is passed in.
        }

        /// <inheritdoc />
        protected override void SetInt(string key, string value)
        {
            if (this.settings.ContainsKey(key))
            {
                this.settings[key] = value;
            }
            else
            {
                this.settings.Add(key, value);
            }
        }

        /// <inheritdoc />
        protected override bool TryGetInt<T>(string key, out T value)
        {
            if (this.settings.ContainsKey(key))
            {
                this.settings[key].TryCast(out value);
                return true;
            }

            value = default(T);
            return false;
        }

        #endregion Methods
    }
}