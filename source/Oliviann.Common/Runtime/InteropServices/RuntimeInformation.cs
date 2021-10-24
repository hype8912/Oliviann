namespace Oliviann.Runtime.InteropServices
{
    #region Usings

    using System.Runtime.InteropServices;

    #endregion Usings

    /// <summary>
    /// Represents a class for determining the current runtime information.
    /// </summary>
    public static class RuntimeInformation
    {
        /// <summary>
        /// Determines whether the current runtime environment is running on
        /// Windows.
        /// </summary>
        /// <returns>
        /// True if running on Windows; otherwise, false.
        /// </returns>
        public static bool IsWindows()
        {
#if NETFRAMEWORK
            return true;
#else
            return System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
#endif
        }
    }
}