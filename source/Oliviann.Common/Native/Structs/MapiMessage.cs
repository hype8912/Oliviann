namespace Oliviann.Native
{
    #region Usings

    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.InteropServices;

    #endregion Usings

    /// <summary>
    /// Represents a structure containing information about a message.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:AccessibleFieldsMustBeginWithUpperCaseLetter",
        Justification = "Reviewed. Suppression is OK here.")]
    public class MapiMessage
    {
        #region Fields

        /// <summary>
        /// Reserved. Must be zero.
        /// </summary>
        public int ulReserved;

        /// <summary>
        /// The text string describing the message subject, typically limited to
        /// 256 characters or less.
        /// </summary>
        public string lpszSubject;

        /// <summary>
        /// The text string containing the message text.
        /// </summary>
        public string lpszNoteText;

        /// <summary>
        /// A string indicating a non-IPM type of message. Client applications
        /// can select message types for their non-IPM messages.
        /// </summary>
        public string lpszMessageType;

        /// <summary>
        /// A string indicating a non-IPM type of message. Client applications
        /// can select message types for their non-IPM messages.
        /// </summary>
        public string lpszDateReceived;

        /// <summary>
        /// A string identifying the conversation thread to which the message
        /// belongs. Some messaging systems can ignore and not return this
        /// member.
        /// </summary>
        public string lpszConversationID;

        /// <summary>
        /// Bitmask of message status flags.
        /// </summary>
        public int flFlags;

        /// <summary>
        /// Pointer to a <see cref="MapiRecipDesc"/> structure containing
        /// information about the sender of the message.
        /// </summary>
        public IntPtr lpOriginator;

        /// <summary>
        /// The number of message recipient structures in the array pointed to
        /// by the <c>lpRecips</c> member. A value of zero indicates no
        /// recipients are included.
        /// </summary>
        public int nRecipCount;

        /// <summary>
        /// Pointer to an array of <see cref="MapiRecipDesc"/> structures, each
        /// containing information about a message recipient.
        /// </summary>
        public IntPtr lpRecips;

        /// <summary>
        /// The number of structures describing file attachments in the array
        /// pointed to by the <c>lpFiles</c> member. A value of zero indicates
        /// no file attachments are included
        /// </summary>
        public int nFileCount;

        /// <summary>
        /// Pointer to an array of <see cref="MapiFileDesc"/> structures, each
        /// containing information about a file attachment.
        /// </summary>
        public IntPtr lpFiles;

        #endregion Fields
    }
}