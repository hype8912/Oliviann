#if NET45 || NET46 || NET47 || NET48

namespace Oliviann.Web.Http
{
    #region Usings

    using System.Net;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web.Http;

    #endregion Usings

    /// <summary>
    /// Represents a WebApi authentication failure result.
    /// </summary>
    public class AuthenticationFailureResult : IHttpActionResult
    {
        #region Constructor/Destructor

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="AuthenticationFailureResult" /> class.
        /// </summary>
        /// <param name="reasonPhrase">The authentication failure reason.
        /// </param>
        /// <param name="request">The current request instance.</param>
        public AuthenticationFailureResult(string reasonPhrase, HttpRequestMessage request)
        {
            this.ReasonPhrase = reasonPhrase;
            this.Request = request;
        }

        #endregion Constructor/Destructor

        #region Properties

        /// <summary>
        /// Gets the reason for the failure.
        /// </summary>
        /// <value>
        /// The reason for the failure.
        /// </value>
        public string ReasonPhrase { get; }

        /// <summary>
        /// Gets the current request instance.
        /// </summary>
        /// <value>
        /// The current request instance.
        /// </value>
        public HttpRequestMessage Request { get; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Executes the asynchronous response message.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>An unauthorized response message.</returns>
        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(this.Execute());
        }

        /// <summary>
        /// Executes the response message.
        /// </summary>
        /// <returns>An unauthorized response message.</returns>
        private HttpResponseMessage Execute()
        {
            var response =
                new HttpResponseMessage(HttpStatusCode.Unauthorized)
                {
                    RequestMessage = this.Request,
                    ReasonPhrase = this.ReasonPhrase
                };
            return response;
        }

        #endregion Methods
    }
}

#endif