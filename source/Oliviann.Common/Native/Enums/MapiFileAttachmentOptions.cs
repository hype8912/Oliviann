namespace Oliviann.Native
{
    #region Usings

    using System;

    #endregion Usings

    /// <summary>
    /// Defines the MAPI file attachment options.
    /// </summary>
    [Flags]
    public enum MapiFileAttachmentOptions
    {
        /// <summary>
        /// The attachment is an OLE object. If MAPI_OLE_STATIC is also set, the
        /// attachment is a static OLE object. If MAPI_OLE_STATIC is not set,
        /// the attachment is an embedded OLE object.
        /// </summary>
        MAPI_OLE = 1,

        /// <summary>
        /// The attachment is a static OLE object.
        /// </summary>
        MAPI_OLE_STATIC = 2
    }
}