#if NET35_OR_GREATER

namespace Oliviann.Data.SqlClient
{
    #region Usings

    using System.Data;
    using System.Data.SqlClient;

    #endregion

    /// <summary>
    /// Represents a collection of commonly used extension methods for extending
    /// SQL parameter collections.
    /// </summary>
    public static class SqlParameterCollectionExtensions
    {
        /// <summary>
        /// Adds a value to the end of the parameter collection.
        /// </summary>
        /// <param name="collection">The parameter collection.</param>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <param name="sqlDbType">The <see cref="T:System.Data.SqlDbType" />
        /// of the parameter to add to the collection.</param>
        /// <param name="value">The value to be added. Use
        /// <see cref="F:System.DBNull.Value" /> instead of null, to indicate a
        /// null value.</param>
        public static void AddWithValue(
            this SqlParameterCollection collection,
            string parameterName,
            SqlDbType sqlDbType,
            object value)
        {
            Oliviann.ADP.CheckArgumentNull(collection, nameof(collection));
            collection.Add(new SqlParameter { ParameterName = parameterName, SqlDbType = sqlDbType, Value = value });
        }
    }
}

#endif