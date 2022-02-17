#if !NET35

namespace Oliviann.Net.Http
{
    #region Usings

    using System;
    using System.Net.Http.Headers;

    #endregion Usings

    /// <summary>
    /// Represents a collection of options for a REST Service client.
    /// </summary>
    public class RestServiceOptions
    {
        #region Properties

        /// <summary>
        /// Gets or sets the authentication header.
        /// </summary>
        /// <value>
        /// The authentication header.
        /// </value>
        public AuthenticationHeaderValue AuthenticationHeader { get; set; }

        /// <summary>
        /// Gets or sets the base URI to the REST web service.
        /// </summary>
        /// <value>
        /// The base URI.
        /// </value>
        public Uri BaseUri { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to disable caching of data
        /// based on the headers.
        /// </summary>
        /// <value>
        /// True if to disable caching of response data; otherwise, false.
        /// </value>
        public bool DisableCaching { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether preference is retrieving
        /// JSON response over XML. Default value is true.
        /// </summary>
        /// <value>
        /// Indicates preference retrieving JSON response over XML.
        /// </value>
        public bool PreferJsonResponse { get; set; } = true;

        /// <summary>
        /// Gets or sets the timespan to wait before the request times out.
        /// Default value is 5 minutes.
        /// </summary>
        /// <value>
        /// The timespan to wait before the request times out.
        /// </value>
        public TimeSpan RequestTimeout { get; set; } = TimeSpan.FromMinutes(5);

        /// <summary>
        /// Gets or sets a value indicating whether to return content for all
        /// status codes.
        /// </summary>
        /// <value>
        /// True if content should be returned for all status code; otherwise,
        /// false to only return content on successful status codes.
        /// </value>
        public bool ReturnContentForAllStatusCodes { get; set; }

        #endregion Properties
    }
}

#endif