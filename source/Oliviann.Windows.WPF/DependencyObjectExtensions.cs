namespace Oliviann.Windows
{
    #region Usings

    using System.Windows;
    using System.Windows.Media;

    #endregion

    /// <summary>
    /// Represents a collection of commonly used extension methods for extending
    /// dependency objects.
    /// </summary>
    public static class DependencyObjectExtensions
    {
        /// <summary>
        /// Gets the first child control object of the specified type.
        /// </summary>
        /// <typeparam name="T">The type of child control to look for.
        /// </typeparam>
        /// <param name="dependency">The parent control.</param>
        /// <returns>The child control of the specified type if found;
        /// otherwise, false.</returns>
        public static T GetChildOfType<T>(this DependencyObject dependency) where T : DependencyObject
        {
            if (dependency == null)
            {
                return null;
            }

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(dependency); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(dependency, i);
                var result = child as T ?? child.GetChildOfType<T>();
                if (result != null)
                {
                    return result;
                }
            }

            return null;
        }

        /// <summary>
        /// Gets the parent window of the specified child object.
        /// </summary>
        /// <param name="child">The child control.</param>
        /// <returns>The parent Window of the specified child if found;
        /// otherwise, null.</returns>
        public static Window GetParentWindow(this DependencyObject child)
        {
            DependencyObject parent = child;
            while (parent != null)
            {
                if (parent is Window parentWindow)
                {
                    return parentWindow;
                }

                parent = LogicalTreeHelper.GetParent(parent);
            }

            return null;
        }
    }
}