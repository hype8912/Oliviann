namespace Oliviann.Win32.SafeHandles
{
#if NET35 || NET40 || NET45

    #region Usings

    using System;
    using System.Diagnostics;
    using Oliviann.Native;
    using Microsoft.Win32.SafeHandles;

    #endregion

    /// <summary>
    /// Provides a managed wrapper for a process handle.
    /// </summary>
    public sealed class SafeProcessHandle : SafeHandleZeroOrMinusOneIsInvalid
    {
        #region Fields

        internal static readonly SafeProcessHandle InvalidHandle = new SafeProcessHandle();

        #endregion

        #region Constructor/Destructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SafeProcessHandle"/>
        /// class.
        /// </summary>
        public SafeProcessHandle() : this(IntPtr.Zero)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SafeProcessHandle"/>
        /// class.
        /// </summary>
        /// <param name="existingHandle">The handle to be wrapped.</param>
        public SafeProcessHandle(IntPtr existingHandle) : this(existingHandle, true)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SafeProcessHandle"/>
        /// class.
        /// </summary>
        /// <param name="existingHandle">The handle to be wrapped.</param>
        /// <param name="ownsHandle">True to reliably let this instance release
        /// the handle during the finalization phase; otherwise, false.</param>
        public SafeProcessHandle(IntPtr existingHandle, bool ownsHandle) : base(ownsHandle)
        {
            SetHandle(existingHandle);
        }

        #endregion

        #region Methods

        /// <summary>
        ///
        /// </summary>
        /// <param name="h">The handle to be wrapped.</param>
        internal void InitialSetHandle(IntPtr h)
        {
            Debug.Assert(IsInvalid, "Safe handle should only be set once");
            base.handle = h;
        }

        #endregion

        /// <summary>
        /// Releases the current handle.
        /// </summary>
        /// <returns>True if the handle is released successfully; otherwise, in
        /// the event of a catastrophic failure, false.</returns>
        protected override bool ReleaseHandle() => UnsafeNativeMethods.CloseHandle(base.handle);
    }

#endif
}