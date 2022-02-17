namespace Oliviann.Web.UI.WebControls
{
    #region Usings

    using System.Linq;
    using System.Text;
    using System.Web.UI.WebControls;
    using Oliviann.Collections.Generic;

    #endregion Usings

    /// <summary>
    /// Represents a collection of commonly used extension methods for extending
    /// list controls.
    /// </summary>
    public static class ListControlExtensions
    {
        /// <summary>
        /// Binds a data source to the specified list control.
        /// </summary>
        /// <param name="control">The list control to bind the source.</param>
        /// <param name="textField">The text field property name.</param>
        /// <param name="valueField">The value field property name.</param>
        /// <param name="source">The data source to be bound.</param>
        public static void BindData(this ListControl control, string textField, string valueField, object source)
        {
            ADP.CheckArgumentNull(control, nameof(control));

            control.DataTextField = textField;
            control.DataValueField = valueField;
            control.DataSource = source;
            control.DataBind();
        }

        /// <summary>
        /// Concatenates the selected values of each <see cref="ListItem"/> of
        /// the specified <paramref name="control"/> using the specified
        /// <paramref name="separator"/>.
        /// </summary>
        /// <param name="control">The list control to iterate.</param>
        /// <param name="separator">The separator.</param>
        /// <returns>A single string of all the list item objects in a delimited
        /// string.</returns>
        public static string Concatenate(this ListControl control, string separator = ",")
        {
            if (control == null)
            {
                return string.Empty;
            }

            if (separator == null)
            {
                separator = ",";
            }

            var builder = new StringBuilder();
            foreach (ListItem item in control.Items)
            {
                if (item.Selected)
                {
                    builder.Append(item.Value + separator);
                }
            }

            return builder.ToString();
        }

        /// <summary>
        /// Resets the selected property of each <see cref="ListItem"/> in the
        /// specified <paramref name="control"/> to false.
        /// </summary>
        /// <param name="control">The list control to iterate.</param>
        public static void Reset(this ListControl control)
        {
            control?.Items.Cast<ListItem>().Where(l => l.Selected).ForEach(l => l.Selected = false);
        }

        /// <summary>
        /// Selects the item matching the specified <paramref name="text"/> in
        /// the <paramref name="control"/>.
        /// </summary>
        /// <param name="control">The control to search.</param>
        /// <param name="text">The text to search for.</param>
        /// <returns>True if the specified <paramref name="text"/> was found in
        /// the list control and set as selected; otherwise, false.</returns>
        public static bool SelectByText(this ListControl control, string text)
        {
            if (control == null)
            {
                return false;
            }

            ListItem item = control.Items.FindByText(text);
            return item.Select();
        }

        /// <summary>
        /// Selects the item matching the specified <paramref name="value"/> in
        /// the <paramref name="control"/>.
        /// </summary>
        /// <param name="control">The control to search.</param>
        /// <param name="value">The value to search for.</param>
        /// <returns>True if the specified <paramref name="value"/> was found in
        /// the list control and set as selected; otherwise, false.</returns>
        public static bool SelectByValue(this ListControl control, string value)
        {
            if (control == null)
            {
                return false;
            }

            ListItem item = control.Items.FindByValue(value);
            return item.Select();
        }

        /// <summary>
        /// Selects the specified <paramref name="item"/> safely.
        /// </summary>
        /// <param name="item">The current list item to be selected.</param>
        /// <returns>True if the specified <paramref name="item"/> set as
        /// selected; otherwise, false.</returns>
        public static bool Select(this ListItem item)
        {
            if (item == null)
            {
                return false;
            }

            item.Selected = true;
            return true;
        }
    }
}