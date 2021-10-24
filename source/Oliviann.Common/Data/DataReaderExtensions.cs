namespace Oliviann.Data
{
    #region Usings

    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics;

    #endregion Usings

    /// <summary>
    /// Represents a collection of commonly used extension methods for extending
    /// data reader objects.
    /// </summary>
    public static class DataReaderExtensions
    {
        /// <summary>
        /// Returns an <see cref="IEnumerable{T}"/> view of the specified data
        /// reader.
        /// </summary>
        /// <param name="reader">The data reader object.</param>
        /// <returns>An enumerable <see cref="IDataRecord"/> returned by the
        /// specified data reader.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="reader" />is
        /// <c>null</c>.</exception>
        public static IEnumerable<IDataRecord> AsEnumerable(this IDataReader reader)
        {
            ADP.CheckArgumentNull(reader, nameof(reader));
            while (reader.Read())
            {
                yield return reader;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="IDataReader"/>
        /// contains one or more rows.
        /// </summary>
        /// <param name="reader">The data reader.</param>
        /// <returns>
        ///     <c>True</c> if the <see cref="IDataReader"/> contains one or
        ///     more rows; otherwise <c>false</c>.
        /// </returns>
        [DebuggerStepThrough]
        public static bool HasRows(this IDataReader reader) => reader.Read();

        /// <summary>
        /// Projects each element of a sequence into a new form.
        /// </summary>
        /// <typeparam name="T">The type of the value returned by
        /// <paramref name="selector"/>.</typeparam>
        /// <param name="reader">A sequence of data reader records to invoke a
        /// transform function on.</param>
        /// <param name="selector">A transform function to apply to each
        /// element.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> whose elements are the
        /// result of invoking the transform function on each element of
        /// <paramref name="reader"/>.</returns>
        public static IEnumerable<T> Select<T>(this IDataReader reader, Func<IDataReader, T> selector)
        {
            while (reader.Read())
            {
                yield return selector(reader);
            }
        }
    }
}