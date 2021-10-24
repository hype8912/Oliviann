namespace Oliviann.Configuration.Providers
{
    #region Usings

    using System.Collections.Generic;

    #endregion Usings

    /// <summary>
    /// Represents a base class for a configuration provider implementation.
    /// </summary>
    public abstract class ConfigurationProvider : IConfigurationProvider
    {
        #region Constructor/Destructor

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="ConfigurationProvider" /> class.
        /// </summary>
        /// <param name="name">The unique name for the provider.</param>
        protected ConfigurationProvider(string name)
        {
            this.Name = name;
        }

        #endregion Constructor/Destructor

        #region Properties

        /// <inheritdoc />
        public string Name { get; protected set; }

        #endregion Properties

        #region Methods

        /// <inheritdoc />
        public abstract IEnumerable<string> GetChildKeys();

        /// <inheritdoc />
        public abstract void Load();

        /// <inheritdoc />
        public virtual void Set(string key, string value)
        {
            ADP.CheckArgumentNullOrEmpty(key, nameof(key));
            this.SetInt(key, value);
        }

        /// <inheritdoc />
        public virtual bool TryGet<T>(string key, out T value)
        {
            ADP.CheckArgumentNullOrEmpty(key, nameof(key));
            return this.TryGetInt(key, out value);
        }

        #endregion Methods

        #region Helper Methods

        /// <summary>
        /// Sets the value for the specified key.
        /// </summary>
        /// <param name="key">The unique key.</param>
        /// <param name="value">The associated value.</param>
        protected abstract void SetInt(string key, string value);

        /// <summary>
        /// Attempts to get the value for the specified key.
        /// </summary>
        /// <typeparam name="T">The type of value to be returned.</typeparam>
        /// <param name="key">The unique provider key.</param>
        /// <param name="value">The value.</param>
        /// <returns>The value for the specified key; otherwise; default value
        /// of the T.</returns>
        protected abstract bool TryGetInt<T>(string key, out T value);

        #endregion Helper Methods
    }
}