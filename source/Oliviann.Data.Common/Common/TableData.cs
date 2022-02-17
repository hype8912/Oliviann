namespace Oliviann.Data.Common
{
    #region Usings

    using System.Collections.Generic;

    #endregion

    /// <summary>
    /// Represents a collection of table data.
    /// </summary>
    public class TableData
    {
        #region Properties

        /// <summary>
        /// Gets or sets the name of the table.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the collection of columns.
        /// </summary>
        public List<ColumnData> Columns { get; set; } = new List<ColumnData>();

        #endregion
    }
}