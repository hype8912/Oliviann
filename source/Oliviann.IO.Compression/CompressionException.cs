namespace Oliviann.IO.Compression
{
    #region Usings

    using System;

    #endregion Usings

    /// <summary>
    /// Represents errors that occur during compression.
    /// </summary>
    [Serializable]
    public class CompressionException : Exception
    {
        #region Constructor/Destructor

        /// <summary>
        /// Initializes a new instance of the <see cref="CompressionException"/>
        /// class.
        /// </summary>
        public CompressionException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CompressionException"/>
        /// class with a specified error message.
        /// </summary>
        /// <param name="message">The error message that explains the reason for
        /// the exception.</param>
        public CompressionException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CompressionException"/>
        /// class with a specified error message and a reference to the inner
        /// exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for
        /// the exception.</param>
        /// <param name="inner">The exception that is the cause of the current
        /// exception, or a <c>null</c> reference (Nothing in Visual Basic) if
        /// no inner exception is specified.</param>
        public CompressionException(string message, Exception inner) : base(message, inner)
        {
        }

        #endregion Constructor/Destructor
    }
}