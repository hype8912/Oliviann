#if !NETSTANDARD1_3

namespace Oliviann.Data
{
    #region Usings

    using System.Data;

    #endregion

    /// <summary>
    /// Represents a collection of commonly used extension methods for extending
    /// data table  collection objects.
    /// </summary>
    public static class DataTableCollectionExtensions
    {
        /// <summary>
        /// Removes the specified table name from the collection of tables.
        /// </summary>
        /// <param name="tables">The collection of tables.</param>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="table">The data table being removed.</param>
        /// <returns>
        /// True if the table was found and removed; otherwise, false.
        /// </returns>
        public static bool TryRemove(this DataTableCollection tables, string tableName, out DataTable table)
        {
            table = tables?[tableName];
            if (table != null)
            {
                tables.Remove(table);
                return true;
            }

            return false;
        }
    }
}

#endif