namespace Oliviann.Native
{
    /// <summary>
    /// Defines the Windows Messages.
    /// </summary>
    public enum WM : uint
    {
        /// <summary>
        /// The WM_NULL message performs no operation. An application sends the
        /// WM_NULL message if it wants to post a message that the recipient
        /// window will ignore.
        /// </summary>
        NULL = 0x0000,

        /// <summary>
        /// An application sends a WM_PASTE message to an edit control or combo
        /// box to copy the current content of the clipboard to the edit control
        /// at the current caret position. Data is inserted only if the
        /// clipboard contains data in CF_TEXT format.
        /// </summary>
        PASTE = 0x0302
    }
}