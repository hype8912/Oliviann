namespace Oliviann.Native
{
    #region Usings

    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.InteropServices;

    #endregion Usings

    /// <summary>
    /// Represents parameters for the SHBrowseForFolder function and receives
    /// information about the folder selected by the user.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:AccessibleFieldsMustBeginWithUpperCaseLetter",
        Justification = "Reviewed. Suppression is OK here.")]
    public struct BROWSEINFO
    {
        /// <summary>
        /// A handle to the owner window for the dialog box.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible", Justification = "Reviewed")]
        public IntPtr hwndOwner;

        /// <summary>
        /// A PIDL that specifies the location of the root folder from which to
        /// start browsing. Only the specified folder and its subfolders in the
        /// namespace hierarchy appear in the dialog box.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible", Justification = "Reviewed")]
        public IntPtr pidlRoot;

        /// <summary>
        /// Pointer to a buffer to receive the display name of the folder
        /// selected by the user. The size of this buffer is assumed to be
        /// MAX_PATH characters.
        /// </summary>
        [MarshalAs(UnmanagedType.LPTStr)]
        public string lpszDisplayName;

        /// <summary>
        /// Pointer to a <c>null</c>-terminated string that is displayed above
        /// the tree view control in the dialog box. This string can be used to
        /// specify instructions to the user.
        /// </summary>
        [MarshalAs(UnmanagedType.LPTStr)]
        public string lpszTitle;

        /// <summary>
        /// Flags that specify the options for the dialog box. This member can
        /// be 0 or a combination of the following values.
        /// </summary>
        public int ulFlags;

        /// <summary>
        /// Pointer to an application-defined function that the dialog box calls
        /// when an event occurs.
        /// </summary>
        [MarshalAs(UnmanagedType.FunctionPtr)]
        public UnsafeNativeMethods.BrowseCallbackProc lpfn;

        /// <summary>
        /// An application-defined value that the dialog box passes to the
        /// callback function, if one is specified in <see cref="lpfn"/>.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible", Justification = "Reviewed. Suppression is OK here.")]
        public IntPtr lParam;
    }
}