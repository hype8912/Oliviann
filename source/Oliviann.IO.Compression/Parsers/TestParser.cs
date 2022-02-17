namespace Oliviann.IO.Compression
{
    #region Usings

    using System;
    using System.Diagnostics;
    using ComponentModel;
    using Properties;

    #endregion Usings

    /// <summary>
    /// Represents a parser for parsing data received by the standard out window
    /// when testing archive data.
    /// </summary>
    internal class TestParser : BaseParser, ICompressionParser
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
        /// <exception cref="CompressionException">Thrown for incorrect archive
        /// password being provided.</exception>
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
            // This message is throw when the file headers have not been encrypted.
            if (line.Contains(" in encrypted file. Wrong password?"))
            {
                var ex = new CompressionException(Resources.TestParser_EncryptedFileError, new UnauthorizedAccessException(line));
                this.ExceptionRelay.InvokeSafe(ex);
                return;
            }

            // Throw exception if password is incorrect with encrypted file headers.
            if (line.Contains(" encrypted archive. Wrong password?"))
            {
                var ex = new CompressionException(Resources.TestParser_EncryptedFileError, new UnauthorizedAccessException(line));
                this.ExceptionRelay.InvokeSafe(ex);
                return;
            }

            if (line.StartsWith("Testing     ", StringComparison.Ordinal))
            {
                this.FileStarted.RaiseEvent(this, new EventArgs<string>(line.Substring(12)));
                return;
            }

            // Other lines seen:
            // Sub items Errors: ##
            // Data Error in encrypted file. Wrong password?
            // CRC Failed in encrypted file. Wrong password?
        }

        #endregion Methods
    }
}