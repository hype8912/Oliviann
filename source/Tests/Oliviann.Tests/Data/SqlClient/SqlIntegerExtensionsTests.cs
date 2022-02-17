namespace Oliviann.Tests.Data.SqlClient
{
    #region Usings

    using System.Collections.Generic;
    using System.Data.SqlTypes;
    using Oliviann.Data.SqlClient;
    using Xunit;

    #endregion Usings

    [Trait("Category", "CI")]
    public class SqlIntegerExtensionsTests
    {
        #region Int16

        /// <summary>
        /// Tests that the method returns a null when a null value is passed in.
        /// </summary>
        [Fact]
        public void ToNullableInt16_Null()
        {
            var value = new SqlInt16();
            Assert.True(value.IsNull);

            short? result = value.ToNullableInt16();
            Assert.False(result.HasValue, "Result value was not null.");
        }

        /// <summary>
        /// Tests that a collection of 16-bit integers to see if they returned
        /// correctly.
        /// </summary>
        [Theory]
        [InlineData(-20)]
        [InlineData(9999)]
        [InlineData(0)]
        [InlineData(short.MinValue)]
        [InlineData(short.MaxValue)]
        public void ToNullableInt16_Values(short input)
        {
            var sqlValue = new SqlInt16(input);
            short? result = sqlValue.ToNullableInt16();

            Assert.True(result.HasValue, "Result value was not null.");
            Assert.Equal(input, result.Value);
        }

        #endregion Int16

        #region Int32

        /// <summary>
        /// Tests that the method returns a null when a null value is passed in.
        /// </summary>
        [Fact]
        public void ToNullableInt32_Null()
        {
            var value = new SqlInt32();
            Assert.True(value.IsNull);

            int? result = value.ToNullableInt32();
            Assert.False(result.HasValue, "Result value was not null.");
        }

        /// <summary>
        /// Tests that a collection of 32-bit integers to see if they returned
        /// correctly.
        /// </summary>
        [Theory]
        [InlineData(-20)]
        [InlineData(999999)]
        [InlineData(0)]
        [InlineData(short.MinValue)]
        [InlineData(short.MaxValue)]
        [InlineData(int.MinValue)]
        [InlineData(int.MaxValue)]
        public void ToNullableInt32_Values(int input)
        {
            var sqlValue = new SqlInt32(input);
            int? result = sqlValue.ToNullableInt32();

            Assert.True(result.HasValue, "Result value was not null.");
            Assert.Equal(input, result.Value);
        }

        #endregion Int32

        #region Int64

        /// <summary>
        /// Tests that the method returns a null when a null value is passed in.
        /// </summary>
        [Fact]
        public void ToNullableInt64_Null()
        {
            var value = new SqlInt64();
            Assert.True(value.IsNull);

            long? result = value.ToNullableInt64();
            Assert.False(result.HasValue, "Result value was not null.");
        }

        private List<long> ConvertToInt64TestValues = new List<long>
        {
            -20,
            999999,
            0,
            short.MinValue,
            short.MaxValue,
            int.MinValue,
            int.MaxValue,
            long.MinValue,
            long.MaxValue
        };

        /// <summary>
        /// Tests that a collection of 64-bit integers to see if they returned
        /// correctly.
        /// </summary>
        [Theory]
        [InlineData(-20)]
        [InlineData(999999)]
        [InlineData(0)]
        [InlineData(short.MinValue)]
        [InlineData(short.MaxValue)]
        [InlineData(int.MinValue)]
        [InlineData(int.MaxValue)]
        [InlineData(long.MinValue)]
        [InlineData(long.MaxValue)]
        public void ToNullableInt64_Values(long input)
        {
            var sqlValue = new SqlInt64(input);
            long? result = sqlValue.ToNullableInt64();

            Assert.True(result.HasValue, "Result value was not null.");
            Assert.Equal(input, result.Value);
        }

        #endregion Int64
    }
}