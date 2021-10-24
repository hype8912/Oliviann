namespace Oliviann.Configuration
{
    #region Usings

    using System.Collections.Generic;
    using Oliviann.Configuration.Providers;

    #endregion Usings

    /// <summary>
    /// Represents a collection of commonly used extension methods for extending
    /// <see cref="ConfigurationBuilder"/>.
    /// </summary>
    public static class ConfigurationBuilderExtensions
    {
        #region Methods

#if NETFRAMEWORK

        /// <summary>
        /// Adds the configuration manager provider to the specified
        /// configuration builder instance.
        /// </summary>
        /// <param name="builder">The configuration builder instance.</param>
        /// <returns>The current configuration builder instance.</returns>
        public static ConfigurationBuilder AddConfigurationManager(this ConfigurationBuilder builder)
        {
            ADP.CheckArgumentNull(builder, nameof(builder));
            builder.AddProvider(new ConfigurationManagerConfigurationProvider());

            return builder;
        }

#endif

        /// <summary>
        /// Adds the environment variables provider to the specified
        /// configuration builder instance.
        /// </summary>
        /// <param name="builder">The configuration builder instance.</param>
        /// <returns>The current configuration builder instance.</returns>
        public static ConfigurationBuilder AddEnvironmentVariables(this ConfigurationBuilder builder)
        {
            ADP.CheckArgumentNull(builder, nameof(builder));
            builder.AddProvider(new EnvironmentVariablesConfigurationProvider());

            return builder;
        }

        /// <summary>
        /// Adds the registry provider to the specified configuration builder
        /// instance.
        /// </summary>
        /// <param name="builder">The configuration builder instance.</param>
        /// <returns>The current configuration builder instance.</returns>
        public static ConfigurationBuilder AddRegistry(this ConfigurationBuilder builder)
        {
            ADP.CheckArgumentNull(builder, nameof(builder));
            builder.AddProvider(new RegistryConfigurationProvider());

            return builder;
        }

        /// <summary>
        /// Adds a named in memory collection to the specified configuration
        /// builder instance.
        /// </summary>
        /// <param name="builder">The configuration builder instance.</param>
        /// <param name="providerName">A unique name for the provider.</param>
        /// <param name="collection">The collection to be added.</param>
        /// <returns>The current configuration builder instance.</returns>
        public static ConfigurationBuilder AddInMemoryCollection(
            this ConfigurationBuilder builder,
            string providerName,
            IDictionary<string, string> collection)
        {
            ADP.CheckArgumentNull(builder, nameof(builder));
            builder.AddProvider(new MemoryConfigurationProvider(providerName, collection));

            return builder;
        }

        #endregion Methods
    }
}