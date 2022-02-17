namespace Oliviann.IO.Compression
{
    /// <summary>
    /// An enumeration type for the archive format types available.
    /// </summary>
    public enum CompressionFormat
    {
        /// <summary>
        /// The BZip2 compression type.
        /// </summary>
        BZip2,

        /// <summary>
        /// The GZip compression type.
        /// </summary>
        GZip,

        /// <summary>
        /// The 7zip compression type.
        /// </summary>
        SevenZip,

        /// <summary>
        /// The Tar compression type.
        /// </summary>
        Tar,

        /// <summary>
        /// The XZ compression type.
        /// </summary>
        XZ,

        /// <summary>
        /// The Win Zip compression type.
        /// </summary>
        Zip,
    }
}