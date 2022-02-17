namespace Oliviann.Data.Base
{
    #region Usings

    using System;
    using System.Data;
    using System.Data.Common;
    using System.Data.SqlTypes;
    using System.Reflection;

    #endregion Usings

    /// <summary>
    /// Represents a database manager factory class.
    /// </summary>
    internal static class DbManagerFactory
    {
        #region Create

        /// <summary>
        /// Creates a new database connection object.
        /// </summary>
        /// <param name="provider">Type of the provider.</param>
        /// <returns>A strong typed database connection object.</returns>
        internal static DbConnection CreateConnection(DatabaseProvider provider)
        {
            var connection = CreateProviderType2(provider, factory => factory.CreateConnection());
            return connection;
        }

        /// <summary>
        /// Creates a new database command object.
        /// </summary>
        /// <param name="provider">Type of the provider.</param>
        /// <returns>A database command object.</returns>
        internal static IDbCommand CreateCommand(DatabaseProvider provider)
        {
            var command = CreateProviderType2<IDbCommand>(provider, factory => factory.CreateCommand());
            return command;
        }

        /// <summary>
        /// Creates a new database adapter object.
        /// </summary>
        /// <param name="provider">Type of the provider.</param>
        /// <returns>A database adapter object.</returns>
        internal static IDbDataAdapter CreateDataAdapter(DatabaseProvider provider)
        {
            var adapter = CreateProviderType2<IDbDataAdapter>(provider, factory => factory.CreateDataAdapter());
            return adapter;
        }

        /// <summary>
        /// Creates a new database command parameter object.
        /// </summary>
        /// <param name="provider">Type of the provider.</param>
        /// <returns>A database command parameter object.</returns>
        internal static IDataParameter CreateParameter(DatabaseProvider provider)
        {
            var parameter = CreateProviderType2<IDbDataParameter>(provider, factory => factory.CreateParameter());
            return parameter;
        }

        #endregion Create

        #region Types

        /// <summary>
        /// Gets the date time value in the correct data type for the specified
        /// provider type.
        /// </summary>
        /// <param name="providerType">Type of the provider.</param>
        /// <param name="date">The date time value.</param>
        /// <returns>
        /// The database specific <c>DateTime</c> type.
        /// </returns>
        internal static object GetDbDateTime(DatabaseProvider providerType, DateTime date)
        {
            switch (providerType)
            {
                case DatabaseProvider.MicrosoftAccess:
                case DatabaseProvider.MicrosoftAccess12:
                case DatabaseProvider.MicrosoftAccess14:
                case DatabaseProvider.MicrosoftAccess15:
                    return date.ToOADate();

                case DatabaseProvider.MicrosoftSqlServer:
                    return date == DateTime.MinValue ? SqlDateTime.MinValue : new SqlDateTime(date);

                default:
                    return date;
            }
        }

        #endregion Types

        #region Helpers

        /// <summary>
        /// Creates a new instance of the provider type.
        /// </summary>
        /// <typeparam name="T">The type of the instance to output.</typeparam>
        /// <param name="provider">The type of provider.</param>
        /// <param name="factory">The function to return the type from the
        /// factory.</param>
        /// <returns>A newly create provider instance.</returns>
        private static T CreateProviderType2<T>(DatabaseProvider provider, Func<DbProviderFactory, T> factory) where T : class
        {
            Type providerFactoryType = provider.GetAttribute<ProviderAttribute>()?.DbProviderFactoryType;
            if (providerFactoryType == null || providerFactoryType.IsValueType)
            {
                return null;
            }

            FieldInfo providerFactoryInfo = providerFactoryType.GetField("Instance", BindingFlags.Public | BindingFlags.Static);
            if (providerFactoryInfo == null)
            {
                return null;
            }

            if (providerFactoryInfo.GetValue(null) is DbProviderFactory providerFactory)
            {
                T factoryResult = factory(providerFactory);
                return factoryResult;
            }

            return null;
        }

        #endregion Helpers
    }
}