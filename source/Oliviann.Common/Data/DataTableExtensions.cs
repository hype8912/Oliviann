#if !NETSTANDARD1_3

namespace Oliviann.Data
{
    #region Usings

    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Text;
    using Oliviann.Collections.Generic;
    using Oliviann.Linq;

    #endregion Usings

    /// <summary>
    /// Represents a collection of commonly used extension methods for extending
    /// data table objects.
    /// </summary>
    public static class DataTableExtensions
    {
        /// <summary>
        /// Retrieves all the values of the specified column name to a
        /// collection.
        /// </summary>
        /// <typeparam name="T">The type of objects in the column.</typeparam>
        /// <param name="table">The data table instance.</param>
        /// <param name="columnName">Name of the column. Case insensitive.</param>
        /// <returns>A collection of values in the specified column name.
        /// </returns>
        public static IReadOnlyCollection<T> ColumnValuesToCollection<T>(this DataTable table, string columnName)
        {
            if (table == null || table.Rows.Count < 1 || columnName.IsNullOrEmpty())
            {
                return CollectionHelpers.CreateReadOnlyCollection<T>();
            }

            int columnIndex = table.Columns.IndexOf(columnName);
            if (columnIndex < 0)
            {
                return CollectionHelpers.CreateReadOnlyCollection<T>();
            }

            return table.Rows.OfType<DataRow>().Select(dr => dr.Field<T>(columnIndex)).ToListHelper();
        }

        /// <summary>
        /// Sets all the columns of the table to read only if set to true.
        /// </summary>
        /// <param name="table">The data table instance.</param>
        /// <param name="readOnly">Optional. If set to true then all the columns
        /// of the data table will be set to read only. If set to false the all
        /// the columns will be set to not read only. Default value is true.
        /// </param>
        /// <returns>The updated data table instance; otherwise, null.</returns>
        public static DataTable SetColumnsReadOnly(this DataTable table, bool readOnly = true)
        {
            if (table == null)
            {
                return null;
            }

            table.Columns.OfType<DataColumn>().ForEach(column => column.ReadOnly = readOnly);
            return table;
        }

        /// <summary>
        /// Converts a data table into a comma separated value string.
        /// </summary>
        /// <param name="table">The data table to be read.</param>
        /// <returns>A comma separated value string that represents the data in
        /// the table.</returns>
        public static string ToCSV(this DataTable table)
        {
            // Return null if there are no columns in the table.
            if (table == null || table.Columns.Count == 0)
            {
                return string.Empty;
            }

            var builder = new StringBuilder();
            Action cleanLineEndings = () => builder.Replace(",", Environment.NewLine, builder.Length - 1, 1);
            Action<string> addColumnToBuilder = col =>
                {
                    if (col.IsNullOrEmpty())
                    {
                        builder.Append(',');
                        return;
                    }

                    string cleanString = col.Replace("\"", "\"\"");
                    builder.Append(cleanString.Contains(',') ? $"\"{cleanString}\"," : $"{cleanString},");
                };

            // Write the column headers.
            foreach (DataColumn column in table.Columns)
            {
                addColumnToBuilder(column.ColumnName);
            }

            cleanLineEndings();

            // Write the column values.
            foreach (DataRow row in table.Rows)
            {
                foreach (object cell in row.ItemArray)
                {
                    addColumnToBuilder(cell.ToString());
                }

                cleanLineEndings();
            }

            return builder.ToString();
        }
    }
}

#endif