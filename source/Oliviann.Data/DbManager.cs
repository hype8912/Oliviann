namespace Oliviann.Data
{
    #region Usings

    using System;
    using System.Data;
    using System.Data.Common;
    using System.Diagnostics;
    using System.Globalization;
    using System.Security;
    using System.Threading;
    using Oliviann.Data.Base;
    using Oliviann.Properties;
    using Oliviann.Security;

    #endregion Usings

    /// <summary>
    /// Represents a database manager class.
    /// </summary>
    public sealed partial class DbManager : IDbManager
    {
        #region Fields

        /// <summary>
        /// Placeholder variable for class being disposed.
        /// </summary>
        private bool isDisposed;

        /// <summary>
        /// Place holder for the typed connection object.
        /// </summary>
        private DbConnection internalConnection;

        #endregion Fields

        #region Constructor/Deconstructor

        /// <summary>
        /// Initializes a new instance of the <see cref="DbManager"/> class.
        /// </summary>
        /// <remarks>
        /// If an instance is created without providing a provider type then a
        /// default provider type of <c>Sql</c> will be used.
        /// </remarks>
        public DbManager() : this(DatabaseProvider.MicrosoftSqlServer, string.Empty)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DbManager"/> class.
        /// </summary>
        /// <param name="providerType">Type of the provider.</param>
        public DbManager(DatabaseProvider providerType) : this(providerType, string.Empty)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DbManager"/> class.
        /// </summary>
        /// <param name="providerType">Type of the provider.</param>
        /// <param name="connectionString">The database connection string.
        /// </param>
        public DbManager(DatabaseProvider providerType, string connectionString)
        {
            this.Provider = providerType;
            this.ConnectionString = connectionString.ToSecureString();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DbManager"/> class.
        /// </summary>
        /// <param name="factory">The custom implementation of the database
        /// manager factory.</param>
        public DbManager(DbProviderFactory factory) => this.CustomFactory = factory;

        /// <summary>
        /// Finalizes an instance of the <see cref="DbManager"/> class. Performs
        /// other cleanup operations before the <see cref="DbManager"/> is
        /// reclaimed by garbage collection.
        /// </summary>
        ~DbManager()
        {
            this.Dispose(false);
        }

        #endregion Constructor/Deconstructor

        #region Properties

        /// <inheritdoc />
        public IDbConnection Connection => this.internalConnection;

        /// <inheritdoc />
        public IDataReader DataReader { get; private set; }

        /// <inheritdoc />
        public DatabaseProvider Provider { get; set; }

        /// <inheritdoc />
        public SecureString ConnectionString { get; set; }

        /// <inheritdoc />
        public IDbCommand Command { get; private set; }

        /// <inheritdoc />
        public DbProviderFactory CustomFactory { get; }

        #endregion Properties

        #region Connection State

        /// <inheritdoc />
        public void Open()
        {
            if (this.Connection != null && this.Connection.State == ConnectionState.Open)
            {
                return;
            }

            DbConnection tempConnection = this.CustomFactory != null
                                              ? this.CustomFactory.CreateConnection()
                                              : DbManagerFactory.CreateConnection(this.Provider);

            if (tempConnection == null)
            {
                throw this.CreateException(Resources.ERR_ProviderNotNull.FormatWith("Connection"), null);
            }

            tempConnection.ConnectionString = this.ConnectionString.ToUnsecureString();
            if (tempConnection.State != ConnectionState.Open)
            {
                tempConnection.Open();
            }

            this.internalConnection = tempConnection;
        }

        /// <inheritdoc />
        public void Close()
        {
            this.ClearParameters();
            if (this.Connection == null)
            {
                return;
            }

            if (this.Connection.State != ConnectionState.Closed)
            {
                this.Connection.Close();
            }
        }

        #endregion Connection State

        #region Close/Dispose

        /// <inheritdoc />
        public void CloseReader()
        {
            if (this.DataReader != null && !this.DataReader.IsClosed)
            {
                this.DataReader.Close();
            }
        }

        /// <inheritdoc />
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion Close/Dispose

        #region Executioners

        /// <summary>
        /// Executes a database dataset command.
        /// </summary>
        /// <param name="commandType">A value indicating how the
        /// <paramref name="commandText"/> value is interpreted.</param>
        /// <param name="commandText">The SQL statement or stored procedure to
        /// execute against the data source.</param>
        /// <returns>A dataset data object.</returns>
        /// <remarks>
        /// Automatically opens the database connection if it isn't already
        /// open.
        /// </remarks>
        /// <exception cref="DbDataException">An unknown internal exception
        /// occurred.</exception>
        public DataSet ExecuteDataSet(CommandType commandType, string commandText)
        {
            try
            {
                IDbDataAdapter dataAdapter = this.CustomFactory != null
                                  ? this.CustomFactory.CreateDataAdapter()
                                  : DbManagerFactory.CreateDataAdapter(this.Provider);

                Oliviann.ADP.CheckNullReference(dataAdapter, Resources.ERR_InstanceNotNull.FormatWith("Data adapter"));

                this.Open();
                this.PrepareCommand(commandType, commandText);
                dataAdapter.SelectCommand = this.Command;

                var dataSet = new DataSet { Locale = CultureInfo.InvariantCulture };
                dataAdapter.Fill(dataSet);
                return dataSet;
            }
            catch (Exception ex)
            {
                throw this.CreateException(Resources.ERR_ExceptionExecuting.FormatWith("dataset"), ex);
            }
        }

        /// <summary>
        /// Executes a database data table command.
        /// </summary>
        /// <param name="commandType">A value indicating how the
        /// <paramref name="commandText"/> value is interpreted.</param>
        /// <param name="commandText">The SQL statement or stored procedure to
        /// execute against the data source.</param>
        /// <param name="commandDelegate">The command delegate that accepts a
        /// <see cref="IDbCommand"/> interface.</param>
        /// <returns>
        /// A <see cref="DataTable"/> result for the specified command text.
        /// </returns>
        /// <remarks>
        /// Automatically opens the database connection if it isn't already
        /// open.
        /// </remarks>
        /// <exception cref="DbDataException">An unknown internal exception
        /// occurred.</exception>
        public DataTable ExecuteDataTable(CommandType commandType, string commandText, Action<IDbCommand> commandDelegate = null)
        {
            try
            {
                this.ExecuteReader(commandType, commandText, commandDelegate);
                var table = new DataTable { Locale = CultureInfo.InvariantCulture };
                table.Load(this.DataReader);
                this.CloseReader();
                return table;
            }
            catch (Exception ex)
            {
                throw this.CreateException(Resources.ERR_ExceptionExecuting.FormatWith("data table"), ex);
            }
        }

        /// <inheritdoc />
        public int ExecuteNonQuery(CommandType commandType, string commandText, Action<IDbCommand> commandDelegate = null)
        {
            try
            {
                this.Open();
                this.PrepareCommand(commandType, commandText);
                commandDelegate?.Invoke(this.Command);

                return this.Command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw this.CreateException(Resources.ERR_ExceptionExecuting.FormatWith("non query"), ex);
            }
        }

        /// <summary>
        /// Executes the database data reader. Also provides an action delegate
        /// to adding additional command parameters before execution of the
        /// query.
        /// </summary>
        /// <param name="commandType">A value indicating how the
        /// <paramref name="commandText"/> value is interpreted.</param>
        /// <param name="commandText">The SQL statement or stored procedure to
        /// execute against the data source.</param>
        /// <param name="commandDelegate">The command delegate that accepts a
        /// <see cref="IDbCommand"/> interface.</param>
        /// <returns>A database data reader object instance.</returns>
        /// <remarks>
        /// Automatically opens the database connection if it isn't already
        /// open. Each request to open the database cost ~110ms.
        /// </remarks>
        /// <exception cref="DbDataException">An unknown internal exception
        /// occurred.</exception>
        public IDataReader ExecuteReader(CommandType commandType, string commandText, Action<IDbCommand> commandDelegate = null)
        {
            try
            {
                this.Open();
                this.PrepareCommand(commandType, commandText);
                commandDelegate?.Invoke(this.Command);

                this.DataReader = this.Command.ExecuteReader();
                return this.DataReader;
            }
            catch (Exception ex)
            {
                throw this.CreateException(Resources.ERR_ExceptionExecuting.FormatWith("data reader"), ex);
            }
        }

        /// <summary>
        /// Executes a database scalar command.
        /// </summary>
        /// <param name="commandType">A value indicating how the
        /// <paramref name="commandText"/> value is interpreted.</param>
        /// <param name="commandText">the SQL statement or stored procedure to
        /// execute against the data source.</param>
        /// <returns>A scalar string array.</returns>
        /// <remarks>
        /// Automatically opens the database connection if it isn't already
        /// open.
        /// </remarks>
        /// <exception cref="DbDataException">An unknown internal exception
        /// occurred.</exception>
        public object ExecuteScalar(CommandType commandType, string commandText)
        {
            try
            {
                this.Open();
                this.PrepareCommand(commandType, commandText);
                return this.Command.ExecuteScalar();
            }
            catch (Exception ex)
            {
                throw this.CreateException(Resources.ERR_ExceptionExecuting.FormatWith("scaler"), ex);
            }
        }

        /// <summary>
        /// Returns the schema information for the data source of this
        /// connection using the specified string for the schema name and the
        /// specified string array for the restriction values.
        /// </summary>
        /// <param name="collectionName">Optional. Specifies the name of the schema to
        /// return. Default value is "Tables".</param>
        /// <param name="restrictionValues">Optional. Specifies a set of restriction
        /// values for the requested schema. Default value is null.</param>
        /// <returns>A <see cref="DataTable"/> that contains schema information.
        /// </returns>
        /// <exception cref="DbDataException">Exception getting database schema.
        /// </exception>
        public DataTable GetSchema(string collectionName = "Tables", string[] restrictionValues = null)
        {
            try
            {
                this.Open();
                DataTable table = this.internalConnection.GetSchema(collectionName, restrictionValues);
                return table;
            }
            catch (Exception ex)
            {
                throw this.CreateException(@"Exception getting database schema.", ex);
            }
        }

        #endregion Executioners

        #region Helper Methods

        /// <summary>
        /// Prepares the database command for execution.
        /// </summary>
        /// <param name="commandType">Type of the database command.</param>
        /// <param name="commandText">The database command text.</param>
        private void PrepareCommand(CommandType commandType, string commandText)
        {
            this.Command = this.CustomFactory != null
                               ? this.CustomFactory.CreateCommand()
                               : DbManagerFactory.CreateCommand(this.Provider);

            if (this.Command == null)
            {
                throw this.CreateException(Resources.ERR_DB_InvalidProvider.FormatWith("command"), null);
            }

            this.Command.Connection = this.Connection;
            this.Command.CommandText = commandText;
            this.Command.CommandType = commandType;

            if (this.Transaction != null)
            {
                this.Command.Transaction = this.Transaction;
            }

            if (this._parameters != null && this._parameters.Count > 0)
            {
                this.AttachParameters();
            }
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="disposing">True to release both managed and unmanaged
        /// resources; false to release only unmanaged resources.</param>
        /// <remarks>Do not try to close the database connection when disposing
        /// the instance. If the garbage collector runs and disposes the
        /// connection, an internal .NET error will be thrown.
        /// <a href="http://social.msdn.microsoft.com/Forums/en-US/b23d8492-1696-4ccb-b4d9-3e7fbacab846/internal-net-framework-data-provider-error-1">
        /// See link for more information.</a></remarks>
        private void Dispose(bool disposing)
        {
            if (!this.isDisposed)
            {
                if (disposing)
                {
                    this.ClearParameters();
                    this.DataReader = null;
                    this.Command = null;
                    this.Transaction = null;
                    this.internalConnection = null;
                }
            }

            this.isDisposed = true;
        }

        /// <summary>
        /// Creates an exception for an error that occurs in this instance.
        /// </summary>
        /// <param name="message">The error message output.</param>
        /// <param name="inner">The inner exception.</param>
        /// <returns>
        /// A new <see cref="DbDataException"/> will the correct data.
        /// </returns>
        private DbDataException CreateException(string message, Exception inner)
        {
            string outMessage = string.Empty;
            var ex = new DbDataException(message, inner);
            ex.Data.Add("Provider", this.Provider);
            outMessage += "Provider: " + this.Provider + Environment.NewLine;
            outMessage += "ThreadId: " + Thread.CurrentThread.ManagedThreadId + Environment.NewLine;

            if (this.Command != null)
            {
                ex.Data.Add("Command", this.Command.CommandText);
                outMessage += "Command: " + this.Command.CommandText + Environment.NewLine;
            }

            if (this.Connection != null)
            {
                ex.Data.Add("State", this.Connection.State);
                outMessage += "State: " + this.Connection.State + Environment.NewLine;
            }

            outMessage += ex;
            Trace.TraceError(outMessage);

            return ex;
        }

        #endregion Helper Methods
    }
}