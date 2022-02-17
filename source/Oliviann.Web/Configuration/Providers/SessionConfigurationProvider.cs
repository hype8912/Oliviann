#if NETFRAMEWORK

namespace Oliviann.Configuration.Providers
{
    #region Usings

    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.SessionState;

    #endregion Usings

    /// <summary>
    /// Represents an ASP.NET Session configuration provider.
    /// </summary>
    public class SessionConfigurationProvider : ConfigurationProvider
    {
        #region Constructor/Destructor

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="SessionConfigurationProvider"/> class.
        /// </summary>
        public SessionConfigurationProvider() : base("Session")
        {
        }

        #endregion Constructor/Destructor

        #region Properties

        /// <inheritdoc />
        public override IEnumerable<string> GetChildKeys() => this.GetCurrentSession().Cast<string>().ToList();

        #endregion Properties

        #region Methods

        /// <inheritdoc />
        public override void Load()
        {
            // Not used. We are going to go direct to the current Session
            // instead of loading it into memory.
        }

        /// <inheritdoc />
        protected override void SetInt(string key, string value) => this.GetCurrentSession()[key] = value;

        /// <inheritdoc />
        protected override bool TryGetInt<T>(string key, out T value)
        {
            if (this.GetCurrentSession()[key] == null)
            {
                value = default;
                return false;
            }

            this.GetCurrentSession()[key].TryCast(out value);
            return true;
        }

        #endregion Methods

        #region Helper Methods

        /// <summary>
        /// Gets the current session instance.
        /// </summary>
        /// <returns>The current session instance.</returns>
        private HttpSessionState GetCurrentSession() => HttpContext.Current.Session;

        #endregion Helper Methods
    }
}

#endif