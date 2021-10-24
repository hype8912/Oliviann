namespace Oliviann.IdentityModel
{
    #region Usings

    using System;

    #endregion

    /// <summary>
    /// Represents a base class for exceptions thrown for request failures.
    /// </summary>
    [Serializable]
    public abstract class RequestException : Exception
    {
        #region Constructor/Destructor

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestException"/>
        /// class.
        /// </summary>
        protected RequestException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestException"/>
        /// class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        protected RequestException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestException"/>
        /// class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for
        /// the exception.</param>
        /// <param name="innerException">The exception that is the cause of the
        /// current exception, or a null reference if no inner exception is
        /// specified.</param>
        protected RequestException(string message, Exception innerException) : base(message, innerException)
        {
        }

        #endregion
    }
}