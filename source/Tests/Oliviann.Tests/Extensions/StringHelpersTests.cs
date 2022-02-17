namespace Oliviann.Tests.Extensions
{
    #region Usings

    using System.Diagnostics;
    using Xunit;

    #endregion Usings

    [Trait("Category", "CI")]
    public class StringHelpersTests
    {
        [Theory]
        [InlineData(null, null)]
        [InlineData(@"\\ietm-fwb-bl6\IMIS\Server", "ietm-fwb-bl6")]
        [InlineData("", null)]
        [InlineData("jklfdgnkjng324u5798413*^&(*&^6239412", null)]
        [InlineData(@"\\ietm-fwb-bl6", "ietm-fwb-bl6")]
        [InlineData(@"\\ietm-fwb-bl6\", "ietm-fwb-bl6")]
        public void ExtractMachineFromUncPathTest_Strings(string input, string expectedResult)
        {
            string result = StringHelpers.ExtractMachineFromUncPath(input);
            Assert.Equal(expectedResult, result);
        }

        /// <summary>
        /// Verifies the optional characters will be defaulted if set to null.
        /// Verifies the length is correct.
        /// </summary>
        [Fact]
        public void GenerateRandomStringTest_Null()
        {
            string result = StringHelpers.GenerateRandomString(10, null);
            Assert.Equal(10, result.Length);
        }

        /// <summary>
        /// Verifies a short and long string return as the correct length with
        /// no exceptions.
        /// </summary>
        [Theory]
        [InlineData(0, 0)]
        [InlineData(15, 15)]
        [InlineData(1000, 1000)]
        [InlineData(100000, 100000)]
        public void GenerateRandomStringTest_Length(int input, int expectedResult)
        {
            string result = StringHelpers.GenerateRandomString(input);
            Assert.Equal(expectedResult, result.Length);
        }

        /// <summary>
        /// Verifies a medium sized string with special characters returns will
        /// all the special characters.
        /// </summary>
        [Fact]
        public void GenerateRandomStringTest_SpecialChars()
        {
            string result = StringHelpers.GenerateRandomString(100, "ABC1@#");

            Debug.WriteLine("Result: " + result);
            Assert.Equal(100, result.Length);
            Assert.Contains("A", result);
            Assert.Contains("B", result);
            Assert.Contains("C", result);
            Assert.Contains("1", result);
            Assert.Contains("@", result);
            Assert.Contains("#", result);
        }
    }
}