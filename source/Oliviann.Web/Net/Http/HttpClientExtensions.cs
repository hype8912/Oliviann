#if NETFRAMEWORK || NETSTANDARD2_0

namespace Oliviann.Net.Http
{
    #region Usings

    using System;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;

    #endregion

    /// <summary>
    /// Represents a collection of commonly used extension methods for extending
    /// <see cref="HttpClient"/>.
    /// </summary>
    public static partial class HttpClientExtensions
    {
        /// <summary>
        /// Sends a PATCH request to a Uri designated as a string as an
        /// asynchronous operation.
        /// </summary>
        /// <param name="client">The HttpClient to send the request.</param>
        /// <param name="requestUri">The Uri the request is sent to.</param>
        /// <param name="content">The HTTP request content sent to the server.
        /// </param>
        /// <returns>The task object representing the asynchronous operation.
        /// </returns>
        /// <remarks>Added in .NET Standard 2.1.</remarks>
        public static Task<HttpResponseMessage> PatchAsync(this HttpClient client, string requestUri, HttpContent content)
            => PatchAsync(client, requestUri, content, CancellationToken.None);

        /// <summary>
        /// Sends a PATCH request as an asynchronous operation.
        /// </summary>
        /// <param name="client">The HttpClient to send the request.</param>
        /// <param name="requestUri">The Uri the request is sent to.</param>
        /// <param name="content">The HTTP request content sent to the server.
        /// </param>
        /// <returns>The task object representing the asynchronous operation.
        /// </returns>
        /// <remarks>Added in .NET Standard 2.1.</remarks>
        public static Task<HttpResponseMessage> PatchAsync(this HttpClient client, Uri requestUri, HttpContent content)
            => PatchAsync(client, requestUri, content, CancellationToken.None);

        /// <summary>
        /// Sends a PATCH request with a cancellation token to a Uri represented
        /// as a string as an asynchronous operation.
        /// </summary>
        /// <param name="client">The HttpClient to send the request.</param>
        /// <param name="requestUri">The Uri the request is sent to.</param>
        /// <param name="content">The HTTP request content sent to the server.
        /// </param>
        /// <param name="cancelToken">A cancellation token that can be used by
        /// other objects or threads to receive notice of cancellation.</param>
        /// <returns>The task object representing the asynchronous operation.
        /// </returns>
        /// <remarks>Added in .NET Standard 2.1.</remarks>
        public static Task<HttpResponseMessage> PatchAsync(this HttpClient client, string requestUri, HttpContent content, CancellationToken cancelToken) =>
            PatchAsync(client, requestUri.IsNullOrEmpty() ? null : new Uri(requestUri, UriKind.RelativeOrAbsolute), content, cancelToken);

        /// <summary>
        /// Sends a PATCH request with a cancellation token as an asynchronous
        /// operation.
        /// </summary>
        /// <param name="client">The HttpClient to send the request.</param>
        /// <param name="requestUri">The Uri the request is sent to.</param>
        /// <param name="content">The HTTP request content sent to the server.
        /// </param>
        /// <param name="cancelToken">A cancellation token that can be used by
        /// other objects or threads to receive notice of cancellation.</param>
        /// <returns>The task object representing the asynchronous operation.
        /// </returns>
        /// <remarks>Added in .NET Standard 2.1.</remarks>
        public static Task<HttpResponseMessage> PatchAsync(this HttpClient client, Uri requestUri, HttpContent content, CancellationToken cancelToken)
        {
            ADP.CheckArgumentNull(client, nameof(client));

            var request = new HttpRequestMessage(new HttpMethod("Patch"), requestUri) { Content = content };
            return client.SendAsync(request, cancelToken);
        }
    }
}

#endif