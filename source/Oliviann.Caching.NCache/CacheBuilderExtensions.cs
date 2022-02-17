namespace Oliviann.Caching
{
    #region Usings

    using System;
    using Oliviann.Caching.NCache;

    #endregion Usings

    /// <summary>
    /// Represents a collection of commonly used extension methods for extending
    /// <see cref="CacheBuilder"/>.
    /// </summary>
    public static class CacheBuilderExtensions
    {
        /// <summary>
        /// Adds the <c>NCache</c> cache provider to the specified cache
        /// builder.
        /// </summary>
        /// <param name="builder">The cache builder instance.</param>
        /// <param name="setOptions">The action for setting the cache options.
        /// </param>
        /// <param name="providerName">Optional. Name of the provider. The
        /// provider name is used as the cache id.</param>
        /// <returns>The current cache builder instance.</returns>
        public static CacheBuilder AddNCache(
            this CacheBuilder builder,
            Action<CacheOptions> setOptions,
            string providerName = null)
        {
            ADP.CheckArgumentNull(builder, nameof(builder));
            ADP.CheckArgumentNull(setOptions, nameof(setOptions));

            var options = new CacheOptions();
            setOptions(options);

            builder.AddProvider(
                providerName.IsNullOrEmpty()
                    ? new NCacheProvider { CacheOptions = options }
                    : new NCacheProvider(providerName) { CacheOptions = options });

            return builder;
        }
    }
}