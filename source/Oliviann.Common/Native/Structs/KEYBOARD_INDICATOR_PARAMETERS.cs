namespace Oliviann.Native
{
    #region Usings

    using Windows.Input;

    #endregion Usings

    /// <summary>
    /// Specifies the state of a keyboard's indicator LEDs.
    /// </summary>
    /// <remarks>
    /// See
    /// <a href="http://msdn.microsoft.com/en-us/library/windows/hardware/ff542331.aspx">
    /// link</a> for more information.
    /// </remarks>
    public struct KEYBOARD_INDICATOR_PARAMETERS
    {
        #region Fields

        /// <summary>
        /// Specifies the unit number of a keyboard device. A keyboard device
        /// name has the format \Device\KeyboardPortN, where the suffix N is the
        /// unit number of the device.
        /// </summary>
        public ushort UnitId;

        /// <summary>
        /// The LEDs on the keyboard that are currently on.
        /// </summary>
        public KeyboardLocks LedFlags;

        #endregion Fields
    }
}