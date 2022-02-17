namespace Oliviann.IO.Compression
{
    #region Usings

    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    #endregion Usings

    /// <summary>
    /// Represents a base parser to help with implementing a compression parser.
    /// </summary>
    internal abstract class BaseParser
    {
        #region Constructor/Destructor

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseParser"/> class.
        /// </summary>
        internal BaseParser()
        {
            this.ArchiveFiles = new List<ArchiveFileInfo>();
            this.Arguments = new CompressionArguments();
        }

        #endregion Constructor/Destructor

        #region Properties

        /// <summary>
        /// Gets the collection of archive files.
        /// </summary>
        public IList<ArchiveFileInfo> ArchiveFiles { get; private set; }

        /// <summary>
        /// Gets or sets the arguments for sharing with the parser.
        /// </summary>
        /// <value>
        /// The arguments to be shared with the parser.
        /// </value>
        public CompressionArguments Arguments { internal get; set; }

        /// <summary>
        /// Gets or sets the exception relay delegate to relay the exception
        /// back to the main thread.
        /// </summary>
        /// <value>
        /// The exception relay delegate to relay the exception back to the main
        /// thread.
        /// </value>
        public Action<Exception> ExceptionRelay { get; set; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Handles the OutDateReceived event of an external
        /// <see cref="Process"/>.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The
        /// <see cref="System.Diagnostics.DataReceivedEventArgs"/> instance
        /// containing the event data.</param>
        public virtual void OutDataReceivedParser(object sender, DataReceivedEventArgs e)
        {
            if (this.Arguments != null && this.Arguments.TraceOutputData)
            {
                Trace.WriteLine(e.Data, "BaseParser");
            }
        }

        #endregion Methods
    }
}