namespace Oliviann.Native
{
    #region Usings

    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.InteropServices;

    #endregion Usings

    /// <summary>
    /// Represents a structure that contains information about a message sender
    /// or recipient.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:AccessibleFieldsMustBeginWithUpperCaseLetter",
        Justification = "Reviewed. Suppression is OK here.")]
    public class MapiRecipDesc
    {
        #region Fields

        /// <summary>
        /// Reserved. Must be zero.
        /// </summary>
        public int ulReserved;

        /// <summary>
        /// Contains a numeric value that indicates the type of recipient.
        /// </summary>
        public MapiRecipientType ulRecipClass;

        /// <summary>
        /// The display name of the message recipient or sender.
        /// </summary>
        public string lpszName;

        /// <summary>
        /// Optional. The recipient or sender's address.
        /// </summary>
        public string lpszAddress;

        /// <summary>
        /// The size, in bytes, of the entry identifier pointed to by the
        /// <c>lpEntryID</c> member.
        /// </summary>
        public int ulEIDSize;

        /// <summary>
        /// Pointer to an opaque entry identifier used by a messaging system
        /// service provider to identify the message recipient.
        /// </summary>
        public IntPtr lpEntryID;

        #endregion Fields
    }
}