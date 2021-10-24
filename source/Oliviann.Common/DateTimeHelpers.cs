namespace Oliviann
{
    #region Usings

    using System;
    using System.Globalization;

    #endregion

    /// <summary>
    /// Represents a collection of commonly used helper methods for making
    /// working with <see cref="DateTime"/> easier.
    /// </summary>
    public static class DateTimeHelpers
    {
        #region Methods

        /// <summary>
        /// Determines whether the specified date time string matches the
        /// expected format.
        /// </summary>
        /// <param name="date">The date time to be validated.</param>
        /// <param name="expectedFormat">The expected format of the date time
        /// value.</param>
        /// <returns>
        /// True if the specified date value matches the specified format.
        /// </returns>
        public static bool IsValidFormat(string date, string expectedFormat)
        {
            return DateTime.TryParseExact(
                date,
                expectedFormat,
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out DateTime _);
        }

        #endregion
    }
}