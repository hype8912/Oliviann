namespace Oliviann.Tests
{
    #region Usings

    using System;
    using Xunit;

    #endregion Usings

    [Trait("Category", "CI")]
    public class HighResolutionDateTimeTests
    {
        [Fact]
        public void GetUtcNow()
        {
            DateTime expected = DateTime.UtcNow;
            DateTime result = HighResolutionDateTime.UtcNow;

            Assert.Equal(expected.Year, result.Year);
            Assert.Equal(expected.Month, result.Month);
            Assert.Equal(expected.Day, result.Day);
            Assert.Equal(expected.Hour, result.Hour);
            Assert.Equal(expected.Minute, result.Minute);
        }
    }
}