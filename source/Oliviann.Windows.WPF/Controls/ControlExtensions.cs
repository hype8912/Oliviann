namespace Oliviann.Windows.Controls
{
    #region Usings

    using System;
    using System.Windows.Controls;

    #endregion

    /// <summary>
    /// Represents a collection of commonly used extension methods for extending
    /// form controls.
    /// </summary>
    public static class ControlExtensions
    {
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
            if (ctrl?.Dispatcher == null || delegateAction == null)
            {
                return;
            }

            if (ctrl.Dispatcher.CheckAccess())
            {
                delegateAction();
            }
            else
            {
                ctrl.Dispatcher.Invoke(delegateAction);
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
            if (ctrl?.Dispatcher == null || delegateAction == null)
            {
                return;
            }

            if (ctrl.Dispatcher.CheckAccess())
            {
                delegateAction(ctrl);
            }
            else
            {
                ctrl.Dispatcher.Invoke(delegateAction, ctrl);
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
            if (ctrl?.Dispatcher == null || delegateFunc == null)
            {
                return default(TOut);
            }

            if (!ctrl.Dispatcher.CheckAccess())
            {
                // Probably not a good idea to cast but we'll give it a shot.
                object delegateResult = ctrl.Dispatcher.Invoke(delegateFunc, ctrl);
                delegateResult.TryCast(out TOut result);
                return result;
            }

            return delegateFunc(ctrl);
        }
    }
}