namespace Oliviann.Tests.Xml.XPath
{
    #region Usings

    using System.Xml.XPath;
    using Oliviann.Xml.XPath;
    using Xunit;

    #endregion Usings

    [Trait("Category", "CI")]
    public class XPathExtensionsTests
    {
        [Fact]
        public void Test_XPathValueOrDefault_Null()
        {
            XPathItem nav = null;
            string result = nav.ValueOrDefault();

            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public void Test_XPathValueOrDefault_Data()
        {
            XPathItem nav = new MoqXPathItem("Hello");
            string result = nav.ValueOrDefault();

            Assert.Equal("Hello", result);
        }
    }
}