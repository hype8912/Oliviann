namespace Oliviann.IO.Compression
{
    /// <summary>
    /// Specifies the overwrite mode during extraction, to overwrite files
    /// already present on disk.
    /// </summary>
    public enum CompressionOverwriteMode
    {
        /// <summary>
        /// Overwrite ALL existing files without prompt.
        /// </summary>
        Overwrite,

        /// <summary>
        /// Skip extracting of existing files.
        /// </summary>
        Skip,

        /// <summary>
        /// Auto rename extracting file (for example, name.txt will be renamed
        /// to name_1.txt).
        /// </summary>
        RenameExtractedFile,

        /// <summary>
        /// Auto rename existing file (for example, name.txt will be renamed to
        /// name_1.txt).
        /// </summary>
        RenameExistingFile
    }
}