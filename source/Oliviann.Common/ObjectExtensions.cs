namespace Oliviann
{
    #region Usings

    using System;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using Oliviann.IO;
    using Oliviann.Properties;

    #endregion Usings

    /// <summary>
    /// Represents a collection of commonly used extension methods for extending
    /// objects.
    /// </summary>
    public static class ObjectExtensions
    {
        #region Reflection

        /// <summary>
        /// Gets the value of the property specified by <paramref name="name"/>
        /// of the given <paramref name="obj"/>.
        /// </summary>
        /// <param name="obj">The object whose property value will be returned.
        /// </param>
        /// <param name="name">The string containing the name of the property to
        /// get.</param>
        /// <returns>The property value for the <paramref name="obj"/> parameter.
        /// </returns>
        /// <exception cref="MissingMemberException">Error finding the
        /// specified property name. Ensure the specified property name [{0}] is
        /// a member of the current object.</exception>
        public static object GetPropertyValue(this object obj, string name)
        {
            PropertyInfo info = obj.GetType().GetProperty(name);
            if (info == null)
            {
                throw new MissingMemberException(Resources.ERR_MissingPropertyName.FormatWith(name));
            }

            return info.CanRead ? info.GetValue(obj, null) : null;
        }

        /// <summary>
        /// Gets the value of the property specified by <paramref name="name"/>
        /// of the given <paramref name="obj"/>.
        /// </summary>
        /// <typeparam name="T">The type of the object to be returned as.
        /// </typeparam>
        /// <param name="obj">The object whose property value will be returned.
        /// </param>
        /// <param name="name">The string containing the name of the property to
        /// get.</param>
        /// <returns>The property value for the <paramref name="obj"/>parameter.
        /// </returns>
        public static T GetPropertyValue<T>(this object obj, string name) => (T)obj.GetPropertyValue(name);

        #endregion Reflection

        #region Validations

        /// <summary>
        /// Determine whether the specified object equals DBNull;
        /// </summary>
        /// <param name="obj">The object to be checked.</param>
        /// <returns>
        /// True if the specified object equals DBNull; otherwise, false.
        /// </returns>
        public static bool IsDbNull(this object obj) => obj == DBNull.Value;

        /// <summary>
        /// Determines whether the specified object is not null.
        /// </summary>
        /// <param name="obj">The object to be checked.</param>
        /// <returns>
        /// True if the specified object is not null; otherwise, false.
        /// </returns>
#if !NET35 && !NET40

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        [DebuggerStepThrough]
        public static bool IsNotNull(this object obj) => obj != null;

        /// <summary>
        /// Determines whether the specified object is null.
        /// </summary>
        /// <param name="obj">The object to be checked.</param>
        /// <returns>
        /// True if the specified object is null; otherwise, false.
        /// </returns>

#if !NET35 && !NET40

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        [DebuggerStepThrough]
        public static bool IsNull(this object obj) => obj == null;

        /// <summary>
        /// Determines whether the specified expression is numeric.
        /// </summary>
        /// <param name="expression">The expression to be checked.</param>
        /// <returns>
        /// True if the specified expression is numeric; otherwise, false.
        /// </returns>
        public static bool IsNumeric(this object expression)
        {
            if (expression == null)
            {
                return false;
            }

            return double.TryParse(
                                   expression.ToString(),
                                   NumberStyles.Any,
                                   NumberFormatInfo.InvariantInfo,
                                   out _);
        }

        #endregion Validations

        /// <summary>
        /// Gets the list box item display text value.
        /// </summary>
        /// <param name="listBoxItem">The list box item.</param>
        /// <param name="displayMember">The
        /// <see cref="T:System.Windows.Forms.ListBox"/> display member.</param>
        /// <returns>The string value for the
        /// <see cref="M:System.Windows.Forms.ListBox.ObjectCollection"/> item.
        /// </returns>
        public static string ItemText(this object listBoxItem, string displayMember)
        {
            if (listBoxItem == null || displayMember.IsNullOrEmpty())
            {
                return string.Empty;
            }

            return listBoxItem.GetPropertyValue<string>(displayMember);
        }

        #region Converts

        /// <summary>
        /// Converts the specified object <paramref name="value"/> to a
        /// <see cref="DateTime"/> object or the default specified
        /// <see cref="DateTime"/> <paramref name="defaultValue"/>.
        /// </summary>
        /// <param name="value">The date time object.</param>
        /// <param name="defaultValue">The default <see cref="DateTime"/> value
        /// if the specified <paramref name="value"/> fails to be parsed.
        /// </param>
        /// <returns>The converted <see cref="DateTime"/> value if it was able
        /// to be parsed; otherwise, the specified
        /// <paramref name="defaultValue"/>.
        /// </returns>
        public static DateTime ToDateTime(this object value, DateTime defaultValue)
        {
            if (value == null)
            {
                return defaultValue;
            }

            return DateTime.TryParse(value.ToString(), out DateTime converted) ? converted : defaultValue;
        }

        /// <summary>
        /// Converts the specified object value to a
        /// <see cref="Nullable{DateTime}"/> object.
        /// </summary>
        /// <param name="value">An object that implements the
        /// <see cref="IConvertible"/> interface, or a <c>null</c> reference.
        /// </param>
        /// <returns>A <see cref="Nullable{DateTime}"/> object for the specified
        /// string.
        /// </returns>
        public static DateTime? ToDateTime(this object value)
        {
            if (value == null)
            {
                return null;
            }

            if (DateTime.TryParse(value.ToString(), out DateTime converted))
            {
                return converted;
            }

            return null;
        }

        /// <summary>
        /// Converts the specified object value to an exact
        /// <see cref="Nullable{DateTime}"/> object using the specified
        /// <paramref name="format"/>.
        /// </summary>
        /// <param name="value">An object containing a date and time to convert.
        /// </param>
        /// <param name="format">The required format of <paramref name="value"/>
        /// .</param>
        /// <returns>
        /// When this method returns, contains the <see cref="DateTime"/> value
        /// equivalent to the date and time contained in
        /// <paramref name="value"/>, if the conversion succeeded, or
        /// <see cref="DateTime.MinValue"/> if the conversion failed. The
        /// conversion fails if either the <paramref name="value"/> or format
        /// parameter is a <c>null</c> reference, is an empty string, or does
        /// not contain a date and time that correspond to the pattern specified
        /// in format.
        /// </returns>
        public static DateTime? ToDateTimeExact(this object value, string format)
        {
            if (value == null)
            {
                return null;
            }

            if (DateTime.TryParseExact(value.ToString(), format, CultureInfo.CurrentCulture, DateTimeStyles.None, out DateTime converted))
            {
                return converted;
            }

            return null;
        }

        /// <summary>
        /// Converts the value of the specified object to a decimal number.
        /// </summary>
        /// <param name="value">An object that implements the
        /// <see cref="IConvertible"/> interface, or a <c>null</c> reference.
        /// </param>
        /// <param name="defaultValue">The default <see cref="Decimal"/> value
        /// if the specified object <paramref name="value"/> fails to be parsed.
        /// </param>
        /// <returns>The converted <see cref="Decimal"/> value if it was able to
        /// be parsed; otherwise, the specified <paramref name="defaultValue"/>.
        /// </returns>
        public static decimal ToDecimal(this object value, decimal defaultValue = 0)
        {
            return decimal.TryParse(value.ToStringSafe(), out decimal converted) ? converted : defaultValue;
        }

        /// <summary>
        /// Converts the value of the specified object to a nullable decimal
        /// number.
        /// </summary>
        /// <param name="value">An object that implements the
        /// <see cref="IConvertible"/> interface, or a null reference.
        /// </param>
        /// <returns>The converted <see cref="Decimal"/> value if it was able to
        /// be parsed; otherwise, null.
        /// </returns>
        public static decimal? ToDecimalNullable(this object value)
        {
            return decimal.TryParse(value.ToStringSafe(), out decimal converted) ? converted : (decimal?)null;
        }

        /// <summary>
        /// Converts the value of the specified object to a double-precision
        /// floating-point number.
        /// </summary>
        /// <param name="value">An object that implements the
        /// <see cref="IConvertible"/> interface, or a <c>null</c> reference.
        /// </param>
        /// <param name="defaultValue">The default <see cref="Double"/> value if
        /// the specified object <paramref name="value"/> fails to be parsed.
        /// </param>
        /// <returns>
        /// The converted <see cref="Double"/> value if it was able to be
        /// parsed; otherwise, the specified <paramref name="defaultValue"/>
        /// </returns>
        public static double ToDouble(this object value, double defaultValue = 0D)
        {
            return double.TryParse(value.ToStringSafe(), out double converted) ? converted : defaultValue;
        }

        /// <summary>
        /// Converts the value of the specified object to a nullable double
        /// number.
        /// </summary>
        /// <param name="value">An object that implements the
        /// <see cref="IConvertible"/> interface, or a null reference.
        /// </param>
        /// <returns>The converted <see cref="double"/> value if it was able to
        /// be parsed; otherwise, null.
        /// </returns>
        public static double? ToDoubleNullable(this object value)
        {
            return double.TryParse(value.ToStringSafe(), out double converted) ? converted : (double?)null;
        }

        /// <summary>
        /// Converts the value of the specified object to a 16-bit signed
        /// integer.
        /// </summary>
        /// <param name="value">An object that implements the
        /// <see cref="IConvertible"/> interface, or a <c>null</c> reference.
        /// </param>
        /// <param name="defaultValue">The default <see cref="Int16"/> value if
        /// the specified object <paramref name="value"/> fails to be parsed.
        /// </param>
        /// <returns>The converted <see cref="Int16"/> value if it was able to
        /// be parsed; otherwise, the specified <paramref name="defaultValue"/>.
        /// </returns>
        public static short ToInt16(this object value, short defaultValue = 0)
        {
            return short.TryParse(value.ToStringSafe(), out short converted) ? converted : defaultValue;
        }

        /// <summary>
        /// Converts the value of the specified object to a nullable short
        /// number.
        /// </summary>
        /// <param name="value">An object that implements the
        /// <see cref="IConvertible"/> interface, or a null reference.
        /// </param>
        /// <returns>The converted <see cref="short"/> value if it was able to
        /// be parsed; otherwise, null.
        /// </returns>
        public static short? ToInt16Nullable(this object value)
        {
            return short.TryParse(value.ToStringSafe(), out short converted) ? converted : (short?)null;
        }

        /// <summary>
        /// Converts the value of the specified object to a 32-bit signed
        /// integer.
        /// </summary>
        /// <param name="value">An object that implements the
        /// <see cref="IConvertible"/> interface, or a <c>null</c> reference.
        /// </param>
        /// <param name="defaultValue">The default <see cref="Int32"/> value if
        /// the specified object <paramref name="value"/> fails to be parsed.
        /// </param>
        /// <returns>The converted <see cref="Int32"/> value if it was able to
        /// be parsed; otherwise, the specified <paramref name="defaultValue"/>.
        /// </returns>
        public static int ToInt32(this object value, int defaultValue = 0)
        {
            return int.TryParse(value.ToStringSafe(), out int converted) ? converted : defaultValue;
        }

        /// <summary>
        /// Converts the value of the specified object to a nullable int
        /// number.
        /// </summary>
        /// <param name="value">An object that implements the
        /// <see cref="IConvertible"/> interface, or a null reference.
        /// </param>
        /// <returns>The converted <see cref="int"/> value if it was able to
        /// be parsed; otherwise, null.
        /// </returns>
        public static int? ToInt32Nullable(this object value)
        {
            return int.TryParse(value.ToStringSafe(), out int converted) ? converted : (int?)null;
        }

        /// <summary>
        /// Converts the value of the specified object to a 64-bit signed
        /// integer.
        /// </summary>
        /// <param name="value">An object that implements the
        /// <see cref="IConvertible"/> interface, or a <c>null</c> reference.
        /// </param>
        /// <param name="defaultValue">The default <see cref="Int64"/> value if
        /// the specified object <paramref name="value"/> fails to be parsed.
        /// </param>
        /// <returns>The converted <see cref="Int64"/> value if it was able to
        /// be parsed; otherwise, the specified <paramref name="defaultValue"/>.
        /// </returns>
        public static long ToInt64(this object value, long defaultValue = 0L)
        {
            return long.TryParse(value.ToStringSafe(), out long converted) ? converted : defaultValue;
        }

        /// <summary>
        /// Converts the value of the specified object to a nullable long
        /// number.
        /// </summary>
        /// <param name="value">An object that implements the
        /// <see cref="IConvertible"/> interface, or a null reference.
        /// </param>
        /// <returns>The converted <see cref="long"/> value if it was able to
        /// be parsed; otherwise, null.
        /// </returns>
        public static long? ToInt64Nullable(this object value)
        {
            return long.TryParse(value.ToStringSafe(), out long converted) ? converted : (long?)null;
        }

        /// <summary>
        /// Converts the value of the specified object to a single-precision
        /// floating-point number.
        /// </summary>
        /// <param name="value">An object that implements the
        /// <see cref="IConvertible"/> interface, or a <c>null</c> reference.
        /// </param>
        /// <param name="defaultValue">The default <see cref="Single"/> value if
        /// the specified object <paramref name="value"/> fails to be parsed.
        /// </param>
        /// <returns>
        /// The converted <see cref="Single"/> value if it was able to be
        /// parsed; otherwise, the specified <paramref name="defaultValue"/>
        /// </returns>
        public static float ToSingle(this object value, float defaultValue = 0)
        {
            return float.TryParse(value.ToStringSafe(), out float converted) ? converted : defaultValue;
        }

        /// <summary>
        /// Converts the value of the specified object to a nullable single
        /// number.
        /// </summary>
        /// <param name="value">An object that implements the
        /// <see cref="IConvertible"/> interface, or a null reference.
        /// </param>
        /// <returns>The converted <see cref="float"/> value if it was able to
        /// be parsed; otherwise, null.
        /// </returns>
        public static float? ToSingleNullable(this object value)
        {
            return float.TryParse(value.ToStringSafe(), out float converted) ? converted : (float?)null;
        }

        /// <summary>
        /// Returns a string that represents the current object by checking for
        /// null first. If the value is null then an empty string is returned.
        /// </summary>
        /// <param name="value">The value to be converted to string.</param>
        /// <returns>A string that represents the current object.</returns>
        public static string ToStringSafe(this object value) => value?.ToString() ?? string.Empty;

        /// <summary>
        /// Returns a string that represents the current object by checking for
        /// null first and then trimming any leading and trailing whitespace
        /// characters.
        /// </summary>
        /// <param name="value">The value to be converted.</param>
        /// <returns>A string that represents the current object with no leading
        /// or trailing whitespace characters.</returns>
        /// <remarks>Do not abuse the use of this method. It's still doing a few
        /// operations to covert an object to a string and then trim the string.
        /// </remarks>
        public static string ToStringTrim(this object value) => value.ToStringSafe().Trim();

        /// <summary>
        /// Converts the value of the specified object to a 16-bit unsigned
        /// integer.
        /// </summary>
        /// <param name="value">An object that implements the
        /// <see cref="IConvertible"/> interface, or a <c>null</c> reference.
        /// </param>
        /// <param name="defaultValue">The default <see cref="UInt16"/> value if
        /// the specified object <paramref name="value"/> fails to be parsed.
        /// </param>
        /// <returns>The converted <see cref="UInt16"/> value if it was able to
        /// be parsed; otherwise, the specified <paramref name="defaultValue"/>.
        /// </returns>
        public static ushort ToUInt16(this object value, ushort defaultValue = 0)
        {
            return ushort.TryParse(value.ToStringSafe(), out ushort converted) ? converted : defaultValue;
        }

        /// <summary>
        /// Converts the value of the specified object to a nullable 16-bit
        /// unsigned integer.
        /// </summary>
        /// <param name="value">An object that implements the
        /// <see cref="IConvertible"/> interface, or a null reference.
        /// </param>
        /// <returns>The converted <see cref="ushort"/> value if it was able to
        /// be parsed; otherwise, null.
        /// </returns>
        public static ushort? ToUInt16Nullable(this object value)
        {
            return ushort.TryParse(value.ToStringSafe(), out ushort converted) ? converted : (ushort?)null;
        }

        /// <summary>
        /// Converts the value of the specified object to a 32-bit unsigned
        /// integer.
        /// </summary>
        /// <param name="value">An object that implements the
        /// <see cref="IConvertible"/> interface, or a <c>null</c> reference.
        /// </param>
        /// <param name="defaultValue">The default <see cref="UInt32"/> value if
        /// the specified object <paramref name="value"/> fails to be parsed.
        /// </param>
        /// <returns>The converted <see cref="UInt32"/> value if it was able to
        /// be parsed; otherwise, the specified <paramref name="defaultValue"/>.
        /// </returns>
        public static uint ToUInt32(this object value, uint defaultValue = 0U)
        {
            return uint.TryParse(value.ToStringSafe(), out uint converted) ? converted : defaultValue;
        }

        /// <summary>
        /// Converts the value of the specified object to a nullable 32-bit
        /// unsigned integer.
        /// </summary>
        /// <param name="value">An object that implements the
        /// <see cref="IConvertible"/> interface, or a null reference.
        /// </param>
        /// <returns>The converted <see cref="uint"/> value if it was able to
        /// be parsed; otherwise, null.
        /// </returns>
        public static uint? ToUInt32Nullable(this object value)
        {
            return uint.TryParse(value.ToStringSafe(), out uint converted) ? converted : (uint?)null;
        }

        /// <summary>
        /// Converts the value of the specified object to a 64-bit unsigned
        /// integer.
        /// </summary>
        /// <param name="value">An object that implements the
        /// <see cref="IConvertible"/> interface, or a <c>null</c> reference.
        /// </param>
        /// <param name="defaultValue">The default <see cref="UInt64"/> value if
        /// the specified object <paramref name="value"/> fails to be parsed.
        /// </param>
        /// <returns>The converted <see cref="UInt64"/> value if it was able to
        /// be parsed; otherwise, the specified <paramref name="defaultValue"/>.
        /// </returns>
        public static ulong ToUInt64(this object value, ulong defaultValue = 0)
        {
            return ulong.TryParse(value.ToStringSafe(), out ulong converted) ? converted : defaultValue;
        }

        /// <summary>
        /// Converts the value of the specified object to a nullable 64-bit
        /// unsigned integer.
        /// </summary>
        /// <param name="value">An object that implements the
        /// <see cref="IConvertible"/> interface, or a null reference.
        /// </param>
        /// <returns>The converted <see cref="ulong"/> value if it was able to
        /// be parsed; otherwise, null.
        /// </returns>
        public static ulong? ToUInt64Nullable(this object value)
        {
            return ulong.TryParse(value.ToStringSafe(), out ulong converted) ? converted : (ulong?)null;
        }

        /// <summary>
        /// Tries to cast the specified object to the requested type.
        /// </summary>
        /// <typeparam name="T">The type to cast the object to.</typeparam>
        /// <param name="obj">The object to be cast.</param>
        /// <param name="result">The result of the cast.</param>
        /// <returns>True if object could be cast to the specified type;
        /// otherwise, false.</returns>
        public static bool TryCast<T>(this object obj, out T result)
        {
            if (obj is T value)
            {
                result = value;
                return true;
            }

            result = default(T);
            return false;
        }

        #endregion Converts
    }
}