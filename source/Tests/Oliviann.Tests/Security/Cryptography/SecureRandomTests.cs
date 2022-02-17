namespace Oliviann.Tests.Security.Cryptography
{
    #region Usings

    using System;
    using Oliviann.Security.Cryptography;
    using Xunit;

    #endregion Usings

    [Trait("Category", "CI")]
    public class SecureRandomTests
    {
        #region NextInt Tests

        /// <summary>
        /// Verifies a random integer is generated.
        /// </summary>
        [Fact]
        public void SecureRandomNext_OneValue()
        {
            var rand = new SecureRandom();
            int result = rand.Next();

            Assert.InRange(result, 0, int.MaxValue);
        }

        /// <summary>
        /// Verifies a value is within the range of the max value.
        /// </summary>
        [Fact]
        public void SecureRandomNextMax_OneValueWithMax()
        {
            var rand = new SecureRandom(99);
            int result = rand.Next(100);

            Assert.InRange(result, 0, 100);
        }

        /// <summary>
        /// Verifies an argument out of range exception is thrown when a max
        /// value is greater than a min value.
        /// </summary>
        [Fact]
        public void SecureRandomNextMinMax_MinGreaterThanMax()
        {
            var rand = new SecureRandom();
            Assert.Throws<ArgumentOutOfRangeException>(() => rand.Next(10, 1));
        }

        /// <summary>
        /// Verifies a max number less than zero throws an argument out of range
        /// exception.
        /// </summary>
        [Fact]
        public void SecureRandomNextMinMax_MaxLessThanZero()
        {
            var rand = new SecureRandom();
            Assert.Throws<ArgumentOutOfRangeException>(() => rand.Next(-99, -1));
        }

        [Theory]
        [InlineData(99, 99)]
        [InlineData(25, 1000)]
        [InlineData(short.MaxValue, 32767785)]
        [InlineData(-99, 200)]
        public void SecureRandomNextMinMax_Values(int min, int max)
        {
            var rand = new SecureRandom();
            int result = rand.Next(min, max);

            Assert.InRange(result, min, max);
        }

        #endregion NextInt Tests

        #region NextDouble Tests

        /// <summary>
        /// Verifies a random double value is created in range.
        /// </summary>
        [Fact]
        public void SecureRandomNextDouble_Values()
        {
            var rand = new SecureRandom();
            double result = rand.NextDouble();

            Assert.InRange(result, 0D, double.MaxValue);
        }

        #endregion NextDouble Tests

        #region NextBytes Tests

        /// <summary>
        /// Verifies a argument null exception is thrown.
        /// </summary>
        [Fact]
        public void SecureRandomNextBytes_NullBuffer()
        {
            byte[] data = null;
            var rand = new SecureRandom();

            Assert.Throws<ArgumentNullException>(() => rand.NextBytes(data));
        }

        /// <summary>
        /// Verifies bytes can be loaded from the buffer.
        /// </summary>
        [Fact]
        public void SecureRandomNextBytes_WithBuffer()
        {
            var data = new byte[128];
            var rand = new SecureRandom();
            rand.NextBytes(data);

            Assert.Equal(128, data.Length);
        }

        /// <summary>
        /// Verifies bytes can be loaded without the buffer.
        /// </summary>
        [Fact]
        public void SecureRandomNextBytes_NoBuffer()
        {
            var data = new byte[128];
            var rand = new SecureRandom(false);
            rand.NextBytes(data);

            Assert.Equal(128, data.Length);
        }

        #endregion NextBytes Tests

        #region Pressure Tests

        [Fact]
        public void SecureRandomNextMinMax_600IterationsWithBuffer()
        {
            var rand = new SecureRandom();

            for (int i = 0; i < 600; i += 1)
            {
                int result = rand.Next(250, 750);
                Assert.InRange(result, 250, 750);
            }
        }

        [Fact]
        public void SecureRandomNextMinMax_600IterationsNoBuffer()
        {
            var rand = new SecureRandom(false);

            for (int i = 0; i < 600; i += 1)
            {
                int result = rand.Next(250, 750);
                Assert.InRange(result, 250, 750);
            }
        }

        #endregion Pressure Tests
    }
}