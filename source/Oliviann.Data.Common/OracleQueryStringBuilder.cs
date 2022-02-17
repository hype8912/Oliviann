namespace Oliviann.Data
{
    #region Usings

    using Oliviann.Data.Common;
    using SqlKata.Compilers;

    #endregion

    /// <summary>
    /// Represents a Oracle query string builder.
    /// </summary>
    public class OracleQueryStringBuilder<T> : DbQueryStringBuilder<T>
    {
        #region Constructor/Destructor

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="OracleQueryStringBuilder{T}"/> class.
        /// </summary>
        /// <param name="entity">The entity object to build the query from.
        /// </param>
        public OracleQueryStringBuilder(T entity) : base(entity, new OracleCompiler { UseLegacyPagination = false })
        {
        }

        #endregion
    }
}