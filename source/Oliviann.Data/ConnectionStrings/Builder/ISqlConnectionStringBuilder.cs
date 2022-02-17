namespace Oliviann.Data.ConnectionStrings.Builder
{
    /// <summary>
    /// Interface for creating a Microsoft SQL Server connection string.
    /// </summary>
    public interface ISqlConnectionStringBuilder : IDbConnectionStringBuilder
    {
        /// <summary>
        /// Sets the name of the application accessing the database.
        /// </summary>
        /// <value>
        /// The name of the application.
        /// </value>
        /// <remarks>
        /// An application name can be 128 characters or less.
        /// </remarks>
        string ApplicationName { set; }

        /// <summary>
        /// Sets a value indicating whether [asynchronous processing].
        /// </summary>
        /// <value>
        ///     <c>True</c> if [asynchronous processing]; otherwise,
        ///     <c>false</c>.
        /// </value>
        bool AsynchronousProcessing { set; }

        /// <summary>
        /// Sets the name of the database file to be attached.
        /// </summary>
        /// <value>
        /// The name of the database file to attach.
        /// </value>
        /// <remarks>
        /// Remote server, HTTP, and UNC path names are not supported.
        /// </remarks>
        string AttachDbFileName { set; }

        /// <summary>
        /// Sets the connection lifetime.
        /// </summary>
        /// <value>The connection lifetime.</value>
        /// <remarks>
        /// A value of zero (0) causes pooled connections to have the maximum connection timeout.
        /// </remarks>
        int ConnectionLifetime { set; }

        /// <summary>
        /// Sets the length of time (in seconds) to wait for a connection to the
        /// server before terminating the attempt and generating an error.
        /// </summary>
        /// <value>
        /// The connection timeout (in seconds).
        /// </value>
        /// <remarks>
        /// Valid values are greater than or equal to 0 and less than or equal
        /// to 2147483647.
        /// </remarks>
        int ConnectionTimeout { set; }

        /// <summary>
        /// Sets a value indicating whether an in-process connection to SQL
        /// Server should be made.
        /// </summary>
        /// <value>
        ///   <c>True</c> if an in-process connection to SQL Server should be
        ///   made; otherwise, <c>false</c>.
        /// </value>
        bool ContextConnection { set; }

        /// <summary>
        /// Sets the language used for database server warning or error
        /// messages.
        /// </summary>
        /// <value>
        /// The current language.
        /// </value>
        /// <remarks>The database name can be 128 characters or less.</remarks>
        string CurrentLanguage { set; }

        /// <summary>
        /// Sets the name or network address of the instance of SQL Server to
        /// which to connect.
        /// </summary>
        /// <value>The database to connect to.</value>
        string DataSource { set; }

        /// <summary>
        /// Sets a value indicating whether SQL Server uses SSL encryption for
        /// all data sent between the client and server if the server has a
        /// certificate installed.
        /// </summary>
        /// <value>
        ///   <c>True</c> if SSL encryption should be used for communication;
        ///   otherwise, <c>false</c>.
        /// </value>
        bool Encrypt { set; }

        /// <summary>
        /// Sets a value indicating whether  the SQL Server connection pooler
        /// automatically enlists the connection in the creation thread's
        /// current transaction context.
        /// </summary>
        /// <value>
        ///   <c>True</c> if the connection should automatically be pooled;
        ///   otherwise, <c>false</c>.
        /// </value>
        bool Enlist { set; }

        /// <summary>
        /// Sets the name of the database.
        /// </summary>
        /// <value>The name of the database.</value>
        /// <remarks>The database name can be 128 characters or less.</remarks>
        string InitialCatalog { set; }

        /// <summary>
        /// Sets a value indicating whether User ID and Password are specified
        /// in the connection or to use the current Windows account credentials
        /// for authentication.
        /// </summary>
        /// <value>When <c>false</c>, User ID and Password are specified in the
        /// connection. When <c>true</c>, the current Windows account
        /// credentials are used for authentication.</value>
        bool IntegratedSecurity { set; }

        /// <summary>
        /// Sets the minimum number of connections that are allowed in the pool.
        /// </summary>
        /// <value>The minimum number of connections that are allowed in the
        /// pool.</value>
        /// <remarks>Values that are greater than Max Pool Size generate an
        /// error.</remarks>
        int MinimumPoolSize { set; }

        /// <summary>
        /// Sets the maximum number of connections that are allowed in the pool.
        /// Valid values are greater than or equal to 1.
        /// </summary>
        /// <value>The maximum number of connections that are allowed in the
        /// pool.</value>
        /// <remarks>Values that are less than Min Pool Size generate an error.
        /// </remarks>
        int MaximumPoolSize { set; }

        /// <summary>
        /// Sets a value indicating whether an application can maintain multiple
        /// active result sets (MARS).
        /// </summary>
        /// <value>
        /// When <c>true</c>, an application can maintain multiple active result
        /// sets (MARS). When <c>false</c>, an application must process or
        /// cancel all result sets from one batch before it can execute any
        /// other batch on that connection.
        /// </value>
        bool MultipleActiveResultSets { set; }

        /// <summary>
        /// Sets the network library used to establish a connection to an
        /// instance of SQL Server.
        /// </summary>
        /// <value>
        /// The network library used to establish a connection to an instance of
        /// SQL Server.
        /// </value>
        /// <example>
        ///     <code>
        ///         dbnmpntw (Named Pipes)
        ///         dbmsrpcn (Multiprotocol, Windows RPC)
        ///         dbmsadsn (Apple Talk)
        ///         dbmsgnet (VIA)
        ///         dbmslpcn (Shared Memory)
        ///         dbmsspxn (IPX/SPX)
        ///         dbmssocn (TCP/IP)
        ///         Dbmsvinn (Banyan Vines)
        ///     </code>
        /// </example>
        string NetworkLibrary { set; }

        /// <summary>
        /// Sets the size in bytes of the network packets used to communicate
        /// with an instance of SQL Server.
        /// </summary>
        /// <value>
        /// The size in bytes of the network packets used to communicate with an
        /// instance of SQL Server.
        /// </value>
        /// <remarks>
        /// The packet size can be greater than or equal to 512 and less than or equal to 32767.
        /// </remarks>
        short PacketSize { set; }

        /// <summary>
        /// Sets the password for the SQL Server account logging on.
        /// </summary>
        /// <value>The password for the SQL Server account logging on.</value>
        /// <remarks>The password must be 128 characters or less.</remarks>
        string Password { set; }

        /// <summary>
        /// Sets a value indicating whether security-sensitive information, such
        /// as the password, is not returned as part of the connection if the
        /// connection is open or has ever been in an open state. Resetting the
        /// connection string resets all connection string values including the
        /// password.
        /// </summary>
        /// <value>
        ///   When set to <c>false</c> (strongly recommended),
        ///   security-sensitive information, such as the password, is not
        ///   returned as part of the connection if the connection is open or
        ///   has ever been in an open state.
        /// </value>
        bool PersistSecurityInfo { set; }

        /// <summary>
        /// Sets a value indicating whether any newly created connection will be
        /// added to the pool when closed by the application. In a next attempt
        /// to open the same connection, that connection will be drawn from the
        /// pool.
        /// </summary>
        /// <value><c>True</c> if connection polling is enabled; otherwise,
        /// <c>false</c>.</value>
        bool Pooling { set; }

        /// <summary>
        /// Sets a value indicating whether replication is supported using the
        /// connection.
        /// </summary>
        /// <value>
        ///   <c>True</c> if replication is supported using the connection;
        ///   otherwise, <c>false</c>.
        /// </value>
        bool Replication { set; }

        /// <summary>
        /// Sets a value indicating whether SSL is used to encrypt the channel
        /// when bypassing walking the certificate chain to validate trust.
        /// </summary>
        /// <value>
        ///     <c>True</c> when SSL is used to encrypt the channel when bypassing
        ///     walking the certificate chain to validate trust; otherwise,
        ///     <c>false</c>.
        /// </value>
        bool TrustServerCertificate { set; }

        /// <summary>
        /// Sets a value that indicates the type system the application expects.
        /// </summary>
        /// <value>
        /// The value that indicates the type system the application expects.
        /// </value>
        /// <example>
        /// Type System Version=SQL Server 2000;
        /// Type System Version=SQL Server 2005;
        /// Type System Version=SQL Server 2008;
        /// Type System Version=Latest;
        /// </example>
        string TypeSystemVersion { set; }

        /// <summary>
        /// Sets the SQL Server login account.
        /// </summary>
        /// <value>The SQL Server login account.</value>
        /// <remarks>The user ID must be 128 characters or less.</remarks>
        string UserId { set; }

        /// <summary>
        /// Sets a value indicating whether to redirect the connection from the
        /// default SQL Server Express instance to a runtime-initiated instance
        /// running under the account of the caller.
        /// </summary>
        /// <value>
        ///   <c>True</c> if user instance redirection is enabled; otherwise,
        ///   <c>false</c>.
        /// </value>
        bool UserInstance { set; }

        /// <summary>
        /// Sets the name of the workstation connecting to SQL Server.
        /// </summary>
        /// <value>the name of the workstation connecting to SQL Server.</value>
        /// <remarks>
        /// The ID must be 128 characters or less.
        /// </remarks>
        string WorkstationId { set; }
    }
}