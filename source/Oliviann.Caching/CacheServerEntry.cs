namespace Oliviann.Caching
{
    /// <summary>
    /// Represents a cache server entry for a distributed cache system.
    /// </summary>
    public class CacheServerEntry
    {
        #region Properties

        /// <summary>
        /// Gets or sets the name of the cache server.
        /// </summary>
        /// <value>
        /// The name of the cache server.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the port number for the cache.
        /// </summary>
        /// <value>
        /// The port number for the cache.
        /// </value>
        public ushort? Port { get; set; }

        #endregion Properties
    }
}