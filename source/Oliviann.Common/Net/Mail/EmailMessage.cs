namespace Oliviann.Net.Mail
{
    #region Usings

    using System;
    using System.Collections.Generic;

    #endregion Usings

    /// <summary>
    /// Represents a email message.
    /// </summary>
    /// <seealso cref="IEmailMessage" />
    [Serializable]
    public class EmailMessage : IEmailMessage
    {
        #region Constructor/Destructor

        /// <summary>
        /// Initializes a new instance of the <see cref="EmailMessage"/> class.
        /// </summary>
        public EmailMessage()
        {
            this.ToAddresses = new List<string>();
            this.CcAddresses = new List<string>();
            this.BccAddresses = new List<string>();
            this.Attachments = new List<string>();
        }

        #endregion Constructor/Destructor

        #region Properties

        /// <inheritdoc />
        public string Id { get; set; }

        /// <inheritdoc />
        public string FromAddress { get; set; }

        /// <inheritdoc />
        public ICollection<string> ToAddresses { get; }

        /// <inheritdoc />
        public ICollection<string> CcAddresses { get; }

        /// <inheritdoc />
        public ICollection<string> BccAddresses { get; }

        /// <inheritdoc />
        public string Subject { get; set; }

        /// <inheritdoc />
        public string Body { get; set; }

        /// <inheritdoc />
        public ICollection<string> Attachments { get; }

        /// <inheritdoc />
        public string HostAddress { get; set; }

        /// <inheritdoc />
        public ushort? HostPort { get; set; }

        #endregion Properties
    }
}