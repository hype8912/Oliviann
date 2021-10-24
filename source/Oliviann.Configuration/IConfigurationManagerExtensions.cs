namespace Oliviann.Configuration
{
    /// <summary>
    /// Represents a collection of extension methods for extending
    /// <see cref="IConfigurationManager"/>.
    /// </summary>
    public static class IConfigurationManagerExtensions
    {
        /// <summary>
        /// Gets the application setting. Shorthand for <c>AppSettings[key]</c>.
        /// </summary>
        /// <param name="manager">The configuration manager instance.</param>
        /// <param name="key">The application setting key.</param>
        /// <returns>The application setting value for the specified key;
        /// otherwise, null.</returns>
        public static string GetAppSetting(this IConfigurationManager manager, string key)
        {
            ADP.CheckArgumentNull(manager, nameof(manager));
            return manager.AppSettings[key];
        }
    }
}