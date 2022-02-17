namespace Oliviann.Data.DatabaseManagement.Copy
{
    #region Usings

    using System;
    using System.Diagnostics;
    using System.IO;
    using Oliviann.Properties;

    #endregion Usings

    /// <summary>
    /// Represents a class for copying a flat file database.
    /// </summary>
    public class CopyDatabase
    {
        #region Contsructor/Destructor

        /// <summary>
        /// Initializes a new instance of the <see cref="CopyDatabase"/> class.
        /// </summary>
        /// <param name="providerType">Type of the provider.</param>
        /// <param name="sourceLocationPath">The source database location path.
        /// </param>
        /// <param name="destinationLocationPath">The destination database
        /// location path.</param>
        /// <exception cref="ArgumentException">SourceLocationPath or
        /// DestinationLocationPath can not be <c>null</c> or empty.</exception>
        public CopyDatabase(DatabaseProvider providerType, string sourceLocationPath, string destinationLocationPath)
        {
            Oliviann.ADP.CheckArgumentNullOrEmpty(sourceLocationPath, Resources.ERR_NullOrEmpty.FormatWith(nameof(sourceLocationPath)));
            Oliviann.ADP.CheckArgumentNullOrEmpty(
                                                destinationLocationPath,
                                                Resources.ERR_NullOrEmpty.FormatWith(nameof(destinationLocationPath)));

            this.Provider = providerType;
            this.SourceLocation = sourceLocationPath;
            this.DestinationLocation = destinationLocationPath;
        }

        #endregion Contsructor/Destructor

        #region Delegates/Events

        /////// <summary>
        /////// Occurs when the copy status message has changed.
        /////// </summary>
        ////public event EventHandler<Exception> CopyStatusChanged;

        #endregion Delegates/Events

        #region Properties

        /// <summary>
        /// Gets the database provider type of the databases.
        /// </summary>
        public DatabaseProvider Provider { get; private set; }

        /// <summary>
        /// Gets the destination database location path.
        /// </summary>
        public string DestinationLocation { get; private set; }

        /// <summary>
        /// Gets the source database location path.
        /// </summary>
        public string SourceLocation { get; private set; }

        #endregion Properties

        /// <summary>
        /// Copies the versions database from the server to the ETool
        /// overwriting the ETool versions database.
        /// </summary>
        /// <exception cref="NotSupportedException">The option of copying
        /// an Oracle database file is not supported. Please re-run using
        /// the Synchronize argument.</exception>
        public void Copy()
        {
            try
            {
                switch (this.Provider)
                {
                    case DatabaseProvider.MicrosoftAccess:
                    case DatabaseProvider.MicrosoftAccess12:
                    case DatabaseProvider.MicrosoftAccess14:
                    case DatabaseProvider.SQLite:
                        this.CopyFlatFileDatabase();
                        break;

                    default:
                        throw new NotSupportedException(Resources.ERR_DB_Copy.FormatWith(this.Provider));
                }
            }
            catch (NotSupportedException ex)
            {
                Trace.TraceError(ex.Message);
                ////this.CopyStatusChangedRelay(ex);
            }
        }

        /// <summary>
        /// Copies the server versions database to the client. Only to be used
        /// with file system based database.
        /// </summary>
        private void CopyFlatFileDatabase()
        {
            try
            {
                File.Copy(this.SourceLocation, this.DestinationLocation, true);
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
                ////this.CopyStatusChangedRelay(ex);
            }
        }

        /////// <summary>
        /////// Fires the copy status changed event when called if not <c>null</c>.
        /////// </summary>
        /////// <param name="e">The exception event.</param>
        ////private void CopyStatusChangedRelay(Exception e)
        ////{
        ////    this.CopyStatusChanged.RaiseEvent(this, e);
        ////}
    }
}