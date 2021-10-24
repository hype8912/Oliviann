namespace Oliviann.IO
{
    #region Usings

    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;

    #endregion Usings

    /// <summary>
    /// Represents a collection of commonly used helper methods for making
    /// working with <see cref="File"/> easier.
    /// </summary>
    public static class FileHelpers
    {
        /// <summary>
        /// Determines whether the specified file name string contains any
        /// invalid characters.
        /// </summary>
        /// <param name="fileName">Name of the file. With or without file
        /// extension.</param>
        /// <returns>True if the file name contains any invalid characters;
        /// otherwise, false.</returns>
        public static bool ContainsInvalidFileNameChars(string fileName)
        {
            ADP.CheckArgumentNullOrEmpty(fileName, nameof(fileName));
            return fileName.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0;
        }

        /// <summary>
        /// Determines whether all the specified files exist.
        /// </summary>
        /// <param name="files">The collection of files to check.</param>
        /// <returns>True if the caller has the required permissions and all
        /// <paramref name="files"/> contain the name of an existing file;
        /// otherwise, false.</returns>
        /// <remarks>This method will return false if any of the files in the
        /// collection do not exist.</remarks>
        public static bool Exists(IEnumerable<string> files)
        {
            ADP.CheckArgumentNull(files, nameof(files));
            return files.All(File.Exists);
        }

        /// <summary>
        /// Determines whether all the specified files exist.
        /// </summary>
        /// <param name="files">The array of files to check.</param>
        /// <returns>True if the caller has the required permissions and all
        /// <paramref name="files"/> contain the name of an existing file;
        /// otherwise, false.</returns>
        /// <remarks>This method will return false if any of the files in the
        /// array do not exist.</remarks>
        public static bool Exists(params string[] files) => Exists(files.AsEnumerable());

        /// <summary>
        /// Determines whether the specified file path is locked for reading
        /// and/or writing.
        /// </summary>
        /// <param name="filePath">The fully qualified name of the new file, or
        /// the relative file name. Do not end the path with the directory
        /// separator character.</param>
        /// <returns>
        /// True if the specified file is locked or an error occurred trying to
        /// determine if the file was locked; otherwise, false.
        /// </returns>
        public static bool IsFileLocked(string filePath)
        {
            if (filePath.IsNullOrEmpty())
            {
                return false;
            }

            var info = new FileInfo(filePath);
            return info.Exists && info.IsFileLocked();
        }

        /// <summary>
        /// Moves the specified file to a new location. Move operation will
        /// create the destination directory if it does not exist.
        /// </summary>
        /// <param name="sourceFile">The path of the file to move.</param>
        /// <param name="destinationFile">The new path for the file.</param>
        /// <param name="overwrite">True if the destination file can be
        /// overwritten; otherwise, false.</param>
        public static void Move(string sourceFile, string destinationFile, bool overwrite = false)
        {
            ADP.CheckArgumentNullOrEmpty(sourceFile, nameof(sourceFile));
            ADP.CheckArgumentNullOrEmpty(destinationFile, nameof(destinationFile));

            var destinationDir = new DirectoryInfo(destinationFile).Parent;
            if (!destinationDir.Exists)
            {
                Debug.WriteLine(
                    "Creating Destination directory.{0}     PATH={1}".FormatWith(Environment.NewLine, destinationDir.FullName),
                    "FileHelpers");
                destinationDir.Create();
            }

            var sourceRoot = new DirectoryInfo(sourceFile).Root;
            var destinationRoot = new DirectoryInfo(destinationFile).Root;
            if (sourceRoot.Name == destinationRoot.Name)
            {
                if (overwrite)
                {
                    Debug.WriteLine(
                        "Deleting destination file.{0}     PATH={1}".FormatWith(Environment.NewLine, destinationFile),
                        "FileHelpers");
                    File.Delete(destinationFile);
                }

                Debug.WriteLine(
                    "Moving destination file.{0}     FROM={1}{0}     TO={2}".FormatWith(
                        Environment.NewLine,
                        sourceFile,
                        destinationFile),
                    "FileHelpers");
                File.Move(sourceFile, destinationFile);
            }
            else
            {
                Debug.WriteLine(
                    "Copying destination file.{0}     FROM={1}{0}     TO={2}{0}     OVERWRITE={3}".FormatWith(
                        Environment.NewLine,
                        sourceFile,
                        destinationFile,
                        overwrite),
                    "FileHelpers");
                File.Copy(sourceFile, destinationFile, overwrite);
            }
        }

        /// <summary>
        /// Reads the complete file as a binary byte array.
        /// </summary>
        /// <param name="filePath">The complete path to the file.</param>
        /// <returns>The complete file as a single byte array.</returns>
        public static byte[] ReadAsByteArray(string filePath)
        {
            ADP.CheckArgumentNull(filePath, nameof(filePath));
            if (!File.Exists(filePath))
            {
                Debug.WriteLine(@"File does not exist: {0}".FormatWith(filePath));
                return null;
            }

            return File.ReadAllBytes(filePath);
        }

        /// <summary>
        /// Reads the complete contents of a file at the specified
        /// <paramref name="filePath"/>.
        /// </summary>
        /// <param name="filePath">The full file path.</param>
        /// <returns>The contents of the file at the specified
        /// <paramref name="filePath"/> if the file exists on disk; otherwise,
        /// an empty string will be returned.</returns>
        /// <remarks>You may run out of memory if you try to read the complete
        /// contents of a extremely large file. Verify the directory path is of
        /// a safe area you want your application to access. Verify the file
        /// extension prior to calling this method against a white list.
        /// </remarks>
        public static string ReadContents(string filePath)
        {
            ADP.CheckArgumentNull(filePath, nameof(filePath));

            // NOTE: Per Veracode read out meeting. Was advised to verify path
            // doesn't have any illegal characters and name doesn't contain any
            // illegal characters.
            string filename = Path.GetFileName(filePath);

            if (!File.Exists(filePath))
            {
                Debug.WriteLine(@"File does not exist: {0}".FormatWith(filePath));
                return string.Empty;
            }

            string contents;
            using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                contents = stream.ReadToEnd();
            }

            return contents;
        }

#if NET35
        /// <summary>Reads the lines of a file.</summary>
        /// <param name="filePath">The file to read.</param>
        /// <returns>All the lines of the file, or the lines that are the result
        /// of a query.</returns>
        public static IEnumerable<string> ReadLines(string filePath)
        {
            ADP.CheckArgumentNull(filePath, nameof(filePath));

            using (TextReader reader = new StreamReader(filePath))
            {
                string currentLine;
                while ((currentLine = reader.ReadLine()) != null)
                {
                    yield return currentLine;
                }
            }
        }
#endif

        /// <summary>
        /// Converts the contents of the specified file path to its equivalent
        /// string representation that is encoded with base-64 digits.
        /// </summary>
        /// <param name="filePath">The file to open for reading.</param>
        /// <returns>The string representation, in base 64, of the contents of
        /// the specified file path.</returns>
        public static string ToBase64String(string filePath)
        {
            ADP.CheckArgumentNullOrEmpty(filePath, nameof(filePath));
            ADP.CheckFileNotFound(filePath);

            byte[] bytes = File.ReadAllBytes(filePath);
            return Convert.ToBase64String(bytes);
        }

        /// <summary>
        /// Converts the specified string, which encodes binary data as base-64
        /// digits, to an equivalent file.
        /// </summary>
        /// <param name="filePath">The file to write to.</param>
        /// <param name="data">The string to convert.</param>
        public static void FromBase64String(string filePath, string data)
        {
            ADP.CheckArgumentNullOrEmpty(filePath, nameof(filePath));
            if (data == null)
            {
                data = string.Empty;
            }

            byte[] bytes = Convert.FromBase64String(data);
            File.WriteAllBytes(filePath, bytes);
        }
    }
}