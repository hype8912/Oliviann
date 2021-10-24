namespace Oliviann.Native
{
    /// <summary>
    /// Specifies the state of the combo box button.
    /// </summary>
    public enum ComboBoxButtonState
    {
        /// <summary>
        /// The button exists and is not pressed.
        /// </summary>
        STATE_SYSTEM_NONE = 0,

        /// <summary>
        /// There is no button.
        /// </summary>
        STATE_SYSTEM_INVISIBLE = 0x00008000,

        /// <summary>
        /// The button is pressed.
        /// </summary>
        STATE_SYSTEM_PRESSED = 0x00000008
    }
}