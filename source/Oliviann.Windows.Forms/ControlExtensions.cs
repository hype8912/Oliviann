namespace Oliviann.Windows.Forms
{
    #region Usings

    using System;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;
    using Oliviann.Native;

    #endregion Usings

    /// <summary>
    /// Represents a collection of commonly used extension methods for extending
    /// form controls.
    /// </summary>
    public static class ControlExtensions
    {
        /// <summary>
        /// Clears the textual cue, or tip, that is displayed by the edit
        /// control.
        /// </summary>
        /// <param name="ctrl">An edit control for the text to be removed from.
        /// </param>
        public static void ClearCueText(this Control ctrl) => SetCueText(ctrl, string.Empty);

        /// <summary>
        /// Recursively clears the text from all the child <see cref="TextBox"/>
        /// controls.
        /// </summary>
        /// <param name="parentControl">The parent control.</param>
        public static void ClearTextBoxes(this Control parentControl)
        {
            if (parentControl == null)
            {
                return;
            }

            foreach (Control ctrl in parentControl.Controls)
            {
                var box = ctrl as TextBox;
                box?.Clear();

                if (ctrl.HasChildren)
                {
                    ClearTextBoxes(ctrl);
                }
            }
        }

        /// <summary>
        /// Disables the user control to respond to user interaction.
        /// </summary>
        /// <param name="item">The form control.</param>
        public static void Disable(this Control item)
        {
            if (item != null)
            {
                item.Enabled = false;
            }
        }

        /// <summary>
        /// Enables the user control to respond to user interaction.
        /// </summary>
        /// <param name="item">The form control.</param>
        public static void Enable(this Control item)
        {
            if (item != null)
            {
                item.Enabled = true;
            }
        }

        /// <summary>
        /// Determines if the specified control was created on a separate thread
        /// or not and then invokes the specified
        /// <paramref name="delegateAction"/> on the same thread as the control.
        /// </summary>
        /// <param name="ctrl">The form control.</param>
        /// <param name="delegateAction">The delegate action to be performed on
        /// the same thread as the control.</param>
        public static void InvokeIfRequired(this Control ctrl, Action delegateAction)
        {
            if (ctrl == null || ctrl.IsDisposed || delegateAction == null)
            {
                return;
            }

            if (ctrl.InvokeRequired)
            {
                ctrl.BeginInvoke(delegateAction);
            }
            else
            {
                delegateAction();
            }
        }

        /// <summary>
        /// Determines if the specified control was created on a separate thread
        /// or not and then invokes the specified
        /// <paramref name="delegateAction" /> on the same thread as the
        /// control.
        /// </summary>
        /// <typeparam name="T">The type of control to be executed.</typeparam>
        /// <param name="ctrl">The form control.</param>
        /// <param name="delegateAction">The delegate action to be performed on
        /// the same thread as the control.</param>
        public static void InvokeIfRequired<T>(this T ctrl, Action<T> delegateAction) where T : Control
        {
            if (ctrl == null || ctrl.IsDisposed || delegateAction == null)
            {
                return;
            }

            if (ctrl.InvokeRequired)
            {
                ctrl.BeginInvoke(delegateAction, ctrl);
            }
            else
            {
                delegateAction(ctrl);
            }
        }

        /// <summary>
        /// Determines if the specified control was created on a separate thread
        /// or not and then invokes the specified
        /// <paramref name="delegateFunc"/> on the same thread as the control.
        /// </summary>
        /// <typeparam name="TIn">The type of the control.</typeparam>
        /// <typeparam name="TOut">The type of the output from the function.
        /// </typeparam>
        /// <param name="ctrl">The control instance.</param>
        /// <param name="delegateFunc">The delegate function to be performed on
        /// the same thread as the control.</param>
        /// <returns>An instance of <typeparamref name="TOut"/> or the default
        /// value.</returns>
        public static TOut InvokeIfRequired<TIn, TOut>(this TIn ctrl, Func<TIn, TOut> delegateFunc) where TIn : Control
        {
            if (ctrl == null || ctrl.IsDisposed || delegateFunc == null)
            {
                return default(TOut);
            }

            if (ctrl.InvokeRequired)
            {
                IAsyncResult result = ctrl.BeginInvoke(delegateFunc, ctrl);
                TOut delegateResult = (TOut)ctrl.EndInvoke(result);
                return delegateResult;
            }

            return delegateFunc(ctrl);
        }

        /// <summary>
        /// Sets the textual cue, or tip, that is displayed by the edit control
        /// to prompt the user for information.
        /// </summary>
        /// <param name="ctrl">An edit control for the text to be displayed on.
        /// </param>
        /// <param name="text">The text to be displayed as the textual cue.
        /// </param>
        /// <remarks>
        /// You cannot set a cue banner on a multi-line edit control or on a
        /// rich edit control. If the cue banner does not display, ensure you
        /// have called Application.EnableVisualStyles() before calling
        /// Application.Run().
        /// </remarks>
        public static void SetCueText(this Control ctrl, string text)
        {
            if (ctrl == null)
            {
                return;
            }

            if (ctrl is ComboBox)
            {
                var info = new COMBOBOXINFO();
                info.cbSize = Marshal.SizeOf(info);
                UnsafeNativeMethods.GetComboBoxInfo(ctrl.Handle, ref info);
                UnsafeNativeMethods.SendMessage(info.hwndEdit, Constants.EM_SETCUEBANNER, 0, text);
            }

            UnsafeNativeMethods.SendMessage(ctrl.Handle, Constants.EM_SETCUEBANNER, 0, text);
        }

        /// <summary>
        /// Associates ToolTip text with the specified control.
        /// </summary>
        /// <param name="ctrl">The System.Windows.Forms.Control to associate the
        /// ToolTip text with.</param>
        /// <param name="caption">The ToolTip text to display when the pointer
        /// is on the control.</param>
        public static void SetToolTip(this Control ctrl, string caption)
        {
            using (var tip = new ToolTip())
            {
                tip.SetToolTip(ctrl, caption);
            }
        }
    }
}