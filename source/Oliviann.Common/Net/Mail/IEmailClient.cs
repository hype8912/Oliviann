namespace Oliviann.Net.Mail
{
    /// <summary>
    /// Represents an implementation of a email client.
    /// </summary>
    public interface IEmailClient
    {
        #region Properties

        /// <summary>
        /// Sets the email message instance to be sent.
        /// </summary>
        /// <value>
        /// The email message instance.
        /// </value>
        IEmailMessage Message { set; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Sends the provided message instance.
        /// </summary>
        void Send();

        #endregion Methods
    }
}