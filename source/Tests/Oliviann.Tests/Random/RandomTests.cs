namespace Oliviann.Tests.Random
{
    #region Usings

    using System;
    using System.Diagnostics;
    using Xunit;

    #endregion Usings

    [Trait("Category", "CI")]
    public class RandomTests
    {
        [Theory]
        [InlineData(0, 15)]
        [InlineData(-15, 15)]
        [InlineData(-15, 0)]
        public void RandomNextTest_Values(int lowerValue, int upperValue)
        {
            int result = new Random().Next(lowerValue, upperValue);

            Trace.WriteLine("Random value: " + result);
            Assert.InRange(result, lowerValue, upperValue);
        }
    }
}