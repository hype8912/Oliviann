#if NOT_COMPLETED
namespace Oliviann.Web.Tests.Http
{
#region Usings

    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Configuration;
    using System.Configuration.Fakes;
    using System.Linq;
    using System.Runtime.Caching;
    using Microsoft.QualityTools.Testing.Fakes;
    using Oliviann.Runtime.Caching;
    using Oliviann.Web.Http;
    using Xunit;

#endregion Usings

    [Trait("Category", "CI")]
    public class SimpleApiTokenAuthorizeAttributeTests
    {
#region Fields

        private const string AuthorizationTokensLoadedKey = "ApiAuthKeysLoaded";

#endregion Fields

#region cTor Tests

        /// <summary>
        /// Verifies no exceptions are thrown and the key for loaded has been
        /// added to the cache.
        /// </summary>
        [Fact]
        public void SimpleApiAuthTokencTorTest_NoTokensInConfig()
        {
            using (ShimsContext.Create())
            {
                ShimConfigurationManager.AppSettingsGet = () => new NameValueCollection();

                var attr = new SimpleApiTokenAuthorizeAttribute();
                IEnumerable<string> keys = this.GetAuthTokenFromConfig();

                Assert.Empty(keys);
                Assert.True(MemoryCache.Default.Contains(AuthorizationTokensLoadedKey));
            }

            this.RemoveAllTokensFromCache();
        }

        /// <summary>
        /// Verifies the tokens aren't loaded if they've already been loaded in
        /// cache.
        /// </summary>
        [Fact]
        public void SimpleApiAuthTokencTorTest_TokensAlreadyLoaded()
        {
            DateTime time = DateTime.UtcNow;
            MemoryCache.Default.Set(AuthorizationTokensLoadedKey, time, DateTimeOffset.MaxValue);

            var attr = new SimpleApiTokenAuthorizeAttribute();
            var result = MemoryCache.Default.Get<DateTime>(AuthorizationTokensLoadedKey);

            Assert.Equal(time, result);
            MemoryCache.Default.Remove(AuthorizationTokensLoadedKey);
        }

#if NOT_COMPLETED
        /// <summary>
        /// Verifies tokens are read from settings file and loaded into cache.
        /// </summary>
        [Fact]
        public void SimpleApiAuthTokencTorTest_LoadTokensFromSettings()
        {
            using (ShimsContext.Create())
            {
                var tokens = new NameValueCollection
                                 {
                                         {
                                             "ApiAuthToken:JuanValdez",
                                             "05d86d358a5a4dd4a6c9887796409c0ac2cacbe5062e40b0b65debe26df375a4"
                                         },
                                         {
                                             "ApiAuthToken:Developer",
                                             "366b79e0f1b1414db341ea18f70fe192560e0420c0514bebafd0308030654574"
                                         }
                                 };
                ShimConfigurationManager.AppSettingsGet = () => tokens;

                var attr = new SimpleApiTokenAuthorizeAttribute();

                Assert.True(MemoryCache.Default.Contains("ApiAuthToken:05D86D358A5A4DD4A6C9887796409C0AC2CACBE5062E40B0B65DEBE26DF375A4"));
                Assert.True(MemoryCache.Default.Contains("ApiAuthToken:366B79E0F1B1414DB341EA18F70FE192560E0420C0514BEBAFD0308030654574"));
            }

            this.RemoveAllTokensFromCache();
        }
#endif

#endregion cTor Tests

#region OnAuthorization Tests

        /// <summary>
        /// Verifies the rest of the authorization code doesn't execute if
        /// disabled.
        /// </summary>
        [Fact(Skip = "Fails::System.IO.FileNotFoundException : Could not load file or assembly 'System.Net.Http, Version=5.2.3.0")]
        public void OnAuthorizationTest_FalseActive()
        {
            var attr = new SimpleApiTokenAuthorizeAttribute(false);
            attr.OnAuthorization(null);

            // If the code continued to execute you would get a null reference exception.
        }

#if NOT_COMPLETED
        [Fact]
        public void OnAuthorizationTest_NullAuthorizationHeader()
        {
            using (ShimsContext.Create())
            {
                ShimHttpRequestMessageExtensions.CreateErrorResponseHttpRequestMessageHttpStatusCodeString =
                    (message, code, text) => new HttpResponseMessage(code) { ReasonPhrase = text };

                var request = new ShimHttpRequestMessage { HeadersGet = () => new ShimHttpRequestHeaders { AuthorizationGet = null } };
                HttpResponseMessage response = null;
                var controller = new ShimHttpControllerContext { RequestGet = () => request };

                var context = new ShimHttpActionContext
                                  {
                                      ControllerContextGet = () => controller,
                                      RequestGet = () => request,
                                      ResponseSetHttpResponseMessage = msg => response = msg
                                  };

                MemoryCache.Default.Add(AuthorizationTokensLoadedKey, DateTime.UtcNow, DateTimeOffset.MaxValue);
                var attr = new SimpleApiTokenAuthorizeAttribute(true);
                attr.OnAuthorization(context);
                this.RemoveAllTokensFromCache();

                Assert.NotNull(response);
                Assert.Equal("Authorization information not provided.", response.ReasonPhrase);
                Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
            }
        }
#endif

#endregion OnAuthorization Tests

#region Helper Methods

        private void RemoveAllTokensFromCache()
        {
            IEnumerable<string> keys =
                MemoryCache.Default.Where(pair => pair.Key.StartsWith("ApiAuthToken:")).Select(pair => pair.Key);
            foreach (string key in keys)
            {
                MemoryCache.Default.Remove(key);
            }

            MemoryCache.Default.Remove(AuthorizationTokensLoadedKey);
        }

        private IEnumerable<string> GetAuthTokenFromConfig()
        {
            return ConfigurationManager.AppSettings.AllKeys.Where(k => k.StartsWith("ApiAuthToken:"));
        }

#endregion Helper Methods
    }
}
#endif