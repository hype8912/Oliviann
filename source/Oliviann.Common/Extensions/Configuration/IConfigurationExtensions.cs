#if !NETFRAMEWORK
namespace Oliviann.Extensions.Configuration
{
    #region Usings

    using Microsoft.Extensions.Configuration;

    #endregion Usings

    /// <summary>
    /// Represents a collection of commonly used extension methods for extending
    /// <see cref="IConfiguration"/> objects.
    /// </summary>
    public static class IConfigurationExtensions
    {
        /// <summary>
        /// Gets the application setting. Shorthand for
        /// <c>GetSection("AppSettings")[key]</c>.
        /// </summary>
        /// <param name="configuration">The configuration instance.</param>
        /// <param name="key">The application setting key.</param>
        /// <returns>The application setting value for the specified key;
        /// otherwise, null.</returns>
        public static string GetAppSetting(this IConfiguration configuration, string key)
        {
            ADP.CheckArgumentNull(configuration, nameof(configuration));
            return configuration.GetSection("AppSettings")[key];
        }
    }
}
#endif