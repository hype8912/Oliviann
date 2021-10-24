namespace Oliviann.ComponentModel.Composition
{
    /// <summary>
    /// Represents an interface for MEF exports to retrieve type metadata on
    /// import.
    /// </summary>
    public interface IExportServiceMetadata
    {
        #region Properties

        /// <summary>
        /// Gets the name of the service host.
        /// </summary>
        /// <value>
        /// The name of the service host.
        /// </value>
        string HostName { get; }

        /// <summary>
        /// Gets the load order the service will be loaded in.
        /// </summary>
        /// <value>
        /// The load order the service will be loaded in.
        /// </value>
        uint LoadOrder { get; }

        #endregion Properties
    }
}