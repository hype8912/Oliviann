namespace Oliviann.Data
{
    #region Usings

    using System.Data;

    #endregion Usings

    /// <summary>
    /// Represents a collection of commonly used extension methods for extending
    /// data connection objects.
    /// </summary>
    public static class ConnectionExtensions
    {
        /// <summary>
        /// Safely closes a database connection by checking if it's null first
        /// and if the database connection is open.
        /// </summary>
        /// <param name="connection">The connection object.</param>
        public static void CloseSafe(this IDbConnection connection)
        {
            if (connection != null && connection.State != ConnectionState.Closed)
            {
                connection.Close();
            }
        }
    }
}