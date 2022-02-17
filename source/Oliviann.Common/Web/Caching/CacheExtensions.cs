#if NETFRAMEWORK

namespace Oliviann.Web.Caching
{
    #region Usings

    using System;
    using System.Web.Caching;
    using Oliviann.Caching;

    #endregion Usings

    /// <summary>
    /// Represents a collection of commonly used extension methods for extending
    /// <see cref="Cache"/>.
    /// </summary>
    public static class CacheExtensions
    {
        /// <summary>
        /// Converts the cache instance to an ICache instance.
        /// </summary>
        /// <param name="cache">The cache instance.</param>
        /// <returns>A new ICache implementation for the specified instance.
        /// </returns>
        public static ICache AsICache(this Cache cache) => new CacheProxy(cache);
    }
}

#endif