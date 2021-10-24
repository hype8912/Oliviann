namespace Oliviann.Configuration
{
    /// <summary>
    /// Represents a collection of commonly used extension methods for extending
    /// <see cref="IConfiguration"/>.
    /// </summary>
    public static class IConfigurationExtensions
    {
        #region Methods

        /// <summary>
        /// Gets the value for the specified key.
        /// </summary>
        /// <param name="configuration">The configuration instance.</param>
        /// <param name="key">The unique key.</param>
        /// <param name="providerName">Optional. Name of the provider.</param>
        /// <returns>
        /// The value for the specified key; otherwise; null.
        /// </returns>
        public static string Get(this IConfiguration configuration, string key, string providerName = null)
        {
            ADP.CheckArgumentNull(configuration, nameof(configuration));
            return configuration.Get<string>(key, providerName);
        }

        /// <summary>
        /// Gets the application setting. Shorthand for
        /// <c>Get("AppSettings:{key}")</c>.
        /// </summary>
        /// <param name="configuration">The configuration instance.</param>
        /// <param name="key">The application setting key.</param>
        /// <param name="providerName">Optional. Name of the provider.</param>
        /// <returns>The application setting value for the specified key;
        /// otherwise, null.</returns>
        public static string GetAppSetting(this IConfiguration configuration, string key, string providerName = null)
        {
            ADP.CheckArgumentNull(configuration, nameof(configuration));
            return configuration.Get<string>("AppSettings:" + key, providerName);
        }

        /// <summary>
        /// Gets the connection string. Shorthand for
        /// <c>Get("ConnectionStrings:{key}")</c>.
        /// </summary>
        /// <param name="configuration">The configuration instance.</param>
        /// <param name="key">The connection string key.</param>
        /// <param name="providerName">Optional. Name of the provider.</param>
        /// <returns>The connection string value for the specified key;
        /// otherwise, null.</returns>
        public static string GetConnectionString(this IConfiguration configuration, string key, string providerName = null)
        {
            ADP.CheckArgumentNull(configuration, nameof(configuration));
            return configuration.Get<string>("ConnectionStrings:" + key, providerName);
        }

        #endregion Methods
    }
}