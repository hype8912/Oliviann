namespace Oliviann.Data.ConnectionStrings
{
    #region Usings

    using System;

    #endregion Usings

    /// <summary>
    /// Represents an entry point for creating a new database connection string.
    /// </summary>
    public class DbConnectionString : IDbConnectionString
    {
        #region Constructor/Destructor

        /// <summary>
        /// Initializes a new instance of the <see cref="DbConnectionString"/>
        /// class.
        /// </summary>
        public DbConnectionString()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DbConnectionString"/>
        /// class using the specified database instance.
        /// </summary>
        /// <param name="databaseObject">The database object.</param>
        public DbConnectionString(IDatabase databaseObject)
        {
            this.DatabaseItem = databaseObject;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DbConnectionString"/>
        /// class.
        /// </summary>
        /// <param name="name">The name of the database.</param>
        /// <param name="provider">The type of the database.</param>
        /// <param name="location">The location of the database.</param>
        /// <param name="userName">The database user login.</param>
        /// <param name="password">The database password.</param>
        /// <param name="systemDatabase">The system database file location.
        /// </param>
        public DbConnectionString(string name, DatabaseProvider provider, string location, string userName, string password, string systemDatabase)
        {
            this.DatabaseItem = new InternalDatabase
                                    {
                                        Name = name,
                                        DatabaseType = provider,
                                        Location = location,
                                        UserName = userName,
                                        Password = password,
                                        SystemDatabase = systemDatabase
                                    };
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
        /// <param name="passwordDecrypter">A function for taking in an
        /// encrypted string and returning a decrypted string of the database
        /// connection password.</param>
        /// <returns>
        /// A database connection string.
        /// </returns>
        public string ConnectionString(Func<string, string> passwordDecrypter)
        {
            Oliviann.ADP.CheckArgumentNull(this.DatabaseItem, @"databaseObject");
            return DbConnectionStringFactory.CreateConnectionString(this.DatabaseItem)?.ConnectionString(passwordDecrypter);
        }
    }
}