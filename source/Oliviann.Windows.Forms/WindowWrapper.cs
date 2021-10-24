namespace Oliviann.Windows.Forms
{
    #region Usings

    using System;
    using System.Windows.Forms;

    #endregion Usings

    /// <summary>
    /// Represents a simple class for implementing only
    /// <see cref="IWin32Window"/>.
    /// </summary>
    public class WindowWrapper : IWin32Window
    {
        #region Constructor/Destructor

        /// <summary>
        /// Initializes a new instance of the <see cref="WindowWrapper" />
        /// class.
        /// </summary>
        /// <param name="handle">The window handle.</param>
        public WindowWrapper(IntPtr handle) => this.Handle = handle;

        #endregion Constructor/Destructor

        #region Properties

        /// <summary>
        /// Gets the handle to the window represented by the implementer.
        /// </summary>
        /// <returns>
        /// A handle to the window represented by the implementer.
        /// </returns>
        public IntPtr Handle { get; }

        #endregion Properties
    }
}