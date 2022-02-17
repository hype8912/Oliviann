namespace Oliviann.Web.UI.WebControls
{
    #region Usings

    using System.Web.UI.WebControls;

    #endregion Usings

    /// <summary>
    /// Represents a collection of commonly used extension methods for extending
    /// list items.
    /// </summary>
    public static class ListItemExtensions
    {
        /// <summary>
        /// Sets the tool tip for the specified item with the specific text.
        /// </summary>
        /// <param name="item">The list item to be set.</param>
        /// <param name="text">Optional. The text for the tool tip to display if
        /// different that the item text. Default value is the item text.
        /// </param>
        public static void SetToolTip(this ListItem item, string text = null) => item?.Attributes.Add("title", text ?? item.Text);
    }
}