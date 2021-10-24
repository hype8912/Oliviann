namespace Oliviann.Diagnostics
{
    #region Usings

    using System;
    using System.Diagnostics;
    using Oliviann.Win32.SafeHandles;
    using Microsoft.Win32.SafeHandles;

    #endregion

    /// <summary>
    /// Represents a proxy implementation for a Process to make testing easier.
    /// </summary>
    public interface IProcessProxy : IDisposable
    {
        #region Properties

        /// <summary>
        /// Gets the native handle to this process.
        /// </summary>
        /// <value>
        /// The native handle to this process.
        /// </value>
        SafeProcessHandle SafeHandle { get; }

        /// <inheritdoc cref="Process.StartInfo"/>
        ProcessStartInfo StartInfo { get; set; }

        #endregion

        /// <summary>
        /// Gets the underlying Process instance.
        /// </summary>
        /// <returns>The underlying Process instance.</returns>
        Process GetProcess();

        /// <inheritdoc cref="Process.Start()"/>
        bool Start();

        /// <inheritdoc cref="Process.WaitForExit()"/>
        void WaitForExit();

        /// <inheritdoc cref="Process.WaitForExit(int)"/>
        bool WaitForExit(int milliseconds);
    }
}