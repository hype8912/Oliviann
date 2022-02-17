#if NETFRAMEWORK

namespace Oliviann.Web.Security
{
    #region Usings

    using System;

#if NET40
    using Oliviann.Extensions.Logging;
#else
    using Microsoft.Extensions.Logging;
#endif

    #endregion Usings

    /// <summary>
    /// Represents a base class for authentication in ASP.NET Web Forms.
    /// </summary>
    public abstract class AuthenticationBase
    {
        #region Fields

        /// <summary>
        /// String value for the return URL session variable.
        /// </summary>
        protected const string ReturnUrlSessionVariable = @"RedirectReturnUrl";

        /// <summary>
        /// The current logger instance.
        /// </summary>
        private readonly ILogger _logger;

        #endregion Fields

        #region Constructor/Destructor

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthenticationBase" />
        /// class.
        /// </summary>
        /// <param name="logger">The logger instance.</param>
        protected AuthenticationBase(ILogger logger)
        {
            this._logger = logger;
        }

        #endregion Constructor/Destructor

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether a unauthenticated return url
        /// that was requested is allowed to be redirected to after successful
        /// authentication.
        /// </summary>
        /// <value>
        /// True if return URL is allowed to be redirected to; otherwise, false.
        /// </value>
        public bool AllowReturnUrl { get; set; }

        #endregion Properties

        /// <summary>
        /// Entry point for performing authentication.
        /// </summary>
        /// <param name="formsAuthenticationDelegate">The delegate method for
        /// passing the login id to for local validation.</param>
        public abstract void Authenticate(Func<string, bool> formsAuthenticationDelegate);

        /// <summary>
        /// Gets the base URL from the entire URL.
        /// </summary>
        /// <param name="url">The entire URL string.</param>
        /// <returns>The base URL up to the first forward slash.</returns>
        protected static string GetBaseUrl(string url) => url.Substring(0, url.IndexOf('/', 8));

        /// <summary>
        /// Safely writes the information to the diagnostics and the page trace
        /// object if not null.
        /// </summary>
        /// <param name="message">The trace message to write to the log.</param>
        /// <param name="category">The trace category that receives the message.
        /// Optional.
        /// </param>
        protected void TraceMessage(string message, string category = "")
        {
            this._logger?.Log<string>(LogLevel.Information, new EventId(0, category), null, new Exception(message), null);
        }

        /// <summary>
        /// Safely writes the warning information to the diagnostics and the
        /// page trace object if not null.
        /// </summary>
        /// <param name="message">The trace message to write to the log.</param>
        /// <param name="category">The trace category that receives the message.
        /// Optional.
        /// </param>
        protected void TraceWarning(string message, string category = "")
        {
            this._logger?.Log<string>(LogLevel.Warning, new EventId(0, category), null, new Exception(message), null);
        }
    }
}

#endif