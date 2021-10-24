namespace Oliviann.Xml
{
    #region Usings

    using System;
    using System.Xml;
    using System.Xml.Schema;
    using System.Xml.Serialization;

    #endregion Usings

    /// <summary>
    /// Implements <see cref="IXmlSerializable"/> to add serialization of string
    /// to CData.
    /// </summary>
    [Serializable]
    public sealed class CData : IXmlSerializable
    {
        #region Constructor/Destructor

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Oliviann.Xml.CData"/>
        /// class.
        /// </summary>
        public CData()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Oliviann.Xml.CData"/>
        /// class.
        /// </summary>
        /// <param name="text">The text to be serialized.</param>
        public CData(string text)
        {
            this.Text = text;
        }

        #endregion Constructor/Destructor

        #region Properties

        /// <summary>
        /// Gets the data text string.
        /// </summary>
        /// <value>The data text string.</value>
        public string Text { get; private set; }

        #endregion Properties

        /// <summary>
        /// This method is reserved and should not be used. When implementing
        /// the
        /// <see cref="T:System.Xml.Serialization.IXmlSerializable"/> interface,
        /// you should return null (Nothing in Visual Basic) from this method,
        /// and instead, if specifying a custom schema  is required, apply the
        /// <see cref="T:System.Xml.Serialization.XmlSchemaProviderAttribute"/>
        /// to the class.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Xml.Schema.XmlSchema"/> that describes the
        /// XML  representation of the object that is produced by the
        /// <see cref="M:System.Xml.Serialization.IXmlSerializable.WriteXml(System.Xml.XmlWriter)"/>
        /// method and consumed by the
        /// <see cref="M:System.Xml.Serialization.IXmlSerializable.ReadXml(System.Xml.XmlReader)"/>
        /// method.
        /// </returns>
        XmlSchema IXmlSerializable.GetSchema() => null;

        /// <summary>
        /// Generates an object from its XML representation.
        /// </summary>
        /// <param name="reader">The <see cref="T:System.Xml.XmlReader"/> stream
        /// from which the object is deserialized.</param>
        void IXmlSerializable.ReadXml(XmlReader reader)
        {
            ADP.CheckArgumentNull(reader, nameof(reader));
            this.Text = reader.ReadElementContentAsString();
        }

        /// <summary>
        /// Converts an object into its XML representation.
        /// </summary>
        /// <param name="writer">The <see cref="T:System.Xml.XmlWriter"/> stream
        /// to which the object is serialized.</param>
        void IXmlSerializable.WriteXml(XmlWriter writer)
        {
            ADP.CheckArgumentNull(writer, nameof(writer));
            writer.WriteCData(this.Text);
        }
    }
}