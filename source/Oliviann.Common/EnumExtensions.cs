namespace Oliviann
{
    #region Usings

    using System;
    using System.ComponentModel;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;
    using Oliviann.ComponentModel;
    using Oliviann.Reflection;

    #endregion Usings

    /// <summary>
    /// Represents a collection of commonly used extension methods for extending
    /// enumerator objects.
    /// </summary>
    public static class EnumExtensions
    {
        /// <summary>
        /// Gets the description attribute for an enumeration field.
        /// </summary>
        /// <param name="value">The enumerator value.</param>
        /// <returns>The description string if available; otherwise, it returns
        /// an empty string.</returns>
        public static string GetDescriptionAttribute(this Enum value) =>
            value.GetAttribute<DescriptionAttribute>()?.Description ?? string.Empty;

        /// <summary>
        /// Gets the first specified attribute for an enumeration field.
        /// </summary>
        /// <typeparam name="T">The type of <see cref="Attribute"/> to retrieve
        /// for the specified enumerator.</typeparam>
        /// <param name="value">The enumerator value.</param>
        /// <returns>The first specified attribute for the enum field if found;
        /// otherwise, null.</returns>
        public static T GetAttribute<T>(this Enum value) where T : Attribute => value.GetAttributes<T>().FirstOrDefault();

        /// <summary>
        /// Gets all the specified attribute instances for an enumeration field.
        /// </summary>
        /// <typeparam name="T">>The type of <see cref="Attribute"/> to retrieve
        /// for the specified enumerator.</typeparam>
        /// <param name="value">The enumerator value.</param>
        /// <returns>An array of attribute objects for the specified enum field.
        /// </returns>
        public static T[] GetAttributes<T>(this Enum value) where T : Attribute =>
            value.GetType().GetField(value.ToString()).GetCustomAttributesCached(typeof(T), false).OfType<T>().ToArray();

        /// <summary>
        /// Gets the specified attribute property value for an enumeration
        /// field.
        /// </summary>
        /// <typeparam name="T">The type of <see cref="Attribute"/> to retrieve
        /// for the specified enumerator.</typeparam>
        /// <param name="value">The enumerator value.</param>
        /// <param name="propertyName">Name of the property for retrieving the
        /// attribute text.</param>
        /// <returns>The text value for the specified attribute of the
        /// enumerator field; otherwise, an empty string.</returns>
        public static string GetAttributeValue<T>(this Enum value, string propertyName) where T : Attribute
        {
            T enumAttribute = value.GetAttribute<T>();
            PropertyInfo property = enumAttribute?.GetType().GetProperty(propertyName);
            return property == null ? string.Empty : property.GetValue(enumAttribute).ToStringSafe();
        }

        /// <summary>
        /// Determines whether an enumeration bit shifted is within the
        /// specified range.
        /// </summary>
        /// <param name="enumValue">The enumeration value.</param>
        /// <param name="numBitsToShift">The number bits to shift.</param>
        /// <param name="minValueAfterShift">The minimum range value after the
        /// shift.</param>
        /// <param name="maxValueAfterShift">The maximum range value after the
        /// shift.</param>
        /// <returns>
        /// true if the bit shifted enum it within the specified range;
        /// otherwise, false.
        /// </returns>
        public static bool IsWithinShiftedRange(
            this Enum enumValue,
            int numBitsToShift,
            int minValueAfterShift,
            int maxValueAfterShift)
        {
            int num = Convert.ToInt32(enumValue, CultureInfo.InvariantCulture);
            int num2 = num >> numBitsToShift;

            // Leave split into double if statements because the code coverage
            // analyzer can't understand complicated if conditions.
            if (num2 << numBitsToShift == num)
            {
                if (num2.IsBetweenOrEqual(minValueAfterShift, maxValueAfterShift))
                {
                    return true;
                }
            }

            return false;
        }
    }
}