namespace Oliviann.Data.ConnectionStrings.Builder
{
    #region Usings

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Oliviann.Collections.Generic;

    #endregion Usings

    /// <summary>
    /// Provides a base class for strongly typed connection string builders.
    /// </summary>
    public sealed class DbConnectionStringBuilder : IDbConnectionStringBuilder,
                                                    ISqlConnectionStringBuilder,
                                                    IAccess2003ConnectionStringBuilder,
                                                    IAccess2007ConnectionStringBuilder,
                                                    IAccess2010ConnectionStringBuilder,
                                                    IOracleConnectionStringBuilder,
                                                    ISqlCompactConnectionStringBuilder,
                                                    IExcel2003ConnectionStringBuilder
    {
        #region Fields

        /// <summary>
        /// Represents an escape character signaling to not decrypted a string
        /// that begin and ends with this character.
        /// </summary>
        private const char DecryptEscapeCharacter = '\'';

        /// <summary>
        /// Place holder for connection string parameters.
        /// </summary>
        private readonly IDictionary<string, string> parameters;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="DbConnectionStringBuilder"/> class.
        /// </summary>
        public DbConnectionStringBuilder() => this.parameters = new Dictionary<string, string>();

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Sets the name of the application accessing the database.
        /// </summary>
        /// <value>
        /// The name of the application.
        /// </value>
        /// <remarks>
        /// An application name can be 128 characters or less.
        /// </remarks>
        public string ApplicationName
        {
            set { this.AddValue(@"Application Name", value); }
        }

        /// <summary>
        /// Sets a value indicating whether [asynchronous processing].
        /// </summary>
        /// <value>
        ///     <c>True</c> if [asynchronous processing]; otherwise, <c>false</c>.
        /// </value>
        public bool AsynchronousProcessing
        {
            set { this.AddValue(@"Asynchronous Processing", value.ToString()); }
        }

        /// <summary>
        /// Sets the name of the database file to be attached.
        /// </summary>
        /// <value>
        /// The name of the database file to attach.
        /// </value>
        /// <remarks>
        /// Remote server, HTTP, and UNC path names are not supported.
        /// </remarks>
        public string AttachDbFileName
        {
            set { this.AddValue(@"AttachDBFilename", value); }
        }

        /// <summary>
        /// Sets the auto shrink threshold.
        /// </summary>
        /// <value>The auto shrink threshold.</value>
        public uint AutoshrinkThreshold
        {
            set { this.AddValue(@"Autoshrink Threshold", value.ToString()); }
        }

        /// <summary>
        /// Sets the connection lifetime.
        /// </summary>
        /// <value>The connection lifetime.</value>
        public int ConnectionLifetime
        {
            set { this.AddValue(@"Connection Lifetime", value.ToString()); }
        }

        /// <summary>
        /// Sets the connection timeout.
        /// </summary>
        /// <value>The connection timeout.</value>
        public int ConnectionTimeout
        {
            set { this.AddValue(@"Connection Timeout", value.ToString()); }
        }

        /// <summary>
        /// Sets a value indicating whether an in-process connection to SQL
        /// Server should be made.
        /// </summary>
        /// <value>
        ///   <c>True</c> if an in-process connection to SQL Server should be
        ///   made; otherwise, <c>false</c>.
        /// </value>
        public bool ContextConnection
        {
            set { this.AddValue(@"Context Connection", value.ToString()); }
        }

        /// <summary>
        /// Sets the language used for database server warning or error
        /// messages.
        /// </summary>
        /// <value>
        /// The current language.
        /// </value>
        /// <remarks>The database name can be 128 characters or less.</remarks>
        public string CurrentLanguage
        {
            set { this.AddValue(@"Current Language", value); }
        }

        /// <summary>
        /// Sets the character set.
        /// </summary>
        /// <value>The character set.</value>
        public string Charset
        {
            set { this.AddValue(@"Charset", value); }
        }

        /// <summary>
        /// Sets the database.
        /// </summary>
        /// <value>The database.</value>
        public string Database
        {
            set { this.AddValue(@"Database", value); }
        }

        /// <summary>
        /// Sets the data source.
        /// </summary>
        /// <value>The data source.</value>
        public string DataSource
        {
            set { this.AddValue(@"Data Source", value); }
        }

        /// <summary>
        /// Sets the DBA privilege.
        /// </summary>
        /// <value>The DBA privilege.</value>
        public string DbaPrivilege
        {
            set { this.AddValue(@"DBA Priviledge", value); }
        }

        /// <summary>
        /// Sets the default lock escalation.
        /// </summary>
        /// <value>The default lock escalation.</value>
        public uint DefaultLockEscalation
        {
            set { this.AddValue(@"Default Lock Escalation", value.ToString()); }
        }

        /// <summary>
        /// Sets the default lock timeout.
        /// </summary>
        /// <value>The default lock timeout.</value>
        public uint DefaultLockTimeout
        {
            set { this.AddValue(@"Default Lock Timeout", value.ToString()); }
        }

        /// <summary>
        /// Sets the database driver.
        /// </summary>
        /// <value>The database driver.</value>
        public string Driver
        {
            set { this.AddValue(@"Driver", value); }
        }

        /// <summary>
        /// Sets the size of the decrease pool.
        /// </summary>
        /// <value>The size of the decrease pool.</value>
        public int DecreasePoolSize
        {
            set { this.AddValue(@"Decr Pool Size", value.ToString()); }
        }

        /// <summary>
        /// Sets the dialect.
        /// </summary>
        /// <value>The dialect.</value>
        public int Dialect
        {
            set { this.AddValue(@"Dialect", value.ToString()); }
        }

        /// <summary>
        /// Sets a value indicating whether this
        /// <see cref="DbConnectionStringBuilder"/> is encrypt.
        /// </summary>
        /// <value><c>True</c> if encrypt; otherwise, <c><c>false</c></c>.
        /// </value>
        public bool Encrypt
        {
            set { this.AddValue(@"Encrypt", value.ToString()); }
        }

        /// <summary>
        /// Sets a value indicating whether this
        /// <see cref="DbConnectionStringBuilder"/> is enlist.
        /// </summary>
        /// <value><c>True</c> if enlist; otherwise, <c><c>false</c></c>.
        /// </value>
        public bool Enlist
        {
            set { this.AddValue(@"Enlist", value.ToString()); }
        }

        /// <summary>
        /// Sets the extended properties.
        /// </summary>
        /// <value>The extended properties.</value>
        public string ExtendedProperties
        {
            set { this.AddValue(@"Extended Properties", value); }
        }

        /// <summary>
        /// Sets the flush interval.
        /// </summary>
        /// <value>The flush interval.</value>
        public uint FlushInterval
        {
            set { this.AddValue(@"Flush Interval", value.ToString()); }
        }

        /// <summary>
        /// Sets the size of the increase pool.
        /// </summary>
        /// <value>The size of the increase pool.</value>
        public int IncreasePoolSize
        {
            set { this.AddValue(@"Incr Pool Size", value.ToString()); }
        }

        /// <summary>
        /// Sets the initial catalog.
        /// </summary>
        /// <value>The initial catalog.</value>
        public string InitialCatalog
        {
            set { this.AddValue(@"Initial Catalog", value); }
        }

        /// <summary>
        /// Sets a value indicating whether [integrated security].
        /// </summary>
        /// <value><c>True</c> if [integrated security]; otherwise, <c>false</c>
        /// .</value>
        public bool IntegratedSecurity
        {
            set { this.AddValue(@"Integrated Security", value.ToString()); }
        }

        /// <summary>
        /// Sets the maximum size of the buffer.
        /// </summary>
        /// <value>The maximum size of the buffer.</value>
        public int MaximumBufferSize
        {
            set { this.AddValue(@"Max Buffer Size", value.ToString()); }
        }

        /// <summary>
        /// Sets the maximum size of the database.
        /// </summary>
        /// <value>The maximum size of the database.</value>
        public int MaximumDatabaseSize
        {
            set { this.AddValue(@"Max Database Size", value.ToString()); }
        }

        /// <summary>
        /// Sets the maximum size of the pool.
        /// </summary>
        /// <value>The maximum size of the pool.</value>
        public int MaximumPoolSize
        {
            set { this.AddValue(@"Max Pool Size", value.ToString()); }
        }

        /// <summary>
        /// Sets the minimum size of the pool.
        /// </summary>
        /// <value>The minimum size of the pool.</value>
        public int MinimumPoolSize
        {
            set { this.AddValue(@"Min Pool Size", value.ToString()); }
        }

        /// <summary>
        /// Sets the file access mode.
        /// </summary>
        /// <value>The file access mode.</value>
        public FileMode Mode
        {
            set { this.AddValue(@"Mode", value.GetDescriptionAttribute()); }
        }

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
        public bool MultipleActiveResultSets
        {
            set { this.AddValue(@"MultipleActiveResultSets", value.ToString()); }
        }

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
        public string NetworkLibrary
        {
            set { this.AddValue(@"Network Library", value); }
        }

        /// <summary>
        /// Sets the size of the packet.
        /// </summary>
        /// <value>The size of the packet.</value>
        public short PacketSize
        {
            set { this.AddValue(@"Packet Size", value.ToString()); }
        }

        /// <summary>
        /// Sets the password.
        /// </summary>
        /// <value>The password.</value>
        public string Password
        {
            set { this.AddValue(@"Password", value); }
        }

        /// <summary>
        /// Sets a value indicating whether [persist security info].
        /// </summary>
        /// <value><c>True</c> if [persist security info]; otherwise,
        /// <c>false</c>.</value>
        public bool PersistSecurityInfo
        {
            set { this.AddValue(@"Persist Security Info", value.ToString()); }
        }

        /// <summary>
        /// Sets the port number.
        /// </summary>
        /// <value>The port number.</value>
        public ushort Port
        {
            set { this.AddValue(@"Port", value.ToString()); }
        }

        /// <summary>
        /// Sets a value indicating whether connection pooling is turned on.
        /// </summary>
        /// <value><c>True</c> if pooling; otherwise, <c>false</c>.</value>
        public bool Pooling
        {
            set { this.AddValue(@"Pooling", value.ToString()); }
        }

        /// <summary>
        /// Sets a value indicating whether replication is supported using the
        /// connection.
        /// </summary>
        /// <value>
        ///   <c>True</c> if replication is supported using the connection;
        ///   otherwise, <c>false</c>.
        /// </value>
        public bool Replication
        {
            set { this.AddValue(@"Replication", value.ToString()); }
        }

        /// <summary>
        /// Sets a value indicating whether SSL is used to encrypt the channel
        /// when bypassing walking the certificate chain to validate trust.
        /// </summary>
        /// <value>
        ///     <c>True</c> when SSL is used to encrypt the channel when bypassing
        ///     walking the certificate chain to validate trust; otherwise,
        ///     <c>false</c>.
        /// </value>
        public bool TrustServerCertificate
        {
            set { this.AddValue(@"TrustServerCertificate", value.ToString()); }
        }

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
        public string TypeSystemVersion
        {
            set { this.AddValue(@"Type System Version", value); }
        }

        /// <summary>
        /// Sets the provider.
        /// </summary>
        /// <value>The provider.</value>
        public string Provider
        {
            set { this.AddValue(@"Provider", value); }
        }

        /// <summary>
        /// Sets the proxy password.
        /// </summary>
        /// <value>The proxy password.</value>
        public string ProxyPassword
        {
            set { this.AddValue(@"Proxy Password", value); }
        }

        /// <summary>
        /// Sets the proxy user id.
        /// </summary>
        /// <value>The proxy user id.</value>
        public string ProxyUserId
        {
            set { this.AddValue(@"Proxy User Id", value); }
        }

        /// <summary>
        /// Sets the password.
        /// </summary>
        /// <value>The password.</value>
        public string Pwd
        {
            set { this.AddValue(@"PWD", value); }
        }

        /// <summary>
        /// Sets the server.
        /// </summary>
        /// <value>The server name.</value>
        public string Server
        {
            set { this.AddValue(@"Server", value); }
        }

        /// <summary>
        /// Sets the session mode.
        /// </summary>
        /// <value>The session mode.</value>
        public string SessionMode
        {
            set { this.AddValue(@"Session Mode", value); }
        }

        /// <summary>
        /// Sets the system database.
        /// </summary>
        /// <value>The system database.</value>
        public string SystemDatabase
        {
            set { this.AddValue(@"Jet OLEDB:System Database", value); }
        }

        /// <summary>
        /// Sets the size of the temp file max.
        /// </summary>
        /// <value>The size of the temp file max.</value>
        public int TempFileMaxSize
        {
            set { this.AddValue(@"Temp File Max Size", value.ToString()); }
        }

        /// <summary>
        /// Sets the temp path.
        /// </summary>
        /// <value>The temp path.</value>
        public string TempPath
        {
            set { this.AddValue(@"Temp Path", value); }
        }

        /// <summary>
        /// Sets a value indicating whether [trusted connection].
        /// </summary>
        /// <value><c>True</c> if [trusted connection]; otherwise, <c>false</c>.
        /// </value>
        public bool TrustedConnection
        {
            set { this.AddValue(@"Trusted_Connection", value.ToString()); }
        }

        /// <summary>
        /// Sets the user id.
        /// </summary>
        /// <value>The user id.</value>
        public string Uid
        {
            set { this.AddValue(@"UID", value); }
        }

        /// <summary>
        /// Sets the user id.
        /// </summary>
        /// <value>The user id.</value>
        public string User
        {
            set { this.AddValue(@"User", value); }
        }

        /// <summary>
        /// Sets the user id.
        /// </summary>
        /// <value>The user id.</value>
        public string UserId
        {
            set { this.AddValue(@"User ID", value); }
        }

        /// <summary>
        /// Sets a value indicating whether to redirect the connection from the
        /// default SQL Server Express instance to a runtime-initiated instance
        /// running under the account of the caller.
        /// </summary>
        /// <value>
        ///   <c>True</c> if user instance redirection is enabled; otherwise,
        ///   <c>false</c>.
        /// </value>
        public bool UserInstance
        {
            set { this.AddValue(@"User Instance", value.ToString()); }
        }

        /// <summary>
        /// Sets the workstation ID.
        /// </summary>
        /// <value>The workstation ID.</value>
        public string WorkstationId
        {
            set { this.AddValue(@"Workstation ID", value); }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Gets and creates a database connection string.
        /// </summary>
        /// <returns>A complete connection string based on the parameters.
        /// </returns>
        public string ConnectionString
        {
            get
            {
                return this.parameters.Aggregate(string.Empty, (current, parameter) => current + JoinPair(parameter.Key, parameter.Value));
            }
        }

        /// <summary>
        /// Adds a custom attribute and value if the attribute is not available.
        /// If the attribute already exists, then the value of the attribute
        /// will be replaced with the new value.
        /// </summary>
        /// <param name="attribute">The attribute name.</param>
        /// <param name="value">The attribute value.</param>
        /// <remarks>
        /// If the attribute or the value are <c>null</c> or empty then no entry
        /// will be added to the connection string.
        /// </remarks>
        public void CustomAttribute(string attribute, string value)
        {
            // Returns if the provided attribute or value are either null or empty.
            if (attribute.IsNullOrEmpty() || value.IsNullOrEmpty())
            {
                return;
            }

            this.AddValue(attribute, value);
        }

        /// <summary>
        /// Adds a custom attribute and value if the attribute is not already
        /// available. If the attribute already exists, then the value of the
        /// attribute will be replaced with the new value.
        /// </summary>
        /// <param name="attribute">The attribute string.</param>
        /// <param name="encryptedValue">The encrypted string value.</param>
        /// <param name="decrypter">The decrypter delegate that accepts an
        /// encrypted string and returns a decrypted string.</param>
        /// <remarks>
        /// If the attribute or the encrypted string are <c>null</c> or empty
        /// then no entry will be added to the connection string.
        /// </remarks>
        /// <exception cref="DbDataException">An error occurred while invoking
        /// the decrypter delegate.</exception>
        public void CustomEncryptedAttribute(string attribute, string encryptedValue, Func<string, string> decrypter = null)
        {
            // Returns if the provided attribute or value are either null or empty.
            if (attribute.IsNullOrEmpty() || encryptedValue.IsNullOrEmpty())
            {
                return;
            }

            string decryptedValue;
            if (encryptedValue.StartsAndEndsWith(DecryptEscapeCharacter))
            {
                // Removes the escape characters at the start and end.
                decryptedValue = encryptedValue.RemoveFirstChar(DecryptEscapeCharacter).RemoveLastChar(DecryptEscapeCharacter);
            }
            else
            {
                if (decrypter == null)
                {
                    decryptedValue = encryptedValue;
                }
                else
                {
                    try
                    {
                        decryptedValue = decrypter(encryptedValue);
                    }
                    catch (Exception ex)
                    {
                        // Catches and exceptions thrown by the delegate method call,
                        // wraps them with our exception, and then throws our exception.
                        var outer = new DbDataException(@"An error occurred while invoking the decrypter delegate.", ex);
                        throw outer;
                    }
                }
            }

            this.CustomAttribute(attribute, decryptedValue);
        }

        /// <summary>
        /// Sets the password depending if the password is wrapped in single
        /// quotes determining if it needs to be decrypted first.
        /// </summary>
        /// <param name="password">The password string.</param>
        /// <param name="decrypter">The delegate for decrypting the password
        /// string.</param>
        /// <remarks>If the password string begins and ends with a single quote
        /// then the single quotes will be removed and the text set to the
        /// password value. If the decrypter value is <c>null</c> then the
        /// specified password will be set to the password value; otherwise the
        /// decrypter delegate will be called to execute returning back a
        /// decrypted string which will be then set to the password value.
        /// </remarks>
        public void SetPassword(string password, Func<string, string> decrypter = null) =>
            this.CustomEncryptedAttribute(@"Password", password, decrypter);

        #endregion Methods

        #region Overrides

        /// <summary>
        /// Returns a <see cref="string"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="string"/> that represents this instance.
        /// </returns>
        public override string ToString() => this.ConnectionString;

        #endregion Overrides

        #region Helpers

        /// <summary>
        /// Joins the key value pair with an equal symbol and ends with a
        /// semi-colon.
        /// </summary>
        /// <param name="key">The string key.</param>
        /// <param name="value">The string value.</param>
        /// <returns>A concatenated key-value pair.</returns>
        private static string JoinPair(string key, string value) => string.Concat(key, @"=", value, @";");

        /// <summary>
        /// Adds the value to the parameters dictionary.
        /// </summary>
        /// <param name="key">The key string.</param>
        /// <param name="value">The value string.</param>
        private void AddValue(string key, string value) => this.parameters.AddOrUpdate(key, value);

        #endregion Helpers
    }
}