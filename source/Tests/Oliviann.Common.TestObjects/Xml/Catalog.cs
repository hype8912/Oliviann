namespace Oliviann.Common.TestObjects.Xml
{
    #region Usings

    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Xml.Serialization;

    #endregion Usings

    /// <summary>
    /// Represents a
    /// </summary>
    [Serializable]
    [XmlRoot(ElementName = "catalog")]
    [ExcludeFromCodeCoverage]
    public class Catalog
    {
        #region Constructor/Destructor

        /// <summary>
        /// Initializes a new instance of the <see cref="Catalog"/> class.
        /// </summary>
        public Catalog()
        {
            this.Books = new List<Book>();
        }

        #endregion Constructor/Destructor

        #region Properties

        [XmlArray("books")]
        [XmlArrayItem(ElementName = "book")]
        public List<Book> Books { get; set; }

        #endregion Properties
    }
}