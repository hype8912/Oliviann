namespace Oliviann.Windows.Forms
{
    #region Usings

    using System;
    using System.ComponentModel;
    using System.Threading;
    using System.Windows.Forms;
    using Oliviann.Properties;

    #endregion Usings

    /// <summary>
    /// Represents a message box form with advanced options.
    /// </summary>
    internal partial class FrmAdvancedMessageBox : Form
    {
        #region Fields

        /// <summary>
        /// Variable for determining if the form was closed because it timed
        /// out.
        /// </summary>
        private bool timerExpired;

        /// <summary>
        /// Place holder for the user selected button result.
        /// </summary>
        private DialogResult selectedResult = DialogResult.None;

        #endregion Fields

        #region Constructor/Destructor

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="FrmAdvancedMessageBox" /> class.
        /// </summary>
        public FrmAdvancedMessageBox()
        {
            this.Buttons = MessageBoxButtons.OK;
            this.Icon = MessageBoxIcon.None;
            this.DefaultButton = MessageBoxDefaultButton.Button1;
            this.Timeout = TimeSpan.FromSeconds(30D);

            this.InitializeComponent();

            this.Worker1.RunWorkerCompleted += this.Worker1_RunWorkerCompleted;
        }

        #endregion Constructor/Destructor

        #region Properties

        /// <summary>
        /// Gets or sets the message box text.
        /// </summary>
        /// <returns>The message box text.</returns>
        public new string Text { get; set; }

        /// <summary>
        /// Gets or sets the message box caption.
        /// </summary>
        /// <value>
        /// The message box caption.
        /// </value>
        public string Caption
        {
            get => base.Text;
            set => base.Text = value;
        }

        /// <summary>
        /// Gets or sets the button combinations to be displayed to the user.
        /// </summary>
        /// <value>
        /// The buttons to be displayed to the user.
        /// </value>
        public MessageBoxButtons Buttons { get; set; }

        /// <summary>
        /// Gets or sets the icon to be displayed to the user.
        /// </summary>
        /// <returns>The icon to be displayed to the user.</returns>
        public new MessageBoxIcon Icon { get; set; }

        /// <summary>
        /// Gets or sets the default button.
        /// </summary>
        /// <value>
        /// The default button.
        /// </value>
        public MessageBoxDefaultButton DefaultButton { get; set; }

        /// <summary>
        /// Gets or sets the timeout before the message box automatically
        /// closes.
        /// </summary>
        /// <value>
        /// The timeout before the message box automatically closes.
        /// </value>
        public TimeSpan Timeout { get; set; }

        #endregion Properties

        #region Form Events

        /// <summary>
        /// Handles the Load event of the FrmAdvancedMessageBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the
        /// event data.</param>
        private void FrmAdvancedMessageBox_Load(object sender, EventArgs e)
        {
            this.LblText.Text = this.Text;
            this.InitializeMessageBoxButtons();
            this.InitializeMessageBoxIcon();
            this.InitializeMessageBoxDefaultButton();

            // Won't create the timer if you specify 0 as a timeout value.
            if (this.Timeout.TotalSeconds > 1)
            {
                this.Worker1.RunWorkerAsync();
            }
            else
            {
                this.StatusStripLabel.Visible = false;
            }
        }

        #endregion Form Events

        #region Button Events

        /// <summary>
        /// Handles the Click event of the Buttons control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the
        /// event data.</param>
        private void Buttons_Click(object sender, EventArgs e)
        {
            var btn = (Button)sender;
            this.selectedResult = btn.Text.ToEnum(DialogResult.No);

            if (this.Worker1.IsBusy)
            {
                this.Worker1.CancelAsync();

                // Hide event is to simulate to the user that the form is done
                // while we wait for the worker to run the completed event
                // otherwise the form may look like it's froze for 1 second.
                this.Hide();
            }

            if (!this.StatusStripLabel.Visible)
            {
                // Only occurs when the timer was less than 1 and the user selects
                // a button.
                this.DialogResult = this.selectedResult;
            }
        }

        /// <summary>
        /// Initializes the message box buttons to be displayed on the form.
        /// </summary>
        private void InitializeMessageBoxButtons()
        {
            switch (this.Buttons)
            {
                case MessageBoxButtons.RetryCancel:
                    this.Btn2.Text = @"Retry";
                    this.Btn2.Show();
                    this.Btn1.Text = @"Cancel";
                    break;

                case MessageBoxButtons.YesNoCancel:
                    this.Btn3.Text = @"Yes";
                    this.Btn3.Show();
                    this.Btn2.Text = @"No";
                    this.Btn2.Show();
                    this.Btn1.Text = @"Cancel";
                    break;

                case MessageBoxButtons.YesNo:
                    this.Btn2.Text = @"Yes";
                    this.Btn2.Show();
                    this.Btn1.Text = @"No";
                    break;

                case MessageBoxButtons.AbortRetryIgnore:
                    this.Btn3.Text = @"Abort";
                    this.Btn3.Show();
                    this.Btn2.Text = @"Retry";
                    this.Btn2.Show();
                    this.Btn1.Text = @"Ignore";
                    break;

                case MessageBoxButtons.OKCancel:
                    this.Btn2.Text = @"OK";
                    this.Btn2.Show();
                    this.Btn1.Text = @"Cancel";
                    break;

                default:
                    return;
            }
        }

        #endregion Button Events

        /// <summary>
        /// Initializes the message box icon on the form.
        /// </summary>
        private void InitializeMessageBoxIcon()
        {
            switch (this.Icon)
            {
                case MessageBoxIcon.Error:
                    this.PicBoxIcon.Image = Images.Error;
                    this.PicBoxIcon.Show();
                    break;

                case MessageBoxIcon.Question:
                    this.PicBoxIcon.Image = Images.Question;
                    this.PicBoxIcon.Show();
                    break;

                case MessageBoxIcon.Warning:
                    this.PicBoxIcon.Image = Images.Warning;
                    this.PicBoxIcon.Show();
                    break;

                case MessageBoxIcon.Information:
                    this.PicBoxIcon.Image = Images.Info;
                    this.PicBoxIcon.Show();
                    break;
            }
        }

        /// <summary>
        /// Initializes the message box default button.
        /// </summary>
        private void InitializeMessageBoxDefaultButton()
        {
            Button btn = null;
            switch (this.DefaultButton)
            {
                case MessageBoxDefaultButton.Button1:
                    btn = this.Btn1;
                    break;

                case MessageBoxDefaultButton.Button2:
                    btn = this.Btn2;
                    break;

                case MessageBoxDefaultButton.Button3:
                    btn = this.Btn3;
                    break;
            }

            if (btn != null && btn.Visible)
            {
                btn.Select();
            }
        }

        #region Background Worker Events

        /// <summary>
        /// Handles the DoWork event of the Worker1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DoWorkEventArgs" /> instance
        /// containing the event data.</param>
        private void Worker1_DoWork(object sender, DoWorkEventArgs e)
        {
            int timeRemaining = this.Timeout.TotalSeconds.ToInt32();
            while (timeRemaining > 0)
            {
                if (this.Worker1.CancellationPending)
                {
                    e.Cancel = true;
                    return;
                }

                int temp = timeRemaining;
                this.InvokeIfRequired(() => this.StatuStripText.Text = temp.ToString());
                Thread.Sleep(1000);
                timeRemaining -= 1;
            }

            this.timerExpired = true;
        }

        /// <summary>
        /// Handles the RunWorkerCompleted event of the Worker1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RunWorkerCompletedEventArgs" />
        /// instance containing the event data.</param>
        private void Worker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.DialogResult = this.timerExpired ? DialogResult.No : this.selectedResult;
        }

        #endregion Background Worker Events
    }
}