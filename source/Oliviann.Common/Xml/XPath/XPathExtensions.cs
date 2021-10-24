namespace Oliviann.Xml.XPath
{
    #region Usings

    using System.Xml.XPath;

    #endregion Usings

    /// <summary>
    /// Represents a collection of commonly used extension methods for extending
    /// XPath objects.
    /// </summary>
    public static class XPathExtensions
    {
        /// <summary>
        /// Gets the current node's string value if not null; otherwise and
        /// empty string.
        /// </summary>
        /// <param name="item">The <see cref="T:System.Xml.XPath.XPathItem"/>
        /// object.
        /// </param>
        /// <returns>The current value of the <paramref name="item"/> if not
        /// null; otherwise, an empty string.</returns>
        public static string ValueOrDefault(this XPathItem item) => item.GetValueOrDefault(x => x.Value, string.Empty);
    }
}