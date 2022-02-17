namespace Oliviann.IO.Compression
{
    #region Usings

    using System;
    using System.Diagnostics;
    using ComponentModel;

    #endregion Usings

    /// <summary>
    /// Represents a parser for parsing data received by the standard out window
    /// when extracting archive data.
    /// </summary>
    internal class ExtractParser : BaseParser, ICompressionParser
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
        /// <exception cref="CompressionException"><c>CompressionException</c>.
        /// </exception>
        public new void OutDataReceivedParser(object sender, DataReceivedEventArgs e)
        {
            base.OutDataReceivedParser(sender, e);
            if (e.Data.IsNullOrEmpty())
            {
                return;
            }

            string line = e.Data;
            if (line.StartsWith("Processing archive", StringComparison.Ordinal))
            {
                // Info debugger message of what file is being processed.
                Debug.WriteLine(line, "SevenZipCompression2");
                return;
            }

            // Throw exception if password is incorrect.
            if (line.Contains("Data Error in encrypted file. Wrong password?"))
            {
                this.ExceptionRelay.InvokeSafe(new CompressionException(line));
                return;
            }

            // Process extracting line for 7zip versions below 16.04.
            if (line.StartsWith("Extracting  ", StringComparison.Ordinal))
            {
                this.FileStarted.RaiseEvent(this, new EventArgs<string>(line.Substring(12)));
                return;
            }

            // Process extracting line for 7zip version 16.04 and above.
            if (line.Length > 2 && line.StartsWith("- ", StringComparison.OrdinalIgnoreCase))
            {
                this.FileStarted.RaiseEvent(this, new EventArgs<string>(line.Substring(2)));
                return;
            }

            // Other lines seen:
            // 7-Zip (a) 9.22 beta  Copyright (c) 1999-2011 Igor Pavlov  2011-04-18
            // Skipping    [PATH]
            // Extracting  [PATH]
            // Folders: ##
            // Everything is Ok
            // Files: ##
            // Size:       ##
            // Compressed: ##
        }

        #endregion Methods
    }
}