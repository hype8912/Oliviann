namespace Oliviann.Security.Cryptography
{
    #region Usings

    using System;
    using System.Security.Cryptography;
    using Oliviann.Properties;

    #endregion Usings

    /// <summary>
    /// Represents a secure random number generator using
    /// <see cref="RandomNumberGenerator"/> with the same interface as
    /// <see cref="Random"/>.
    /// </summary>
    public class SecureRandom : Random
    {
        #region Fields

        /// <summary>
        /// The random service provider.
        /// </summary>
        private readonly RandomNumberGenerator provider = RandomNumberGenerator.Create();

        /// <summary>
        /// The byte buffer of random values.
        /// </summary>
        private byte[] valuesBuffer;

        /// <summary>
        /// The buffer position.
        /// </summary>
        private int bufferPosition;

        #endregion Fields

        #region Constructor/Destructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SecureRandom"/> class.
        /// </summary>
        public SecureRandom() : this(true)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SecureRandom"/> class.
        /// </summary>
        /// <param name="ignoredSeed">The ignored seed.</param>
        public SecureRandom(int ignoredSeed) : this(true)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SecureRandom"/> class.
        /// </summary>
        /// <param name="enableRandomPool">if set to true enables a random pool.
        /// </param>
        public SecureRandom(bool enableRandomPool)
        {
            this.IsRandomPoolEnabled = enableRandomPool;
        }

        #endregion Constructor/Destructor

        #region Properties

        /// <summary>
        /// Gets a value indicating whether this instance is random pool
        /// enabled.
        /// </summary>
        /// <value>
        /// True if this instance is random pool enabled; otherwise, false.
        /// </value>
        public bool IsRandomPoolEnabled { get; }

        #endregion Properties

        #region Methods

        /// <inheritdoc />
        public override int Next() => (int)this.GetRandomUInt32() & 0x7FFFFFFF;

        /// <inheritdoc />
        public override int Next(int maxValue) => this.Next(0, maxValue);

        /// <inheritdoc />
        public override int Next(int minValue, int maxValue)
        {
            if (minValue > maxValue)
            {
                throw new ArgumentOutOfRangeException(nameof(minValue), Resources.ERR_MinNotGreaterThanMax);
            }

            if (maxValue < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(maxValue), Resources.ERR_MaxNotLessThanZero);
            }

            if (minValue == maxValue)
            {
                return minValue;
            }

            // Possibly need another check to see if min or max values are greater than 0.
            long diff = maxValue - minValue;

            while (true)
            {
                uint rand = this.GetRandomUInt32();

                long max = 1 + (long)uint.MaxValue;
                long remainder = max % diff;

                if (rand < max - remainder)
                {
                    return (int)(minValue + rand % diff);
                }
            }
        }

        /// <inheritdoc />
        public override double NextDouble() => this.GetRandomUInt32() / (1.0 + uint.MaxValue);

        /// <inheritdoc />
        public override void NextBytes(byte[] buffer)
        {
            ADP.CheckArgumentNull(buffer, nameof(buffer));

            lock (this)
            {
                if (this.IsRandomPoolEnabled && this.valuesBuffer == null)
                {
                    this.InitBuffer();
                }

                // Can we fit the requested number of bytes in the buffer?
                if (this.IsRandomPoolEnabled && this.valuesBuffer.Length >= buffer.Length)
                {
                    int count = buffer.Length;

                    this.EnsureRandomBuffer(count);
                    Buffer.BlockCopy(this.valuesBuffer, this.bufferPosition, buffer, 0, count);
                    this.bufferPosition += count;
                }
                else
                {
                    // Draw bytes directly from the provider.
                    this.provider.GetBytes(buffer);
                }
            }
        }

        #endregion Methods

        #region Helper Methods

        /// <summary>
        /// Initializes the buffer with the proper size and data.
        /// </summary>
        private void InitBuffer()
        {
            if (this.IsRandomPoolEnabled)
            {
                if (this.valuesBuffer == null || this.valuesBuffer.Length != 512)
                {
                    this.valuesBuffer = new byte[512];
                }
            }
            else
            {
                if (this.valuesBuffer == null || this.valuesBuffer.Length != 4)
                {
                    this.valuesBuffer = new byte[4];
                }
            }

            this.provider.GetBytes(this.valuesBuffer);
            this.bufferPosition = 0;
        }

        /// <summary>
        /// Gets one random unsigned 32-bit integer in a thread safe manner.
        /// </summary>
        /// <returns>A random unsigned int.</returns>
        private uint GetRandomUInt32()
        {
            lock (this)
            {
                this.EnsureRandomBuffer(4);

                uint rand = BitConverter.ToUInt32(this.valuesBuffer, this.bufferPosition);
                this.bufferPosition += 4;
                return rand;
            }
        }

        /// <summary>
        /// Ensures that we have enough bytes in the random buffer.
        /// </summary>
        /// <param name="requiredBytes">The number of required bytes.</param>
        private void EnsureRandomBuffer(int requiredBytes)
        {
            if (this.valuesBuffer == null)
            {
                this.InitBuffer();
            }

            if (requiredBytes > this.valuesBuffer.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(requiredBytes), Resources.ERR_NotGreaterThanBuffer);
            }

            if ((this.valuesBuffer.Length - this.bufferPosition) < requiredBytes)
            {
                this.InitBuffer();
            }
        }

        #endregion Helper Methods
    }
}