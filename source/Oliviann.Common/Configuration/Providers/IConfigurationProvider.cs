namespace Oliviann.Configuration.Providers
{
    #region Usings

    using System.Collections.Generic;

    #endregion Usings

    /// <summary>
    /// Represents a single application configuration provider implementation.
    /// </summary>
    public interface IConfigurationProvider
    {
        #region Properties

        /// <summary>
        /// Gets the unique name of the provider.
        /// </summary>
        /// <value>
        /// The unique provider name.
        /// </value>
        string Name { get; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Gets all the child keys.
        /// </summary>
        /// <returns>A collection of child keys available.</returns>
        IEnumerable<string> GetChildKeys();

        /// <summary>
        /// Loads this instance.
        /// </summary>
        void Load();

        /// <summary>
        /// Sets the value for the specified key.
        /// </summary>
        /// <param name="key">The unique key.</param>
        /// <param name="value">The associated value.</param>
        void Set(string key, string value);

        /// <summary>
        /// Attempts to get the value for the specified key.
        /// </summary>
        /// <typeparam name="T">The type of value to be returned.</typeparam>
        /// <param name="key">The unique provider key.</param>
        /// <param name="value">The value.</param>
        /// <returns>The value for the specified key; otherwise; default value
        /// of the T.</returns>
        bool TryGet<T>(string key, out T value);

        #endregion Methods
    }
}