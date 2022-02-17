namespace Oliviann.Caching.Redis
{
    #region Usings

    using System;

    #endregion

    /// <summary>
    /// Represents an implementation for a collection of configurable options
    /// for a Redis provider.
    /// </summary>
    public interface IRedisOptions
    {
        #region Properties

        /// <summary>
        /// Gets or sets the connection string.
        /// </summary>
        /// <value>
        /// The connection string.
        /// </value>
        string ConnectionString { get; set; }

        /// <summary>
        /// Gets or sets the serializer function.
        /// </summary>
        /// <value>
        /// The serializer function.
        /// </value>
        Func<object, byte[]> Serializer { get; set; }

        /// <summary>
        /// Gets or sets the deserializer function.
        /// </summary>
        /// <value>
        /// The deserializer function.
        /// </value>
        Func<byte[], object> Deserializer { get; set; }

        #endregion
    }
}