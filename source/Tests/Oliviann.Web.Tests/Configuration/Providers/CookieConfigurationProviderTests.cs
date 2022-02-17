#if NETFRAMEWORK

namespace Oliviann.Web.Tests.Configuration.Providers
{
    #region Usings

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web;
    using Oliviann.Configuration.Providers;
    using Xunit;

    #endregion Usings

    [Trait("Category", "CI")]
    public class CookieConfigurationProviderTests
    {
        #region Constructor/Destructor

        public CookieConfigurationProviderTests()
        {
            HttpContext.Current = MockHelpers.CreateHttpContext();
        }

        #endregion

        /// <summary>
        /// Verifies an argument exception is thrown when an invalid name is
        /// passed in.
        /// </summary>
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void CookieProvidercTorTest_InvalidNames(string cookieName)
        {
            Assert.Throws<ArgumentNullException>(() => new CookieConfigurationProvider(cookieName));
        }

        /// <summary>
        /// Verifies calling the load method doesn't throw an exception.
        /// </summary>
        [Fact]
        public void CookieProviderLoadTest()
        {
            var provider = new CookieConfigurationProvider("Taco");
            provider.Load();
        }

        /// <summary>
        /// Verifies can read all the keys from a matching cookie.
        /// </summary>
        [Fact]
        public void CookieProviderGetChildKeysTest_MatchingCookie()
        {
            var cookie = new HttpCookie("DaveId", "12345678");
            cookie.Values.Add("UserName", "1234567");
            cookie.Values.Add("Location", "USA");
            HttpContext.Current.Request.Cookies.Add(cookie);

            var provider = new CookieConfigurationProvider("DaveId");
            IEnumerable<string> result = provider.GetChildKeys();

            Assert.NotNull(result);
            Assert.Equal(9, result.Count());
        }

        /// <summary>
        /// Verifies can read all the keys from a matching cookie.
        /// </summary>
        [Fact]
        public void CookieProviderGetChildKeysTest_MatchingCookieDuplicateKeys()
        {
            var cookie = new HttpCookie("DaveId", "12345678");
            cookie.Values.Add("Name", "Sam");
            cookie.Values.Add("Location", "USA");
            HttpContext.Current.Request.Cookies.Add(cookie);

            var provider = new CookieConfigurationProvider("DaveId");
            IEnumerable<string> result = provider.GetChildKeys();

            Assert.NotNull(result);
            Assert.Equal(9, result.Count());
        }

        /// <summary>
        /// Verifies an empty collection is returned for a non-matching cookie.
        /// </summary>
        [Fact]
        public void CookieProviderGetChildKeysTest_NonMatchingCookie()
        {
            HttpContext.Current.Request.Cookies.Add(new HttpCookie("JamieId", "12345678"));

            var provider = new CookieConfigurationProvider("SusanX1");
            IEnumerable<string> result = provider.GetChildKeys();

            Assert.NotNull(result);
            Assert.Empty(result);
        }

        #region TryGet Tests

        [Theory]
        [InlineData("Name", true, "JoshId2")]
        [InlineData("Domain", true, "se")]
        [InlineData("Value", true, "1234567&Name=Josh&Location=USA")]
        [InlineData("Path", true, "/")]
        [InlineData("Sub:Name", true, "Josh")]
        [InlineData("Sub:Location", true, "USA")]
        [InlineData("Location", true, "USA")]
        [InlineData("Taco", false, null)]
        public void CookieProviderTryGetTest_StringValues(string key, bool expectedResult, string expectedValue)
        {
            var cookie = new HttpCookie("JoshId2", "1234567");
            cookie.Values.Add("Name", "Josh");
            cookie.Values.Add("Location", "USA");
            cookie.Domain = "se";
            HttpContext.Current.Request.Cookies.Add(cookie);

            var provider = new CookieConfigurationProvider("JoshId2");
            bool result = provider.TryGet(key, out string value);

            Assert.Equal(expectedResult, result);
            Assert.Equal(expectedValue, value);
        }

        [Theory]
        [InlineData("HttpOnly", true, false)]
        [InlineData("Secure", true, false)]
        public void CookieProviderTryGetTest_BoolValues(string key, bool expectedResult, bool expectedValue)
        {
            var cookie = new HttpCookie("JoshId2", "1234567");
            cookie.Values.Add("Name", "Josh");
            cookie.Values.Add("Location", "USA");
            cookie.Domain = "se";
            HttpContext.Current.Request.Cookies.Add(cookie);

            var provider = new CookieConfigurationProvider("JoshId2");
            bool result = provider.TryGet(key, out bool value);

            Assert.Equal(expectedResult, result);
            Assert.Equal(expectedValue, value);
        }

        [Theory]
        [InlineData("Expires", true, "01/01/2028")]
        public void CookieProviderTryGetTest_DateTimeValues(string key, bool expectedResult, string expectedValue)
        {
            var cookie = new HttpCookie("JoshId2", "1234567");
            cookie.Values.Add("Name", "Josh");
            cookie.Values.Add("Location", "USA");
            cookie.Domain = "se";
            cookie.Expires = new DateTime(2028, 01, 01);
            HttpContext.Current.Request.Cookies.Add(cookie);

            var provider = new CookieConfigurationProvider("JoshId2");
            bool result = provider.TryGet(key, out DateTime value);

            Assert.Equal(expectedResult, result);
            Assert.Equal(expectedValue.ToDateTime(DateTime.MinValue), value);
        }

        [Fact]
        public void CookieProviderTryGetTest_MissingCookie()
        {
            var cookie = new HttpCookie("JoshId2", "1234567");
            cookie.Values.Add("Name", "Josh");
            cookie.Values.Add("Location", "USA");
            cookie.Domain = "se";
            HttpContext.Current.Request.Cookies.Add(cookie);

            var provider = new CookieConfigurationProvider("JoshId41");
            bool result = provider.TryGet("Name", out string value);

            Assert.False(result);
            Assert.Null(value);
        }

        [Fact]
        public void CookieProviderTryGetTest_NoSubKeysAndMissedKey()
        {
            var cookie = new HttpCookie("JoshId2", "1234567");
            HttpContext.Current.Request.Cookies.Add(cookie);

            var provider = new CookieConfigurationProvider("JoshId2");
            bool result = provider.TryGet("UserNa", out string value);

            Assert.False(result);
            Assert.Null(value);
        }

        #endregion

        #region Set Tests

        /// <summary>
        /// Verifies an exception isn't thrown when a cookie isn't found.
        /// </summary>
        [Fact]
        public void CookieProviderSetTest_MissingCookie()
        {
            var provider = new CookieConfigurationProvider("JoshC1");
            provider.Set("Value", "Hello");
        }

        /// <summary>
        /// Verifies you can set the string values correctly.
        /// </summary>
        [Theory]
        [InlineData("Domain", "se")]
        [InlineData("Path", "/localhost")]
        [InlineData("Value", "Yummy Tacos")]
        [InlineData("Sub:Name", "Josh")]
        [InlineData("Location", "USA")]
        public void CookieProviderSetTest_StringValues(string key, string value)
        {
            var cookie = new HttpCookie("JoshC1");
            HttpContext.Current.Request.Cookies.Add(cookie);

            var provider = new CookieConfigurationProvider("JoshC1");
            provider.Set(key, value);

            bool result = provider.TryGet(key, out string resultValue);
            Assert.True(result);
            Assert.Equal(value, resultValue);
        }

        /// <summary>
        /// Verifies you can set boolean values correctly.
        /// </summary>
        [Theory]
        [InlineData("HttpOnly", "true")]
        [InlineData("Secure", "true")]
        public void CookieProviderSetTest_BoolValues(string key, string value)
        {
            var cookie = new HttpCookie("JoshC1");
            HttpContext.Current.Request.Cookies.Add(cookie);

            var provider = new CookieConfigurationProvider("JoshC1");
            provider.Set(key, value);

            bool result = provider.TryGet(key, out bool resultValue);
            Assert.True(result);
            Assert.True(resultValue);
        }

        /// <summary>
        /// Verifies you can set DateTime values correctly.
        /// </summary>
        [Theory]
        [InlineData("Expires", "01/01/2020")]
        public void CookieProviderSetTest_DateTimeValues(string key, string value)
        {
            var cookie = new HttpCookie("JoshC1");
            HttpContext.Current.Request.Cookies.Add(cookie);

            var provider = new CookieConfigurationProvider("JoshC1");
            provider.Set(key, value);

            bool result = provider.TryGet(key, out DateTime resultValue);
            Assert.True(result);
            Assert.Equal(value.ToDateTime(DateTime.MaxValue), resultValue);
        }

        /// <summary>
        /// Verifies you can set DateTime values correctly.
        /// </summary>
        [Theory]
        [InlineData("Sub:", "Taco")]
        public void CookieProviderSetTest_InvalidKeys(string key, string value)
        {
            var cookie = new HttpCookie("JoshC1");
            HttpContext.Current.Request.Cookies.Add(cookie);

            var provider = new CookieConfigurationProvider("JoshC1");
            provider.Set(key, value);

            Assert.DoesNotContain(key, provider.GetChildKeys());
        }

        #endregion
    }
}

#endif