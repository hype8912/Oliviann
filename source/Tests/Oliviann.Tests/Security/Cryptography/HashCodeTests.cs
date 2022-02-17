namespace Oliviann.Tests.Security.Cryptography
{
    #region Usings

    using Oliviann.Security.Cryptography;
    using Xunit;

    #endregion Usings

    [Trait("Category", "CI")]
    public class HashCodeTests
    {
        /// <summary>
        /// Verifies a specific value returns the correct hash code.
        /// </summary>
        [Theory]
        [InlineData(null, 148298089)]
        [InlineData("", -796076882)]
        [InlineData("Taco Bell", 126819901)]
        [InlineData("The quick brown fox jumps over the lazy dog", -1828523062)]
        [InlineData("Oliviann$%^I23456789O", 1553780631)]
        [InlineData("QWer!@34tyuiop098765", 1316560664)]
        public void Combine1Test_ValuesNoSeed(string input, int expectedResult)
        {
            int result = HashCode.Combine(input);
            Assert.Equal(expectedResult, result);
        }

        /// <summary>
        /// Verifies a specific value returns the correct hash code.
        /// </summary>
        [Theory]
        [InlineData(null, 17U, 1659108282)]
        [InlineData("", 17U, 153186862)]
        [InlineData("Taco Bell", 17, -1990729692)]
        [InlineData("The quick brown fox jumps over the lazy dog", 17U, -89366755)]
        [InlineData("Oliviann$%^I23456789O", 17U, 1551783962)]
        [InlineData("QWer!@34tyuiop098765", 17U, -328765153)]
        public void Combine1Test_ValuesWithSeed(string input, uint seed, int expectedResult)
        {
            int result = HashCode.Combine(input, seed);
            Assert.Equal(expectedResult, result);
        }

        /// <summary>
        /// Verifies a specific value returns the correct hash code.
        /// </summary>
        [Theory]
        [InlineData(null, null, -558656237)]
        [InlineData("", null, 698078410)]
        [InlineData(null, "", 408419805)]
        [InlineData("", "", -1881998777)]
        [InlineData("Taco", "Bell", 1800309522)]
        [InlineData("The quick brown fox jumps", " over the lazy dog", -1366276220)]
        [InlineData("Oliviann$%^I", "23456789O", -1482864688)]
        [InlineData("QWer!@34ty", "uiop098765", -9204666)]
        public void Combine2Test_ValuesNoSeed(string input1, string input2, int expectedResult)
        {
            int result = HashCode.Combine(input1, input2);
            Assert.Equal(expectedResult, result);
        }

        /// <summary>
        /// Verifies a specific value returns the correct hash code.
        /// </summary>
        [Theory]
        [InlineData(null, null, -1912460486)]
        [InlineData("", null, -247945973)]
        [InlineData(null, "", 728924801)]
        [InlineData("", "", -397984866)]
        [InlineData("Taco", "Bell", 1566048021)]
        [InlineData("The quick brown fox jumps", " over the lazy dog", -750111648)]
        [InlineData("Oliviann$%^I", "23456789O", 1336941492)]
        [InlineData("QWer!@34ty", "uiop098765", 98710978)]
        public void Combine4Test_ValuesNoSeed(string input1, string input2, int expectedResult)
        {
            int result = HashCode.Combine(input1, input2, input1, input2);
            Assert.Equal(expectedResult, result);
        }
    }
}