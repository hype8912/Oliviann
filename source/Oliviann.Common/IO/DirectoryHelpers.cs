namespace Oliviann.IO
{
    #region Usings

    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
#if NETFRAMEWORK || NETCOREAPP3_0
    using Microsoft.VisualBasic.FileIO;
#endif

    #endregion Usings

    /// <summary>
    /// Represents a collection of commonly used helper methods for making
    /// working with <see cref="Directory"/> easier.
    /// </summary>
    public static class DirectoryHelpers
    {
        /// <summary>
        /// Determines whether the given <paramref name="directories"/> refer to
        /// an existing directory on disk.
        /// </summary>
        /// <param name="directories">The paths to test.</param>
        /// <returns><c>True</c> if all <paramref name="directories"/> refer to
        /// an existing directory; otherwise, <c>false</c>.</returns>
        public static bool Exists(IEnumerable<string> directories)
        {
            ADP.CheckArgumentNull(directories, nameof(directories));
            return directories.All(Directory.Exists);
        }

        /// <summary>
        /// Determines whether the given <paramref name="directories"/> refer to
        /// an existing directory on disk.
        /// </summary>
        /// <param name="directories">The paths to test.</param>
        /// <returns><c>True</c> if all <paramref name="directories"/> refer to
        /// an existing directory; otherwise, <c>false</c>.</returns>
        public static bool Exists(params string[] directories) => Exists(directories.AsEnumerable());

#if NETFRAMEWORK || NETCOREAPP3_0

        /// <summary>
        /// Moves the contents of the source directory to the destination
        /// directory.
        /// </summary>
        /// <param name="sourceDirectoryPath">The source directory path to be
        /// moved.</param>
        /// <param name="destinationDirectoryPath">>The location to which the
        /// directory contents should be moved.</param>
        /// <param name="overwrite">True to overwrite existing files and
        /// directories; otherwise false. Default is false.</param>
        public static void Move(string sourceDirectoryPath, string destinationDirectoryPath, bool overwrite = false)
        {
            ADP.CheckArgumentNullOrEmpty(sourceDirectoryPath, nameof(sourceDirectoryPath));
            ADP.CheckArgumentNullOrEmpty(destinationDirectoryPath, nameof(destinationDirectoryPath));

            if (!Directory.Exists(destinationDirectoryPath))
            {
                Directory.CreateDirectory(destinationDirectoryPath);
            }

            var destinationRoot = new DirectoryInfo(destinationDirectoryPath).Root;
            var sourceRoot = new DirectoryInfo(sourceDirectoryPath).Root;
            if (sourceRoot.Name == destinationRoot.Name)
            {
                if (overwrite)
                {
                    Directory.Delete(destinationDirectoryPath, true);
                }

                Directory.Move(sourceDirectoryPath, destinationDirectoryPath);
            }
            else
            {
                FileSystem.CopyDirectory(sourceDirectoryPath, destinationDirectoryPath, overwrite);
                Directory.Delete(sourceDirectoryPath, true);
            }
        }

        /// <summary>
        /// Copies the contents of the source directory to the destination
        /// directory.
        /// </summary>
        /// <param name="sourceDirectoryPath">The source directory path to be
        /// copied.
        /// </param>
        /// <param name="destinationDirectoryPath">The location to which the
        /// directory contents should be copied.</param>
        /// <param name="overwrite">True to overwrite existing files; otherwise
        /// false. Default is false.</param>
        public static void Copy(string sourceDirectoryPath, string destinationDirectoryPath, bool overwrite = false)
        {
            ADP.CheckArgumentNullOrEmpty(sourceDirectoryPath, nameof(sourceDirectoryPath));
            ADP.CheckArgumentNullOrEmpty(destinationDirectoryPath, nameof(destinationDirectoryPath));
            FileSystem.CopyDirectory(sourceDirectoryPath, destinationDirectoryPath, overwrite);
        }

#endif
    }
}