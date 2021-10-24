namespace Oliviann.Xml
{
    #region Usings

    using System.Xml;

    #endregion Usings

    /// <summary>
    /// Represents a collection of commonly used extension methods for extending
    /// xml objects.
    /// </summary>
    public static class XmlExtensions
    {
        /// <summary>
        /// Gets the single element node inner text from the specified XML
        /// <paramref name="node"/>.
        /// </summary>
        /// <param name="node">The <see cref="T:System.Xml.XmlNode"/> object.
        /// </param>
        /// <returns>The inner text for the specified <paramref name="node"/> if
        /// not null; otherwise, an empty string.
        /// </returns>
        public static string InnerTextOrDefault(this XmlNode node)
        {
            return node.GetValueOrDefault(n => n.InnerText, string.Empty);
        }

        /// <summary>
        /// Gets the concatenated values of the node and all its children.
        /// </summary>
        /// <param name="document">The XML document object.</param>
        /// <param name="elementTagName">The element tag name.</param>
        /// <returns>The concatenated values of the node and all its child
        /// nodes.
        /// </returns>
        public static string GetElementByTagName(this XmlDocument document, string elementTagName)
        {
            ADP.CheckArgumentNull(document, nameof(document));
            XmlNodeList lst = document.GetElementsByTagName(elementTagName);
            return lst.Count > 0 ? lst[0].InnerText : null;
        }
    }
}