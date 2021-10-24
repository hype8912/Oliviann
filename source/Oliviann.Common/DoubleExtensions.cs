namespace Oliviann
{
    #region Usings

    using System;

    #endregion Usings

    /// <summary>
    /// Represents a collection of commonly used extension methods for extending
    /// double.
    /// </summary>
    public static class DoubleExtensions
    {
        /// <summary>
        /// Converts a JAVA time stamp to a UTC date time.
        /// </summary>
        /// <param name="javaTimeStamp">The JAVA time stamp.</param>
        /// <returns>A new date time instance that represents the specified JAVA
        /// time stamp.</returns>
        public static DateTime ConvertJavaTimeStampToDateTime(this double javaTimeStamp)
            => new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(javaTimeStamp);

        /// <summary>
        /// Converts a UNIX time stamp to a UTC date time.
        /// </summary>
        /// <param name="unixTimeStamp">The UNIX time stamp.</param>
        /// <returns>A new date time instance that represents the specified UNIX
        /// time stamp.</returns>
        public static DateTime ConvertUnixTimeStampToDateTime(this double unixTimeStamp)
            => new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(unixTimeStamp);

        /// <summary>
        /// Determines if the value is a whole number.
        /// </summary>
        /// <param name="value">The value to be checked.</param>
        /// <returns>
        /// True if the specified value is a whole number; otherwise, false.
        /// </returns>
        public static bool IsWholeNumber(this double value) => Math.Abs(value % 1) <= double.Epsilon * 100;

        /// <summary>
        /// Converts a Fahrenheit temperature value to a Celsius value.
        /// </summary>
        /// <param name="fahrenheitTemperature">The Fahrenheit temperature
        /// value.</param>
        /// <returns>A Celsius temperature value.</returns>
        public static double ToCelsius(this double fahrenheitTemperature) => 5.0 / 9.0 * (fahrenheitTemperature - 32);

        /// <summary>
        /// Converts a Celsius temperature value to a Fahrenheit value.
        /// </summary>
        /// <param name="celsiusTemperature">The Celsius temperature
        /// value.</param>
        /// <returns>A Fahrenheit temperature value.</returns>
        public static double ToFahrenheit(this double celsiusTemperature) => ((9.0 / 5.0) * celsiusTemperature) + 32;
    }
}