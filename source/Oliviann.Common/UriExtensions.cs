namespace Oliviann
{
    #region Usings

    using System;
    using System.Collections.Specialized;
    using System.IO;
#if NETSTANDARD1_3
    using Oliviann.Web;
#else
    using System.Web;
#endif

    #endregion Usings

    /// <summary>
    /// Represents a collection of commonly used extension methods for extending
    /// <see cref="Uri"/> class.
    /// </summary>
    public static class UriExtensions
    {
#if !NETSTANDARD1_3

        /// <summary>
        /// Adds a collection of query parameters to a base URL.
        /// </summary>
        /// <param name="baseUri">The base URI.</param>
        /// <param name="parameters">The collection of parameters to be added.
        /// </param>
        /// <returns>A newly created URI instance containing the specified
        /// parameters.</returns>
        public static Uri AddQuery(this Uri baseUri, NameValueCollection parameters)
        {
            if (baseUri == null || parameters == null || parameters.Count < 1)
            {
                return baseUri;
            }

            NameValueCollection queryCollection = HttpUtility.ParseQueryString(baseUri.Query);
            queryCollection.Add(parameters);

            var builder = new UriBuilder(baseUri) { Query = queryCollection.ToString() };
            return builder.Uri;
        }

#endif

        /// <summary>
        /// Combines the specified base URI.
        /// </summary>
        /// <param name="baseUri">The base URI.</param>
        /// <param name="relativeUri">The relative URI.</param>
        /// <returns>The base Uri combined with the relative Uri.</returns>
        public static Uri Combine(this Uri baseUri, string relativeUri)
        {
            if (relativeUri.IsNullOrEmpty())
            {
                return baseUri;
            }

            string fullUri;
            if (baseUri.IsFile)
            {
                fullUri = Path.Combine(baseUri.LocalPath, relativeUri);
            }
            else
            {
                fullUri = baseUri.AbsoluteUri.Trim('/') + '/' + relativeUri.Trim('/');
            }

            return new Uri(fullUri);
        }
    }
}