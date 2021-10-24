namespace Oliviann
{
    #region Usings

    using System;
    using System.Linq;
    using System.Text;

    #endregion Usings

    /// <summary>
    /// Represents a collection of commonly used extension methods for extending
    /// arrays.
    /// </summary>
    public static class ArrayExtensions
    {
        /// <summary>
        /// Delimits all the elements in the specified array.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the array segment.
        /// </typeparam>
        /// <param name="source">The source array.</param>
        /// <returns>A new array segment for the specified source array.
        /// </returns>
        public static ArraySegment<T> AsSegment<T>(this T[] source) => new ArraySegment<T>(source);

        /// <summary>
        /// Delimits the specified range of the elements in the specified array.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the array segment.
        /// </typeparam>
        /// <param name="source">The array containing the range of elements to
        /// delimit.</param>
        /// <param name="offset">The zero-based index of the first element in
        /// the range.</param>
        /// <param name="count">The number of elements in the range.</param>
        /// <returns>A new array segment for the specified source array and
        /// range.
        /// </returns>
        public static ArraySegment<T> AsSegment<T>(this T[] source, int offset, int count) =>
            new ArraySegment<T>(source, offset, count);

        /// <summary>
        /// Determines whether an element is in the array.
        /// </summary>
        /// <typeparam name="T">The type of the elements of the array.
        /// </typeparam>
        /// <param name="source">The one-dimensional, zero-based Array to
        /// search.</param>
        /// <param name="value">The object to locate in array.</param>
        /// <returns>
        ///   <c>True</c> if the item is found in the array; otherwise,
        ///   <c>false</c>.
        /// </returns>
        public static bool Contains<T>(this T[] source, T value) => Array.IndexOf(source, value) > -1;

        /// <summary>
        /// Converts the source array to a unicode string.
        /// </summary>
        /// <param name="source">The source array.</param>
        /// <returns>A string matching the specified source array.</returns>
        public static string ConvertToString(this ushort[] source)
        {
            if (source == null)
            {
                return string.Empty;
            }

            var builder = new StringBuilder(source.Length);
            foreach (ushort value in source)
            {
                builder.Append((char)value);
            }

            return builder.ToString();
        }

        /// <summary>
        /// Determines if a string array contains the specified value string and
        /// then returns the first index the value was found in.
        /// </summary>
        /// <param name="source">The string array being searched.</param>
        /// <param name="value">The string object to seek.</param>
        /// <returns>
        /// The first index value the string is contained in if found;
        /// otherwise, -1.
        /// </returns>
        public static int FirstIndexOf(this string[] source, string value) =>
            source.FirstIndexOf(value, StringComparison.Ordinal);

        /// <summary>
        /// Determines if a string array contains the specified value string and
        /// then returns the first index the value was found in.
        /// </summary>
        /// <param name="source">The string array being searched.</param>
        /// <param name="value">The string object to seek.
        /// </param>
        /// <param name="comparisonType">One of the
        /// <see cref="StringComparison"/> values.</param>
        /// <returns>
        /// The first index value the string is contained in if found;
        /// otherwise, -1.
        /// </returns>
        public static int FirstIndexOf(this string[] source, string value, StringComparison comparisonType)
        {
            if (source == null || source.Length < 1)
            {
                return -1;
            }

            for (int i = 0; i < source.Length; i += 1)
            {
                if (string.Equals(source[i], value, comparisonType))
                {
                    return i;
                }
            }

            return -1;
        }

        #region Converts

        /// <summary>
        /// Converts the specified <paramref name="byteArray"/> to a hexadecimal
        /// string using X.
        /// </summary>
        /// <param name="byteArray">The byte array to be converted.</param>
        /// <returns>A string object representing the specified
        /// <paramref name="byteArray"/>.</returns>
        public static string ToHexString(this byte[] byteArray)
        {
#if NET35
            return string.Join(string.Empty, byteArray.Select(b => b.ToString("X")).ToArray());
#else
            return string.Join(string.Empty, byteArray.Select(b => b.ToString("X")));
#endif
        }

        /// <summary>
        /// Converts the specified <paramref name="byteArray"/> to a hexadecimal
        /// string with a two position value using X2.
        /// </summary>
        /// <param name="byteArray">The byte array to be converted.</param>
        /// <returns>A string object representing the specified
        /// <paramref name="byteArray"/>.</returns>
        public static string ToHexString2(this byte[] byteArray)
        {
#if NET35
            return string.Join(string.Empty, byteArray.Select(b => b.ToString("X2")).ToArray());
#else
            return string.Join(string.Empty, byteArray.Select(b => b.ToString("X2")));
#endif
        }

        /// <summary>
        /// Gets the value of an array element by the index if the array
        /// contains enough elements or returns the optional default value.
        /// </summary>
        /// <typeparam name="T">The type of the element.</typeparam>
        /// <param name="array">The array to retrieve from.</param>
        /// <param name="index">The index of the element to get the value for.
        /// </param>
        /// <param name="defaultValue">The default value encase the array isn't
        /// as long as the index.</param>
        /// <returns>The value contained in the specified array element;
        /// otherwise, the specified default value.</returns>
        /// <remarks>This method tries to prevent you from getting an index of
        /// range error when trying to retrieve a value from an index that
        /// doesn't exist.</remarks>
        public static T ValueOrDefault<T>(this T[] array, uint index, T defaultValue = default(T))
        {
            if (array == null || array.Length == 0 || array.Length < index)
            {
                return defaultValue;
            }

            return array[index];
        }

        #endregion Converts
    }
}