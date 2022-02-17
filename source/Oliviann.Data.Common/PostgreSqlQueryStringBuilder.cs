namespace Oliviann.Data
{
    #region Usings

    using Oliviann.Data.Common;
    using SqlKata.Compilers;

    #endregion

    /// <summary>
    /// Represents a PostgreSQL query string builder.
    /// </summary>
    public class PostgreSqlQueryStringBuilder<T> : DbQueryStringBuilder<T>
    {
        #region Constructor/Destructor

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="PostgreSqlQueryStringBuilder{T}"/> class.
        /// </summary>
        /// <param name="entity">The entity object to build the query from.
        /// </param>
        public PostgreSqlQueryStringBuilder(T entity) : base(entity, new PostgresCompiler())
        {
        }

        #endregion
    }
}