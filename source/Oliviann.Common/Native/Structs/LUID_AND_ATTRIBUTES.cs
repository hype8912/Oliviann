namespace Oliviann.Native
{
    #region Usings

    using System.Runtime.InteropServices;

    #endregion Usings

    /// <summary>
    /// Represents a <see cref="LUID">locally unique identifier</see> (LUID)
    /// and its attributes.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct LUID_AND_ATTRIBUTES
    {
        /// <summary>
        /// Specifies an <see cref="LUID"/> value.
        /// </summary>
        public LUID Luid;

        /// <summary>
        /// Specifies attributes of the LUID.
        /// </summary>
        public uint Attributes;
    }
}