namespace Oliviann.Tests.Web.Caching
{
    #region Usings

    using System;
    using System.Web;
    using System.Web.Caching;
    using Oliviann.Caching;
    using Oliviann.Web.Caching;
    using Xunit;

    #endregion Usings

    [Trait("Category", "CI")]
    public class CacheExtensionsTests
    {
        #region Add Tests

        /// <summary>
        /// Verifies that a null cache object throws an argument null exception.
        /// </summary>
        [Fact]
        public void AddOrGetExistingTest_NullCache()
        {
            Cache cache = null;
            Assert.Throws<ArgumentNullException>(
                () => cache.AsICache().AddOrGetExisting<object>("Taco44", null, Cache.NoAbsoluteExpiration));
        }

        /// <summary>
        /// Verifies that a null cache key object throws an argument null
        /// exception.
        /// </summary>
        [Fact]
        public void AddOrGetExistingTest_NullKey()
        {
            var cache = new Cache();
            Assert.Throws<ArgumentNullException>(
                () => cache.AsICache().AddOrGetExisting<object>(null, null, Cache.NoAbsoluteExpiration));
        }

        /// <summary>
        /// Verifies that if the object is already in the cache that it returns
        /// the correct result.
        /// </summary>
        [Fact]
        public void AddOrGetExistingTest_FoundInitialObject()
        {
            Cache runtimeCache = HttpRuntime.Cache;
            runtimeCache.Add(
                "AnyKey14",
                "You Found Me!",
                null,
                Cache.NoAbsoluteExpiration,
                Cache.NoSlidingExpiration,
                CacheItemPriority.Default,
                null);

            string result = runtimeCache.AsICache().AddOrGetExisting<string>("AnyKey14", null, DateTime.MaxValue);
            Assert.Equal("You Found Me!", result);

            runtimeCache.Remove("AnyKey14");
        }

        /// <summary>
        /// Verifies that if the object isn't in the cache and the delete is
        /// null that a argument null exception is thrown.
        /// </summary>
        [Fact]
        public void AddOrGetExistingTest_MissingObjectAndNullDelegate()
        {
            Cache runtimeCache = HttpRuntime.Cache;
            Assert.Throws<ArgumentNullException>(
                () => runtimeCache.AsICache().AddOrGetExisting<string>("AnyKey08", null, DateTime.MaxValue));
        }

        /// <summary>
        /// Verifies that the delegate is called and isn't inserted into the
        /// cache when a null is returned.
        /// </summary>
        [Fact]
        public void AddOrGetExistingTest_DelegateIsCalledAndNotInserted()
        {
            Cache runtimeCache = HttpRuntime.Cache;

            bool funcCalled = false;
            Func<string> delFunc = () =>
                {
                    funcCalled = true;
                    return null;
                };

            string result = runtimeCache.AsICache().AddOrGetExisting("AnyKey11", delFunc, DateTime.MaxValue);

            Assert.True(funcCalled);
            Assert.Null(result);
            Assert.False(runtimeCache.AsICache().Contains("AnyKey11"));
        }

        /// <summary>
        /// Verifies that the delegate is called and the object is inserted into
        /// the cache.
        /// </summary>
        [Fact]
        public void AddOrGetExistingTest_DelegateIsCalledAndObjectIsInserted()
        {
            Cache runtimeCache = HttpRuntime.Cache;

            bool funcCalled = false;
            Func<string> delFunc = () =>
                {
                    funcCalled = true;
                    return "You Found Me!";
                };

            string result = runtimeCache.AsICache().AddOrGetExisting("AnyKey22", delFunc, DateTime.MaxValue);

            Assert.True(funcCalled);
            Assert.Equal("You Found Me!", result);
            Assert.True(runtimeCache.AsICache().Contains("AnyKey22"));

            runtimeCache.Remove("AnyKey22");
        }

        #endregion Add Tests

        #region Get Tests

        /// <summary>
        /// Verifies ...
        /// </summary>
        [Fact]
        public void GetTest_Object()
        {
            Cache runtimeCache = HttpRuntime.Cache;
            runtimeCache.Add(
                "Hello06",
                "You Found Me!",
                null,
                Cache.NoAbsoluteExpiration,
                Cache.NoSlidingExpiration,
                CacheItemPriority.Default,
                null);

            string result = runtimeCache.AsICache().Get<string>("Hello06");
            Assert.Equal("You Found Me!", result);

            runtimeCache.Remove("Hello06");
        }

        #endregion Get Tests
    }
}