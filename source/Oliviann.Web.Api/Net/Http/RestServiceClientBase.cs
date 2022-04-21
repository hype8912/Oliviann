namespace Oliviann.Net.Http
{
    #region Usings

    using System;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;

    #endregion Usings

    /// <summary>
    /// Represents a base class for consuming REST web services.
    /// </summary>
    public abstract class RestServiceClientBase : IDisposable
    {
        #region Fields

        /// <summary>
        /// The client cache instance.
        /// </summary>
        private readonly IRestServiceClientCache clientCache;

        /// <summary>
        /// The HTTP client instance.
        /// </summary>
        private HttpClient client;

        #endregion Fields

        #region Constructor/Destructor

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="RestServiceClientBase"/> class.
        /// </summary>
        protected RestServiceClientBase()
        {
            this.clientCache = new RestServiceClientCache();
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="RestServiceClientBase"/>
        /// class.
        /// </summary>
        ~RestServiceClientBase() => this.Dispose(false);

        #endregion Constructor/Destructor

        #region Properties

        /// <summary>
        /// Gets the configurable service options.
        /// </summary>
        /// <value>The configurable service options.</value>
        public virtual RestServiceOptions Options { get; internal set; } = new RestServiceOptions();

        /// <summary>
        /// Gets the client instance.
        /// </summary>
        /// <value>
        /// The HTTP client instance.
        /// </value>
        protected HttpClient Client => this.client ??= this.CreateHttpClient();

        #endregion Properties

        #region Methods

        /// <summary>
        /// Performs application-defined tasks associated with freeing,
        /// releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing">True to release both managed and unmanaged
        /// resources; false to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.client?.Dispose();
                this.client = null;
            }
        }

        /// <summary>
        /// Creates a new HTTP client instance.
        /// </summary>
        /// <returns>The newly create client instance.</returns>
        protected virtual HttpClient CreateHttpClient()
        {
            var httpClient = new HttpClient { BaseAddress = this.Options.BaseUri, Timeout = this.Options.RequestTimeout };
            httpClient.DefaultRequestHeaders.Accept.Clear();

            string contentType = this.Options.PreferJsonResponse ? "application/json" : "application/xml";
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));

            if (this.Options.AuthenticationHeader != null)
            {
                httpClient.DefaultRequestHeaders.Authorization = this.Options.AuthenticationHeader;
            }

            return httpClient;
        }

        /// <summary>
        /// Gets the endpoint result synchronous.
        /// </summary>
        /// <param name="requestUri">The endpoint request URI.</param>
        /// <param name="resultType">The type of object being requested from the
        /// endpoint.</param>
        /// <returns>The endpoint result object if successful status code.
        /// </returns>
        protected object GetResult(string requestUri, Type resultType) =>
            this.GetResult(requestUri, content => content.ReadAsAsync(resultType).Result);

        /// <summary>
        /// Gets the endpoint result synchronous.
        /// </summary>
        /// <typeparam name="T">The type of object being requested from the
        /// endpoint.</typeparam>
        /// <param name="requestUri">The endpoint request URI.</param>
        /// <returns>The endpoint result object if successful status code.
        /// </returns>
        protected T GetResult<T>(string requestUri) => this.GetResult(requestUri, content => content.ReadAsAsync<T>().Result);

#if !NET35 && !NET40
        /// <summary>
        /// Gets the endpoint result asynchronous.
        /// </summary>
        /// <typeparam name="T">The type of object being requested from the
        /// endpoint.</typeparam>
        /// <param name="requestUri">The endpoint request URI.</param>
        /// <returns>The endpoint result object if successful status code.
        /// </returns>
        protected virtual async Task<T> GetResultAsync<T>(string requestUri)
        {
            T cacheResult = this.GetResponseFromCache<T>(requestUri);
            if (!cacheResult.IsDefault())
            {
                return cacheResult;
            }

            T result = default;
            HttpResponseMessage response = await this.Client.GetAsync(requestUri).ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
            {
                result = await response.Content.ReadAsAsync<T>().ConfigureAwait(false);
                this.AddResponseToCache(response, result);
            }

            return result;
        }
#endif

        /// <summary>
        /// Patches the specified value to the requested url.
        /// </summary>
        /// <param name="requestUri">The endpoint request URI.</param>
        /// <param name="value">The value to post.</param>
        /// <param name="resultType">Type of the result.</param>
        /// <returns>The endpoint result object if successful status code.</returns>
        protected object PatchResult(string requestUri, object value, Type resultType) =>
            this.PatchResult(requestUri, value, content => content.ReadAsAsync(resultType).Result);

        /// <summary>
        /// Patches the specified value to the requested url.
        /// </summary>
        /// <typeparam name="TOut">The type of the endpoint result.</typeparam>
        /// <param name="requestUri">The endpoint request URI.</param>
        /// <returns>The endpoint result object if successful status code.</returns>
        protected TOut PatchResult<TOut>(string requestUri) => this.PatchResult<string, TOut>(requestUri, string.Empty);

        /// <summary>
        /// Patches the specified value to the requested url.
        /// </summary>
        /// <typeparam name="TIn">The type of the endpoint input.</typeparam>
        /// <typeparam name="TOut">The type of the endpoint result.</typeparam>
        /// <param name="requestUri">The endpoint request URI.</param>
        /// <param name="value">The value to post.</param>
        /// <returns>The endpoint result object if successful status code.</returns>
        protected TOut PatchResult<TIn, TOut>(string requestUri, TIn value) =>
            this.PatchResult(requestUri, value, content => content.ReadAsAsync<TOut>().Result);

        /// <summary>
        /// Posts the specified value to the requested url.
        /// </summary>
        /// <param name="requestUri">The endpoint request URI.</param>
        /// <param name="value">The value to post.</param>
        /// <param name="resultType">Type of the result.</param>
        /// <returns>The endpoint result object if successful status code.</returns>
        protected object PostResult(string requestUri, object value, Type resultType) =>
            this.PostResult(requestUri, value, content => content.ReadAsAsync(resultType).Result);

        /// <summary>
        /// Posts the specified value to the requested url.
        /// </summary>
        /// <typeparam name="TOut">The type of the endpoint result.</typeparam>
        /// <param name="requestUri">The endpoint request URI.</param>
        /// <returns>The endpoint result object if successful status code.</returns>
        protected TOut PostResult<TOut>(string requestUri) => this.PostResult<string, TOut>(requestUri, string.Empty);

        /// <summary>
        /// Posts the specified value to the requested url.
        /// </summary>
        /// <typeparam name="TIn">The type of the endpoint input.</typeparam>
        /// <typeparam name="TOut">The type of the endpoint result.</typeparam>
        /// <param name="requestUri">The endpoint request URI.</param>
        /// <param name="value">The value to post.</param>
        /// <returns>The endpoint result object if successful status code.</returns>
        protected TOut PostResult<TIn, TOut>(string requestUri, TIn value) =>
            this.PostResult(requestUri, value, content => content.ReadAsAsync<TOut>().Result);

        /// <summary>
        /// Puts the specified value to the requested url.
        /// </summary>
        /// <param name="requestUri">The endpoint request URI.</param>
        /// <param name="value">The value to post.</param>
        /// <param name="resultType">Type of the result.</param>
        /// <returns>The endpoint result object if successful status code.</returns>
        protected object PutResult(string requestUri, object value, Type resultType) =>
            this.PutResult(requestUri, value, content => content.ReadAsAsync(resultType).Result);

        /// <summary>
        /// Puts the specified value to the requested url.
        /// </summary>
        /// <typeparam name="TOut">The type of the endpoint result.</typeparam>
        /// <param name="requestUri">The endpoint request URI.</param>
        /// <returns>The endpoint result object if successful status code.</returns>
        protected TOut PutResult<TOut>(string requestUri) => this.PutResult<string, TOut>(requestUri, string.Empty);

        /// <summary>
        /// Puts the specified value to the requested url.
        /// </summary>
        /// <typeparam name="TIn">The type of the endpoint input.</typeparam>
        /// <typeparam name="TOut">The type of the endpoint result.</typeparam>
        /// <param name="requestUri">The endpoint request URI.</param>
        /// <param name="value">The value to post.</param>
        /// <returns>The endpoint result object if successful status code.</returns>
        protected TOut PutResult<TIn, TOut>(string requestUri, TIn value) =>
            this.PutResult(requestUri, value, content => content.ReadAsAsync<TOut>().Result);

        /// <summary>
        /// Deletes the endpoint result synchronous.
        /// </summary>
        /// <param name="requestUri">The endpoint request URI.</param>
        /// <param name="resultType">The type of object being requested from the
        /// endpoint.</param>
        /// <returns>The endpoint result object if successful status code.
        /// </returns>
        protected object DeleteResult(string requestUri, Type resultType) =>
            this.DeleteResult(requestUri, content => content.ReadAsAsync(resultType).Result);

        /// <summary>
        /// Deletes the endpoint result synchronous.
        /// </summary>
        /// <typeparam name="T">The type of object being requested from the
        /// endpoint.</typeparam>
        /// <param name="requestUri">The endpoint request URI.</param>
        /// <returns>The endpoint result object if successful status code.
        /// </returns>
        protected T DeleteResult<T>(string requestUri) =>
            this.DeleteResult(requestUri, content => content.ReadAsAsync<T>().Result);

        #endregion Methods

        #region Helper Methods

        /// <summary>
        /// Gets the endpoint result synchronous.
        /// </summary>
        /// <typeparam name="T">The type of object being requested from the
        /// endpoint.</typeparam>
        /// <param name="requestUri">The endpoint request URI.</param>
        /// <param name="contentReader">The delegate content reader for reading
        /// the response content.</param>
        /// <returns>
        /// The endpoint result object if successful status code.
        /// </returns>
        protected virtual T GetResult<T>(string requestUri, Func<HttpContent, T> contentReader)
        {
            T cacheResult = this.GetResponseFromCache<T>(requestUri);
            if (!cacheResult.IsDefault())
            {
                return cacheResult;
            }

            T result = default;
            HttpResponseMessage response = this.Client.GetAsync(requestUri).Result;
            if (response.IsSuccessStatusCode || this.Options.ReturnContentForAllStatusCodes)
            {
                result = contentReader(response.Content);

                // We still aren't going to cache a bad result.
                if (response.IsSuccessStatusCode)
                {
                    this.AddResponseToCache(response, result);
                }
            }

            return result;
        }

        /// <summary>
        /// Patches the specified value to the endpoint synchronous.
        /// </summary>
        /// <typeparam name="TIn">The type of object being sent to the endpoint.
        /// </typeparam>
        /// <typeparam name="TOut">The type of object being requested from the
        /// endpoint.</typeparam>
        /// <param name="requestUri">The endpoint request URI.</param>
        /// <param name="value">The endpoint input value.</param>
        /// <param name="resultContentReader">The result content reader.</param>
        /// <returns>The endpoint result object if successful status code.
        /// </returns>
        protected virtual TOut PatchResult<TIn, TOut>(string requestUri, TIn value, Func<HttpContent, TOut> resultContentReader)
        {
            TOut result = default;
            HttpResponseMessage response = this.Options.PreferJsonResponse
                                               ? this.Client.PatchAsJsonAsync(requestUri, value).Result
                                               : this.Client.PatchAsXmlAsync(requestUri, value).Result;

            if (response.IsSuccessStatusCode ||
                this.Options.ReturnContentForAllStatusCodes)
            {
                result = resultContentReader(response.Content);
            }

            return result;
        }

        /// <summary>
        /// Posts the specified value to the endpoint synchronous.
        /// </summary>
        /// <typeparam name="TIn">The type of object being sent to the endpoint.
        /// </typeparam>
        /// <typeparam name="TOut">The type of object being requested from the
        /// endpoint.</typeparam>
        /// <param name="requestUri">The endpoint request URI.</param>
        /// <param name="value">The endpoint input value.</param>
        /// <param name="resultContentReader">The result content reader.</param>
        /// <returns>The endpoint result object if successful status code.
        /// </returns>
        protected virtual TOut PostResult<TIn, TOut>(string requestUri, TIn value, Func<HttpContent, TOut> resultContentReader)
        {
            TOut result = default;
            HttpResponseMessage response = this.Options.PreferJsonResponse
                                               ? this.Client.PostAsJsonAsync(requestUri, value).Result
                                               : this.Client.PostAsXmlAsync(requestUri, value).Result;

            if (response.IsSuccessStatusCode ||
                this.Options.ReturnContentForAllStatusCodes)
            {
                result = resultContentReader(response.Content);
            }

            return result;
        }

        /// <summary>
        /// Puts the specified value to the endpoint synchronous.
        /// </summary>
        /// <typeparam name="TIn">The type of object being sent to the endpoint.
        /// </typeparam>
        /// <typeparam name="TOut">The type of object being requested from the
        /// endpoint.</typeparam>
        /// <param name="requestUri">The endpoint request URI.</param>
        /// <param name="value">The endpoint input value.</param>
        /// <param name="resultContentReader">The result content reader.</param>
        /// <returns>The endpoint result object if successful status code.
        /// </returns>
        protected virtual TOut PutResult<TIn, TOut>(string requestUri, TIn value, Func<HttpContent, TOut> resultContentReader)
        {
            TOut result = default;
            HttpResponseMessage response = this.Options.PreferJsonResponse
                                               ? this.Client.PutAsJsonAsync(requestUri, value).Result
                                               : this.Client.PutAsXmlAsync(requestUri, value).Result;

            if (response.IsSuccessStatusCode ||
                this.Options.ReturnContentForAllStatusCodes)
            {
                result = resultContentReader(response.Content);
            }

            return result;
        }

        /// <summary>
        /// Deletes the endpoint result synchronous.
        /// </summary>
        /// <typeparam name="T">The type of object being requested from the
        /// endpoint.</typeparam>
        /// <param name="requestUri">The endpoint request URI.</param>
        /// <param name="contentReader">The delegate content reader for reading
        /// the response content.</param>
        /// <returns>
        /// The endpoint result object if successful status code.
        /// </returns>
        protected virtual T DeleteResult<T>(string requestUri, Func<HttpContent, T> contentReader)
        {
            T result = default;
            HttpResponseMessage response = this.Client.DeleteAsync(requestUri).Result;

            if (response.IsSuccessStatusCode ||
                this.Options.ReturnContentForAllStatusCodes)
            {
                result = contentReader(response.Content);
            }

            return result;
        }

        /// <summary>
        /// Gets cached response data for the specified url if caching is not
        /// disabled.
        /// </summary>
        /// <typeparam name="T">Type of object to be returned.</typeparam>
        /// <param name="requestUri">The URI for which to get response data for.
        /// </param>
        /// <returns>The cached response data if one exists; otherwise, the
        /// default value.</returns>
        protected T GetResponseFromCache<T>(string requestUri)
        {
            return this.Options.DisableCaching ? default : this.clientCache.GetResponseFromCache<T>(requestUri);
        }

        /// <summary>
        /// Cache the current response.
        /// </summary>
        /// <typeparam name="T">Type of object to retrieve.</typeparam>
        /// <param name="response">The <c>HttpResponseMessage</c> for given
        /// request</param>
        /// <param name="result">Payload of current response message.</param>
        protected void AddResponseToCache<T>(HttpResponseMessage response, T result)
        {
            if (!this.Options.DisableCaching)
            {
                this.clientCache.ClientCacheSet(response, result);
            }
        }

        #endregion Helper Methods
    }
}