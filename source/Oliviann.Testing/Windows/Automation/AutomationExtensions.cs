#if NET40 || NET45

namespace Oliviann.Testing.Windows.Automation
{
    #region Usings

    using System;
    using System.Threading;
    using System.Windows.Automation;

    #endregion Usings

    /// <summary>
    /// Represents a collection of commonly used extension methods for extending
    /// <see cref="System.Windows.Automation"/>.
    /// </summary>
    public static class AutomationExtensions
    {
        /// <summary>
        /// Finds a button on the parent element by the specified
        /// <paramref name="id"/>.
        /// </summary>
        /// <param name="parentElement">The parent element to search.</param>
        /// <param name="id">The automation id to search for.</param>
        /// <returns>An automation element to the specified button if found,
        /// otherwise, null.</returns>
        public static AutomationElement FindButtonByAutomationId(this AutomationElement parentElement, string id)
        {
            return parentElement.FindFirst(
                TreeScope.Descendants,
                new AndCondition(
                    new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Button),
                    new PropertyCondition(AutomationElement.AutomationIdProperty, id)));
        }

        /// <summary>
        /// Finds a button on the parent element by the specified
        /// <paramref name="name"/>.
        /// </summary>
        /// <param name="parentElement">The parent element to search.</param>
        /// <param name="name">The button name to search for.</param>
        /// <returns>An automation element to the specified button if found,
        /// otherwise, null.</returns>
        public static AutomationElement FindButtonByName(this AutomationElement parentElement, string name)
        {
            return parentElement.FindFirst(
                TreeScope.Descendants,
                new AndCondition(
                    new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Button),
                    new PropertyCondition(AutomationElement.NameProperty, name)));
        }

        /// <summary>
        /// Finds a check box on the parent element by the specified
        /// <paramref name="id"/>.
        /// </summary>
        /// <param name="parentElement">The parent element to search.</param>
        /// <param name="id">The automation id to search for.</param>
        /// <returns>An automation element to the specified check box if found,
        /// otherwise, null.</returns>
        public static AutomationElement FindCheckBoxByAutomationId(this AutomationElement parentElement, string id)
        {
            return parentElement.FindFirst(
                TreeScope.Descendants,
                new AndCondition(
                    new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.CheckBox),
                    new PropertyCondition(AutomationElement.AutomationIdProperty, id)));
        }

        /// <summary>
        /// Finds a label on the parent element by the specified
        /// <paramref name="id"/>.
        /// </summary>
        /// <param name="parentElement">The parent element to search.</param>
        /// <param name="id">The automation id to search for.</param>
        /// <returns>An automation element to the specified label if found,
        /// otherwise, null.</returns>
        public static AutomationElement FindLabelByAutomationId(this AutomationElement parentElement, string id)
        {
            return parentElement.FindFirst(
                TreeScope.Descendants,
                new AndCondition(
                    new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Text),
                    new PropertyCondition(AutomationElement.AutomationIdProperty, id)));
        }

        /// <summary>
        /// Finds a menu item on the parent element by the specified
        /// <paramref name="id"/>.
        /// </summary>
        /// <param name="parentElement">The parent element to search.</param>
        /// <param name="id">The automation id to search for.</param>
        /// <returns>An automation element to the specified menu item if found,
        /// otherwise, null.</returns>
        public static AutomationElement FindMenuItemByAutomationId(this AutomationElement parentElement, string id)
        {
            return parentElement.FindFirst(
                TreeScope.Descendants,
                new AndCondition(
                    new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.MenuItem),
                    new PropertyCondition(AutomationElement.AutomationIdProperty, id)));
        }

        /// <summary>
        /// Finds a menu item on the parent element by the specified
        /// <paramref name="name"/>.
        /// </summary>
        /// <param name="parentElement">The parent element to search.</param>
        /// <param name="name">The menu item name to search for.</param>
        /// <returns>An automation element to the specified menu item if found,
        /// otherwise, null.</returns>
        public static AutomationElement FindMenuItemByName(this AutomationElement parentElement, string name)
        {
            return parentElement.FindFirst(
                TreeScope.Descendants,
                new AndCondition(
                    new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.MenuItem),
                    new PropertyCondition(AutomationElement.NameProperty, name)));
        }

        /// <summary>
        /// Finds a text box on the parent element by the specified
        /// <paramref name="id"/>.
        /// </summary>
        /// <param name="parentElement">The parent element to search.</param>
        /// <param name="id">The automation id to search for.</param>
        /// <returns>An automation element to the specified text box if found,
        /// otherwise, null.</returns>
        public static AutomationElement FindTextBoxByAutomationId(this AutomationElement parentElement, string id)
        {
            return parentElement.FindFirst(
                TreeScope.Descendants,
                new AndCondition(
                    new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Edit),
                    new PropertyCondition(AutomationElement.AutomationIdProperty, id)));
        }

        /// <summary>
        /// Finds a window on the parent element by the specified
        /// <paramref name="windowName"/>.
        /// </summary>
        /// <param name="parentElement">The parent element to search.</param>
        /// <param name="windowName">Name of the window to find.</param>
        /// <returns>An automation element to the specified window if found,
        /// otherwise, null.</returns>
        public static AutomationElement FindWindowByName(this AutomationElement parentElement, string windowName)
        {
            return parentElement.FindFirst(
                TreeScope.Descendants,
                new AndCondition(
                    new PropertyCondition(AutomationElement.NameProperty, windowName),
                    new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Window)));
        }

        /// <summary>
        /// Tries to find a window on the parent element by the specified
        /// <paramref name="windowName" /> for 30 seconds. The window object
        /// will be return as soon as the object is found.
        /// </summary>
        /// <param name="parentElement">The parent element to search.</param>
        /// <param name="windowName">Name of the window to find.</param>
        /// <returns>
        /// An automation element to the specified window if found in the
        /// specified time limit, otherwise, null.
        /// </returns>
        /// <remarks>
        /// Searches for the window every 250 milliseconds for the specified
        /// timeout period and returns it if found early.
        /// </remarks>
        public static AutomationElement FindWindowByNameWait(this AutomationElement parentElement, string windowName) =>
            FindWindowByNameWait(parentElement, windowName, TimeSpan.FromSeconds(30D));

        /// <summary>
        /// Tries to find a window on the parent element by the specified
        /// <paramref name="windowName" /> for the specified timespan. The
        /// window object will be return as soon as the object is found.
        /// </summary>
        /// <param name="parentElement">The parent element to search.</param>
        /// <param name="windowName">Name of the window to find.</param>
        /// <param name="timeout">The timespan to search for the window object
        /// </param>
        /// <returns>
        /// An automation element to the specified window if found in the
        /// specified time limit, otherwise, null.
        /// </returns>
        /// <remarks>
        /// Searches for the window every 250 milliseconds for the specified
        /// timeout period and returns it if found early.
        /// </remarks>
        public static AutomationElement FindWindowByNameWait(
            this AutomationElement parentElement,
            string windowName,
            TimeSpan timeout)
        {
            double timeoutMilliseconds = timeout.TotalMilliseconds;
            AutomationElement window = null;
            int elapsed = 0;
            while (elapsed < timeoutMilliseconds)
            {
                window = parentElement.FindWindowByName(windowName);
                if (window != null)
                {
                    break;
                }

                Thread.Sleep(250);
                elapsed += 250;
            }

            return window;
        }
    }
}

#endif