namespace Oliviann.Web.UI.WebControls
{
    #region Usings

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.UI.WebControls;

    #endregion Usings

    /// <summary>
    /// Represents a collection of commonly used extension methods for extending
    /// list item collections.
    /// </summary>
    public static class ListItemCollectionExtensions
    {
        /// <summary>
        /// Sorts the items in the collection by the text using the current
        /// culture.
        /// </summary>
        /// <param name="items">The collection of items to be sorted.</param>
        /// <param name="descending">Optional. Sorts the items in descending
        /// order if set to true; otherwise, false.</param>
        /// <remarks>This method was found to be faster for sorting strings in a
        /// small set of results instead of using OrderBy. Although in large
        /// sets of over 75,000 items, OrderBy has been found to be faster than
        /// List sort.</remarks>
        public static void SortByText(this ListItemCollection items, bool descending = false)
        {
            if (items == null || items.Count < 2)
            {
                return;
            }

            List<ListItem> list = items.Cast<ListItem>().ToList();
            if (descending)
            {
                list.Sort((x, y) => string.Compare(y.Text, x.Text, StringComparison.CurrentCulture));
            }
            else
            {
                list.Sort((x, y) => string.Compare(x.Text, y.Text, StringComparison.CurrentCulture));
            }

            items.Clear();
            foreach (ListItem item in list)
            {
                items.Add(item);
            }
        }
    }
}