namespace Oliviann.IO
{
    #region Usings

    using System.Diagnostics;
    using System.IO;

    #endregion Usings

    /// <summary>
    /// Represents a collection of commonly used extension methods for extending
    /// <see cref="FileInfo"/>.
    /// </summary>
    public static class FileInfoExtensions
    {
        /// <summary>
        /// Determines whether the specified file information is locked for
        /// reading or writing.
        /// </summary>
        /// <param name="fileInformation">The file information object.</param>
        /// <returns>
        /// True if the specified file is locked; otherwise, false.
        /// </returns>
        public static bool IsFileLocked(this FileInfo fileInformation)
        {
            ADP.CheckArgumentNull(fileInformation, nameof(fileInformation));
            FileStream stream = null;

            try
            {
                stream = fileInformation.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            }
            catch (IOException)
            {
                return true;
            }
            finally
            {
                stream.DisposeSafe();
            }

            return false;
        }

        /// <summary>
        /// Deletes the specified read only file.
        /// </summary>
        /// <param name="fileInformation">The file information object.</param>
        public static void DeleteReadOnlyFile(this FileInfo fileInformation)
        {
            if (fileInformation == null || !fileInformation.Exists)
            {
                return;
            }

            if (fileInformation.IsReadOnly)
            {
                File.SetAttributes(fileInformation.FullName, FileAttributes.Normal);
            }

            fileInformation.Delete();
        }

        /// <summary>
        /// Returns the file name of the specified
        /// <paramref name="fileInformation"/> without the extension.
        /// </summary>
        /// <param name="fileInformation">The file information object.</param>
        /// <returns>
        /// The string returned by <see cref="FileInfo.Name"/>, minus the last
        /// period (.) and all the characters following it.
        /// </returns>
        [DebuggerStepThrough]
        public static string NameWithoutExtension(this FileInfo fileInformation)
        {
            ADP.CheckArgumentNull(fileInformation, nameof(fileInformation));
            return Path.GetFileNameWithoutExtension(fileInformation.Name);
        }
    }
}