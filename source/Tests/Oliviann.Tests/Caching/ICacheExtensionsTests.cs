namespace Oliviann.Tests.Caching
{
    #region Usings

    using System;
    using System.Collections.Generic;
    using Oliviann.Caching;
    using Moq;
    using Xunit;

    #endregion Usings

    [Trait("Category", "CI")]
    public class ICacheExtensionsTests
    {
        #region Properties

        public static IEnumerable<object[]> KeyValueData
            => new[] { new object[] { null, null }, new object[] { "Oliviann", null }, new object[] { "Oliviann", "F-15C" }, };

        public static IEnumerable<object[]> KeyValueExpireData
            =>
            new[]
                {
                    new object[] { null, null, DateTimeOffset.MinValue },
                    new object[] { "Oliviann", null, DateTimeOffset.MaxValue },
                    new object[] { "Oliviann", "F-15C", DateTimeOffset.UtcNow },
                };

        public static IEnumerable<object[]> KeyValueSpanData
            =>
            new[]
                {
                    new object[] { null, null, TimeSpan.MinValue },
                    new object[] { "Oliviann", null, TimeSpan.MaxValue },
                    new object[] { "Oliviann", "F-15C", TimeSpan.FromDays(1) },
                };

        #endregion Properties

        #region Add Tests

        /// <summary>
        /// Verifies the cache entry values are created correctly when passed to
        /// the ICache instance.
        /// </summary>
        [Theory]
        [MemberData(nameof(KeyValueData))]
        public void AddCacheTest_KeyValue(string key, object value)
        {
            CacheEntry entry = null;
            var mocCache = new Mock<ICache>();
            mocCache.Setup(c => c.Add(It.IsAny<CacheEntry>(), It.IsAny<string>()))
                .Returns(true)
                .Callback<CacheEntry, string>((e, p) => { entry = e; });
            mocCache.Object.Add(key, value, null);

            Assert.Equal(key, entry.Key);
            Assert.Equal(value, entry.Value);
            Assert.Equal(DateTimeOffset.MaxValue, entry.AbsoluteExpiration);
            Assert.Equal(TimeSpan.Zero, entry.SlidingExpiration);
            Assert.Equal(CacheTarget.Default, entry.Target);
        }

        /// <summary>
        /// Verifies the cache entry values are created correctly when passed to
        /// the ICache instance.
        /// </summary>
        [Theory]
        [MemberData(nameof(KeyValueExpireData))]
        public void AddCacheTest_KeyValueExpire(string key, object value, DateTimeOffset expiration)
        {
            CacheEntry entry = null;
            var mocCache = new Mock<ICache>();
            mocCache.Setup(c => c.Add(It.IsAny<CacheEntry>(), It.IsAny<string>()))
                .Returns(true)
                .Callback<CacheEntry, string>((e, p) => { entry = e; });
            mocCache.Object.Add(key, value, expiration);

            Assert.Equal(key, entry.Key);
            Assert.Equal(value, entry.Value);
            Assert.Equal(expiration, entry.AbsoluteExpiration);
            Assert.Equal(TimeSpan.Zero, entry.SlidingExpiration);
            Assert.Equal(CacheTarget.Default, entry.Target);
        }

        /// <summary>
        /// Verifies the cache entry values are created correctly when passed to
        /// the ICache instance.
        /// </summary>
        [Theory]
        [MemberData(nameof(KeyValueSpanData))]
        public void AddCacheTest_KeyValueSpan(string key, object value, TimeSpan expiration)
        {
            CacheEntry entry = null;
            var mocCache = new Mock<ICache>();
            mocCache.Setup(c => c.Add(It.IsAny<CacheEntry>(), It.IsAny<string>()))
                .Returns(true)
                .Callback<CacheEntry, string>((e, p) => { entry = e; });
            mocCache.Object.Add(key, value, expiration);

            Assert.Equal(key, entry.Key);
            Assert.Equal(value, entry.Value);
            Assert.Equal(DateTimeOffset.MaxValue, entry.AbsoluteExpiration);
            Assert.Equal(expiration, entry.SlidingExpiration);
            Assert.Equal(CacheTarget.Default, entry.Target);
        }

        #endregion Add Tests

        #region AddOrGetExisting Tests

        /// <summary>
        /// Verifies an exception is thrown for a null cache instance.
        /// </summary>
        [Fact]
        public void AddOrGetExistingTest_NullCache()
        {
            ICache cache = null;
            Func<string> del = () => "result";

            Assert.Throws<ArgumentNullException>(() => cache.AddOrGetExisting(new CacheEntry("key"), del));
        }

        /// <summary>
        /// Verifies an exception is thrown for a null cache entry instance.
        /// </summary>
        [Fact]
        public void AddOrGetExistingTest_NullCacheEntry()
        {
            CacheEntry entry = null;
            Func<string> del = () => "result";

            var mocCache = new Mock<ICache>();
            mocCache.Setup(c => c.Add(It.IsAny<CacheEntry>(), It.IsAny<string>())).Returns(true);

            Assert.Throws<ArgumentNullException>(() => mocCache.Object.AddOrGetExisting(entry, del));
        }

        /// <summary>
        /// Verifies an entry returns the correct value if it's already in the
        /// cache.
        /// </summary>
        [Fact]
        public void AddOrGetExistingTest_CacheContainsGetItem()
        {
            var entry = new CacheEntry("Hello");
            Action<CacheEntry, string> keyValueValidator = (e, p) =>
            {
                Assert.Equal(entry.Key, e.Key);
                Assert.Equal(entry.Target, e.Target);
            };

            var mocCache = new Mock<ICache>();
            mocCache.Setup(c => c.Contains(It.IsAny<CacheEntry>(), It.IsAny<string>()))
                .Returns(true)
                .Callback(keyValueValidator);

            mocCache.Setup(c => c.Get(It.IsAny<CacheEntry>(), It.IsAny<string>()))
                .Returns("World")
                .Callback(keyValueValidator);

            string result = mocCache.Object.AddOrGetExisting<string>(entry, null);
            Assert.Equal("World", result);
        }

        /// <summary>
        /// Verifies an exception is thrown when a null delegate is passed in.
        /// </summary>
        [Fact]
        public void AddOrGetExistingTest_NullDelegate()
        {
            var entry = new CacheEntry("Hello");
            var mocCache = new Mock<ICache>();
            mocCache.Setup(c => c.Contains(It.IsAny<CacheEntry>(), It.IsAny<string>()))
                .Returns(false)
                .Callback<CacheEntry, string>(
                (e, p) =>
                {
                    Assert.Equal(entry.Key, e.Key);
                    Assert.Equal(entry.Target, e.Target);
                });

            Assert.Throws<ArgumentNullException>(() => mocCache.Object.AddOrGetExisting<string>(entry, null));
        }

        /// <summary>
        /// Verifies the cache add isn't called with the result value from the
        /// getter delegate is null.
        /// </summary>
        [Fact]
        public void AddOrGetExistingTest_DelegateNullResult()
        {
            var entry = new CacheEntry("Hello");
            var mocCache = new Mock<ICache>();
            mocCache.Setup(c => c.Contains(It.IsAny<CacheEntry>(), It.IsAny<string>()))
                .Returns(false)
                .Callback<CacheEntry, string>(
                    (e, p) =>
                    {
                        Assert.Equal(entry.Key, e.Key);
                        Assert.Equal(entry.Target, e.Target);
                    });

            bool addCalled = false;
            mocCache.Setup(c => c.Add(It.IsAny<CacheEntry>(), It.IsAny<string>()))
                .Returns(false)
                .Callback<CacheEntry, string>((e, p) => addCalled = true);

            Func<string> del = () => null;
            string result = mocCache.Object.AddOrGetExisting(entry, del);

            Assert.Null(result);
            Assert.False(addCalled);
        }

        /// <summary>
        /// Verifies a non-null result from the delegate function is added to
        /// the cache and the result value is returned correctly.
        /// </summary>
        [Fact]
        public void AddOrGetExistingTest_DelegateNonNullResult()
        {
            var entry = new CacheEntry("Hello");
            var mocCache = new Mock<ICache>();
            mocCache.Setup(c => c.Contains(It.IsAny<CacheEntry>(), It.IsAny<string>()))
                .Returns(false)
                .Callback<CacheEntry, string>(
                    (e, p) =>
                    {
                        Assert.Equal(entry.Key, e.Key);
                        Assert.Equal(entry.Target, e.Target);
                    });

            mocCache.Setup(c => c.Add(It.IsAny<CacheEntry>(), It.IsAny<string>()))
                .Returns(true)
                .Callback<CacheEntry, string>(
                    (e, p) =>
                    {
                        Assert.Equal(entry.Key, e.Key);
                        Assert.Equal(entry.Target, e.Target);
                        Assert.Equal("World", e.Value);
                    });

            Func<string> del = () => "World";
            string result = mocCache.Object.AddOrGetExisting(entry, del);

            Assert.Equal("World", result);
        }

        /// <summary>
        /// Verifies a non-null result from the delegate function is added to
        /// the cache and the result value is returned correctly.
        /// </summary>
        [Fact]
        public void AddOrGetExistingTest_KeyOnlyDelegateNonNullResult()
        {
            var entry = new CacheEntry("Hello");
            var mocCache = new Mock<ICache>();
            mocCache.Setup(c => c.Contains(It.IsAny<CacheEntry>(), It.IsAny<string>()))
                .Returns(false)
                .Callback<CacheEntry, string>(
                    (e, p) =>
                    {
                        Assert.Equal(entry.Key, e.Key);
                        Assert.Equal(entry.Target, e.Target);
                    });

            mocCache.Setup(c => c.Add(It.IsAny<CacheEntry>(), It.IsAny<string>()))
                .Returns(true)
                .Callback<CacheEntry, string>(
                    (e, p) =>
                    {
                        Assert.Equal(entry.Key, e.Key);
                        Assert.Equal(entry.Target, e.Target);
                        Assert.Equal("World", e.Value);
                    });

            Func<string> del = () => "World";
            string result = mocCache.Object.AddOrGetExisting(entry.Key, del);

            Assert.Equal("World", result);
        }

        #endregion AddOrGetExisting Tests

        #region Contains Tests

        /// <summary>
        /// Verifies a null cache returns false.
        /// </summary>
        [Fact]
        public void ContainsCacheTest_NullCache()
        {
            ICache cache = null;
            bool result = cache.Contains("key");

            Assert.False(result);
        }

        /// <summary>
        /// Verifies a valid cache hit returns true.
        /// </summary>
        [Fact]
        public void ContainsCacheTest_CacheHit()
        {
            CacheEntry entry = null;
            var mocCache = new Mock<ICache>();
            mocCache.Setup(c => c.Contains(It.IsAny<CacheEntry>(), It.IsAny<string>()))
                .Returns(true)
                .Callback<CacheEntry, string>((e, p) => { entry = e; });
            bool result = mocCache.Object.Contains("hello");

            Assert.True(result);
            Assert.Equal("hello", entry.Key);
            Assert.Equal(CacheTarget.Default, entry.Target);
        }

        /// <summary>
        /// Verifies a valid cache hit returns true.
        /// </summary>
        [Fact]
        public void ContainsCacheTest_CacheMiss()
        {
            CacheEntry entry = null;
            var mocCache = new Mock<ICache>();
            mocCache.Setup(c => c.Contains(It.IsAny<CacheEntry>(), It.IsAny<string>()))
                .Returns(false)
                .Callback<CacheEntry, string>((e, p) => { entry = e; });
            bool result = mocCache.Object.Contains("hello");

            Assert.False(result);
            Assert.Equal("hello", entry.Key);
            Assert.Equal(CacheTarget.Default, entry.Target);
        }

        #endregion Contains Tests

        #region Get Tests

        /// <summary>
        /// Verifies getting an entry with a null cache instance returns the
        /// default type value.
        /// </summary>
        [Fact]
        public void GetTest_NullCache()
        {
            ICache cache = null;
            string result = cache.Get<string>("key");

            Assert.Null(result);
        }

        /// <summary>
        /// Verifies getting a cache  entry passes the correct values.
        /// </summary>
        [Fact]
        public void GetTest_Valid()
        {
            CacheEntry entry = null;
            var mocCache = new Mock<ICache>();
            mocCache.Setup(c => c.Get(It.IsAny<CacheEntry>(), It.IsAny<string>()))
                .Returns("Taco")
                .Callback<CacheEntry, string>((e, p) => { entry = e; });
            string result = mocCache.Object.Get<string>("hello");

            Assert.Equal("Taco", result);
            Assert.Equal("hello", entry.Key);
            Assert.Equal(CacheTarget.Default, entry.Target);
        }

        #endregion Get Tests

        #region Set Tests

        /// <summary>
        /// Verifies the cache entry values are set correctly when passed to
        /// the ICache instance.
        /// </summary>
        [Theory]
        [MemberData(nameof(KeyValueData))]
        public void SetCacheTest_KeyValue(string key, object value)
        {
            CacheEntry entry = null;
            var mocCache = new Mock<ICache>();
            mocCache.Setup(c => c.Set(It.IsAny<CacheEntry>(), It.IsAny<string>())).Callback<CacheEntry, string>((e, p) => { entry = e; });
            mocCache.Object.Set(key, value);

            Assert.Equal(key, entry.Key);
            Assert.Equal(value, entry.Value);
            Assert.Equal(DateTimeOffset.MaxValue, entry.AbsoluteExpiration);
            Assert.Equal(TimeSpan.Zero, entry.SlidingExpiration);
            Assert.Equal(CacheTarget.Default, entry.Target);
        }

        /// <summary>
        /// Verifies the cache entry values are set correctly when passed to
        /// the ICache instance.
        /// </summary>
        [Theory]
        [MemberData(nameof(KeyValueExpireData))]
        public void SetCacheTest_KeyValueExpire(string key, object value, DateTimeOffset expiration)
        {
            CacheEntry entry = null;
            var mocCache = new Mock<ICache>();
            mocCache.Setup(c => c.Set(It.IsAny<CacheEntry>(), It.IsAny<string>())).Callback<CacheEntry, string>((e, p) => { entry = e; });
            mocCache.Object.Set(key, value, expiration);

            Assert.Equal(key, entry.Key);
            Assert.Equal(value, entry.Value);
            Assert.Equal(expiration, entry.AbsoluteExpiration);
            Assert.Equal(TimeSpan.Zero, entry.SlidingExpiration);
            Assert.Equal(CacheTarget.Default, entry.Target);
        }

        /// <summary>
        /// Verifies the cache entry values are set correctly when passed to
        /// the ICache instance.
        /// </summary>
        [Theory]
        [MemberData(nameof(KeyValueSpanData))]
        public void SetCacheTest_KeyValueSpan(string key, object value, TimeSpan expiration)
        {
            CacheEntry entry = null;
            var mocCache = new Mock<ICache>();
            mocCache.Setup(c => c.Set(It.IsAny<CacheEntry>(), It.IsAny<string>())).Callback<CacheEntry, string>((e, p) => { entry = e; });
            mocCache.Object.Set(key, value, expiration);

            Assert.Equal(key, entry.Key);
            Assert.Equal(value, entry.Value);
            Assert.Equal(DateTimeOffset.MaxValue, entry.AbsoluteExpiration);
            Assert.Equal(expiration, entry.SlidingExpiration);
            Assert.Equal(CacheTarget.Default, entry.Target);
        }

        #endregion Set Tests

        #region Remove Tests

        /// <summary>
        /// Verifies a null cache instance returns null.
        /// </summary>
        [Fact]
        public void RemoveCacheTest_NullCache()
        {
            ICache cache = null;
            object result = cache.Remove("key");

            Assert.Null(result);
        }

        /// <summary>
        /// Verifies a valid entry passes the correct values.
        /// </summary>
        [Fact]
        public void RemoveCacheTest_Valid()
        {
            CacheEntry entry = null;
            var mocCache = new Mock<ICache>();
            mocCache.Setup(c => c.Remove(It.IsAny<CacheEntry>(), It.IsAny<string>()))
                .Returns("Taco")
                .Callback<CacheEntry, string>((e, p) => { entry = e; });
            object result = mocCache.Object.Remove("hello");

            Assert.Equal("Taco", result);
            Assert.Equal("hello", entry.Key);
            Assert.Equal(CacheTarget.Default, entry.Target);
        }

        #endregion Remove Tests
    }
}