namespace Oliviann.Data.Common
{
    #region Usings

    using System.Data.Common;

    #endregion Usings

    /// <summary>
    /// Represents a generic database provider factory for wrapping a sealed or
    /// singleton <see cref="DbProviderFactory"/> instance.
    /// </summary>
    /// <typeparam name="T">The type of database provider factory.</typeparam>
    /// <seealso cref="System.Data.Common.DbProviderFactory" />
    public abstract class GenericDbProviderFactory<T> : DbProviderFactory where T : DbProviderFactory
    {
        #region Fields

        /// <summary>
        /// The database provider factory instance.
        /// </summary>
        private readonly T factory;

        #endregion Fields

        #region Constructor/Destructor

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="GenericDbProviderFactory{T}" /> class.
        /// </summary>
        /// <param name="providerFactory">The provider factory instance.</param>
        protected GenericDbProviderFactory(T providerFactory)
        {
            Oliviann.ADP.CheckArgumentNull(providerFactory, nameof(providerFactory));
            this.factory = providerFactory;
        }

        #endregion Constructor/Destructor

        #region Properties

        /// <summary>
        /// Specifies whether the specific
        /// <see cref="T:System.Data.Common.DbProviderFactory" /> supports the
        /// <see cref="T:System.Data.Common.DbDataSourceEnumerator" /> class.
        /// </summary>
        /// <returns>True if the instance of the
        /// <see cref="T:System.Data.Common.DbProviderFactory" /> supports the
        /// <see cref="T:System.Data.Common.DbDataSourceEnumerator" /> class;
        /// otherwise false.</returns>
        public override bool CanCreateDataSourceEnumerator => this.factory.CanCreateDataSourceEnumerator;

        #endregion Properties

        #region Methods

        /// <summary>Returns a new instance of the provider's class that
        /// implements the <see cref="T:System.Data.Common.DbCommand" /> class.
        /// </summary>
        /// <returns>A new instance of
        /// <see cref="T:System.Data.Common.DbCommand" />.</returns>
        public override DbCommand CreateCommand() => this.factory.CreateCommand();

        /// <summary>Returns a new instance of the provider's class that
        /// implements the <see cref="T:System.Data.Common.DbCommandBuilder" />
        /// class.</summary>
        /// <returns>A new instance of
        /// <see cref="T:System.Data.Common.DbCommandBuilder" />.</returns>
        public override DbCommandBuilder CreateCommandBuilder() => this.factory.CreateCommandBuilder();

        /// <summary>Returns a new instance of the provider's class that
        /// implements the <see cref="T:System.Data.Common.DbConnection" />
        /// class.</summary>
        /// <returns>A new instance of
        /// <see cref="T:System.Data.Common.DbConnection" />.</returns>
        public override DbConnection CreateConnection() => this.factory.CreateConnection();

        /// <summary>Returns a new instance of the provider's class that
        /// implements the
        /// <see cref="T:System.Data.Common.DbConnectionStringBuilder" /> class.
        /// </summary>
        /// <returns>A new instance of
        /// <see cref="T:System.Data.Common.DbConnectionStringBuilder" />.
        /// </returns>
        public override DbConnectionStringBuilder CreateConnectionStringBuilder() => this.factory.CreateConnectionStringBuilder();

        /// <summary>Returns a new instance of the provider's class that
        /// implements the <see cref="T:System.Data.Common.DbDataAdapter" />
        /// class.</summary>
        /// <returns>A new instance of
        /// <see cref="T:System.Data.Common.DbDataAdapter" />.</returns>
        public override DbDataAdapter CreateDataAdapter() => this.factory.CreateDataAdapter();

        /// <summary>Returns a new instance of the provider's class that
        /// implements the
        /// <see cref="T:System.Data.Common.DbDataSourceEnumerator" /> class.
        /// </summary>
        /// <returns>A new instance of
        /// <see cref="T:System.Data.Common.DbDataSourceEnumerator" />.
        /// </returns>
        public override DbDataSourceEnumerator CreateDataSourceEnumerator() => this.factory.CreateDataSourceEnumerator();

        /// <summary>Returns a new instance of the provider's class that
        /// implements the <see cref="T:System.Data.Common.DbParameter" />
        /// class.</summary>
        /// <returns>A new instance of
        /// <see cref="T:System.Data.Common.DbParameter" />.</returns>
        public override DbParameter CreateParameter() => this.factory.CreateParameter();

#if NETFRAMEWORK

        /// <summary>Returns a new instance of the provider's class that
        /// implements the provider's version of the
        /// <see cref="T:System.Security.CodeAccessPermission" /> class.
        /// </summary>
        /// <returns>A <see cref="T:System.Security.CodeAccessPermission" />
        /// object for the specified
        /// <see cref="T:System.Security.Permissions.PermissionState" />.
        /// </returns>
        /// <param name="state">One of the
        /// <see cref="T:System.Security.Permissions.PermissionState" /> values.
        /// </param>
        public override System.Security.CodeAccessPermission CreatePermission(System.Security.Permissions.PermissionState state)
            => this.factory.CreatePermission(state);

#endif

        #endregion Methods
    }
}