namespace Oliviann.Data
{
    #region Usings

    using System;

    #endregion Usings

    /// <summary>
    /// Represents an abstract class for a database object to build from.
    /// </summary>
    [Serializable]
    public abstract class DatabaseBase : IDisposable
    {
        #region Properties

        /// <summary>
        /// Gets or sets the database connection string.
        /// </summary>
        /// <value>The database connection string.</value>
        protected string ConnectionString { get; set; }

        /// <summary>
        /// Gets or sets the database provider.
        /// </summary>
        /// <value>The database provider.</value>
        protected DatabaseProvider Provider { get; set; }

        /// <summary>
        /// Gets or sets the database manager instance.
        /// </summary>
        /// <value>The database manager instance.</value>
        protected IDbManager Manager { get; set; }

        #endregion Properties

        /// <summary>
        /// Performs application-defined tasks associated with freeing,
        /// releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            this.Manager?.Dispose();
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Gets the default instance of the database manager if not <c>null</c>
        /// ; otherwise, it creates a new instance and returns the new database
        /// manager instance.
        /// </summary>
        /// <returns>The database manager instance.</returns>
        protected IDbManager GetManager()
        {
            return this.Manager ??= new DbManager(this.Provider, this.ConnectionString);
        }

        /// <summary>
        /// Creates a instance of the current database manager if not
        /// <c>null</c>; otherwise, creates a new database manager using the
        /// current connection string and provider.
        /// </summary>
        /// <returns>A newly created database manager.</returns>
        protected IDbManager CreateManager()
        {
            return this.Manager != null
                       ? new DbManager { ConnectionString = this.Manager.ConnectionString, Provider = this.Manager.Provider }
                       : new DbManager(this.Provider, this.ConnectionString);
        }
    }
}