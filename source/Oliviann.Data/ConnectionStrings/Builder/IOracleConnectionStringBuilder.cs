namespace Oliviann.Data.ConnectionStrings.Builder
{
    /// <summary>
    /// Interface for creating an Oracle connection string.
    /// </summary>
    public interface IOracleConnectionStringBuilder : IDbConnectionStringBuilder
    {
        /// <summary>
        /// Sets the connection lifetime.
        /// </summary>
        /// <value>The connection lifetime.</value>
        int ConnectionLifetime { set; }

        /// <summary>
        /// Sets the connection timeout.
        /// </summary>
        /// <value>The connection timeout.</value>
        int ConnectionTimeout { set; }

        /// <summary>
        /// Sets the data source.
        /// </summary>
        /// <value>The data source.</value>
        string DataSource { set; }

        /// <summary>
        /// Sets the DBA privilege.
        /// </summary>
        /// <value>The DBA privilege.</value>
        string DbaPrivilege { set; }

        /// <summary>
        /// Sets the size of the decrease pool.
        /// </summary>
        /// <value>The size of the decrease pool.</value>
        int DecreasePoolSize { set; }

        /// <summary>
        /// Sets the size of the increase pool.
        /// </summary>
        /// <value>The size of the increase pool.</value>
        int IncreasePoolSize { set; }

        /// <summary>
        /// Sets a value indicating whether [integrated security].
        /// </summary>
        /// <value><c>True</c> if [integrated security]; otherwise, <c>false</c>.</value>
        bool IntegratedSecurity { set; }

        /// <summary>
        /// Sets the minimum size of the pool.
        /// </summary>
        /// <value>The minimum size of the pool.</value>
        int MinimumPoolSize { set; }

        /// <summary>
        /// Sets the password.
        /// </summary>
        /// <value>The password.</value>
        string Password { set; }

        /// <summary>
        /// Sets the proxy password.
        /// </summary>
        /// <value>The proxy password.</value>
        string ProxyPassword { set; }

        /// <summary>
        /// Sets the proxy user id.
        /// </summary>
        /// <value>The proxy user id.</value>
        string ProxyUserId { set; }

        /// <summary>
        /// Sets the user id.
        /// </summary>
        /// <value>The user id.</value>
        string UserId { set; }
    }
}