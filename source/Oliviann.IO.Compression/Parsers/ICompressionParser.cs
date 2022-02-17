namespace Oliviann.IO.Compression
{
    #region Usings

    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using ComponentModel;

    #endregion Usings

    /// <summary>
    /// Represents an interface for implementing a compression output parser.
    /// </summary>
    internal interface ICompressionParser
    {
        #region Events

        /// <summary>
        /// Occurs when file extraction has started.
        /// </summary>
        event EventHandler<EventArgs<string>> FileStarted;

        #endregion Events

        #region Properties

        /// <summary>
        /// Gets the collection of archive files.
        /// </summary>
        IList<ArchiveFileInfo> ArchiveFiles { get; }

        /// <summary>
        /// Sets the arguments for sharing with the parser.
        /// </summary>
        /// <value>
        /// The arguments to be shared with the parser.
        /// </value>
        CompressionArguments Arguments { set; }

        /// <summary>
        /// Gets or sets the exception relay delegate to relay the exception
        /// back to the main thread.
        /// </summary>
        /// <value>
        /// The exception relay delegate to relay the exception back to the main
        /// thread.
        /// </value>
        Action<Exception> ExceptionRelay { get; set; }

        #endregion Properties

        /// <summary>
        /// Handles the OutDateReceived event of an external
        /// <see cref="Process"/>.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The
        /// <see cref="System.Diagnostics.DataReceivedEventArgs"/> instance
        /// containing the event data.</param>
        void OutDataReceivedParser(object sender, DataReceivedEventArgs e);
    }
}