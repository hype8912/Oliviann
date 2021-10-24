namespace Oliviann.Native
{
    #region Usings

    using System;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
#if !NETSTANDARD1_3
    using System.Runtime.ConstrainedExecution;
#endif
    using System.Runtime.InteropServices;
    using System.Security;
    using System.Text;
    using Oliviann.Diagnostics;
    using Oliviann.Win32.SafeHandles;
    using Oliviann.Windows;
    using Microsoft.Win32.SafeHandles;

    #endregion Usings

    /// <summary>
    /// Represents a class unsafe static methods. Basically a class of
    /// P/Invokes.
    /// </summary>
    [SuppressMessage("Microsoft.Interoperability", "CA1401:PInvokesShouldNotBeVisible",
        Justification = "This is a common shared library.")]
#if !NETSTANDARD1_3
    [System.Security.SuppressUnmanagedCodeSecurity]
#endif
    public static class UnsafeNativeMethods
    {
        /// <summary>
        /// Sets the current char set of the version.
        /// </summary>
#if NETSTANDARD1_3
        private const CharSet CurrentCharSet = CharSet.Unicode;
#else
        private const CharSet CurrentCharSet = CharSet.Auto;
#endif

        #region Delegates

        /// <summary>
        /// Specifies an application-defined callback function used to send
        /// messages to, and process messages from, a Browse dialog box
        /// displayed in response to a call to SHBrowseForFolder.
        /// </summary>
        /// <param name="hwnd">The window handle of the browse dialog box.
        /// </param>
        /// <param name="msg">The dialog box event that generated the message.
        /// </param>
        /// <param name="lp">A value whose meaning depends on the event
        /// specified in <paramref name="msg"/>.</param>
        /// <param name="wp">An application-defined value that was specified in
        /// the <paramref name="lp"/> member of the BROWSEINFO structure used in
        /// the call to SHBrowseForFolder.</param>
        /// <returns>Returns zero to dismiss the dialog or nonzero to keep the
        /// dialog displayed.</returns>
        public delegate int BrowseCallbackProc(IntPtr hwnd, int msg, IntPtr lp, IntPtr wp);

        #endregion Delegates

        /// <summary>
        ///  Enables or disables privileges in the specified access token.
        ///  Enabling or disabling privileges in an access token requires
        ///  TOKEN_ADJUST_PRIVILEGES access.
        /// </summary>
        /// <param name="tokenHandle">A handle to the access token that contains
        /// the privileges to be modified.</param>
        /// <param name="disableAllPrivileges">Specifies whether the function
        /// disables all of the token's privileges. If this value is <c>true</c>
        /// , the function disables all privileges and ignores the
        /// <paramref name="newState"/> parameter. If it is <c>false</c>, the
        /// function modifies privileges based on the information pointed to by
        /// the <paramref name="newState"/> parameter.
        /// </param>
        /// <param name="newState">A pointer to a TOKEN_PRIVILEGES structure
        /// that specifies an array of privileges and their attributes.</param>
        /// <param name="bufferLength">Specifies the size, in bytes, of the
        /// buffer pointed to by the <paramref name="previousState"/> parameter.
        /// </param>
        /// <param name="previousState">A pointer to a buffer that the function
        /// fills with a TOKEN_PRIVILEGES structure that contains the previous
        /// state of any privileges that the function modifies.</param>
        /// <param name="returnLength">A pointer to a variable that receives the
        /// required size, in bytes, of the buffer pointed to by the
        /// <paramref name="previousState"/> parameter.</param>
        /// <returns><c>true</c> if the function succeeds; otherwise,
        /// <c>false</c>.</returns>
        [DllImport(@"advapi32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool AdjustTokenPrivileges(
            SafeTokenHandle tokenHandle,
            [MarshalAs(UnmanagedType.Bool)] bool disableAllPrivileges,
            ref TOKEN_PRIVILEGES newState,
            uint bufferLength,
            IntPtr previousState,
            IntPtr returnLength);

        /// <summary>
        /// Brings the specified window to the top of the Z order. If the window
        /// is a top-level window, it is activated. If the window is a child
        /// window, the top-level parent window associated with the child window
        /// is activated.
        /// </summary>
        /// <param name="hWnd">A handle to the window to bring to the top of the
        /// Z order.</param>
        /// <returns>
        /// <c>True</c> if the function succeeds; otherwise, <c>false</c>.
        /// </returns>
        /// <remarks>
        ///     See
        ///     <a href="http://msdn.microsoft.com/en-us/library/ms632673(VS.85).aspx">
        ///     link</a> for more information.
        /// </remarks>
        [DllImport(@"user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool BringWindowToTop(IntPtr hWnd);

        /// <summary>
        /// Closes an open object handle.
        /// </summary>
        /// <param name="handle">A valid handle to an open object.</param>
        /// <returns>True if the handle closed successfully; otherwise, false.
        /// </returns>
#if NETSTANDARD1_3
        [DllImport(@"kernel32.dll", SetLastError = true, CharSet = CurrentCharSet)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool CloseHandle(IntPtr handle);
#else

        [DllImport(@"kernel32.dll", SetLastError = true, CharSet = CurrentCharSet)]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        [SuppressUnmanagedCodeSecurity]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool CloseHandle(IntPtr handle);

#endif

        /// <summary>
        /// Frees a block of task memory previously allocated through a call to
        /// the <c>CoTaskMemAlloc</c> or <c>CoTaskMemRealloc</c> function.
        /// </summary>
        /// <param name="pv">A pointer to the memory block to be freed. If this
        /// parameter is NULL, the function has no effect.</param>
        [DllImport(@"ole32.dll", CharSet = CurrentCharSet, SetLastError = true, ExactSpelling = true)]
        public static extern void CoTaskMemFree(IntPtr pv);

        /// <summary>
        /// Defines, redefines, or deletes MS-DOS device names.
        /// </summary>
        /// <param name="dwFlags">The controllable aspects of the
        /// DefineDosDevice function.</param>
        /// <param name="lpDeviceName">A pointer to an MS-DOS device name string
        /// specifying the device the function is defining, redefining, or
        /// deleting. The device name string must not have a colon as the last
        /// character, unless a drive letter is being defined, redefined, or
        /// deleted. For example, drive C would be the string "C:". In no case
        /// is a trailing backslash ("\") allowed.</param>
        /// <param name="lpTargetPath">A pointer to a path string that will
        /// implement this device. The string is an MS-DOS path string unless
        /// the DDD_RAW_TARGET_PATH flag is specified, in which case this string
        /// is a path string.</param>
        /// <returns>If the function succeeds, the return value is nonzero;
        /// otherwise, the return value is zero.</returns>
        /// <remarks>
        /// See
        /// <a href="http://msdn.microsoft.com/en-us/library/windows/desktop/aa363904.aspx">
        /// link</a> for more information.
        /// </remarks>
        [DllImport(@"kernel32.dll", SetLastError = true)]
        public static extern bool DefineDosDevice(DefineDosDeviceOptions dwFlags, string lpDeviceName, string lpTargetPath);

        /// <summary>
        /// Sends a control code directly to a specified device driver, causing
        /// the corresponding device to perform the corresponding operation.
        /// </summary>
        /// <param name="fileHandle">A handle to the device on which the
        /// operation is to be performed. The device is typically a volume,
        /// directory, file, or stream. To retrieve a device handle, use the
        /// CreateFile function.</param>
        /// <param name="ioControlCode">The control code for the operation. This
        /// value identifies the specific operation to be performed and the type
        /// of device on which to perform it.</param>
        /// <param name="inBuffer">A pointer to the input buffer that contains
        /// the data required to perform the operation.</param>
        /// <param name="cbInBuffer">The size of the input buffer, in bytes.
        /// </param>
        /// <param name="outBuffer">A pointer to the output buffer that is to
        /// receive the data returned by the operation. </param>
        /// <param name="cbOutBuffer">The size of the output buffer, in bytes.
        /// </param>
        /// <param name="cbBytesReturned">A pointer to a variable that receives
        /// the size of the data stored in the output buffer, in bytes.</param>
        /// <param name="overlapped">A pointer to an OVERLAPPED structure.
        /// </param>
        /// <returns>If the function succeeds, the return value is nonzero;
        /// otherwise, the return value is zero.</returns>
        /// <remarks>
        /// See
        /// <a href="http://msdn.microsoft.com/en-us/library/windows/desktop/aa363216.aspx">
        /// link</a> for more information.
        /// </remarks>
        [DllImport(@"kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern bool DeviceIoControl(
                                                  SafeFileHandle fileHandle,
                                                  uint ioControlCode,
                                                  IntPtr inBuffer,
                                                  uint cbInBuffer,
                                                  IntPtr outBuffer,
                                                  uint cbOutBuffer,
                                                  out uint cbBytesReturned,
                                                  IntPtr overlapped);

        /// <summary>
        /// Create a completely new access token for specified existing token.
        /// </summary>
        /// <param name="existingTokenHandle">The existing token handle.</param>
        /// <param name="impersonationLevel">The security impersonation level of
        /// the new token.</param>
        /// <param name="duplicateTokenHandle">The new duplicate token handle.
        /// </param>
        /// <returns>True if the operation completed successfully; otherwise,
        /// false.</returns>
        [DllImport(@"advapi32.dll", SetLastError = true)]
        public static extern bool DuplicateToken(
                                                 IntPtr existingTokenHandle,
                                                 SecurityImpersonationLevel impersonationLevel,
                                                 out IntPtr duplicateTokenHandle);

        /// <summary>
        /// Logs off the interactive user, shuts down the system, or shuts down
        /// and restarts the system.
        /// </summary>
        /// <param name="uFlags">The shutdown type.</param>
        /// <param name="dwReason">The reason for initiating the shutdown.
        /// </param>
        /// <returns><c>true</c> if the command was successfully sent to
        /// Windows; otherwise, <c>false</c>.</returns>
        [DllImport(@"user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool ExitWindowsEx(ExitWindows uFlags, ShutdownReason dwReason);

        /// <summary>
        /// Retrieves a handle to the top-level window whose class name and
        /// window name match the specified strings.
        /// </summary>
        /// <param name="className">Name of the class.</param>
        /// <param name="windowName">The window name (the window's title). If
        /// this parameter is NULL, all window names match.</param>
        /// <returns>
        /// If the function succeeds, the return value is a handle to the window
        /// that has the specified class name and window name. If the function
        /// fails, the return value is NULL. To get extended error information,
        /// call Marshall.GetLastError.
        /// </returns>
        /// <remarks>
        /// See <a href="http://msdn.microsoft.com/en-us/library/ms633499.aspx">
        /// link</a> for more information.
        /// </remarks>
        [DllImport(@"user32.dll", CharSet = CurrentCharSet, SetLastError = true)]
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist",
            Justification = "This is a common shared library.")]
        [SuppressMessage("Microsoft.Globalization", "CA2101:SpecifyMarshalingForPInvokeStringArguments", MessageId = "0",
            Justification = "False positive.")]
        public static extern IntPtr FindWindowW(string className, string windowName);

        /// <summary>
        /// Retrieves the window handle to the active window attached to the
        /// calling thread's message queue.
        /// </summary>
        /// <returns>The return value is the handle to the active window
        /// attached to the calling thread's message queue. Otherwise, the
        /// return value is NULL.</returns>
        /// <remarks>
        /// See
        /// <a href="http://msdn.microsoft.com/en-us/library/windows/desktop/ms646292.aspx">
        /// link</a> for more information.
        /// </remarks>
        [DllImport(@"user32.dll", CharSet = CurrentCharSet, ExactSpelling = true)]
        public static extern IntPtr GetActiveWindow();

        /// <summary>
        /// Retrieves information about the specified combo box.
        /// </summary>
        /// <param name="hWnd">A handle to the combo box.</param>
        /// <param name="pcbi">A pointer to a <see cref="COMBOBOXINFO"/>
        /// structure that receives the information. You must set
        /// <see cref="COMBOBOXINFO.cbSize"/> before calling this function.
        /// </param>
        /// <returns>True if the function succeeds; otherwise, false.</returns>
        [DllImport(@"user32.dll", CharSet = CurrentCharSet)]
        public static extern bool GetComboBoxInfo(IntPtr hWnd, ref COMBOBOXINFO pcbi);

        /// <summary>
        /// Retrieves a handle to the foreground window (the window with which
        /// the user is currently working). The system assigns a slightly higher
        /// priority to the thread that creates the foreground window than it
        /// does to other threads.
        /// </summary>
        /// <returns>
        /// The return value is a handle to the foreground window. The
        /// foreground window can be NULL in certain circumstances, such as when
        /// a window is losing activation.
        /// </returns>
        /// <remarks>
        /// See <a href="http://msdn.microsoft.com/en-us/library/ms633505.aspx">
        /// link</a> for more information.
        /// </remarks>
        [DllImport(@"user32.dll", CharSet = CurrentCharSet, ExactSpelling = true)]
        public static extern IntPtr GetForegroundWindow();

        /// <summary>
        /// Retrieves a module handle for the specified module. The module must
        /// have been loaded by the calling process.
        /// </summary>
        /// <param name="moduleName">The name of the loaded module (either a
        /// .dll or .exe file). If the file name extension is omitted, the
        /// default library extension .dll is appended. The file name string can
        /// include a trailing point character (.) to indicate that the module
        /// name has no extension. The string does not have to specify a path.
        /// When specifying a path, be sure to use backslashes (\), not forward
        /// slashes (/). The name is compared (case independently) to the names
        /// of modules currently mapped into the address space of the calling
        /// process.</param>
        /// <returns>If the function succeeds, the return value is a handle to
        /// the specified module; otherwise, NULL.</returns>
        /// <remarks>
        /// See <a href="http://msdn.microsoft.com/en-us/library/windows/desktop/ms683199.aspx">
        /// link</a> for more information.
        /// </remarks>
        [DllImport(@"kernel32.dll", CharSet = CurrentCharSet)]
        public static extern IntPtr GetModuleHandle(string moduleName);

        /// <summary>
        /// The GetSystemTimePreciseAsFileTime function retrieves the current
        /// system date and time with the highest possible level of precision
        /// (&lt;1us). The retrieved information is in Coordinated Universal
        /// Time (UTC) format.
        /// </summary>
        /// <param name="fileTime">The file time.</param>
        [DllImport("kernel32.dll", CallingConvention = CallingConvention.Winapi)]
        public static extern void GetSystemTimePreciseAsFileTime(out long fileTime);

#if !NETSTANDARD1_3

        /// <summary>
        /// Determines whether the specified window handle identifies an
        /// existing window.
        /// </summary>
        /// <param name="hWnd">A handle to the window to be tested.</param>
        /// <returns>
        /// true if the specified window handle identifies an existing window;
        /// otherwise, false.
        /// </returns>
        /// <remarks>
        /// See <a href="http://msdn.microsoft.com/en-us/library/windows/desktop/ms633528.aspx">
        /// link</a> for more information.
        /// </remarks>
        [DllImport(@"user32.dll", CharSet = CurrentCharSet, ExactSpelling = true)]
        public static extern bool IsWindow(HandleRef hWnd);

#endif

        /// <summary>
        /// Determines whether the specified process is running under WOW64.
        /// </summary>
        /// <param name="hSourceProcessHandle">A handle to the current process.
        /// </param>
        /// <param name="isWow64">True if the process is running under WOW64;
        /// otherwise, false.</param>
        /// <returns>True if the function succeeds; otherwise, false.</returns>
        /// <remarks>
        /// See
        /// <a href="http://msdn.microsoft.com/en-us/library/windows/desktop/ms684139.aspx">link
        /// </a> for more information.
        /// </remarks>
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool IsWow64Process([In] IntPtr hSourceProcessHandle, [Out] out bool isWow64);

#if !NETSTANDARD1_3

        /// <summary>
        /// Attempts to log a user on to the local computer. You cannot use
        /// LogonUser to log on to a remote computer.
        /// </summary>
        /// <param name="lpszUsername">The name of the user account to log on
        /// to.</param>
        /// <param name="lpszDomain">The name of the domain or server whose
        /// account database contains the <paramref name="lpszUsername"/>
        /// account.</param>
        /// <param name="lpszPassword">The plain text password for the user
        /// account specified by <paramref name="lpszUsername"/>.</param>
        /// <param name="dwLogonType">The type of logon operation to perform.
        /// </param>
        /// <param name="dwLogonProvider">the logon provider.</param>
        /// <param name="phToken">A reference to a token that represents the
        /// specified user.</param>
        /// <returns>Returns a non-zero value for successful; otherwise, zero.
        /// </returns>
        [DllImport(@"advapi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern bool LogonUser(
                                            string lpszUsername,
                                            string lpszDomain,
                                            string lpszPassword,
                                            LogonType dwLogonType,
                                            LogonProvider dwLogonProvider,
                                            out SafeTokenHandle phToken);

#endif

        /// <summary>
        /// Retrieves the locally unique identifier (LUID) used on a specified
        /// system to locally represent the specified privilege name.
        /// </summary>
        /// <param name="lpSystemName">A pointer to a <c>null</c>-terminated
        /// string that specifies the name of the system on which the privilege
        /// name is retrieved.</param>
        /// <param name="lpName">A pointer to a <c>null</c>-terminated string
        /// that specifies the name of the privilege, as defined in the
        /// <c>Winnt</c>.h header file.</param>
        /// <param name="lpLuid">A pointer to a variable that receives the LUID
        /// by which the privilege is known on the system specified by the
        /// <paramref name="lpSystemName"/> parameter.</param>
        /// <returns><c>true</c> if the lookup completed successfully; otherwise,
        /// <c>false</c>.</returns>
        [DllImport(@"advapi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool LookupPrivilegeValue(string lpSystemName, string lpName, out LUID lpLuid);

        /// <summary>Sends a email message.</summary>
        /// <param name="lhSession">Handle to a simple MAPI session or zero.
        /// </param>
        /// <param name="windowHandle">Parent window handle or zero.</param>
        /// <param name="message">The email message instance.</param>
        /// <param name="flags">The bitmask option flags.</param>
        /// <param name="reserved">Reserved. Must be zero.</param>
        /// <returns>A result code of the operation.</returns>
        [DllImport("MAPI32.DLL")]
        public static extern int MAPISendMail(
            IntPtr lhSession,
            IntPtr windowHandle,
            MapiMessage message,
            MapiSendMethod flags,
            uint reserved);

#if !NETSTANDARD1_3

        /// <summary>
        /// Displays a modal dialog box that contains a system icon, a set of
        /// buttons, and a brief application-specific message, such as status or
        /// error information. The message box returns an integer value that
        /// indicates which button the user clicked or automatically timed out.
        /// </summary>
        /// <param name="hWnd">A handle to the owner window of the message box
        /// to be created. If this parameter is NULL, the message box has no
        /// owner window.</param>
        /// <param name="text">The message to be displayed. If the string
        /// consists of more than one line, you can separate the lines using a
        /// carriage return and/or linefeed character between each line.</param>
        /// <param name="caption">The dialog box title. If this parameter is
        /// NULL, the default title is Error.</param>
        /// <param name="type">The contents and behavior of the dialog box. This
        /// parameter can be a combination of flags from the following groups of
        /// flags.</param>
        /// <param name="wLanguageId">The w language id.</param>
        /// <param name="milliseconds">The number of milliseconds to wait before
        /// the message box is automatically closed.</param>
        /// <returns>
        /// If a message box has a Cancel button, the function returns the
        /// IDCANCEL value if either the ESC key is pressed or the Cancel button
        /// is selected. If the message box has no Cancel button, pressing ESC
        /// has no effect. If the function fails, the return value is zero. To
        /// get extended error information, call GetLastError. If the function
        /// succeeds, the return value is one of the menu-item values.
        /// </returns>
        /// <remarks>
        /// See
        /// <a href="http://msdn.microsoft.com/en-us/library/windows/desktop/ms645505.aspx">
        /// link</a> for more information.
        /// </remarks>
        [DllImport(@"user32.dll", CharSet = CurrentCharSet)]
        public static extern int MessageBoxTimeout(HandleRef hWnd, string text, string caption, int type, short wLanguageId, int milliseconds);

#endif

        /// <summary>
        /// Opens the access token associated with a process.
        /// </summary>
        /// <param name="processHandle">A handle to the process whose access
        /// token is opened.</param>
        /// <param name="desiredAccess">Specifies an access mask that specifies
        /// the requested types of access to the access token.</param>
        /// <param name="tokenHandle">A pointer to a handle that identifies the
        /// newly opened access token when the function returns.</param>
        /// <returns><c>true</c> if the process completed successfully;
        /// otherwise,
        /// <c>false</c>.</returns>
        [DllImport(@"advapi32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool OpenProcessToken(SafeHandle processHandle, uint desiredAccess, out SafeTokenHandle tokenHandle);

        /// <summary>
        /// Terminates the impersonation of a client application.
        /// </summary>
        /// <returns>True is the operation is successful; otherwise, false.
        /// </returns>
        [DllImport(@"advapi32.dll", SetLastError = true)]
        public static extern bool RevertToSelf();

        /// <summary>
        /// Calls the window procedure for the specified window using a string
        /// lParam and does not return until the window procedure has processed
        /// the message.
        /// </summary>
        /// <param name="hWnd">A handle to the window whose window procedure
        /// will receive the message.</param>
        /// <param name="msg">The message to be sent.</param>
        /// <param name="wParam">Additional message-specific information.
        /// </param>
        /// <param name="lParam">Additional message-specific information.
        /// </param>
        /// <returns>The return value specifies the result of the message
        /// processing; it depends on the message sent.</returns>
        [DllImport(@"user32.dll", CharSet = CurrentCharSet)]
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1625:ElementDocumentationMustNotBeCopiedAndPasted",
            Justification = "Reviewed. Suppression is OK here.")]
        public static extern IntPtr SendMessage(SafeHandle hWnd, int msg, int wParam, string lParam);

#if !NETSTANDARD1_3

        /// <summary>
        /// Calls the window procedure for the specified window using a string
        /// lParam and does not return until the window procedure has processed
        /// the message.
        /// </summary>
        /// <param name="hWnd">A handle to the window whose window procedure
        /// will receive the message.</param>
        /// <param name="msg">The message to be sent.</param>
        /// <param name="wParam">Additional message-specific information.
        /// </param>
        /// <param name="lParam">Additional message-specific information.
        /// </param>
        /// <returns>The return value specifies the result of the message
        /// processing; it depends on the message sent.</returns>
        [DllImport(@"user32.dll", CharSet = CurrentCharSet)]
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1625:ElementDocumentationMustNotBeCopiedAndPasted",
            Justification = "Reviewed. Suppression is OK here.")]
        public static extern IntPtr SendMessage(IntPtr hWnd, int msg, int wParam, string lParam);

        /// <summary>
        /// Calls the window procedure for the specified window using an integer
        /// lParam and does not return until the window procedure has processed
        /// the message.
        /// </summary>
        /// <param name="hWnd">A handle to the window whose window procedure
        /// will receive the message.</param>
        /// <param name="msg">The message to be sent.</param>
        /// <param name="wParam">Additional message-specific information.
        /// </param>
        /// <param name="lParam">Additional message-specific information.
        /// </param>
        /// <returns>The return value specifies the result of the message
        /// processing; it depends on the message sent.</returns>
        [DllImport(@"user32.dll", CharSet = CurrentCharSet)]
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1625:ElementDocumentationMustNotBeCopiedAndPasted",
            Justification = "Reviewed. Suppression is OK here.")]
        public static extern IntPtr SendMessage(HandleRef hWnd, int msg, int wParam, int lParam);

#endif

        /// <summary>
        /// Brings the thread that created the specified window into the
        /// foreground and activates the window. Keyboard input is directed to
        /// the window, and various visual cues are changed for the user. The
        /// system assigns a slightly higher priority to the thread that created
        /// the foreground window than it does to other threads.
        /// </summary>
        /// <param name="hWnd">A handle to the window that should be activated
        /// and brought to the foreground.</param>
        /// <returns>
        /// <c>True</c> if the window was brought to the foreground; otherwise,
        /// <c>false</c>.
        /// </returns>
        /// <remarks>
        ///     See
        ///     <a href="http://msdn.microsoft.com/en-us/library/ms633539.aspx">
        ///     link</a> for more information.
        /// </remarks>
        [DllImport(@"user32.dll", CharSet = CurrentCharSet, ExactSpelling = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
#if NETFRAMEWORK
        public static extern bool SetForegroundWindow(IntPtr hWnd);

#else
        public static extern bool SetForegroundWindow(SafeHandle hWnd);
#endif

        /// <summary>
        /// Sets the specified window's show state.
        /// </summary>
        /// <param name="hWnd">A handle to the window.</param>
        /// <param name="cmdShow">Controls how the window is to be shown.
        /// </param>
        /// <returns>
        /// True if the window was previously hidden; otherwise, false.
        /// </returns>
        /// <remarks>
        ///     See
        ///     <a href="http://msdn.microsoft.com/en-us/library/ms633548.aspx">
        ///     link</a> for more information.
        /// </remarks>
        [DllImport(@"user32.dll", CharSet = CurrentCharSet)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool ShowWindow(IntPtr hWnd, int cmdShow);

        /// <summary>
        /// Continues an enumeration of network resources that was started by a
        /// call to the <see cref="WNetOpenEnum"/> function.
        /// </summary>
        /// <param name="hEnum">Handle that identifies an enumeration instance.
        /// This handle must be returned by the <see cref="WNetOpenEnum"/>
        /// function.</param>
        /// <param name="lpcCount">Pointer to a variable specifying the number
        /// of entries requested. If the number requested is –1, the function
        /// returns as many entries as possible.</param>
        /// <param name="lpBuffer">Pointer to the buffer that receives the
        /// enumeration results. The results are returned as an array of
        /// <see cref="NetResource"/> structures.</param>
        /// <param name="lpBufferSize">Pointer to a variable that specifies the
        /// size of the <paramref name="lpBuffer"/> parameter, in bytes. If the
        /// buffer is too small to receive even one entry, this parameter
        /// receives the required size of the buffer.</param>
        /// <returns>The number of entries retrieved.</returns>
        [DllImport(@"mpr.dll")]
        public static extern uint WNetEnumResource(IntPtr hEnum, ref int lpcCount, IntPtr lpBuffer, ref uint lpBufferSize);

        /// <summary>
        /// Starts an enumeration of network resources or existing connections.
        /// You can continue the enumeration by calling the
        /// <see cref="WNetEnumResource"/> function.
        /// </summary>
        /// <param name="dwScope">Scope of the enumeration.</param>
        /// <param name="dwType">Resource types to be enumerated.</param>
        /// <param name="dwUsage">Resource usage type to be enumerated.</param>
        /// <param name="lpNetResource">Pointer to a <see cref="NetResource"/>
        /// structure that specifies the container to enumerate.</param>
        /// <param name="lphEnum">Pointer to an enumeration handle that can be
        /// used in a subsequent call to <see cref="WNetEnumResource"/>.</param>
        /// <returns>A single value indicating if a system error occurred or
        /// not.</returns>
        [DllImport(@"mpr.dll")]
        public static extern uint WNetOpenEnum(
            ResourceScope dwScope,
            ResourceType dwType,
            ResourceUsage dwUsage,
            NetResource lpNetResource,
            ref IntPtr lphEnum);

#region METHODS

        /// <summary>
        /// A macro used to create a unique system I/O control code (IOCTL).
        /// </summary>
        /// <param name="deviceType">Defines the type of device for the given
        /// IOCTL. This parameter can be no bigger then a WORD value. The values
        /// used by Microsoft are in the range 0-32767 and the values
        /// 32768-65535 are reserved for use by OEMs and IHVs.</param>
        /// <param name="function">Defines an action within the device category.
        /// That function codes 0-2047 are reserved for Microsoft, and 2048-4095
        /// are reserved for OEMs and IHVs. A function code can be no larger
        /// then 4095.</param>
        /// <param name="method">Defines the method codes for how buffers are
        /// passed for I/O and file system controls.</param>
        /// <param name="access">Defines the access check value for any access.
        /// </param>
        /// <returns>A unique system I/O control code</returns>
        /// <exception cref="ArgumentOutOfRangeException"><c>function</c> is out
        /// of range.</exception>
        public static uint CTL_CODE(ushort deviceType, ushort function, uint method, uint access)
        {
            if (function > 4095)
            {
                throw new ArgumentOutOfRangeException(nameof(function));
            }

            return (((uint)deviceType << 16) | (access << 14) | ((uint)function << 2) | method);
        }

        /// <summary>
        /// Retrieves a handle to the top-level window whose class name and
        /// window name match the specified strings.
        /// </summary>
        /// <param name="className">Name of the class.</param>
        /// <param name="windowName">The window name (the window's title). If
        /// this parameter is NULL, all window names match.</param>
        /// <returns>
        /// If the function succeeds, the return value is a handle to the window
        /// that has the specified class name and window name.</returns>
        public static IntPtr FindWindow(string className, string windowName)
        {
            IntPtr pointer = FindWindowW(className, windowName);
            return pointer;
        }

        /// <summary>
        /// Sets the specified <paramref name="processName"/> main window as
        /// foreground window.
        /// </summary>
        /// <param name="processName">Name of the process.</param>
        [SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands",
            Justification = "This is a common shared library.")]
        public static void SetProcessForeground(string processName)
        {
            Process[] proc = Process.GetProcessesByName(processName);
            if (proc.Length > 0)
            {
#if NETFRAMEWORK
                SetForegroundWindow(proc[0].MainWindowHandle);
#else
                SetForegroundWindow(proc[0].SafeHandle);
#endif
            }
        }

#endregion

#region Classes

        /// <summary>
        /// Represents a class for shell32.dll native functions.
        /// </summary>
        public static class Shell32
        {
            /// <summary>
            /// Displays a dialog box that enables the user to select a Shell
            /// folder.
            /// </summary>
            /// <param name="lpbi">A pointer to a <see cref="BROWSEINFO"/>
            /// structure that contains information used to display the dialog
            /// box.</param>
            /// <returns>Returns a PIDL that specifies the location of the
            /// selected folder relative to the root of the namespace. If the
            /// user chooses the Cancel button in the dialog box, the return
            /// value is NULL.</returns>
            [DllImport(@"shell32.dll", CharSet = CurrentCharSet)]
            public static extern IntPtr SHBrowseForFolder(ref BROWSEINFO lpbi);

            /// <summary>
            /// Converts an item identifier list to a file system path.
            /// </summary>
            /// <param name="pidl">The address of an item identifier list that
            /// specifies a file or directory location relative to the root of
            /// the namespace (the desktop).</param>
            /// <param name="pszPath">The address of a buffer to receive the
            /// file system path. This buffer must be at least MAX_PATH
            /// characters in size.</param>
            /// <returns>Returns TRUE if successful; otherwise, FALSE.</returns>
            [DllImport(@"shell32.dll", CharSet = CurrentCharSet)]
            public static extern bool SHGetPathFromIDList(IntPtr pidl, IntPtr pszPath);

            /// <summary>
            /// Converts an item identifier list to a file system path.
            /// </summary>
            /// <param name="pidl">The address of an item identifier list that
            /// specifies a file or directory location relative to the root of
            /// the namespace (the desktop).</param>
            /// <param name="pszPath">The address of a buffer to receive the
            /// file system path. This buffer must be at least MAX_PATH
            /// characters in size.</param>
            /// <returns>Returns TRUE if successful; otherwise, FALSE.</returns>
            [DllImport(@"Shell32.dll", CharSet = CurrentCharSet)]
            public static extern bool SHGetPathFromIDList(IntPtr pidl, [MarshalAs(UnmanagedType.LPTStr)] StringBuilder pszPath);

            /// <summary>
            /// Retrieves a pointer to the ITEMIDLIST structure of a special
            /// folder.
            /// </summary>
            /// <param name="hwnd">The HWND.</param>
            /// <param name="csidl">A CSIDL value that identifies the folder of
            /// interest.</param>
            /// <param name="ppidl">A PIDL specifying the folder's location
            /// relative to the root of the namespace (the desktop). It is the
            /// responsibility of the calling application to free the returned
            /// IDList by using <see cref="CoTaskMemFree"/>.</param>
            /// <returns>S_OK if the function succeeds, otherwise, the HRESULT
            /// error code.</returns>
            [DllImport(@"shell32.dll")]
            public static extern int SHGetSpecialFolderLocation(IntPtr hwnd, int csidl, ref IntPtr ppidl);
        }

#endregion
    }
}