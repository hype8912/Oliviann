namespace Oliviann.Win32.SafeHandles
{
    #region Usings

    using Oliviann.Native;
    using Microsoft.Win32.SafeHandles;

    #endregion Usings

    /// <summary>
    /// Represents a safe Windows token handle.
    /// </summary>
    public sealed class SafeTokenHandle : SafeHandleMinusOneIsInvalid
    {
        #region Constructor/Destructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SafeTokenHandle" />
        /// class.
        /// </summary>
        public SafeTokenHandle() : base(true)
        {
        }

        #endregion Constructor/Destructor

        #region Methods

        /// <summary>
        /// When overridden in a derived class, executes the code required to
        /// free the handle.
        /// </summary>
        /// <returns>
        /// True if the handle is released successfully; otherwise, in the event
        /// of a catastrophic failure, false. In this case, it generates a
        /// releaseHandleFailed MDA Managed Debugging Assistant.
        /// </returns>
        protected override bool ReleaseHandle() => UnsafeNativeMethods.CloseHandle(base.handle);

        #endregion Methods
    }
}