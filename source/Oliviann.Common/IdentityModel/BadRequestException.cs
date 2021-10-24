﻿namespace Oliviann.IdentityModel
{
    #region Usings

    using System;

    #endregion

    /// <summary>
    /// Represents a bad request is made to the server.
    /// </summary>
    public class BadRequestException : RequestException
    {
        #region Constructor/Destructor

        /// <summary>
        /// Initializes a new instance of the <see cref="BadRequestException"/>
        /// class.
        /// </summary>
        public BadRequestException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BadRequestException"/>
        /// class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public BadRequestException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BadRequestException"/>
        /// class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for
        /// the exception.</param>
        /// <param name="innerException">The exception that is the cause of the
        /// current exception, or a null reference if no inner exception is
        /// specified.</param>
        public BadRequestException(string message, Exception innerException) : base(message, innerException)
        {
        }

        #endregion
    }
}