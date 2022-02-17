#if NETFRAMEWORK

namespace Oliviann.Configuration.Providers
{
    #region Usings

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

    #endregion

    /// <summary>
    /// Represents an ASP.NET Cookie configuration provider.
    /// </summary>
    public class CookieConfigurationProvider : ConfigurationProvider
    {
        #region Constructor/Destructor

        /// <summary>Initializes a new instance of the
        /// <see cref="CookieConfigurationProvider"/> class.</summary>
        /// <param name="cookieName">The name of the cookie.</param>
        public CookieConfigurationProvider(string cookieName) : base("Cookie:" + cookieName)
        {
            ADP.CheckArgumentNullOrEmpty(cookieName, nameof(cookieName));
        }

        #endregion

        #region Properties

        /// <inheritdoc />
        public override IEnumerable<string> GetChildKeys()
        {
            HttpCookie cookie = this.GetCurrentCookie();
            if (cookie == null)
            {
                return Enumerable.Empty<string>();
            }

            var items = new List<string> { "Name", "Domain", "Expires", "HttpOnly", "Path", "Secure", "Value" };
            if (cookie.HasKeys)
            {
                items.AddRange(cookie.Values.AllKeys.Where(key => key != null).Select(key => "Sub:" + key));
            }

            return items;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        public override void Load()
        {
            // Not used. We are going to go direct to the current cookie
            // instead of loading it into memory.
        }

        /// <inheritdoc />
        protected override void SetInt(string key, string value)
        {
            HttpCookie cookie = this.GetCurrentCookie();
            if (cookie == null)
            {
                // TODO: Figure out how to create a cookie if it doesn't exist.
                return;
            }

            if (key.StartsWith("Sub:", StringComparison.OrdinalIgnoreCase))
            {
                if (key.Length < 5)
                {
                    return;
                }

                key = key.Substring(4);
            }
            else
            {
                switch (key)
                {
                    case "Domain":
                        cookie.Domain = value;
                        return;

                    case "Expires":
                        if (DateTime.TryParse(value, out DateTime time))
                        {
                            cookie.Expires = time;
                        }

                        return;

                    case "HttpOnly":
                        cookie.HttpOnly = value.ToBoolean();
                        return;

                    case "Path":
                        cookie.Path = value;
                        return;

                    case "Secure":
                        cookie.Secure = value.ToBoolean();
                        return;

                    case "Value":
                        cookie.Value = value;
                        return;
                }
            }

            cookie.Values[key] = value;
        }

        /// <inheritdoc />
        protected override bool TryGetInt<T>(string key, out T value)
        {
            HttpCookie cookie = this.GetCurrentCookie();
            if (cookie == null)
            {
                value = default;
                return false;
            }

            if (key.StartsWith("Sub:", StringComparison.OrdinalIgnoreCase))
            {
                key = key.Substring(4);
            }
            else
            {
                switch (key)
                {
                    case "Name":
                        cookie.Name.TryCast(out value);
                        return true;

                    case "Domain":
                        cookie.Domain.TryCast(out value);
                        return true;

                    case "Expires":
                        cookie.Expires.TryCast(out value);
                        return true;

                    case "HttpOnly":
                        cookie.HttpOnly.TryCast(out value);
                        return true;

                    case "Path":
                        cookie.Path.TryCast(out value);
                        return true;

                    case "Secure":
                        cookie.Secure.TryCast(out value);
                        return true;

                    case "Value":
                        cookie.Value.TryCast(out value);
                        return true;
                }
            }

            if (cookie.HasKeys && cookie.Values.AllKeys.Contains(key))
            {
                cookie.Values[key].TryCast(out value);
                return true;
            }

            value = default;
            return false;
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Gets the current request cookie instance.
        /// </summary>
        /// <returns>The current request cookie instance.</returns>
        private HttpCookie GetCurrentCookie() => HttpContext.Current?.Request.Cookies.Get(this.Name.Substring(7));

        #endregion Helper Methods
    }
}

#endif