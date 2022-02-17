#if NETFRAMEWORK

namespace Oliviann.Web.Tests
{
    #region Usings

    using System;
    using System.Web;
#if NET40
    using Oliviann.Extensions.Logging;
#else
    using Microsoft.Extensions.Logging;
#endif
    using Xunit;

    #endregion Usings

    [Trait("Category", "CI")]
    public class TraceContextLoggerTests
    {
        /// <summary>
        /// Verifies that no call throws an exception if a null context is
        /// passed in.
        /// </summary>
        [Fact]
        public void TraceContextLoggerTest_NullContext()
        {
            TraceContext context = null;
            var logger = new TraceContextLogger(context);

            Assert.True(logger.TraceMessagesToPage);
            Assert.Null(logger.BeginScope(string.Empty));
            Assert.False(logger.IsEnabled(LogLevel.Warning));

            logger.Log(LogLevel.Critical, new EventId(0, "Category"), string.Empty, new Exception("Something wrong"), null);
        }

        /// <summary>
        /// Verifies what you set the value to is what you get back.
        /// </summary>
        [Fact]
        public void TraceContextLoggerTest_SetTraceToPage()
        {
            TraceContext context = null;
            var logger = new TraceContextLogger(context);

            Assert.True(logger.TraceMessagesToPage);
            logger.TraceMessagesToPage = false;
            Assert.False(logger.TraceMessagesToPage);
            logger.TraceMessagesToPage = true;
            Assert.True(logger.TraceMessagesToPage);
        }
    }
}

#endif