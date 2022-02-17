namespace Oliviann.Tests.Extensions
{
    #region Usings

    using System;
    using System.Collections.Specialized;
    using Xunit;

    #endregion Usings

    [Trait("Category", "CI")]
    public class UriExtensionsTests
    {
        #region AddQuery Tests

        /// <summary>
        /// Verifies a null URI returns a null result.
        /// </summary>
        [Fact]
        public void UriAddQueryTest_NullUrl()
        {
            Uri tempUri = null;
            var parms = new NameValueCollection { { "role", "6" }, { "user", "1545853" }, { "limit", "1000" } };
            Uri result = tempUri.AddQuery(parms);

            Assert.Null(result);
        }

        /// <summary>
        /// Verifies a null parameter collection returns the base URI object
        /// back.
        /// </summary>
        [Fact]
        public void UriAddQueryTest_NullParameters()
        {
            var tempUri = new Uri("http://i.web.oliviann.com");
            Uri result = tempUri.AddQuery(null);

            Assert.Equal(tempUri, result);
        }

        /// <summary>
        /// Verifies an empty parameter collection returns the base URI object
        /// back.
        /// </summary>
        [Fact]
        public void UriAddQueryTest_EmptyParameters()
        {
            var tempUri = new Uri("http://i.web.oliviann.com");
            Uri result = tempUri.AddQuery(new NameValueCollection());

            Assert.Equal(tempUri, result);
        }

        /// <summary>
        /// Verifies parameters are added to a URI correctly.
        /// </summary>
        [Fact]
        public void UriAddQuery_AddParameters()
        {
            var tempUri = new Uri("http://i.web.oliviann.com");
            var parms = new NameValueCollection { { "role", "6" }, { "user", "1545853" }, { "limit", "1000" } };
            Uri result = tempUri.AddQuery(parms);

            string resultStr = result.ToString();
            Assert.Contains("http://i.web.oliviann.com/?", resultStr);

            foreach (string key in parms.AllKeys)
            {
                string pair = key + "=" + parms[key];
                Assert.Contains(pair, resultStr);
            }
        }

        #endregion AddQuery Tests

        #region Combine Tests

        /// <summary>
        /// Verifies a null URI returns a null result.
        /// </summary>
        [Fact]
        public void UriCombineTest_NullUri()
        {
            Uri tempUri = null;
            Uri result = tempUri.Combine(null);

            Assert.Null(result);
        }

        [Theory]
        [InlineData("http://www.oliviann.com", null, "http://www.oliviann.com/")]
        [InlineData("http://www.oliviann.com", "", "http://www.oliviann.com/")]
        [InlineData("http://www.oliviann.com:12025", "", "http://www.oliviann.com:12025/")]
        [InlineData("http://www.oliviann.com", "hello", "http://www.oliviann.com/hello")]
        [InlineData("http://www.oliviann.com/", "hello", "http://www.oliviann.com/hello")]
        [InlineData("http://www.oliviann.com/", "hello/world/c", "http://www.oliviann.com/hello/world/c")]
        [InlineData("http://www.oliviann.com/", "hello/world/file.txt", "http://www.oliviann.com/hello/world/file.txt")]
        [InlineData("file://c:/windows", "", "file:///c:/windows")]
        [InlineData("file://c:/windows/", "", "file:///c:/windows/")]
        [InlineData("file://c:/windows/", "hello.txt", "file:///c:/windows/hello.txt")]
        public void UriCombineTest_Strings(string input1, string input2, string expectedResult)
        {
            var tempUri = new Uri(input1);
            string result = tempUri.Combine(input2).ToString();

            Assert.Equal(expectedResult, result);
        }

        #endregion Combine Tests
    }
}