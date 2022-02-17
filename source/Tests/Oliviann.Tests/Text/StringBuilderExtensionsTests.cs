namespace Oliviann.Tests.Text
{
    #region Usings

    using System;
    using System.Collections.Generic;
    using System.Text;
    using Oliviann.Text;
    using Xunit;

    #endregion Usings

    [Trait("Category", "CI")]
    public class StringBuilderExtensionsTests
    {
        #region AppendLine Tests

        [Theory]
        [InlineData(null)]
        [InlineData("World")]
        public void StringBuilder_AppendLineFormatTest_NullFormat(object arguments)
        {
            var builder = new StringBuilder("Hello");
            Assert.Throws<ArgumentNullException>(() => builder.AppendLineFormat(null, arguments: arguments));
        }

        [Fact]
        public void StringBuilder_AppendLineFormatTest_NullArgs()
        {
            var builder = new StringBuilder("Hello");
            Assert.Throws<ArgumentNullException>(() => builder.AppendLineFormat("{0}", null));
        }

        [Fact]
        public void StringBuilder_AppendLineFormatTest_Strings()
        {
            var builder = new StringBuilder();
            builder.AppendLineFormat("Hello {0}!", "World");

            Assert.Equal("Hello World!\r\n", builder.ToString());
        }

        #endregion AppendLine Tests

        #region IsNullOrEmpty Tests

        [Fact]
        public void StringBuilder_IsNullOrEmptyTest_NullInstance()
        {
            StringBuilder builder = null;
            bool result = builder.IsNullOrEmpty();

            Assert.True(result);
        }

        [Theory]
        [InlineData(null, true)]
        [InlineData("", true)]
        [InlineData("     ", false)]
        [InlineData("Hello tacos", false)]
        public void StringBuilder_IsNullOrEmptyTest_Strings(string input, bool expectedResult)
        {
            var builder = new StringBuilder(input);
            bool result = builder.IsNullOrEmpty();

            Assert.Equal(expectedResult, result);
        }

        #endregion IsNullOrEmpty Tests

        #region ReplaceAll Tests

        [Fact]
        public void StringBuilderReplaceAllTest_NullBuilder()
        {
            StringBuilder builder = null;
            var items = new Dictionary<string, string> { { "rl", "RL" } };

            StringBuilder result = builder.ReplaceAll(items);
            Assert.Null(result);
        }

        [Fact]
        public void StringBuilderReplaceAllTest_NullCollection()
        {
            var builder = new StringBuilder("Hello World");
            Dictionary<string, string> items = null;

            StringBuilder result = builder.ReplaceAll(items);
            Assert.Equal("Hello World", builder.ToString());
        }

        #endregion
    }
}