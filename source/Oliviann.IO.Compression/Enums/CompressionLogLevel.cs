namespace Oliviann.IO.Compression
{
    /// <summary>
    /// Defines the level to log the output of the 7zip library.
    /// </summary>
    public enum CompressionLogLevel
    {
        /// <summary>
        /// Disable log (default).
        /// </summary>
        Disable = 0,

        /// <summary>
        /// Show names of processed files in log.
        /// </summary>
        Info = 1,

        /// <summary>
        /// Show names of additional files that were processed internally in
        /// solid archives: skipped files for "Extract" operation, repacked
        /// files for "Add" / "Update" operations.
        /// </summary>
        Detailed = 2,

        /// <summary>
        /// Show information about additional operations (Analyze, Replicate)
        /// for "Add" / "Update" operations.
        /// </summary>
        Verabose = 3
    }
}