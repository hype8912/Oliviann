namespace Oliviann.Web.Authentication.CloudFoundry
{
    #region Usings

    using System;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authentication.Cookies;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.IdentityModel.Protocols.OpenIdConnect;

    #endregion

    /// <summary>
    /// Represents a collection of methods for extending the functionality of
    /// <see cref="IServiceCollection"/>.
    /// </summary>
    public static class IServiceCollectionExtensions
    {
        #region Fields

        /// <summary>
        /// The name of the OIDC authentication scheme.
        /// </summary>
        private const string OIDCAuthenticationScheme = "oidc";

        #endregion

        /// <summary>
        /// Adds the Cloud Foundry SSO tile authentication capability to the
        /// service collection.
        /// </summary>
        /// <param name="services">The IServiceCollection to add services to.
        /// </param>
        /// <param name="configureOptions">The delegate for configuring the
        /// single sign on options.</param>
        /// <returns>The IServiceCollection so that additional calls can be
        /// chained.</returns>
        public static IServiceCollection AddSingleSignOnTile(this IServiceCollection services,
            Action<SingleSignOnOptions> configureOptions)
        {
            ADP.CheckArgumentNull(configureOptions, nameof(configureOptions));

            var options = new SingleSignOnOptions();
            configureOptions.Invoke(options);
            return services.AddSingleSignOnTile(options.ClientUrl, options.ClientId, options.ClientSecret);
        }

        /// <summary>
        /// Adds the Cloud Foundry SSO tile authentication capability to the
        /// service collection.
        /// </summary>
        /// <param name="services">The IServiceCollection to add services to.
        /// </param>
        /// <param name="options">The single sign on options.</param>
        /// <returns>The IServiceCollection so that additional calls can be
        /// chained.</returns>
        public static IServiceCollection AddSingleSignOnTile(this IServiceCollection services, SingleSignOnOptions options)
        {
            ADP.CheckArgumentNull(options, nameof(options));
            return services.AddSingleSignOnTile(options.ClientUrl, options.ClientId, options.ClientSecret);
        }

        /// <summary>
        /// Adds the Cloud Foundry SSO tile authentication capability to the
        /// service collection.
        /// </summary>
        /// <param name="services">The IServiceCollection to add services to.
        /// </param>
        /// <param name="clientUrl">The client URL from Cloud Foundry.</param>
        /// <param name="clientId">The client id from Cloud Foundry.</param>
        /// <param name="clientSecret">The client secret from Cloud Foundry.
        /// </param>
        /// <returns>The IServiceCollection so that additional calls can be
        /// chained.</returns>
        public static IServiceCollection AddSingleSignOnTile(this IServiceCollection services, string clientUrl, string clientId, string clientSecret)
        {
            ADP.CheckArgumentNullOrEmpty(clientUrl, nameof(clientUrl));
            ADP.CheckArgumentNullOrEmpty(clientId, nameof(clientId));
            ADP.CheckArgumentNullOrEmpty(clientSecret, nameof(clientSecret));

            services.AddAuthentication(options =>
                {
                    options.DefaultChallengeScheme = options.DefaultAuthenticateScheme = OIDCAuthenticationScheme;
                    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                })
                .AddOpenIdConnect(
                    OIDCAuthenticationScheme,
                    o =>
                {
                    o.Authority = clientUrl;
                    o.ClientId = clientId;
                    o.ClientSecret = clientSecret;
                    o.GetClaimsFromUserInfoEndpoint = true;
                    o.ResponseType = OpenIdConnectResponseType.Code;
                    o.SaveTokens = true;
                    o.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;

                    // The SSO Service broker sets fields in odd fields,
                    // so it helps to map them
                    o.ClaimActions.MapAll();
                })
                .AddCookie();

            return services;
        }
    }
}