#if NETFRAMEWORK

namespace Oliviann.Web.Tests
{
    #region Usings

    using System;
    using System.Collections.Specialized;
    using System.Configuration;
    using System.Web;
    using Oliviann.Collections.Specialized;
    using Moq;
    using Xunit;

    #endregion Usings

    [Trait("Category", "CI")]
    public class HttpHeaderModuleTests
    {
        #region Headers Execution Test

        /// <summary>
        /// Verifies a duplicate entry appends the string and not an additional
        /// entry.
        /// </summary>
        [Fact]
        public void OnBeginRequestTest_AddDuplicateHeader()
        {
            ConfigurationManager.AppSettings["HTTP_CustomHeaders"] = "usersid";
            ConfigurationManager.AppSettings["usersid"] = "76894520";

            var collection = new NameValueCollection { { "usersid", "4675309" } };
            var requestMoc = new Mock<HttpRequestBase>();
            requestMoc.SetupGet(r => r.Headers).Returns(collection.SetReadOnly(true));

            var contextMoc = new Mock<HttpContextBase>();
            contextMoc.SetupGet(c => c.Request).Returns(requestMoc.Object);

            BaseHttpModule module = new HttpHeaderModule();
            module.OnBeginRequest(contextMoc.Object, EventArgs.Empty);

            Assert.NotNull(collection);
            Assert.Single(collection);
            Assert.Equal("4675309,76894520", collection["usersid"]);
        }

        /// <summary>
        /// Verifies adding custom headers completes successfully.
        /// </summary>
        [Fact]
        public void OnBeginRequestTest_AddCustomHeaders()
        {
            ConfigurationManager.AppSettings["HTTP_CustomHeaders"] = "usersid;manager";
            ConfigurationManager.AppSettings["usersid"] = "9657458";
            ConfigurationManager.AppSettings["manager"] = "cn=523748,ou=People,O=Oliviann,c=US";

            var collection = new NameValueCollection();
            var requestMoc = new Mock<HttpRequestBase>();
            requestMoc.SetupGet(r => r.Headers).Returns(collection.SetReadOnly(true));

            var contextMoc = new Mock<HttpContextBase>();
            contextMoc.SetupGet(c => c.Request).Returns(requestMoc.Object);

            BaseHttpModule module = new HttpHeaderModule();
            module.OnBeginRequest(contextMoc.Object, EventArgs.Empty);

            Assert.NotNull(collection);
            Assert.Equal(2, collection.Count);
        }

        /// <summary>
        /// Verifies no errors were thrown and no headers were added to the
        /// collection.
        /// </summary>
        [Fact]
        public void OnBeginRequestTest_NoCustomHeaders()
        {
            ConfigurationManager.AppSettings["HTTP_CustomHeaders"] = string.Empty;

            var collection = new NameValueCollection();
            var requestMoc = new Mock<HttpRequestBase>();
            requestMoc.SetupGet(r => r.Headers).Returns(collection.SetReadOnly(true));

            var contextMoc = new Mock<HttpContextBase>();
            contextMoc.SetupGet(c => c.Request).Returns(requestMoc.Object);

            BaseHttpModule module = new HttpHeaderModule();
            module.OnBeginRequest(contextMoc.Object, EventArgs.Empty);

            Assert.NotNull(collection);
            Assert.Empty(collection);
        }

        #endregion Headers Execution Test
    }
}

#endif