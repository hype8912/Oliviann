namespace Oliviann.Data.ConnectionStrings
{
    #region Usings

    using System;
    using Builder;

    #endregion Usings

    /// <summary>
    /// Class for creating a new Microsoft Excel database connection string
    /// for version 2003 and less.
    /// </summary>
    public class Excel2003ConnectionString : IDbConnectionString
    {
        #region Constructor/Deconstructor

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="Excel2003ConnectionString"/> class.
        /// </summary>
        public Excel2003ConnectionString()
        {
        }

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="Excel2003ConnectionString"/> class.
        /// </summary>
        /// <param name="databaseObject">The database object.</param>
        public Excel2003ConnectionString(IDatabase databaseObject)
        {
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
        /// Creates an database connection string to an Microsoft Excel 2003 or
        /// less workbook sheet for extracting data with no header columns.
        /// </summary>
        /// <param name="decrypter">A function for taking in an encrypted string
        /// and returning a decrypted string.</param>
        /// <returns>
        /// An excel 2003 database connection string.
        /// </returns>
        public string ConnectionString(Func<string, string> decrypter)
        {
            Oliviann.ADP.CheckArgumentNull(this.DatabaseItem, @"databaseObject");
            IExcel2003ConnectionStringBuilder builder = new DbConnectionStringBuilder
            {
                DataSource = this.DatabaseItem.Location,
                Provider = this.DatabaseItem.DatabaseType.GetProviderAttribute()
            };

            builder.CustomAttribute(@"Extended Properties", @"""Excel 8.0;HDR=NO;""");

            return builder.ConnectionString;
        }
    }
}