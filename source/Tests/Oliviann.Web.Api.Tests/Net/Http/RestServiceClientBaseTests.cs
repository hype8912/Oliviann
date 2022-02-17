namespace Oliviann.Web.Tests.Net.Http
{
    #region Usings

    using System;
    using Oliviann.Net.Http;
    using Moq;
    using Xunit;

    #endregion Usings

    [Trait("Category", "CI")]
    public class RestServiceClientBaseTests
    {
        /// <summary>
        /// Verifies disposing a default instance doesn't throw an exception.
        /// </summary>
        [Fact]
        public void RestServiceClientBaseTest_DisposeDefault()
        {
            var mocClient = new Mock<RestServiceClientBase> { CallBase = true };
           mocClient.Object.Dispose();
        }

        #region Properties Tests

        /// <summary>
        /// Verifies the properties default values are the correct values.
        /// </summary>
        [Fact]
        public void RestServiceClientBaseTest_DefaultProperties()
        {
            var mocClient = new Mock<RestServiceClientBase> { CallBase = true };
            RestServiceOptions options = mocClient.Object.Options;

            Assert.Null(options.BaseUri);
            Assert.True(options.PreferJsonResponse);
            Assert.Equal(5, options.RequestTimeout.Minutes);
            Assert.False(options.DisableCaching);
        }

        /// <summary>
        /// Verifies setting the properties return the correct value.
        /// </summary>
        [Fact]
        public void RestServiceClientBaseTest_SetProperties()
        {
            var mocClient = new Mock<RestServiceClientBase>();
            mocClient.Setup(c => c.Options)
                .Returns(
                    () => new RestServiceOptions
                    {
                        BaseUri = new Uri("http://i.web.oliviann.com/culture/service/"),
                        PreferJsonResponse = false,
                        RequestTimeout = TimeSpan.FromSeconds(20),
                        DisableCaching = true
                    });

            RestServiceOptions options = mocClient.Object.Options;

            Assert.Equal("http://i.web.oliviann.com/culture/service/", options.BaseUri.AbsoluteUri);
            Assert.False(options.PreferJsonResponse);
            Assert.Equal(20, options.RequestTimeout.Seconds);
            Assert.True(options.DisableCaching);
        }

        #endregion Properties Tests

        #region Test Class

        public class RestServiceClientWrapper : RestServiceClientBase
        {
        }

        #endregion
    }
}