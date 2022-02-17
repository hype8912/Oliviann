namespace Oliviann.Tests.Extensions
{
    #region Usings

    using Xunit;

    #endregion Usings

    [Trait("Category", "CI")]
    public class BooleanExtensionsTests
    {
        #region ToYesNoString Tests

        [Theory]
        [InlineData(true, "Yes")]
        [InlineData(false, "No")]
        public void ToYesNoStringTest_Values(bool input, string expectedResult)
        {
            string result = input.ToYesNoString();
            Assert.Equal(expectedResult, result);
        }

        #endregion ToYesNoString Tests

        #region ToBinaryTypeNumber Tests

        [Theory]
        [InlineData(true, 1)]
        [InlineData(false, 0)]
        public void ToBitTest_Values(bool input, int expectedResult)
        {
            int result = input.ToBit();
            Assert.Equal(expectedResult, result);
        }

        #endregion ToBinaryTypeNumber Tests
    }
}