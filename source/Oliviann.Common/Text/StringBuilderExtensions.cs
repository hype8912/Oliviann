namespace Oliviann.Text
{
    #region Usings

    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Text;

    #endregion Usings

    /// <summary>
    /// Represents a collection of commonly used extension methods for extending
    /// string builders.
    /// </summary>
    public static class StringBuilderExtensions
    {
        /// <summary>
        /// Appends the string returned by processing a composite format string,
        /// which contains zero or more format items, to the specified builder
        /// instance. Each format item is replaced by the string representation
        /// of a single argument and then a new line it appended.
        /// </summary>
        /// <param name="builder">The string builder instance.</param>
        /// <param name="format">A string containing zero or more format
        /// specifications.</param>
        /// <param name="arguments">An array of objects to format.</param>
        /// <returns>A reference to this instance with format appended. Any
        /// format specification in format is replaced by the string
        /// representation of the corresponding object argument.</returns>
        [DebuggerStepThrough]
        public static StringBuilder AppendLineFormat(this StringBuilder builder, string format, params object[] arguments)
        {
            return builder.AppendLineFormat(null, format, arguments);
        }

        /// <summary>
        /// Appends the string returned by processing a composite format string,
        /// which contains zero or more format items, to the specified builder
        /// instance. Each format item is replaced by the string representation
        /// of a single argument and then a new line it appended.
        /// </summary>
        /// <param name="builder">The string builder instance.</param>
        /// <param name="provider">An object that supplies culture-specific
        /// formatting information.</param>
        /// <param name="format">A string containing zero or more format
        /// specifications.</param>
        /// <param name="arguments">An array of objects to format.</param>
        /// <returns>
        /// A reference to this instance with format appended. Any format
        /// specification in format is replaced by the string representation of
        /// the corresponding object argument.
        /// </returns>
        [DebuggerStepThrough]
        public static StringBuilder AppendLineFormat(this StringBuilder builder, IFormatProvider provider, string format, params object[] arguments)
        {
            builder.AppendFormat(provider, format, arguments).AppendLine();
            return builder;
        }

        /// <summary>
        /// Indicates whether the specified builder is a null reference or the
        /// length is less than 1 character.
        /// </summary>
        /// <param name="builder">The builder to test.</param>
        /// <returns>
        /// True if the specified builder is null or the length is less than 1
        /// character.
        /// </returns>
        public static bool IsNullOrEmpty(this StringBuilder builder)
        {
            return builder == null || builder.Length < 1;
        }

        /// <summary>
        /// Replaces a collection of matching strings in a given string.
        /// </summary>
        /// <param name="builder">The string builder instance.</param>
        /// <param name="strings">The collection of replacement strings to be
        /// matched.</param>
        /// <returns>A reference to this instance with matched strings replaced.
        /// </returns>
        public static StringBuilder ReplaceAll(this StringBuilder builder, IEnumerable<KeyValuePair<string, string>> strings)
        {
            if (builder == null || strings == null)
            {
                return builder;
            }

            foreach (KeyValuePair<string, string> pair in strings)
            {
                builder.Replace(pair.Key, pair.Value);
            }

            return builder;
        }
    }
}