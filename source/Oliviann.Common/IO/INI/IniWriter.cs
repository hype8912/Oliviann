namespace Oliviann.IO
{
    #region Usings

    using System;
    using System.IO;
    using System.Runtime.InteropServices;

    #endregion Usings

    /// <summary>
    /// Represents a class for writing INI files using the native Windows APIs.
    /// </summary>
    public class IniWriter : IDisposable
    {
        #region Constructor/Destructor

        /// <summary>
        /// Initializes a new instance of the <see cref="IniWriter"/> class.
        /// </summary>
        /// <param name="iniFilePath">The ini file path.</param>
        /// <exception cref="FileNotFoundException">The specified ini file path
        /// does not exist.</exception>
        public IniWriter(string iniFilePath)
        {
            ADP.CheckArgumentNullOrEmpty(iniFilePath, nameof(iniFilePath));
            ADP.CheckFileNotFound(iniFilePath, string.Empty);
            this.FilePath = iniFilePath;
            this.AutoFlush = true;
        }

        #endregion Constructor/Destructor

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether to flush the cache data when
        /// needed automatically or only flush when a manual flush is invoked.
        /// Default value is <c>true</c>.
        /// </summary>
        /// <value>
        ///   <c>True</c> if auto flush is enabled; otherwise, <c>false</c>.
        /// </value>
        public bool AutoFlush { get; set; }

        /// <summary>
        /// Gets the full ini file path.
        /// </summary>
        /// <value>The ini file path.</value>
        public string FilePath { get; private set; }

        #endregion Properties

        #region Close/Dispose

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

        #region Write

        /// <summary>
        /// Writes the string value to the int file using the defined encoding.
        /// </summary>
        /// <param name="section">The ini group section.</param>
        /// <param name="keyName">Name of the key.</param>
        /// <param name="newString">The new string value to be saved.</param>
        /// <param name="encoding">The character set encoding scheme to use.
        /// </param>
        /// <returns>An integer value that the operation completed.</returns>
        public virtual int WriteString(string section, string keyName, string newString, CharSet encoding = CharSet.Ansi)
        {
            int retValue = encoding.Equals(CharSet.Ansi)
                               ? WritePrivateProfileString(section, keyName, newString, this.FilePath)
                               : WritePrivateProfileStringUnicode(section, keyName, newString, this.FilePath);

            if (this.AutoFlush)
            {
                this.Flush();
            }

            return retValue;
        }

        #endregion Write

        #region Flush

        /// <summary>
        /// Forces all the cached changes to be written to the ini file.
        /// </summary>
        public virtual void Flush()
        {
            WritePrivateProfileString(null, null, null, this.FilePath);
        }

        #endregion Flush

        /// <summary>
        /// Performs application-defined tasks associated with freeing,
        /// releasing, or resetting unmanaged resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and
        /// unmanaged resources; <c>false</c> to release only unmanaged
        /// resources.</param>
        protected virtual void Dispose(bool disposing)
        {
        }

        #region Dll Kernel Ansi

        /// <summary>
        /// Copies a string into the specified section of an initialization
        /// file.
        /// </summary>
        /// <param name="lpSectionName">The name of the section to which the
        /// string will be copied. If  the section does not exist, it is
        /// created. The name of the section is case-independent;  the string
        /// can be any combination of uppercase and lowercase letters.</param>
        /// <param name="lpKeyName">The name of the key to be associated with a
        /// string. If the key  does not exist in the specified section, it is
        /// created. If this parameter is NULL, the  entire section, including
        /// all entries within the section, is deleted.</param>
        /// <param name="valueString">A <c>null</c>-terminated string to be
        /// written to the file. If this  parameter is NULL, the key pointed to
        /// by the <paramref name="lpKeyName"/> parameter is deleted.</param>
        /// <param name="iniFileName">The name of the initialization file.
        /// </param>
        /// <returns>
        /// If the function successfully copies the string to the initialization
        /// file, the return  value is nonzero.If the function fails, or if it
        /// flushes the cached version of the most  recently accessed
        /// initialization file, the return value is zero.
        /// </returns>
        /// <remarks>
        /// If the <paramref name="iniFileName"/> parameter does not contain a
        /// full path and file name for the file,  WritePrivateProfileString
        /// searches the Windows directory for the file. If the file does  not
        /// exist, this function creates the file in the Windows directory. If
        /// <paramref name="iniFileName"/> contains a full path and file name
        /// and the file does not exist,  WritePrivateProfileString creates the
        /// file. The specified directory must already exist. The system keeps a
        /// cached version of the most recent registry file mapping to improve
        /// performance. If <paramref name="lpSectionName"/>,
        /// <paramref name="lpKeyName"/>, and <paramref name="valueString"/>
        /// parameters are NULL, the function  flushes the cache. While the
        /// system is editing the cached version of the file, processes  that
        /// edit the file itself will use the original file until the cache has
        /// been cleared.
        /// </remarks>
        [DllImport(@"kernel32.dll", EntryPoint = @"WritePrivateProfileStringA", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
        private static extern int WritePrivateProfileString(
            string lpSectionName,
            string lpKeyName,
            string valueString,
            string iniFileName);

        #endregion Dll Kernel Ansi

        #region Dll Kernel Unicode

        [DllImport(@"kernel32.dll", EntryPoint = @"WritePrivateProfileStringW", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
        private static extern int WritePrivateProfileStringUnicode(
            string iniSectionName,
            string iniKeyName,
            string valueString,
            string iniFileName);

        #endregion Dll Kernel Unicode
    }
}