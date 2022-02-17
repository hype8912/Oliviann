namespace Oliviann.Tests.Runtime.Caching
{
    #region Usings

    using System;
    using System.Runtime.Caching;
    using System.Threading;
    using Oliviann.Runtime.Caching;
    using Xunit;

    #endregion Usings

    [Trait("Category", "CI")]
    public class MemoryCacheExtensionsTests
    {
        #region Add Tests

        [Fact]
        public void Cache_AddGetTest_NullCache()
        {
            ObjectCache cache = null;
            Assert.Throws<ArgumentNullException>(() => cache.AddOrGetExisting<string>("AddTest_NullCache", null));
        }

        [Fact]
        public void Cache_AddGetTest_NullKey()
        {
            Assert.Throws<ArgumentNullException>(() => MemoryCache.Default.AddOrGetExisting<string>(null, null));
        }

        [Fact]
        public void Cache_AddGetTest_GetExistingString()
        {
            MemoryCache.Default.Insert("AddGetTest_GetExistingString1", "World");
            var result = MemoryCache.Default.AddOrGetExisting<string>("AddGetTest_GetExistingString1", null);

            Assert.Equal("World", result);
        }

        [Fact]
        public void Cache_AddGetTest_AddNewString()
        {
            Func<string> testFunc = () => "World2";
            var result = MemoryCache.Default.AddOrGetExisting("AddGetTest_GetExistingString2", testFunc);

            Assert.True(MemoryCache.Default.Contains("AddGetTest_GetExistingString2"));
            Assert.Equal("World2", result);
        }

        [Fact]
        public void Cache_AddGetTest_AddNewStringWithExpiration()
        {
            Func<string> testFunc = () => "World";
            var result = MemoryCache.Default.AddOrGetExisting("AddGetTest_AddNewStringWithExpiration", testFunc, TimeSpan.FromSeconds(3D));

            Assert.True(MemoryCache.Default.Contains("AddGetTest_AddNewStringWithExpiration"));
            Assert.Equal("World", result);

            Thread.Sleep(TimeSpan.FromSeconds(6D));
            Assert.False(MemoryCache.Default.Contains("AddGetTest_AddNewStringWithExpiration"));
        }

        [Fact]
        public void Cache_AddGetTest_NullPolicy()
        {
            Func<string> testFunc = () => "World";
            CacheItemPolicy policy = null;
            object result = MemoryCacheExtensions.AddOrGetExisting(MemoryCache.Default, "Cache_AddGetTest_NullPolicy", testFunc, policy);

            Assert.True(MemoryCache.Default.Contains("Cache_AddGetTest_NullPolicy"));
            Assert.Equal("World", result);

            MemoryCache.Default.Remove("Cache_AddGetTest_NullPolicy");
        }

        #endregion Add Tests

        #region Get Tests

        /// <summary>
        /// Verifies a null cache instance returns a null result.
        /// </summary>
        [Fact]
        public void Cache_GetTest_Null()
        {
            ObjectCache cache = null;
            string result = cache.Get<string>("HelloX");

            Assert.Null(result);
        }

        /// <summary>
        /// Verifies a null key throws an argument null exception.
        /// </summary>
        [Fact]
        public void Cache_GetTest_NullKey()
        {
            Assert.Throws<ArgumentNullException>(() => MemoryCache.Default.Get<string>(null));
        }

        /// <summary>
        /// Verifies a key in the cache returns the correct value.
        /// </summary>
        [Fact]
        public void Cache_GetTest_String()
        {
            MemoryCache.Default.Insert("HelloY", "World");
            string result = MemoryCache.Default.Get<string>("HelloY");

            Assert.Equal("World", result);
        }

        /// <summary>
        /// Verifies a key not in the cache returns a null value for nullable
        /// type.
        /// </summary>
        [Fact]
        public void Cache_GetTest_MissingKey_StringType()
        {
            string result = MemoryCache.Default.Get<string>("Abc123OliviannTacos");

            Assert.Null(result);
        }

        /// <summary>
        /// Verifies an key not in the cache returns the default value for a
        /// non-null type.
        /// </summary>
        [Fact]
        public void Cache_GetTest_MissingKey_BoolType()
        {
            bool result = MemoryCache.Default.Get<bool>("Abc123OliviannTacoZ");

            Assert.False(result);
        }

        #endregion Get Tests

        #region TryGetValue Tests

        [Fact]
        public void TryGetValueTest_NullCache()
        {
            MemoryCache cache = null;
            bool result = cache.TryGetValue("Taco", out string value);

            Assert.False(result);
            Assert.Null(value);
        }

        [Fact]
        public void TryGetValueTest_MissingKey()
        {
            bool result = MemoryCache.Default.TryGetValue("Taco3678", out string value);

            Assert.False(result);
            Assert.Null(value);
        }

        [Fact]
        public void TryGetValueTest_ValidKey()
        {
            MemoryCache.Default.Add("Taco432", "Bell", DateTimeOffset.MaxValue);
            bool result = MemoryCache.Default.TryGetValue("Taco432", out string value);

            Assert.True(result);
            Assert.Equal("Bell", value);
        }

        #endregion

        #region Insert Tests

        [Fact]
        public void Cache_InsertTest_NullCache()
        {
            ObjectCache cache = null;
            Assert.Throws<ArgumentNullException>(() => cache.Insert("Hello4", "World"));
        }

        [Fact]
        public void Cache_InsertTest_NullKey()
        {
            Assert.Throws<ArgumentNullException>(() => MemoryCache.Default.Insert(null, "World"));
        }

        [Fact]
        public void Cache_InsertTest_NewString()
        {
            MemoryCache.Default.Insert("Hello3", "World");

            Assert.True(MemoryCache.Default.Contains("Hello3"));
            Assert.Equal("World", MemoryCache.Default.Get<string>("Hello3"));
        }

        [Fact]
        public void Cache_InsertTest_NewString_WithExpiration()
        {
            MemoryCache.Default.Insert("Hello2", "World", TimeSpan.FromSeconds(5D));

            Assert.True(MemoryCache.Default.Contains("Hello2"));
            Assert.Equal("World", MemoryCache.Default.Get<string>("Hello2"));
        }

        [Fact]
        public void Cache_InsertTest_NewString_WithExpirationAndRemoved()
        {
            MemoryCache.Default.Insert("Hello_WithExpire", "World", TimeSpan.FromSeconds(3D));
            Assert.True(MemoryCache.Default.Contains("Hello_WithExpire"));
            Assert.Equal("World", MemoryCache.Default.Get<string>("Hello_WithExpire"));

            Thread.Sleep(TimeSpan.FromSeconds(6D));
            Assert.False(MemoryCache.Default.Contains("Hello_WithExpire"));
        }

        [Fact]
        public void Cache_InsertTest_UpdateString()
        {
            MemoryCache.Default.Insert("Hello_UpdateString", "World");
            Assert.True(MemoryCache.Default.Contains("Hello_UpdateString"));
            Assert.Equal("World", MemoryCache.Default.Get<string>("Hello_UpdateString"));

            MemoryCache.Default.Insert("Hello_UpdateString", "Professor");
            Assert.True(MemoryCache.Default.Contains("Hello_UpdateString"));
            Assert.Equal("Professor", MemoryCache.Default.Get<string>("Hello_UpdateString"));
        }

        #endregion Insert Tests
    }
}