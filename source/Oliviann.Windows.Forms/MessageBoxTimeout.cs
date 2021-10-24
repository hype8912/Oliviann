namespace Oliviann.Windows.Forms
{
    #region Usings

    using System;
    using System.ComponentModel;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;
    using Oliviann.Native;
    using Oliviann.Properties;

    #endregion Usings

    /// <summary>
    /// Displays a message box that can contain text, buttons, symbols, and
    /// timeouts that inform and instruct the user. This version of the timeout
    /// MessageBox uses the native windows form to display the information to
    /// the user.
    /// </summary>
    public class MessageBoxTimeout
    {
        #region Fields

        /// <summary>
        /// The default number of milliseconds for the message box to timeout.
        /// </summary>
        private const int DEFAULT_TIMEOUT = 30000;

        /// <summary>
        /// The Help button id.
        /// </summary>
        private const int HELP_BUTTON = 16384;

        /// <summary>
        /// The Abort button id.
        /// </summary>
        private const int IDABORT = 3;

        /// <summary>
        /// The Cancel button id.
        /// </summary>
        private const int IDCANCEL = 2;

        /// <summary>
        /// The Ignore button id.
        /// </summary>
        private const int IDIGNORE = 5;

        /// <summary>
        /// The No button id.
        /// </summary>
        private const int IDNO = 7;

        /// <summary>
        /// The OK button id.
        /// </summary>
        private const int IDOK = 1;

        /// <summary>
        /// The Retry button id.
        /// </summary>
        private const int IDRETRY = 4;

        /// <summary>
        /// The Yes button id.
        /// </summary>
        private const int IDYES = 6;

        /// <summary>
        /// Shared array for the help information objects.
        /// </summary>
        private static HelpInfo[] helpInfoTable;

        #endregion Fields

        #region Constructor/Destructor

        /// <summary>
        /// Prevents a default instance of the <see cref="MessageBoxTimeout"/>
        /// class from being created.
        /// </summary>
        private MessageBoxTimeout()
        {
        }

        #endregion Constructor/Destructor

        #region Properties

        /// <summary>
        /// Gets the help info for the current.
        /// </summary>
        /// <value>
        /// The help info.
        /// </value>
        internal static HelpInfo HelpInfo
        {
            get
            {
                if (helpInfoTable != null && helpInfoTable.Length > 0)
                {
                    return helpInfoTable[helpInfoTable.Length - 1];
                }

                return null;
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Displays a message box with the specified text, caption, buttons,
        /// icon, default button, options, and Help button.
        /// </summary>
        /// <param name="text">The text to display in the message box.</param>
        /// <param name="caption">The text to display in the title bar of the
        /// message box.</param>
        /// <param name="buttons">One of the <see cref="MessageBoxButtons"/>
        /// values that specifies which buttons to display in the message box.
        /// </param>
        /// <param name="icon">One of the <see cref="MessageBoxIcon"/> values
        /// that specifies which icon to display in the message box.</param>
        /// <param name="defaultButton">One of the
        /// <see cref="MessageBoxDefaultButton"/> values that specifies the
        /// default button for the message box.</param>
        /// <param name="options">One of the <see cref="MessageBoxOptions"/>
        /// values that specifies which display and association options will be
        /// used for the message box.</param>
        /// <param name="timeOutMilliseconds">The number of milliseconds that
        /// must expire before the <see cref="MessageBoxTimeout"/> automatically
        /// closes.</param>
        /// <returns>
        /// One of the <see cref="DialogResult"/> values.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="buttons"/> is not a member of
        /// <see cref="MessageBoxButtons"/>. -or- <paramref name="icon"/> is not
        /// a member of <see cref="MessageBoxIcon"/>. -or- The
        /// <paramref name="defaultButton"/> specified is not a member of
        /// <see cref="MessageBoxDefaultButton"/>.</exception>
        /// <exception cref="InvalidOperationException">An attempt was made to
        /// display the <see cref="MessageBoxTimeout"/>in a process that is not
        /// running in User Interactive mode. This is specified by the
        /// <see cref="System.Windows.Forms.SystemInformation.UserInteractive"/>
        /// property.</exception>
        /// <exception cref="ArgumentException"><paramref name="options"/>
        /// specified both
        /// <see cref="System.Windows.Forms.MessageBoxOptions.DefaultDesktopOnly"/>
        /// and
        /// <see cref="System.Windows.Forms.MessageBoxOptions.ServiceNotification"/>
        /// . -or- <paramref name="buttons"/>specified an invalid combination of
        /// <see cref="MessageBoxButtons"/>.</exception>
        public static DialogResult Show(
            string text,
            string caption = "",
            MessageBoxButtons buttons = MessageBoxButtons.OK,
            MessageBoxIcon icon = MessageBoxIcon.None,
            MessageBoxDefaultButton defaultButton = MessageBoxDefaultButton.Button1,
            MessageBoxOptions options = 0,
            int timeOutMilliseconds = DEFAULT_TIMEOUT) =>
            Show(
                text,
                caption,
                buttons,
                icon,
                defaultButton,
                options,
                TimeSpan.FromMilliseconds(timeOutMilliseconds));

        /// <summary>
        /// Displays a message box with the specified text, caption, buttons,
        /// icon, default button, options, and Help button.
        /// </summary>
        /// <param name="text">The text to display in the message box.</param>
        /// <param name="caption">The text to display in the title bar of the
        /// message box.</param>
        /// <param name="buttons">One of the <see cref="MessageBoxButtons"/>
        /// values that specifies which buttons to display in the message box.
        /// </param>
        /// <param name="icon">One of the <see cref="MessageBoxIcon"/> values
        /// that specifies which icon to display in the message box.</param>
        /// <param name="defaultButton">One of the
        /// <see cref="MessageBoxDefaultButton"/> values that specifies the
        /// default button for the message box.</param>
        /// <param name="options">One of the <see cref="MessageBoxOptions"/>
        /// values that specifies which display and association options will be
        /// used for the message box.</param>
        /// <param name="timeOut">The time span that must expire before the
        /// <see cref="MessageBoxTimeout"/> automatically closes.</param>
        /// <returns>One of the <see cref="DialogResult" /> values.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="buttons"/> is not a member of
        /// <see cref="MessageBoxButtons"/>. -or- <paramref name="icon"/> is not
        /// a member of <see cref="MessageBoxIcon"/>. -or- The
        /// <paramref name="defaultButton"/> specified is not a member of
        /// <see cref="MessageBoxDefaultButton"/>.</exception>
        /// <exception cref="InvalidOperationException">An attempt was made to
        /// display the <see cref="MessageBoxTimeout"/> in a process that is not
        /// running in User Interactive mode. This is specified by the
        /// <see cref="System.Windows.Forms.SystemInformation.UserInteractive"/>
        /// property.</exception>
        /// <exception cref="ArgumentException"><paramref name="options"/>
        /// specified both
        /// <see cref="System.Windows.Forms.MessageBoxOptions.DefaultDesktopOnly"/>
        /// and
        /// <see cref="System.Windows.Forms.MessageBoxOptions.ServiceNotification"/>
        /// . -or- <paramref name="buttons"/>specified an invalid combination of
        /// <see cref="MessageBoxButtons"/>.
        /// </exception>
        public static DialogResult Show(
            string text,
            string caption,
            MessageBoxButtons buttons,
            MessageBoxIcon icon,
            MessageBoxDefaultButton defaultButton,
            MessageBoxOptions options,
            TimeSpan timeOut) =>
            ShowCore(null, text, caption, buttons, icon, defaultButton, options, false, timeOut);

        /// <summary>
        /// Displays a message box with the specified text, caption, buttons,
        /// icon, default button, options, and Help button.
        /// </summary>
        /// <param name="text">The text to display in the message box.</param>
        /// <param name="helpFilePath">The path and name of the Help file to
        /// display when the user clicks the Help button.</param>
        /// <param name="keyword">The Help keyword to display when the user
        /// clicks the Help button.</param>
        /// <param name="caption">The text to display in the title bar of the
        /// message box.</param>
        /// <param name="buttons">One of the <see cref="MessageBoxButtons"/>
        /// values that specifies which buttons to display in the message box.
        /// </param>
        /// <param name="icon">One of the <see cref="MessageBoxIcon"/> values
        /// that specifies which icon to display in the message box.</param>
        /// <param name="defaultButton">One of the
        /// <see cref="MessageBoxDefaultButton"/> values that specifies the
        /// default button for the message box.</param>
        /// <param name="options">One of the <see cref="MessageBoxOptions"/>
        /// values that specifies which display and association options will be
        /// used for the message box.</param>
        /// <param name="timeOutMilliseconds">The number of milliseconds that
        /// must expire before the <see cref="MessageBoxTimeout"/> automatically
        /// closes.</param>
        /// <returns>One of the <see cref="DialogResult" /> values.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="buttons"/> is not a member of
        /// <see cref="MessageBoxButtons"/>. -or- <paramref name="icon"/> is not
        /// a member of <see cref="MessageBoxIcon"/>. -or- The
        /// <paramref name="defaultButton"/> specified is not a member of
        /// <see cref="MessageBoxDefaultButton"/>.</exception>
        /// <exception cref="InvalidOperationException">An attempt was made to
        /// display the <see cref="MessageBoxTimeout"/> in a process that is not
        /// running in User Interactive mode. This is specified by the
        /// <see cref="System.Windows.Forms.SystemInformation.UserInteractive"/>
        /// property.</exception>
        /// <exception cref="ArgumentException"><paramref name="options"/>
        /// specified both
        /// <see cref="System.Windows.Forms.MessageBoxOptions.DefaultDesktopOnly"/>
        /// and
        /// <see cref="System.Windows.Forms.MessageBoxOptions.ServiceNotification"/>
        /// . -or- <paramref name="buttons"/>specified an invalid combination of
        /// <see cref="MessageBoxButtons"/>.
        /// </exception>
        public static DialogResult Show(
            string text,
            string helpFilePath,
            string keyword = "",
            string caption = "",
            MessageBoxButtons buttons = MessageBoxButtons.OK,
            MessageBoxIcon icon = MessageBoxIcon.None,
            MessageBoxDefaultButton defaultButton = MessageBoxDefaultButton.Button1,
            MessageBoxOptions options = 0,
            int timeOutMilliseconds = DEFAULT_TIMEOUT)
        {
            var hpi = new HelpInfo(helpFilePath, keyword);
            return ShowCore(null, text, caption, buttons, icon, defaultButton, options, hpi, TimeSpan.FromMilliseconds(timeOutMilliseconds));
        }

        /// <summary>
        /// Displays a message box with the specified text, caption, buttons,
        /// icon, default button, options, and Help button, using the specified
        /// Help file, HelpNavigator, and Help topic.
        /// </summary>
        /// <param name="text">The text to display in the message box.</param>
        /// <param name="helpFilePath">The path and name of the Help file to
        /// display when the user clicks the Help button.</param>
        /// <param name="navigator">One of the <see cref="HelpNavigator"/>
        /// values.</param>
        /// <param name="param">The numeric ID of the Help topic to display when
        /// the user clicks the Help button.</param>
        /// <param name="caption">The text to display in the title bar of the
        /// message box.</param>
        /// <param name="buttons">One of the <see cref="MessageBoxButtons"/>
        /// values that specifies which buttons to display in the message box.
        /// </param>
        /// <param name="icon">One of the <see cref="MessageBoxIcon"/> values
        /// that specifies which icon to display in the message box.</param>
        /// <param name="defaultButton">One of the
        /// <see cref="MessageBoxDefaultButton"/> values that specifies the
        /// default button for the message box.</param>
        /// <param name="options">One of the <see cref="MessageBoxOptions"/>
        /// values that specifies which display and association options will be
        /// used for the message box.</param>
        /// <param name="timeOutMilliseconds">The number of milliseconds that
        /// must expire before the <see cref="MessageBoxTimeout"/> automatically
        /// closes.</param>
        /// <returns>One of the <see cref="DialogResult" /> values.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="buttons"/> is not a member of
        /// <see cref="MessageBoxButtons"/>. -or- <paramref name="icon"/> is not
        /// a member of <see cref="MessageBoxIcon"/>. -or- The
        /// <paramref name="defaultButton"/> specified is not a member of
        /// <see cref="MessageBoxDefaultButton"/>.</exception>
        /// <exception cref="InvalidOperationException">An attempt was made to
        /// display the <see cref="MessageBoxTimeout"/> in a process that is not
        /// running in User Interactive mode. This is specified by the
        /// <see cref="System.Windows.Forms.SystemInformation.UserInteractive"/>
        /// property.</exception>
        /// <exception cref="ArgumentException"><paramref name="options"/>
        /// specified both
        /// <see cref="System.Windows.Forms.MessageBoxOptions.DefaultDesktopOnly"/>
        /// and
        /// <see cref="System.Windows.Forms.MessageBoxOptions.ServiceNotification"/>
        /// . -or- <paramref name="buttons"/>specified an invalid combination of
        /// <see cref="MessageBoxButtons"/>.
        /// </exception>
        public static DialogResult Show(
            string text,
            string helpFilePath,
            HelpNavigator navigator,
            object param = null,
            string caption = "",
            MessageBoxButtons buttons = MessageBoxButtons.OK,
            MessageBoxIcon icon = MessageBoxIcon.None,
            MessageBoxDefaultButton defaultButton = MessageBoxDefaultButton.Button1,
            MessageBoxOptions options = 0,
            int timeOutMilliseconds = DEFAULT_TIMEOUT)
        {
            var hpi = new HelpInfo(helpFilePath, navigator, param);
            return ShowCore(null, text, caption, buttons, icon, defaultButton, options, hpi, TimeSpan.FromMilliseconds(timeOutMilliseconds));
        }

        /// <summary>
        /// Displays a message box with the specified text, caption, buttons,
        /// icon, default button, options, and Help button.
        /// </summary>
        /// <param name="owner">An implementation of <see cref="IWin32Window"/>
        /// that will own the modal dialog box.</param>
        /// <param name="text">The text to display in the message box.</param>
        /// <param name="caption">The text to display in the title bar of the
        /// message box.</param>
        /// <param name="buttons">One of the <see cref="MessageBoxButtons"/>
        /// values that specifies which buttons to display in the message box.
        /// </param>
        /// <param name="icon">One of the <see cref="MessageBoxIcon"/> values
        /// that specifies which icon to display in the message box.</param>
        /// <param name="defaultButton">One of the
        /// <see cref="MessageBoxDefaultButton"/> values that specifies the
        /// default button for the message box.</param>
        /// <param name="options">One of the <see cref="MessageBoxOptions"/>
        /// values that specifies which display and association options will be
        /// used for the message box.</param>
        /// <param name="timeOutMilliseconds">The number of milliseconds that
        /// must expire before the <see cref="MessageBoxTimeout"/>
        /// automatically closes.</param>
        /// <returns>
        /// One of the <see cref="DialogResult"/> values.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="buttons"/> is not a member of
        /// <see cref="MessageBoxButtons"/>. -or- <paramref name="icon"/> is not
        /// a member of <see cref="MessageBoxIcon"/>. -or- The
        /// <paramref name="defaultButton"/> specified is not a member of
        /// <see cref="MessageBoxDefaultButton"/>.</exception>
        /// <exception cref="InvalidOperationException">An attempt was made to
        /// display the <see cref="MessageBoxTimeout"/>in a process that is not
        /// running in User Interactive mode. This is specified by the
        /// <see cref="System.Windows.Forms.SystemInformation.UserInteractive"/>
        /// property.</exception>
        /// <exception cref="ArgumentException"><paramref name="options"/>
        /// specified both
        /// <see cref="System.Windows.Forms.MessageBoxOptions.DefaultDesktopOnly"/>
        /// and
        /// <see cref="System.Windows.Forms.MessageBoxOptions.ServiceNotification"/>
        /// . -or- <paramref name="buttons"/>specified an invalid combination of
        /// <see cref="MessageBoxButtons"/>.</exception>
        public static DialogResult Show(
            IWin32Window owner,
            string text,
            string caption = "",
            MessageBoxButtons buttons = MessageBoxButtons.OK,
            MessageBoxIcon icon = MessageBoxIcon.None,
            MessageBoxDefaultButton defaultButton = MessageBoxDefaultButton.Button1,
            MessageBoxOptions options = 0,
            int timeOutMilliseconds = DEFAULT_TIMEOUT) =>
            Show(
                owner,
                text,
                caption,
                buttons,
                icon,
                defaultButton,
                options,
                TimeSpan.FromMilliseconds(timeOutMilliseconds));

        /// <summary>
        /// Displays a message box with the specified text, caption, buttons,
        /// icon, default button, options, and Help button.
        /// </summary>
        /// <param name="owner">An implementation of <see cref="IWin32Window"/>
        /// that will own the modal dialog box.</param>
        /// <param name="text">The text to display in the message box.</param>
        /// <param name="caption">The text to display in the title bar of the
        /// message box.</param>
        /// <param name="buttons">One of the <see cref="MessageBoxButtons"/>
        /// values that specifies which buttons to display in the message box.
        /// </param>
        /// <param name="icon">One of the <see cref="MessageBoxIcon"/> values
        /// that specifies which icon to display in the message box.</param>
        /// <param name="defaultButton">One of the
        /// <see cref="MessageBoxDefaultButton"/> values that specifies the
        /// default button for the message box.</param>
        /// <param name="options">One of the <see cref="MessageBoxOptions"/>
        /// values that specifies which display and association options will be
        /// used for the message box.</param>
        /// <param name="timeOut">The time span that must expire before the
        /// <see cref="MessageBoxTimeout"/> automatically closes.</param>
        /// <returns>One of the <see cref="DialogResult" /> values.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="buttons"/> is not a member of
        /// <see cref="MessageBoxButtons"/>. -or- <paramref name="icon"/> is not
        /// a member of <see cref="MessageBoxIcon"/>. -or- The
        /// <paramref name="defaultButton"/> specified is not a member of
        /// <see cref="MessageBoxDefaultButton"/>.</exception>
        /// <exception cref="InvalidOperationException">An attempt was made to
        /// display the <see cref="MessageBoxTimeout"/> in a process that is not
        /// running in User Interactive mode. This is specified by the
        /// <see cref="System.Windows.Forms.SystemInformation.UserInteractive"/>
        /// property.</exception>
        /// <exception cref="ArgumentException"><paramref name="options"/>
        /// specified both
        /// <see cref="System.Windows.Forms.MessageBoxOptions.DefaultDesktopOnly"/>
        /// and
        /// <see cref="System.Windows.Forms.MessageBoxOptions.ServiceNotification"/>
        /// . -or- <paramref name="buttons"/>specified an invalid combination of
        /// <see cref="MessageBoxButtons"/>.
        /// </exception>
        public static DialogResult Show(
            IWin32Window owner,
            string text,
            string caption,
            MessageBoxButtons buttons,
            MessageBoxIcon icon,
            MessageBoxDefaultButton defaultButton,
            MessageBoxOptions options,
            TimeSpan timeOut) =>
            ShowCore(owner, text, caption, buttons, icon, defaultButton, options, false, timeOut);

        /// <summary>
        /// Displays a message box with the specified text, caption, buttons,
        /// icon, default button, options, and Help button, using the specified
        /// Help file and Help keyword.
        /// </summary>
        /// <param name="owner">An implementation of <see cref="IWin32Window"/>
        /// that will own the modal dialog box.</param>
        /// <param name="text">The text to display in the message box.</param>
        /// <param name="helpFilePath">The path and name of the Help file to
        /// display when the user clicks the Help button.</param>
        /// <param name="keyword">The Help keyword to display when the user
        /// clicks the Help button.</param>
        /// <param name="caption">The text to display in the title bar of the
        /// message box.</param>
        /// <param name="buttons">One of the <see cref="MessageBoxButtons"/>
        /// values that specifies which buttons to display in the message box.
        /// </param>
        /// <param name="icon">One of the <see cref="MessageBoxIcon"/> values
        /// that specifies which icon to display in the message box.</param>
        /// <param name="defaultButton">One of the
        /// <see cref="MessageBoxDefaultButton"/> values that specifies the
        /// default button for the message box.</param>
        /// <param name="options">One of the <see cref="MessageBoxOptions"/>
        /// values that specifies which display and association options will be
        /// used for the message box.</param>
        /// <param name="timeOutMilliseconds">The number of milliseconds that
        /// must expire before the <see cref="MessageBoxTimeout"/> automatically
        /// closes.</param>
        /// <returns>One of the <see cref="DialogResult" /> values.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="buttons"/> is not a member of
        /// <see cref="MessageBoxButtons"/>. -or- <paramref name="icon"/> is not
        /// a member of <see cref="MessageBoxIcon"/>. -or- The
        /// <paramref name="defaultButton"/> specified is not a member of
        /// <see cref="MessageBoxDefaultButton"/>.</exception>
        /// <exception cref="InvalidOperationException">An attempt was made to
        /// display the <see cref="MessageBoxTimeout"/> in a process that is not
        /// running in User Interactive mode. This is specified by the
        /// <see cref="System.Windows.Forms.SystemInformation.UserInteractive"/>
        /// property.</exception>
        /// <exception cref="ArgumentException"><paramref name="options"/>
        /// specified both
        /// <see cref="System.Windows.Forms.MessageBoxOptions.DefaultDesktopOnly"/>
        /// and
        /// <see cref="System.Windows.Forms.MessageBoxOptions.ServiceNotification"/>
        /// . -or- <paramref name="buttons"/>specified an invalid combination of
        /// <see cref="MessageBoxButtons"/>.
        /// </exception>
        public static DialogResult Show(
            IWin32Window owner,
            string text,
            string helpFilePath,
            string keyword = "",
            string caption = "",
            MessageBoxButtons buttons = MessageBoxButtons.OK,
            MessageBoxIcon icon = MessageBoxIcon.None,
            MessageBoxDefaultButton defaultButton = MessageBoxDefaultButton.Button1,
            MessageBoxOptions options = 0,
            int timeOutMilliseconds = DEFAULT_TIMEOUT)
        {
            var hpi = new HelpInfo(helpFilePath, keyword);
            return ShowCore(owner, text, caption, buttons, icon, defaultButton, options, hpi, TimeSpan.FromMilliseconds(timeOutMilliseconds));
        }

        /// <summary>
        /// Displays a message box with the specified text, caption, buttons,
        /// icon, default button, options, and Help button, using the specified
        /// Help file, HelpNavigator, and Help topic.
        /// </summary>
        /// <param name="owner">An implementation of <see cref="IWin32Window"/>
        /// that will own the modal dialog box.</param>
        /// <param name="text">The text to display in the message box.</param>
        /// <param name="helpFilePath">The path and name of the Help file to
        /// display when the user clicks the Help button.</param>
        /// <param name="navigator">One of the <see cref="HelpNavigator"/>
        /// values.</param>
        /// <param name="param">The numeric ID of the Help topic to display when
        /// the user clicks the Help button.</param>
        /// <param name="caption">The text to display in the title bar of the
        /// message box.</param>
        /// <param name="buttons">One of the <see cref="MessageBoxButtons"/>
        /// values that specifies which buttons to display in the message box.
        /// </param>
        /// <param name="icon">One of the <see cref="MessageBoxIcon"/> values
        /// that specifies which icon to display in the message box.</param>
        /// <param name="defaultButton">One of the
        /// <see cref="MessageBoxDefaultButton"/> values that specifies the
        /// default button for the message box.</param>
        /// <param name="options">One of the <see cref="MessageBoxOptions"/>
        /// values that specifies which display and association options will be
        /// used for the message box.</param>
        /// <param name="timeOutMilliseconds">The number of milliseconds that
        /// must expire before the <see cref="MessageBoxTimeout"/> automatically
        /// closes.</param>
        /// <returns>One of the <see cref="DialogResult" /> values.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="buttons"/> is not a member of
        /// <see cref="MessageBoxButtons"/>. -or- <paramref name="icon"/> is not
        /// a member of <see cref="MessageBoxIcon"/>. -or- The
        /// <paramref name="defaultButton"/> specified is not a member of
        /// <see cref="MessageBoxDefaultButton"/>.</exception>
        /// <exception cref="InvalidOperationException">An attempt was made to
        /// display the <see cref="MessageBoxTimeout"/> in a process that is not
        /// running in User Interactive mode. This is specified by the
        /// <see cref="System.Windows.Forms.SystemInformation.UserInteractive"/>
        /// property.</exception>
        /// <exception cref="ArgumentException"><paramref name="options"/>
        /// specified both
        /// <see cref="System.Windows.Forms.MessageBoxOptions.DefaultDesktopOnly"/>
        /// and
        /// <see cref="System.Windows.Forms.MessageBoxOptions.ServiceNotification"/>
        /// . -or- <paramref name="buttons"/>specified an invalid combination of
        /// <see cref="MessageBoxButtons"/>.
        /// </exception>
        public static DialogResult Show(
            IWin32Window owner,
            string text,
            string helpFilePath,
            HelpNavigator navigator,
            object param = null,
            string caption = "",
            MessageBoxButtons buttons = MessageBoxButtons.OK,
            MessageBoxIcon icon = MessageBoxIcon.None,
            MessageBoxDefaultButton defaultButton = MessageBoxDefaultButton.Button1,
            MessageBoxOptions options = 0,
            int timeOutMilliseconds = DEFAULT_TIMEOUT)
        {
            var hpi = new HelpInfo(helpFilePath, navigator, param);
            return ShowCore(owner, text, caption, buttons, icon, defaultButton, options, hpi, TimeSpan.FromMilliseconds(timeOutMilliseconds));
        }

        /// <summary>
        /// Displays a message box with the specified text, caption, buttons,
        /// icon, default button, options, and Help button.
        /// </summary>
        /// <param name="owner">An implementation of <see cref="IWin32Window"/>
        /// that will own the modal dialog box.</param>
        /// <param name="text">The text to display in the message box.</param>
        /// <param name="caption">The text to display in the title bar of the
        /// message box.</param>
        /// <param name="buttons">One of the <see cref="MessageBoxButtons"/>
        /// values that specifies which buttons to display in the message box.
        /// </param>
        /// <param name="icon">One of the <see cref="MessageBoxIcon"/> values
        /// that specifies which icon to display in the message box.</param>
        /// <param name="defaultButton">One of the
        /// <see cref="MessageBoxDefaultButton"/> values that specifies the
        /// default button for the message box.</param>
        /// <param name="options">One of the <see cref="MessageBoxOptions"/>
        /// values that specifies which display and association options will be
        /// used for the message box.</param>
        /// <param name="showHelp">true for the help button to be displayed to
        /// the user.</param>
        /// <param name="timeOut">The time span that must expire before the
        /// <see cref="MessageBoxTimeout"/> automatically closes.</param>
        /// <returns>One of the <see cref="DialogResult" /> values.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="buttons"/> is not a member of
        /// <see cref="MessageBoxButtons"/>. -or- <paramref name="icon"/> is not
        /// a member of <see cref="MessageBoxIcon"/>. -or- The
        /// <paramref name="defaultButton"/> specified is not a member of
        /// <see cref="MessageBoxDefaultButton"/>.</exception>
        /// <exception cref="InvalidOperationException">An attempt was made to
        /// display the <see cref="MessageBoxTimeout"/> in a process that is not
        /// running in User Interactive mode. This is specified by the
        /// <see cref="System.Windows.Forms.SystemInformation.UserInteractive"/>
        /// property.</exception>
        /// <exception cref="ArgumentException"><paramref name="options"/>
        /// specified both
        /// <see cref="System.Windows.Forms.MessageBoxOptions.DefaultDesktopOnly"/>
        /// and
        /// <see cref="System.Windows.Forms.MessageBoxOptions.ServiceNotification"/>
        /// . -or- <paramref name="buttons"/>specified an invalid combination of
        /// <see cref="MessageBoxButtons"/>.
        /// </exception>
        /// <exception cref="Win32Exception">Cannot load the shell32.dll DLL
        /// into memory.</exception>
        private static DialogResult ShowCore(
            IWin32Window owner,
            string text,
            string caption,
            MessageBoxButtons buttons,
            MessageBoxIcon icon,
            MessageBoxDefaultButton defaultButton,
            MessageBoxOptions options,
            bool showHelp,
            TimeSpan timeOut)
        {
            // Validate the input parameters for various conditions.
            ValidateInputs(owner, buttons, icon, defaultButton, options, showHelp);

            if ((options & ~(MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign)) != 0)
            {
                IntSecurity.UnmanagedCode.Demand();
            }

            IntSecurity.SafeSubWindows.Demand();
            int type = showHelp ? HELP_BUTTON : 0;
            type |= (int)(buttons | (MessageBoxButtons)(int)icon | (MessageBoxButtons)(int)defaultButton | (MessageBoxButtons)(int)options);
            IntPtr zero = IntPtr.Zero;
            if (showHelp || ((options & (MessageBoxOptions.ServiceNotification | MessageBoxOptions.DefaultDesktopOnly)) == 0))
            {
                zero = owner == null ? UnsafeNativeMethods.GetActiveWindow() : GetSafeHandle(owner);
            }

            DialogResult result =
                Win32ToDialogResult(
                    UnsafeNativeMethods.MessageBoxTimeout(
                        new HandleRef(owner, zero),
                        text,
                        caption,
                        type,
                        0,
                        timeOut.TotalMilliseconds.ToInt32()));

            UnsafeNativeMethods.SendMessage(new HandleRef(owner, zero), 7, 0, 0);
            return result;
        }

        /// <summary>
        /// Displays a message box with the specified text, caption, buttons,
        /// icon, default button, options, and Help button, using the specified
        /// Help file, HelpNavigator, and Help topic.
        /// </summary>
        /// <param name="owner">An implementation of <see cref="IWin32Window"/>
        /// that will own the modal dialog box.</param>
        /// <param name="text">The text to display in the message box.</param>
        /// <param name="caption">The text to display in the title bar of the
        /// message box.</param>
        /// <param name="buttons">One of the <see cref="MessageBoxButtons"/>
        /// values that specifies which buttons to display in the message box.
        /// </param>
        /// <param name="icon">One of the <see cref="MessageBoxIcon"/> values
        /// that specifies which icon to display in the message box.</param>
        /// <param name="defaultButton">One of the
        /// <see cref="MessageBoxDefaultButton"/> values that specifies the
        /// default button for the message box.</param>
        /// <param name="options">One of the <see cref="MessageBoxOptions"/>
        /// values that specifies which display and association options will be
        /// used for the message box.</param>
        /// <param name="hpi">The <see cref="HelpInfo"/> instance.</param>
        /// <param name="timeOut">The time span that must expire before the
        /// <see cref="MessageBoxTimeout"/> automatically closes.</param>
        /// <returns>One of the <see cref="DialogResult" /> values.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="buttons"/> is not a member of
        /// <see cref="MessageBoxButtons"/>. -or- <paramref name="icon"/> is not
        /// a member of <see cref="MessageBoxIcon"/>. -or- The
        /// <paramref name="defaultButton"/> specified is not a member of
        /// <see cref="MessageBoxDefaultButton"/>.</exception>
        /// <exception cref="InvalidOperationException">An attempt was made to
        /// display the <see cref="MessageBoxTimeout"/> in a process that is not
        /// running in User Interactive mode. This is specified by the
        /// <see cref="System.Windows.Forms.SystemInformation.UserInteractive"/>
        /// property.</exception>
        /// <exception cref="ArgumentException"><paramref name="options"/>
        /// specified both
        /// <see cref="System.Windows.Forms.MessageBoxOptions.DefaultDesktopOnly"/>
        /// and
        /// <see cref="System.Windows.Forms.MessageBoxOptions.ServiceNotification"/>
        /// . -or- <paramref name="buttons"/>specified an invalid combination of
        /// <see cref="MessageBoxButtons"/>.
        /// </exception>
        private static DialogResult ShowCore(
            IWin32Window owner,
            string text,
            string caption,
            MessageBoxButtons buttons,
            MessageBoxIcon icon,
            MessageBoxDefaultButton defaultButton,
            MessageBoxOptions options,
            HelpInfo hpi,
            TimeSpan timeOut)
        {
            DialogResult result;

            try
            {
                PushHelpInfo(hpi);
                result = ShowCore(owner, text, caption, buttons, icon, defaultButton, options, true, timeOut);
            }
            finally
            {
                PopHelpInfo();
            }

            return result;
        }

        #endregion Methods

        #region Helpers

        /// <summary>
        /// Pulls the help info object from the help info array.
        /// </summary>
        private static void PopHelpInfo()
        {
            if (helpInfoTable == null)
            {
                return;
            }

            if (helpInfoTable.Length == 1)
            {
                helpInfoTable = null;
            }
            else
            {
                int length = helpInfoTable.Length - 1;
                var destinationArray = new HelpInfo[length];
                Array.Copy(helpInfoTable, destinationArray, length);
                helpInfoTable = destinationArray;
            }
        }

        /// <summary>
        /// Pushes the help info object to the help info array.
        /// </summary>
        /// <param name="hpi">The help info object data.</param>
        private static void PushHelpInfo(HelpInfo hpi)
        {
            HelpInfo[] infoArray;
            int length = 0;
            if (helpInfoTable == null)
            {
                infoArray = new HelpInfo[1];
            }
            else
            {
                length = helpInfoTable.Length;
                infoArray = new HelpInfo[length + 1];
                Array.Copy(helpInfoTable, infoArray, length);
            }

            infoArray[length] = hpi;
            helpInfoTable = infoArray;
        }

        /// <summary>
        /// Gets a window handle safely.
        /// </summary>
        /// <param name="window">An implementation of <see cref="IWin32Window"/>
        /// that will own the modal dialog box.</param>
        /// <returns>A handle to the specified <paramref name="window"/>.
        /// </returns>
        /// <exception cref="System.ComponentModel.Win32Exception">6 - The
        /// current <paramref name="window"/> is not a Window.</exception>
        private static IntPtr GetSafeHandle(IWin32Window window)
        {
            if (window is Control control)
            {
                return control.Handle;
            }

            IntSecurity.AllWindows.Demand();
            IntPtr zero = window.Handle;
            if ((zero != IntPtr.Zero) && !UnsafeNativeMethods.IsWindow(new HandleRef(null, zero)))
            {
                throw new Win32Exception(6);
            }

            return zero;
        }

        /// <summary>
        /// Converts a Win32 result to a .NET <see cref="DialogResult"/>.
        /// </summary>
        /// <param name="value">The Win32 value.</param>
        /// <returns>One of the <see cref="DialogResult" /> values.</returns>
        private static DialogResult Win32ToDialogResult(int value)
        {
            switch (value)
            {
                case IDOK:
                    return DialogResult.OK;

                case IDCANCEL:
                    return DialogResult.Cancel;

                case IDABORT:
                    return DialogResult.Abort;

                case IDRETRY:
                    return DialogResult.Retry;

                case IDIGNORE:
                    return DialogResult.Ignore;

                case IDYES:
                    return DialogResult.Yes;

                case IDNO:
                    return DialogResult.No;
            }

            return DialogResult.No;
        }

        /// <summary>
        /// Validates some of the input parameters before displaying message
        /// box.
        /// </summary>
        /// <param name="owner">An implementation of <see cref="IWin32Window"/>
        /// that will own the modal dialog box.</param>
        /// <param name="buttons">One of the <see cref="MessageBoxButtons"/>
        /// values that specifies which buttons to display in the message box.
        /// </param>
        /// <param name="icon">One of the <see cref="MessageBoxIcon"/> values
        /// that specifies which icon to display in the message box.</param>
        /// <param name="defaultButton">One of the
        /// <see cref="MessageBoxDefaultButton"/> values that specifies the
        /// default button for the message box.</param>
        /// <param name="options">One of the <see cref="MessageBoxOptions"/>
        /// values that specifies which display and association options will be
        /// used for the message box.</param>
        /// <param name="showHelp">true for the help button to be displayed to
        /// the user.</param>
        private static void ValidateInputs(
            IWin32Window owner,
            MessageBoxButtons buttons,
            MessageBoxIcon icon,
            MessageBoxDefaultButton defaultButton,
            MessageBoxOptions options,
            bool showHelp)
        {
            if (!((int)buttons).IsBetweenOrEqual(0, 5))
            {
                throw new ArgumentOutOfRangeException(nameof(buttons), (int)buttons, Resources.ERR_ValueOutOfRange.FormatWith("MessageBoxButtons"));
            }

            if (!icon.IsWithinShiftedRange(4, 0, 4))
            {
                throw new ArgumentOutOfRangeException(nameof(icon), (int)icon, Resources.ERR_ValueOutOfRange.FormatWith("MessageBoxIcon"));
            }

            if (!defaultButton.IsWithinShiftedRange(8, 0, 2))
            {
                throw new ArgumentOutOfRangeException(nameof(defaultButton), (int)defaultButton, Resources.ERR_ValueOutOfRange.FormatWith("MessageBoxDefaultButton"));
            }

            if (!System.Windows.Forms.SystemInformation.UserInteractive && ((options & (MessageBoxOptions.ServiceNotification | MessageBoxOptions.DefaultDesktopOnly)) == 0))
            {
                throw new InvalidOperationException(Resources.MessageBoxTimeoutNoUserInteractiveEx);
            }

            if ((owner != null) && ((options & (MessageBoxOptions.ServiceNotification | MessageBoxOptions.DefaultDesktopOnly)) != 0))
            {
                throw new ArgumentException(Resources.MessageBoxTimeoutNoWindowOwner, nameof(options));
            }

            if (showHelp && ((options & (MessageBoxOptions.ServiceNotification | MessageBoxOptions.DefaultDesktopOnly)) != 0))
            {
                throw new ArgumentException(Resources.MessageBoxTimeoutHelpButtonEx, nameof(options));
            }
        }

        #endregion Helpers
    }
}