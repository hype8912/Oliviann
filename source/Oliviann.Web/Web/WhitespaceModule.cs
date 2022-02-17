#if NETFRAMEWORK

namespace Oliviann.Web
{
    #region Usings

    using System;
    using System.Configuration;
    using System.IO;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Web;

    #endregion Usings

    /// <summary>
    /// Represents a module for removing the whitespace from HTML data before
    /// it's sent to the client.
    /// </summary>
    public sealed class WhitespaceModule : BaseHttpModule
    {
        #region Properties

        /// <summary>
        /// Gets a value indicating whether remove the whitespace from the HTML
        /// data.
        /// </summary>
        /// <value>
        /// True if whitespace is to be removed from HTML data; otherwise,
        /// false.
        /// </value>
        /// <remarks>To use the WhiteSpace module, you need to add a setting to
        /// <c>WebConfigAppSettings</c> section named "RemoveHtmlWhiteSpace" and
        /// set the value to true or false.</remarks>
        public static bool RemoveHtmlWhiteSpace => Convert.ToBoolean(ConfigurationManager.AppSettings["RemoveHtmlWhiteSpace"]);

        #endregion Properties

        #region Methods

        /// <summary>
        /// Called when the begin request event is raised.
        /// </summary>
        /// <param name="context">The http base context.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the
        /// event data.</param>
        public override void OnBeginRequest(HttpContextBase context, EventArgs e)
        {
            if (context == null || context.Request.Headers["X-MicrosoftAjax"] == "Delta=true")
            {
                return;
            }

            if (context.Request.RawUrl.Contains(".aspx") && RemoveHtmlWhiteSpace)
            {
                context.Response.Filter = new WhitespaceFilter(context.Response.Filter);
            }
        }

        #endregion Methods

        #region Classes

        /// <summary>
        /// Represents a class for re-writing to stream.
        /// </summary>
        private class WhitespaceFilter : Stream
        {
            #region Fields

            /// <summary>
            /// Regular expression string for removing just about everything but
            /// sometimes too aggressive.
            /// </summary>
            private static readonly Regex Reg0 = new(@"(?<=[^])\t{2,}|(?<=[>])\s{2,}(?=[<])|(?<=[>])\s{2,11}(?=[<])|(?=[\n])\s{2,}");

            /// <summary>
            /// Regular expression string for removing white space between tags.
            /// </summary>
            private static readonly Regex Reg1 = new(@">\s+<", RegexOptions.Compiled);

            /// <summary>
            /// Regular expression string for removing multiple returns.
            /// </summary>
            private static readonly Regex Reg2 = new(@"\n\s+", RegexOptions.Compiled);

            /// <summary>
            /// Regular expression string for removing returns after
            /// semi-colons.
            /// </summary>
            private static readonly Regex Reg3 = new(@";\s+", RegexOptions.Compiled);

            /// <summary>
            /// Regular expression string for removing returns after close
            /// bracket.
            /// </summary>
            private static readonly Regex Reg4 = new(@"}\s+", RegexOptions.Compiled);

            /// <summary>
            /// Regular expression string for removing returns after open
            /// bracket.
            /// </summary>
            private static readonly Regex Reg5 = new(@"{\s+", RegexOptions.Compiled);

            /// <summary>
            /// Placeholder for stream as read-only.
            /// </summary>
            private readonly Stream toSink;

            #endregion Fields

            #region Constructor/Destructor

            /// <summary>
            /// Initializes a new instance of the <see cref="WhitespaceFilter"/>
            /// class.
            /// </summary>
            /// <param name="sink">The sink stream.</param>
            public WhitespaceFilter(Stream sink) => this.toSink = sink;

            #endregion Constructor/Destructor

            #region Properites

            /// <summary>
            /// Gets a value indicating whether the current stream supports
            /// reading, when overridden in a derived class.
            /// </summary>
            /// <value></value>
            /// <returns><c>true</c> if the stream supports reading; otherwise,
            /// <c>false</c>.
            /// </returns>
            public override bool CanRead => true;

            /// <summary>
            /// Gets a value indicating whether the current stream supports
            /// seeking, when overridden in a derived class.
            /// </summary>
            /// <value></value>
            /// <returns><c>true</c> if the stream supports seeking; otherwise,
            /// <c>false</c>.
            /// </returns>
            public override bool CanSeek => true;

            /// <summary>
            /// Gets a value indicating whether the current stream supports
            /// writing, when overridden in a derived class.
            /// </summary>
            /// <value></value>
            /// <returns><c>true</c> if the stream supports writing; otherwise,
            /// <c>false</c>.
            /// </returns>
            public override bool CanWrite => true;

            /// <summary>
            /// Gets the length in bytes of the stream, when overridden in a
            /// derived class.
            /// </summary>
            /// <value></value>
            /// <returns>
            /// A long value representing the length of the stream in bytes.
            /// </returns>
            /// <exception cref="T:System.NotSupportedException">
            /// A class derived from Stream does not support seeking.
            /// </exception>
            /// <exception cref="T:System.ObjectDisposedException">
            /// Methods were called after the stream was closed.
            /// </exception>
            public override long Length => 0;

            /// <summary>
            /// Gets or sets the position within the current stream, when
            /// overridden in a derived class.
            /// </summary>
            /// <value></value>
            /// <returns>
            /// The current position within the stream.
            /// </returns>
            /// <exception cref="T:System.IO.IOException">
            /// An I/O error occurs.
            /// </exception>
            /// <exception cref="T:System.NotSupportedException">
            /// The stream does not support seeking.
            /// </exception>
            /// <exception cref="T:System.ObjectDisposedException">
            /// Methods were called after the stream was closed.
            /// </exception>
            public override long Position { get; set; }

            #endregion Properites

            #region Methods

            /// <summary>
            /// When overridden in a derived class, clears all buffers for this
            /// stream and causes any buffered data to be written to the
            /// underlying device.
            /// </summary>
            /// <exception cref="T:System.IO.IOException">
            /// An I/O error occurs.
            /// </exception>
            public override void Flush() => this.toSink.Flush();

            /// <summary>
            /// When overridden in a derived class, reads a sequence of bytes
            /// from the current stream and advances the position within the
            /// stream by the number of bytes read.
            /// </summary>
            /// <param name="buffer">An array of bytes. When this method
            /// returns, the buffer contains the specified byte array with the
            /// values between
            /// <paramref name="offset"/> and (<paramref
            /// name="offset"/> + <paramref name="count"/> - 1) replaced by the
            /// bytes read from the current source.</param>
            /// <param name="offset">The zero-based byte offset in
            /// <paramref name="buffer"/> at which to begin storing the data
            /// read from the current stream.</param>
            /// <param name="count">The maximum number of bytes to be read from
            /// the current stream.</param>
            /// <returns>
            /// The total number of bytes read into the buffer. This can be less
            /// than the number of bytes requested if that many bytes are not
            /// currently available, or zero (0) if the end of the stream has
            /// been reached.
            /// </returns>
            /// <exception cref="T:System.ArgumentException">
            /// The sum of <paramref name="offset"/> and
            /// <paramref name="count"/> is larger than the buffer length.
            /// </exception>
            /// <exception cref="T:System.ArgumentNullException">
            ///     <paramref name="buffer"/> is <c>null</c>.
            /// </exception>
            /// <exception cref="T:System.ArgumentOutOfRangeException">
            ///     <paramref name="offset"/> or <paramref name="count"/> is
            ///     negative.
            /// </exception>
            /// <exception cref="T:System.IO.IOException">
            /// An I/O error occurs.
            /// </exception>
            /// <exception cref="T:System.NotSupportedException">
            /// The stream does not support reading.
            /// </exception>
            /// <exception cref="T:System.ObjectDisposedException">
            /// Methods were called after the stream was closed.
            /// </exception>
            public override int Read(byte[] buffer, int offset, int count) => this.toSink.Read(buffer, offset, count);

            /// <summary>
            /// When overridden in a derived class, sets the position within the
            /// current stream.
            /// </summary>
            /// <param name="offset">A byte offset relative to the
            /// <paramref name="origin"/> parameter.</param>
            /// <param name="origin">A value of type
            /// <see cref="T:System.IO.SeekOrigin"/> indicating the reference
            /// point used to obtain the new position.</param>
            /// <returns>
            /// The new position within the current stream.
            /// </returns>
            /// <exception cref="T:System.IO.IOException">
            /// An I/O error occurs.
            /// </exception>
            /// <exception cref="T:System.NotSupportedException">
            /// The stream does not support seeking, such as if the stream is
            /// constructed from a pipe or console output.
            /// </exception>
            /// <exception cref="T:System.ObjectDisposedException">
            /// Methods were called after the stream was closed.
            /// </exception>
            public override long Seek(long offset, SeekOrigin origin) => this.toSink.Seek(offset, origin);

            /// <summary>
            /// When overridden in a derived class, sets the length of the
            /// current stream.
            /// </summary>
            /// <param name="value">The desired length of the current stream in
            /// bytes.</param>
            /// <exception cref="T:System.IO.IOException">
            /// An I/O error occurs.
            /// </exception>
            /// <exception cref="T:System.NotSupportedException">
            /// The stream does not support both writing and seeking, such as if
            /// the stream is constructed from a pipe or console output.
            /// </exception>
            /// <exception cref="T:System.ObjectDisposedException">
            /// Methods were called after the stream was closed.
            /// </exception>
            public override void SetLength(long value) => this.toSink.SetLength(value);

            /// <summary>
            /// Closes the current stream and releases any resources (such as
            /// sockets and file handles) associated with the current stream.
            /// </summary>
            public override void Close()
            {
                this.toSink.Close();
                base.Close();
            }

            /// <summary>
            /// When overridden in a derived class, writes a sequence of bytes
            /// to the current stream and advances the current position within
            /// this stream by the number of bytes written.
            /// </summary>
            /// <param name="buffer">An array of bytes. This method copies
            /// <paramref name="count"/> bytes from
            /// <paramref name="buffer"/> to the current stream.</param>
            /// <param name="offset">The zero-based byte offset in
            /// <paramref name="buffer"/> at which to begin copying bytes to the
            /// current stream.</param>
            /// <param name="count">The number of bytes to be written to the
            /// current stream.</param>
            /// <exception cref="T:System.ArgumentException">
            /// The sum of <paramref name="offset"/> and
            /// <paramref name="count"/> is greater than the buffer length.
            /// </exception>
            /// <exception cref="T:System.ArgumentNullException">
            ///     <paramref name="buffer"/> is <c>null</c>.
            /// </exception>
            /// <exception cref="T:System.ArgumentOutOfRangeException">
            ///     <paramref name="offset"/> or <paramref name="count"/> is
            ///     negative.
            /// </exception>
            /// <exception cref="T:System.IO.IOException">
            /// An I/O error occurs.
            /// </exception>
            /// <exception cref="T:System.NotSupportedException">
            /// The stream does not support writing.
            /// </exception>
            /// <exception cref="T:System.ObjectDisposedException">
            /// Methods were called after the stream was closed.
            /// </exception>
            public override void Write(byte[] buffer, int offset, int count)
            {
                string html = Encoding.Default.GetString(buffer, offset, count);

                html = RemoveWhiteSpace(html);

                byte[] outdata = Encoding.Default.GetBytes(html);
                this.toSink.Write(outdata, 0, outdata.GetLength(0));
            }

            #endregion Methods

            #region Page Optimization

            /// <summary>
            /// Removes the white space.
            /// </summary>
            /// <param name="html">The dirty html text.</param>
            /// <returns>Returns clean html.</returns>
            private static string RemoveWhiteSpace(string html)
            {
                ////html = Reg0.Replace(html, string.Empty);
                html = Reg1.Replace(html, "><");
                html = Reg2.Replace(html, string.Empty);
                html = Reg3.Replace(html, "; ");
                ////html = Reg4.Replace(html, "} ");
                html = Reg5.Replace(html, "{ ");

                return html;
            }

            #endregion Page Optimization
        }

        #endregion Classes
    }
}

#endif