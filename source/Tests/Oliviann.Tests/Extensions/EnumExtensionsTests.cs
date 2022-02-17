namespace Oliviann.Tests.Extensions
{
    using System;
    using System.Windows.Forms;
    using Xunit;

    [Trait("Category", "CI")]
    public class EnumExtensionsTests
    {
        private enum TestDayOfWeek
        {
            [System.ComponentModel.Description("Sunday")]
            Sun,

            [System.ComponentModel.Description("Monday")]
            Mon,

            [System.ComponentModel.Description("Tuesday")]
            Tues,

            [System.ComponentModel.Description("Wednesday")]
            Wed,

            [System.ComponentModel.Description("Thursday")]
            Thu,

            [System.ComponentModel.Description("Friday")]
            Fri,

            [System.ComponentModel.Description("Saturday")]
            Sat
        }

        #region GetDescriptionAttribute Tests

        [Fact]
        public void Test1_GetDescriptionAttribute()
        {
            string result = TestDayOfWeek.Mon.GetDescriptionAttribute();
            Assert.Equal("Monday", result);
        }

        [Fact]
        public void Test2_GetDescriptionAttribute()
        {
            string result = DayOfWeek.Monday.GetDescriptionAttribute();
            Assert.Equal(string.Empty, result);
        }

        #endregion GetDescriptionAttribute Tests

        #region GetAttributeValue Tests

        [Fact]
        public void Test1_GetAttributeValue()
        {
            string result = TestDayOfWeek.Mon.GetAttributeValue<System.ComponentModel.DescriptionAttribute>("Description");
            Assert.Equal("Monday", result);
        }

        [Fact]
        public void Test2_GetAttributeValue()
        {
            string result = DayOfWeek.Monday.GetAttributeValue<System.ComponentModel.DescriptionAttribute>("Description");
            Assert.Equal(string.Empty, result);
        }

        #endregion GetAttributeValue Tests

        #region IsWithinShiftedRange Tests

        [Fact]
        public void IsWithinShiftedRangeTest_ZeroZeroZero()
        {
            bool result = TestDayOfWeek.Mon.IsWithinShiftedRange(0, 0, 0);
            Assert.False(result);
        }

        [Fact]
        public void IsWithinShiftedRangeTest_InValidRange()
        {
            bool result = MessageBoxIcon.Information.IsWithinShiftedRange(4, 0, 3);
            Assert.False(result);
        }

        [Theory]
        [InlineData(MessageBoxIcon.None, true)]
        [InlineData(MessageBoxIcon.Error, true)]
        [InlineData(MessageBoxIcon.Warning, true)]
        [InlineData(MessageBoxIcon.Information, true)]
        public void IsWithinShiftedRangeTest_ValidRange(MessageBoxIcon input, bool expectedResult)
        {
            bool result = input.IsWithinShiftedRange(4, 0, 4);
            Assert.Equal(expectedResult, result);
        }

        #endregion IsWithinShiftedRange Tests
    }
}