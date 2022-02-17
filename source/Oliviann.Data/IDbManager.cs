namespace Oliviann.Data
{
    #region Usings

    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Security;

    #endregion Usings

    /// <summary>
    /// Interface for implementing <see cref="DbManager"/> class.
    /// </summary>
    public interface IDbManager : IDisposable
    {
        #region Properties

        /// <summary>
        /// Gets the command.
        /// </summary>
        /// <value>The command.</value>
        IDbCommand Command { get; }

        /// <summary>
        /// Gets the connection.
        /// </summary>
        /// <value>The connection.</value>
        IDbConnection Connection { get; }

        /// <summary>
        /// Gets or sets the connection string.
        /// </summary>
        /// <value>The connection string.</value>
        SecureString ConnectionString { get; set; }

        /// <summary>
        /// Gets the custom database manager factory implementation, if set.
        /// </summary>
        /// <value>
        /// The custom database manager factory implementation.
        /// </value>
        DbProviderFactory CustomFactory { get; }

        /// <summary>
        /// Gets the data reader.
        /// </summary>
        /// <value>The data reader.</value>
        IDataReader DataReader { get; }

        /// <summary>
        /// Gets the parameters.
        /// </summary>
        /// <value>The parameters.</value>
        IList<IDataParameter> Parameters { get; }

        /// <summary>
        /// Gets or sets the type of the provider.
        /// </summary>
        /// <value>The type of the provider.</value>
        DatabaseProvider Provider { get; set; }

        /// <summary>
        /// Gets the transaction.
        /// </summary>
        /// <value>The transaction.</value>
        IDbTransaction Transaction { get; }

        #endregion Properties

        #region Connection State

        /// <summary>
        /// Closes this database connection instance.
        /// </summary>
        /// <remarks>
        /// If a connection pool is used then the database connection is
        /// released and added back to the connection pool for reuse.
        /// </remarks>
        void Close();

        /// <summary>
        /// Opens this database connection instance.
        /// </summary>
        void Open();

        #endregion Connection State

        #region Transactions

        /// <summary>
        /// Starts a database transaction with the specified isolation level.
        /// </summary>
        /// <param name="level">The isolation level under which the transaction
        /// should run.</param>
        void BeginTransaction(IsolationLevel level);

        /// <summary>
        /// Commits the database transaction.
        /// </summary>
        void CommitTransaction();

        /// <summary>
        /// Rolls back a transaction from a pending state.
        /// </summary>
        void RollbackTransaction();

        #endregion Transactions

        #region Parameters

        /// <summary>
        /// Adds the specified parameter to the parameter collection.
        /// </summary>
        /// <param name="parameter">The data parameter to be added.</param>
        void AddParameter(IDataParameter parameter);

        /// <summary>
        /// Removes all <see cref="IDataParameter"/> objects from the parameter
        /// collection.
        /// </summary>
        void ClearParameters();

        /// <summary>
        /// Gets the value of the specified parameter.
        /// </summary>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <returns>An object that is the value of the parameter. The default
        /// value is <c>null</c>.</returns>
        object GetParameterValue(string parameterName);

        #endregion Parameters

        #region Executioners

        /// <summary>
        /// Executes a database dataset command.
        /// </summary>
        /// <param name="commandType">A value indicating how the
        /// <paramref name="commandText"/> value is interpreted.</param>
        /// <param name="commandText">The SQL statement or stored procedure to
        /// execute against the data source.</param>
        /// <returns>A dataset data object.</returns>
        DataSet ExecuteDataSet(CommandType commandType, string commandText);

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
        ///     A data table result for the specified command text.
        /// </returns>
        DataTable ExecuteDataTable(CommandType commandType, string commandText, Action<IDbCommand> commandDelegate = null);

        /// <summary>
        /// Executes an SQL statement against the Connection and returns the
        /// number of rows affected. Also provides an action delegate to adding
        /// additional command parameters before execution of the query.
        /// </summary>
        /// <param name="commandType">A value indicating how the
        /// <paramref name="commandText"/> value is interpreted.</param>
        /// <param name="commandText">The SQL statement or stored procedure to
        /// execute against the data source.</param>
        /// <param name="commandDelegate">The command delegate that accepts a
        /// <see cref="IDbCommand"/> interface.</param>
        /// <returns>
        /// For UPDATE, INSERT, and DELETE statements, the return value is the
        /// number of rows affected by the command. For all other types of
        /// statements, the return value is -1.
        /// </returns>
        /// <exception cref="DbDataException">An unknown internal exception
        /// occurred.</exception>
        /// <remarks>
        /// Automatically opens the database connection if it isn't already
        /// open.
        /// </remarks>
        int ExecuteNonQuery(CommandType commandType, string commandText, Action<IDbCommand> commandDelegate = null);

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
        IDataReader ExecuteReader(CommandType commandType, string commandText, Action<IDbCommand> commandDelegate = null);

        /// <summary>
        /// Executes a database scalar command.
        /// </summary>
        /// <param name="commandType">A value indicating how the
        /// <paramref name="commandText"/> value is interpreted.</param>
        /// <param name="commandText">the SQL statement or stored procedure to
        /// execute against the data source.</param>
        /// <returns>A scalar string array.</returns>
        object ExecuteScalar(CommandType commandType, string commandText);

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
        DataTable GetSchema(string collectionName = "Tables", string[] restrictionValues = null);

        #endregion Executioners

        #region Close/Dispose

        /// <summary>
        /// Closes the reader.
        /// </summary>
        void CloseReader();

        #endregion Close/Dispose
    }
}