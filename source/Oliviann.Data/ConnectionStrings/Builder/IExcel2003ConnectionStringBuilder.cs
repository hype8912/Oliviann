namespace Oliviann.Data.ConnectionStrings.Builder
{
    /// <summary>
    /// Interface for creating an Ms Excel 97-2003 connection string.
    /// </summary>
    public interface IExcel2003ConnectionStringBuilder
    {
        /// <summary>
        /// Sets the data source.
        /// </summary>
        /// <value>The data source.</value>
        string DataSource { set; }

        /// <summary>
        /// Sets the provider.
        /// </summary>
        /// <value>The provider.</value>
        string Provider { set; }

        /// <summary>
        /// Gets and creates a database connection string.
        /// </summary>
        /// <returns>A complete connection string based on the parameters.</returns>
        string ConnectionString { get; }

        /// <summary>
        /// Adds a custom attribute and value if item is not available.
        /// </summary>
        /// <param name="attribute">The attribute name.</param>
        /// <param name="value">The attribute value.</param>
        void CustomAttribute(string attribute, string value);
    }
}