namespace Oliviann.Xml.Linq
{
    #region Usings

    using System.Xml.Linq;

    #endregion Usings

    /// <summary>
    /// Represents a collection of commonly used extension methods for extending
    /// xml elements.
    /// </summary>
    public static class XElementExtensions
    {
        /// <summary>
        /// Gets the concatenated text contents of this element if not null.
        /// </summary>
        /// <param name="element">The XML element to be read.</param>
        /// <param name="defaultValue">Default value is an empty string. The
        /// default value to be returned if the element is null.</param>
        /// <returns>
        /// A <see cref="T:System.String" /> that contains all of the text
        /// content of this element. If there are multiple text nodes, they will
        /// be concatenated into a single string; otherwise, the default value
        /// to be returned.
        /// </returns>
        public static string ValueOrDefault(this XElement element, string defaultValue = "") => element?.Value ?? defaultValue;
    }
}