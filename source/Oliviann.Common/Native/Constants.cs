namespace Oliviann.Native
{
    /// <summary>
    /// Represents a collection of constant values to be used with Native
    /// methods.
    /// </summary>
    public static class Constants
    {
        /// <summary>
        /// Sets the textual cue, or tip, that is displayed by the edit control
        /// to prompt the user for information.
        /// </summary>
        internal const int EM_SETCUEBANNER = 0x1501;

        /// <summary>
        /// Gets the text that is displayed as the textual cue, or tip, in an
        /// edit control.
        /// </summary>
        internal const int EM_GETCUEBANNER = 0x1502;
    }
}