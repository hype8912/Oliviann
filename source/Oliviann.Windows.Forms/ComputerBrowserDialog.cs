namespace Oliviann.Windows.Forms
{
    #region Usings

    using System;
    using System.Runtime.InteropServices;
    using System.Threading;
    using System.Windows.Forms;
    using Oliviann.Native;
    using Oliviann.Properties;

    #endregion Usings

    /// <summary>
    /// Prompts the user to select a network computer. This class cannot be
    /// inherited.
    /// </summary>
    public sealed class ComputerBrowserDialog : CommonDialog
    {
        #region Fields

        /// <summary>
        /// Place holder for the description text.
        /// </summary>
        private string _descriptionText;

        /// <summary>
        /// Place holder for the selected computer.
        /// </summary>
        private string _selectedComputer;

        #endregion Fields

        #region Constructor/Destructor

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="ComputerBrowserDialog"/> class.
        /// </summary>
        public ComputerBrowserDialog() => this.Reset();

        #endregion Constructor/Destructor

        #region Properties

        /// <summary>
        /// Gets or sets the computer selected by the user.
        /// </summary>
        /// <value>
        /// The computer first selected in the dialog box or the last computer
        /// selected by the user. The default is an empty string ("").
        /// </value>
        public string SelectedComputer
        {
            get => this._selectedComputer;
            set => this._selectedComputer = value ?? string.Empty;
        }

        /// <summary>
        /// Gets or sets the descriptive text displayed above the tree view
        /// control in the dialog box.
        /// </summary>
        /// <value>
        /// The description to display. The default is an empty string ("").
        /// </value>
        public string Description
        {
            get => this._descriptionText;
            set => this._descriptionText = value ?? string.Empty;
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// When overridden in a derived class, resets the properties of a
        /// common dialog box to their default values.
        /// </summary>
        /// <filterpriority>1</filterpriority>
        public override void Reset()
        {
            this._descriptionText = string.Empty;
            this._selectedComputer = string.Empty;
        }

        /// <summary>
        /// When overridden in a derived class, specifies a common dialog box.
        /// </summary>
        /// <returns>
        /// <c>true</c> if the dialog box was successfully run; otherwise,
        /// <c>false</c>.
        /// </returns>
        /// <param name="hwndOwner">A value that represents the window handle of
        /// the owner window for the common dialog box. </param>
        protected override bool RunDialog(IntPtr hwndOwner)
        {
            IntPtr zero = IntPtr.Zero;
            bool flag = false;
            UnsafeNativeMethods.Shell32.SHGetSpecialFolderLocation(hwndOwner, (int)EnvironmentFolders.SpecialFolder.NetworkNeighborhood, ref zero);
            if (zero == IntPtr.Zero)
            {
                throw new InvalidOperationException(Resources.ComputerBrowserDialogInvalidOpEx);
            }

            int num = (int)BrowseInfoFlags.BrowseForComputer + (int)BrowseInfoFlags.NewDialogStyle + (int)BrowseInfoFlags.NoNewFolderButton;
            if (Control.CheckForIllegalCrossThreadCalls && (Application.OleRequired() != ApartmentState.STA))
            {
                throw new ThreadStateException(Resources.ComputerBrowserDialogThreadStateEx);
            }

            IntPtr pidl = IntPtr.Zero;
            try
            {
                var lpbi = new BROWSEINFO
                               {
                                   pidlRoot = zero,
                                   hwndOwner = hwndOwner,
                                   lpszDisplayName = new string('\0', 260 * Marshal.SystemDefaultCharSize),
                                   lpszTitle = this._descriptionText,
                                   ulFlags = num,
                                   lpfn = null,
                                   lParam = IntPtr.Zero
                               };
                pidl = UnsafeNativeMethods.Shell32.SHBrowseForFolder(ref lpbi);
                if (pidl != IntPtr.Zero)
                {
                    this._selectedComputer = lpbi.lpszDisplayName;
                    flag = true;
                }
            }
            finally
            {
                if (pidl != IntPtr.Zero)
                {
                    UnsafeNativeMethods.CoTaskMemFree(pidl);
                }
            }

            return flag;
        }

        #endregion Methods
    }
}