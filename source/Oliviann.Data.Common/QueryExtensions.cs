namespace Oliviann.Data.Common
{
    #region Usings

    using SqlKata;

    #endregion

    /// <summary>
    /// Represents a collection of extension methods to make working with
    /// <see cref="Query"/> easier.
    /// </summary>
    public static class QueryExtensions
    {
        /// <summary>
        /// Compiles a where equals clause for a specific query instance.
        /// </summary>
        /// <param name="query">The query instance</param>
        /// <param name="columnName">The column name to be queried.</param>
        /// <param name="value">The value of the column.</param>
        /// <returns></returns>
        public static Query WhereEquals(this Query query, string columnName, object value) => query.Where(columnName, "=", value);
    }
}