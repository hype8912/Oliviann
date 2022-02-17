namespace Oliviann.Caching
{
    #region Usings

    using Oliviann.Caching.EnterpriseLibrary;

    #endregion Usings

    /// <summary>
    /// Represents a collection of commonly used extension methods for extending
    /// <see cref="CacheBuilder"/>.
    /// </summary>
    public static class CacheBuilderExtensions
    {
        /// <summary>
        /// Adds the enterprise library cache provider to the specified cache
        /// builder instance.
        /// </summary>
        /// <param name="builder">The cache builder instance.</param>
        /// <param name="providerName">Optional. Name of the provider.</param>
        /// <returns>The current cache builder instance.</returns>
        public static CacheBuilder AddEnterpriseLibraryCache(this CacheBuilder builder, string providerName = null)
        {
            ADP.CheckArgumentNull(builder, nameof(builder));
            builder.AddProvider(
                providerName.IsNullOrWhiteSpace()
                    ? new EnterpriseLibraryCacheProvider()
                    : new EnterpriseLibraryCacheProvider(providerName));

            return builder;
        }
    }
}