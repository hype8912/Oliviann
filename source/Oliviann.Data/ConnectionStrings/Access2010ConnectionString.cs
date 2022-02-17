namespace Oliviann.Data.ConnectionStrings
{
    #region Usings

    using System;
    using Oliviann.Data.ConnectionStrings.Builder;

    #endregion Usings

    /// <summary>
    /// Class for creating a new Microsoft Access database connection string
    /// for version 2010.
    /// </summary>
    public class Access2010ConnectionString : IDbConnectionString
    {
        #region Constructor/Deconstructor

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="Access2010ConnectionString"/> class.
        /// </summary>
        public Access2010ConnectionString()
        {
        }

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="Access2010ConnectionString"/> class.
        /// </summary>
        /// <param name="databaseObject">The database object.</param>
        public Access2010ConnectionString(IDatabase databaseObject)
        {
            Oliviann.ADP.CheckArgumentNull(databaseObject, nameof(databaseObject));
            ADP.CheckProviderInvalid(databaseObject.DatabaseType, DatabaseProvider.MicrosoftAccess14);
            this.DatabaseItem = databaseObject;
        }

        #endregion Constructor/Deconstructor

        #region Properties

        /// <summary>
        /// Gets or sets the database object.
        /// </summary>
        /// <value>The database object.</value>
        public IDatabase DatabaseItem { get; set; }

        #endregion Properties

        /// <summary>
        /// Creates an access 2010 database connection string.
        /// </summary>
        /// <param name="decrypter">A function for taking in an encrypted string
        /// and returning a decrypted string.</param>
        /// <returns>
        /// An access 2010 database connection string.
        /// </returns>
        /// <exception cref="NullReferenceException">
        /// Throws exception if database object is <c>null</c> or not initialized.
        /// </exception>
        public string ConnectionString(Func<string, string> decrypter)
        {
            Oliviann.ADP.CheckArgumentNull(this.DatabaseItem, @"DatabaseItem");
            IAccess2010ConnectionStringBuilder builder = new DbConnectionStringBuilder
            {
                DataSource = this.DatabaseItem.Location,
                Provider = this.DatabaseItem.DatabaseType.GetProviderAttribute()
            };

            if (!this.DatabaseItem.UserName.IsNullOrEmpty())
            {
                builder.UserId = this.DatabaseItem.UserName;
            }

            builder.SetPassword(this.DatabaseItem.Password, decrypter);
            return builder.ConnectionString;
        }
    }
}