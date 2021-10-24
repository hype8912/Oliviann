namespace Oliviann.Native
{
    /// <summary>
    /// Defines the MAPI message recipient type.
    /// </summary>
    public enum MapiRecipientType
    {
        /// <summary>
        /// Indicates the original sender of the message.
        /// </summary>
        MAPI_ORIG = 0,

        /// <summary>
        /// Indicates a primary message recipient.
        /// </summary>
        MAPI_TO = 1,

        /// <summary>
        /// Indicates a recipient of a message copy.
        /// </summary>
        MAPI_CC = 2,

        /// <summary>
        /// Indicates a recipient of a blind copy.
        /// </summary>
        MAPI_BCC = 3
    }
}