namespace Oliviann.Windows.Forms
{
    #region Usings

    using System.Windows.Forms;

    #endregion Usings

    /// <summary>
    /// Represents a dummy form with a small size to use for help with
    /// displaying information to the user.
    /// </summary>
    /// <remarks>
    /// This form will not be displayed in the Windows Taskbar. The form will be
    /// located in the 0, 0 location of the screen with a pixel size of 1, 1.
    /// </remarks>
    internal partial class DummyForm : Form
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DummyForm"/> class.
        /// </summary>
        internal DummyForm()
        {
            this.InitializeComponent();
        }
    }
}