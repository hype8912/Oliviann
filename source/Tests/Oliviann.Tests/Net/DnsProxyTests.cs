namespace Oliviann.Tests.Net
{
    #region Usings

    using System.Net;

    using Oliviann.Net;
    using Xunit;

    #endregion Usings

    [Trait("Category", "CI")]
    public class DnsProxyTests
    {
        [Fact]
        public void GetHostNameTest_Valid()
        {
            string currentHost = Dns.GetHostName();

            var proxy = new DnsProxy();
            string result = proxy.GetHostName();

            Assert.EndsWith(currentHost, result);
        }
    }
}