#if NETFRAMEWORK

namespace Oliviann.Web.Tests
{
    #region Usings

    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Threading;
    using System.Web;
    using Moq;
    using Xunit;

    #endregion Usings

    [Trait("Category", "CI")]
    public class PageLoadSpeedModuleTests
    {
        #region GetSettingsMultiplier Tests

        /// <summary>
        /// Verifies the output is in the format set.
        /// </summary>
        [Theory]
        [InlineData("milliseconds", 1D)]
        [InlineData("seconds", 0.001)]
        public void GetSettingsMultiplier_Seconds_Milliseconds(string input, double expectedMultiplier)
        {
            KeyValuePair<double, string> result = PageLoadSpeedModule.GetSettingsMultiplier(input);

            Assert.Equal(result.Key, expectedMultiplier);
            Assert.Equal(input, result.Value);
        }

        /// <summary>
        /// Verifies the output is in milliseconds for parameters.
        /// </summary>
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("Yo! Let's go to Taco Bell.")]
        public void GetSettingsMultiplier_BasicStrings(string input)
        {
            KeyValuePair<double, string> result = PageLoadSpeedModule.GetSettingsMultiplier(input);

            Assert.Equal(1D, result.Key);
            Assert.Equal("milliseconds", result.Value);
        }

        #endregion GetSettingsMultiplier Tests

        #region BuildOutputString Tests

        /// <summary>
        /// Verifies the result string starts and ends with the correct text.
        /// </summary>
        [Theory]
        [InlineData(1D, "milliseconds", " milliseconds. -->", false)]
        [InlineData(0.001, "seconds", " seconds. -->", true)]
        public void BuildOutputStringTest_Seconds(double inputTime, string setting, string expectedEndsWith, bool expectedRound)
        {
            Stopwatch sw = Stopwatch.StartNew();
            var settings = new KeyValuePair<double, string>(inputTime, setting);
            Thread.Sleep(300);
            sw.Stop();

            string result = PageLoadSpeedModule.BuildOutputString(settings, sw);
            Trace.WriteLine("Result: " + result);

            Assert.NotNull(result);
            Assert.StartsWith("<!-- Dynamic page generated in ", result);
            Assert.EndsWith(expectedEndsWith, result);
            Assert.True(result.Contains("0.") == expectedRound, "Result string does not contain seconds calculation text.");
        }

        /// <summary>
        /// Verifies the result string only contains 3 decimal points.
        /// </summary>
        [Fact]
        public void BuildOutputStringTest_SecondsRound()
        {
            Stopwatch sw = Stopwatch.StartNew();
            var settings = new KeyValuePair<double, string>(0.001, "seconds");
            Thread.Sleep(2356);
            sw.Stop();

            string result = PageLoadSpeedModule.BuildOutputString(settings, sw);
            Trace.WriteLine("Result: " + result);

            Assert.NotNull(result);
            Assert.StartsWith("<!-- Dynamic page generated in ", result);
            Assert.EndsWith(" seconds. -->", result);

            int indexStart = result.IndexOf("2.");
            string substringText = result.Substring(indexStart, 5);
            Assert.True(substringText.Length == 5, "Result string does not contain seconds calculation text.");
        }

        #endregion BuildOutputString Tests

        #region Content Type Tests

        /// <summary>
        /// Verifies that the 2 methods the page load module overrides writes
        /// the correct result to the page response.
        /// </summary>
        [Theory]
        [InlineData("text/html")]
        public void PageLoadSpeedModuleTest_GoodContentType(string contentType)
        {
            string result = null;

            var responseMoc = new Mock<HttpResponseBase>();
            responseMoc.Setup(r => r.Write(It.IsAny<string>())).Callback<string>(r => result = r);
            responseMoc.SetupGet(r => r.ContentType).Returns(contentType);

            var contextMoc = new Mock<HttpContextBase>();
            contextMoc.SetupGet(c => c.Response).Returns(responseMoc.Object);

            BaseHttpModule module = new PageLoadSpeedModule();
            HttpContextBase context = contextMoc.Object;
            module.OnPreRequestHandlerExecute(context, EventArgs.Empty);
            module.OnPostReleaseRequestState(context, EventArgs.Empty);

            Assert.NotNull(result);
            Assert.StartsWith("<!-- Dynamic page generated in ", result);
            Assert.True(result.EndsWith(" milliseconds. -->"), "Result string doesn't end with correct text.");
        }

        /// <summary>
        /// Verifies a specific content type is not written to.
        /// </summary>
        [Theory]
        [InlineData(@"application/json")]
        [InlineData(@"image/jpeg")]
        [InlineData(@"text/css")]
        public void PageLoadSpeedModuleTest_BadContentType(string contentType)
        {
            string result = null;

            var responseMoc = new Mock<HttpResponseBase>();
            responseMoc.Setup(r => r.Write(It.IsAny<string>())).Callback<string>(r => result = r);
            responseMoc.SetupGet(r => r.ContentType).Returns(contentType);

            var contextMoc = new Mock<HttpContextBase>();
            contextMoc.SetupGet(c => c.Response).Returns(responseMoc.Object);

            BaseHttpModule module = new PageLoadSpeedModule();
            HttpContextBase context = contextMoc.Object;
            module.OnPreRequestHandlerExecute(context, EventArgs.Empty);
            module.OnPostReleaseRequestState(context, EventArgs.Empty);

            Assert.Null(result);
        }

        #endregion Content Type Tests
    }
}

#endif