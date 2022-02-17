namespace Oliviann.Common.TestObjects.Xml
{
    #region Usings

    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    #endregion Usings

    /// <summary>
    /// Represents a
    /// </summary>
    [DataContract]
    [Serializable]
    [ExcludeFromCodeCoverage]
    public class Book2
    {
        #region Properties

        [DataMember(Name = "id")]
        [XmlAttribute("id")]
        public int Id { get; set; }

        [DataMember(Name = "author")]
        [XmlElement("author")]
        public string Author { get; set; }

        [DataMember(Name = "description")]
        [XmlElement("description")]
        public string Description { get; set; }

        [DataMember(Name = "title")]
        [XmlElement("title")]
        public string Title { get; set; }

        [DataMember(Name = "year")]
        [XmlElement("year")]
        public uint Year { get; set; }

        #endregion Properties
    }
}