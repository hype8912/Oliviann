namespace Oliviann.Tests.Xml.Linq
{
    #region Usings

    using System.Xml.Linq;
    using Oliviann.Xml.Linq;
    using Xunit;

    #endregion Usings

    [Trait("Category", "CI")]
    public class XElementExtensionsTests
    {
        [Fact]
        public void XElementValueOrDefaultTest_NullElement()
        {
            XElement element = null;
            string result = element.ValueOrDefault();

            Assert.Equal(result, string.Empty);
        }

        [Fact]
        public void XElementValueOrDefaultTest_NullElement_DefaultValue()
        {
            XElement element = null;
            string result = element.ValueOrDefault("Tacos");

            Assert.Equal("Tacos", result);
        }

        [Fact]
        public void XElementValueOrDefaultTest_Data()
        {
            var element = new XElement("IsValid", true);
            string result = element.ValueOrDefault();

            Assert.Equal("true", result);
        }
    }
}