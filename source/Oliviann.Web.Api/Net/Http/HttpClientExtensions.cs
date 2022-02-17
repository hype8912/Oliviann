namespace Oliviann.Net.Http
{
    #region Usings

    using System;
    using System.Net.Http;
    using System.Net.Http.Formatting;
    using System.Net.Http.Headers;
    using System.Threading;
    using System.Threading.Tasks;

    #endregion

    /// <summary>
    /// Represents a collection of commonly used extension methods for extending
    /// <see cref="HttpClient"/>.
    /// </summary>
    public static class HttpClientExtensions
    {
        /// <summary>
        /// Sends a PATCH request as an asynchronous operation to the specified
        /// Uri with the given value serialized as JSON.
        /// </summary>
        /// <typeparam name="T">The type of value.</typeparam>
        /// <param name="client">The client used to make the request.</param>
        /// <param name="requestUri">The Uri the request is sent to.</param>
        /// <param name="value">The value that will be placed in the request's
        /// entity body.</param>
        /// <returns>A task object representing the asynchronous operation.
        /// </returns>
        public static Task<HttpResponseMessage> PatchAsJsonAsync<T>(this HttpClient client, string requestUri, T value) =>
            client.PatchAsJsonAsync(requestUri, value, CancellationToken.None);

        /// <summary>
        /// Sends a PATCH request as an asynchronous operation to the specified
        /// Uri with the given value serialized as JSON.
        /// </summary>
        /// <typeparam name="T">The type of value.</typeparam>
        /// <param name="client">The client used to make the request.</param>
        /// <param name="requestUri">The Uri the request is sent to.</param>
        /// <param name="value">The value that will be placed in the request's
        /// entity body.</param>
        /// <param name="cancelToken">A cancellation token that can be used by
        /// other objects or threads to receive notice of cancellation.</param>
        /// <returns>A task object representing the asynchronous operation.
        /// </returns>
        public static Task<HttpResponseMessage> PatchAsJsonAsync<T>(this HttpClient client, string requestUri, T value, CancellationToken cancelToken) =>
            client.PatchAsync(requestUri.IsNullOrEmpty() ? null : new Uri(requestUri, UriKind.RelativeOrAbsolute), value, new JsonMediaTypeFormatter(), null, cancelToken);

        /// <summary>
        /// Sends a PATCH request as an asynchronous operation to the specified
        /// Uri with the given value serialized as JSON.
        /// </summary>
        /// <typeparam name="T">The type of value.</typeparam>
        /// <param name="client">The client used to make the request.</param>
        /// <param name="requestUri">The Uri the request is sent to.</param>
        /// <param name="value">The value that will be placed in the request's
        /// entity body.</param>
        /// <returns>A task object representing the asynchronous operation.
        /// </returns>
        public static Task<HttpResponseMessage> PatchAsJsonAsync<T>(this HttpClient client, Uri requestUri, T value) =>
            client.PatchAsJsonAsync(requestUri, value, CancellationToken.None);

        /// <summary>
        /// Sends a PATCH request as an asynchronous operation to the specified
        /// Uri with the given value serialized as JSON.
        /// </summary>
        /// <typeparam name="T">The type of value.</typeparam>
        /// <param name="client">The client used to make the request.</param>
        /// <param name="requestUri">The Uri the request is sent to.</param>
        /// <param name="value">The value that will be placed in the request's
        /// entity body.</param>
        /// <param name="cancelToken">A cancellation token that can be used by
        /// other objects or threads to receive notice of cancellation.</param>
        /// <returns>A task object representing the asynchronous operation.
        /// </returns>
        public static Task<HttpResponseMessage> PatchAsJsonAsync<T>(this HttpClient client, Uri requestUri, T value, CancellationToken cancelToken) =>
            client.PatchAsync(requestUri, value, new JsonMediaTypeFormatter(), null, cancelToken);

        /// <summary>
        /// Sends a PATCH request as an asynchronous operation to the specified
        /// Uri with the given value serialized as XML.
        /// </summary>
        /// <typeparam name="T">The type of value.</typeparam>
        /// <param name="client">The client used to make the request.</param>
        /// <param name="requestUri">The Uri the request is sent to.</param>
        /// <param name="value">The value that will be placed in the request's
        /// entity body.</param>
        /// <returns>A task object representing the asynchronous operation.
        /// </returns>
        public static Task<HttpResponseMessage> PatchAsXmlAsync<T>(this HttpClient client, string requestUri, T value) =>
            client.PatchAsJsonAsync(requestUri, value, CancellationToken.None);

        /// <summary>
        /// Sends a PATCH request as an asynchronous operation to the specified
        /// Uri with the given value serialized as XML.
        /// </summary>
        /// <typeparam name="T">The type of value.</typeparam>
        /// <param name="client">The client used to make the request.</param>
        /// <param name="requestUri">The Uri the request is sent to.</param>
        /// <param name="value">The value that will be placed in the request's
        /// entity body.</param>
        /// <param name="cancelToken">A cancellation token that can be used by
        /// other objects or threads to receive notice of cancellation.</param>
        /// <returns>A task object representing the asynchronous operation.
        /// </returns>
        public static Task<HttpResponseMessage> PatchAsXmlAsync<T>(this HttpClient client, string requestUri, T value, CancellationToken cancelToken) =>
            client.PatchAsync(requestUri.IsNullOrEmpty() ? null : new Uri(requestUri, UriKind.RelativeOrAbsolute), value, new XmlMediaTypeFormatter(), null, cancelToken);

        /// <summary>
        /// Sends a PATCH request as an asynchronous operation to the specified
        /// Uri with the given value serialized as XML.
        /// </summary>
        /// <typeparam name="T">The type of value.</typeparam>
        /// <param name="client">The client used to make the request.</param>
        /// <param name="requestUri">The Uri the request is sent to.</param>
        /// <param name="value">The value that will be placed in the request's
        /// entity body.</param>
        /// <returns>A task object representing the asynchronous operation.
        /// </returns>
        public static Task<HttpResponseMessage> PatchAsXmlAsync<T>(this HttpClient client, Uri requestUri, T value) =>
            client.PatchAsJsonAsync(requestUri, value, CancellationToken.None);

        /// <summary>
        /// Sends a PATCH request as an asynchronous operation to the specified
        /// Uri with the given value serialized as XML.
        /// </summary>
        /// <typeparam name="T">The type of value.</typeparam>
        /// <param name="client">The client used to make the request.</param>
        /// <param name="requestUri">The Uri the request is sent to.</param>
        /// <param name="value">The value that will be placed in the request's
        /// entity body.</param>
        /// <param name="cancelToken">A cancellation token that can be used by
        /// other objects or threads to receive notice of cancellation.</param>
        /// <returns>A task object representing the asynchronous operation.
        /// </returns>
        public static Task<HttpResponseMessage> PatchAsXmlAsync<T>(this HttpClient client, Uri requestUri, T value, CancellationToken cancelToken) =>
            client.PatchAsync(requestUri, value, new XmlMediaTypeFormatter(), null, cancelToken);

        /// <summary>
        /// Sends a PATCH request as an asynchronous operation to the specified
        /// Uri with value serialized using the given formatter.
        /// </summary>
        /// <typeparam name="T">The type of value.</typeparam>
        /// <param name="client">The client used to make the request.</param>
        /// <param name="requestUri">The Uri the request is sent to.</param>
        /// <param name="value">The value that will be placed in the request's
        /// entity body.</param>
        /// <param name="formatter">The formatter used to serialize the value.
        /// </param>
        /// <param name="mediaType">The authoritative value of the request's
        /// content's Content-Type header. Can be null in which case the
        /// <paramref name="formatter">formatter's</paramref> default content
        /// type will be used.</param>
        /// <param name="cancelToken">A cancellation token that can be used by
        /// other objects or threads to receive notice of cancellation.</param>
        /// <returns>A task object representing the asynchronous operation.
        /// </returns>
        public static Task<HttpResponseMessage> PatchAsync<T>(
            this HttpClient client,
            Uri requestUri,
            T value,
            MediaTypeFormatter formatter,
            MediaTypeHeaderValue mediaType,
            CancellationToken cancelToken)
        {
            ADP.CheckArgumentNull(client, nameof(client));

            ObjectContent<T> objectContent = new(value, formatter, mediaType);
            return client.PatchAsync(requestUri, objectContent, cancelToken);
        }
    }
}