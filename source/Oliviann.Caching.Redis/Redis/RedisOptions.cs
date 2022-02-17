namespace Oliviann.Caching.Redis
{
    #region Usings

    using System;
    using System.Runtime.Serialization.Formatters.Binary;

    #endregion

    /// <summary>
    /// Represents a collection of configurable options for the Redis cache
    /// provider.
    /// </summary>
    public class RedisOptions : IRedisOptions
    {
        #region Properties

        /// <summary>
        /// Gets or sets the Redis connection string.
        /// </summary>
        /// <value>
        /// The Redis connection string.
        /// </value>
        public string ConnectionString { get; set; }

        /// <summary>
        /// Gets or sets the serializer implementation. Default is
        /// <see cref="BinaryFormatter"/>.
        /// </summary>
        /// <value>
        /// The serializer implementation.
        /// </value>
        public Func<object, byte[]> Serializer { get; set; }

        /// <summary>
        /// Gets or sets the deserializer implementation. Default is
        /// <see cref="BinaryFormatter"/>.
        /// </summary>
        /// <value>
        /// The deserializer implementation.
        /// </value>
        public Func<byte[], object> Deserializer { get; set; }

        #endregion
    }
}