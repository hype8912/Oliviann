namespace Oliviann.Native
{
    #region Usings

    using System;

    #endregion Usings

    /// <summary>
    /// Flags that specify the options for the dialog box.
    /// </summary>
    [Flags]
    public enum BrowseInfoFlags
    {
        /// <summary>
        /// Allow folder junctions such as a library or a compressed file with a
        /// .zip file name extension to be browsed.
        /// </summary>
        BrowseFileJunctions = 0x00010000,

        /// <summary>
        /// Only return computers.
        /// </summary>
        BrowseForComputer = 0x00001000,

        /// <summary>
        /// Only allow the selection of printers.
        /// </summary>
        BrowseForPrinter = 0x00002000,

        /// <summary>
        /// The browse dialog box displays files as well as folders.
        /// </summary>
        BrowseIncludeFiles = 0x00004000,

        /// <summary>
        /// The browse dialog box can display URLs.
        /// </summary>
        BrowseIncludeURLs = 0x00000080,

        /// <summary>
        /// Do not include network folders below the domain level in the dialog
        /// box's tree view control.
        /// </summary>
        DontGoBelowDomain = 0x00000002,

        /// <summary>
        ///  Include an edit control in the browse dialog box that allows the
        ///  user to type the name of an item.
        /// </summary>
        EditBox = 0x00000010,

        /// <summary>
        /// Use the new user interface. Setting this flag provides the user with
        /// a larger dialog box that can be resized. The dialog box has several
        /// new capabilities, including: drag-and-drop capability within the
        /// dialog box, reordering, shortcut menus, new folders, delete, and
        /// other shortcut menu commands.
        /// </summary>
        NewDialogStyle = 0x00000040,

        /// <summary>
        /// Do not include the New Folder button in the browse dialog box.
        /// </summary>
        NoNewFolderButton = 0x00000200,

        /// <summary>
        /// When the selected item is a shortcut, return the PIDL of the
        /// shortcut itself rather than its target.
        /// </summary>
        NoTranslateTargets = 0x00000400,

        /// <summary>
        /// Only return file system ancestors. An ancestor is a subfolder that
        /// is beneath the root folder in the namespace hierarchy.
        /// </summary>
        ReturnFileSystemAncestors = 0x00000008,

        /// <summary>
        /// Only return file system directories.
        /// </summary>
        ReturnOnlyFileSystemDirs = 0x00000001,

        /// <summary>
        /// The browse dialog box can display sharable resources on remote
        /// systems. This is intended for applications that want to expose
        /// remote shares on a local system.
        /// </summary>
        Shareable = 0x00008000,

        /// <summary>
        /// When combined with NewDialogStyle, adds a usage hint to the dialog
        /// box, in place of the edit box. EditBox overrides this flag.
        /// </summary>
        UsageHint = 0x00000100,

        /// <summary>
        /// Use the new user interface, including an edit box. This flag is
        /// equivalent to EditBox | NewDialogStyle.
        /// </summary>
        UseNewUI = EditBox | NewDialogStyle,

        /// <summary>
        /// If the user types an invalid name into the edit box, the browse
        /// dialog box calls the application's BrowseCallbackProc with the
        /// BFFM_VALIDATEFAILED message.
        /// </summary>
        Validate = 0x00000020
    }
}