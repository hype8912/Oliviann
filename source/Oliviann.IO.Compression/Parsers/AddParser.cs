namespace Oliviann.IO.Compression
{
    #region Usings

    using System;
    using System.Diagnostics;
    using ComponentModel;

    #endregion Usings

    /// <summary>
    /// Represents a parser for parsing data received by the standard out window
    /// when compressing archive data.
    /// </summary>
    internal class AddParser : BaseParser, ICompressionParser
    {
        #region Events

        /// <summary>
        /// Occurs when file extraction has started.
        /// </summary>
        public event EventHandler<EventArgs<string>> FileStarted;

        #endregion Events

        #region Methods

        /// <summary>
        /// Handles the OutDateReceived event of an external
        /// <see cref="Process"/>.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The
        /// <see cref="System.Diagnostics.DataReceivedEventArgs"/> instance
        /// containing the event data.</param>
        public new void OutDataReceivedParser(object sender, DataReceivedEventArgs e)
        {
            base.OutDataReceivedParser(sender, e);
            if (e.Data.IsNullOrEmpty())
            {
                return;
            }

            string line = e.Data;
            if (line.StartsWith("Creating archive", StringComparison.Ordinal))
            {
                // Info debugger message of what file is being created.
                Debug.WriteLine(line, "SevenZipCompression2");
                return;
            }

            if (line.StartsWith("Updating archive", StringComparison.Ordinal))
            {
                // Info debugger message of what file is being updated.
                Debug.WriteLine(line, "SevenZipCompression2");
                return;
            }

            if (line.StartsWith("Compressing  ", StringComparison.Ordinal))
            {
                this.FileStarted.RaiseEvent(this, new EventArgs<string>(line.Substring(13)));
                return;
            }

            // Throw exception if parameters are incorrect.
            if (line.Contains("The parameter is incorrect."))
            {
                this.ExceptionRelay.InvokeSafe(new CompressionException(line));
                return;
            }

            // Throw exception if command line is incorrect.
            if (line.Contains("Incorrect command line"))
            {
                this.ExceptionRelay.InvokeSafe(new CompressionException(line));
                return;
            }

            // Other lines seen:
            // 7-Zip [64] 9.22 beta  Copyright (c) 1999-2011 Igor Pavlov  2011-04-18
            // Scanning
            // System error:
            // Everything is Ok
        }

        #endregion Methods
    }
}