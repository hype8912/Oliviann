namespace Oliviann.Data.DatabaseManagement.Shrink
{
    #region Usings

    using System;
    using System.Data;
    using Oliviann.Data.ConnectionStrings;

    #endregion Usings

    /// <summary>
    /// Shrinks a SQL Server database using DBCC SHRINKDATABASE command.
    /// </summary>
    public class ShrinkMsSqlServer : IDbShrink
    {
        #region Contsructor/Deconstructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ShrinkMsSqlServer"/>
        /// class.
        /// </summary>
        /// <param name="databaseObject">The database object.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="databaseObject"/> is null.</exception>
        public ShrinkMsSqlServer(IDatabase databaseObject)
        {
            Oliviann.ADP.CheckArgumentNull(databaseObject, nameof(databaseObject));
            this.Database = databaseObject;
        }

        #endregion Contsructor/Deconstructor

        #region Properties

        /// <summary>
        /// Gets the database object.
        /// </summary>
        /// <value>The database object.</value>
        public IDatabase Database { get; }

        #endregion Properties

        /// <summary>
        /// Shrinks the specified database.
        /// </summary>
        /// <param name="decrypter">A function for taking in an encrypted string
        /// and returning a decrypted string.</param>
        /// <returns>
        ///   <c>True</c> if the database shrinks successfully; otherwise,
        /// <c>false</c>.
        /// </returns>
        public bool ShrinkDatabase(Func<string, string> decrypter)
        {
            var connectionString = new DbConnectionString(this.Database).ConnectionString(decrypter);
            if (connectionString.IsNullOrEmpty())
            {
                return false;
            }

            const string SQL = @"DBCC SHRINKDATABASE(N'{0}')";
            var manager = new DbManager(this.Database.DatabaseType, connectionString);
            bool result;

            try
            {
                string name = this.GetDatabaseName();
                if (name.IsNullOrEmpty())
                {
                    return false;
                }

                manager.ExecuteNonQuery(CommandType.Text, string.Format(SQL, name));
                result = true;
            }
            catch (Exception)
            {
                result = false;
            }
            finally
            {
                manager.Close();
            }

            return result;
        }

        /// <summary>
        /// Gets the name of the database.
        /// </summary>
        /// <returns>A string value representing the database name.</returns>
        /// <exception cref="IndexOutOfRangeException">
        /// Oliviann.Data.DatabaseManagement.Shrink.GetDatabaseName: Error parsing
        /// backslashes.</exception>
        private string GetDatabaseName()
        {
            string loc = this.Database.Location;
            if (!loc.Contains(@"\"))
            {
                return loc;
            }

            string[] items = loc.Split('\\');
            switch (items.Length)
            {
                case 2:
                    return items[1];

                case 3:
                    return items[2];
            }

            throw new IndexOutOfRangeException("Oliviann.Data.DatabaseManagement.Shrink.GetDatabaseName: Error parsing backslashes to get database name.");
        }
    }
}