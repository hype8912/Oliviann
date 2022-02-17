namespace Oliviann.Tests.Extensions
{
    #region Usings

    using System;
    using System.Collections.Generic;
    using Oliviann;
    using Xunit;

    #endregion Usings

    [Trait("Category", "CI")]
    public class DoubleExtensionsTests
    {
        #region ConvertJavaTimeStampToDateTime Tests

        public static IEnumerable<object[]> JavaTimeStampTestValues
            =>
                new[]
                {
                    new object[] { 0D, new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    new object[] { 1525723309667D, new DateTime(2018, 5, 7, 20, 01, 49, 667, DateTimeKind.Utc) },
                };

        /// <summary>
        /// Verifies the JAVA time is converted correctly.
        /// </summary>
        [Theory]
        [MemberData(nameof(JavaTimeStampTestValues))]
        public void ConvertJavaTimeStampToDateTimeTests(double input, DateTime expectedResult)
        {
            DateTime result = input.ConvertJavaTimeStampToDateTime();
            Assert.Equal(expectedResult, result);
        }

        #endregion ConvertJavaTimeStampToDateTime Tests

        #region ConvertUnixTimeStampToDateTime Tests

        public static IEnumerable<object[]> UnixTimeStampTestValues
            =>
            new[]
            {
                new object[] { 0D, new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                new object[] { 1525721131D, new DateTime(2018, 5, 7, 19, 25, 31, 0, DateTimeKind.Utc) },
            };

        /// <summary>
        /// Verifies the UNIX time is converted correctly.
        /// </summary>
        [Theory]
        [MemberData(nameof(UnixTimeStampTestValues))]
        public void ConvertUnixTimeStampToDateTimeTests(double input, DateTime expectedResult)
        {
            DateTime result = input.ConvertUnixTimeStampToDateTime();
            Assert.Equal(expectedResult, result);
        }

        #endregion ConvertUnixTimeStampToDateTime Tests

        [Theory]
        [InlineData(0.0D, true)]
        [InlineData(19.0D, true)]
        [InlineData(-21.5D, false)]
        [InlineData(-14.00000000000000D, true)]
        [InlineData(-56D, true)]
        [InlineData(17D, true)]
        [InlineData(9999.999D, false)]
        public void IsWholeNumberTests(double input, bool expectedResult)
        {
            bool result = input.IsWholeNumber();
            Assert.Equal(expectedResult, result);
        }
    }
}