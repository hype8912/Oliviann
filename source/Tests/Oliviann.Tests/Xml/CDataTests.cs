namespace Oliviann.Tests.Xml
{
    #region Usings

    using System;
    using System.Xml;
    using System.Xml.Schema;
    using System.Xml.Serialization;
    using Oliviann.Xml;
    using Oliviann.Xml.Serialization;
    using Xunit;

    #endregion Usings

    [Trait("Category", "CI")]
    public class CDataTests
    {
        #region Fields

        private const string AboutXmlText = @"<about><disclosure>
      <![CDATA[Disclosure text for about here.]]>
    </disclosure></about>";

        #endregion Fields

        [Fact]
        public void CDataTest_ctor()
        {
            var data = new CData();
            string result = data.Text;

            Assert.Null(result);
        }

        [Fact]
        public void CDataTest_ctorText()
        {
            var data = new CData("Hello World");
            string result = data.Text;

            Assert.Equal("Hello World", result);
        }

        [Fact]
        public void CDataGetSchemaTest_Schema()
        {
            IXmlSerializable data = new CData("Hello World");
            XmlSchema result = data.GetSchema();

            Assert.Null(result);
        }

        [Fact]
        public void CDataReadXmlTest_NullReader()
        {
            var data = new CData("Hello World");
            var serializable = (IXmlSerializable)data;
            Assert.Throws<ArgumentNullException>(() => serializable.ReadXml(null));
        }

        [Fact]
        public void CDataReadXmlTest_ReadData()
        {
            About ab = null;
            new BasicXmlSerializer<About>().DeserializeString(AboutXmlText, about => ab = about);

            Assert.NotNull(ab);
            Assert.NotNull(ab.Disclosure);
            string result = ab.Disclosure.Text;

            Assert.Equal("\n      Disclosure text for about here.\n    ", result);
        }

        [Fact]
        public void CDataReadXmlTest_ReadData2()
        {
            About ab = null;
            Action<About> act = about => ab = about;
            var settings = new XmlReaderSettings { IgnoreWhitespace = true };
            new BasicXmlSerializer<About>().DeserializeString(AboutXmlText, act, settings);

            Assert.NotNull(ab);
            Assert.NotNull(ab.Disclosure);
            string result = ab.Disclosure.Text;

            Assert.Equal(@"Disclosure text for about here.", result);
        }

        [Fact]
        public void CDataWriteXmlTest_NullWriter()
        {
            var data = new CData("Hello World");
            var serializable = (IXmlSerializable)data;
            Assert.Throws<ArgumentNullException>(() => serializable.WriteXml(null));
        }

        [Fact]
        public void CDataWriteXmlTest_WriteData()
        {
            var ab = new About { Disclosure = new CData("Disclosure text for about here.") };
            string result = new BasicXmlSerializer<About>().SerializeString(ab);

            Assert.Contains("<disclosure><![CDATA[Disclosure text for about here.]]></disclosure>", result);
        }

        #region Classes

        [XmlRoot(ElementName = "about")]
        [Serializable]
        public class About
        {
            [XmlElement("disclosure", Type = typeof(CData))]
            public CData Disclosure { get; set; }
        }

        #endregion Classes
    }
}