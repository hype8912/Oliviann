namespace Oliviann.Windows
{
    #region Usings

    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Runtime.InteropServices;
    using Oliviann.Diagnostics;
    using Oliviann.Native;
    using Oliviann.Properties;
    using Oliviann.Win32.SafeHandles;

    #endregion Usings

    /// <summary>
    /// Represents a class for performing common Operating System functions.
    /// </summary>
    public static class OS
    {
        #region Fields

        /// <summary>
        /// Access control privilege.
        /// </summary>
        private const uint TOKEN_QUERY = 0x0008;

        /// <summary>
        /// Access control privilege.
        /// </summary>
        private const uint TOKEN_ADJUST_PRIVILEGES = 0x0020;

        /// <summary>
        /// Access control privilege.
        /// </summary>
        private const uint SE_PRIVILEGE_ENABLED = 0x00000002;

        /// <summary>
        /// UAC privilege value for shutdown.
        /// </summary>
        private const string SE_SHUTDOWN_NAME = "SeShutdownPrivilege";

        #endregion Fields

        #region Methods

        /// <summary>
        /// Reboots the current computer using built-in Windows functions and
        /// not Process calls or command line methods.
        /// </summary>
        /// <exception cref="Win32Exception">Failed to open process token
        /// handle.</exception>
        /// <exception cref="Win32Exception">Failed to open lookup shutdown
        /// privilege.</exception>
        /// <exception cref="Win32Exception">Failed to adjust process token
        /// privileges.</exception>
        /// <exception cref="Win32Exception">Failed to reboot system.
        /// </exception>
        public static void Reboot()
        {
            SafeTokenHandle tokenHandle = null;

            try
            {
                // Get process token.
                SafeHandle processHandle = Process.GetCurrentProcess().GetSafeHandle();

                if (!UnsafeNativeMethods.OpenProcessToken(processHandle, TOKEN_QUERY | TOKEN_ADJUST_PRIVILEGES, out tokenHandle))
                {
                    throw new Win32Exception(Marshal.GetLastWin32Error(), Resources.ERR_FailedOpenHandle);
                }

                // Lookup the shutdown privilege.
                var tokenPrivs = new TOKEN_PRIVILEGES
                                     {
                                         PrivilegeCount = 1,
                                         Privileges = new LUID_AND_ATTRIBUTES[1]
                                     };
                tokenPrivs.Privileges[0].Attributes = SE_PRIVILEGE_ENABLED;

                if (!UnsafeNativeMethods.LookupPrivilegeValue(null, SE_SHUTDOWN_NAME, out tokenPrivs.Privileges[0].Luid))
                {
                    throw new Win32Exception(Marshal.GetLastWin32Error(), Resources.ERR_FailedOpenShutdownPriv);
                }

                // Add the shutdown privilege to the process token
                if (!UnsafeNativeMethods.AdjustTokenPrivileges(tokenHandle, false, ref tokenPrivs, 0, IntPtr.Zero, IntPtr.Zero))
                {
                    throw new Win32Exception(Marshal.GetLastWin32Error(), Resources.ERR_FailedAdjustPriv);
                }

                // Reboot
                if (!UnsafeNativeMethods.ExitWindowsEx(
                    ExitWindows.Reboot, ShutdownReason.MajorApplication | ShutdownReason.MinorReconfig | ShutdownReason.FlagPlanned))
                {
                    throw new Win32Exception(Marshal.GetLastWin32Error(), Resources.ERR_FailedReboot);
                }
            }
            finally
            {
                tokenHandle.DisposeSafe();
            }
        }

        #endregion Methods
    }
}