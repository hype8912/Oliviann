namespace Oliviann.Windows.Forms
{
    #region Usings

    using System;
    using System.ComponentModel;
    using System.Threading;
    using System.Windows;
    using System.Windows.Forms;
    using System.Windows.Interop;
    using Oliviann.Properties;

    #endregion Usings

    /// <summary>
    /// Displays a message box that can contain text, buttons, symbols, and
    /// timeouts that inform and instruct the user. This version of the timeout
    /// MessageBox uses a custom form to display the information to the user.
    /// </summary>
    public class MessageBoxTimeout2
    {
        #region Fields

        /// <summary>
        /// The default number of milliseconds for the message box to timeout.
        /// </summary>
        private const int DEFAULT_TIMEOUT = 30000;

        #endregion Fields

        #region Constructor/Destructor

        /// <summary>
        /// Prevents a default instance of the <see cref="MessageBoxTimeout2"/>
        /// class from being created.
        /// </summary>
        private MessageBoxTimeout2()
        {
        }

        #endregion Constructor/Destructor

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
        /// <param name="options">One of the
        /// <see cref="System.Windows.Forms.MessageBoxOptions"/>values that
        /// specifies which display and association options will be used for the
        /// message box.</param>
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
            System.Windows.Forms.MessageBoxOptions options = 0,
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
        /// Displays a message box in front of the specified window. The message
        /// box displays a message, title bar caption, button, and icon; and
        /// accepts a default message box result, complies with the specified
        /// options, and returns a result.
        /// </summary>
        /// <param name="text">A <see cref="String"/> that specifies the text to
        /// display.</param>
        /// <param name="caption">A <see cref="String"/> that specifies the
        /// title bar caption to display.</param>
        /// <param name="buttons">A <see cref="MessageBoxButton"/> value that
        /// specifies which button or buttons to display.</param>
        /// <param name="icon">A <see cref="MessageBoxImage"/> value that
        /// specifies the icon to display.</param>
        /// <param name="defaultResult">A <see cref="MessageBoxResult"/> value
        /// that specifies the default result of the message box.</param>
        /// <param name="options">A
        /// <see cref="System.Windows.Forms.MessageBoxOptions"/> value object
        /// that specifies the options.</param>
        /// <param name="timeOutMilliseconds">The number of milliseconds that
        /// must expire before the <see cref="MessageBoxTimeout"/> automatically
        /// closes.</param>
        /// <returns>A <see cref="MessageBoxResult"/> value that specifies which
        /// message box button is clicked by the user.</returns>
        public static MessageBoxResult Show(
            string text,
            string caption = "",
            MessageBoxButton buttons = MessageBoxButton.OK,
            MessageBoxImage icon = MessageBoxImage.None,
            MessageBoxResult defaultResult = MessageBoxResult.OK,
            System.Windows.MessageBoxOptions options = System.Windows.MessageBoxOptions.None,
            int timeOutMilliseconds = DEFAULT_TIMEOUT) =>
            Show(
                text,
                caption,
                buttons,
                icon,
                defaultResult,
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
        /// <param name="options">One of the
        /// <see cref="System.Windows.Forms.MessageBoxOptions"/>values that
        /// specifies which display and association options will be used for the
        /// message box.</param>
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
            System.Windows.Forms.MessageBoxOptions options,
            TimeSpan timeOut) =>
            ShowCore(null, text, caption, buttons, icon, defaultButton, options, false, timeOut);

        /// <summary>
        /// Displays a message box in front of the specified window. The message
        /// box displays a message, title bar caption, button, and icon; and
        /// accepts a default message box result, complies with the specified
        /// options, and returns a result.
        /// </summary>
        /// <param name="text">A <see cref="String"/> that specifies the text to
        /// display.</param>
        /// <param name="caption">A <see cref="String"/> that specifies the
        /// title bar caption to display.</param>
        /// <param name="buttons">A <see cref="MessageBoxButton"/> value that
        /// specifies which button or buttons to display.</param>
        /// <param name="icon">A <see cref="MessageBoxImage"/> value that
        /// specifies the icon to display.</param>
        /// <param name="defaultResult">A <see cref="MessageBoxResult"/> value
        /// that specifies the default result of the message box.</param>
        /// <param name="options">A
        /// <see cref="System.Windows.Forms.MessageBoxOptions"/> value object
        /// that specifies the options.</param>
        /// <param name="timeOut">The time span that must expire before the
        /// <see cref="MessageBoxTimeout"/> automatically closes.</param>
        /// <returns>A <see cref="MessageBoxResult"/> value that specifies which
        /// message box button is clicked by the user.</returns>
        public static MessageBoxResult Show(
            string text,
            string caption,
            MessageBoxButton buttons,
            MessageBoxImage icon,
            MessageBoxResult defaultResult,
            System.Windows.MessageBoxOptions options,
            TimeSpan timeOut) =>
            ShowCore(IntPtr.Zero, text, caption, buttons, icon, defaultResult, options, timeOut);

        /// <summary>
        /// Displays a message box with the specified text, caption, buttons,
        /// icon, default button, options, and Help button.
        /// </summary>
        /// <param name="owner">An implementation of
        /// <see cref="System.Windows.Forms.IWin32Window"/>that will own the
        /// modal dialog box.</param>
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
        /// <param name="options">One of the
        /// <see cref="System.Windows.Forms.MessageBoxOptions"/>values that
        /// specifies which display and association options will be used for the
        /// message box.</param>
        /// <param name="timeOutMilliseconds">The number of milliseconds that
        /// must expire before the <see cref="MessageBoxTimeout"/>automatically
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
            System.Windows.Forms.IWin32Window owner,
            string text,
            string caption = "",
            MessageBoxButtons buttons = MessageBoxButtons.OK,
            MessageBoxIcon icon = MessageBoxIcon.None,
            MessageBoxDefaultButton defaultButton = MessageBoxDefaultButton.Button1,
            System.Windows.Forms.MessageBoxOptions options = 0,
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
        /// Displays a message box in front of the specified window. The message
        /// box displays a message, title bar caption, button, and icon; and
        /// accepts a default message box result, complies with the specified
        /// options, and returns a result.
        /// </summary>
        /// <param name="owner">A <see cref="Window"/> that represents the owner
        /// window of the message box.</param>
        /// <param name="text">A <see cref="String" /> that specifies the text
        /// to display.</param>
        /// <param name="caption">A <see cref="String" /> that specifies the
        /// title bar caption to display.</param>
        /// <param name="buttons">A <see cref="MessageBoxButton" /> value that
        /// specifies which button or buttons to display.</param>
        /// <param name="icon">A <see cref="MessageBoxImage" /> value that
        /// specifies the icon to display.</param>
        /// <param name="defaultResult">A <see cref="MessageBoxResult" /> value
        /// that specifies the default result of the message box.</param>
        /// <param name="options">A
        /// <see cref="System.Windows.Forms.MessageBoxOptions" /> value object
        /// that specifies the options.</param>
        /// <param name="timeOutMilliseconds">The number of milliseconds that
        /// must expire before the <see cref="MessageBoxTimeout" />automatically
        /// closes.</param>
        /// <returns>
        /// A <see cref="MessageBoxResult" /> value that specifies which message
        /// box button is clicked by the user.
        /// </returns>
        public static MessageBoxResult Show(
            Window owner,
            string text,
            string caption,
            MessageBoxButton buttons = MessageBoxButton.OK,
            MessageBoxImage icon = MessageBoxImage.None,
            MessageBoxResult defaultResult = MessageBoxResult.OK,
            System.Windows.MessageBoxOptions options = System.Windows.MessageBoxOptions.None,
            int timeOutMilliseconds = DEFAULT_TIMEOUT) =>
            Show(
                owner,
                text,
                caption,
                buttons,
                icon,
                defaultResult,
                options,
                TimeSpan.FromMilliseconds(timeOutMilliseconds));

        /// <summary>
        /// Displays a message box with the specified text, caption, buttons,
        /// icon, default button, options, and Help button.
        /// </summary>
        /// <param name="owner">An implementation of
        /// <see cref="System.Windows.Forms.IWin32Window"/>that will own the
        /// modal dialog box.</param>
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
        /// <param name="options">One of the
        /// <see cref="System.Windows.Forms.MessageBoxOptions"/>values that
        /// specifies which display and association options will be used for the
        /// message box.</param>
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
            System.Windows.Forms.IWin32Window owner,
            string text,
            string caption,
            MessageBoxButtons buttons,
            MessageBoxIcon icon,
            MessageBoxDefaultButton defaultButton,
            System.Windows.Forms.MessageBoxOptions options,
            TimeSpan timeOut) =>
            ShowCore(owner, text, caption, buttons, icon, defaultButton, options, false, timeOut);

        /// <summary>
        /// Displays a message box in front of the specified window. The message
        /// box displays a message, title bar caption, button, and icon; and
        /// accepts a default message box result, complies with the specified
        /// options, and returns a result.
        /// </summary>
        /// <param name="owner">A <see cref="Window"/> that represents the owner
        /// window of the message box.</param>
        /// <param name="text">A <see cref="string" /> that specifies the text
        /// to display.</param>
        /// <param name="caption">A <see cref="string" /> that specifies the
        /// title bar caption to display.</param>
        /// <param name="buttons">A <see cref="MessageBoxButton" /> value that
        /// specifies which button or buttons to display.</param>
        /// <param name="icon">A <see cref="MessageBoxImage" /> value that
        /// specifies the icon to display.</param>
        /// <param name="defaultResult">A <see cref="MessageBoxResult" /> value
        /// that specifies the default result of the message box.</param>
        /// <param name="options">A
        /// <see cref="System.Windows.Forms.MessageBoxOptions" /> value object
        /// that specifies the options.</param>
        /// <param name="timeOut">The time span that must expire before the
        /// <see cref="MessageBoxTimeout"/> automatically closes.</param>
        /// <returns>
        /// A <see cref="MessageBoxResult" /> value that specifies which message
        /// box button is clicked by the user.
        /// </returns>
        public static MessageBoxResult Show(
            Window owner,
            string text,
            string caption,
            MessageBoxButton buttons,
            MessageBoxImage icon,
            MessageBoxResult defaultResult,
            System.Windows.MessageBoxOptions options,
            TimeSpan timeOut) =>
            ShowCore(
                new WindowInteropHelper(owner).Handle,
                text,
                caption,
                buttons,
                icon,
                defaultResult,
                options,
                timeOut);

        /// <summary>
        /// Displays a message box with the specified text, caption, buttons,
        /// icon, default button, options, and Help button.
        /// </summary>
        /// <param name="owner">An implementation of
        /// <see cref="System.Windows.Forms.IWin32Window"/>that will own the
        /// modal dialog box.</param>
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
        /// <param name="options">One of the
        /// <see cref="System.Windows.Forms.MessageBoxOptions"/>values that
        /// specifies which display and association options will be used for the
        /// message box. Partially supported.</param>
        /// <param name="showHelp">true for the help button to be displayed to
        /// the user. NOT USED</param>
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
            System.Windows.Forms.IWin32Window owner,
            string text,
            string caption,
            MessageBoxButtons buttons,
            MessageBoxIcon icon,
            MessageBoxDefaultButton defaultButton,
            System.Windows.Forms.MessageBoxOptions options,
            bool showHelp,
            TimeSpan timeOut)
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

            ////if (!System.Windows.Forms.SystemInformation.UserInteractive && ((options & (System.Windows.Forms.MessageBoxOptions.ServiceNotification | System.Windows.Forms.MessageBoxOptions.DefaultDesktopOnly)) == 0))
            ////{
            ////    throw new InvalidOperationException("Showing a modal dialog box or form when the application is not running in UserInteractive mode is not a valid operation. Specify the ServiceNotification or DefaultDesktopOnly style to display a notification from a service application.");
            ////}

            ////if ((owner != null) && ((options & (System.Windows.Forms.MessageBoxOptions.ServiceNotification | System.Windows.Forms.MessageBoxOptions.DefaultDesktopOnly)) != 0))
            ////{
            ////    throw new ArgumentException(@"Showing a service notification message box with an owner window is not a valid operation. Use the Show method that does not take an owner.", "options");
            ////}

            ////if (showHelp && ((options & (MessageBoxOptions.ServiceNotification | MessageBoxOptions.DefaultDesktopOnly)) != 0))
            ////{
            ////    throw new ArgumentException(@"Showing a service notification message box with a Help button is not a valid operation.", "options");
            ////}

            ////if ((options & ~(MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign)) != 0)
            ////{
            ////    IntSecurity.UnmanagedCode.Demand();
            ////}

            var result = DialogResult.None;
            FrmAdvancedMessageBox frm = null;

            try
            {
                frm = new FrmAdvancedMessageBox
                          {
                              Text = text,
                              Caption = caption,
                              Buttons = buttons,
                              Icon = icon,
                              DefaultButton = defaultButton,
                              Timeout = timeOut
                          };

                if ((options & System.Windows.Forms.MessageBoxOptions.RtlReading) != 0)
                {
                    frm.RightToLeft = RightToLeft.Yes;
                    frm.RightToLeftLayout = true;
                }

                if (Thread.CurrentThread.IsBackground)
                {
                    var dummy = new DummyForm();
                    dummy.Load += (sender, args) =>
                        {
                            result = frm.ShowDialog(owner);
                            ((Form)sender).Close();
                        };
                    System.Windows.Forms.Application.Run(dummy);
                    dummy.Dispose();
                }
                else
                {
                    result = frm.ShowDialog(owner);
                }
            }
            finally
            {
                frm.DisposeSafe();
            }

            return result;
        }

        /// <summary>
        /// Displays a message box in front of the specified window. The message
        /// box displays a message, title bar caption, button, and icon; and
        /// accepts a default message box result, complies with the specified
        /// options, and returns a result.
        /// </summary>
        /// <param name="owner">A <see cref="Window"/> that represents the owner
        /// window of the message box.</param>
        /// <param name="text">A <see cref="String"/> that specifies the text to
        /// display.</param>
        /// <param name="caption">A <see cref="String"/> that specifies the
        /// title bar caption to display.</param>
        /// <param name="buttons">A <see cref="MessageBoxButton"/> value that
        /// specifies which button or buttons to display.</param>
        /// <param name="icon">A <see cref="MessageBoxImage"/> value that
        /// specifies the icon to display.</param>
        /// <param name="defaultResult">A <see cref="MessageBoxResult"/> value
        /// that specifies the default result of the message box.</param>
        /// <param name="options">A
        /// <see cref="System.Windows.Forms.MessageBoxOptions"/> value object
        /// that specifies the options.</param>
        /// <param name="timeOut">The time span that must expire before the
        /// <see cref="MessageBoxTimeout"/> automatically closes.</param>
        /// <returns>
        /// A <see cref="MessageBoxResult"/> value that specifies which message
        /// box button is clicked by the user.
        /// </returns>
        private static MessageBoxResult ShowCore(
            IntPtr owner,
            string text,
            string caption,
            MessageBoxButton buttons,
            MessageBoxImage icon,
            MessageBoxResult defaultResult,
            System.Windows.MessageBoxOptions options,
            TimeSpan timeOut)
        {
            var defaultButton = (MessageBoxDefaultButton)DefaultResultToButtonNumber(defaultResult, buttons);

            DialogResult result = ShowCore(
                new WindowWrapper(owner),
                text,
                caption,
                (MessageBoxButtons)buttons,
                (MessageBoxIcon)icon,
                defaultButton,
                (System.Windows.Forms.MessageBoxOptions)options,
                false,
                timeOut);

            return (MessageBoxResult)result;
        }

        #endregion Methods

        #region Helpers

        /// <summary>
        /// Gets the default button number for the specified result.
        /// </summary>
        /// <param name="result">The dialog button result.</param>
        /// <param name="button">The message box buttons.</param>
        /// <returns>An integer representing the correct default button to
        /// number.</returns>
        private static int DefaultResultToButtonNumber(MessageBoxResult result, MessageBoxButton button)
        {
            if (result == MessageBoxResult.None)
            {
                return 0;
            }

            switch (button)
            {
                case MessageBoxButton.OK:
                    return 0;

                case MessageBoxButton.OKCancel:
                    return result != MessageBoxResult.Cancel ? 0 : 256;

                case MessageBoxButton.YesNoCancel:
                    if (result != MessageBoxResult.No)
                    {
                        return result == MessageBoxResult.Cancel ? 512 : 0;
                    }

                    return 256;

                case MessageBoxButton.YesNo:
                    return result != MessageBoxResult.No ? 0 : 256;

                default:
                    return 0;
            }
        }

        #endregion Helpers
    }
}