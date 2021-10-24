namespace Oliviann.Native
{
    #region Usings

    using System;

    #endregion Usings

    /// <summary>
    /// Defines the method to send the MAPI message.
    /// </summary>
    [Flags]
    public enum MapiSendMethod
    {
        /// <summary>
        /// A dialog box should be displayed to prompt the user to log on if
        /// required.
        /// </summary>
        MAPI_LOGON_UI = 0,

        /// <summary>
        /// An attempt is made to create a new session rather than acquire the
        /// environment's shared session.
        /// </summary>
        MAPI_NEW_SESSION = 1,

        /// <summary>
        /// A modeless dialog box should be displayed to prompt the user for
        /// recipients and other sending options.
        /// </summary>
        MAPI_DIALOG_MODELESS = 4,

        /// <summary>
        /// A application modal dialog box should be displayed to prompt the
        /// user for recipients and other sending options.
        /// </summary>
        MAPI_DIALOG = 8,

        /// <summary>
        /// Forces the message to be sent using Unicode. Do not convert the
        /// message to ANSI if the provider does not support Unicode.
        /// </summary>
        MAPI_FORCE_UNICODE = 262144
    }
}