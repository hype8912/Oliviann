#if NETFRAMEWORK

namespace Oliviann.Web.Tests.Configuration.Providers
{
    #region Usings

    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using Oliviann.Configuration.Providers;
    using Xunit;

    #endregion Usings

    [Trait("Category", "CI")]
    public class SessionConfigurationProviderTests
    {
        #region Constructor/Destructor

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="SessionConfigurationProviderTests"/> class.
        /// </summary>
        public SessionConfigurationProviderTests()
        {
            HttpContext.Current = MockHelpers.CreateHttpContext();
        }

        #endregion

        /// <summary>
        /// Verifies calling the load method doesn't throw an exception.
        /// </summary>
        [Fact]
        public void SessionProviderLoadTest()
        {
            var provider = new SessionConfigurationProvider();
            provider.Load();
        }

        /// <summary>
        /// Verifies can read all the keys from the session.
        /// </summary>
        [Fact]
        public void SessionProviderGetChildKeysTest_StringValues()
        {
            HttpContext.Current.Session["JuanId"] = 12345678;
            HttpContext.Current.Session["Abc123"] = "Hello";

            var provider = new SessionConfigurationProvider();
            IEnumerable<string> result = provider.GetChildKeys();

            Assert.NotNull(result);
            Assert.Equal(2, result.Count());

            HttpContext.Current.Session.RemoveAll();
        }

        /// <summary>
        /// Verifies retrieving values from the session returns the correct
        /// value.
        /// </summary>
        [Theory]
        [InlineData("Juan123", true, "12345678")]
        [InlineData("Airplanes", true, "Oliviann")]
        [InlineData("Tacos", false, null)]
        public void SessionProviderTryGetTest_StringValues(string key, bool expectedResult, string expectedValue)
        {
            HttpContext.Current.Session["Josh123"] = "12345678";
            HttpContext.Current.Session["Airplanes"] = "Oliviann";

            var provider = new SessionConfigurationProvider();
            bool result = provider.TryGet(key, out string resultValue);

            Assert.Equal(expectedResult, result);
            Assert.Equal(expectedValue, resultValue);

            HttpContext.Current.Session.RemoveAll();
        }

        /// <summary>
        /// Verifies a session variable with a value can be added.
        /// </summary>
        [Fact]
        public void SessionProviderSetTest_AddNewVariable()
        {
            var provider = new SessionConfigurationProvider();
            provider.Set("DaveAbc", "18796");
            Assert.Single(HttpContext.Current.Session);

            provider.TryGet("DaveAbc", out string value);
            Assert.Equal("18796", value);

            HttpContext.Current.Session.RemoveAll();
        }

        /// <summary>
        /// Verifies updating a session variable value updates correctly.
        /// </summary>
        [Fact]
        public void SessionProviderSetTest_UpdateExistingVariable()
        {
            var provider = new SessionConfigurationProvider();
            provider.Set("RustyAb21", "18CO796");
            Assert.Single(HttpContext.Current.Session);

            provider.TryGet("RustyAb21", out string value);
            Assert.Equal("18CO796", value);

            provider.Set("RustyAb21", "Tacos");
            provider.TryGet("RustyAb21", out string value2);
            Assert.Equal("Tacos", value2);

            HttpContext.Current.Session.RemoveAll();
        }

        /// <summary>
        /// Verifies that setting a session variable to null does not remove it
        /// but sets the value to null.
        /// </summary>
        [Fact]
        public void SessionProviderSetTest_RemoveVariable()
        {
            var provider = new SessionConfigurationProvider();
            provider.Set("SampsonAbX", "1F796");
            Assert.Single(HttpContext.Current.Session);

            provider.Set("SampsonAbX", null);
            Assert.Single(HttpContext.Current.Session);

            HttpContext.Current.Session.RemoveAll();
        }
    }
}

#endif