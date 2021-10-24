namespace Oliviann.Windows.Forms
{
    #region Usings

    using System;
    using System.Runtime.InteropServices;
    using System.Security;
    using System.Windows.Forms;

    #endregion Usings

    /// <summary>
    /// Represents a Windows text box control that uses a
    /// <see cref="SecureString"/> object as the backing store.
    /// </summary>
    public partial class SecureTextBox : TextBox
    {
        #region Fields

        /// <summary>
        /// Variable to display the character inputted.
        /// </summary>
        private bool displayChar;

        #endregion Fields

        #region Constructor/Destructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SecureTextBox"/> class.
        /// </summary>
        public SecureTextBox()
        {
            this.InitializeComponent();
            this.PasswordChar = '*';
        }

        #endregion Constructor/Destructor

        #region Properties

        /// <summary>
        /// Gets or sets the current text in the <see cref="SecureTextBox"/>.
        /// </summary>
        /// <value>
        /// The text displayed in the control.
        /// </value>
        public SecureString SecureText { get; set; } = new SecureString();

        /// <summary>
        /// Gets this string instance as a character array.
        /// </summary>
        /// <remarks>
        /// _Note that this is still visible plainly in memory and should
        /// be 'consumed' as quickly as possible, then the contents 'zero-ed' so
        /// that they cannot be viewed.
        /// </remarks>
        public char[] CharacterData
        {
            get
            {
                char[] bytes;
                IntPtr handle = IntPtr.Zero;

                try
                {
                    handle = Marshal.SecureStringToBSTR(this.SecureText);
                    bytes = new char[this.SecureText.Length];
                    Marshal.Copy(handle, bytes, 0, this.SecureText.Length);
                }
                finally
                {
                    if (handle != IntPtr.Zero)
                    {
                        Marshal.ZeroFreeBSTR(handle);
                    }
                }

                return bytes;
            }
        }

        #endregion Properties

        #region Overrides

        /// <summary>
        /// Determines if a character is an input character that the control
        /// recognizes.
        /// </summary>
        /// <param name="charCode">The character to test.</param>
        /// <returns>
        /// True if the character should be sent directly to the control and not
        /// preprocessed; otherwise, false.
        /// </returns>
        protected override bool IsInputChar(char charCode)
        {
            bool isChar = base.IsInputChar(charCode);
            if (!isChar)
            {
                this.displayChar = true;
                return false;
            }

            if (!char.IsControl(charCode) && !char.IsHighSurrogate(charCode) && !char.IsLowSurrogate(charCode))
            {
                this.HandleInputNonControlChar(charCode);
            }
            else
            {
                this.HandleInputControlChar(charCode);
            }

            return true;
        }

        /// <summary>
        /// Determines whether the specified key is an input key or a special
        /// key that requires preprocessing.
        /// </summary>
        /// <param name="keyData">One of the key's values.</param>
        /// <returns>
        /// <c>true</c> if the specified key is an input key; otherwise,
        /// <c>false</c>.
        /// </returns>
        protected override bool IsInputKey(Keys keyData)
        {
            // Note: This whole section is only to deal with the 'Delete' key.
            bool allowedToDelete = (keyData & Keys.Delete) == Keys.Delete;

            // Debugging only
            ////this.Parent.Text = keyData + ' ' + ((int)keyData) + @" allowedToDelete = " + allowedToDelete;

            if (allowedToDelete)
            {
                if (this.SelectionLength == this.SecureText.Length)
                {
                    this.SecureText.Clear();
                }
                else if (this.SelectionLength > 0)
                {
                    for (int i = 0; i < this.SelectionLength; i += 1)
                    {
                        this.SecureText.RemoveAt(this.SelectionStart);
                    }
                }
                else
                {
                    if ((keyData & Keys.Delete) == Keys.Delete && this.SelectionStart < this.Text.Length)
                    {
                        this.SecureText.RemoveAt(this.SelectionStart);
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Processes a keyboard message.
        /// </summary>
        /// <param name="m">A <see cref="T:System.Windows.Forms.Message"/>,
        /// passed by reference, that represents the window message to process.
        /// </param>
        /// <returns>
        /// <c>true</c> if the message was processed by the control; otherwise,
        /// <c>false</c>.
        /// </returns>
        protected override bool ProcessKeyMessage(ref Message m)
        {
            if (this.displayChar)
            {
                return base.ProcessKeyMessage(ref m);
            }

            this.displayChar = true;
            return true;
        }

        #endregion Overrides

        #region Helpers

        /// <summary>
        /// Handles the input non control character.
        /// </summary>
        /// <param name="charCode">The character to test.</param>
        private void HandleInputNonControlChar(char charCode)
        {
            int startPosition = this.SelectionStart;
            if (this.SelectionLength > 0)
            {
                for (int i = 0; i < this.SelectionLength; i += 1)
                {
                    this.SecureText.RemoveAt(this.SelectionStart);
                }
            }

            if (startPosition == this.SecureText.Length)
            {
                this.SecureText.AppendChar(charCode);
            }
            else
            {
                this.SecureText.InsertAt(startPosition, charCode);
            }

            this.Text = new string('*', this.SecureText.Length);
            this.displayChar = false;
            startPosition += 1;
            this.SelectionStart = startPosition;
        }

        /// <summary>
        /// Handles the select input control character.
        /// </summary>
        /// <param name="charCode">The character to test.</param>
        private void HandleInputControlChar(char charCode)
        {
            int startPosition = this.SelectionStart;

            // We need to check what key has been pressed.
            if (charCode == (int)Keys.Back)
            {
                if (this.SelectionLength == 0 && startPosition > 0)
                {
                    startPosition -= 1;
                    this.SecureText.RemoveAt(startPosition);
                    this.Text = new string('*', this.SecureText.Length);
                    this.SelectionStart = startPosition;
                }
                else if (this.SelectionLength > 0)
                {
                    for (int i = 0; i < this.SelectionLength; i += 1)
                    {
                        this.SecureText.RemoveAt(this.SelectionStart);
                    }
                }

                // If we don't do this, we get a 'double' BACK keystroke effect
                this.displayChar = false;
            }
        }

        #endregion Helpers
    }
}