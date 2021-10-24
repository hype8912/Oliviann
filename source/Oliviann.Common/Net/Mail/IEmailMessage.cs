namespace Oliviann.Net.Mail
{
    #region Usings

    using System.Collections.Generic;

    #endregion Usings

    /// <summary>
    /// Represents an implementation of a EMail message.
    /// </summary>
    public interface IEmailMessage
    {
        #region Properties

        /// <summary>
        /// Gets or sets the unique message identifier.
        /// </summary>
        /// <value>
        /// The unique message identifier.
        /// </value>
        string Id { get; set; }

        /// <summary>
        /// Gets or sets the from mail address.
        /// </summary>
        /// <value>
        /// The from mail address.
        /// </value>
        string FromAddress { get; set; }

        /// <summary>
        /// Gets the collection of To addresses.
        /// </summary>
        /// <value>
        /// The collection of To addresses.
        /// </value>
        ICollection<string> ToAddresses { get; }

        /// <summary>
        /// Gets the collection of Cc addresses.
        /// </summary>
        /// <value>
        /// The collection of Cc addresses.
        /// </value>
        ICollection<string> CcAddresses { get; }

        /// <summary>
        /// Gets the collection of Bcc addresses.
        /// </summary>
        /// <value>
        /// The collection of Bcc addresses.
        /// </value>
        ICollection<string> BccAddresses { get; }

        /// <summary>
        /// Gets or sets the message subject.
        /// </summary>
        /// <value>
        /// The message subject.
        /// </value>
        string Subject { get; set; }

        /// <summary>
        /// Gets or sets the message body.
        /// </summary>
        /// <value>
        /// The message body.
        /// </value>
        string Body { get; set; }

        /// <summary>
        /// Gets the collection of attachment paths.
        /// </summary>
        /// <value>
        /// The collection of attachment paths.
        /// </value>
        ICollection<string> Attachments { get; }

        /// <summary>
        /// Gets or sets the SMTP host address.
        /// </summary>
        /// <value>
        /// The SMTP host address.
        /// </value>
        string HostAddress { get; set; }

        /// <summary>
        /// Gets or sets the SMTP host port.
        /// </summary>
        /// <value>
        /// The SMTP host port.
        /// </value>
        ushort? HostPort { get; set; }

        #endregion Properties
    }
}