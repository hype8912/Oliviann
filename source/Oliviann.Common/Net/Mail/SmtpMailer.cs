#if !NETSTANDARD1_3

namespace Oliviann.Net.Mail
{
    #region Usings

    using System.Net.Mail;
    using Oliviann.Collections.Generic;
    using Oliviann.Properties;

    #endregion Usings

    /// <summary>
    /// Represents a simple SMTP email sender.
    /// </summary>
    public class SmtpMailer : IEmailClient
    {
        #region Properties

        /// <inheritdoc />
        public IEmailMessage Message { get; set; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Sends the provided message instance.
        /// </summary>
        public void Send()
        {
            ADP.CheckNullReference(this.Message, Resources.ERR_ValueNullAndNotIEmailMessage);
            MailMessage senderMessage = null;

            try
            {
                senderMessage = new MailMessage
                                {
                                    From = new MailAddress(this.Message.FromAddress),
                                    Subject = this.Message.Subject,
                                    Body = this.Message.Body
                                };

                this.Message.ToAddresses.ForEach(senderMessage.To.Add);
                this.Message.CcAddresses.ForEach(senderMessage.CC.Add);
                foreach (string attachPath in this.Message.Attachments)
                {
                    senderMessage.Attachments.Add(attachPath);
                }

                this.SendEmail(senderMessage);
            }
            finally
            {
                senderMessage.DisposeSafe();
            }
        }

        /// <summary>
        /// Uses an SMTP client to send the specified message.
        /// </summary>
        /// <param name="message">The mail message to be sent.</param>
        private void SendEmail(MailMessage message)
        {
            SmtpClient client = null;

            try
            {
                client = new SmtpClient(this.Message.HostAddress);
                if (this.Message.HostPort.HasValue)
                {
                    client.Port = this.Message.HostPort.Value;
                }

                client.Send(message);
            }
            finally
            {
#if !NET35
                client.DisposeSafe();
#endif
            }
        }

        #endregion Methods
    }
}

#endif