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
    public class Access2003ConnectionString : IDbConnectionString
    {
        #region Constructor/Deconstructor

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="Access2003ConnectionString"/> class.
        /// </summary>
        public Access2003ConnectionString()
        {
        }

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="Access2003ConnectionString"/> class.
        /// </summary>
        /// <param name="databaseObject">The database object.</param>
        public Access2003ConnectionString(IDatabase databaseObject)
        {
            Oliviann.ADP.CheckArgumentNull(databaseObject, nameof(databaseObject));
            ADP.CheckProviderInvalid(databaseObject.DatabaseType, DatabaseProvider.MicrosoftAccess);
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
        /// Creates an access 2003 database connection string.
        /// </summary>
        /// <param name="decrypter">A function for taking in an encrypted string
        /// and returning a decrypted string.</param>
        /// <returns>
        /// An access 2003 database connection string.
        /// </returns>
        /// <exception cref="NullReferenceException">
        /// Throws exception if database object is <c>null</c> or not
        /// initialized.
        /// </exception>
        public string ConnectionString(Func<string, string> decrypter)
        {
            Oliviann.ADP.CheckArgumentNull(this.DatabaseItem, nameof(this.DatabaseItem));
            IAccess2003ConnectionStringBuilder builder = new DbConnectionStringBuilder
            {
                DataSource = this.DatabaseItem.Location,
                Provider = this.DatabaseItem.DatabaseType.GetProviderAttribute()
            };

            ////builder.CustomAttribute(@"Jet OLEDB:Database Locking Mode", @"1");
            ////builder.CustomAttribute("Mode", "16");
            ////builder.CustomAttribute("Jet OLEDB:Engine Type", "5");

            if (!this.DatabaseItem.UserName.IsNullOrEmpty())
            {
                builder.UserId = this.DatabaseItem.UserName;
            }

            builder.SetPassword(this.DatabaseItem.Password, decrypter);
            if (!this.DatabaseItem.SystemDatabase.IsNullOrEmpty())
            {
                builder.SystemDatabase = this.DatabaseItem.SystemDatabase;
            }

            return builder.ConnectionString;
        }

        #region Unused Code

        ////public string ExclusiveConnectionString(Func<string, string> decrypter)
        ////{
        ////    ADP.CheckArgumentNull(this.DatabaseItem, @"databaseObject");
        ////    IAccess2003ConnectionStringBuilder builder = new DbConnectionStringBuilder
        ////    {
        ////        DataSource = this.DatabaseItem.Location,
        ////        Provider = this.DatabaseItem.DatabaseType.GetProviderAttribute(),
        ////        UserId = @"Admin"
        ////    };

        ////    if (!this.DatabaseItem.Password.IsNullOrEmpty())
        ////    {
        ////        builder.Password = decrypter == null ? this.DatabaseItem.Password : decrypter(this.DatabaseItem.Password);
        ////    }

        ////    if (!this.DatabaseItem.SystemDatabase.IsNullOrEmpty())
        ////    {
        ////        builder.SystemDatabase = this.DatabaseItem.SystemDatabase;
        ////    }

        ////    builder.CustomAttribute(@"Mode", @"Share Deny Exclusive");
        ////    return builder.ConnectionString;
        ////}

        #endregion Unused Code
    }
}