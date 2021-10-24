namespace Oliviann.Native
{
    #region Usings

    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.InteropServices;

    #endregion Usings

    /// <summary>
    /// Represents a combo box status information.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:AccessibleFieldsMustBeginWithUpperCaseLetter",
        Justification = "Reviewed. Suppression is OK here.")]
    public struct COMBOBOXINFO
    {
        /// <summary>
        /// The size, in bytes, of the structure.
        /// </summary>
        public int cbSize;

        /// <summary>
        /// A <see cref="RECT"/> structure that specifies the coordinates of the
        /// edit box.
        /// </summary>
        public RECT rcItem;

        /// <summary>
        /// A <see cref="RECT"/> structure that specifies the coordinates of the
        /// button that contains the drop-down arrow.
        /// </summary>
        public RECT rcButton;

        /// <summary>
        /// The combo box button state.
        /// </summary>
        public ComboBoxButtonState stateButton;

        /// <summary>
        /// A handle to the combo box.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible",
            Justification = "Reviewed. Suppression is OK here.")]
        public IntPtr hwndCombo;

        /// <summary>
        /// A handle to the edit box.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible",
            Justification = "Reviewed. Suppression is OK here.")]
        public IntPtr hwndEdit;

        /// <summary>
        /// A handle to the drop-down list.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible",
            Justification = "Reviewed. Suppression is OK here.")]
        public IntPtr hwndList;
    }
}