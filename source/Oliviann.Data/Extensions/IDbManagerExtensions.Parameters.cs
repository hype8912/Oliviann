namespace Oliviann.Data
{
    #region Usings

    using System.Data;
    using Oliviann.Data.Base;

    #endregion

    /// <summary>
    /// Represents a
    /// </summary>
    public static partial class IDbManagerExtensions
    {
        /// <summary>
        /// Adds an <see cref="IDataParameter" /> to the parameter collection
        /// given the parameter name and value.
        /// </summary>
        /// <param name="manager">The database manager instance.</param>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <param name="value">An object that is the value of the
        /// <see cref="IDataParameter" />.</param>
        public static void AddParameter(this IDbManager manager, string parameterName, object value) =>
            AddParameter(manager, parameterName, value, ParameterDirection.Input);

               /// <summary>
        /// Adds an <see cref="IDataParameter" /> to the parameter collection
        /// given the parameter name, value, and direction.
        /// </summary>
        /// <param name="manager">The database manager instance.</param>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <param name="value">An object that is the value of the
        /// <see cref="IDataParameter" />.</param>
        /// <param name="parameterDirection">One of the
        /// <see cref="ParameterDirection" /> values.</param>
        public static void AddParameter(this IDbManager manager, string parameterName, object value, ParameterDirection parameterDirection)
        {
            IDataParameter parameter = CreateNewParameter(manager);
            if (parameter == null)
            {
                return;
            }

            parameter.Direction = parameterDirection;
            parameter.ParameterName = parameterName;
            parameter.Value = value;
            manager.AddParameter(parameter);
        }

        /// <summary>
        /// Adds an <see cref="IDataParameter" /> to the parameter collection
        /// given the parameter name, value, direction, type, and column.
        /// </summary>
        /// <param name="manager">The database manager instance.</param>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <param name="value">An object that is the value of the
        /// <see cref="IDataParameter" />.</param>
        /// <param name="parameterDirection">One of the
        /// <see cref="ParameterDirection" /> values.</param>
        /// <param name="dataType">One of the <see cref="DbType" /> values.</param>
        /// <param name="sourceColumn">The name of the source column.</param>
        public static void AddParameter(
                                        this IDbManager manager,
                                        string parameterName,
                                        object value,
                                        ParameterDirection parameterDirection,
                                        DbType dataType,
                                        string sourceColumn)
        {
            IDataParameter parameter = CreateNewParameter(manager);
            if (parameter == null)
            {
                return;
            }

            parameter.Direction = parameterDirection;
            parameter.ParameterName = parameterName;
            parameter.Value = value;
            parameter.DbType = dataType;
            parameter.SourceColumn = sourceColumn;
            manager.AddParameter(parameter);
        }

        #region Helper Methods

        /// <summary>
        /// Creates new database command parameter from the manager.
        /// </summary>
        /// <param name="manager">The database manager instance.</param>
        /// <returns>A database command parameter object.</returns>
        private static IDataParameter CreateNewParameter(IDbManager manager)
        {
            IDataParameter parameter = manager.CustomFactory != null
                ? manager.CustomFactory.CreateParameter()
                : DbManagerFactory.CreateParameter(manager.Provider);

            return parameter;
        }

        #endregion
    }
}