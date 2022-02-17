namespace Oliviann.Data.ConnectionStrings
{
    #region Usings

    using System;
    using Builder;

    #endregion Usings

    /// <summary>
    /// Represents a class for creating a new Microsoft SQL Compact database
    /// connection string.
    /// </summary>
    public class SqlCompactConnectionString : IDbConnectionString
    {
        #region Constructor/Destructor

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="SqlCompactConnectionString"/> class.
        /// </summary>
        public SqlCompactConnectionString()
        {
        }

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="SqlCompactConnectionString"/> class.
        /// </summary>
        /// <param name="databaseObject">The database object.</param>
        public SqlCompactConnectionString(IDatabase databaseObject)
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
        public string ConnectionString(Func<string, string> decrypter)
        {
            Oliviann.ADP.CheckArgumentNull(this.DatabaseItem, @"databaseObject");
            ISqlConnectionStringBuilder builder = new DbConnectionStringBuilder { DataSource = this.DatabaseItem.Location };

            if (!this.DatabaseItem.UserName.IsNullOrEmpty())
            {
                builder.UserId = this.DatabaseItem.UserName;
            }

            builder.SetPassword(this.DatabaseItem.Password, decrypter);
            return builder.ConnectionString;
        }
    }
}