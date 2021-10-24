namespace Oliviann
{
    /// <summary>
    /// Represents a collection of commonly used extension methods for extending
    /// integers.
    /// </summary>
    public static class IntegerExtensions
    {
        /// <summary>
        /// Determines if the specified number is a valid port and then either
        /// returns the current number if the value is valid or the default
        /// value.
        /// </summary>
        /// <param name="number">The port number to evaluate.</param>
        /// <param name="defaultValue">The default port value to be returned if
        /// the specified number is not a valid port number.</param>
        /// <returns>If the specified number is a valid port number then the
        /// specified number will be returned; otherwise, the specified default
        /// value is returned unless the default value is not a valid port where
        /// 0 will be returned in this instance.</returns>
        public static int ValidPortOrDefault(this int number, int defaultValue = 80)
        {
            if (number.IsValidPort())
            {
                return number;
            }

            return defaultValue.IsValidPort() ? defaultValue : 0;
        }

        #region Validations

        /// <summary>
        /// Determines whether the specified number is a valid port number.
        /// </summary>
        /// <param name="number">The number to evaluate.</param>
        /// <returns>
        ///   <c>True</c> if the specified number is a valid port number;
        ///   otherwise, <c>false</c>.
        /// </returns>
        public static bool IsValidPort(this int number) => number.IsBetween(-1, 65536);

        /// <summary>
        /// Determines whether the specified number is even.
        /// </summary>
        /// <param name="number">The number to evaluate.</param>
        /// <returns>
        /// true if the specified number is even; otherwise, false.
        /// </returns>
        public static bool IsEven(this int number) => number % 2 == 0;

        /// <summary>
        /// Determines whether the specified number is odd.
        /// </summary>
        /// <param name="number">The number to evaluate.</param>
        /// <returns>
        /// true if the specified number is odd; otherwise, false.
        /// </returns>
        public static bool IsOdd(this int number) => !IsEven(number);

        /// <summary>
        /// Determines whether the specified number is greater than the
        /// specified value.
        /// </summary>
        /// <param name="number">The number to be compared.</param>
        /// <param name="value">The value to be compared.</param>
        /// <returns>
        ///   <c>True</c> if the specified number is greater than; otherwise,
        ///   <c>false</c>.
        /// </returns>
        public static bool IsGreaterThan(this int number, int value) => number > value;

        /// <summary>
        /// Determines whether the specified number is less than the
        /// specified value.
        /// </summary>
        /// <param name="number">The number to be compared.</param>
        /// <param name="value">The value to be compared.</param>
        /// <returns>
        ///   <c>True</c> if the specified number is less than; otherwise,
        ///   <c>false</c>.
        /// </returns>
        public static bool IsLessThan(this int number, int value) => number < value;

        /// <summary>
        /// Determines whether the specified number is greater than or equal to
        /// the specified value.
        /// </summary>
        /// <param name="number">The number to be compared.</param>
        /// <param name="value">The value to be compared.</param>
        /// <returns>
        ///   <c>True</c> if the specified number is greater than or equal to;
        ///   otherwise,
        ///   <c>false</c>.
        /// </returns>
        public static bool IsGreaterThanOrEqual(this int number, int value) => number >= value;

        /// <summary>
        /// Determines whether the specified number is less than or equal to
        /// the specified value.
        /// </summary>
        /// <param name="number">The number to be compared.</param>
        /// <param name="value">The value to be compared.</param>
        /// <returns>
        ///   <c>True</c> if the specified number is less than or equal to;
        ///   otherwise,
        ///   <c>false</c>.
        /// </returns>
        public static bool IsLessThanOrEqual(this int number, int value) => number <= value;

        /// <summary>
        /// Determines whether the specified number is greater than the lower
        /// bound value and less than the upper bound value.
        /// </summary>
        /// <param name="number">The number to be compared.</param>
        /// <param name="lowerValue">The lower bound value.</param>
        /// <param name="upperValue">The upper bound value.</param>
        /// <returns>
        ///   <c>True</c> if the specified number is between the specified
        ///   values; otherwise,
        /// <c>false</c>.
        /// </returns>
        public static bool IsBetween(this int number, int lowerValue, int upperValue)
            => number.IsGreaterThan(lowerValue) && number.IsLessThan(upperValue);

        /// <summary>
        /// Determines whether the specified number is greater than or equal to
        /// the lower bound value and less than or equal to the upper bound
        /// value.
        /// </summary>
        /// <param name="number">The number to be compared.</param>
        /// <param name="lowerValue">The lower bound value.</param>
        /// <param name="upperValue">The upper bound value.</param>
        /// <returns>
        ///   <c>True</c> if the specified number is between the specified values;
        ///   otherwise,
        /// <c>false</c>.
        /// </returns>
        public static bool IsBetweenOrEqual(this int number, int lowerValue, int upperValue)
            => number.IsGreaterThanOrEqual(lowerValue) && number.IsLessThanOrEqual(upperValue);

        #endregion Validations

        #region Converts

        /// <summary>
        /// Converts the value of the specified integer value to a 8-bit
        /// unsigned byte.
        /// </summary>
        /// <param name="value">An integer value.</param>
        /// <param name="defaultValue">The default <see cref="byte"/> value if
        /// the specified integer <paramref name="value"/> fails to be parsed.
        /// </param>
        /// <returns>The converted <see cref="byte"/> value if it was able to be
        /// parsed; otherwise, the specified <paramref name="defaultValue"/>.
        /// </returns>
        public static byte ToByte(this int value, byte defaultValue = default(byte))
            => byte.TryParse(value.ToString(), out byte converted) ? converted : defaultValue;

        #endregion Converts
    }
}