namespace Oliviann.IO
{
    #region Usings

    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Threading;

    #endregion Usings

    /// <summary>
    /// Represents a collection of commonly used extension methods for extending
    /// <see cref="DirectoryInfo"/>.
    /// </summary>
    public static class DirectoryInfoExtensions
    {
        /// <summary>
        /// Determines whether the specified directory info is a reparse point
        /// or junction.
        /// </summary>
        /// <param name="directory">The directory info to be checked.</param>
        /// <returns>True if the specified directory is a reparse point;
        /// otherwise, false.</returns>
        public static bool IsReparsePoint(this DirectoryInfo directory)
        {
            ADP.CheckArgumentNull(directory, nameof(directory));

            FileAttributes attributes = File.GetAttributes(directory.FullName);
            return (attributes & FileAttributes.ReparsePoint) == FileAttributes.ReparsePoint;
        }

        /// <summary>
        /// Calculates the total size of the specified directory.
        /// </summary>
        /// <param name="directory">The directory info to be searched.</param>
        /// <param name="includeSubdirectories">Includes all sub-directories if
        /// set to True; otherwise, just top level directory.</param>
        /// <returns>The total size of the specified directory info object.
        /// </returns>
        /// <remarks>In .NET 4.5 this function calculates 62 GB of directories
        /// and subdirectories in about 4 seconds.</remarks>
        public static long Size(this DirectoryInfo directory, bool includeSubdirectories = true)
        {
            if (directory.IsReparsePoint())
            {
                return 0;
            }

            long totalSize = 0;
            try
            {
                // Calculates total size of files in the current directory.
#if NET35
                long filesTotal = directory.GetFiles().Sum(f => f.Length);
#else
                long filesTotal = directory.EnumerateFiles().Sum(f => f.Length);
#endif
                Interlocked.Add(ref totalSize, filesTotal);

                if (!includeSubdirectories)
                {
                    // Exits if sub-directories shouldn't be included.
                    return totalSize;
                }

                // Gets the list of sub-directories in the parent directory.
                DirectoryInfo[] directories = directory.GetDirectories();

                // Loops through the sub-directories in parallel calculating the
                // total size of files.
                System.Threading.Tasks.Parallel.For<long>(
                    0,
                    directories.Length,
                    () => 0,
                    (arrayIndex, loopState, directoriesSubTotal) =>
                        {
                            directoriesSubTotal += Size(directories[arrayIndex]);
                            return directoriesSubTotal;
                        },
                    directoriesTotal => Interlocked.Add(ref totalSize, directoriesTotal));
            }
            catch (UnauthorizedAccessException ex)
            {
                Trace.TraceError(ex.Message);
            }

            return totalSize;
        }
    }
}