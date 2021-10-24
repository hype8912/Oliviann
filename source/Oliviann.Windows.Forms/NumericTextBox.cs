namespace Oliviann.Windows.Forms
{
    #region Usings

    using System;
    using System.Linq;
    using System.Windows.Forms;
    using Oliviann.Native;

    #endregion

    /// <summary>
    /// Represents a Windows text box control that only allows numeric values.
    /// </summary>
    public partial class NumericTextBox : TextBox
    {
        #region Fields

        /// <summary>
        /// The maximum value allowed.
        /// </summary>
        private decimal maxValue = decimal.MaxValue;

        /// <summary>
        /// The minimum value allowed.
        /// </summary>
        private decimal minValue = decimal.MinValue;

        /// <summary>
        /// The previous text from the last change.
        /// </summary>
        private string previousText = string.Empty;

        #endregion

        #region Constructor/Destructor

        /// <summary>
        /// Initializes a new instance of the <see cref="NumericTextBox"/>
        /// class.
        /// </summary>
        public NumericTextBox()
        {
            InitializeComponent();
            base.Multiline = false;
            this.SetMaximumLength();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether to allow decimal values.
        /// Default value is true.
        /// </summary>
        /// <value>
        /// True if to allow decimal values; otherwise, false.
        /// </value>
        public bool AcceptsDecimalValues { get; set; } = true;

        /// <summary>
        /// Gets a value indicating whether pressing the TAB key in a
        /// multiline text box control types a TAB character in the control
        /// instead of moving the focus to the next control in the tab order.
        /// </summary>
        public new bool AcceptsTab
        {
            get => false;
            set { }
        }

        /// <summary>
        /// Gets or sets a value indicating whether pressing ENTER in a
        /// multiline <see cref="T:System.Windows.Forms.TextBox" /> control
        /// creates a new line of text in the control or activates the default
        /// button for the form.
        /// </summary>
        public new bool AcceptsReturn
        {
            get => false;
            set { }
        }

        /// <summary>
        /// Gets or sets the maximum number of characters the user can type or
        /// paste into the text box control.
        /// </summary>
        public override int MaxLength
        {
            get => base.MaxLength;
            set => this.SetMaximumLength(value);
        }

        /// <summary>
        /// Gets or sets the maximum value of the text box.
        /// </summary>
        /// <value>The maximum value of the text box.</value>
        public decimal MaxValue
        {
            get => this.maxValue;
            set
            {
                if (value < this.MinValue)
                {
                    return;
                }

                this.maxValue = value;
                if (this.Value > this.maxValue)
                {
                    this.Value = this.maxValue;
                }
            }
        }

        /// <summary>
        /// Gets or sets the minimum value of the text box.
        /// </summary>
        /// <value>The minimum value of the text box.</value>
        public decimal MinValue
        {
            get => this.minValue;
            set
            {
                if (value > this.MaxValue)
                {
                    return;
                }

                this.minValue = value;
                if (this.Value < this.minValue)
                {
                    this.Value = this.minValue;
                }

                this.SetMaximumLength();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this is a multiline
        /// <see cref="T:System.Windows.Forms.TextBox" /> control.
        /// </summary>
        public override bool Multiline
        {
            get => false;
            set { }
        }

        /// <summary>
        /// Gets the current text in the
        /// <see cref="T:System.Windows.Forms.TextBox" />.
        /// </summary>
        public override string Text
        {
            get => base.Text;
            set
            {
                if (value.IsNullOrWhiteSpace())
                {
                    base.Text = string.Empty;
                    return;
                }

                decimal tempValue = value.ToDecimal();
                this.Value = tempValue;
            }
        }

        /// <summary>
        /// Gets or sets the decimal value of the text box.
        /// </summary>
        /// <value>
        /// The decimal value of the text box.
        /// </value>
        public decimal Value
        {
            get => this.Text.ToDecimal();

            set
            {
                if (value < this.MinValue || value > this.MaxValue)
                {
                    return;
                }

                string txt = value.ToString();
                if (this.AcceptsDecimalValues && this.MinValue < 0)
                {
                    base.Text = txt;
                    return;
                }

                if (this.AcceptsDecimalValues && this.MinValue >= 0)
                {
                    base.Text = value < 0 ? "0" : txt;
                    return;
                }

                if (!this.AcceptsDecimalValues && this.MinValue < 0)
                {
                    base.Text = decimal.Truncate(value).ToString();
                    return;
                }

                if (!this.AcceptsDecimalValues && this.MinValue >= 0)
                {
                    base.Text = value < 0 ? "0" : decimal.Truncate(value).ToString();
                }
            }
        }

        /// <summary>
        /// Indicates whether a multiline text box control automatically wraps
        /// words to the beginning of the next line when necessary.
        /// </summary>
        public new bool WordWrap
        {
            get => false;
            set { }
        }

        #endregion

        #region Overrides

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.KeyPress" />
        /// event.
        /// </summary>
        /// <param name="e">A
        /// <see cref="T:System.Windows.Forms.KeyPressEventArgs" /> that
        /// contains the event data.</param>
        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            e.Handled = false;

            // Allow basic control functions of Ctrl + A, Ctrl + V, Ctrl + C.
            if (char.IsControl(e.KeyChar) && (e.KeyChar == '\u0001' || e.KeyChar == '\u0003' || e.KeyChar == '\u0016'))
            {
                base.OnKeyPress(e);
                return;
            }

            // Validates char is a digit or a backspace.
            if (char.IsDigit(e.KeyChar) || e.KeyChar == '\b')
            {
                base.OnKeyPress(e);
                return;
            }

            // Validates allowing a single decimal but not as the first
            // character in the text box and not as the first character after a
            // negative sign.
            if (this.AcceptsDecimalValues && e.KeyChar == '.' &&
                this.Text.Length != 0 &&
                !this.Text.Contains(e.KeyChar) &&
                char.IsDigit(this.Text.Last()))
            {
                base.OnKeyPress(e);
                return;
            }

            // Validates allowing a single negative sign but only as the first
            // character in the text box.
            if (this.MinValue < 0 && e.KeyChar == '-' && this.Text.Length == 0)
            {
                base.OnKeyPress(e);
                return;
            }

            e.Handled = true;
        }

        /// <summary>
        /// Occurs when the Text property value changes.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
        protected override void OnTextChanged(EventArgs e)
        {
            decimal currentValue = this.Value;
            if (currentValue < this.MinValue || currentValue > this.MaxValue)
            {
                base.Text = this.previousText;
            }
            else
            {
                this.previousText = base.Text;
                base.OnTextChanged(e);
            }
        }

        /// <summary>
        /// Processes the Windows messages.
        /// </summary>
        /// <param name="m">A Windows Message object.</param>
        protected override void WndProc(ref Message m)
        {
            if (m.Msg != (int)WM.PASTE)
            {
                // Handle all other messages normally.
                base.WndProc(ref m);
                return;
            }

            string text = Clipboard.GetText();
            if (decimal.TryParse(text, out decimal value))
            {
                if (this.Text.Length == 0)
                {
                    this.Value = value;
                    return;
                }

                // TODO: Handle negative values pasted in. Not allowing pasting of negative numbers at this point unless the text box is empty.
                if (text.Contains('-'))
                {
                    return;
                }

                // Ensures we don't paste in a double decimal point.
                if (!this.Text.Contains('.') || !text.Contains('.'))
                {
                    base.WndProc(ref m);
                    return;
                }
            }
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Sets the base text box maximum length based on what this text box
        /// accepts.
        /// </summary>
        /// <param name="proposedMaximumValue">The proposed maximum value if
        /// specified.</param>
        private void SetMaximumLength(int? proposedMaximumValue = null)
        {
            int maximumValue = 28;
            if (this.MinValue < 0)
            {
                maximumValue += 1;
            }

            int propMaximumValue = proposedMaximumValue.GetValueOrDefault(int.MaxValue);
            base.MaxLength = propMaximumValue > maximumValue ? maximumValue : propMaximumValue;
        }

        #endregion
    }
}