namespace Oliviann.Native
{
    #region Usings

    using System;

    #endregion Usings

    /// <summary>
    /// Defines the IOCTL device functions for Windows components.
    /// </summary>
    [Flags]
    public enum FileDevice : ushort
    {
        /// <summary>
        /// Keyboard device.
        /// </summary>
        Keyboard = 0x0000000b,
    }
}