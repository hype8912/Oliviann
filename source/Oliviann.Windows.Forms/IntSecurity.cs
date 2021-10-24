namespace Oliviann.Windows.Forms
{
    #region Usings

    using System.Security;
    using System.Security.Permissions;

    #endregion Usings

    /// <summary>
    /// Represents a
    /// </summary>
    internal static class IntSecurity
    {
        #region Fields

        /// <summary>
        /// Backing field for property <see cref="AllWindows"/>.
        /// </summary>
        private static CodeAccessPermission allWindows;

        /// <summary>
        /// Backing field for property <see cref="SafeSubWindows"/>.
        /// </summary>
        private static CodeAccessPermission safeSubWindows;

        /// <summary>
        /// Backing field for property <see cref="UnmanagedCode"/>.
        /// </summary>
        private static CodeAccessPermission unmanagedCode;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets access permission for querying for all windows.
        /// </summary>
        /// <value>
        /// Access permission for querying for all windows.
        /// </value>
        public static CodeAccessPermission AllWindows
        {
            get { return allWindows ??= new UIPermission(UIPermissionWindow.AllWindows); }
        }

        /// <summary>
        /// Gets access permission for querying for sub windows.
        /// </summary>
        /// <value>
        /// Access permission for querying for sub windows.
        /// </value>
        public static CodeAccessPermission SafeSubWindows
        {
            get { return safeSubWindows ??= new UIPermission(UIPermissionWindow.SafeSubWindows); }
        }

        /// <summary>
        /// Gets security permission for executing unmanaged code.
        /// </summary>
        /// <value>
        /// Security permission for executing unmanaged code.
        /// </value>
        public static CodeAccessPermission UnmanagedCode
        {
            get { return unmanagedCode ??= new SecurityPermission(SecurityPermissionFlag.UnmanagedCode); }
        }

        #endregion Properties
    }
}