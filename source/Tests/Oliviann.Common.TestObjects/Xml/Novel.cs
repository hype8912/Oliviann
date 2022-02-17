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
    public class Novel
    {
        #region Properties

        [DataMember(Name = "idX")]
        [XmlAttribute("idX")]
        public int NovelId { get; set; }

        [DataMember(Name = "authorX")]
        [XmlElement("authorX")]
        public string NovelAuthor { get; set; }

        [DataMember(Name = "descriptionX")]
        [XmlElement("descriptionX")]
        public string NovelDescription { get; set; }

        [DataMember(Name = "titleX")]
        [XmlElement("titleX")]
        public string NovelTitle { get; set; }

        [DataMember(Name = "yearX")]
        [XmlElement("yearX")]
        public int NovelYear { get; set; }

        #endregion Properties
    }
}