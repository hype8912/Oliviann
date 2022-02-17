namespace Oliviann.Tests.Net
{
    #region Usings

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.NetworkInformation;
    using System.Text;

    using Oliviann.Net;

    using Moq;

    using Xunit;

    #endregion Usings

    [Trait("Category", "CI")]
    public class IDnsProxyExtensionsTests
    {
        [Fact]
        public void GetFQDNTest_NullProxy()
        {
            IDnsProxy proxy = null;
            Assert.Throws<ArgumentNullException>(() => proxy.GetFQDN());
        }

        [Fact]
        public void GetFQDNTest_DoesNotContainsNetwork()
        {
            var mocProxy = new Mock<IDnsProxy>();
            mocProxy.Setup(p => p.GetHostName()).Returns("MyName");

            string result = mocProxy.Object.GetFQDN();
            Assert.StartsWith("MyName", result);
            Assert.EndsWith("oliviann.com", result);
        }

        [Fact]
        public void GetFQDNTest_ContainsNetwork()
        {
            string currentDomainName = IPGlobalProperties.GetIPGlobalProperties().DomainName;
            var mocProxy = new Mock<IDnsProxy>();
            mocProxy.Setup(p => p.GetHostName()).Returns("thisname:" + currentDomainName);

            string result = mocProxy.Object.GetFQDN();
            Assert.Equal("thisname:" + currentDomainName, result);
        }
    }
}