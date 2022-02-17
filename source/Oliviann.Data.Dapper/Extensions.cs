namespace Oliviann.Data
{
    #region Usings

    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;

#if !NET35
    using System.Threading.Tasks;
#endif
    using Dapper;

    #endregion Usings

    /// <summary>
    ///  Represents a class for common extension methods for the
    ///  <see cref="DbManager"/> for working with Dapper.
    /// </summary>
    public static class Extensions
    {
        #region Reader

        /// <summary>
        /// Provides a generic wrapper around an <see cref="IDataReader"/> for
        /// dynamically mapping POCO objects.
        /// </summary>
        /// <typeparam name="T">The type of data object model being read from
        /// the database.</typeparam>
        /// <param name="manager">The database manager instance ready for
        /// execution.</param>
        /// <param name="commandText">The SQL string command to be performed.
        /// </param>
        /// <param name="parameters">Optional. The query parameters.</param>
        /// <param name="commandType">Optional. The type of command to be
        /// executed. Default value is <c>CommandType.Text</c>.</param>
        /// <returns>A collection of typed objects unless the database has not
        /// been initialized; otherwise, the value will null.</returns>
        /// <example>
        /// <code>
        ///     public class Dog
        ///     {
        ///         public string Name { get; set; }
        ///         public float? Weight { get; set; }
        ///         public int? Age { get; set; }
        ///     }
        ///
        ///     Manager.ExecuteReaderWrapper&lt;Dog&gt;("SELECT Age = @Age").ToList();
        /// </code>
        /// </example>
        public static IEnumerable<T> ExecuteReaderWrapper<T>(
            this IDbManager manager,
            string commandText,
            object parameters = null,
            CommandType commandType = CommandType.Text)
        {
            if (manager == null)
            {
                return Enumerable.Empty<T>();
            }

            IEnumerable<T> items;
            try
            {
                manager.Open();
                items = manager.Connection.Query<T>(commandText, parameters, manager.Transaction, commandType: commandType);
            }
            finally
            {
                manager.Close();
            }

            return items;
        }

#if !NET40 && !NET35
        /// <summary>
        /// Provides a generic wrapper around an <see cref="IDataReader"/> for
        /// dynamically mapping POCO objects asynchronously.
        /// </summary>
        /// <typeparam name="T">The type of data object model being read from
        /// the database.</typeparam>
        /// <param name="manager">The database manager instance ready for
        /// execution.</param>
        /// <param name="commandText">The SQL string command to be performed.
        /// </param>
        /// <param name="parameters">Optional. The query parameters.</param>
        /// <param name="commandType">Optional. The type of command to be
        /// executed. Default value is <c>CommandType.Text</c>.</param>
        /// <returns>A collection of typed objects unless the database has not
        /// been initialized; otherwise, the value will null.</returns>
        /// <example>
        /// <code>
        ///     public class Dog
        ///     {
        ///         public string Name { get; set; }
        ///         public float? Weight { get; set; }
        ///         public int? Age { get; set; }
        ///     }
        ///
        ///     Manager.ExecuteReaderWrapperAsync&lt;Dog&gt;("SELECT Age = @Age").ToList();
        /// </code>
        /// </example>
        public static async Task<IReadOnlyCollection<T>> ExecuteReaderWrapperAsync<T>(
            this IDbManager manager,
            string commandText,
            object parameters = null,
            CommandType commandType = CommandType.Text)
        {
            if (manager == null)
            {
                return new List<T>();
            }

            IReadOnlyCollection<T> items;
            try
            {
                manager.Open();
                IEnumerable<T> result = await manager.Connection.QueryAsync<T>(
                    commandText,
                    parameters,
                    manager.Transaction,
                    commandType: commandType).ConfigureAwait(false);
                items = result.ToList();
            }
            finally
            {
                manager.Close();
            }

            return items;
        }
#endif

        #endregion Reader

        #region Single

        /// <summary>
        /// Provides a generic wrapper around an <see cref="IDataReader"/> for
        /// dynamically mapping a scaler object.
        /// </summary>
        /// <typeparam name="T">The type of data object being returned from the
        /// database.</typeparam>
        /// <param name="manager">The database manager instance ready for
        /// execution.</param>
        /// <param name="commandText">The SQL string command to be performed.
        /// </param>
        /// <param name="parameters">Optional. The query parameters.</param>
        /// <returns>A single entity of typed object unless the database has not
        /// been initialized; otherwise, the value will null.</returns>
        public static T ExecuteScalar<T>(
            this IDbManager manager,
            string commandText,
            object parameters = null)
        {
            if (manager == null)
            {
                return default(T);
            }

            T items;
            try
            {
                manager.Open();

#if NET35
                items = manager.Connection.ExecuteScalar<T>(commandText, parameters, manager.Transaction, null, CommandType.Text);
#else
                items = manager.Connection.QuerySingle<T>(
                    commandText,
                    parameters,
                    manager.Transaction,
                    commandType: CommandType.Text);
#endif
            }
            finally
            {
                manager.Close();
            }

            return items;
        }

#if !NET40 && !NET35
        /// <summary>
        /// Provides a generic wrapper around an <see cref="IDataReader"/> for
        /// dynamically mapping a scaler object.
        /// </summary>
        /// <typeparam name="T">The type of data object being returned from the
        /// database.</typeparam>
        /// <param name="manager">The database manager instance ready for
        /// execution.</param>
        /// <param name="commandText">The SQL string command to be performed.
        /// </param>
        /// <param name="parameters">Optional. The query parameters.</param>
        /// <returns>A single entity of typed object unless the database has not
        /// been initialized; otherwise, the value will null.</returns>
        public static async Task<T> ExecuteScalerAsync<T>(
            this IDbManager manager,
            string commandText,
            object parameters = null)
        {
            if (manager == null)
            {
                return default(T);
            }

            T items;
            try
            {
                manager.Open();
                items = await manager.Connection.QuerySingleAsync<T>(
                    commandText,
                    parameters,
                    manager.Transaction,
                    commandType: CommandType.Text).ConfigureAwait(false);
            }
            finally
            {
                manager.Close();
            }

            return items;
        }
#endif

        #endregion Single

        #region Non Query

        /// <summary>
        /// Provides a generic wrapper around executing a database non-query
        /// while using dynamically mapped parameters.
        /// </summary>
        /// <param name="manager">The database manager instance ready for
        /// execution.</param>
        /// <param name="commandText">The SQL string command to be performed.
        /// </param>
        /// <param name="parameters">Optional. The query parameters.</param>
        /// <param name="commandType">Optional. The type of command to be
        /// executed. Default value is <c>CommandType.Text</c>.</param>
        /// <returns>
        /// True if the database transaction completed successfully; otherwise,
        /// false.
        /// </returns>
        public static bool ExecuteNonQueryWrapper(
            this IDbManager manager,
            string commandText,
            object parameters = null,
            CommandType commandType = CommandType.Text)
        {
            if (manager == null)
            {
                return false;
            }

            int returnValue;
            try
            {
                manager.Open();
                returnValue = manager.Connection.Execute(
                    commandText,
                    parameters,
                    manager.Transaction,
                    commandType: commandType);
            }
            finally
            {
                manager.Close();
            }

            return Convert.ToBoolean(returnValue);
        }

#if !NET40 && !NET35
        /// <summary>
        /// Provides a generic wrapper around executing a database non-query
        /// while using dynamically mapped parameters asynchronously.
        /// </summary>
        /// <param name="manager">The database manager instance ready for
        /// execution.</param>
        /// <param name="commandText">The SQL string command to be performed.
        /// </param>
        /// <param name="parameters">Optional. The query parameters.</param>
        /// <param name="commandType">Optional. The type of command to be
        /// executed. Default value is <c>CommandType.Text</c>.</param>
        /// <returns>
        /// True if the database transaction completed successfully; otherwise,
        /// false.
        /// </returns>
        public static async Task<bool> ExecuteNonQueryWrapperAsync(
            this IDbManager manager,
            string commandText,
            object parameters = null,
            CommandType commandType = CommandType.Text)
        {
            if (manager == null)
            {
                return false;
            }

            int returnValue;
            try
            {
                manager.Open();
                returnValue = await manager.Connection.ExecuteAsync(
                    commandText,
                    parameters,
                    manager.Transaction,
                    commandType: commandType).ConfigureAwait(false);
            }
            finally
            {
                manager.Close();
            }

            return Convert.ToBoolean(returnValue);
        }
#endif

        #endregion Non Query
    }
}