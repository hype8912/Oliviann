namespace Oliviann.Native
{
    #region Usings

    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.InteropServices;

    #endregion Usings

    /// <summary>
    /// Represents a structure that contains information about a file containing
    /// a message attachment stored as a temporary file.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:AccessibleFieldsMustBeginWithUpperCaseLetter",
        Justification = "Reviewed. Suppression is OK here.")]
    public class MapiFileDesc
    {
        #region Fields

        /// <summary>
        /// Reserved. Must be zero.
        /// </summary>
        public int ulReserved;

        /// <summary>
        /// The file attachment option flags.
        /// </summary>
        public MapiFileAttachmentOptions flFlags;

        /// <summary>
        /// An integer used to indicate where in the message text to render the
        /// attachment.
        /// </summary>
        public int nPosition;

        /// <summary>
        /// The fully qualified path of the attached file.
        /// </summary>
        public string lpszPathName;

        /// <summary>
        /// The attachment filename seen by the recipient, which may differ from
        /// the filename in the <c>lpszPathName</c> member if temporary files
        /// are being used. If the <c>lpszFileName</c> member is empty or NULL,
        /// the filename from <c>lpszPathName</c> is used.
        /// </summary>
        public string lpszFileName;

        /// <summary>
        /// Pointer to the attachment file type, which can be represented with a
        /// <c>MapiFileTagExt</c> structure. A value of null indicates an
        /// unknown file type or a file type determined by the operating system.
        /// </summary>
        public IntPtr lpFileType;

        #endregion Fields
    }
}