namespace Oliviann.Data.ConnectionStrings
{
    #region Usings

    using System;
    using Builder;
    using Properties;

    #endregion Usings

    /// <summary>
    /// Class for creating a new Microsoft SQL database connection string.
    /// </summary>
    public class SqlConnectionString : IDbConnectionString
    {
        #region Constructor/Destructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlConnectionString"/>
        /// class.
        /// </summary>
        public SqlConnectionString()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlConnectionString"/>
        /// class.
        /// </summary>
        /// <param name="databaseObject">The database object.</param>
        public SqlConnectionString(IDatabase databaseObject)
        {
            this.DatabaseItem = databaseObject;
        }

        #endregion Constructor/Destructor

        #region Properties

        /// <summary>
        /// Gets or sets the database object.
        /// </summary>
        /// <value>The database object.</value>
        public IDatabase DatabaseItem { get; set; }

        #endregion Properties

        /// <summary>
        /// Gets a connection string for connecting to the specific database.
        /// </summary>
        /// <param name="decrypter">A function for taking in an encrypted string
        /// and returning a decrypted string.</param>
        /// <returns>A database connection string.</returns>
        /// <exception cref="NullReferenceException">The database object can not
        /// be <c>null</c>.</exception>
        /// <remarks>
        /// Methods allows for a named instance of SQL to be used or not. If
        /// only 1 backslash is found in the database location string then a
        /// named instance connection string will not be create, but if 2
        /// backslashes are found in the database location string then a named
        /// instance connection string will be created.
        /// </remarks>
        public string ConnectionString(Func<string, string> decrypter)
        {
            Oliviann.ADP.CheckArgumentNull(this.DatabaseItem, @"databaseObject");
            ISqlConnectionStringBuilder builder = new DbConnectionStringBuilder();
            this.SetDataSourceAndCatalog(builder);

            if (!this.DatabaseItem.UserName.IsNullOrEmpty())
            {
                builder.UserId = this.DatabaseItem.UserName;
            }

            builder.SetPassword(this.DatabaseItem.Password, decrypter);
            return builder.ConnectionString;
        }

        /// <summary>
        /// Sets the database data source and initial catalog properties.
        /// </summary>
        /// <param name="builder">The connection string builder instance.
        /// </param>
        /// <exception cref="NullReferenceException">The database location can
        /// not be <c>null</c>.</exception>
        private void SetDataSourceAndCatalog(ISqlConnectionStringBuilder builder)
        {
            string[] server = this.DatabaseItem.Location.Split('\\');
            if (server.Length < 2)
            {
                Oliviann.ADP.NullReference(Resources.ERR_LocationNotNull.FormatWith("database"));
            }

            string dataSource;
            if (server.Length == 2)
            {
                dataSource = server[0];
                builder.InitialCatalog = server[1];
            }
            else
            {
                // Sets data source and initial catalog if a named instance is
                // used.
                dataSource = string.Concat(server[0], @"\", server[1]);
                builder.InitialCatalog = server[2];
            }

            // I've never seen anyone set a connection port to zero but we're
            // just not going to allow it in our framework. We are taking it
            // as 0 is the default number if nothing was set to it.
            if (this.DatabaseItem.Port != 0 && this.DatabaseItem.Port.IsValidPort())
            {
                dataSource += ',' + this.DatabaseItem.Port;
            }

            builder.DataSource = dataSource;
        }
    }
}