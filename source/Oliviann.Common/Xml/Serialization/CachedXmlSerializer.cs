#if !NET35

namespace Oliviann.Xml.Serialization
{
    #region Usings

    using System;
    using System.Diagnostics;
    using System.IO;
#if NETFRAMEWORK
    using Oliviann.Runtime.Caching;
    using System.Runtime.Caching;
#else
    using Microsoft.Extensions.Caching.Memory;
#endif
    using System.Runtime.Serialization;
    using System.Xml.Serialization;
    using Oliviann.Properties;

    #endregion Usings

    /// <summary>
    /// Represents a generic class for serializing data to/from a file or string
    /// with additional cache capabilities.
    /// </summary>
    /// <typeparam name="T">The type object for serializing. Must be of type
    /// class.</typeparam>
    public class CachedXmlSerializer<T> : BasicXmlSerializer<T> where T : class
    {
        #region Contsructor/Destructor

#if NETFRAMEWORK

        /// <summary>
        /// The current cache instance.
        /// </summary>
        private readonly MemoryCache cacheInstance = MemoryCache.Default;

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="CachedXmlSerializer{T}"/> class.
        /// </summary>
        public CachedXmlSerializer() : this(new CacheItemPolicy
                                                {
                                                    AbsoluteExpiration = ObjectCache.InfiniteAbsoluteExpiration,
                                                    SlidingExpiration = ObjectCache.NoSlidingExpiration,
                                                    Priority = CacheItemPriority.Default,
                                                    RemovedCallback = null
                                                })
        {
        }

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="CachedXmlSerializer{T}"/> class.
        /// </summary>
        /// <param name="cachePolicy">The set of eviction details for a specific
        /// serializer cache entry.</param>
        public CachedXmlSerializer(CacheItemPolicy cachePolicy)
        {
            this.Policy = cachePolicy;
        }

#else
        /// <summary>
        /// The current cache instance.
        /// </summary>
        private readonly IMemoryCache cacheInstance;

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="CachedXmlSerializer{T}"/> class.
        /// </summary>
        /// <param name="cache">The current application cache instance.</param>
        public CachedXmlSerializer(IMemoryCache cache) : this(cache, new MemoryCacheEntryOptions())
        {
        }

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="CachedXmlSerializer{T}"/> class.
        /// </summary>
        /// <param name="cache">The current application cache instance.</param>
        /// <param name="cacheOptions">The set of eviction details for a
        /// specific serializer cache entry.</param>
        public CachedXmlSerializer(IMemoryCache cache, MemoryCacheEntryOptions cacheOptions)
        {
            this.cacheInstance = cache;
            this.Options = cacheOptions;
        }
#endif

        #endregion Contsructor/Destructor

        #region Properties

#if NETFRAMEWORK

        /// <summary>Gets or sets the cache entry policy details.</summary>
        /// <value>The cache entry policy.</value>
        /// <remarks>If you change the policy after inserting a few items, then
        /// the new policy will only apply to the newly inserted items.
        /// </remarks>
        protected CacheItemPolicy Policy { get; set; }

#else
        /// <summary>Gets or sets the cache entry options details.</summary>
        /// <value>The cache entry options.</value>
        /// <remarks>If you change the options after inserting a few items, then
        /// the new options will only apply to the newly inserted items.
        /// </remarks>
        protected MemoryCacheEntryOptions Options { get; set; }
#endif

        #endregion Properties

        #region File

        /// <summary>
        /// Serializes the model data for type T to the specified
        /// <paramref name="filePath"/>.
        /// </summary>
        /// <param name="filePath">The file path of the XML file.</param>
        /// <param name="serializableData">The serializable data model object.
        /// </param>
        /// <exception cref="SerializationException">An error occurred
        /// serializing file. See inner exception for more details.
        /// </exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope",
            Justification = "Everything is in a try/finally block so it is being caught and disposed correctly.")]
        public new void SerializeFile(string filePath, T serializableData)
        {
            ADP.CheckArgumentNull(filePath, nameof(filePath));
            FileStream stream = null;
            StreamWriter writer = null;
            XmlSerializer serializer = null;

            try
            {
                serializer = this.GetSerializer();
#if NETFRAMEWORK
                if (this.Policy != null && this.Policy.RemovedCallback != null)
                {
                    this.RemoveSerializer();
                }
#else
                if (this.Options != null && this.Options.PostEvictionCallbacks.Count > 0)
                {
                    this.RemoveSerializer();
                }
#endif

                stream = File.Open(filePath, FileMode.Create);
                writer = new StreamWriter(stream) { AutoFlush = false };
                serializer.Serialize(writer, serializableData);
                writer.Flush();
            }
            catch (Exception inner)
            {
                var ex = new SerializationException(Resources.ERR_SerializingInnerEx.FormatWith("file"), inner);
                ex.Data.Add("FilePath", filePath);
                Trace.TraceError(ex.ToString());
                throw ex;
            }
            finally
            {
                writer.DisposeSafe();
                stream.DisposeSafe();
                this.AddSerializer(serializer);
            }
        }

#endregion File

#region Serializer

        /// <summary>
        /// Removes the cached serializer from the common cache.
        /// </summary>
        public void RemoveSerializer()
        {
            string key = GenerateCacheKey();
            if (key != null)
            {
                this.cacheInstance.Remove(key);
            }
        }

        /// <summary>
        /// Gets the a serializer object reference for the type T. If serializer
        /// already exists in cache then object will be pulled from the versus
        /// creating a new serializer instance.
        /// </summary>
        /// <returns>An XML serializer instance for the type T.</returns>
        /// <remarks>
        /// Creating serializer objects are costly because they use reflection
        /// to read all the objects. Caching and reusing the serializer will
        /// reduce load for further requests. Profiling has shown that the
        /// initial request will take about 740ms to complete. Each following
        /// cached request only takes about 100ms.
        /// </remarks>
        protected override XmlSerializer GetSerializer()
        {
            string key = GenerateCacheKey();
            if (key != null && this.cacheInstance.TryGetValue(key, out object cacheSerializer))
            {
                return (XmlSerializer)cacheSerializer;
            }

            var serializer = new XmlSerializer(typeof(T));
            this.AddSerializer(serializer);
            return serializer;
        }

        /// <summary>
        /// Generates a unique cache key for the type T.
        /// </summary>
        /// <returns>A string representing the key for caching.</returns>
        private static string GenerateCacheKey() => typeof(T).FullName;

        /// <summary>
        /// Adds the serializer to the common cache.
        /// </summary>
        /// <param name="serializer">The XML serializer object.</param>
        private void AddSerializer(XmlSerializer serializer)
        {
            string key = GenerateCacheKey();
            if (key != null && serializer != null && this.cacheInstance.Get(key) == null)
            {
#if NETFRAMEWORK
                this.cacheInstance.Add(new CacheItem(key, serializer), this.Policy);
#else
                this.cacheInstance.Set(key, serializer, this.Options);
#endif
            }
        }

#endregion Serializer
    }
}

#endif