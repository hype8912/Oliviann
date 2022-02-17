#if NETFRAMEWORK

namespace Oliviann.Configuration
{
    #region Usings

    using Oliviann.Configuration.Providers;

    #endregion Usings

    /// <summary>
    /// Represents a collection of commonly used extension methods for extending
    /// <see cref="ConfigurationBuilder"/>.
    /// </summary>
    public static class ConfigurationBuilderExtensions
    {
        /// <summary>
        /// Adds the cookie provider to the specified configuration
        /// builder instance.
        /// </summary>
        /// <param name="builder">The configuration builder instance.</param>
        /// <param name="cookieName">The unique name of the cookie.</param>
        /// <returns>The current configuration builder instance.</returns>
        public static ConfigurationBuilder AddCookie(this ConfigurationBuilder builder, string cookieName)
        {
            ADP.CheckArgumentNull(builder, nameof(builder));
            builder.AddProvider(new CookieConfigurationProvider(cookieName));

            return builder;
        }

        /// <summary>
        /// Adds the session provider to the specified configuration builder
        /// instance.
        /// </summary>
        /// <param name="builder">The configuration builder instance.</param>
        /// <returns>The current configuration builder instance.</returns>
        public static ConfigurationBuilder AddSession(this ConfigurationBuilder builder)
        {
            ADP.CheckArgumentNull(builder, nameof(builder));
            builder.AddProvider(new SessionConfigurationProvider());

            return builder;
        }
    }
}

#endif