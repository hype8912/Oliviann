namespace Oliviann.Data.ConnectionStrings
{
    /// <summary>
    /// A database connection string builder.
    /// </summary>
    internal static class DbConnectionStringFactory
    {
        /// <summary>
        /// Creates a new connection string object for the specified database
        /// provider.
        /// </summary>
        /// <param name="providerType">Type of the provider.</param>
        /// <returns>A database connection string object.</returns>
        internal static IDbConnectionString CreateConnectionString(DatabaseProvider providerType)
        {
            switch (providerType)
            {
                case DatabaseProvider.MicrosoftAccess:
                    return new Access2003ConnectionString();

                case DatabaseProvider.MicrosoftAccess12:
                case DatabaseProvider.MicrosoftAccess14:
                    return new Access2007ConnectionString();

                case DatabaseProvider.MicrosoftExcel:
                case DatabaseProvider.MicrosoftExcel12:
                case DatabaseProvider.MicrosoftExcel14:
                    return new Excel2003ConnectionString();
#if NETFRAMEWORK
                case DatabaseProvider.Oracle:
                    goto default;
#endif
#if SQL_CE
                case DatabaseProvider.MicrosoftSqlCompact:
                    return new SqlCompactConnectionString();
#endif
                case DatabaseProvider.MicrosoftSqlServer:
                    return new SqlConnectionString();

                default:
                    return null;
            }
        }

        /// <summary>
        /// Creates a new connection string object using the specified database
        /// object.
        /// </summary>
        /// <param name="databaseObject">The database object.</param>
        /// <returns>A database connection string object.</returns>
        internal static IDbConnectionString CreateConnectionString(IDatabase databaseObject)
        {
            IDbConnectionString connection = CreateConnectionString(databaseObject.DatabaseType);
            if (connection != null)
            {
                connection.DatabaseItem = databaseObject;
            }

            return connection;
        }
    }
}