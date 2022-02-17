namespace Oliviann.Data
{
    #region Usings

    using Oliviann.Data.Common;
    using SqlKata.Compilers;

    #endregion

    /// <summary>
    /// Represents a MySql query string builder.
    /// </summary>
    public class MySqlQueryStringBuilder<T> : DbQueryStringBuilder<T>
    {
        #region Constructor/Destructor

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="MySqlQueryStringBuilder{T}"/> class.
        /// </summary>
        /// <param name="entity">The entity object to build the query from.
        /// </param>
        public MySqlQueryStringBuilder(T entity) : base(entity, new MySqlCompiler())
        {
        }

        #endregion
    }
}