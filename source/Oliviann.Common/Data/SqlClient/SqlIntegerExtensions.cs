namespace Oliviann.Data.SqlClient
{
    #region Usings

    using System.Data.SqlTypes;

    #endregion Usings

    /// <summary>
    /// Represents a collection of commonly used extension methods for extending
    /// SQL integers.
    /// </summary>
    public static class SqlIntegerExtensions
    {
        #region Converts

        /// <summary>
        /// Converts a 16-bit SQL integer value to a nullable 16-bit integer
        /// value.
        /// </summary>
        /// <param name="value">The 16-bit SQL integer value.</param>
        /// <returns>A 32-bit nullable integer of the same value.</returns>
        public static short? ToNullableInt16(this SqlInt16 value)
        {
            return value.IsNull ? (short?)null : value.Value;
        }

        /// <summary>
        /// Converts a 32-bit SQL integer value to a nullable 32-bit integer
        /// value.
        /// </summary>
        /// <param name="value">The 32-bit SQL integer value.</param>
        /// <returns>A 32-bit nullable integer of the same value.</returns>
        public static int? ToNullableInt32(this SqlInt32 value)
        {
            return value.IsNull ? (int?)null : value.Value;
        }

        /// <summary>
        /// Converts a 64-bit SQL integer value to a nullable 64-bit integer
        /// value.
        /// </summary>
        /// <param name="value">The 64-bit SQL integer value.</param>
        /// <returns>A 64-bit nullable integer of the same value.</returns>
        public static long? ToNullableInt64(this SqlInt64 value)
        {
            return value.IsNull ? (long?)null : value.Value;
        }

        #endregion Converts
    }
}