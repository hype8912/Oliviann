namespace Oliviann.IO.Compression
{
    #region Usings

    using System;

    #endregion Usings

    /// <summary>
    /// Represents an archive file object.
    /// </summary>
    public class ArchiveFileInfo
    {
        #region Properties

        /// <summary>
        /// Gets or sets the name of the file.
        /// </summary>
        /// <value>
        /// The name of the file.
        /// </value>
        public string FileName { get; set; }

        /// <summary>
        /// Gets or sets the size of the file.
        /// </summary>
        /// <value>
        /// The size of the file.
        /// </value>
        public ulong Size { get; set; }

        /// <summary>
        /// Gets or sets the last modified time.
        /// </summary>
        /// <value>
        /// The last modified time.
        /// </value>
        public DateTime LastModifiedTime { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is a
        /// directory.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is a directory; otherwise, <c>false</c>
        /// .
        /// </value>
        public bool IsDirectory { get; set; }

        /// <summary>
        /// Gets or sets the calculated checksum.
        /// </summary>
        /// <value>
        /// The calculated checksum.
        /// </value>
        public string CRC { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is encrypted.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is encrypted; otherwise, <c>false</c>.
        /// </value>
        public bool IsEncrypted { get; set; }

        /// <summary>
        /// Gets or sets the compression method.
        /// </summary>
        /// <value>
        /// The compression method.
        /// </value>
        public string Method { get; set; }

        /// <summary>
        /// Gets or sets the block this file is in.
        /// </summary>
        /// <value>
        /// The block this file is in.
        /// </value>
        public uint Block { get; set; }

        #endregion Properties
    }
}