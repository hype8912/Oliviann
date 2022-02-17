namespace Oliviann.Data.ConnectionStrings.Builder
{
    /// <summary>
    /// Interface for creating an Ms Access 97-2003 connection string.
    /// </summary>
    public interface IAccess2003ConnectionStringBuilder : IDbConnectionStringBuilder
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
        /// Sets the system database.
        /// </summary>
        /// <value>The system database.</value>
        string SystemDatabase { set; }

        /// <summary>
        /// Sets the user id.
        /// </summary>
        /// <value>The user id.</value>
        string UserId { set; }
    }
}