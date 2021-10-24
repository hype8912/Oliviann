namespace Oliviann.Windows
{
    #region Usings

    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;

    #endregion

    /// <summary>
    /// Represents a collection of commonly used extension methods for extending
    /// <see cref="Application"/>.
    /// </summary>
    public static class ApplicationExtensions
    {
        /// <summary>
        /// Determines whether the specified window name is open.
        /// </summary>
        /// <typeparam name="T">The Window type to search for.</typeparam>
        /// <param name="app">The application instance. If null the current
        /// application instance will be used.</param>
        /// <param name="name">Optional. The name of the specific window to
        /// search for.</param>
        /// <returns>True if the specified Window name is open; otherwise,
        /// false.</returns>
        public static bool IsWindowOpen<T>(this Application app, string name = "") where T : Window
        {
            if (app == null)
            {
                app = Application.Current;
            }

            IEnumerable<T> windows = app.Windows.OfType<T>();
            return name.IsNullOrEmpty() ? windows.Any() : windows.Any(w => w.Name == name);
        }
    }
}