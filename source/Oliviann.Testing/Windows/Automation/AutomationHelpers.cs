#if NET40 || NET45

namespace Oliviann.Testing.Windows.Automation
{
    #region Usings

    using System.Windows.Automation;

    #endregion Usings

    /// <summary>
    /// Represents a collection of commonly used helper methods for making
    /// working with <see cref="System.Windows.Automation"/> namespace easier.
    /// </summary>
    public static class AutomationHelpers
    {
        /// <summary>
        /// Finds a window on the desktop by the specified
        /// <paramref name="windowName"/>.
        /// </summary>
        /// <param name="windowName">Name of the window to find.</param>
        /// <returns>An automation element to the specified window if found,
        /// otherwise, null.</returns>
        public static AutomationElement FindWindowByName(string windowName) =>
            AutomationElement.RootElement.FindWindowByName(windowName);

        /// <summary>
        /// Tries to find a window on the desktop by the specified
        /// <paramref name="windowName"/> for 30 seconds.
        /// </summary>
        /// <param name="windowName">Name of the window.</param>
        /// <returns>An automation element to the specified window if found in
        /// 30 seconds; otherwise, null.</returns>
        public static AutomationElement FindWindowByNameWait(string windowName) =>
            AutomationElement.RootElement.FindWindowByNameWait(windowName);
    }
}

#endif