namespace Oliviann.Net.Http
{
    #region Usings

    using System;
    using System.Net.Http;
    using System.Reflection;
    using Oliviann.Properties;
    using Castle.DynamicProxy;

    #endregion Usings

    /// <summary>
    /// Represents a REST web service consumer class.
    /// </summary>
    /// <typeparam name="T">The interface type to proxy.</typeparam>
    public class RestServiceConsumer<T> where T : class
    {
        #region Fields

        /// <summary>
        /// The shared proxy generator instance.
        /// </summary>
        private readonly ProxyGenerator generator;

        #endregion Fields

        #region Constructor/Destructor

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="RestServiceConsumer{T}" /> class.
        /// </summary>
        /// <param name="baseUrl">The web service base URL.</param>
        public RestServiceConsumer(Uri baseUrl)
        {
            this.generator = new ProxyGenerator();
            this.Options = new RestServiceOptions { BaseUri = baseUrl };
        }

        #endregion Constructor/Destructor

        #region Properties

        /// <summary>
        /// Gets the configurable service options.
        /// </summary>
        /// <value>The configurable service options.</value>
        public RestServiceOptions Options { get; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Creates a proxy for communicating with the interface.
        /// </summary>
        /// <param name="endpointUrl">The endpoint URL. URL parameters must
        /// match the argument name on the method being called from the proxy.
        /// </param>
        /// <returns>A new proxy instance of the interface.</returns>
        public T Proxy(string endpointUrl) => this.Proxy(endpointUrl, HttpMethod.Get);

        /// <summary>
        /// Creates a proxy for communicating with the interface.
        /// </summary>
        /// <param name="endpointUrl">The endpoint URL. URL parameters must
        /// match the argument name on the method being called from the proxy.
        /// </param>
        /// <param name="method">The method action type.</param>
        /// <returns>
        /// A new proxy instance of the interface.
        /// </returns>
        public T Proxy(string endpointUrl, HttpMethod method)
        {
            ADP.CheckArgumentNullOrEmpty(endpointUrl, nameof(endpointUrl));

            var interceptor = new RestServiceInterceptor { Options = this.Options, EndpointUrl = endpointUrl, Method = method };
            T client = this.CreateProxy(interceptor);
            return client;
        }

        /// <summary>
        /// Creates a new proxy for the specified interface.
        /// </summary>
        /// <param name="interceptor">The interceptor class to perform the work.
        /// </param>
        /// <returns>A proxy instance from the generator.</returns>
        /// <exception cref="System.ArgumentException">Class generic type must
        /// be of type interface.</exception>
        private T CreateProxy(IInterceptor interceptor)
        {
#if NET35 || NET40
            bool isInterface = typeof(T).IsInterface;
#else
            bool isInterface = typeof(T).GetTypeInfo().IsInterface;
#endif

            if (!isInterface)
            {
                throw new ArgumentException(Resources.ERR_ClassTypeMustBeInterface);
            }

            return this.generator.CreateInterfaceProxyWithoutTarget<T>(interceptor);
        }

        #endregion Methods
    }
}