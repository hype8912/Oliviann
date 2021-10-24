#if !NET35

namespace Oliviann.Caching
{
    #region Usings

    using Oliviann.Caching.Providers;

    #endregion Usings

    /// <summary>
    /// Represents a collection of commonly used extension methods for extending
    /// <see cref="CacheBuilder"/>.
    /// </summary>
    public static class CacheBuilderExtensions
    {
        /// <summary>
        /// Adds the memory cache provider to the specified cache builder
        /// instance.
        /// </summary>
        /// <param name="builder">The cache builder instance.</param>
        /// <param name="providerName">Optional. Name of the provider.</param>
        /// <returns>The current cache builder instance.</returns>
        public static CacheBuilder AddMemoryCache(this CacheBuilder builder, string providerName = null)
        {
            ADP.CheckArgumentNull(builder, nameof(builder));
            builder.AddProvider(
                providerName.IsNullOrWhiteSpace() ? new MemoryCacheProvider() : new MemoryCacheProvider(providerName));

            return builder;
        }
    }
}

#endif