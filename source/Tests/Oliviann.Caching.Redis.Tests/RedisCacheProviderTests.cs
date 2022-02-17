namespace Oliviann.Caching.Redis.Tests
{
    #region Usings

    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Threading;
    using StackExchange.Redis;
    using Xunit;

    #endregion Usings

    [Trait("Category", "Redis")]
    public class RedisCacheProviderTests
    {
        #region Fields

        private readonly RedisCacheProvider provider;

        #endregion Fields

        #region Constructor/Destructor

        public RedisCacheProviderTests()
        {
            this.provider = new RedisCacheProvider(new RedisOptions { ConnectionString = ConfigurationManager.AppSettings["redis.connection"] });
        }

        #endregion Constructor/Destructor

        #region Add Tests

        /// <summary>
        /// Verifies a null value entry isn't entered into the cache.
        /// </summary>
        [Fact]
        public void RedisCacheAddTest_NullValue()
        {
            var entry = new CacheEntry("AddTest2", null);
            bool result = this.provider.Add(entry);
            Assert.False(result);

            Assert.False(this.provider.Contains(entry));
        }

        /// <summary>
        /// Verifies a valid entry is added to the cache.
        /// </summary>
        [Fact]
        public void RedisCacheAddTest_ValidEntry()
        {
            var entry = new CacheEntry("AddTest1", "BoeingAddTest1");
            bool result = this.provider.Add(entry);
            Assert.True(result);
            Assert.True(this.provider.Contains(entry));

            this.provider.Remove(entry);
        }

        /// <summary>
        /// Verifies an entry already in the cache isn't overwritten with a new
        /// value.
        /// </summary>
        [Fact]
        public void RedisCacheAddTest_ValidEntryNoOverwrite()
        {
            var entry1 = new CacheEntry("Oliviann222", "Fly Me Away Now");
            this.provider.Add(entry1);
            Assert.True(this.provider.Contains(entry1));

            var entry2 = new CacheEntry(entry1.Key, "The planes go zoom");
            this.provider.Add(entry2);
            Assert.True(this.provider.Contains(entry1));

            object result = this.provider.Get(entry1);
            Assert.NotNull(result);
            Assert.Equal(entry1.Value, (string)result);

            this.provider.Remove(entry1);
        }

        /// <summary>
        /// Verifies an entry set with an absolute expiration is removed from
        /// the cache.
        /// </summary>
        [Fact]
        public void RedisCacheAddTest_VerifyExpiration()
        {
            var entry = new CacheEntry("Oliviann123", "Fly Me Away Now") { AbsoluteExpiration = DateTimeOffset.UtcNow.AddSeconds(2) };
            this.provider.Add(entry);
            Assert.True(this.provider.Contains(entry));

            Thread.Sleep(2500);
            Assert.False(this.provider.Contains(entry));
        }

        #endregion Add Tests

        #region Contains Tests

        /// <summary>
        /// Verifies a collection of string keys don't exist in the cache.
        /// </summary>
        [Theory]
        ////[InlineData(null)]
        [InlineData("")]
        [InlineData("Hello1234")]
        public void RedisCacheContainsTest_FalseTests(string key)
        {
            var entry = new CacheEntry(key);

            bool result = this.provider.Contains(entry);
            Assert.False(result);
        }

        #endregion Contains Tests

        #region Get Tests

        /// <summary>
        /// Verifies a cache key miss returns a null object.
        /// </summary>
        [Theory]
        [InlineData("")]
        [InlineData("Hello123456")]
        public void RedisCacheGetTest_FalseKeys(string key)
        {
            var entry = new CacheEntry(key);

            object result = this.provider.Get(entry);
            Assert.Null(result);
        }

        /// <summary>
        /// Verifies you can add a entry to the cache and then get the same
        /// entry value.
        /// </summary>
        [Fact]
        public void RedisCacheGetTest_ValidKey()
        {
            var entry = new CacheEntry("Oliviann543", "FlyMeAway");
            bool addResult = this.provider.Add(entry);
            Assert.True(addResult);

            object result = this.provider.Get(entry);
            Assert.NotNull(result);
            Assert.Equal(entry.Value, (string)result);

            this.provider.Remove(entry);
        }

        /// <summary>
        /// Verifies an invalid key isn't returned in the keys collection.
        /// </summary>
        [Fact]
        public void RedisCacheGetKeysTest_MissingKey()
        {
            IReadOnlyCollection<string> result = this.provider.GetKeys();
            Assert.NotNull(result);
            Assert.DoesNotContain("Tacos", result);
        }

        /// <summary>
        /// Verifies a valid key is returned in the keys collection.
        /// </summary>
        [Fact]
        public void RedisCacheGetKeysTest_WithKeys()
        {
            var entry = new CacheEntry("Abc1234cbd", "FlyMeAway2");
            bool addResult = this.provider.Add(entry);
            Assert.True(addResult);

            IReadOnlyCollection<string> result = this.provider.GetKeys();
            Assert.NotNull(result);
            Assert.Contains(entry.Key, result);

            this.provider.Remove(entry);
        }

        #endregion Get Tests

        #region Set Tests

        /// <summary>
        /// Verifies a null value entry isn't entered into the cache.
        /// </summary>
        [Fact]
        public void RedisCacheSetTest_NullValue()
        {
            var entry = new CacheEntry("SetTest2", null);
            this.provider.Set(entry);

            Assert.False(this.provider.Contains(entry));
        }

        /// <summary>
        /// Verifies an entry that isn't in the cache can be added.
        /// </summary>
        [Fact]
        public void RedisCacheSetTest_InsertEntry()
        {
            var entry = new CacheEntry("Oliviann543", "Fly Me Away Now");
            this.provider.Set(entry);

            Assert.True(this.provider.Contains(entry));
            this.provider.Remove(entry);
        }

        /// <summary>
        /// Verifies an entry set with an absolute expiration is removed from
        /// the cache.
        /// </summary>
        [Fact]
        public void RedisCacheSetTest_VerifyExpiration()
        {
            var entry = new CacheEntry("Oliviann853", "Fly Me Away Now") { AbsoluteExpiration = DateTimeOffset.UtcNow.AddSeconds(2) };
            this.provider.Set(entry);
            Assert.True(this.provider.Contains(entry));

            Thread.Sleep(2500);
            Assert.False(this.provider.Contains(entry));
        }

        /// <summary>
        /// Verifies you can update the value of a specific cache key.
        /// </summary>
        [Fact]
        public void RedisCacheSetTest_UpdateEntry()
        {
            var entry1 = new CacheEntry("Oliviann543", "Fly Me Away Now");
            this.provider.Set(entry1);
            Assert.True(this.provider.Contains(entry1));

            var entry2 = new CacheEntry(entry1.Key, "The planes go zoom");
            this.provider.Set(entry2);
            Assert.True(this.provider.Contains(entry1));

            object result = this.provider.Get(entry1);
            Assert.NotNull(result);
            Assert.Equal(entry2.Value, (string)result);

            this.provider.Remove(entry1);
        }

        #endregion Set Tests

#if DEBUG

        /// <summary>
        /// Verifies creating an instance and disposing doesn't thrown an
        /// exception.
        /// </summary>
        [Fact]
        public void RedisGetDatabaseTest_Default()
        {
            IDatabase database = this.provider.GetDatabase();
            Assert.NotNull(database);
        }

#endif

        /// <summary>
        /// Verifies an exception isn't thrown when disposing a connection
        /// that hasn't been set yet.
        /// </summary>
        [Fact]
        public void RedisCacheInstanceTest_Dispose()
        {
            var provider = new RedisCacheProvider(new RedisOptions { ConnectionString = ConfigurationManager.AppSettings["redis.connection"] });
            provider.Dispose();
        }
    }
}