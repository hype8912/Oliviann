#if NETSTANDARD1_3
namespace Oliviann.Win32.SafeHandles
{
    #region Usings

    using System;
    using System.Runtime.InteropServices;

    #endregion

    /// <summary>
    /// Provides a base class for Win32 safe handle implementations in which the
    /// value of -1 indicates an invalid handle.
    /// </summary>
    public abstract class SafeHandleMinusOneIsInvalid : SafeHandle
    {
        #region Constructor/Destructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SafeHandleMinusOneIsInvalid"/> class.
        /// </summary>
        /// <param name="ownsHandle">True to reliably let this instance release
        /// the handle during the finalization phase; otherwise, false.</param>
        protected SafeHandleMinusOneIsInvalid(bool ownsHandle) : base(new IntPtr(-1), ownsHandle)
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets a value that indicates whether the handle is invalid.
        /// </summary>
        public override bool IsInvalid => handle == new IntPtr(-1);

        #endregion
    }
}
#endif