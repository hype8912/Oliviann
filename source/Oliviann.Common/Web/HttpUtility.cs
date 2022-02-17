#if NETSTANDARD1_3
namespace Oliviann.Web
{
    #region Usings

    using System;
    using System.Collections.Specialized;

    #endregion

    /// <summary>
    /// Provides methods for encoding and decoding URLs when processing Web
    /// requests.
    /// </summary>
    public static class HttpUtility
    {
        /// <summary>Converts a string that has been HTML-encoded for HTTP
        /// transmission into a decoded string.</summary>
        /// <returns>A decoded string.</returns>
        /// <param name="value">The string to decode.</param>
        public static string HtmlDecode(string value) => System.Net.WebUtility.HtmlDecode(value);

        /// <summary>Converts a string to an HTML-encoded string.</summary>
        /// <returns>An encoded string.</returns>
        /// <param name="value">The string to encode.</param>
        public static string HtmlEncode(string value) => System.Net.WebUtility.HtmlEncode(value);

        /// <summary>
        /// Parses a query string into a NameValueCollection.
        /// </summary>
        /// <param name="query">The query string to parse.</param>
        /// <returns>A NameValueCollection of query parameters and values.
        /// </returns>
        public static NameValueCollection ParseQueryString(string query)
        {
            var items = new NameValueCollection();
            if (query.IsNullOrEmpty())
            {
                return items;
            }

            string[] args = query.Split('&', '?');
            foreach (string arg in args)
            {
                if (arg.IsNullOrEmpty())
                {
                    continue;
                }

                int index = arg.IndexOf('=');
                if (index < 0)
                {
                    continue;
                }

                items.Add(Uri.UnescapeDataString(arg.Substring(0, index)), Uri.UnescapeDataString(arg.Substring(index + 1)));
            }

            return items;
        }
    }
}
#endif