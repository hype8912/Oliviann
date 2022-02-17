namespace Oliviann.Data
{
    #region Usings

    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;

    #endregion

    /// <summary>
    /// Represents a
    /// </summary>
    public static partial class IDbManagerExtensions
    {
        /// <summary>
        /// Provides a generic wrapper around an <see cref="IDataReader"/>
        /// allowing you to execute a delegate action for each record read
        /// within the data reader instance.
        /// </summary>
        /// <param name="manager">The database manager instance ready for
        /// execution.</param>
        /// <param name="commandText">The SQL string command to be performed.
        /// </param>
        /// <param name="parameters">The data parameters and values. May use
        /// <c>null</c> if no parameters are required for the query. An @ symbol
        /// will automatically be added to the beginning of each parameter key.
        /// </param>
        /// <param name="preAction">The action to be performed before all the
        /// data records are read individually.</param>
        /// <param name="readerAction">The action to be performed when each data
        /// record is read from the data reader.</param>
        public static void ExecuteReaderWrapper(
                                                this IDbManager manager,
                                                string commandText,
                                                IDictionary<string, object> parameters,
                                                Action<IDataReader> preAction,
                                                Action<IDataReader> readerAction)
        {
            if (manager == null)
            {
                return;
            }

            try
            {
                // Add parameters to manager.
                parameters.FormatAddParameters(manager);

                // Executes the data reader for the specified query.
                IDataReader dr = manager.ExecuteReader(CommandType.Text, commandText);

                // Performs the pre-action event if not null.
                preAction?.Invoke(dr);

                // Loops through all the read objects and performs the post
                // action event.
                if (readerAction != null)
                {
                    while (dr.Read())
                    {
                        readerAction(dr);
                    }
                }
            }
            finally
            {
                manager.CloseReader();
                manager.Close();
            }
        }

        /// <summary>
        /// Provides a generic wrapper around an <see cref="IDataReader"/> for a
        /// single returned object.
        /// </summary>
        /// <typeparam name="T">The type of data object model being read from
        /// the database.</typeparam>
        /// <param name="manager">The database manager instance ready for
        /// execution.</param>
        /// <param name="commandText">The SQL string command to be performed.
        /// </param>
        /// <param name="parameters">The data parameters and values. May use
        /// <c>null</c> if no parameters are required for the query. An @ symbol
        /// will automatically be added to the beginning of each parameter key.
        /// </param>
        /// <param name="preAction">The action to be performed before all the
        /// data records are read individually.</param>
        /// <param name="mapping">The function for mapping the data reader
        /// values to the corresponding property.</param>
        /// <returns>
        /// A single typed object unless the database has not been initialized;
        /// otherwise, the value will <c>null</c>.
        /// </returns>
        public static T ExecuteReaderWrapperSingle<T>(
                                                      this IDbManager manager,
                                                      string commandText,
                                                      IDictionary<string, object> parameters,
                                                      Action<IDataReader> preAction,
                                                      Func<IDataReader, T> mapping)
        {
            List<T> items = manager.ExecuteReaderWrapper(commandText, parameters, preAction, mapping);
            return items.FirstOrDefault();
        }

        /// <summary>
        /// Provides a generic wrapper around an <see cref="IDataReader"/>.
        /// </summary>
        /// <typeparam name="T">The type of data object model being read from
        /// the database.</typeparam>
        /// <param name="manager">The database manager instance ready for
        /// execution.</param>
        /// <param name="commandText">The SQL string command to be performed.
        /// </param>
        /// <param name="parameters">The data parameters and values. May use
        /// <c>null</c> if no parameters are required for the query. An @ symbol
        /// will automatically be added to the beginning of each parameter key.
        /// </param>
        /// <param name="preAction">The action to be performed before all the
        /// data records are read individually.</param>
        /// <param name="mapping">The function for mapping the data reader
        /// values to the corresponding property.</param>
        /// <param name="commandTimeout">The time in seconds to wait for the
        /// command to execute.</param>
        /// <returns>
        /// A collection of typed objects unless the database has not been
        /// initialized; otherwise, the value will <c>null</c>.
        /// </returns>
        /// <example>
        /// Ordinals
        /// <code>
        /// int prodId, id, dtm;
        /// prodId = id = dtm = 0;
        /// Action&lt;IDataReader&gt; ordinals = dr =>
        ///     {
        ///         prodId = dr.GetOrdinal(@"ProductionID");
        ///         id = dr.GetOrdinal(@"SoftwareID");
        ///         dtm = dr.GetOrdinal(@"DTM");
        ///     };
        /// </code>
        /// Mapper
        /// <code>
        /// Func&lt;IDataReader, SoftwareUsage&gt; mapper = dr =>
        ///     new SoftwareUsage
        ///     {
        ///         ProductionId = (int)dr[prodId],
        ///         SoftwareNumber = (short)dr[id],
        ///         DtmVerified = (bool)dr[dtm]
        ///     };
        /// </code>
        /// Call
        /// <code>
        /// var items = this.Manager.ExecuteReaderWrapper(Sql, null, null, mapper);
        /// </code>
        /// </example>
        public static List<T> ExecuteReaderWrapper<T>(
                                                      this IDbManager manager,
                                                      string commandText,
                                                      IDictionary<string, object> parameters,
                                                      Action<IDataReader> preAction,
                                                      Func<IDataReader, T> mapping,
                                                      int? commandTimeout = new int?())
        {
            if (manager == null)
            {
                return new List<T>();
            }

            var items = new List<T>();

            try
            {
                // Add parameters to manager.
                parameters.FormatAddParameters(manager);

                // Set the command timeout value if different.
                Action<IDbCommand> command = null;
                if (commandTimeout.HasValue)
                {
                    command = c => { c.CommandTimeout = commandTimeout.Value; };
                }

                // Executes the data reader for the specified query.
                IDataReader dr = manager.ExecuteReader(CommandType.Text, commandText, command);

                // Performs the pre-action event if not null.
                preAction?.Invoke(dr);

                // Loops through all the read objects and performs the post action event.
                if (mapping != null)
                {
                    while (dr.Read())
                    {
                        items.Add(mapping(dr));
                    }
                }
            }
            finally
            {
                manager.CloseReader();
                manager.Close();
            }

            return items;
        }
    }
}