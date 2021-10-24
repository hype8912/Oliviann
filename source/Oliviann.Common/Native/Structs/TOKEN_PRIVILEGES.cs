namespace Oliviann.Native
{
    #region Usings

    using System.Runtime.InteropServices;

    #endregion Usings

    /// <summary>
    /// Contains information about a set of privileges for an access token.
    /// </summary>
    public struct TOKEN_PRIVILEGES
    {
        /// <summary>
        /// This must be set to the number of entries in the Privileges
        /// array.
        /// </summary>
        public uint PrivilegeCount;

        /// <summary>
        /// Specifies an array of <see cref="LUID_AND_ATTRIBUTES"/>
        /// structures.
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
        public LUID_AND_ATTRIBUTES[] Privileges;
    }
}