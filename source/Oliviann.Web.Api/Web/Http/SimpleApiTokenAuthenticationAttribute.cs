#if DEBUG && (NET45 || NET46 || NET47 || NET48)

namespace Oliviann.Web.Http
{
    #region Usings

    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Runtime.Caching;
    using System.Security.Principal;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web.Http.Filters;

    #endregion Usings

    /// <summary>
    /// Represents a
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public sealed class SimpleApiTokenAuthenticationAttribute : FilterAttribute, IAuthenticationFilter
    {
        #region Fields

        /// <summary>
        /// The prefix string to the API token before it's loaded in cache.
        /// </summary>
        private const string CacheKeyPrefix = "ApiAuthToken:";

        /// <summary>
        /// The prefix string to the authorization tokens in the setting file.
        /// </summary>
        private const string SettingsPrefixString = "ApiAuthToken:";

        #endregion Fields

        #region Constructor/Destructor

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="SimpleApiTokenAuthenticationAttribute"/> class.
        /// </summary>
        public SimpleApiTokenAuthenticationAttribute()
        {
            this.LoadAuthorizationTokensToCache();
        }

        #endregion Constructor/Destructor

        #region Properties

        /// <inheritdoc />
        public override bool AllowMultiple => false;

        /// <summary>
        /// Gets the cache instance.
        /// </summary>
        /// <value>
        /// The cache instance.
        /// </value>
        private MemoryCache CacheInstance => MemoryCache.Default;

        #endregion Properties

        #region Methods

        /// <summary>
        /// Authenticates the current request.
        /// </summary>
        /// <param name="context">The current authentication context.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>An empty task.</returns>
        public async Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
            // 1. Look for credentials in the request.
            HttpRequestMessage request = context.Request;
            AuthenticationHeaderValue authorization = request.Headers.Authorization;

            // 2. If there are no credentials, do nothing.
            if (authorization == null)
            {
                // Authentication information was not provided.
                return;
            }

            // 3. If there are credentials but the filter does not recognize the
            //    authentication scheme, do nothing.
            if (!authorization.Scheme.EqualsOrdinalIgnoreCase("Bearer"))
            {
                // Authentication scheme is incorrect.
                return;
            }

            // 4. If there are credentials that the filter understands, try to validate them.
            // 5. If the credentials are bad, set the error result.
            if (authorization.Parameter.IsNullOrEmpty())
            {
                // Authentication has been denied for this request.
                context.ErrorResult = new AuthenticationFailureResult("Authentication has been denied for this request.", request);
                return;
            }

            // 6. Validates API Token is in the valid token list.
            string authString = authorization.Parameter.ToUpperInvariant();
            if (!this.CacheInstance.Contains(CacheKeyPrefix + authString))
            {
                // Authentication has been denied for this request.
                context.ErrorResult = new AuthenticationFailureResult("Authorization has been denied for this request.", request);
            }

            var id = new GenericIdentity(authString);
            var principal = new GenericPrincipal(id, null);
            context.Principal = principal;
        }

        /// <summary>
        /// Executes the challenge response of the authentication type.
        /// </summary>
        /// <param name="context">The current authentication challenge context.
        /// </param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A challenge unauthorized result.</returns>
        public async Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
        {
            var challenge = new AuthenticationHeaderValue("Bearer");
            context.Result = new AddChallengeOnUnauthorizedResult(challenge, context.Result);
        }

        #endregion Methods

        #region Helper Methods

        /// <summary>
        /// Reads the authorization tokens from the application settings and
        /// loads them to the cache.
        /// </summary>
        /// <remarks>We used <see cref="MemoryCache"/> to reduce the load on
        /// polling data in the settings file.</remarks>
        private void LoadAuthorizationTokensToCache()
        {
            const string AuthorizationTokensLoadedKey = "ApiAuthKeysLoaded";
            if (this.CacheInstance.Contains(AuthorizationTokensLoadedKey))
            {
                // We've already loaded all the token keys to the cache.
                return;
            }

            List<string> keys = ConfigurationManager.AppSettings.AllKeys.Where(k => k.StartsWith(SettingsPrefixString)).ToList();
            int settingsPrefixLength = SettingsPrefixString.Length;
            foreach (string authKeyTokens in keys)
            {
                string token = ConfigurationManager.AppSettings[authKeyTokens].ToUpperInvariant();
                string value = authKeyTokens.Substring(settingsPrefixLength);
                this.CacheInstance.Add(CacheKeyPrefix + token, value, DateTimeOffset.MaxValue);
            }

            this.CacheInstance.Add(AuthorizationTokensLoadedKey, DateTime.UtcNow, DateTimeOffset.MaxValue);
        }

        #endregion Helper Methods
    }
}

#endif