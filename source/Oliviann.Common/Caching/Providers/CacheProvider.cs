namespace Oliviann.Caching.Providers
{
    #region Usings

    using System.Collections.Generic;
    using Oliviann.Collections.Generic;

    #endregion

    /// <summary>
    /// Represents a base class for a cache provider implementation.
    /// </summary>
    public abstract class CacheProvider : ICacheProvider
    {
        #region Constructor/Destructor

        /// <summary>
        /// Initializes a new instance of the <see cref="CacheProvider" />
        /// class.
        /// </summary>
        /// <param name="name">The unique name for the provider.</param>
        protected CacheProvider(string name)
        {
            this.Name = name;
        }

        #endregion Constructor/Destructor

        #region Properties

        /// <inheritdoc />
        public string Name { get; private set; }

        #endregion Properties

        #region Methods

        /// <inheritdoc />
        public abstract bool Add(CacheEntry entry);

        /// <inheritdoc />
        public abstract bool Contains(CacheEntry entry);

        /// <inheritdoc />
        public abstract object Get(CacheEntry entry);

        /// <inheritdoc />
        public abstract IReadOnlyCollection<string> GetKeys();

        /// <inheritdoc />
        public abstract void Set(CacheEntry entry);

        /// <inheritdoc />
        public abstract object Remove(CacheEntry entry);

        #endregion Methods
    }
}