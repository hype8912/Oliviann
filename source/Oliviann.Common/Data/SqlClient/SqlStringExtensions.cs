namespace Oliviann.Data.SqlClient
{
    #region Usings

    using System.Data.SqlTypes;

    #endregion Usings

    /// <summary>
    /// Represents a collection of commonly used extension methods for extending
    /// SQL string.
    /// </summary>
    public static class SqlStringExtensions
    {
        /// <summary>
        /// Gets the current value or the specified default value.
        /// </summary>
        /// <param name="value">The value to be retrieved.</param>
        /// <param name="defaultValue">The default value to be returned if the
        /// specified value is null.</param>
        /// <returns>The specified value as a string if not null; otherwise, the
        /// default value.</returns>
        public static string ValueOrDefault(this SqlString value, string defaultValue = null)
        {
            return value.IsNull ? defaultValue : value.Value;
        }
    }
}