namespace Oliviann.Data.DatabaseManagement.Shrink
{
    /// <summary>
    /// Represents a factory for shrinking databases.
    /// </summary>
    internal static class DbShrinkFactory
    {
        /// <summary>
        /// Creates a new shrink agent for the specified database provider if
        /// supported.
        /// </summary>
        /// <param name="databaseObject">The database object.</param>
        /// <returns>
        /// A new <see cref="IDbShrink"/> object if
        /// <see cref="DatabaseProvider"/> is supported; otherwise, <c>null</c>.
        /// </returns>
        internal static IDbShrink CreateShrinkAgent(IDatabase databaseObject)
        {
            switch (databaseObject.DatabaseType)
            {
                case DatabaseProvider.MicrosoftAccess:
                    return new CompactAccess(databaseObject);

                case DatabaseProvider.MicrosoftAccess12:
                case DatabaseProvider.MicrosoftAccess14:
                    return new CompactAccess12(databaseObject);

                case DatabaseProvider.MicrosoftSqlServer:
                    return new ShrinkMsSqlServer(databaseObject);

                default:
                    return null;
            }
        }
    }
}