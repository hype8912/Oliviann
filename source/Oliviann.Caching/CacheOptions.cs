namespace Oliviann.Caching
{
    #region Usings

    using System.Collections.Generic;

    #endregion Usings

    /// <summary>
    /// Represents a collection of cache options. Not all implementations will
    /// take advantage of each cache option.
    /// </summary>
    public class CacheOptions
    {
        #region Constructor/Destructor

        /// <summary>
        /// Initializes a new instance of the <see cref="CacheOptions" /> class.
        /// </summary>
        public CacheOptions()
        {
            this.Servers = new List<CacheServerEntry>();
        }

        #endregion Constructor/Destructor

        #region Properties

        /// <summary>
        /// Gets or sets a collection of cache server entries.
        /// </summary>
        /// <value>
        /// The collection of cache server entries.
        /// </value>
        public ICollection<CacheServerEntry> Servers { get; set; }

        #endregion Properties
    }
}