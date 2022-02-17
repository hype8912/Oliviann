namespace Oliviann.Data
{
    /// <summary>
    /// Represents an internal database object implementing the
    /// <see cref="IDatabase"/> interface.
    /// </summary>
    internal class InternalDatabase : IDatabase
    {
        #region Properties

        /// <summary>
        /// Gets or sets the database name.
        /// </summary>
        /// <value>The database name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the type of the database.
        /// </summary>
        /// <value>The type of the database.</value>
        public DatabaseProvider DatabaseType { get; set; }

        /// <summary>
        /// Gets or sets the location of the database.
        /// </summary>
        /// <value>The location of the database.</value>
        public string Location { get; set; }

        /// <summary>
        /// Gets or sets the database connection port number.
        /// </summary>
        /// <value>
        /// The database connection port number.
        /// </value>
        public int Port { get; set; }

        /// <summary>
        /// Gets or sets the database login.
        /// </summary>
        /// <value>The database login.</value>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the database password.
        /// </summary>
        /// <value>The database password.</value>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the system database file location.
        /// </summary>
        /// <value>The system database file location.</value>
        public string SystemDatabase { get; set; }

        #endregion Properties

        #region Cloneable

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>
        /// A new object that is a copy of this instance.
        /// </returns>
        public object Clone() => this.MemberwiseClone();

        #endregion Cloneable

        #region Overrides

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString() => this.Name;

        #endregion Overrides
    }
}