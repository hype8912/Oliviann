namespace Oliviann
{
    #region Usings

    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
#if NETSTANDARD1_3
    using Oliviann.Web;
#else
    using System.Web;
#endif
    using Oliviann.Properties;
    using Oliviann.Text;

    #endregion Usings

    /// <summary>
    /// Represents a collection of commonly used extension methods for extending
    /// strings.
    /// </summary>
    public static class StringExtensions
    {
        #region Cleaners

        /// <summary>
        /// Returns a string containing a specified number of characters from
        /// the left side of a string.
        /// </summary>
        /// <param name="text">String from which the leftmost characters are
        /// returned.</param>
        /// <param name="length">The number of characters from the left to be
        /// returned.</param>
        /// <returns>A string containing a specified number of characters from
        /// the left side of a string.</returns>
        public static string Left(this string text, int length)
        {
            if (text == null || length < 1)
            {
                return string.Empty;
            }

            return text.Substring(length);
        }

        /// <summary>
        /// Returns a string that contains all the characters starting from a
        /// specified position in a string.
        /// </summary>
        /// <param name="text">String from which characters are returned.
        /// </param>
        /// <param name="startingIndex">Starting position of the characters to
        /// return.</param>
        /// <returns>Returns a string containing a specified number of
        /// characters from a string.</returns>
        public static string Mid(this string text, int startingIndex)
        {
            if (text == null || startingIndex > text.Length)
            {
                return string.Empty;
            }

            return startingIndex < 1 ? text : text.Substring(startingIndex);
        }

        /// <summary>
        /// Retrieves a substring from this instance. The substring starts at
        /// the last character from the right and has the specified length.
        /// Basically it returns the last number of characters of a string.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="length">The number of characters in the substring to be
        /// retrieved.
        /// </param>
        /// <returns>A string that is equivalent to the substring length that
        /// begins at the last index in this instance from the right.</returns>
        public static string Right(this string text, int length)
        {
            if (text == null || length < 1)
            {
                return string.Empty;
            }

            return text.Substring(text.Length - Math.Min(length, text.Length));
        }

        /// <summary>
        /// Replaces a collection of old strings with a single new string in a
        /// base string.
        /// </summary>
        /// <param name="text">The base text to search.</param>
        /// <param name="oldStrings">The collection of old strings to be
        /// replaced.</param>
        /// <param name="newString">The new string to replace all the old
        /// strings with.</param>
        /// <returns>A new string with </returns>
        public static string ReplaceAll(this string text, string[] oldStrings, string newString)
        {
            if (oldStrings == null || oldStrings.Length == 0)
            {
                return text;
            }

            return text.ReplaceAll(oldStrings.ToDictionary(str => str, str => newString));
        }

        /// <summary>
        /// Replaces a collection of matching strings in a given string.
        /// </summary>
        /// <param name="text">The base text to search.</param>
        /// <param name="strings">The collection of replacement strings to be
        /// matched.</param>
        /// <returns>A new string with the matching replacement strings.
        /// </returns>
        public static string ReplaceAll(this string text, IEnumerable<KeyValuePair<string, string>> strings)
        {
            if (text.IsNullOrEmpty())
            {
                return text;
            }

            var builder = new StringBuilder(text);
            return builder.ReplaceAll(strings).ToString();
        }

        /// <summary>
        /// Removes the first character if search string starts with the
        /// specified character value.
        /// </summary>
        /// <param name="text">The text data.</param>
        /// <param name="value">String character to look for.</param>
        /// <returns>A string with the first character removed if it matched;
        /// otherwise, the original text string.</returns>
        public static string RemoveFirstChar(this string text, char value) => text.StartsWith(value) ? text.Substring(1) : text;

        /// <summary>
        /// Removes the last character if search string ends with the specified
        /// character value.
        /// </summary>
        /// <param name="text">The text data.</param>
        /// <param name="value">String character to look for.</param>
        /// <returns>A string with the last character removed if it matched;
        /// otherwise, the original text string.</returns>
        public static string RemoveLastChar(this string text, char value) =>
            text.EndsWith(value) ? text.Remove(text.Length - 1, 1) : text;

        /// <summary>
        /// Removes all the space characters from the specified string.
        /// </summary>
        /// <param name="text">The text string to remove spaces.</param>
        /// <returns>A string with all the space characters removed.</returns>
        public static string RemoveSpaces(this string text) => RemoveChar(text, ' ');

        /// <summary>
        /// Removes all instances of the character in the specified string.
        /// </summary>
        /// <param name="text">The text string to remove character.</param>
        /// <param name="character">The character to be removed.</param>
        /// <returns>A new string with all the specified characters removed.
        /// </returns>
        public static unsafe string RemoveChar(this string text, char character)
        {
            if (text == null)
            {
                return null;
            }

            int len = text.Length;
            fixed (char* pStr = text)
            {
                int dstIdx = 0;
                for (int i = 0; i < len; i++)
                {
                    if (pStr[i] != character)
                    {
                        pStr[dstIdx++] = pStr[i];
                    }
                }

                return new string(pStr, 0, dstIdx);
            }
        }

        /// <summary>
        /// Removes all leading and trailing white-space characters from the
        /// current System.String object by determining if the string object is
        /// null or empty first.
        /// </summary>
        /// <param name="text">The text data.</param>
        /// <returns>
        /// The string that remains after all white-space characters are removed
        /// from the start and end of the current string.
        /// </returns>
        public static string TrimSafe(this string text) => !string.IsNullOrEmpty(text) ? text.Trim() : text;

        /// <summary>
        /// Truncates the specified text if it is greater than the specified
        /// trim length and adds '...' to the end of the string. If the
        /// specified text was less than the specified trim length then the
        /// original string is returned.
        /// </summary>
        /// <param name="text">The text to be truncated.</param>
        /// <param name="trimLength">The length to truncate the string.</param>
        /// <param name="suffix">Optional. The suffix string to be added at the
        /// end if the text was truncated. Default value is '...'.</param>
        /// <returns>
        /// Null if the specified text is null. The original specified text if
        /// the trim length is greater than the length of the specified text or
        /// the trim length is 0; otherwise, the truncated string with the
        /// suffix appended to the end.
        /// </returns>
        public static string Truncate(this string text, int trimLength, string suffix = "...")
        {
            if (text == null)
            {
                return null;
            }

            if (trimLength < 0 || text.Length <= trimLength)
            {
                return text;
            }

            if (suffix == null)
            {
                suffix = "...";
            }

            return text.Substring(0, trimLength) + suffix;
        }

        #region Path Separators

        /// <summary>
        /// Looks at the specified <paramref name="path"/> for forward slashes
        /// (/) and adds a URI path separator if necessary; otherwise, a folder
        /// path separator will be added if necessary.
        /// </summary>
        /// <param name="path">The path to search.</param>
        /// <returns>
        /// A path ending with a URI path separator if any are contained in the
        /// specified <paramref name="path"/>; otherwise, a path ending with a
        /// folder path separator.
        /// </returns>
        /// <remarks>Will not add a path separator if the path already ends with
        /// a path separator.</remarks>
        public static string AddPathSeparator(this string path) =>
            path != null && path.Contains('/') ? path.AddUriPathSeparator() : path.AddFolderPathSeparator();

        /// <summary>
        /// Adds the path separator if string is not empty.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>Empty string if path is empty; otherwise, a path ending
        /// with a path separator.</returns>
        public static string AddPathSeparatorIfNotEmpty(this string path) =>
            path == string.Empty ? string.Empty : path.AddPathSeparator();

        /// <summary>
        /// Adds the ending path separator, by calling
        /// <see cref="Path.DirectorySeparatorChar"/>, to the end of the
        /// specified folder path if one does not exist.
        /// </summary>
        /// <param name="folderPath">The folder path.</param>
        /// <returns>A folder path with an ending folder path separator.
        /// </returns>
        public static string AddFolderPathSeparator(this string folderPath) =>
            folderPath.EndsWith(Path.DirectorySeparatorChar) ? folderPath : folderPath + Path.DirectorySeparatorChar;

        /// <summary>
        /// Adds the ending path separator to the end of the specified URI
        /// path if one does not exist.
        /// </summary>
        /// <param name="uriPath">The URI path.</param>
        /// <returns>A URI path with an ending URI separator.</returns>
        public static string AddUriPathSeparator(this string uriPath) =>
            uriPath.EndsWith(Path.AltDirectorySeparatorChar) ? uriPath : uriPath + Path.AltDirectorySeparatorChar;

        #endregion Path Separators

        /// <summary>
        /// Upper cases the first character of the specified string.
        /// </summary>
        /// <param name="text">The text data.</param>
        /// <returns>The same input string with the first character upper case.
        /// </returns>
        public static string UppercaseFirstChar(this string text) =>
            text.IsNullOrEmpty() ? string.Empty : char.ToUpper(text[0]) + text.Substring(1);

#if !NETSTANDARD1_3

        /// <summary>
        /// Converts the specified string to title case (except for words that
        /// are entirely in uppercase, which are considered to be acronyms)
        /// using the current culture info.
        /// </summary>
        /// <param name="text">The string to convert to title case.</param>
        /// <returns>The specified string converted to title case.</returns>
        public static string ToTitleCase(this string text) =>
            text.IsNullOrEmpty() ? text : CultureInfo.CurrentCulture.TextInfo.ToTitleCase(text);

#endif

        /// <summary>
        /// Returns a copy of this <see cref="string"/> object converted to
        /// uppercase using the casing rules of the invariant culture.
        /// </summary>
        /// <param name="text">The text to be converted.</param>
        /// <returns>A <see cref="string"/> object in uppercase if not null;
        /// otherwise, an empty string.</returns>
        public static string ToUpperInvariantSafe(this string text) => text == null ? string.Empty : text.ToUpperInvariant();

        /// <summary>
        /// Reverses the specified text string.
        /// </summary>
        /// <param name="text">The text to be reversed.</param>
        /// <returns>Returns the same object if null or empty; otherwise, a new
        /// reverse string.</returns>
        public static unsafe string Reverse(this string text)
        {
            if (text.IsNullOrEmpty())
            {
                return text;
            }

            string newStr = text;
            int lowerIndex = 0;
            int upperIndex = newStr.Length - 1;

            fixed (char* pStr = newStr)
            {
                while (lowerIndex < upperIndex)
                {
                    char temp = pStr[upperIndex];

                    pStr[upperIndex--] = pStr[lowerIndex];
                    pStr[lowerIndex++] = temp;
                }
            }

            return newStr;
        }

        /// <summary>
        /// Returns a <see cref="T:System.String" /> array that contains the
        /// substrings in this string that are delimited by elements of a
        /// specified <see cref="T:System.Char" /> array.
        /// </summary>
        /// <param name="text">The text data.</param>
        /// <param name="options">Specify
        /// <see cref="F:System.StringSplitOptions.RemoveEmptyEntries"></see> to
        /// omit empty array elements from the array returned, or
        /// <see cref="F:System.StringSplitOptions.None"></see> to include empty
        /// array elements in the array returned.</param>
        /// <param name="separator">An array of Unicode characters that delimit
        /// the substrings in this instance, an empty array containing no
        /// delimiters, or <c>null</c>. </param>
        /// <returns>Returns a string array that contains the substrings in this
        /// string that are delimited by elements of a specified string array. A
        /// parameter specifies whether to return empty array elements.
        /// </returns>
        public static string[] Split(this string text, StringSplitOptions options, params char[] separator) =>
            text.Split(separator, options);

        #endregion Cleaners

        #region Validations

        #region Contains

        /// <summary>
        /// Returns a value indicating whether the specified char object occurs
        /// within this string.
        /// </summary>
        /// <param name="text">The text data.</param>
        /// <param name="value">The char to seek.</param>
        /// <returns>
        ///     <c>True</c> if the value parameter occurs within this string, or
        ///     if value is the empty string (""); otherwise, <c>false</c>.
        /// </returns>
        /// <remarks>
        /// This method performs an ordinal (case-sensitive and
        /// culture-insensitive) comparison.  The search begins at the first
        /// character position of this string and continues through  the last
        /// character position.
        /// </remarks>
        [DebuggerStepThrough]
        public static bool Contains(this string text, char value) => text.IndexOf(value) >= 0;

        /// <summary>
        /// Returns a value indicating whether the specified string object
        /// occurs within this string.
        /// </summary>
        /// <param name="text">The text data.</param>
        /// <param name="value">The value to seek.</param>
        /// <param name="ignoreCase">True to ignore case when comparing this
        /// string and <paramref name="value"/>; otherwise, <c>false</c>.
        /// Default value is false.</param>
        /// <returns>
        /// True if the value parameter occurs within this string, or if value
        /// is the empty string (""); otherwise, false.
        /// </returns>
        [DebuggerStepThrough]
        public static bool Contains(this string text, string value, bool ignoreCase = false)
        {
            return text.Contains(value, ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal);
        }

        /// <summary>
        /// Returns a value indicating whether the specified string object
        /// occurs within this string.
        /// </summary>
        /// <param name="text">The text data.</param>
        /// <param name="value">The string to seek.</param>
        /// <param name="comparisonType">One of the enumeration values that
        /// specifies the rules for the search.</param>
        /// <returns>True if the value parameter occurs within this string, or
        /// if value is the empty string (""); otherwise, false.</returns>
        [DebuggerStepThrough]
        public static bool Contains(this string text, string value, StringComparison comparisonType)
            => text.IndexOf(value, 0, comparisonType) >= 0;

        /// <summary>
        /// Returns a value indicating whether any of the specified chars object
        /// occurs within this string.
        /// </summary>
        /// <param name="text">The text data.</param>
        /// <param name="values">The chars to seek.</param>
        /// <returns>
        /// True if any of the value parameter occurs within this string, or if
        /// value is the empty string (""); otherwise, false.
        /// </returns>
        /// <remarks>
        /// This method performs an ordinal (case-sensitive and
        /// culture-insensitive) comparison. The search begins at the first
        /// character position of this string and continues through the last
        /// character position.
        /// </remarks>
        public static bool ContainsAny(this string text, params char[] values) => values.Any(text.Contains);

        /// <summary>
        /// Returns a value indicating whether any of the the specified string
        /// object occurs within this string.
        /// </summary>
        /// <param name="text">The text data.</param>
        /// <param name="values">The values to seek.</param>
        /// <returns>
        /// True if any values parameters occurs within this string, or if value
        /// is the empty string (""); otherwise, false.
        /// </returns>
        public static bool ContainsAny(this string text, params string[] values) => text.ContainsAny(false, values);

        /// <summary>
        /// Returns a value indicating whether any of the the specified string
        /// object occurs within this string.
        /// </summary>
        /// <param name="text">The text data.</param>
        /// <param name="ignoreCase">If set to <c>true</c> [ignore case].
        /// </param>
        /// <param name="values">The values to seek.</param>
        /// <returns>
        /// True if any values parameters occurs within this string, or if value
        /// is the empty string (""); otherwise, false.
        /// </returns>
        public static bool ContainsAny(this string text, bool ignoreCase, params string[] values)
            => values.Any(s => text.Contains(s, ignoreCase));

        /// <summary>
        /// Returns a value indicating whether any of the the specified string
        /// object occurs within this string.
        /// </summary>
        /// <param name="text">The text data.</param>
        /// <param name="ignoreCase">Ignore case if set to true.
        /// </param>
        /// <param name="values">The values to seek.</param>
        /// <returns>
        /// True if any values parameters occurs within this string, or if value
        /// is the empty string (""); otherwise, false.
        /// </returns>
        public static bool ContainsAny(this string text, bool ignoreCase, IEnumerable<string> values)
            => values.Any(s => text.Contains(s, ignoreCase));

        /// <summary>
        /// Returns a value indicating whether all of the specified chars object
        /// occurs within this string.
        /// </summary>
        /// <param name="text">The text data.</param>
        /// <param name="values">The chars to seek.</param>
        /// <returns>
        /// True if the value parameter occurs within this string, or if value
        /// is the empty string (""); otherwise, false.
        /// </returns>
        /// <remarks>
        /// This method performs an ordinal (case-sensitive and
        /// culture-insensitive) comparison. The search begins at the first
        /// character position of this string and continues through the last
        /// character position.
        /// </remarks>
        public static bool ContainsAll(this string text, params char[] values) => values.All(text.Contains);

        /// <summary>
        /// Returns a value indicating whether all of the the specified string
        /// object occurs within this string.
        /// </summary>
        /// <param name="text">The text data.</param>
        /// <param name="values">The values to seek.</param>
        /// <returns>
        /// True if all values parameters occurs within this string, or if value
        /// is the empty string (""); otherwise, false.
        /// </returns>
        public static bool ContainsAll(this string text, params string[] values) => text.ContainsAll(false, values);

        /// <summary>
        /// Returns a value indicating whether all of the the specified string
        /// object occurs within this string.
        /// </summary>
        /// <param name="text">The text data.</param>
        /// <param name="ignoreCase">If set to true, ignore character case.
        /// </param>
        /// <param name="values">The values to seek.</param>
        /// <returns>
        /// True if all values parameters occurs within this string, or if value
        /// is the empty string (""); otherwise, false.
        /// </returns>
        public static bool ContainsAll(this string text, bool ignoreCase, params string[] values)
            => values.All(s => text.Contains(s, ignoreCase));

        #endregion Contains

        #region EndsWith

        /// <summary>
        /// Determines whether the ending of this string matches the specified
        /// char when compared.
        /// </summary>
        /// <param name="text">The text data.</param>
        /// <param name="value">The char to seek.</param>
        /// <param name="ignoreCase">Value indicating to ignore character
        /// casing.</param>
        /// <returns>
        /// <c>True</c> if the value parameter matches the ending of this
        /// string; otherwise, <c>false</c>.
        /// </returns>
        public static bool EndsWith(this string text, char value, bool ignoreCase = false)
        {
            if (text.IsNullOrEmpty())
            {
                return false;
            }

            int index = text.Length - 1;
            char lastChar = text[index];
            if (!ignoreCase)
            {
                return lastChar == value;
            }

            return lastChar == char.ToLowerInvariant(value) || lastChar == char.ToUpperInvariant(value);
        }

        #endregion EndsWith

        #region Equals

        /// <summary>
        /// Compare strings using culture-sensitive sort rules, the current
        /// culture, and ignoring the case of the strings being compared.
        /// </summary>
        /// <param name="a">The first string to compare, or null.</param>
        /// <param name="b">The second string to compare, or null.</param>
        /// <returns>True if the value of the a parameter is equal to the value
        /// of the b parameter; otherwise, false.</returns>
        public static bool EqualsCurrentCultureIgnoreCase(this string a, string b)
            => string.Equals(a, b, StringComparison.CurrentCultureIgnoreCase);

#if !NETSTANDARD1_3

        /// <summary>
        /// Compare strings using culture-sensitive sort rules, the invariant
        /// culture, of the strings being compared.
        /// </summary>
        /// <param name="a">The first string to compare, or null.</param>
        /// <param name="b">The second string to compare, or null.</param>
        /// <returns>True if the value of the a parameter is equal to the value
        /// of the b parameter; otherwise, false.</returns>
        public static bool EqualsInvariant(this string a, string b)
            => string.Equals(a, b, StringComparison.InvariantCulture);

        /// <summary>
        /// Compare strings using culture-sensitive sort rules, the invariant
        /// culture, and ignoring the case of the strings being compared.
        /// </summary>
        /// <param name="a">The first string to compare, or null.</param>
        /// <param name="b">The second string to compare, or null.</param>
        /// <returns>True if the value of the a parameter is equal to the value
        /// of the b parameter; otherwise, false.</returns>
        public static bool EqualsInvariantIgnoreCase(this string a, string b)
            => string.Equals(a, b, StringComparison.InvariantCultureIgnoreCase);

#endif

        /// <summary>
        /// Compare strings using ordinal sort rules of the strings being
        /// compared.
        /// </summary>
        /// <param name="a">The first string to compare, or null.</param>
        /// <param name="b">The second string to compare, or null.</param>
        /// <returns>True if the value of the a parameter is equal to the value
        /// of the b parameter; otherwise, false.</returns>
        public static bool EqualsOrdinal(this string a, string b) => string.Equals(a, b, StringComparison.Ordinal);

        /// <summary>
        /// Compare strings using ordinal sort rules and ignoring the case of
        /// the strings being compared.
        /// </summary>
        /// <param name="a">The first string to compare, or null.</param>
        /// <param name="b">The second string to compare, or null.</param>
        /// <returns>True if the value of the a parameter is equal to the value
        /// of the b parameter; otherwise, false.</returns>
        public static bool EqualsOrdinalIgnoreCase(this string a, string b)
            => string.Equals(a, b, StringComparison.OrdinalIgnoreCase);

        #endregion Equals

        /// <summary>
        /// Determines whether the specified text contains only alpha
        /// characters.
        /// </summary>
        /// <param name="text">The string to test.</param>
        /// <returns>
        /// True if the specified text only contains alpha characters;
        /// otherwise, false.
        /// </returns>
        public static bool IsAlphaCharacters(this string text)
        {
            if (text.IsNullOrEmpty())
            {
                return false;
            }

            for (int i = 0; i < text.Length; i++)
            {
                if (!char.IsLetter(text[i]))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Determines whether the specified text contains only alpha and
        /// whitespace characters.
        /// </summary>
        /// <param name="text">The string to test.</param>
        /// <returns>
        /// True if the specified text only contains alpha and whitespace
        /// characters; otherwise, false.
        /// </returns>
        public static bool IsAlphaWhiteSpaceCharacters(this string text)
            => text != null && text.All(c => char.IsLetter(c) || char.IsWhiteSpace(c));

        /// <summary>
        /// Determines whether all the characters in the string are only digits.
        /// </summary>
        /// <param name="text">The string to test.</param>
        /// <returns>
        /// True if the specified string is only digits; otherwise, false.
        /// </returns>
        public static bool IsDigitsOnly(this string text)
        {
            if (text.IsNullOrEmpty())
            {
                return false;
            }

            for (int i = 0; i < text.Length; i++)
            {
                if ((text[i] ^ '0') > 9)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Determines whether the specified path is directory.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>
        /// True if the specified path is directory; otherwise, false.
        /// </returns>
        /// <exception cref="FileNotFoundException">Thrown when string path is
        /// not found.</exception>
        public static bool IsDirectory(this string path)
        {
            try
            {
                return (File.GetAttributes(path) & FileAttributes.Directory) != 0;
            }
            catch (FileNotFoundException)
            {
                throw new FileNotFoundException(Resources.ERR_PathNotResolved, path);
            }
        }

        /// <summary>
        /// Determines whether the specified text is the specified length.
        /// </summary>
        /// <param name="text">The text data.</param>
        /// <param name="length">The required length.</param>
        /// <returns>
        /// True if the specified text is the correct length; otherwise, false.
        /// </returns>
        public static bool IsLength(this string text, int length) => text != null && text.Length == length;

        #region IsMatch

        /// <summary>
        /// Indicates whether the regular expression finds a match in the input
        /// string, using the regular expression specified  and the matching
        /// options supplied in the options parameter.
        /// </summary>
        /// <param name="input">The string to search for a match.</param>
        /// <param name="regularExpression">The regular expression pattern to
        /// match.</param>
        /// <param name="options">The <see cref="Regex"/> options. A bitwise OR
        /// combination of <see cref="RegexOptions"/> enumeration values.
        /// Default value is None.
        /// </param>
        /// <returns>
        /// True if the regular expression finds a match; otherwise, false.
        /// </returns>
        [DebuggerStepThrough]
        public static bool IsMatch(this string input, string regularExpression, RegexOptions options = RegexOptions.None)
            => Regex.IsMatch(input, regularExpression, options);

        #endregion IsMatch

        /// <summary>
        /// Indicates whether the specified string is a <c>null</c> reference or
        /// an String.Empty string.
        /// </summary>
        /// <param name="text">The string to test.</param>
        /// <returns>
        /// True if the value parameter is a null reference or an empty string
        /// (""); otherwise, false.
        /// </returns>
        /// <remarks>
        /// <seealso cref="string.IsNullOrEmpty"/> for more information.
        /// </remarks>
        [DebuggerStepThrough]
        public static bool IsNullOrEmpty(this string text) => string.IsNullOrEmpty(text);

        /// <summary>
        /// Indicates whether a specified string is null, empty, or consists
        /// only of white-space characters.
        /// </summary>
        /// <param name="text">The string to test.</param>
        /// <returns>
        /// True if the value parameter is null or <see cref="string.Empty"/>,
        /// or if <paramref name="text"/> consists exclusively of white-space
        /// characters.
        /// </returns>
        [DebuggerStepThrough]
        public static bool IsNullOrWhiteSpace(this string text)
        {
#if NET35
            return text == null || text.All(char.IsWhiteSpace);
#else
            return string.IsNullOrWhiteSpace(text);
#endif
        }

        /// <summary>
        /// Determines whether the specified text is a valid email address per
        /// RFC 822.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns>
        ///   <c>True</c> if the specified text is a valid email address;
        ///   otherwise, <c>false</c>.
        /// </returns>
        /// <remarks>
        /// The <see cref="Regex"/> expression used for this comparison can be
        /// found
        /// <see url="http://haacked.com/archive/2007/08/21/i-knew-how-to-validate-an-email-address-until-i.aspx">
        /// here</see>.
        /// </remarks>
        public static bool IsValidEmailAddress(this string text)
            => text != null && RegularExpressions.EmailValidationExpression.IsMatch(text);

        /// <summary>
        /// Determines whether the specified text is a number and greater than
        /// or equal to the minimum number.
        /// </summary>
        /// <param name="text">The text to validate.</param>
        /// <param name="minimumValue">The minimum number value.</param>
        /// <returns>
        ///     <c>True</c> if the specified text is valid; otherwise,
        ///     <c>false</c>.
        /// </returns>
        public static bool IsValidInteger(this string text, int minimumValue)
        {
            if (!int.TryParse(text, out int value))
            {
                return false;
            }

            return value >= minimumValue;
        }

        #region StartsWith

        /// <summary>
        /// Determines whether the beginning of this string matches the
        /// specified character <paramref name="value"/> when compared.
        /// </summary>
        /// <param name="text">The text data.</param>
        /// <param name="value">The <see cref="char"/> object to compare.
        /// </param>
        /// <param name="ignoreCase"><c>True</c> to ignore case when comparing
        /// this string and <paramref name="value"/>; otherwise, <c>false</c>.
        /// Default value is <c>false</c>.</param>
        /// <returns>
        /// <c>True</c> if the <paramref name="value"/> parameter matches the
        /// beginning of this string; otherwise, <c>false</c>.
        /// </returns>
        public static bool StartsWith(this string text, char value, bool ignoreCase = false)
        {
            if (text.IsNullOrEmpty())
            {
                return false;
            }

            char firstChar = text[0];
            if (!ignoreCase)
            {
                return firstChar == value;
            }

            return firstChar == char.ToLowerInvariant(value) || firstChar == char.ToUpperInvariant(value);
        }

        /// <summary>
        /// Determines whether the beginning and ending of this string matches
        /// the specified character <paramref name="value"/> when compared.
        /// </summary>
        /// <param name="text">The text data.</param>
        /// <param name="value">The <see cref="char"/> object to compare.
        /// </param>
        /// <param name="ignoreCase"><c>True</c> to ignore case when comparing
        /// this string and <paramref name="value"/>; otherwise, <c>false</c>.
        /// Default value is <c>false</c>.</param>
        /// <returns>
        /// <c>True</c> if the <paramref name="value"/> parameter matches the
        /// beginning and ending of this string; otherwise, <c>false</c>.
        /// </returns>
        public static bool StartsAndEndsWith(this string text, char value, bool ignoreCase = false)
        {
            return text != null && text.StartsWith(value, ignoreCase) && text.EndsWith(value, ignoreCase);
        }

        /// <summary>
        /// Determines whether the beginning and ending of this string matches
        /// the specified string when compared using the specified comparison
        /// option.
        /// </summary>
        /// <param name="text">The text data.</param>
        /// <param name="value">The <see cref="string"/> object to compare.
        /// </param>
        /// <param name="comparisonType">One of the
        /// <see cref="StringComparison"/> values that determines how this
        /// string and value are compared.</param>
        /// <returns>
        /// <c>True</c> if the <paramref name="value"/> parameter matches the
        /// beginning and ending of this string; otherwise, <c>false</c>.
        /// </returns>
        public static bool StartsAndEndsWith(this string text, string value, StringComparison comparisonType)
        {
            return text != null && text.StartsWith(value, comparisonType) && text.EndsWith(value, comparisonType);
        }

#if !NETSTANDARD1_3

        /// <summary>
        /// Determines whether the beginning and ending of this string matches
        /// the specified string when compared using the specified culture.
        /// </summary>
        /// <param name="text">The text data.</param>
        /// <param name="value">The <see cref="string"/> object to compare.
        /// </param>
        /// <param name="ignoreCase"><c>True</c> to ignore case when comparing
        /// this string and <paramref name="value"/>; otherwise, <c>false</c>.
        /// Default value <c>false</c>.</param>
        /// <param name="culture">Cultural information that determines how this
        /// string and value are compared. If culture is <c>null</c>, the
        /// current culture is used. Default value is <c>null</c>.</param>
        /// <returns>
        /// <c>True</c> if the <paramref name="value"/> parameter matches the
        /// beginning and ending of this string; otherwise, <c>false</c>.
        /// </returns>
        public static bool StartsAndEndsWith(this string text, string value, bool ignoreCase = false, CultureInfo culture = null)
        {
            return text != null && text.StartsWith(value, ignoreCase, culture) && text.EndsWith(value, ignoreCase, culture);
        }

#endif

        #endregion StartsWith

        /// <summary>
        /// Validates the specified text that it contains the minimum specified
        /// capital letters.
        /// </summary>
        /// <param name="text">The text to be searched.</param>
        /// <param name="count">The minimum number of capital letters to be
        /// <c>true</c>.</param>
        /// <returns>
        ///     <c>True</c> if the specified text contains a capital letter
        ///     count greater than or equal to the specified character count;
        ///     otherwise, <c>false</c>.
        /// </returns>
        public static bool ValidateCapitalLetters(this string text, int count)
        {
            if (text == null || count < 0)
            {
                return false;
            }

            int upperCaseCount = 0;
            for (int i = 0; i < text.Length; i++)
            {
                if (text[i] >= 65 && text[i] <= 90)
                {
                    upperCaseCount++;
                }
            }

            return upperCaseCount >= count;
        }

        /// <summary>
        /// Validates dashes based on input count.
        /// </summary>
        /// <param name="text">The text to be searched.</param>
        /// <param name="validCount">The number of dashes to be <c>true</c>.
        /// </param>
        /// <returns><c>True</c> if dashes count equals count; otherwise,
        /// <c>false</c>.</returns>
        /// <remarks>
        /// <para>Tested the performance of this method using the Performance
        /// Library and found that the <see langword="foreach"/> loop was the
        /// fastest code for this specific search method. Do not convert this
        /// method to a linq method. Linq was 4 times slower than a
        /// <c>foreach</c> loop.</para>
        /// <para>Passing in a text value of null and a valid count of 0 will
        /// still return false because the search text in null. Passing an empty
        /// string value and a valid count of 0 will still return a <c>true</c>
        /// value.</para>
        /// </remarks>
        public static bool ValidateDashes(this string text, int validCount)
        {
            if (text == null)
            {
                return false;
            }

            int count = text.Count(character => character == '-');
            return count == validCount;
        }

        /// <summary>
        /// Validates the specified text that it contains the specified numeric
        /// character count.
        /// </summary>
        /// <param name="text">The text to be searched.</param>
        /// <param name="count">The minimum number of numeric characters to be
        /// <c>true</c>.</param>
        /// <returns>
        ///     <c>True</c> if the specified text contains a numeric character
        ///     count greater than or equal to the specified character count;
        ///     otherwise, <c>false</c>.
        /// </returns>
        public static bool ValidateNumbers(this string text, int count)
        {
            return text != null && count > -1 && RegularExpressions.NumbersRegEx.Matches(text).Count >= count;
        }

        #endregion Validations

#region Html

#if !NETSTANDARD1_3

        /// <summary>
        /// Converts the specified string to a HTML-encoded string.
        /// </summary>
        /// <param name="data">The data string.</param>
        /// <returns>The HTML encoded string representing the current data input
        /// string.</returns>
        [DebuggerStepThrough]
        public static string EncodeHtml(this string data) => HttpUtility.HtmlEncode(data);

        /// <summary>
        /// Converts the specified HTML-encoded string into a decoded string.
        /// </summary>
        /// <param name="encodedData">The data string.</param>
        /// <returns>The HTML decoded string representing the current data input
        /// string.</returns>
        [DebuggerStepThrough]
        public static string DecodeHtml(this string encodedData) => HttpUtility.HtmlDecode(encodedData);

#endif

#endregion Html

#region Converts

        /// <summary>
        /// Converts the file path location from admin UNC path to local path.
        /// </summary>
        /// <param name="filePathLocation">The file path location.</param>
        /// <returns>A local file path location string.</returns>
        /// <exception cref="NullReferenceException">Exception thrown if the
        /// specified file path location is null.</exception>
        public static string ConvertAdminFilePath(this string filePathLocation)
        {
            int index = filePathLocation.IndexOf('$');
            return index > -1
                       ? filePathLocation.Substring(index - 1).Replace('$', ':')
                       : filePathLocation;
        }

        /// <summary>
        /// Retrieves a substring from this instance. The substring starts at a
        /// specified character position and has a specified length.
        /// </summary>
        /// <param name="text">The string value.</param>
        /// <param name="startIndex">The zero-based starting character position
        /// of a substring in this instance. Negative values will be set to
        /// zero.</param>
        /// <param name="length">The number of characters in the substring.
        /// </param>
        /// <returns>A string that is equivalent to the substring of length that
        /// begins at startIndex in this instance; otherwise, an empty string.
        /// </returns>
        public static string SubstringSafe(this string text, int startIndex, int length)
        {
            if (text == null || text.Length <= startIndex || length < 1)
            {
                return string.Empty;
            }

            if (startIndex < 0)
            {
                startIndex = 0;
            }

            return text.Length - startIndex <= length ? text.Substring(startIndex) : text.Substring(startIndex, length);
        }

        /// <summary>
        /// Converts the specified string value to a boolean value. Acceptable
        /// values are "yes" and "true", any other values will be returned as
        /// false. Comparison is case insensitive.
        /// </summary>
        /// <param name="value">The string value.</param>
        /// <returns>True if the value is "true" or "yes"; otherwise, false.
        /// </returns>
        public static bool ToBoolean(this string value)
        {
            return "TRUE".EqualsOrdinalIgnoreCase(value) ||
                   "YES".EqualsOrdinalIgnoreCase(value);
        }

        /// <summary>
        /// Converts the specified string value to a boolean value. Acceptable
        /// values are "no" and "false", any other values will be returned as
        /// true. Comparison is case insensitive.
        /// </summary>
        /// <param name="value">The string value.</param>
        /// <returns>False if the value is "false" or "no"; otherwise, true.
        /// </returns>
        public static bool ToBooleanMatchFalse(this string value)
        {
            return !"FALSE".EqualsOrdinalIgnoreCase(value) &&
                   !"NO".EqualsOrdinalIgnoreCase(value);
        }

        /// <summary>
        /// Converts the string representation of the name or numeric value of
        /// one or more enumerated constants to an equivalent enumerated object.
        /// </summary>
        /// <typeparam name="TEnum">An enumeration type.</typeparam>
        /// <param name="value">A string containing the name or value to
        /// convert.</param>
        /// <param name="ignoreCase">True to ignore case; false to regard case.
        /// </param>
        /// <returns>An object of type <typeparamref name="TEnum"/> whose value
        /// is represented by value.</returns>
#if NET35
        public static TEnum ToEnum<TEnum>(this string value, bool ignoreCase = false) where TEnum : struct
#else

        public static TEnum ToEnum<TEnum>(this string value, bool ignoreCase = false) where TEnum : Enum
#endif
        {
            return (TEnum)Enum.Parse(typeof(TEnum), value, ignoreCase);
        }

        /// <summary>
        /// Converts the string representation of the name or numeric value of
        /// one or more enumerated constants to an equivalent enumerated object.
        /// </summary>
        /// <typeparam name="TEnum">An enumeration type.</typeparam>
        /// <param name="value">A string containing the name or value to
        /// convert.</param>
        /// <param name="defaultValue">The default enumeration value if the
        /// conversion fails.</param>
        /// <param name="ignoreCase">True to ignore case; false to regard case.
        /// </param>
        /// <returns>An object of type <typeparamref name="TEnum"/> whose value
        /// is represented by value or specified default value.</returns>
        public static TEnum ToEnum<TEnum>(this string value, TEnum defaultValue, bool ignoreCase = false) where TEnum : struct
        {
#if NET35
            try
            {
                // This is not a great implementation but will work to have
                // backwards compatibility with NET35.
                return value.ToEnum<TEnum>(ignoreCase);
            }
            catch (Exception)
            {
                return defaultValue;
            }
#else
            return Enum.TryParse(value, ignoreCase, out TEnum result) ? result : defaultValue;
#endif
        }

        /// <summary>
        /// Converts a string of format [HH],[MM],[SS] to a
        /// <see cref="TimeSpan"/>.
        /// </summary>
        /// <param name="value">The string value.</param>
        /// <exception cref="System.FormatException">
        /// String to be converted must be in the following format:
        /// <c>[HH],[MM],[SS]</c>.
        /// </exception>
        /// <returns>
        /// A <see cref="TimeSpan"/> for the specified value.
        /// </returns>
        public static TimeSpan ToTimeSpan(this string value)
        {
            try
            {
                string[] splitDt = value.Split(',');
                int hh = int.Parse(splitDt[0]);
                int mm = int.Parse(splitDt[1]);
                int ss = int.Parse(splitDt[2]);
                return new TimeSpan(hh, mm, ss);
            }
            catch (Exception)
            {
                throw new FormatException(Resources.ERR_StringTimeSpanFormat);
            }
        }

        /// <summary>
        /// Converts the specified string value to a <see cref="Uri"/> and
        /// returns the <see cref="string"/> representation.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The <see cref="string"/> representation of the Uri.
        /// </returns>
        /// <example>
        /// <code>
        /// Console.WriteLine(Path.Combine(@"http://www.mystore.com", @"merchandise.html").ToUriString());
        /// </code>Prints: http://www.mystore.com/merchandise.html
        /// </example>
        [DebuggerStepThrough]
        public static string ToUriString(this string value) => new Uri(value).ToString();

        /// <summary>
        /// Gets the specified string value or a <c>null</c> value.
        /// </summary>
        /// <param name="text">The input text.</param>
        /// <returns>The input string value if greater than zero; otherwise,
        /// <c>null</c>.</returns>
        [DebuggerStepThrough]
        public static string ValueOrDefault(this string text) => text.IsNullOrEmpty() ? null : text;

#endregion Converts

#region Formats

        /// <summary>
        /// Replaces the format item in a specified System.String with the text
        /// equivalent of the value of a corresponding System.Object instance in
        /// a specified array.
        /// </summary>
        /// <param name="format">A composite format string.</param>
        /// <param name="arguments">An System.Object array containing zero or
        /// more objects to format.</param>
        /// <returns>A copy of format in which the format items have been
        /// replaced by the System.String equivalent of the corresponding
        /// instances of System.Object in arguments.</returns>
        [DebuggerStepThrough]
        public static string FormatWith(this string format, params object[] arguments) => string.Format(format, arguments);

        /// <summary>
        /// Replaces the format item in a specified <see cref="System.String"/>
        /// with the text equivalent of the value of a corresponding
        /// <see cref="System.Object"/> instance in a specified array. A
        /// specified parameter supplies culture-specific formatting
        /// information.
        /// </summary>
        /// <param name="format">A composite format string.</param>
        /// <param name="provider">An <see cref="System.IFormatProvider"/> that
        /// supplies culture-specific formatting information.</param>
        /// <param name="arguments">An System.Object array containing zero or
        /// more objects to format.</param>
        /// <returns>A copy of format in which the format items have been
        /// replaced by the <see cref="System.String"/> equivalent of the
        /// corresponding instances of <see cref="System.Object"/> in arguments.
        /// </returns>
        [DebuggerStepThrough]
        public static string FormatWith(this string format, IFormatProvider provider, params object[] arguments)
            => string.Format(provider, format, arguments);

        /// <summary>
        /// Wraps the specified text string by appending the append text to the
        /// start and end of the text.
        /// </summary>
        /// <param name="text">The text to be wrapped.</param>
        /// <param name="appendText">The string to append to the start and end.
        /// </param>
        /// <returns>Returns the specified <paramref name="text"/> wrapped with
        /// the <paramref name="appendText"/>.</returns>
        public static string WrapWith(this string text, string appendText)
        {
            if (appendText.IsNullOrEmpty())
            {
                return text;
            }

            if (text.IsNullOrEmpty())
            {
                return appendText + appendText;
            }

            return string.Concat(appendText, text, appendText);
        }

#endregion Formats

#region Joins

        /// <summary>
        /// Concatenates all the elements of a string array, using the specified
        /// separator between each element.
        /// </summary>
        /// <param name="separator">The string to use as a separator.
        /// <paramref name="separator"/> is included in the returned string only
        /// if <paramref name="values"/> has more than one element.</param>
        /// <param name="values">An array that contains the elements to
        /// concatenate.</param>
        /// <returns>A string that consists of the elements in
        /// <paramref name="values"/> delimited by the
        /// <paramref name="separator"/> string. If <paramref name="values"/> is
        /// an empty array, the method returns String.Empty.</returns>
        public static string Join(this string separator, params string[] values) => string.Join(separator, values);

        /// <summary>
        /// Concatenates the elements of an object array, using the specified
        /// separator between each element.
        /// </summary>
        /// <param name="separator">The string to use as a separator.
        /// <paramref name="separator"/> is included in the returned string only
        /// if <paramref name="values"/> has more than one element.</param>
        /// <param name="values">An array that contains the elements to
        /// concatenate.</param>
        /// <returns>A string that consists of the elements of
        /// <paramref name="values"/> delimited by the
        /// <paramref name="separator"/> string. If <paramref name="values"/> is
        /// an empty array, the method returns String.Empty.</returns>
        public static string Join(this string separator, params object[] values)
        {
#if NET35
            ADP.CheckArgumentNull(values, nameof(values));
            if (separator == null)
            {
                separator = string.Empty;
            }

            var builder = new StringBuilder();
            for (int index = 0; index < values.Length; index += 1)
            {
                builder.Append(separator);
                if (values[index] != null)
                {
                    builder.Append(values[index].ToStringSafe());
                }
            }

            return builder.ToString();
#else
            return string.Join(separator, values);
#endif
        }

        /// <summary>
        /// Concatenates the members of a collection, using the specified
        /// separator between each member.
        /// </summary>
        /// <param name="values">A collection that contains the string to
        /// concatenate.</param>
        /// <param name="separator">The string to use as a separator.
        /// <paramref name="separator"/> is included in the returned string only
        /// if <paramref name="values"/> has more than one element.</param>
        /// <returns>A string that consists of the members of
        /// <paramref name="values"/> delimited by the
        /// <paramref name="separator"/> string. If <paramref name="values"/>
        /// has no members, the method returns String.Empty.</returns>
        [DebuggerStepThrough]
        public static string Join(this IEnumerable<string> values, string separator)
        {
#if NET35
            return string.Join(separator, values.ToArray());
#else
            return string.Join<string>(separator, values);
#endif
        }

#endregion Joins
    }
}