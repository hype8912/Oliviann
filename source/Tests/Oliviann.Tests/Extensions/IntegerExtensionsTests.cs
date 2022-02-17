namespace Oliviann.Tests.Extensions
{
    #region Usings

    using Xunit;

    #endregion Usings

    [Trait("Category", "CI")]
    public class IntegerExtensionsTests
    {
        [Theory]
        [InlineData(0, 0)]
        [InlineData(25, 25)]
        [InlineData(65535, 65535)]
        [InlineData(-10, 80)]
        public void Integers_ValidPortOrDefaultTest(int input, int expectedResult)
        {
            int result = input.ValidPortOrDefault();
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData(77, 99, 77)]
        [InlineData(70000, 99, 99)]
        [InlineData(80000, -200, 0)]
        public void Integers_ValidPortOrDefaultTest_DefaultValue(int input, int defaultValue, int expectedResult)
        {
            int result = input.ValidPortOrDefault(defaultValue);
            Assert.Equal(expectedResult, result);
        }

        #region IsValidPort Tests

        /// <summary>
        /// Verifies a set of low value port numbers are invalid. NOTE: Running
        /// all the way from int.MinValue to 0 takes 4 minutes to complete so a
        /// short subset of numbers is used.
        /// </summary>
        [Fact]
        public void Integers_IsValidPortTest_LowValues()
        {
            for (int value = short.MinValue; value < 0; value += 1)
            {
                bool result = value.IsValidPort();
                Assert.False(result, "Invalid port number was found to be valid.\n\tValue[{0}]".FormatWith(value));
            }
        }

        /// <summary>
        /// Verifies a set of high value port numbers are invalid. NOTE: Running
        /// all the way to int.MaxValue takes 6 minutes so a short subset is
        /// used.
        /// </summary>
        [Fact]
        public void Integers_IsValidPortTest_HighValues()
        {
            const int StopValue = 65536 * 2;
            for (int value = 65536; value < StopValue; value += 1)
            {
                bool result = value.IsValidPort();
                Assert.False(result, "Invalid port number was found to be valid.\n\tValue[{0}]".FormatWith(value));
            }
        }

        /// <summary>
        /// Verifies all the valid port numbers are returned as valid numbers.
        /// </summary>
        [Fact]
        public void Integers_IsValidPort()
        {
            for (int value = 0; value <= 65535; value += 1)
            {
                bool result = value.IsValidPort();
                Assert.True(result, "Valid port number was found to be invalid.\n\tValue[{0}]".FormatWith(value));
            }
        }

        #endregion IsValidPort Tests

        [Theory]
        [InlineData(9, false)]
        [InlineData(10, true)]
        [InlineData(0, true)]
        [InlineData(-7, false)]
        [InlineData(-12, true)]
        public void Integers_IsEvenTest(int input, bool expectedResult)
        {
            bool result = input.IsEven();
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData(9, true)]
        [InlineData(10, false)]
        [InlineData(0, false)]
        [InlineData(-7, true)]
        [InlineData(-12, false)]
        public void Integers_IsOddTest(int input, bool expectedResult)
        {
            bool result = input.IsOdd();
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData(9, 8, true)]
        [InlineData(10, 0, true)]
        [InlineData(0, 5, false)]
        [InlineData(-7, -12, true)]
        [InlineData(-12, -7, false)]
        public void Integers_IsGreaterThanTest(int input, int value, bool expectedResult)
        {
            bool result = input.IsGreaterThan(value);
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData(9, 8, false)]
        [InlineData(10, 0, false)]
        [InlineData(0, 5, true)]
        [InlineData(-7, -12, false)]
        [InlineData(-12, -7, true)]
        public void Integers_IsLessThanTest(int input, int value, bool expectedResult)
        {
            bool result = input.IsLessThan(value);
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData(9, 8, true)]
        [InlineData(10, 0, true)]
        [InlineData(0, 5, false)]
        [InlineData(-7, -12, true)]
        [InlineData(-12, -7, false)]
        [InlineData(9, 9, true)]
        [InlineData(-6, 21, false)]
        public void Integers_IsGreaterThanOrEqualTest(int input, int value, bool expectedResult)
        {
            bool result = input.IsGreaterThanOrEqual(value);
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData(9, 8, false)]
        [InlineData(10, 0, false)]
        [InlineData(0, 5, true)]
        [InlineData(-7, -12, false)]
        [InlineData(-12, -7, true)]
        [InlineData(9, 9, true)]
        [InlineData(-6, 21, true)]
        public void Integers_IsLessThanOrEqualTest(int input, int value, bool expectedResult)
        {
            bool result = input.IsLessThanOrEqual(value);
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData(9, 5, 10, true)]
        [InlineData(10, 8, 13, true)]
        [InlineData(0, 4, 6, false)]
        [InlineData(-7, -12, -6, true)]
        [InlineData(-12, -6, 0, false)]
        [InlineData(9, 0, 9, false)]
        [InlineData(-6, -6, 6, false)]
        public void Integers_IsBetweenTest(int input, int lowValue, int upValue, bool expectedResult)
        {
            bool result = input.IsBetween(lowValue, upValue);
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData(9, 5, 10, true)]
        [InlineData(10, 8, 13, true)]
        [InlineData(0, 4, 6, false)]
        [InlineData(-7, -12, -6, true)]
        [InlineData(-12, -6, 0, false)]
        [InlineData(9, 0, 9, true)]
        [InlineData(-6, -6, 6, true)]
        public void Integers_IsBetweenOrEqualTest(int input, int lowValue, int upValue, bool expectedResult)
        {
            bool result = input.IsBetweenOrEqual(lowValue, upValue);
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData(6, 6)]
        [InlineData(0, 0)]
        [InlineData(-21, 0)]
        [InlineData(255, 255)]
        [InlineData(2147483647, 0)]
        [InlineData(-2147483648, 0)]
        public void IntToByteTest(int input, byte expectedResult)
        {
            byte result = input.ToByte();
            Assert.Equal(expectedResult, result);
        }
    }
}