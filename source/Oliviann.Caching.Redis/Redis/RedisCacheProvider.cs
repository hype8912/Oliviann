namespace Oliviann.Caching.Redis
{
    #region Usings

    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Runtime.Serialization.Formatters;
    using System.Runtime.Serialization.Formatters.Binary;
    using Oliviann.Caching.Providers;
    using StackExchange.Redis;

    #endregion Usings

    /// <summary>
    /// Represents a Redis cache provider.
    /// </summary>
    /// <remarks>Any object that will be added to the cache must have the
    /// <c>serializable</c> attribute on it if the default serializers are used.
    /// The default serializer is <see cref="BinaryFormatter"/> and comes with
    /// certain risks. Never round trip data to a cache you do not trust.
    /// </remarks>
    public class RedisCacheProvider : CacheProvider, IDisposable
    {
        #region Fields

        /// <summary>
        /// The default cache provider name.
        /// </summary>
        private const string DefaultName = "RedisCache";

        /// <summary>
        /// The current Redis configured options.
        /// </summary>
        private readonly IRedisOptions options;

        /// <summary>
        /// The Redis connection instance.
        /// </summary>
        private ConnectionMultiplexer redisConnection;

        #endregion Fields

        #region Constructor/Destructor

        /// <summary>
        /// Initializes a new instance of the <see cref="RedisCacheProvider"/>
        /// class.
        /// </summary>
        public RedisCacheProvider() : this(DefaultName)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RedisCacheProvider"/>
        /// class.
        /// </summary>
        /// <param name="name">The unique name for the provider.</param>
        public RedisCacheProvider(string name) : this(name, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RedisCacheProvider" />
        /// class.
        /// </summary>
        /// <param name="redisOptions">The redis options.</param>
        public RedisCacheProvider(IRedisOptions redisOptions) : this(DefaultName, redisOptions)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RedisCacheProvider" />
        /// class.
        /// </summary>
        /// <param name="name">The unique name for the provider.</param>
        /// <param name="redisOptions">The redis options.</param>
        /// <exception cref="InvalidOperationException">Invalid serializer
        /// configuration. Either both the serializer and deserializer must be
        /// set or both must be null.</exception>
        public RedisCacheProvider(string name, IRedisOptions redisOptions) : base(name ?? DefaultName)
        {
            this.options = redisOptions ?? new RedisOptions();
            if ((this.options.Deserializer == null && this.options.Serializer != null) ||
                (this.options.Deserializer != null && this.options.Serializer == null))
            {
                throw new InvalidOperationException();
            }

            if (this.options.Serializer == null)
            {
                this.options.Serializer = this.Serialize;
            }

            if (this.options.Deserializer == null)
            {
                this.options.Deserializer = this.Deserialize;
            }
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="RedisCacheProvider"/> class.
        /// </summary>
        ~RedisCacheProvider()
        {
            this.Dispose(false);
        }

        #endregion Constructor/Destructor

        #region Properties

        /// <summary>
        /// Gets or sets the serializer function. Default is
        /// <see cref="BinaryFormatter"/>.
        /// </summary>
        /// <value>
        /// The serializer function.
        /// </value>
        [Obsolete("Use RedisOptions instead.")]
        public Func<object, byte[]> Serializer
        {
            get => this.options.Serializer;
            set => this.options.Serializer = value;
        }

        /// <summary>
        /// Gets or sets the deserializer function. Default is
        /// <see cref="BinaryFormatter"/>.
        /// </summary>
        /// <value>
        /// The deserializer function.
        /// </value>
        [Obsolete("Use RedisOptions instead.")]
        public Func<byte[], object> Deserializer
        {
            get => this.options.Deserializer;
            set => this.options.Deserializer = value;
        }

        /// <summary>
        /// Gets or sets the connection string.
        /// </summary>
        /// <value>
        /// The connection string.
        /// </value>
        [Obsolete("Use RedisOptions instead.")]
        public string ConnectionString
        {
            get => this.options.ConnectionString;
            set => this.options.ConnectionString = value;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        public override bool Add(CacheEntry entry)
        {
            if (entry == null || entry.Key.IsNullOrEmpty())
            {
                return false;
            }

            byte[] serializedData = this.options.Serializer(entry.Value);
            if (serializedData == null)
            {
                return false;
            }

            TimeSpan? expire = entry.AbsoluteExpiration == DateTimeOffset.MaxValue
                ? (TimeSpan?)null
                : entry.AbsoluteExpiration - DateTimeOffset.UtcNow;

            bool result = this.GetDatabase().StringSet(entry.Key, serializedData, expire, When.NotExists);
            return result;
        }

        /// <inheritdoc />
        public override bool Contains(CacheEntry entry) => this.GetDatabase().KeyExists(entry.Key);

        /// <inheritdoc />
        public override object Get(CacheEntry entry)
        {
            RedisValue value = this.GetDatabase().StringGet(entry.Key);
            return value.IsNull ? null : this.options.Deserializer(value);
        }

        /// <inheritdoc />
        public override IReadOnlyCollection<string> GetKeys()
        {
            var keys = new List<string>();
            if (this.redisConnection == null)
            {
                this.redisConnection = this.CreateConnection();
            }

            EndPoint[] endpoints = this.redisConnection.GetEndPoints();
            foreach (EndPoint endpoint in endpoints)
            {
                IServer server = this.redisConnection.GetServer(endpoint);
                IEnumerable<RedisKey> serverKeys = server.Keys(pattern: "*");
                keys.AddRange(serverKeys.Select(serverKey => serverKey.ToString()));
            }

            return keys;
        }

        /// <inheritdoc />
        public override void Set(CacheEntry entry)
        {
            byte[] serializedData = this.options.Serializer(entry.Value);
            if (serializedData == null)
            {
                return;
            }

            TimeSpan? expire = entry.AbsoluteExpiration == DateTimeOffset.MaxValue
                ? (TimeSpan?)null
                : entry.AbsoluteExpiration - DateTimeOffset.UtcNow;

            this.GetDatabase().StringSet(entry.Key, serializedData, expire);
        }

        /// <inheritdoc />
        public override object Remove(CacheEntry entry)
        {
            object value = this.Get(entry);
            this.GetDatabase().KeyDelete(entry.Key);
            return value;
        }

        /// <inheritdoc />
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing">True to release both managed and unmanaged
        /// resources; false to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.redisConnection.DisposeSafe();
                this.redisConnection = null;
            }
        }

        #endregion Methods

        #region Helper Methods

        /// <summary>
        /// Gets a new Redis database connection for communicating with the
        /// Redis server(s).
        /// </summary>
        /// <returns>A new Redis database connection.</returns>
#if DEBUG

        internal
#else
        private
#endif
            IDatabase GetDatabase()
        {
            if (this.redisConnection == null)
            {
                this.redisConnection = this.CreateConnection();
            }

            return this.redisConnection.GetDatabase();
        }

        /// <summary>
        /// Creates a new Redis connection for communicating with the Redis
        /// server(s).
        /// </summary>
        /// <returns>A newly created Redis connection.</returns>
        /// <exception cref="ArgumentException">Redis connection string cannot
        /// be empty. See documentation on how to set the connection string.
        /// </exception>
        private ConnectionMultiplexer CreateConnection()
        {
            string connectionString = this.options.ConnectionString;
            if (connectionString.IsNullOrWhiteSpace())
            {
                throw new ArgumentException("Redis connection string cannot be empty. See documentation on how to set the connection string.");
            }

            return ConnectionMultiplexer.Connect(connectionString);
        }

        /// <summary>
        /// Serializes the specified item.
        /// </summary>
        /// <param name="item">The item to be serialized.</param>
        /// <returns>A byte array that represents the input object.</returns>
        private byte[] Serialize(object item)
        {
            if (item == null)
            {
                return null;
            }

            byte[] serializedData;
            using (var stream = new MemoryStream())
            {
                this.GetFormatter().Serialize(stream, item);
                serializedData = stream.ToArray();
            }

            return serializedData;
        }

        /// <summary>
        /// Deserializes the specified data.
        /// </summary>
        /// <param name="data">The serialized data to be deserialized.</param>
        /// <returns>An object that represents the input object.</returns>
        private object Deserialize(byte[] data)
        {
            if (data == null)
            {
                return null;
            }

            object deserializedData;
            using (var stream = new MemoryStream(data))
            {
                stream.Seek(0, SeekOrigin.Begin);
                deserializedData = this.GetFormatter().Deserialize(stream);
            }

            return deserializedData;
        }

        /// <summary>
        /// Gets a new instance of the serialization formatter.
        /// </summary>
        /// <returns>A new configured binary formatter instance.</returns>
        private BinaryFormatter GetFormatter() => new BinaryFormatter { TypeFormat = FormatterTypeStyle.TypesWhenNeeded };

        #endregion Helper Methods
    }
}