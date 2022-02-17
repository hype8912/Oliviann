namespace Oliviann.Data
{
    #region Usings

    using Oliviann.Data.Common;
    using SqlKata.Compilers;

    #endregion

    /// <summary>
    /// Represents a SQLite query string builder.
    /// </summary>
    public class SQLiteQueryStringBuilder<T> : DbQueryStringBuilder<T>
    {
        #region Constructor/Destructor

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="SQLiteQueryStringBuilder{T}"/> class.
        /// </summary>
        /// <param name="entity">The entity object to build the query from.
        /// </param>
        public SQLiteQueryStringBuilder(T entity) : base(entity, new SqliteCompiler())
        {
        }

        #endregion
    }
}