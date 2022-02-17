#if NETFRAMEWORK

namespace Oliviann.Web
{
    #region Usings

    using System;
    using System.Diagnostics;
    using System.Web;
#if NET40
    using Oliviann.Extensions.Logging;
#else
    using Microsoft.Extensions.Logging;
#endif

    #endregion

    /// <summary>
    /// Represents a trace context logger.
    /// </summary>
    public class TraceContextLogger : ILogger
    {
        #region Fields

        /// <summary>
        /// The current trace context.
        /// </summary>
        private readonly TraceContext currentContext;

        #endregion

        #region Constructor/Destructor

        /// <summary>
        /// Initializes a new instance of the <see cref="TraceContextLogger" />
        /// class.
        /// </summary>
        /// <param name="context">The tract context instance.</param>
        public TraceContextLogger(TraceContext context)
        {
            this.currentContext = context;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether to trace messages to the
        /// page. Default value is true.
        /// </summary>
        /// <value>
        /// True if to trace messages to the page; otherwise, false.
        /// </value>
        public bool TraceMessagesToPage { get; set; } = true;

        #endregion

        #region Methods

        /// <summary>
        /// Begins a logical operation scope. Not used.
        /// </summary>
        /// <typeparam name="TState">The type of state.</typeparam>
        /// <param name="state">The identifier for the scope.</param>
        /// <returns>
        /// An IDisposable that ends the logical operation scope on dispose.
        /// </returns>
        public IDisposable BeginScope<TState>(TState state) => null;

        /// <inheritdoc />
        public bool IsEnabled(LogLevel logLevel) => this.currentContext?.IsEnabled ?? false;

        /// <inheritdoc />
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (logLevel == LogLevel.None || exception == null)
            {
                return;
            }

            if (logLevel == LogLevel.Warning || logLevel == LogLevel.Error || logLevel == LogLevel.Critical)
            {
                Trace.TraceWarning(exception.Message);
            }
            else
            {
                Trace.WriteLine(exception.Message, eventId.Name);
            }

            this.TraceLogToPage(logLevel, eventId.Name, exception.Message);
        }

        /// <summary>
        /// Traces the log to the page.
        /// </summary>
        /// <param name="logLevel">Entry will be written on this level.</param>
        /// <param name="category">The trace category that receives the message.
        /// Optional.</param>
        /// <param name="message">The trace message to write to the log.</param>
        private void TraceLogToPage(LogLevel logLevel, string category, string message)
        {
            if (!this.TraceMessagesToPage || this.currentContext == null)
            {
                return;
            }

            if (logLevel == LogLevel.Warning || logLevel == LogLevel.Error || logLevel == LogLevel.Critical)
            {
                this.currentContext.Warn(category, message);
            }
            else
            {
                this.currentContext.Write(category, message);
            }
        }

        #endregion
    }
}

#endif