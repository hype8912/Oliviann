namespace Oliviann.Data.ConnectionStrings.Builder
{
    /// <summary>
    /// Interface for creating an Ms Access 2007 connection string.
    /// </summary>
    public interface IAccess2007ConnectionStringBuilder : IDbConnectionStringBuilder
    {
        /// <summary>
        /// Sets the data source.
        /// </summary>
        /// <value>The data source.</value>
        string DataSource { set; }

        /// <summary>
        /// Sets the password.
        /// </summary>
        /// <value>The password.</value>
        string Password { set; }

        /// <summary>
        /// Sets the provider.
        /// </summary>
        /// <value>The provider.</value>
        string Provider { set; }

        /// <summary>
        /// Sets the user id.
        /// </summary>
        /// <value>The user id.</value>
        string UserId { set; }
    }
}