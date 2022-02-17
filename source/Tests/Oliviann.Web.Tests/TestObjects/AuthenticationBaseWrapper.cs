#if NETFRAMEWORK

namespace Oliviann.Tests.TestObjects
{
    #region Usings

    using System;
    using System.Web;
    using Oliviann.Web;
    using Oliviann.Web.Security;
    using Microsoft.Extensions.Logging;

    #endregion Usings

    /// <summary>
    /// Represents a wrapper for unit testing the authentication base class.
    /// </summary>
    internal class AuthenticationBaseWrapper : AuthenticationBase
    {
        #region Fields

        private TraceContext ctx;

        #endregion Fields

        #region Constructor/Destructor

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="AuthenticationBaseWrapper" /> class.
        /// </summary>
        /// <param name="context">The trace context instance.</param>
        public AuthenticationBaseWrapper(TraceContext context = null) : base(new TraceContextLogger(context))
        {
            this.ctx = context;
        }

        public AuthenticationBaseWrapper(ILogger logger) : base(logger)
        {
        }

        #endregion Constructor/Destructor

        #region Methods

        internal string GetBaseUrlCaller(string inputText) => GetBaseUrl(inputText);

        public override void Authenticate(Func<string, bool> formsAuthenticationDelegate)
        {
            throw new NotImplementedException();
        }

        internal void TraceBaseMessage(string message, string category = "") => this.TraceMessage(message, category);

        internal void TraceBaseWarning(string message, string category = "") => this.TraceWarning(message, category);

        #endregion Methods
    }
}

#endif