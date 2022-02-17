namespace Oliviann.Tests.Caching
{
    #region Usings

    using System;
    using Oliviann.Caching;
    using Xunit;

    #endregion Usings

    [Trait("Category", "CI")]
    public class CacheEntryTests
    {
        /// <summary>
        /// Verifies the default instance values.
        /// </summary>
        [Fact]
        public void CacheEntryTest_DefaultValues()
        {
            var entry = new CacheEntry(null);

            Assert.Equal(DateTimeOffset.MaxValue, entry.AbsoluteExpiration);
            Assert.Null(entry.Key);
            Assert.Equal(TimeSpan.Zero, entry.SlidingExpiration);
            Assert.Equal(CacheTarget.Default, entry.Target);
            Assert.Null(entry.Value);
        }

        /// <summary>
        /// Verifies the default instance values when setting the key.
        /// </summary>
        [Fact]
        public void CacheEntryTest_SetKeyConstructor()
        {
            var entry = new CacheEntry("Oliviann");

            Assert.Equal(DateTimeOffset.MaxValue, entry.AbsoluteExpiration);
            Assert.Equal("Oliviann", entry.Key);
            Assert.Equal(TimeSpan.Zero, entry.SlidingExpiration);
            Assert.Equal(CacheTarget.Default, entry.Target);
            Assert.Null(entry.Value);
        }

        /// <summary>
        /// Verifies the default instance values when setting the key and value.
        /// </summary>
        [Fact]
        public void CacheEntryTest_SetKeyValueConstructor()
        {
            var entry = new CacheEntry("Oliviann", "F-15C");

            Assert.Equal(DateTimeOffset.MaxValue, entry.AbsoluteExpiration);
            Assert.Equal("Oliviann", entry.Key);
            Assert.Equal(TimeSpan.Zero, entry.SlidingExpiration);
            Assert.Equal(CacheTarget.Default, entry.Target);
            Assert.Equal("F-15C", (string)entry.Value);
        }

        /// <summary>
        /// Verifies the property values after being set.
        /// </summary>
        [Fact]
        public void CacheEntryTest_SetProperties()
        {
            DateTimeOffset absExpiration = DateTimeOffset.UtcNow;
            TimeSpan slide = TimeSpan.FromSeconds(30);
            var entry = new CacheEntry("Oliviann", "F-15C")
                            {
                                AbsoluteExpiration = absExpiration,
                                SlidingExpiration = slide,
                                Target = CacheTarget.Grid
                            };

            Assert.Equal(absExpiration, entry.AbsoluteExpiration);
            Assert.Equal("Oliviann", entry.Key);
            Assert.Equal(slide, entry.SlidingExpiration);
            Assert.Equal(CacheTarget.Grid, entry.Target);
            Assert.Equal("F-15C", (string)entry.Value);
        }
    }
}