namespace Oliviann
{
    /// <summary>
    /// Represents a collection of commonly used extension methods for extending
    /// char objects.
    /// </summary>
    public static class CharExtensions
    {
        #region Methods

        /// <summary>
        /// Compare characters using culture-sensitive rules, the invariant
        /// culture, and ignoring the case of the characters being compared.
        /// </summary>
        /// <param name="a">The first character to compare.</param>
        /// <param name="b">The second character to compare.</param>
        /// <returns>True if the value of the a parameter is equal to the value
        /// of the b parameter; otherwise, false.</returns>
        public static bool EqualsInvariantIgnoreCase(this char a, char b) => char.ToUpperInvariant(a) == char.ToUpperInvariant(b);

        /// <summary>
        /// Concatenates a specified separator <see cref="char"/> between each
        /// element of a specified <see cref="string"/> array, yielding a single
        /// concatenated string.
        /// </summary>
        /// <param name="separator">A <see cref="char"/>.</param>
        /// <param name="value">An array of <see cref="string"/>.</param>
        /// <returns>A <see cref="string"/> object consisting of the strings in
        /// <paramref name="value"/> joined by <paramref name="separator"/>. Or,
        /// <see cref="string.Empty"/> if <paramref name="value"/> has no
        /// elements, or separator and all the elements of
        /// <paramref name="value"/> are <see cref="string.Empty"/>.
        /// </returns>
        public static string Join(this char separator, params string[] value) =>
            string.Join(new string(separator, 1), value, 0, value.Length);

        #endregion Methods
    }
}