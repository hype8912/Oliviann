namespace Oliviann.Data.DatabaseManagement.Shrink
{
    #region Usings

    using System;
    using System.Data.SqlServerCe;
    using System.IO;
    using Oliviann.Data.ConnectionStrings;
    using Oliviann.Properties;

    #endregion Usings

    /// <summary>
    /// Represents a class for shrinking a MS SQL Server Compact database.
    /// </summary>
    public class ShrinkMsSqlCompact : IDbShrink
    {
        #region Constructor/Deconstructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ShrinkMsSqlCompact"/>
        /// class.
        /// </summary>
        /// <param name="databaseObject">The database object.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="databaseObject" /> is null.</exception>
        public ShrinkMsSqlCompact(IDatabase databaseObject)
        {
            Oliviann.ADP.CheckArgumentNull(databaseObject, nameof(databaseObject));
            this.Database = databaseObject;
        }

        #endregion Constructor/Deconstructor

        #region Properties

        /// <summary>
        /// Gets the database object.
        /// </summary>
        /// <value>The database object.</value>
        public IDatabase Database { get; private set; }

        #endregion Properties

        /// <summary>
        /// Shrinks the specified database.
        /// </summary>
        /// <param name="decrypter">A function for taking in an encrypted string
        /// and returning a decrypted string.</param>
        /// <returns>
        /// True if the database shrinks successfully; otherwise, false.
        /// </returns>
        /// <exception cref="DbDataException">Throws exception when an error
        /// occurs while trying to compact the database.</exception>
        public bool ShrinkDatabase(Func<string, string> decrypter)
        {
            string connectionString = new DbConnectionString(this.Database).ConnectionString(decrypter);
            if (connectionString.IsNullOrEmpty() || !this.ValidDatabaseFiles())
            {
                return false;
            }

            SqlCeEngine engine = null;

            try
            {
                engine = new SqlCeEngine { LocalConnectionString = connectionString };

                // Specify null destination connection string for in-place compaction.
                // Specify connection string for new database options.
                engine.Compact(connectionString);
            }
            catch (Exception ex)
            {
                var inner = new DbDataException(Resources.ERR_DB_Compact.FormatWith("Microsoft SQL Server Compact"), ex);
                inner.Data.Add("LocalConnectionString", connectionString);
                throw inner;
            }
            finally
            {
                engine.DisposeSafe();
            }

            return true;
        }

        #region Helper Methods

        /// <summary>Verifies the database file exists.</summary>
        /// <returns>
        /// True if the database exists; otherwise, false.
        /// </returns>
        private bool ValidDatabaseFiles()
        {
            return !this.Database.Location.IsNullOrEmpty() && File.Exists(this.Database.Location);
        }

        #endregion Helper Methods
    }
}