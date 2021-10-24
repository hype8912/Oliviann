namespace Oliviann
{
    #region Usings

    using System;
    using Oliviann.Security.Cryptography;

    #endregion Usings

    /// <summary>
    /// Represents a collection of commonly used helper methods for making
    /// working with <see cref="String"/>s easier.
    /// </summary>
    public static class StringHelpers
    {
        #region Fields

        /// <summary>
        /// The default ASCII alpha character set to use for generating a random
        /// string.
        /// </summary>
        public const string DefaultAsciiAlphaCharacters = @"ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";

        /// <summary>
        /// The default ASCII alphanumeric character string to use for generating
        /// a random string.
        /// </summary>
        public const string DefaultAsciiAlphanumericCharacters = @"ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

        #endregion Fields

        /// <summary>
        /// Extracts the machine name or value from the specified UNC directory
        /// path.
        /// </summary>
        /// <param name="uncPath">The UNC directory path.</param>
        /// <returns>
        /// The machine name value if found; otherwise, <c>null</c>.
        /// </returns>
        public static string ExtractMachineFromUncPath(string uncPath)
        {
            if (uncPath == null || !uncPath.StartsWith(@"\\"))
            {
                return null;
            }

            int index = uncPath.IndexOf('\\', 3);
            return index < 0 ? uncPath.Substring(2) : uncPath.Substring(2, index - 2);
        }

        /// <summary>
        /// Generates a random string of a specified size using only the
        /// characters provided. Recommend not to use this method for anything
        /// security related.
        /// </summary>
        /// <param name="length">The length of the string to generate.</param>
        /// <param name="chars">Optional. The characters to use for generating
        /// the string.
        /// </param>
        /// <returns>A randomly generated string using the specified
        /// <paramref name="chars"/>.</returns>
        public static string GenerateRandomString(int length, string chars = DefaultAsciiAlphaCharacters)
        {
            if (length < 1)
            {
                return string.Empty;
            }

            if (chars.IsNullOrEmpty())
            {
                chars = DefaultAsciiAlphaCharacters;
            }

            var rand = new SecureRandom();
            var buffer = new char[length];
            for (int i = 0; i < length; i += 1)
            {
                buffer[i] = chars[rand.Next(chars.Length)];
            }

            return new string(buffer);
        }
    }
}