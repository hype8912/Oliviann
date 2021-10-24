namespace Oliviann.IO
{
    #region Usings

    using System;

    #endregion Usings

    /// <summary>
    /// Represents an invalid path was passed.
    /// </summary>
    [Serializable]
    public class InvalidPathException : Exception
    {
        #region Contsructor/Destructor

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidPathException"/>
        /// class.
        /// </summary>
        public InvalidPathException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidPathException"/>
        /// class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public InvalidPathException(string message) : base(message)
        {
        }

#if !NETSTANDARD1_3

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidPathException"/>
        /// class with serialized data.
        /// </summary>
        /// <param name="info">The
        /// <see cref="T:System.Runtime.Serialization.SerializationInfo"/> that
        /// holds the serialized object data about the exception being thrown.
        /// </param>
        /// <param name="context">The
        /// <see cref="T:System.Runtime.Serialization.StreamingContext"/> that
        /// contains contextual information about the source or destination.
        /// </param>
        /// <exception cref="T:System.ArgumentNullException">The
        /// <paramref name="info"/> parameter is <c>null</c>. </exception>
        /// <exception cref="T:System.Runtime.Serialization.SerializationException">
        /// The class name is <c>null</c> or
        /// <see cref="P:System.Exception.HResult"/> is zero (0). </exception>
        public InvalidPathException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context)
        {
        }

#endif

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidPathException"/>
        /// class with a specified error message and a reference to the inner
        /// exception that is the root cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for
        /// the exception.</param>
        /// <param name="innerException">An instance of System.Exception that is
        /// the cause of the current Exception. If inner is not a <c>null</c>
        /// reference (Nothing in Visual Basic), then the current Exception is
        /// raised in a catch block handling inner.</param>
        public InvalidPathException(string message, Exception innerException) : base(message, innerException)
        {
        }

        #endregion Contsructor/Destructor
    }
}