namespace Oliviann.Tests.Extensions
{
    #region Usings

    using System;
    using Xunit;

    #endregion Usings

    /// <summary>
    /// Represents a collection of Unit Tests for testing the String Extensions
    /// static class.
    /// </summary>
    [Trait("Category", "CI")]
    public class DateTimeExtensionsTests
    {
        #region Fields

        private static readonly DateTime TestDate1 = new DateTime(2009, 02, 27, 12, 11, 22);

        #endregion Fields

        [Fact]
        public void Test_ISO8601String()
        {
            string result = TestDate1.ToISO8601String();
            Assert.Equal("2009-02-27T12:11:22", result);
        }

        [Fact]
        public void Test_LongDateLongTimeString()
        {
            string result = TestDate1.ToLongDateLongTimeString();
            Assert.True(result == "Fri, February 27, 2009 12:11:22 PM" || result == "Friday, February 27, 2009 12:11:22 PM");
        }

        [Fact]
        public void Test_LongDateShortTimeString()
        {
            string result = TestDate1.ToLongDateShortTimeString();
            Assert.True(result == "Fri, February 27, 2009 12:11 PM" || result == "Friday, February 27, 2009 12:11 PM");
        }

        [Fact]
        public void Test_MonthDayString()
        {
            string result = TestDate1.ToMonthDayString();
            Assert.Equal("February 27", result);
        }

        [Fact]
        public void Test_MonthYearString()
        {
            string result = TestDate1.ToMonthYearString();
            Assert.StartsWith("February", result);
            Assert.EndsWith("2009", result);
        }

        [Fact]
        public void Test_RFC1123String()
        {
            string result = TestDate1.ToRFC1123String();
            Assert.Equal("Fri, 27 Feb 2009 12:11:22 GMT", result);
        }

        [Fact]
        public void Test_RoundTripString()
        {
            string result = TestDate1.ToRoundTripString();

            Assert.Equal("2009-02-27T12:11:22.0000000", result);
        }

        [Fact]
        public void Test_UniversalDateTimeLongString()
        {
            // NOTE: We can't test the hours and PM or AM because of time zone
            // differences.
            string result = TestDate1.ToUniversalDateTimeLongString();
            string shouldBeValue1 = "Fri, February 27, 2009 ";
            string shouldBeValue2 = "Friday, February 27, 2009 ";

            // Test the starting date.
            Assert.True(result.StartsWith(shouldBeValue1) || result.StartsWith(shouldBeValue2));

            // Test the minutes and seconds.
            Assert.Contains(":11:22 ", result);
        }

        [Fact]
        public void Test_UniversalDateTimeShortString()
        {
            string result = TestDate1.ToUniversalDateTimeShortString();
            string shouldBeValue = "2009-02-27 12:11:22Z";

            Assert.Equal(shouldBeValue, result);
        }
    }
}