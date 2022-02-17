namespace Oliviann.IO.Compression
{
    #region Usings

    using System;
    using System.Diagnostics;
    using ComponentModel;

    #endregion Usings

    /// <summary>
    /// Represents a parser for parsing data received by the standard out window
    /// when listing archive data using the "show technical info" attribute.
    /// </summary>
    internal class ListParser : BaseParser, ICompressionParser
    {
        #region Fields

        /// <summary>
        /// Tells the parser when to start parsing data.
        /// </summary>
        private bool beginParsing;

        /// <summary>
        /// Place holder for the current file archive being parsed and built.
        /// </summary>
        private ArchiveFileInfo currentArchive;

        #endregion Fields

        #region Events

#pragma warning disable CS0067

        /// <summary>
        /// NOT USED.
        /// </summary>
        public event EventHandler<EventArgs<string>> FileStarted;

#pragma warning restore CS0067

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
            if (line.StartsWith("Listing archive: ", StringComparison.Ordinal))
            {
                // Info debugger message of what file is being processed.
                Debug.WriteLine(line, "SevenZipCompression2");
                return;
            }

            if (line.StartsWith("----------", StringComparison.OrdinalIgnoreCase))
            {
                // Tells the parser it has passed the header data and can begin
                // parsing the actual file data.
                this.beginParsing = true;
                return;
            }

            if (!this.beginParsing)
            {
                return;
            }

            if (line.StartsWith("Path = ", StringComparison.Ordinal))
            {
                this.currentArchive = new ArchiveFileInfo { FileName = line.Substring(7) };
                return;
            }

            if (this.currentArchive == null)
            {
                return;
            }

            if (line.StartsWith("Size = ", StringComparison.Ordinal))
            {
                this.currentArchive.Size = line.Substring(7).ToUInt64();
                return;
            }

            if (line.StartsWith("Modified = ", StringComparison.Ordinal))
            {
                this.currentArchive.LastModifiedTime = line.Substring(11).ToDateTime(DateTime.MinValue);
                return;
            }

            if (line.StartsWith("Attributes = ", StringComparison.Ordinal))
            {
                if (line.Contains('D'))
                {
                    this.currentArchive.IsDirectory = true;
                }

                return;
            }

            if (this.currentArchive.IsDirectory)
            {
                // Going to ignore directories right now unless we decide we need them.
                ////this.ArchiveFiles.Add(this.currentArchive);
                return;
            }

            if (line.StartsWith("CRC = ", StringComparison.Ordinal))
            {
                // Directories don't have CRC.
                this.currentArchive.CRC = line.Substring(6);
                return;
            }

            if (line.StartsWith("Encrypted = ", StringComparison.Ordinal))
            {
                // Directories aren't encrypted unless headers are selected to
                // be encrypted.
                this.currentArchive.IsEncrypted = line.Substring(12).Contains('+');
                return;
            }

            if (line.StartsWith("Method = ", StringComparison.Ordinal))
            {
                // Directories don't have Methods.
                this.currentArchive.Method = line.Substring(9);
                return;
            }

            if (line.StartsWith("Block = ", StringComparison.Ordinal))
            {
                // Directories don't have Blocks.
                this.currentArchive.Block = line.Substring(8).ToUInt32();
                this.ArchiveFiles.Add(this.currentArchive);
                return;
            }

            // Other lines seen:
            // 7-Zip (a) 9.22 beta  Copyright (c) 1999-2011 Igor Pavlov  2011-04-18
            // --
            // Packed Size = ##
            // Enter password (will not be echoed):
            // Error: C:\Users\svcfwbversantdba\AppData\Local\Temp\20e02bdz\IMIS_5_1.USAF.C130.2012.5.4_Full_C13002_12_1.idu: Can't allocate required memory
            // Errors: 1
        }

        #endregion Methods
    }
}