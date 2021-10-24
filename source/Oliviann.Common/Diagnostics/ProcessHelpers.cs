namespace Oliviann.Diagnostics
{
    #region Usings

    using System;
    using System.Diagnostics;
    using System.Security.Principal;
    using Oliviann.Runtime.InteropServices;
    using Oliviann.Win32.SafeHandles;
    using Microsoft.Win32.SafeHandles;

    #endregion Usings

    /// <summary>
    /// Represents a collection of commonly used helper methods for making
    /// working with <see cref="Process"/> easier.
    /// </summary>
    public static class ProcessHelpers
    {
        /// <summary>
        /// Gets the native handle to the current process.
        /// </summary>
        /// <param name="currentProcess">The process to get a handle for.
        /// </param>
        /// <returns>The native handle to this process.</returns>
        public static SafeProcessHandle GetSafeHandle(this Process currentProcess)
        {
            ADP.CheckArgumentNull(currentProcess, nameof(currentProcess));
#if NET35 || NET40 || NET45
            return new SafeProcessHandle(currentProcess.Handle);
#else
            return currentProcess.SafeHandle;
#endif
        }

        /// <summary>
        /// Starts the process resource for the specified path under the logon
        /// user.
        /// </summary>
        /// <param name="fileName">An application or document with which to
        /// start a process.</param>
        /// <param name="startInfoOptions">Optional. The additional start
        /// information options.</param>
        /// <returns>
        /// The process instance started.
        /// </returns>
        public static Process StartAsLogonUser(string fileName, Action<ProcessStartInfo> startInfoOptions = null)
        {
            var startInfo = new ProcessStartInfo(fileName);
#if !NETSTANDARD1_3
            if (RuntimeInformation.IsWindows())
            {
#pragma warning disable PC001
                startInfo.UserName = WindowsIdentity.GetCurrent().Name;
#pragma warning restore PC001
            }
#endif

            startInfoOptions?.Invoke(startInfo);
            return Process.Start(startInfo);
        }
    }
}