namespace Oliviann.Tests.Caching
{
    #region Usings

    using System;
    using System.Runtime.Caching;
    using Oliviann.Caching;
    using Xunit;

    #endregion Usings

    [Trait("Category", "CI")]
    public class CacheBuilderExtensionsTests
    {
        /// <summary>
        /// Verifies an exception is thrown when a null builder is passed.
        /// </summary>
        [Fact]
        public void AddMemoryCacheTest_NullBuilder()
        {
            CacheBuilder builder = null;
            Assert.Throws<ArgumentNullException>(() => builder.AddMemoryCache());
        }

        /// <summary>
        /// Verifies a default memory cache provider is added to the builder.
        /// </summary>
        [Fact]
        public void AddMemoryCacheTest_DefaultProvider()
        {
            var builder = new CacheBuilder();
            builder.AddMemoryCache();

            MemoryCache.Default.Add("Whata", "Burger", DateTimeOffset.MaxValue);
            Assert.True(builder.Contains(new CacheEntry("Whata")));
        }

        /// <summary>
        /// Verifies a named memory cache provider is added to the builder.
        /// </summary>
        [Fact]
        public void AddMemoryCacheTest_NamedProvider()
        {
            var builder = new CacheBuilder();
            builder.AddMemoryCache("Restaurants");

            MemoryCache.Default.Add("Whata", "Burger", DateTimeOffset.MaxValue);
            Assert.False(builder.Contains(new CacheEntry("Whata")));
        }
    }
}