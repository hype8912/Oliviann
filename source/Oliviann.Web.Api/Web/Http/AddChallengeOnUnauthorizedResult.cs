#if NET45 || NET46 || NET47 || NET48

namespace Oliviann.Web.Http
{
    #region Usings

    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web.Http;

    #endregion Usings

    /// <summary>
    /// Represents a WebApi unauthorized challenge result.
    /// </summary>
    public class AddChallengeOnUnauthorizedResult : IHttpActionResult
    {
        #region Constructor/Destructor

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="AddChallengeOnUnauthorizedResult" /> class.
        /// </summary>
        /// <param name="challenge">The challenge header value.</param>
        /// <param name="innerResult">The inner result.</param>
        public AddChallengeOnUnauthorizedResult(AuthenticationHeaderValue challenge, IHttpActionResult innerResult)
        {
            this.Challenge = challenge;
            this.InnerResult = innerResult;
        }

        #endregion Constructor/Destructor

        #region Properties

        /// <summary>
        /// Gets the challenge header value.
        /// </summary>
        /// <value>
        /// The challenge header value.
        /// </value>
        public AuthenticationHeaderValue Challenge { get; }

        /// <summary>
        /// Gets the inner request result.
        /// </summary>
        /// <value>
        /// The inner request result.
        /// </value>
        public IHttpActionResult InnerResult { get; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Executes the current instance asynchronous.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The challenge response message.</returns>
        public async Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            HttpResponseMessage response = await this.InnerResult.ExecuteAsync(cancellationToken);
            if (response.StatusCode != HttpStatusCode.Unauthorized)
            {
                return response;
            }

            // Only add 1 challenge per authentication scheme.
            if (response.Headers.WwwAuthenticate.All(h => h.Scheme != this.Challenge.Scheme))
            {
                response.Headers.WwwAuthenticate.Add(this.Challenge);
            }

            return response;
        }

        #endregion Methods
    }
}

#endif