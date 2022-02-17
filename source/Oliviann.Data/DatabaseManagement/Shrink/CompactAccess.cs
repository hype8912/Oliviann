namespace Oliviann.Data.DatabaseManagement.Shrink
{
    #region Usings

    using System;
    using System.IO;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using ConnectionStrings;
    using Properties;

    #endregion Usings

    /// <summary>
    /// Represents a class for compacting a Microsoft Access 97-2003 database.
    /// </summary>
    /// <remarks>If used on a Microsoft Access 2007 or above database engine,
    /// Jet will down convert the database to Microsoft Access 2003 database
    /// format.</remarks>
    public class CompactAccess : IDbShrink
    {
        #region Contsructor/Deconstructor

        /// <summary>
        /// Initializes a new instance of the <see cref="CompactAccess"/> class.
        /// </summary>
        /// <param name="databaseObject">The database object.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="databaseObject"/> is <c>null</c>.</exception>
        public CompactAccess(IDatabase databaseObject)
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
            File.Delete(this.DestinationDatabase.Location);

            string oldConnectionString = new DbConnectionString(this.Database).ConnectionString(decrypter);
            string newConnectionString = new DbConnectionString(this.DestinationDatabase).ConnectionString(decrypter);
            bool result = CompactJetDatabase(oldConnectionString, newConnectionString);
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
        /// Compacts a Microsoft Access 97-2003 Database using Jet.
        /// </summary>
        /// <param name="oldConnectionString">The old connection string.</param>
        /// <param name="newConnectionString">The new connection string.</param>
        /// <returns><c>True</c> if the database compacted successfully;
        /// otherwise, <c>false</c>.</returns>
        /// <exception cref="DbDataException">An error occurred compacting the
        /// specified database.</exception>
        private static bool CompactJetDatabase(string oldConnectionString, string newConnectionString)
        {
            object engine = null;

            try
            {
                engine = Type.GetTypeFromProgID("JRO.JetEngine").CreateInstance();
                engine.GetType().InvokeMember(
                    "CompactDatabase",
                    BindingFlags.InvokeMethod,
                    null,
                    engine,
                    new object[] { oldConnectionString, newConnectionString });
            }
            catch (Exception ex)
            {
                var inner = new DbDataException(Resources.ERR_DB_Compact.FormatWith(@"Microsoft Access 97 - 2003"), ex);
                inner.Data.Add("oldConnectionString", oldConnectionString);
                inner.Data.Add("newConnectionString", newConnectionString);
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
            var folderPath = Path.GetDirectoryName(filePath) + @"\";
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