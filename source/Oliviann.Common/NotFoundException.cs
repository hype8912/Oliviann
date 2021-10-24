namespace Oliviann
{
    #region Usings

    using System;

    #endregion Usings

    /// <summary>
    /// Represents an exception that is thrown when an object is not present.
    /// </summary>
    public class NotFoundException :
#if NETSTANDARD1_3
        Exception
#else
        ApplicationException
#endif
    {
        #region Constructor/Destructor

        /// <summary>
        /// Initializes a new instance of the <see cref="NotFoundException"/>
        /// class with a specialized error message.
        /// </summary>
        /// <param name="message">A message that describes the error.</param>
        public NotFoundException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NotFoundException"/>
        /// class that uses a specified error message and a reference to the
        /// inner exception.
        /// </summary>
        /// <param name="message">A message that describes the error.</param>
        /// <param name="innerException">The exception that is the cause of the
        /// current exception, or a null reference if no inner exception is
        /// specified.</param>
        public NotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        #endregion Constructor/Destructor
    }
}