namespace Oliviann.IO
{
    #region Usings

    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;

    #endregion Usings

    /// <summary>
    /// Represents a line reader for reading a data source line by line.
    /// </summary>
    public class LineReader : IEnumerable<string>
    {
        #region Fields

        /// <summary>
        /// The shared text reader instance.
        /// </summary>
        private readonly Func<TextReader> source;

        #endregion Fields

        #region Constructor/Destructor

        /// <summary>
        /// Initializes a new instance of the <see cref="LineReader"/> class
        /// using the specified file path and UTF8 encoding.
        /// </summary>
        /// <param name="filePath">The complete file path to be read.</param>
        public LineReader(string filePath) : this(filePath, Encoding.UTF8)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LineReader"/> class
        /// using the specified file path and encoding.
        /// </summary>
        /// <param name="filePath">The complete file path to be read.</param>
        /// <param name="encoding">The character encoding to use.</param>
        public LineReader(string filePath, Encoding encoding) : this(GetReader(filePath, encoding))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LineReader"/> class
        /// using the specified source.
        /// </summary>
        /// <param name="source">The data source.</param>
        public LineReader(Func<TextReader> source) => this.source = source;

        #endregion Constructor/Destructor

        #region Methods

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1" /> that can
        /// be used to iterate through the collection.
        /// </returns>
        /// <exception cref="ArgumentNullException">Source cannot be null.
        /// </exception>
        public IEnumerator<string> GetEnumerator()
        {
            ADP.CheckArgumentNull(this.source, "source");
            using (TextReader reader = this.source())
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    yield return line;
                }
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator" /> object that can
        /// be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

        /// <summary>
        /// Gets a reader for the data source.
        /// </summary>
        /// <param name="filePath">The complete file path to be read.</param>
        /// <param name="encoding">The character encoding to use.</param>
        /// <returns>The data source function.</returns>
        private static Func<TextReader> GetReader(string filePath, Encoding encoding)
        {
#if NETSTANDARD1_3
            return () =>
            {
                var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, FileOptions.SequentialScan);
                return new StreamReader(stream, encoding, true, 1024, false);
            };
#else
            return () => new StreamReader(filePath, encoding);
#endif
        }

        #endregion Methods
    }
}