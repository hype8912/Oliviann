namespace Oliviann.Net.Http
{
    #region Usings

    using System.Net.Http;
    using System.Reflection;
    using Oliviann.Collections.Generic;
    using Castle.DynamicProxy;

    #endregion Usings

    /// <summary>
    /// Represents a REST service client proxy interceptor.
    /// </summary>
    internal class RestServiceInterceptor : RestServiceClientBase, IInterceptor
    {
        #region Properties

        /// <summary>
        /// Gets or sets the endpoint request URI.
        /// </summary>
        /// <value>
        /// The endpoint request URI.
        /// </value>
        internal string EndpointUrl { get; set; }

        /// <summary>
        /// Gets or sets the method action.
        /// </summary>
        /// <value>The method action.</value>
        internal HttpMethod Method { get; set; } = HttpMethod.Get;

        #endregion Properties

        #region Methods

        /// <summary>
        /// Intercepts the specified invocation.
        /// </summary>
        /// <param name="invocation">The invocation instance.</param>
        public void Intercept(IInvocation invocation)
        {
            ParameterInfo[] parameters = invocation.Method.GetParameters();
            string url = this.EndpointUrl;

            // Replace the url parameters with the input method argument values.
            for (int i = 0; i < parameters.Length; i += 1)
            {
                url = url.Replace("{" + parameters[i].Name + "}", invocation.Arguments[i].ToString());
            }

            if (this.Method == HttpMethod.Get)
            {
                invocation.ReturnValue = this.GetResult(url, invocation.Method.ReturnType);
                return;
            }

            if (this.Method == HttpMethod.Delete)
            {
                invocation.ReturnValue = this.DeleteResult(url, invocation.Method.ReturnType);
                return;
            }

            object value = null;
            if (this.Method == HttpMethod.Post || this.Method == HttpMethod.Put)
            {
                // Take the first invocation argument if available to post.
                if (!invocation.Arguments.IsNullOrEmpty())
                {
                    value = invocation.Arguments[0];
                }
            }

            if (this.Method == HttpMethod.Post)
            {
                invocation.ReturnValue = this.PostResult(url, value, invocation.Method.ReturnType);
            }

            if (this.Method == HttpMethod.Put)
            {
                invocation.ReturnValue = this.PutResult(url, value, invocation.Method.ReturnType);
            }
        }

        #endregion Methods
    }
}