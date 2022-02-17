#if NETFRAMEWORK

namespace Oliviann.Tests.Web.Security
{
    #region Usings

    using System;
    using Oliviann.Tests.TestObjects;
    using Microsoft.Extensions.Logging;
    using Moq;
    using Xunit;

    #endregion Usings

    [Trait("Category", "CI")]
    public class AuthenticationBaseTests
    {
        #region GetBaseUrl Tests

        [Theory]
        [InlineData(null, typeof(NullReferenceException))]
        [InlineData("", typeof(ArgumentOutOfRangeException))]
        public void GetBaseUrlTest_Exceptions(string input, Type expectedException)
        {
            var wrapper = new AuthenticationBaseWrapper();
            Assert.Throws(expectedException, () => wrapper.GetBaseUrlCaller(input));
        }

        [Theory]
        [InlineData("https://store.google.com/us/product/nest_audio?", "https://store.google.com")]
        [InlineData("https://store.google.com/us/product/nest_audio?", "https://store.google.com")]
        public void GetBaseUrlTest_Strings(string input, string expectedResult)
        {
            var wrapper = new AuthenticationBaseWrapper();
            string result = wrapper.GetBaseUrlCaller(input);
            Assert.Equal(expectedResult, result);
        }

        #endregion GetBaseUrl Tests

        #region AllowReturnUrl Tests

        /// <summary>
        /// Verifies setting and getting the value is truly what we set it.
        /// </summary>
        [Fact]
        public void AllowReturnUrlTest_SetGet()
        {
            var authBase = new AuthenticationBaseWrapper();
            Assert.False(authBase.AllowReturnUrl, "Allow return url default value is true.");

            authBase.AllowReturnUrl = true;
            Assert.True(authBase.AllowReturnUrl, "Allow return url is set to false.");
        }

        #endregion AllowReturnUrl Tests

        #region Trace Tests

        [Fact]
        public void TraceMessageTest_NullLogger()
        {
            string logText = "This is a message.";

            var authBase = new AuthenticationBaseWrapper((ILogger)null);
            authBase.TraceBaseMessage(logText);
        }

        [Fact]
        public void TraceMessageTest_ValidNoCategory()
        {
            string logText = "This is a message.";
            var mocLogger = new Mock<ILogger>();
            mocLogger.Setup(
                l => l.Log(
                    It.Is<LogLevel>(level => level == LogLevel.Information),
                    It.Is<EventId>(e => e.Id == 0 && e.Name == string.Empty),
                    It.IsAny<string>(),
                    It.Is<Exception>(ex => ex.Message == logText),
                    It.IsAny<Func<string, Exception, string>>()));

            var authBase = new AuthenticationBaseWrapper(mocLogger.Object);
            authBase.TraceBaseMessage(logText);
        }

        [Fact]
        public void TraceMessageTest_ValidWitCategory()
        {
            string logText = "This is a message.";
            string cat = "Category";
            var mocLogger = new Mock<ILogger>();
            mocLogger.Setup(
                l => l.Log(
                    It.Is<LogLevel>(level => level == LogLevel.Information),
                    It.Is<EventId>(e => e.Id == 0 && e.Name == cat),
                    It.IsAny<string>(),
                    It.Is<Exception>(ex => ex.Message == logText),
                    It.IsAny<Func<string, Exception, string>>()));

            var authBase = new AuthenticationBaseWrapper(mocLogger.Object);
            authBase.TraceBaseMessage(logText, cat);
        }

        [Fact]
        public void TraceWarningTest_NullLogger()
        {
            string logText = "This is a message.";

            var authBase = new AuthenticationBaseWrapper((ILogger)null);
            authBase.TraceBaseWarning(logText);
        }

        [Fact]
        public void TraceWarningTest_ValidNoCategory()
        {
            string logText = "This is a message.";
            var mocLogger = new Mock<ILogger>();
            mocLogger.Setup(
                l => l.Log(
                    It.Is<LogLevel>(level => level == LogLevel.Warning),
                    It.Is<EventId>(e => e.Id == 0 && e.Name == string.Empty),
                    It.IsAny<string>(),
                    It.Is<Exception>(ex => ex.Message == logText),
                    It.IsAny<Func<string, Exception, string>>()));

            var authBase = new AuthenticationBaseWrapper(mocLogger.Object);
            authBase.TraceBaseWarning(logText);
        }

        [Fact]
        public void TraceWarningTest_ValidWitCategory()
        {
            string logText = "This is a message.";
            string cat = "Category";
            var mocLogger = new Mock<ILogger>();
            mocLogger.Setup(
                l => l.Log(
                    It.Is<LogLevel>(level => level == LogLevel.Warning),
                    It.Is<EventId>(e => e.Id == 0 && e.Name == cat),
                    It.IsAny<string>(),
                    It.Is<Exception>(ex => ex.Message == logText),
                    It.IsAny<Func<string, Exception, string>>()));

            var authBase = new AuthenticationBaseWrapper(mocLogger.Object);
            authBase.TraceBaseWarning(logText, cat);
        }

        #endregion Trace Tests
    }
}

#endif