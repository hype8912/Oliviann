namespace Oliviann.Diagnostics
{
    #region Usings

    using System;
    using System.Diagnostics;
    using Oliviann.Win32.SafeHandles;
    using Microsoft.Win32.SafeHandles;

    #endregion

    /// <summary>
    /// Represents a process wrapper proxy.
    /// </summary>
    public class ProcessProxy : IProcessProxy
    {
        #region Fields

        /// <summary>
        /// The current process.
        /// </summary>
        private readonly Process _currentProcess;

        #endregion

        #region Constructor/Destructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessProxy"/> class.
        /// </summary>
        public ProcessProxy() : this(new Process())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessProxy" /> class.
        /// </summary>
        /// <param name="currentProcess">The current process.</param>
        public ProcessProxy(Process currentProcess)
        {
            ADP.CheckArgumentNull(currentProcess, nameof(currentProcess));
            this._currentProcess = currentProcess;
        }

        #endregion

        #region Properties

        /// <inheritdoc />
        public SafeProcessHandle SafeHandle
        {
            get
            {
#if NET35 || NET40 || NET45
                return new SafeProcessHandle(this._currentProcess.Handle);
#else
                return this._currentProcess.SafeHandle;
#endif
            }
        }

        /// <inheritdoc />
        public ProcessStartInfo StartInfo
        {
            get => this._currentProcess.StartInfo;
            set => this._currentProcess.StartInfo = value;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        public void Dispose() => this._currentProcess.Dispose();

        /// <inheritdoc />
        public Process GetProcess() => this._currentProcess;

        /// <inheritdoc />
        public bool Start() => this._currentProcess.Start();

        /// <inheritdoc />
        public void WaitForExit() => this._currentProcess.WaitForExit();

        /// <inheritdoc />
        public bool WaitForExit(int milliseconds) => this._currentProcess.WaitForExit(milliseconds);

        #endregion
    }
}