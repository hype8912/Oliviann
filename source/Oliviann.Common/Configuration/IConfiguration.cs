namespace Oliviann.Configuration
{
    /// <summary>
    /// Represents an application configuration implementation.
    /// </summary>
    public interface IConfiguration
    {
        #region Methods

        /// <summary>
        /// Gets the value for the specified key.
        /// </summary>
        /// <typeparam name="T">The type of instance to be returned.</typeparam>
        /// <param name="key">The unique key.</param>
        /// <param name="providerName">Optional. Name of the provider.</param>
        /// <returns>
        /// The value for the specified key; otherwise; default value
        /// of the T.
        /// </returns>
        T Get<T>(string key, string providerName = null) where T : class;

        #endregion Methods
    }
}