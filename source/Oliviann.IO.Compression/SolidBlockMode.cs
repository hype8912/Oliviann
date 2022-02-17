namespace Oliviann.IO.Compression
{
    #region Using

    using System;

    #endregion Using

    /// <summary>
    /// Represents a class model for the sold block parameters.
    /// </summary>
    [Serializable]
    public class SolidBlockMode
    {
        #region Constructor/Destructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SolidBlockMode" />
        /// class.
        /// </summary>
        internal SolidBlockMode()
        {
            this.Enabled = true;
            this.UseBlockPerExtension = false;
        }

        #endregion Constructor/Destructor

        /// <summary>
        /// Gets or sets a value indicating whether the use of solid block
        /// mode is enabled. The default value is true.
        /// </summary>
        /// <value>
        /// true if enabled; otherwise, false.
        /// </value>
        /// <example>
        /// off | on
        /// </example>
        public bool Enabled { get; set; }

        /// <summary>
        /// Gets or sets limit for the number of files in one solid block.
        /// </summary>
        /// <value>
        /// The limit for the number of files in one solid block.
        /// </value>
        /// <example>
        /// <c>{N}f</c>
        /// </example>
        public int BlockFileCountLimit { get; set; }

        /// <summary>
        /// Gets or sets the limit for the total size of a solid block in
        /// bytes.
        /// </summary>
        /// <value>
        /// The block file size limit.
        /// </value>
        /// <example>
        /// {N}b | {N}k | {N}m | {N}g
        /// </example>
        public FileSize BlockSizeLimit { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to use a separate solid
        /// block for each new file extension.
        /// </summary>
        /// <value>
        /// true if to use a separate solid block for each new file
        /// extension; otherwise, false.
        /// </value>
        public bool UseBlockPerExtension { get; set; }
    }
}