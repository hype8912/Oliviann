namespace Oliviann.Data.Common
{
    /// <summary>
    /// Represents a collection of column data.
    /// </summary>
    public class ColumnData
    {
        #region Properties

        /// <summary>
        /// Gets or sets the sort order of the column.
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// Gets or sets the name of the column.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a value this column is a primary key.
        /// </summary>
        public bool IsPrimaryKey { get; set; }

        /// <summary>
        /// Gets or sets the value of the column.
        /// </summary>
        public object Value { get; set; }

        #endregion
    }
}