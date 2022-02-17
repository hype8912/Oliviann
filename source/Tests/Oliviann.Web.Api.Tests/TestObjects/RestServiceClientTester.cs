namespace Oliviann.Web.Tests.TestObjects
{
    #region Usings

    using System.Net.Http;
    using Oliviann.Common.TestObjects.Xml;
    using Oliviann.Net.Http;

    #endregion Usings

    /// <summary>
    /// Represents a
    /// </summary>
    public class RestServiceClientTester : RestServiceClientBase
    {
        #region Fields

        private HttpClient client = null;

        #endregion Fields

        #region Constructor/Destructor

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="RestServiceClientTester"/> class.
        /// </summary>
        public RestServiceClientTester()
        {
        }

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="RestServiceClientTester"/> class.
        /// </summary>
        /// <param name="client">The client.</param>
        public RestServiceClientTester(HttpClient client)
        {
            this.client = client;
        }

        #endregion Constructor/Destructor

        #region Methods

        public Book GetBook()
        {
            return this.GetResult<Book>("api/Book/1");
        }

        /// <summary>
        /// Creates a new HTTP client instance.
        /// </summary>
        /// <returns>The newly create client instance.</returns>
        protected override HttpClient CreateHttpClient()
        {
            if (this.client == null)
            {
                return base.CreateHttpClient();
            }

            return this.client;
        }

        #endregion Methods
    }
}