namespace Oliviann.Tests.Xml
{
    #region Usings

    using System;
    using System.Xml;
    using Oliviann.Xml;
    using Xunit;

    #endregion Usings

    [Trait("Category", "CI")]
    public class XmlExtensionsTests
    {
        private const XmlNode NullNode = null;

        [Fact]
        public void XmlNodeInnerTextOrDefaultTest_NullNode()
        {
            string result = NullNode.InnerTextOrDefault();
            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public void XmlNodeInnerTextOrDefaultTest_ValidNode()
        {
            var doc = new XmlDocument();
            doc.LoadXml(@"<book genre='novel' ISBN='1-861001-57-5'>
                            <title>Pride And Prejudice</title>
                          </book>");

            XmlNode node = doc.DocumentElement;
            string result = node.InnerTextOrDefault();
            Assert.Equal("Pride And Prejudice", result);
        }

        [Fact]
        public void GetElementByTagNameTest_NullDoc()
        {
            XmlDocument doc = null;
            Assert.Throws<ArgumentNullException>(() => doc.GetElementByTagName(null));
        }

        [Fact]
        public void GetElementByTagNameTest_NullElementName()
        {
            var doc = new XmlDocument();
            doc.LoadXml(@"<book genre='novel' ISBN='1-861001-57-5'>
                            <title>Pride And Prejudice</title>
                          </book>");

            Assert.Throws<ArgumentNullException>(() => doc.GetElementByTagName(null));
        }

        [Fact]
        public void GetElementByTagNameTest_ValidElement()
        {
            var doc = new XmlDocument();
            doc.LoadXml(@"<book genre='novel' ISBN='1-861001-57-5'>
                            <title>Pride And Prejudice</title>
                          </book>");

            string result = doc.GetElementByTagName("title");
            Assert.Equal("Pride And Prejudice", result);
        }

        [Fact]
        public void GetElementByTagNameTest_InvalidElement()
        {
            var doc = new XmlDocument();
            doc.LoadXml(@"<book genre='novel' ISBN='1-861001-57-5'>
                            <title>Pride And Prejudice</title>
                          </book>");

            string result = doc.GetElementByTagName("chicken");
            Assert.Null(result);
        }
    }
}