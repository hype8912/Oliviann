namespace Oliviann.Data
{
    #region Usings

    using Oliviann.Data.Common;
    using SqlKata.Compilers;

    #endregion

    /// <summary>
    /// Represents a MS SQL Server query string builder.
    /// </summary>
    public class SqlQueryStringBuilder<T> : DbQueryStringBuilder<T>
    {
        #region Constructor/Destructor

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="SqlQueryStringBuilder{T}"/> class.
        /// </summary>
        /// <param name="entity">The entity object to build the query from.
        /// </param>
        public SqlQueryStringBuilder(T entity) : base(entity, new SqlServerCompiler { UseLegacyPagination = false })
        {
        }

        #endregion
    }
}