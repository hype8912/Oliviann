namespace Oliviann.Data
{
    #region Usings

    using System;
    using System.Xml.Serialization;

    #endregion Usings

    /// <summary>
    /// Implements a basic database model.
    /// </summary>
    public interface IDatabase : ICloneable
    {
        #region Properties

        /// <summary>
        /// Gets or sets the database name.
        /// </summary>
        /// <value>The database name.</value>
        [XmlAttribute(@"name")]
        string Name { get; set; }

        /// <summary>
        /// Gets or sets the type of the database.
        /// </summary>
        /// <value>The type of the database.</value>
        [XmlAttribute(@"type")]
        DatabaseProvider DatabaseType { get; set; }

        /// <summary>
        /// Gets or sets the location of the database.
        /// </summary>
        /// <value>The location of the database.</value>
        [XmlElement(@"location")]
        string Location { get; set; }

        /// <summary>
        /// Gets or sets the database connection port number.
        /// </summary>
        /// <value>
        /// The database connection port number.
        /// </value>
        [XmlElement(@"port")]
        int Port { get; set; }

        /// <summary>
        /// Gets or sets the database login.
        /// </summary>
        /// <value>The database login.</value>
        [XmlElement(@"username")]
        string UserName { get; set; }

        /// <summary>
        /// Gets or sets the database password.
        /// </summary>
        /// <value>The database password.</value>
        [XmlElement(@"password")]
        string Password { get; set; }

        /// <summary>
        /// Gets or sets the system database file location.
        /// </summary>
        /// <value>The system database file location.</value>
        [XmlElement(@"systemdatabase")]
        string SystemDatabase { get; set; }

        #endregion Properties
    }
}