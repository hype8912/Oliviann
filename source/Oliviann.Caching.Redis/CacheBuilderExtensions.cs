namespace Oliviann.Caching
{
    #region Usings

    using Oliviann.Caching.Redis;

    #endregion Usings

    /// <summary>
    /// Represents a collection of commonly used extension methods for extending
    /// <see cref="CacheBuilder"/>.
    /// </summary>
    public static class CacheBuilderExtensions
    {
        /// <summary>
        /// Adds the <c>Redis</c> cache provider to the specified cache builder
        /// instance.
        /// </summary>
        /// <param name="builder">The cache builder instance.</param>
        /// <param name="connectionString">The provider connection string.
        /// </param>
        /// <param name="providerName">Optional. Name of the provider.</param>
        /// <returns>The current cache builder instance.</returns>
        public static CacheBuilder AddRedisCache(this CacheBuilder builder, string connectionString, string providerName = null)
        {
            ADP.CheckArgumentNull(builder, nameof(builder));
            var options = new RedisOptions { ConnectionString = connectionString };

            builder.AddProvider(
                providerName.IsNullOrWhiteSpace()
                    ? new RedisCacheProvider(options)
                    : new RedisCacheProvider(providerName, options));

            return builder;
        }
    }
}