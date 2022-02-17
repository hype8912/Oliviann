namespace Oliviann.Tests.Web
{
    #region Usings

    using System;
    using Oliviann.Web;
    using Xunit;

    #endregion Usings

    [Trait("Category", "CI")]
    public class MimeMappingTests
    {
        /// <summary>
        /// Verifies passing in a null file extension throws an argument null
        /// exception.
        /// </summary>
        [Fact]
        public void GetMimeMappingTest_NullFileExtension()
        {
            Assert.Throws<ArgumentNullException>(() => MimeMapping.GetMimeMapping(null));
        }

        /// <summary>
        /// Verifies a collection mime type file extensions and files return the expected result.
        /// </summary>
        [Theory]
        [InlineData("", "application/octet-stream")]
        [InlineData(".json", "application/json")]
        [InlineData(".jSoN", "application/json")]
        [InlineData(".doc", "application/msword")]
        [InlineData(".xml", "text/xml")]
        [InlineData(".xML", "text/xml")]
        [InlineData("taco.json", "application/octet-stream")] // we did not implement file matching.
        [InlineData("taco.xml", "text/xml")]
        public void GetMimeMappingTest_Values(string input, string expectedResult)
        {
            string result = MimeMapping.GetMimeMapping(input);
            Assert.Equal(expectedResult, result);
        }
    }
}