namespace Oliviann.Native
{
    #region Usings

    using System.Runtime.InteropServices;

    #endregion Usings

    /// <summary>
    /// An LUID is a 64-bit value guaranteed to be unique only on the system
    /// on which it was generated.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct LUID
    {
        /// <summary>
        /// Low-order bits.
        /// </summary>
        public uint LowPart;

        /// <summary>
        /// High-order bits.
        /// </summary>
        public uint HighPart;
    }
}