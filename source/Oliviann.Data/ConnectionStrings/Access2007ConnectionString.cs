namespace Oliviann.Data.ConnectionStrings
{
    #region Usings

    using System;
    using Oliviann.Data.ConnectionStrings.Builder;

    #endregion Usings

    /// <summary>
    /// Class for creating a new Microsoft Access database connection string
    /// for version 2003 and less.
    /// </summary>
    public class Access2007ConnectionString : IDbConnectionString
    {
        #region Constructor/Deconstructor

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="Access2007ConnectionString"/> class.
        /// </summary>
        public Access2007ConnectionString()
        {
        }

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="Access2007ConnectionString"/> class.
        /// </summary>
        /// <param name="databaseObject">The database object.</param>
        public Access2007ConnectionString(IDatabase databaseObject)
        {
            Oliviann.ADP.CheckArgumentNull(databaseObject, nameof(databaseObject));
            ADP.CheckProviderInvalid(databaseObject.DatabaseType, DatabaseProvider.MicrosoftAccess12);
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
        /// Creates an access 2007 database connection string.
        /// </summary>
        /// <param name="decrypter">A function for taking in an encrypted string
        /// and returning a decrypted string.</param>
        /// <returns>
        /// An access 2007 database connection string.
        /// </returns>
        /// <exception cref="NullReferenceException">
        /// Throws exception if database object is <c>null</c> or not
        /// initialized.
        /// </exception>
        public string ConnectionString(Func<string, string> decrypter)
        {
            Oliviann.ADP.CheckArgumentNull(this.DatabaseItem, @"DatabaseItem");
            IAccess2007ConnectionStringBuilder builder = new DbConnectionStringBuilder
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