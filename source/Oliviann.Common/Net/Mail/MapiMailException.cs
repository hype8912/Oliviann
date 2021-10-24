namespace Oliviann.Net.Mail
{
    #region Usings

    using System;

    #endregion Usings

    /// <summary>
    /// Represents a MAPI mailer exception.
    /// </summary>
    [Serializable]
    public class MapiMailException : Exception
    {
        #region Constructor/Destructor

        /// <summary>
        /// Initializes a new instance of the <see cref="MapiMailException"/>
        /// class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="errorCode">Numerical value for the assigned error.
        /// </param>
        public MapiMailException(string message, int errorCode) : base(message) => this.HResult = errorCode;

        #endregion Constructor/Destructor
    }
}