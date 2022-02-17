namespace Oliviann.Web.Tests.Net.Http
{
    #region Usings

    using System;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using Oliviann.Net.Http;
    using Xunit;

    #endregion Usings

    /// <summary>
    /// Testing client side api request caching Using SOAGen default template
    /// output APIs that use cache-control: max-age=20
    /// </summary>
    [Trait("Category", "CI")]
    public class RestServiceClientCacheTests
    {
        #region ClientCacheSet Test

        [Fact]
        public void ClientCacheSetTest_NoCacheSet()
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Headers.CacheControl = new CacheControlHeaderValue { NoCache = true };

            var cache = new MockRestServiceClientCache();
            cache.ClientCacheSet(response, "Hello World!123");

            string result = cache.GetResponseFromCache<string>("https://i.web.oliviann.com/culture/service/user/xt682v");
            Assert.Null(result);
        }

        /// <summary>
        /// Verifies a good response adds the result object to the cache.
        /// </summary>
        [Fact]
        public void ClientCacheSetTest_GoodParams()
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Headers.CacheControl = new CacheControlHeaderValue { NoCache = false, MaxAge = TimeSpan.FromSeconds(5) };
            response.RequestMessage = new HttpRequestMessage(HttpMethod.Get, "https://i.web.oliviann.com/culture/service/boeingUser/uw956a");

            var cache = new MockRestServiceClientCache();
            cache.ClientCacheSet(response, "Hello World!4567");

            string result = cache.GetResponseFromCache<string>("https://i.web.oliviann.com/culture/service/bUser/abc123a");
            Assert.Equal("Hello World!4567", result);
        }

        #endregion ClientCacheSet Test

        #region ShouldCacheResponse Tests

        /// <summary>
        /// Verifies a null cache control header value returns false.
        /// </summary>
        [Fact]
        public void ShouldCacheResponseTest_NullCacheControl()
        {
            CacheControlHeaderValue control = null;
            var cache = new MockRestServiceClientCache();

            bool result = cache.TestShouldCacheResponse(control);
            Assert.False(result);
        }

        /// <summary>
        /// Verifies a set no cache value returns the correct result.
        /// </summary>
        [Theory]
        [InlineData(true, false)]
        [InlineData(false, true)]
        public void ShouldCacheResponseTest_NoCacheSet(bool input, bool expectedResult)
        {
            var control = new CacheControlHeaderValue { NoCache = input };
            var cache = new MockRestServiceClientCache();

            bool result = cache.TestShouldCacheResponse(control);
            Assert.Equal(expectedResult, result);
        }

        #endregion ShouldCacheResponse Tests

        #region SetCache Tests

        /// <summary>
        /// Verifies an argument null exception is thrown if a null key is set.
        /// </summary>
        [Fact]
        public void SetCacheTest_NullKey()
        {
            var cache = new MockRestServiceClientCache();
            Assert.Throws<ArgumentNullException>(() => cache.TestSetCache(null, "hello", TimeSpan.FromDays(1)));
        }

        /// <summary>
        /// Verifies a null cache control age time span returns without entering
        /// anything in cache.
        /// </summary>
        [Fact]
        public void SetCacheTest_NullCacheControlAge()
        {
            var cache = new MockRestServiceClientCache();
            cache.TestSetCache("Hello1", "World1", null);

            string result = cache.GetResponseFromCache<string>("Hello1");
            Assert.Null(result);
        }

        /// <summary>
        /// Verifies good parameters adds the entry to the cache.
        /// </summary>
        [Fact]
        public void SetCacheTest_GoodParams()
        {
            int rand = new Random().Next(10, 50);
            var cache = new MockRestServiceClientCache();
            cache.TestSetCache("Hello" + rand, "World" + rand, TimeSpan.FromSeconds(5));

            string result = cache.GetResponseFromCache<string>("Hello" + rand);
            Assert.Equal("World" + rand, result);
        }

        #endregion SetCache Tests

        #region Helper Classes

        internal class MockRestServiceClientCache : RestServiceClientCache
        {
            internal bool TestShouldCacheResponse(CacheControlHeaderValue cacheControl)
            {
                return this.ShouldCacheResponse(cacheControl);
            }

            internal void TestSetCache(string key, object result, TimeSpan? cacheControlMaxAge)
            {
                this.SetCache(key, result, cacheControlMaxAge);
            }
        }

        #endregion Helper Classes
    }
}