namespace Oliviann.IO.Compression
{
    /// <summary>
    /// Instructs the 7zip library the action to be performed.
    /// </summary>
    public enum CompressionCommand
    {
        /// <summary>
        /// Adds files to archive.
        /// </summary>
        Add,

        /// <summary>
        /// Measures speed of the CPU and checks RAM for errors.
        /// </summary>
        Benchmark,

        /// <summary>
        /// Deletes files from archive.
        /// </summary>
        Delete,

        /// <summary>
        /// Extracts files from an archive to the current directory or to the
        /// output directory.
        /// </summary>
        Extract,

        /// <summary>
        /// Lists contents of archive.
        /// </summary>
        List,

        /// <summary>
        /// Tests archive files.
        /// </summary>
        Test,

        /// <summary>
        /// Update older files in the archive and add files that are not already
        /// in the archive.
        /// </summary>
        /// <remarks>
        /// The current version of 7-Zip cannot change an archive which was
        /// created with the solid option switched on.
        /// </remarks>
        Update,

        /// <summary>
        /// Extracts files from an archive with their full paths in the current
        /// directory, or in an output directory if specified.
        /// </summary>
        ExtractFullPaths
    }
}