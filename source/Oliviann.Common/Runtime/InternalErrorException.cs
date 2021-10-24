namespace Oliviann.Runtime
{
    #region Usings

    using System;
    using System.Globalization;
    using Oliviann.Properties;

    #endregion Usings

    /// <summary>
    /// Represents an internal error exception event.
    /// </summary>
    [Serializable]
    public class InternalErrorException : Exception
    {
        #region Constructor/Destructor

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="InternalErrorException"/> class.
        /// </summary>
        /// <param name="message">The internal error message.</param>
        public InternalErrorException(string message) :
            base(string.Format(CultureInfo.CurrentCulture, Resources.InternalExceptionMessage, message))
        {
        }

#if !NETSTANDARD1_3

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="InternalErrorException"/> class.
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
        /// <paramref name="info"/> parameter is <c>null</c>.</exception>
        /// <exception cref="T:System.Runtime.Serialization.SerializationException">
        /// The class name is <c>null</c> or
        /// <see cref="P:System.Exception.HResult"/>is zero (0). </exception>
        public InternalErrorException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context)
        {
        }

#endif

        #endregion Constructor/Destructor
    }
}