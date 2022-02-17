#if NET40 || NET45 || NET46 || NET47 || NET48

namespace Oliviann.Web.Http
{
    #region Usings

    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Runtime.Caching;
    using System.Web.Http;
    using System.Web.Http.Controllers;

    #endregion Usings

    /// <summary>
    /// Represents a simple API authorization filter for a Web API controller.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public sealed class SimpleApiTokenAuthorizeAttribute : AuthorizeAttribute
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

        /// <summary>
        /// Determines if the authorization feature is active.
        /// </summary>
        private readonly bool isActive;

        #endregion Fields

        #region Constructor/Destructor

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="SimpleApiTokenAuthorizeAttribute"/> class.
        /// </summary>
        public SimpleApiTokenAuthorizeAttribute() : this(true)
        {
        }

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="SimpleApiTokenAuthorizeAttribute"/> class.
        /// </summary>`
        /// <param name="active">True if authorization feature is active;
        /// otherwise, false to disable authorization.</param>
        public SimpleApiTokenAuthorizeAttribute(bool active)
        {
            this.isActive = active;
            this.LoadAuthorizationTokensToCache();
        }

        #endregion Constructor/Destructor

        #region Properties

        /// <summary>
        /// Gets the cache instance.
        /// </summary>
        /// <value>
        /// The cache instance.
        /// </value>
        private MemoryCache CacheInstance => MemoryCache.Default;

        #endregion Properties

        #region Methods

        /// <summary>Calls when an action is being authorized.</summary>
        /// <param name="context">The current action context.</param>
        public override void OnAuthorization(HttpActionContext context)
        {
            if (!this.isActive)
            {
                return;
            }

            // 1. Look for credentials in the request.
            HttpRequestMessage request = context.Request;
            AuthenticationHeaderValue authorization = request.Headers.Authorization;

            // 2. If there are no credentials, do nothing.
            if (authorization == null)
            {
                context.Response = this.CreateUnauthorizedResponse(context, "Authorization information not provided.");
                return;
            }

            // 3. If there are credentials but the filter does not recognize the
            //    authentication scheme, do nothing.
            if (!authorization.Scheme.EqualsOrdinalIgnoreCase("Bearer"))
            {
                context.Response = this.CreateUnauthorizedResponse(context, "Authorization scheme is incorrect.");
                return;
            }

            // 4. If there are credentials that the filter understands, try to validate them.
            // 5. If the credentials are bad, set the error result.
            if (authorization.Parameter.IsNullOrEmpty())
            {
                context.Response = this.CreateUnauthorizedResponse(context, "Authorization has been denied for this request.");
                return;
            }

            // 6. Validates API Token is in the valid token list.
            string authString = authorization.Parameter.ToUpperInvariant();
            if (!this.CacheInstance.Contains(CacheKeyPrefix + authString))
            {
                context.Response = this.CreateUnauthorizedResponse(context, "Authorization has been denied for this request.");
            }
        }

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

        /// <summary>
        /// Creates an unauthorized error response with a message.
        /// </summary>
        /// <param name="context">The HTTP action context.</param>
        /// <param name="message">The message to be displayed in the error
        /// response.</param>
        /// <returns>An unauthorized response message with the specified
        /// message.</returns>
        private HttpResponseMessage CreateUnauthorizedResponse(HttpActionContext context, string message)
        {
            return context.ControllerContext.Request.CreateErrorResponse(HttpStatusCode.Unauthorized, message);
        }

        #endregion Methods
    }
}

#endif