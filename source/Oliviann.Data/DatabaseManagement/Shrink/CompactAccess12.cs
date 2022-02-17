namespace Oliviann.Data.DatabaseManagement.Shrink
{
    #region Usings

    using System;
    using System.IO;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using Oliviann.Properties;

    #endregion Usings

    /// <summary>
    /// Represents a class for compacting a Microsoft Access 2007 database.
    /// </summary>
    public class CompactAccess12 : IDbShrink
    {
        #region Contsructor/Deconstructor

        /// <summary>
        /// Initializes a new instance of the <see cref="CompactAccess12"/> class.
        /// </summary>
        /// <param name="databaseObject">The database object.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="databaseObject"/> is <c>null</c>.</exception>
        public CompactAccess12(IDatabase databaseObject)
        {
            Oliviann.ADP.CheckArgumentNull(databaseObject, @"databaseObject");
            this.Database = databaseObject;
        }

        #endregion Contsructor/Deconstructor

        #region Properties

        /// <summary>
        /// Gets the database object.
        /// </summary>
        /// <value>The database object.</value>
        public IDatabase Database { get; private set; }

        /// <summary>
        /// Gets or sets the destination database.
        /// </summary>
        /// <value>The destination database.</value>
        private IDatabase DestinationDatabase { get; set; }

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
        /// <exception cref="DbDataException">The specified database can not be
        /// compacted because it is in use by another application.</exception>
        public bool ShrinkDatabase(Func<string, string> decrypter)
        {
            if (!VerifyDatabaseFiles(this.Database.Location))
            {
                var inner = new DbDataException(Resources.ERR_DB_CanNotCompact.FormatWith(this.Database.Name), inner: null);
                throw inner;
            }

            this.CreateDestinationDatabaseObject();

            try
            {
                File.Delete(this.DestinationDatabase.Location);
            }
            catch (Exception ex)
            {
                var outer = new DbDataException(Resources.ERR_DB_CanNotDelete.FormatWith(this.Database.Name), ex);
                throw outer;
            }

            bool result = CompactAceDatabase(this.Database.Location, this.DestinationDatabase.Location, this.Database.Password);
            if (result)
            {
                File.Delete(this.Database.Location);
                File.Move(this.DestinationDatabase.Location, this.Database.Location);
            }
            else
            {
                File.Delete(this.DestinationDatabase.Location);
            }

            return result;
        }

        /// <summary>
        /// Compacts a Microsoft Access 2007 Database using
        /// Microsoft.Office.Interop.Access.DAO database engine.
        /// </summary>
        /// <param name="oldDatabaseLocation">The old database location path.
        /// </param>
        /// <param name="newDatabaseLocation">The new database location path.
        /// </param>
        /// <param name="password">The password for compacting the database if
        /// the database is password protected.</param>
        /// <returns>
        ///   <c>True</c> if the database compacted successfully; otherwise,
        ///   <c>false</c>.
        /// </returns>
        /// <exception cref="DbDataException">An error occurred compacting the
        /// specified database.</exception>
        private static bool CompactAceDatabase(string oldDatabaseLocation, string newDatabaseLocation, string password = @"")
        {
            object engine = null;

            try
            {
                engine = Type.GetTypeFromProgID(@"DAO.DBEngine.120").CreateInstance();
                var parms = password.IsNullOrEmpty()
                                ? new object[] { oldDatabaseLocation, newDatabaseLocation, @"dbLangGeneral" }
                                : new object[] { oldDatabaseLocation, newDatabaseLocation, @"dbLangGeneral", null, @";pwd=" + password };
                engine.GetType().InvokeMember(@"CompactDatabase", BindingFlags.InvokeMethod, null, engine, parms);
            }
            catch (Exception ex)
            {
                var inner = new DbDataException(Resources.ERR_DB_Compact.FormatWith(@"Microsoft Access 2007"), ex);
                inner.Data.Add(@"oldDatabaseLocation", oldDatabaseLocation);
                inner.Data.Add(@"newDatabaseLocation", newDatabaseLocation);
                throw inner;
            }
            finally
            {
                if (engine != null)
                {
                    Marshal.ReleaseComObject(engine);
                }
            }

            return true;
        }

        #region Helper Methods

        /// <summary>
        /// Verifies the database files.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <returns><c>True</c> if the database exists and isn't locked;
        /// otherwise, <c>false</c>.</returns>
        private static bool VerifyDatabaseFiles(string filePath)
        {
            string folderPath = Path.GetDirectoryName(filePath) + @"\";
            return File.Exists(filePath) && !File.Exists(folderPath + Path.GetFileNameWithoutExtension(filePath) + @".ldb");
        }

        /// <summary>
        /// Creates the destination database object.
        /// </summary>
        private void CreateDestinationDatabaseObject()
        {
            var newDbObject = (IDatabase)this.Database.Clone();
            newDbObject.Location = Path.GetDirectoryName(this.Database.Location) + @"\" +
                                   Path.GetFileNameWithoutExtension(this.Database.Location) + @"_TEMP" +
                                   Path.GetExtension(this.Database.Location);
            this.DestinationDatabase = newDbObject;
        }

        #endregion Helper Methods
    }
}