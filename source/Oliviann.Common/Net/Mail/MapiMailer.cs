#if NETFRAMEWORK

namespace Oliviann.Net.Mail
{
    #region Usings

    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Runtime.InteropServices;
    using Oliviann.Collections.Generic;
    using Oliviann.Native;
    using Oliviann.Properties;

    #endregion Usings

    /// <summary>
    /// Represents a simple MAPI email sender.
    /// </summary>
    /// <remarks>Messages with over 20 attachments have been shown to cause
    /// errors. Default limit is set to 100.</remarks>
    public class MapiMailer : IEmailClient
    {
        #region Constructor/Destructor

        /// <summary>
        /// Initializes a new instance of the <see cref="MapiMailer"/> class.
        /// </summary>
        public MapiMailer() : this((ushort)100)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MapiMailer" /> class.
        /// </summary>
        /// <param name="maxAttachmentLimit">The maximum number of file
        /// attachments allowed per email message.</param>
        public MapiMailer(ushort maxAttachmentLimit)
        {
            this.MaxAttachmentLimit = maxAttachmentLimit;
        }

        #endregion Constructor/Destructor

        #region Properties

        /// <inheritdoc />
        public IEmailMessage Message { get; set; }

        /// <summary>
        /// Gets or sets the maximum number of file attachments allowed per
        /// message.
        /// </summary>
        /// <value>
        /// The maximum number of file attachments allowed per message.
        /// </value>
        public ushort MaxAttachmentLimit { get; set; }

        #endregion Properties

        #region Methods

        /// <inheritdoc />
        public void Send()
        {
            ADP.CheckNullReference(this.Message, Resources.ERR_ValueNullAndNotIEmailMessage);
            MapiMessage msg = null;

            try
            {
                msg = new MapiMessage { lpszSubject = this.Message.Subject, lpszNoteText = this.Message.Body };
                msg.lpRecips = this.AddRecipients(msg);
                msg.lpFiles = this.AddAttachments(msg);

                int result = this.SendEmail(msg, MapiSendMethod.MAPI_LOGON_UI);
                if (result > 1)
                {
                    throw new MapiMailException(this.GetErrorMessage(result), result);
                }
            }
            finally
            {
                if (msg != null)
                {
                    this.Cleanup(msg);
                }
            }
        }

        #endregion Methods

        #region Helper Methods

        /// <summary>
        /// Adds the recipients to the specified MAPI message.
        /// </summary>
        /// <param name="msg">The MAPI message instance to add recipients to.
        /// </param>
        /// <returns>A pointer to the recipients instance.</returns>
        private IntPtr AddRecipients(MapiMessage msg)
        {
            if (this.Message.ToAddresses.Count == 0 && this.Message.CcAddresses.Count == 0)
            {
                msg.nRecipCount = 0;
                return IntPtr.Zero;
            }

            var recipients = new List<MapiRecipDesc>();

            // Convert "To" recipients to description.
            ConvertRecipients(this.Message.ToAddresses, MapiRecipientType.MAPI_TO);

            // Convert "Cc" recipients to description.
            ConvertRecipients(this.Message.CcAddresses, MapiRecipientType.MAPI_CC);

            // Convert "Bcc" recipients to description.
            ConvertRecipients(this.Message.BccAddresses, MapiRecipientType.MAPI_BCC);

            // Convert recipients to structures.
            int size = Marshal.SizeOf(typeof(MapiRecipDesc));
            IntPtr ptr = Marshal.AllocHGlobal(recipients.Count * size);

            var ptrSize = (long)ptr;
            foreach (MapiRecipDesc recipDesc in recipients)
            {
                Marshal.StructureToPtr(recipDesc, (IntPtr)ptrSize, false);
                ptrSize += size;
            }

            msg.nRecipCount = recipients.Count;
            return ptr;

            // Local function for converting recipients.
            void ConvertRecipients(IEnumerable<string> addresses, MapiRecipientType type)
            {
                recipients.AddRange(addresses.Select(address => new MapiRecipDesc { ulRecipClass = type, lpszName = address }));
            }
        }

        /// <summary>
        /// Adds the file attachments to the specified MAPI message.
        /// </summary>
        /// <param name="msg">The MAPI message instance to add the attachments
        /// to.</param>
        /// <returns>A pointer to the file attachments instance.</returns>
        private IntPtr AddAttachments(MapiMessage msg)
        {
            if (this.Message.Attachments.IsNullOrEmpty())
            {
                msg.nFileCount = 0;
                return IntPtr.Zero;
            }

            int maxLimit = this.Message.Attachments.Count > this.MaxAttachmentLimit
                ? this.MaxAttachmentLimit
                : this.Message.Attachments.Count;

            int size = Marshal.SizeOf(typeof(MapiFileDesc));
            IntPtr ptr = Marshal.AllocHGlobal(maxLimit * size);

            var fileDesc = new MapiFileDesc { nPosition = -1 };
            var ptrSize = (long)ptr;

            int fileCount = 1;
            foreach (string attachmentPath in this.Message.Attachments)
            {
                // This makes sure we only add the number of attachments
                // that are allowed.
                if (fileCount > maxLimit)
                {
                    break;
                }

                fileDesc.lpszFileName = Path.GetFileName(attachmentPath);
                fileDesc.lpszPathName = attachmentPath;

                Marshal.StructureToPtr(fileDesc, (IntPtr)ptrSize, false);
                ptrSize += size;
                fileCount += 1;
            }

            msg.nFileCount = this.Message.Attachments.Count;
            return ptr;
        }

        /// <summary>
        /// Sends the specified MAPI email message.
        /// </summary>
        /// <param name="msg">The MAPI email message instance.</param>
        /// <param name="method">The method for the message to be sent.</param>
        /// <returns>An integer result code.</returns>
        private int SendEmail(MapiMessage msg, MapiSendMethod method)
        {
            int resultCode = UnsafeNativeMethods.MAPISendMail(IntPtr.Zero, IntPtr.Zero, msg, method, 0U);
            return resultCode;
        }

        /// <summary>
        /// Gets the error message based on the specified error code.
        /// </summary>
        /// <param name="errorCode">The resulting error code.</param>
        /// <returns>A string message associated with the specified error code.
        /// </returns>
        private string GetErrorMessage(int errorCode)
        {
            string[] errorMessages = {
                                         "OK [0]", "User abort [1]", "General MAPI failure [2]", "MAPI login failure [3]",
                                         "Disk full [4]", "Insufficient memory [5]", "Access denied [6]", "-unknown- [7]",
                                         "Too many sessions [8]", "Too many files were specified [9]",
                                         "Too many recipients were specified [10]",
                                         "A specified attachment was not found [11]", "Attachment open failure [12]",
                                         "Attachment write failure [13]", "Unknown recipient [14]",
                                         "Bad recipient type [15]", "No messages [16]", "Invalid message [17]",
                                         "Text too large [18]", "Invalid session [19]", "Type not supported [20]",
                                         "A recipient was specified ambiguously [21]", "Message in use [22]",
                                         "Network failure [23]", "Invalid edit fields [24]", "Invalid recipients [25]",
                                         "Not supported [26]"
                                     };

            return errorCode > 26 ? "MAPI error [" + errorCode + "]" : errorMessages[errorCode];
        }

        /// <summary>
        /// Cleans up the memory after the message has been sent.
        /// </summary>
        /// <param name="msg">The to clean up after.</param>
        private void Cleanup(MapiMessage msg)
        {
            // Clean up recipients.
            if (msg.lpRecips != IntPtr.Zero)
            {
                int size = Marshal.SizeOf(typeof(MapiRecipDesc));
                var ptr = (long)msg.lpRecips;
                for (int i = 0; i < msg.nRecipCount; i += 1)
                {
                    Marshal.DestroyStructure((IntPtr)ptr, typeof(MapiRecipDesc));
                    ptr += size;
                }

                Marshal.FreeHGlobal(msg.lpRecips);
            }

            // Cleanup Attachments
            if (msg.lpFiles != IntPtr.Zero)
            {
                int size = Marshal.SizeOf(typeof(MapiFileDesc));
                var ptr = (long)msg.lpFiles;
                for (int i = 0; i < msg.nFileCount; i += 1)
                {
                    Marshal.DestroyStructure((IntPtr)ptr, typeof(MapiFileDesc));
                    ptr += size;
                }

                Marshal.FreeHGlobal(msg.lpFiles);
            }
        }

        #endregion Helper Methods
    }
}

#endif