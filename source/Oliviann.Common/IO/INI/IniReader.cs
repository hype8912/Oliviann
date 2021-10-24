namespace Oliviann.IO
{
    #region Usings

    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Text;
    using Oliviann.Collections.Generic;

    #endregion Usings

    /// <summary>
    /// Represents a class for reading INI files using the native Windows APIs.
    /// </summary>
    public class IniReader : IDisposable
    {
        #region Constructor/Destructor

        /// <summary>
        /// Initializes a new instance of the <see cref="IniReader"/> class.
        /// </summary>
        /// <param name="iniFilePath">The INI file path.</param>
        /// <exception cref="ArgumentNullException">The specified INI file path
        /// cannot be null.</exception>
        /// <exception cref="FileNotFoundException">The specified INI file path
        /// does not exist.</exception>
        public IniReader(string iniFilePath)
        {
            ADP.CheckArgumentNull(iniFilePath, nameof(iniFilePath));
            ADP.CheckFileNotFound(iniFilePath, string.Empty);
            this.FilePath = iniFilePath;
        }

        #endregion Constructor/Destructor

        #region Properties

        /// <summary>
        /// Gets the full INI file path.
        /// </summary>
        /// <value>The INI file path.</value>
        public string FilePath { get; private set; }

        #endregion Properties

        #region Close/Dispose

        /// <summary>
        /// Closes the <see cref="IniReader"/> and releases any system resources
        /// associated with the <see cref="IniReader"/>.
        /// </summary>
        public virtual void Close() => this.Dispose();

        /// <summary>
        /// Performs application-defined tasks associated with freeing,
        /// releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion Close/Dispose

        /// <summary>
        /// Reads the INI value for a specific group and key. If the length
        /// returned is longer than the max size of the allocated string then it
        /// will automatically double the allocation space until the allocation
        /// space is large enough for the returned string.
        /// </summary>
        /// <param name="section">The INI group section.</param>
        /// <param name="keyName">Name of the key.</param>
        /// <param name="defaultValue">The default value to be returned if no
        /// section and key are found.</param>
        /// <param name="encoding">The character set encoding scheme to use.
        /// </param>
        /// <returns>
        /// A string value read from the INI file using specified encoding.
        /// </returns>
        public virtual string ReadString(string section, string keyName, string defaultValue, CharSet encoding = CharSet.Ansi)
        {
            for (int maxsize = 256;; maxsize *= 2)
            {
                var builder = new StringBuilder(maxsize);
                int length = encoding == CharSet.Ansi
                                 ? GetPrivateProfileString(section, keyName, defaultValue, builder, maxsize, this.FilePath)
                                 : GetPrivateProfileStringUnicode(section, keyName, defaultValue, builder, maxsize, this.FilePath);

                if (length < maxsize - 1)
                {
                    return builder.ToString();
                }
            }
        }

        /// <summary>
        /// Retrieves the whole section inside the INI file.
        /// </summary>
        /// <param name="section">The INI group section.</param>
        /// <returns>A collection of section data (keys and values).</returns>
        public virtual IReadOnlyCollection<string> ReadSection(string section)
        {
            const int BufferSize = 2048;
            var builder = new StringBuilder();

            IntPtr returnedString = Marshal.AllocCoTaskMem(BufferSize);
            try
            {
                int bytesReturned = GetPrivateProfileSection(section, returnedString, BufferSize, this.FilePath);

                for (int i = 0; i < bytesReturned - 1; i += 1)
                {
                    var charValue = (char)Marshal.ReadByte(new IntPtr((long)returnedString + i));
                    builder.Append(charValue);
                }
            }
            finally
            {
                Marshal.FreeCoTaskMem(returnedString);
            }

            string sectionData = builder.ToString();
            return sectionData.Split('\0').ToListHelper();
        }

        /// <summary>
        /// Retrieves all the section names inside the INI file.
        /// </summary>
        /// <returns>A collection of section names.</returns>
        public virtual IReadOnlyCollection<string> ReadSectionNames() => this.GetSectionKeyNames(null);

        /// <summary>
        /// Retrieves all the section key names for a specified section group.
        /// </summary>
        /// <param name="section">The INI group section.</param>
        /// <returns>A generic string list of section keys.</returns>
        public virtual IReadOnlyCollection<string> GetSectionKeyNames(string section)
        {
            for (int maxsize = 256;; maxsize *= 2)
            {
                var bytes = new byte[maxsize];
                int length = GetPrivateProfileString(section, null, null, bytes, maxsize, this.FilePath);

                if (length < maxsize - 1)
                {
                    string temp = Encoding.ASCII.GetString(bytes, 0, length - (length > 0 ? 1 : 0));
                    return temp.Split('\0').ToListHelper();
                }
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing,
        /// releasing, or resetting unmanaged resources.
        /// </summary>
        /// <param name="disposing">True to release both managed and unmanaged
        /// resources; false to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
        }

        #region Dll Kernel Ansi

        /////// <summary>
        /////// Retrieves an integer associated with a key in the specified section
        /////// of an initialization file.
        /////// </summary>
        /////// <param name="lpSectionName">The name of the section in the
        /////// initialization file.</param>
        /////// <param name="lpKeyName">The name of the key whose value is to be
        /////// retrieved. This value is in  the form of a string; the
        /////// GetPrivateProfileInt function converts the string into an integer
        /////// and returns the integer.</param>
        /////// <param name="lpDefault">The default value to return if the key name
        /////// cannot be found in the initialization file.</param>
        /////// <param name="iniFileName">The name of the initialization file. If
        /////// this parameter does not  contain a full path to the file, the system
        /////// searches for the file in the Windows directory.</param>
        /////// <returns>The return value is the integer equivalent of the string
        /////// following the specified key name in the specified  initialization
        /////// file. If the key is not found, the return value is the specified
        /////// default value.</returns>
        /////// <remarks>The function searches the file for a key that matches the
        /////// name specified by the <paramref name="lpKeyName"/> parameter under
        /////// the section name specified by the <paramref name="lpSectionName"/>
        /////// parameter.</remarks>
        ////[DllImport(@"kernel32.dll", EntryPoint = @"GetPrivateProfileIntA", SetLastError = true, CharSet = CharSet.Ansi)]
        ////private static extern int GetPrivateProfileInt(
        ////    string lpSectionName,
        ////    string lpKeyName,
        ////    int lpDefault,
        ////    string iniFileName);

        /// <summary>
        /// Retrieves all the keys and values for the specified section of an
        /// initialization file.
        /// </summary>
        /// <param name="lpSectionName">The name of the section in the
        /// initialization file.</param>
        /// <param name="returnedString">A pointer to a buffer that receives the
        /// key name and value pairs  associated with the named section. The
        /// buffer is filled with one or more <c>null</c>-terminated  strings;
        /// the last string is followed by a second <c>null</c> character.
        /// </param>
        /// <param name="stringLength">The size of the buffer pointed to by the
        /// <paramref name="returnedString"/> parameter, in characters. The
        /// maximum profile section size is 32,767 characters.</param>
        /// <param name="iniFileName">The name of the initialization file. If
        /// this parameter does not  contain a full path to the file, the system
        /// searches for the file in the Windows directory.</param>
        /// <returns>
        /// The return value specifies the number of characters copied to the
        /// buffer, not including  the terminating <c>null</c> character. If the
        /// buffer is not large enough to contain all the key  name and value
        /// pairs associated with the named section, the return value is equal
        /// to <paramref name="stringLength"/> minus two.
        /// </returns>
        /// <remarks>
        /// The data in the buffer pointed to by the
        /// <paramref name="returnedString"/> parameter consists of one or more
        /// <c>null</c>-terminated strings, followed by a final <c>null</c>
        /// character. Each string has the following format: key=string The
        /// GetPrivateProfileSection function is not case-sensitive; the string
        /// pointed to by the
        /// <paramref name="lpSectionName"/> parameter can be a combination of
        /// uppercase and lowercase letters. This operation is atomic; no
        /// updates to the specified initialization file are allowed  while the
        /// key name and value pairs for the section are being copied to the
        /// buffer pointed to  by the <paramref name="returnedString"/>
        /// parameter.
        /// </remarks>
        [DllImport(@"kernel32.dll", EntryPoint = @"GetPrivateProfileSectionA", SetLastError = true, CharSet = CharSet.Ansi)]
        private static extern int GetPrivateProfileSection(
            string lpSectionName,
            IntPtr returnedString,
            int stringLength,
            string iniFileName);

        /////// <summary>
        /////// Retrieves the names of all sections in an initialization file.
        /////// </summary>
        /////// <param name="returnedValue">A string builder to a buffer that
        /////// receives the section names associated  with the named file. The
        /////// buffer is filled with one or more <c>null</c>-terminated strings;
        /////// the last string  is followed by a second <c>null</c> character.
        /////// </param>
        /////// <param name="valueLength">The size of the buffer pointed to by the
        /////// <paramref name="returnedValue"/> parameter, in characters.</param>
        /////// <param name="iniFileName">The name of the initialization file. If
        /////// this parameter is NULL, the  function searches the Win.ini file. If
        /////// this parameter does not contain a full path to the file,  the system
        /////// searches for the file in the Windows directory.</param>
        /////// <returns>The return value specifies the number of characters copied
        /////// to the specified buffer, not  including the terminating <c>null</c>
        /////// character. If the buffer is not large enough to contain all the
        /////// section names associated with the specified initialization file, the
        /////// return value is equal to the  size specified by
        /////// <paramref name="valueLength"/> minus two.</returns>
        /////// <remarks>This operation is atomic; no updates to the initialization
        /////// file are allowed while the  section names are being copied to the
        /////// buffer.</remarks>
        ////[DllImport(@"kernel32.dll", EntryPoint = @"GetPrivateProfileSectionNamesA", CharSet = CharSet.Ansi,
        //// ExactSpelling = true, SetLastError = true)]
        ////private static extern int GetPrivateProfileSectionNames(
        ////    StringBuilder returnedValue,
        ////    int valueLength,
        ////    string iniFileName);

        /// <summary>
        /// Retrieves a string from the specified section in an initialization
        /// file.
        /// </summary>
        /// <param name="lpSectionName">The name of the section containing the
        /// key name. If  this parameter is NULL, the GetPrivateProfileString
        /// function copies all section  names in the file to the supplied
        /// buffer.</param>
        /// <param name="lpKeyName">The name of the key whose associated string
        /// is to be  retrieved. If this parameter is <c>null</c>, all key names
        /// in the section specified  by the <paramref name="lpSectionName"/>
        /// parameter are copied to the buffer specified by the
        /// <paramref name="returnedValue"/> parameter.</param>
        /// <param name="defaultValue">A default string. If the
        /// <paramref name="lpKeyName"/> key cannot be  found in the
        /// initialization file, GetPrivateProfileString copies the default
        /// string to the <paramref name="returnedValue"/> buffer. If this
        /// parameter is NULL, the default  is an empty string, "". Avoid
        /// specifying a default string with trailing blank characters. The
        /// function inserts a <c>null</c> character in the
        /// <paramref name="returnedValue"/> buffer to strip  any trailing
        /// blanks.</param>
        /// <param name="returnedValue">A pointer to the buffer that receives
        /// the retrieved string.</param>
        /// <param name="valueLength">The size of the buffer pointed to by the
        /// <paramref name="returnedValue"/> parameter, in characters.</param>
        /// <param name="iniFileName">The name of the initialization file. If
        /// this  parameter does not contain a full path to the file, the system
        /// searches  for the file in the Windows directory.</param>
        /// <returns>The return value is the number of characters copied to the
        /// buffer,  not including the terminating <c>null</c> character. If
        /// neither <paramref name="lpSectionName"/> nor
        /// <paramref name="lpKeyName"/> is NULL and the supplied destination
        /// buffer is too small to hold the requested string, the string is
        /// truncated and followed by a <c>null</c> character, and the return
        /// value is equal to
        /// <paramref name="valueLength"/> minus one. If either
        /// <paramref name="lpSectionName"/> or <paramref name="lpKeyName"/> is
        /// NULL and the supplied destination  buffer is too small to hold all
        /// the strings, the last string is truncated and followed by two
        /// <c>null</c> characters. In this case, the return  value is equal to
        /// <paramref name="valueLength"/> minus two.</returns>
        [DllImport(@"kernel32.dll", EntryPoint = @"GetPrivateProfileStringA", CharSet = CharSet.Ansi, ExactSpelling = true,
            SetLastError = true)]
        private static extern int GetPrivateProfileString(
            string lpSectionName,
            string lpKeyName,
            string defaultValue,
            StringBuilder returnedValue,
            int valueLength,
            string iniFileName);

        [DllImport(@"kernel32.dll", EntryPoint = @"GetPrivateProfileStringA", CharSet = CharSet.Ansi, ExactSpelling = true,
            SetLastError = true)]
        private static extern int GetPrivateProfileString(
            string lpSectionName,
            string lpKeyName,
            string defaultValue,
            [MarshalAs(UnmanagedType.LPArray)] byte[] result,
            int valueLength,
            string iniFileName);

        #endregion Dll Kernel Ansi

        #region Dll Kernel Unicode

        [DllImport(@"kernel32.dll", EntryPoint = @"GetPrivateProfileStringW", CharSet = CharSet.Unicode, ExactSpelling = true,
            SetLastError = true)]
        private static extern int GetPrivateProfileStringUnicode(
            string iniSectionName,
            string iniKeyName,
            string defaultValue,
            StringBuilder returnedValue,
            int valueLength,
            string iniFileName);

        ////[DllImport("kernel32.dll", EntryPoint = "GetPrivateProfileIntW", CharSet = CharSet.Unicode, ExactSpelling = true,
        //// SetLastError = true)]
        ////private static extern int GetPrivateProfileIntUni(
        ////    string lpApplicationName,
        ////    string lpKeyName,
        ////    string lpDefault,
        ////    string lpFileName);

        #endregion Dll Kernel Unicode
    }
}