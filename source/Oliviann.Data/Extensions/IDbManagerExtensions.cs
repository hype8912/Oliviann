namespace Oliviann.Data
{
    #region Usings

    using System;
    using System.Collections.Generic;
    using System.Data;

    #endregion Usings

    /// <summary>
    /// Represents a collection of commonly used extension methods for extending
    /// <see cref="IDbManager"/> objects.
    /// </summary>
    public static partial class IDbManagerExtensions
    {
        #region Non Query

        /// <summary>
        /// Provides a generic wrapper around executing a database non query.
        /// </summary>
        /// <typeparam name="T">The type of data object model being sent to the
        /// database.</typeparam>
        /// <param name="manager">The database manager instance ready for
        /// execution.</param>
        /// <param name="commandText">The SQL string command to be performed.
        /// </param>
        /// <param name="parameters">The data parameters and values. May use
        /// <c>null</c> if no parameters are required for the query. An @ symbol
        /// will automatically be added to the beginning of each parameter key
        /// if one is not already there.
        /// </param>
        /// <param name="data">The collection of data being sent to the
        /// database.</param>
        /// <param name="failedItemHandler">A handler for when an item fails by
        /// returning the failed object. Useful when wanting the transaction to
        /// continue by need to know what single item failed.</param>
        /// <param name="exceptionHandler">A database exception handler if an
        /// exception is thrown.</param>
        /// <returns>
        /// True if the database transaction completed successfully; otherwise,
        /// false.
        /// </returns>
        public static bool ExecuteNonQueryWrapper<T>(
                                                     this IDbManager manager,
                                                     string commandText,
                                                     Func<T, IDictionary<string, object>> parameters,
                                                     IEnumerable<T> data,
                                                     Action<T> failedItemHandler = null,
                                                     Action<Exception> exceptionHandler = null)
        {
            if (manager == null)
            {
                return false;
            }

            int returnValue = 1;

            try
            {
                manager.BeginTransaction();
                foreach (T record in data)
                {
                    // Add parameters to manager.
                    parameters?.Invoke(record).FormatAddParameters(manager);

                    returnValue = manager.ExecuteNonQuery(CommandType.Text, commandText);
                    if (returnValue == 0)
                    {
                        if (failedItemHandler != null)
                        {
                            failedItemHandler(record);
                        }
                        else
                        {
                            break;
                        }
                    }

                    manager.ClearParameters();
                }

                manager.CommitTransaction();
            }
            catch (Exception ex)
            {
                returnValue = 0;
                manager.RollbackTransaction();
                exceptionHandler?.Invoke(ex);
            }
            finally
            {
                manager.Close();
            }

            return Convert.ToBoolean(returnValue);
        }

        #endregion Non Query

        #region Scaler

        /// <summary>
        /// Provides a generic wrapper around an scalar database object.
        /// </summary>
        /// <param name="manager">The database manager instance ready for
        /// execution.</param>
        /// <param name="commandText">The SQL string command to be performed.
        /// </param>
        /// <param name="parameters">The data parameters and values. May use
        /// <c>null</c> if no parameters are required for the query. An @ symbol
        /// will automatically be added to the beginning of each parameter key.
        /// </param>
        /// <param name="exceptionHandler">A database exception handler if an
        /// exception is thrown.</param>
        /// <returns>
        /// An object value returned from the database.
        /// </returns>
        public static object ExecuteScalerWrapper(
            this IDbManager manager,
            string commandText,
            IDictionary<string, object> parameters,
            Action<Exception> exceptionHandler = null) =>
            manager.ExecuteScalerWrapper<object>(commandText, parameters, exceptionHandler);

        /// <summary>
        /// Provides a generic wrapper around an scalar database object.
        /// </summary>
        /// <typeparam name="T">The type of data object being read from the
        /// database.</typeparam>
        /// <param name="manager">The database manager instance ready for
        /// execution.</param>
        /// <param name="commandText">The SQL string command to be performed.
        /// </param>
        /// <param name="parameters">The data parameters and values. May use
        /// <c>null</c> if no parameters are required for the query. An @ symbol
        /// will automatically be added to the beginning of each parameter key.
        /// </param>
        /// <param name="exceptionHandler">A database exception handler if an
        /// exception is thrown.</param>
        /// <returns>
        /// An object value returned from the database. An object returned from
        /// the database converted to the type <typemref name="T"/>.
        /// </returns>
        public static T ExecuteScalerWrapper<T>(
                                                this IDbManager manager,
                                                string commandText,
                                                IDictionary<string, object> parameters,
                                                Action<Exception> exceptionHandler = null) where T : class
        {
            if (manager == null)
            {
                return default(T);
            }

            object returnValue;

            try
            {
                // Add parameters to manager.
                parameters.FormatAddParameters(manager);
                returnValue = manager.ExecuteScalar(CommandType.Text, commandText);
            }
            catch (Exception ex)
            {
                returnValue = 0;
                exceptionHandler?.Invoke(ex);
            }
            finally
            {
                manager.Close();
            }

            return (T)returnValue;
        }

        #endregion Scaler

        #region Transactions

        /// <summary>
        /// Starts a database transaction with the specified isolation level.
        /// </summary>
        /// <param name="manager">The database manager instance.</param>
        public static void BeginTransaction(this IDbManager manager) => manager?.BeginTransaction(IsolationLevel.Unspecified);

        #endregion

        #region Extension Helpers

        /// <summary>Formats the add parameters for adding them to the specified
        /// <paramref name="manager"/>.</summary>
        /// <param name="parameters">The parameters to be added to the
        /// <paramref name="manager"/>.</param>
        /// <param name="manager">The database manager instance.</param>
        private static void FormatAddParameters(this IEnumerable<KeyValuePair<string, object>> parameters, IDbManager manager)
        {
            if (parameters == null || manager == null)
            {
                return;
            }

            foreach (KeyValuePair<string, object> pair in parameters)
            {
                manager.AddParameter(pair.Key.StartsWith('@') ? pair.Key : '@' + pair.Key, pair.Value);
            }
        }

        #endregion Extension Helpers
    }
}