﻿namespace Oliviann
{
    #region Usings

    using System;
    using System.ComponentModel;
    using System.Threading.Tasks;

    #endregion Usings

    /// <summary>
    /// Represents a collection of commonly used extension methods for extending
    /// delegates.
    /// </summary>
    public static class DelegateExtensions
    {
        #region Raises

        /// <summary>
        /// Raises the event for the specified event handler safely by checking
        /// it for null first.
        /// </summary>
        /// <typeparam name="TEventArgs">The type of the event arguments.
        /// </typeparam>
        /// <param name="handler">The event handler.</param>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An <see cref="System.EventArgs" /> that contains the
        /// event data.</param>
        public static void RaiseEvent<TEventArgs>(this EventHandler handler, object sender, TEventArgs e)
            where TEventArgs : EventArgs
        {
            handler?.Invoke(sender, e);
        }

        /// <summary>
        /// Raises the event for the specified event handler safely by checking
        /// it for <c>null</c> first.
        /// </summary>
        /// <param name="handler">The event handler.</param>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The
        /// <see cref="System.ComponentModel.PropertyChangedEventArgs"/>
        /// instance containing the event data.</param>
        public static void RaiseEvent(this PropertyChangedEventHandler handler, object sender, PropertyChangedEventArgs e) =>
            handler?.Invoke(sender, e);

        /// <summary>
        /// Raises the event for the specified event handler safely by checking
        /// it for <c>null</c> first.
        /// </summary>
        /// <typeparam name="TEventArgs">The type of the event data generated by
        /// the event.</typeparam>
        /// <param name="handler">The event handler.</param>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An <see cref="System.EventArgs"/> that contains the
        /// event data.</param>
        public static void RaiseEvent<TEventArgs>(this EventHandler<TEventArgs> handler, object sender, TEventArgs e)
            where TEventArgs : EventArgs
        {
            handler?.Invoke(sender, e);
        }

        /// <summary>
        /// Raises the event for the specified event handler safely by checking
        /// it for <c>null</c> first.
        /// </summary>
        /// <typeparam name="TArgs">The type of object returned by the action.
        /// </typeparam>
        /// <param name="handler">The action handler.</param>
        /// <param name="sender">The source of the action.</param>
        /// <param name="e">The data returned by the action when invoked.
        /// </param>
        public static void RaiseEvent<TArgs>(this Action<object, TArgs> handler, object sender, TArgs e) where TArgs : class =>
            handler.InvokeSafe(sender, e);

        #endregion Raises

        #region Invokes

        /// <summary>
        /// Encapsulates a method that takes no parameters safely by checking
        /// for <c>null</c> first and does not return a value.
        /// </summary>
        /// <param name="delegateAction">A delegate that takes no parameters and
        /// does not return a value.</param>
        public static void InvokeSafe(this Action delegateAction) => delegateAction?.Invoke();

        /// <summary>
        /// Encapsulates a method that has a single parameter safely by checking
        /// for <c>null</c> first and does not return a value.
        /// </summary>
        /// <typeparam name="T">The type of the parameter of the method that
        /// this delegate encapsulates.</typeparam>
        /// <param name="delegateAction">A delegate that has a single parameter
        /// and does not return a value.</param>
        /// <param name="obj">The parameter of the method that this delegate
        /// encapsulates.</param>
        public static void InvokeSafe<T>(this Action<T> delegateAction, T obj) => delegateAction?.Invoke(obj);

        /// <summary>
        /// Invokes the safe.
        /// </summary>
        /// <typeparam name="T1">The type of the 1.</typeparam>
        /// <typeparam name="T2">The type of the 2.</typeparam>
        /// <param name="delegateAction">The delegate action.</param>
        /// <param name="obj1">The obj1.</param>
        /// <param name="obj2">The obj2.</param>
        public static void InvokeSafe<T1, T2>(this Action<T1, T2> delegateAction, T1 obj1, T2 obj2) =>
            delegateAction?.Invoke(obj1, obj2);

        /// <summary>
        /// Encapsulates a function that has a single parameter safely by
        /// checking for null first.
        /// </summary>
        /// <typeparam name="TIn">The type of the parameter of the method that
        /// this delegate encapsulates.</typeparam>
        /// <typeparam name="TOut">The type of the output value.</typeparam>
        /// <param name="delegateFunction">A delegate that has a single
        /// parameter.</param>
        /// <param name="obj">The parameter of the method that this delegate
        /// encapsulates.</param>
        /// <returns>
        /// The default value of the <typeparamref name="TOut"/> if the delegate
        /// is null; otherwise, the result of the delegate.
        /// </returns>
        public static TOut InvokeSafe<TIn, TOut>(this Func<TIn, TOut> delegateFunction, TIn obj)
        {
            return delegateFunction == null ? default(TOut) : delegateFunction(obj);
        }

        /// <summary>
        /// Encapsulates a function that has a two parameters safely by checking
        /// for null first.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the method
        /// that this delegate encapsulates.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the method
        /// that this delegate encapsulates.</typeparam>
        /// <typeparam name="TOut">The type of the output value.</typeparam>
        /// <param name="delegateFunction">A delegate that has two input
        /// parameters.</param>
        /// <param name="obj1">The first parameter of the method that this
        /// delegate encapsulates.</param>
        /// <param name="obj2">The second parameter of the method that this
        /// delegate encapsulates.</param>
        /// <returns>
        /// The default value of the <typeparamref name="TOut"/> if the delegate
        /// is null; otherwise, the result of the delegate.
        /// </returns>
        public static TOut InvokeSafe<T1, T2, TOut>(this Func<T1, T2, TOut> delegateFunction, T1 obj1, T2 obj2)
        {
            return delegateFunction == null ? default(TOut) : delegateFunction(obj1, obj2);
        }

        /// <summary>
        /// Raises the event for the specified event handler asynchronously.
        /// </summary>
        /// <typeparam name="TEventArgs">The type of the event data generated by
        /// the event.</typeparam>
        /// <param name="handler">The event handler.</param>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An <see cref="System.EventArgs"/> that contains the
        /// event data.</param>
        public static Task InvokeAsync<TEventArgs>(this EventHandler<TEventArgs> handler, object sender, TEventArgs e)
            where TEventArgs : EventArgs
        {
            return Task.Factory.FromAsync((callback, obj) => handler.BeginInvoke(sender, e, callback, obj), handler.EndInvoke,
                null);
        }

        #endregion Invokes
    }
}