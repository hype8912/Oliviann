namespace Oliviann.Data.ConnectionStrings.Builder
{
    /// <summary>
    /// Interface to build a connection string for Microsoft SQL Server Compact.
    /// </summary>
    public interface ISqlCompactConnectionStringBuilder : IDbConnectionStringBuilder
    {
        /// <summary>
        /// Sets the autoshrink threshold.
        /// </summary>
        /// <value>The autoshrink threshold.</value>
        /// <remarks>
        /// The percent of free space in the database file that is allowed
        /// before autoshrink begins. A value of 100 disables autoshrink. If not
        /// specified, the default value is 60.
        /// </remarks>
        uint AutoshrinkThreshold { set; }

        /// <summary>
        /// Sets the data source.
        /// </summary>
        /// <value>The data source.</value>
        /// <remarks>
        /// The file path and name of the SQL Server Compact 3.5 SP1 database.
        /// To indicate a relative path to the database from the application
        /// directory, use the Data Source = |DataDirectory| (enclosed in pipe
        /// symbols) substitution string. Use the SetData method on the
        /// <c>AppDomain</c> object to set the application's data directory.
        /// DataDirectory is not supported for devices.
        /// </remarks>
        string DataSource { set; }

        /// <summary>
        /// Sets the default lock escalation.
        /// </summary>
        /// <value>The default lock escalation.</value>
        /// <remarks>
        /// The number of locks a transaction will acquire before attempting
        /// escalation from row to page, or from page to table. If not
        /// specified, the default value is 100.
        /// </remarks>
        uint DefaultLockEscalation { set; }

        /// <summary>
        /// Sets the default lock timeout.
        /// </summary>
        /// <value>The default lock timeout.</value>
        /// <remarks>
        /// The default number of milliseconds that a transaction will wait for
        /// a lock. If not specified, the default value is 2000.
        /// </remarks>
        uint DefaultLockTimeout { set; }

        /// <summary>
        /// Sets a value indicating whether this
        /// <see cref="DbConnectionStringBuilder"/> is encrypt.
        /// </summary>
        /// <value><c>True</c> if encrypt; otherwise, <c>false</c>.</value>
        bool Encrypt { set; }

        /// <summary>
        /// Sets a value indicating whether this
        /// <see cref="DbConnectionStringBuilder"/> is enlist.
        /// </summary>
        /// <value><c>True</c> if enlist; otherwise, <c>false</c>.</value>
        /// <remarks>
        /// By default, enlist value is <c>false</c>. This can be set to
        /// <c>true</c>. If a connection to SQL Server Compact 3.5 SP1 database
        /// is opened by using Enlist set to <c>true</c>, the connection is
        /// promoted to a transaction.
        /// </remarks>
        bool Enlist { set; }

        /// <summary>
        /// Sets the flush interval.
        /// </summary>
        /// <value>The flush interval.</value>
        /// <remarks>
        /// Specified the interval time (in seconds) before all committed
        /// transactions are flushed to disk. If not specified, the default
        /// value is 10.
        /// </remarks>
        uint FlushInterval { set; }

        /// <summary>
        /// Sets the maximum size of the buffer.
        /// </summary>
        /// <value>The maximum size of the buffer.</value>
        /// <remarks>
        /// The largest amount of memory, in kilobytes, that SQL Server Compact
        /// 3.5 SP1 can use before it starts flushing changes to disk. If not
        /// specified, the default value is 640.
        /// </remarks>
        int MaximumBufferSize { set; }

        /// <summary>
        /// Sets the maximum size of the database.
        /// </summary>
        /// <value>The maximum size of the database.</value>
        /// <remarks>
        /// The maximum size of the database, in Megabytes. If not specified,
        /// the default value is 128.
        /// </remarks>
        int MaximumDatabaseSize { set; }

        /// <summary>
        /// Sets the file access mode.
        /// </summary>
        /// <value>The file access mode.</value>
        /// <remarks>
        /// The mode to use when opening the database file. If not specified,
        /// the default value is 'Read Write'.
        /// </remarks>
        FileMode Mode { set; }

        /// <summary>
        /// Sets the password.
        /// </summary>
        /// <value>The password.</value>
        /// <remarks>
        /// The database password, which can be up to 40 characters in length.
        /// If not specified, the default value is no password. This property is
        /// required if you enable encryption on the database. If you specify a
        /// password, encryption is automatically enabled on the database. If
        /// you specify a blank password, the database will not be encrypted.
        /// </remarks>
        string Password { set; }

        /// <summary>
        /// Sets a value indicating whether [persist security info].
        /// </summary>
        /// <value><c>True</c> if [persist security info]; otherwise,
        /// <c>false</c>.</value>
        /// <remarks>
        /// When set to <c>false</c> (which is strongly recommended),
        /// security-sensitive information, such as the password, is not
        /// returned as part of the connection if the connection is open or has
        /// ever been in an open state. Resetting the connection string resets
        /// all connection string values, including the password. The default
        /// value is <c>false</c>.
        /// </remarks>
        bool PersistSecurityInfo { set; }

        /// <summary>
        /// Sets the size of the temp file max.
        /// </summary>
        /// <value>The size of the temp file max.</value>
        /// <remarks>
        /// The maximum size of the temporary database file, in Megabytes. If
        /// not specified, the default value is 128.
        /// </remarks>
        int TempFileMaxSize { set; }

        /// <summary>
        /// Sets the temp path.
        /// </summary>
        /// <value>The temp path.</value>
        /// <remarks>
        /// The location of the temporary database. If not specified, the
        /// default is to use the database specified in the data source property
        /// for temporary storage.
        /// </remarks>
        string TempPath { set; }
    }
}