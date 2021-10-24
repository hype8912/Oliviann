namespace Oliviann.Windows
{
    #region Usings

    using System;
    using System.Windows;
    using System.Windows.Threading;

    #endregion

    /// <summary>
    /// Represents a collection of commonly used extension methods for extending
    /// UI Elements.
    /// </summary>
    public static class UIElementExtensions
    {
        /// <summary>
        /// Refreshes the specified element in the Window.
        /// </summary>
        /// <param name="element">The element.</param>
        public static void Refresh(this UIElement element)
        {
            Action emptyDelegate = () => { };
            element?.Dispatcher?.Invoke(DispatcherPriority.Render, emptyDelegate);
        }
    }
}