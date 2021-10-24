namespace Oliviann.Native
{
    /// <summary>
    /// Specifies the logon provider type.
    /// </summary>
    public enum LogonProvider
    {
        /// <summary>
        /// Use the standard logon provider for the system. It provides the
        /// maximum compatibility with current and future release of Windows NT.
        /// </summary>
        LOGON32_PROVIDER_DEFAULT = 0,

        /// <summary>
        /// Uses the Windows NT 3.5 logon provider.
        /// </summary>
        LOGON32_PROVIDER_WINNT35 = 1,

        /// <summary>
        /// Use the NTLM logon provider.
        /// </summary>
        LOGON32_PROVIDER_WINNT40 = 2,

        /// <summary>
        /// Use the negotiate logon provider.
        /// </summary>
        LOGON32_PROVIDER_WINNT50 = 3
    }
}