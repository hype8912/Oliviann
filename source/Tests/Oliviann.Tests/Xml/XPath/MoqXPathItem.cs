namespace Oliviann.Tests.Xml.XPath
{
    #region Usings

    using System;
    using System.Xml;
    using System.Xml.Schema;
    using System.Xml.XPath;

    #endregion Usings

    /// <summary>
    /// Represents a
    /// </summary>
    public class MoqXPathItem : XPathItem
    {
        private readonly string _value;

        public MoqXPathItem(string text)
        {
            this._value = text;
        }

        public override object ValueAs(Type returnType, IXmlNamespaceResolver nsResolver)
        {
            throw new NotImplementedException();
        }

        public override bool IsNode
        {
            get { throw new NotImplementedException(); }
        }

        public override XmlSchemaType XmlType
        {
            get { throw new NotImplementedException(); }
        }

        public override string Value
        {
            get { return this._value; }
        }

        public override object TypedValue
        {
            get { throw new NotImplementedException(); }
        }

        public override Type ValueType
        {
            get { throw new NotImplementedException(); }
        }

        public override bool ValueAsBoolean
        {
            get { throw new NotImplementedException(); }
        }

        public override DateTime ValueAsDateTime
        {
            get { throw new NotImplementedException(); }
        }

        public override double ValueAsDouble
        {
            get { throw new NotImplementedException(); }
        }

        public override int ValueAsInt
        {
            get { throw new NotImplementedException(); }
        }

        public override long ValueAsLong
        {
            get { throw new NotImplementedException(); }
        }
    }
}