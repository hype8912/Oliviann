namespace Oliviann.IO
{
    using System.Runtime.InteropServices;

    /// <summary>
    /// Represents a collection of commonly used extension methods for extending
    /// <see cref="IniReader"/> and <see cref="IniWriter"/>.
    /// </summary>
    public static class IniExtensions
    {
        #region Reader Methods

        /// <summary>
        /// Reads the INI value for a specific group and key. If the returned
        /// value is  not of an integer value, then the default input value is
        /// returned.
        /// </summary>
        /// <param name="reader">The INI reader instance.</param>
        /// <param name="section">The INI group section.</param>
        /// <param name="keyName">Name of the key.</param>
        /// <param name="defaultValue">The default value to be returned if no
        /// section and key are found.</param>
        /// <returns>
        /// A integer value read from the INI file.
        /// </returns>
        /// <remarks>
        /// Instead of using the native "GetPrivateProfileInt" windows API, we
        /// are using the  current get string value and then using managed code
        /// to convert the string value to an integer value; otherwise, the
        /// default value is returned if the conversion fails.
        /// </remarks>
        public static int ReadInteger(this IniReader reader, string section, string keyName, int defaultValue = 0)
        {
            ADP.CheckArgumentNull(reader, nameof(reader));

            string temp = reader.ReadString(section, keyName, defaultValue.ToString(), CharSet.Unicode);
            return temp.ToInt32(defaultValue);
        }

        /// <summary>
        /// Determines if a key exists in a specific section.
        /// </summary>
        /// <param name="reader">The INI reader instance.</param>
        /// <param name="section">The INI group section.</param>
        /// <param name="keyName">Name of the key.</param>
        /// <returns>True if the specified key exists in the specified section;
        /// otherwise, false.</returns>
        public static bool SectionKeyExists(this IniReader reader, string section, string keyName)
        {
            ADP.CheckArgumentNull(reader, nameof(reader));
            return reader.ReadString(section, keyName, string.Empty).Length > 0;
        }

        #endregion Reader Methods

        #region Writer Methods

        /// <summary>
        /// Writes the integer value to the int file using the defined encoding.
        /// </summary>
        /// <param name="writer">The INI writer instance.</param>
        /// <param name="section">The ini group section.</param>
        /// <param name="keyName">Name of the key.</param>
        /// <param name="newValue">The new integer value to be saved.</param>
        /// <param name="encoding">Optional. The character set encoding scheme
        /// to use.</param>
        /// <returns>An integer value that the operation completed.</returns>
        public static int WriteInt(
            this IniWriter writer,
            string section,
            string keyName,
            int newValue,
            CharSet encoding = CharSet.Ansi)
        {
            ADP.CheckArgumentNull(writer, nameof(writer));
            return writer.WriteString(section, keyName, newValue.ToString(), encoding);
        }

        /// <summary>
        /// Deletes the key from the specific section.
        /// </summary>
        /// <param name="writer">The INI writer instance.</param>
        /// <param name="section">The INI group section.</param>
        /// <param name="keyName">Name of the key.</param>
        public static void DeleteKey(this IniWriter writer, string section, string keyName)
        {
            ADP.CheckArgumentNull(writer, nameof(writer));
            ADP.CheckArgumentNullOrEmpty(section, nameof(section));
            ADP.CheckArgumentNullOrEmpty(keyName, nameof(keyName));

            writer.WriteString(section, keyName, null);
        }

        /// <summary>
        /// Deletes a whole section group from the INI file.
        /// </summary>
        /// <param name="writer">The INI writer instance.</param>
        /// <param name="section">The INI group section.</param>
        public static void DeleteSection(this IniWriter writer, string section)
        {
            ADP.CheckArgumentNull(writer, nameof(writer));
            ADP.CheckArgumentNullOrEmpty(section, nameof(section));

            writer.WriteString(section, null, null);
        }

        #endregion Writer Methods
    }
}