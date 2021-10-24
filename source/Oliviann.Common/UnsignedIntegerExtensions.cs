namespace Oliviann
{
    #region Usings

    using System.Collections.Generic;
    using System.Text;

    #endregion

    /// <summary>
    /// Represents a collection of commonly used extension methods for extending
    /// unsigned integers.
    /// </summary>
    public static class UnsignedIntegerExtensions
    {
        #region Fields

        /// <summary>
        /// The mapping dictionary of roman numeral values.
        /// </summary>
        /// <remarks>https://en.wiktionary.org/wiki/Appendix:Roman_numerals</remarks>
        private static readonly IDictionary<uint, string> NumberToRomanNumerals =
            new Dictionary<uint, string>
                {
                    { 100000, "ↈ" }, // |I| with overline or C with overline.
                    { 50000, "ↇ" }, // L with overline.
                    { 10000, "ↂ" }, // X with overline
                    { 5000, "ↁ" }, // V with overline
                    { 1000, "M" },
                    { 900, "CM" },
                    { 500, "D" },
                    { 400, "CD" },
                    { 100, "C" },
                    { 90, "XC" },
                    { 50, "L" },
                    { 40, "XL" },
                    { 10, "X" },
                    { 9, "IX" },
                    { 5, "V" },
                    { 4, "IV" },
                    { 1, "I" },
                };

        #endregion

        /// <summary>
        /// Converts the unsigned number to a roman numeral string.
        /// </summary>
        /// <param name="number">The number to be converted.</param>
        /// <returns>A string of roman numeral characters that match the
        /// specified number.</returns>
        /// <remarks>Needs more testing with numbers above 5,000.</remarks>
        public static string ToRomanNumerals(this uint number)
        {
            var result = new StringBuilder();
            foreach (KeyValuePair<uint, string> pair in NumberToRomanNumerals)
            {
                while (number >= pair.Key)
                {
                    result.Append(pair.Value);
                    number -= pair.Key;
                }
            }

            return result.ToString();
        }
    }
}