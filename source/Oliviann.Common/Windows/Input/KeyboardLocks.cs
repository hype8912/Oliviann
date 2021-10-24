namespace Oliviann.Windows.Input
{
    #region Usings

    using System;

    #endregion Usings

    /// <summary>
    /// Indicates a specific lock indicator on the keyboard.
    /// </summary>
    [Flags]
    public enum KeyboardLocks : ushort
    {
        /// <summary>
        /// Indicates no lock indicators are on.
        /// </summary>
        None = 0,

        /// <summary>
        /// Indicates the SCROLL LOCK LED is on.
        /// </summary>
        ScrollLockOn = 1,

        /// <summary>
        /// Indicates the NUM LOCK LED is on.
        /// </summary>
        NumLockOn = 2,

        /// <summary>
        /// Indicates the CAPS LOCK LED is on.
        /// </summary>
        CapsLockOn = 4
    }
}