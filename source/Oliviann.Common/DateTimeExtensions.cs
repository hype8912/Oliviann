namespace Oliviann
{
    #region Usings

    using System;
    using System.Diagnostics.CodeAnalysis;

    #endregion Usings

    /// <summary>
    /// Represents a collection of commonly used extension methods for extending
    /// DateTime objects.
    /// </summary>
    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1631:DocumentationMustMeetCharacterPercentage",
        Justification = "Examples show an output example.")]
    public static class DateTimeExtensions
    {
        /// <summary>
        /// Converts the value of the current System.DateTime object to its
        /// equivalent sortable date/time string that conforms to ISO 8601
        /// standards.
        /// </summary>
        /// <param name="date">The System.DateTime object to convert.</param>
        /// <returns>A string that contains the sortable date/time string
        /// representation of the current System.DateTime object.</returns>
        /// <example>
        /// Ex. 2009-02-27T12:12:22
        /// </example>
        public static string ToISO8601String(this DateTime date) => date.ToString("s");

        /// <summary>
        /// Converts the value of the current System.DateTime object to its
        /// equivalent long date short time string.
        /// </summary>
        /// <param name="date">The System.DateTime object to convert.</param>
        /// <returns>A string that contains the long date and short time string
        /// representation of the current System.DateTime object.</returns>
        /// <example>
        /// Friday, February 27, 2009 12:11:22 PM
        /// </example>
        public static string ToLongDateLongTimeString(this DateTime date) => date.ToString("F");

        /// <summary>
        /// Converts the value of the current System.DateTime object to its
        /// equivalent long date long time string.
        /// </summary>
        /// <param name="date">The System.DateTime object to convert.</param>
        /// <returns>A string that contains the long date and long time string
        /// representation of the current System.DateTime object.</returns>
        /// <example>
        /// Friday, February 27, 2009 12:11 PM
        /// </example>
        public static string ToLongDateShortTimeString(this DateTime date) => date.ToString("f");

        /// <summary>
        /// Converts the value of the current System.DateTime object to its
        /// equivalent Month day string.
        /// </summary>
        /// <param name="date">The System.DateTime object to convert.</param>
        /// <returns>A string that contains the month day string representation
        /// of the current System.DateTime object.</returns>
        /// <example>
        /// February 27
        /// </example>
        public static string ToMonthDayString(this DateTime date) => date.ToString("m");

        /// <summary>
        /// Converts the value of the current System.DateTime object to its
        /// equivalent Month year string.
        /// </summary>
        /// <param name="date">The System.DateTime object to convert.</param>
        /// <returns>A string that contains the month year string representation
        /// of the current System.DateTime object.</returns>
        /// <example>
        /// February, 2009
        /// </example>
        public static string ToMonthYearString(this DateTime date) => date.ToString("y");

        /// <summary>
        /// Converts the value of the current System.DateTime object to its
        /// equivalent RFC1123 string.
        /// </summary>
        /// <param name="date">The System.DateTime object to convert.</param>
        /// <returns>A string that contains the RFC1123 string representation of
        /// the current System.DateTime object.</returns>
        /// <example>
        /// Fri, 27 Feb 2009 12:12:22 GMT
        /// </example>
        public static string ToRFC1123String(this DateTime date) => date.ToString("r");

        /// <summary>
        /// Converts the value of the current System.DateTime object to its
        /// equivalent round-trip date time string.
        /// </summary>
        /// <param name="date">The System.DateTime object to convert.</param>
        /// <returns>A string that contains the round-trip date time string
        /// representation of the current System.DateTime object.</returns>
        /// <example>
        /// Ex. 2009-02-27T12:11:22.0000000
        /// </example>
        public static string ToRoundTripString(this DateTime date) => date.ToString("o");

        /// <summary>
        /// Converts the value of the current System.DateTime object to its
        /// equivalent universal sortable long date time string.
        /// </summary>
        /// <param name="date">The System.DateTime object to convert.</param>
        /// <returns>A string that contains the universal sortable long date
        /// time string representation of the current System.DateTime object.
        /// </returns>
        /// <example>
        /// Friday, February 27, 2009 8:12:22 PM
        /// </example>
        public static string ToUniversalDateTimeLongString(this DateTime date) => date.ToString("U");

        /// <summary>
        /// Converts the value of the current System.DateTime object to its
        /// equivalent universal sortable short date time string.
        /// </summary>
        /// <param name="date">The System.DateTime object to convert.</param>
        /// <returns>A string that contains the universal sortable short date
        /// time string representation of the current System.DateTime object.
        /// </returns>
        /// <example>
        /// 2009-02-27 12:12:22Z
        /// </example>
        public static string ToUniversalDateTimeShortString(this DateTime date) => date.ToString("u");
    }
}