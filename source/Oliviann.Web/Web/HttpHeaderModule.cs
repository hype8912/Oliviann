#if NETFRAMEWORK

namespace Oliviann.Web
{
    #region Usings

    using System;
    using System.Collections.Specialized;
    using System.Configuration;
    using System.Web;
    using Oliviann.Collections.Specialized;

    #endregion Usings

    /// <summary>
    /// Represents an HTTP module for injecting custom headers into the request.
    /// </summary>
    /// <remarks>
    /// You can request the variable by either calling
    /// <c>Request.ServerVariables.Get("HTTP_YOURVARIABLENAME")</c> or by
    /// calling <c>Request.Headers["YourVariableName"]</c>.
    /// </remarks>
    public class HttpHeaderModule : HttpParameterBase
    {
        #region Methods

        /// <summary>
        /// Called when the begin request event is raised.
        /// </summary>
        /// <param name="context">The current HTTP base context.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the
        /// event data.</param>
        public override void OnBeginRequest(HttpContextBase context, EventArgs e)
        {
            // Get all the custom headers information.
            NameValueCollection customHeaders = GetCustomParameters(
                ConfigurationManager.AppSettings["HTTP_CustomHeaders"],
                this.GetParameterValue);
            if (customHeaders.Count < 1)
            {
                return;
            }

            // Add the required custom headers to the request headers collection.
            // NOTE: Partially added code to not append headers that already
            // exist but decide not to use it until there is a need.
            ////bool allowDuplicateHeaders = ConfigurationManager.AppSettings["HTTP_AllowDuplicateHeaders"].ToBooleanMatchFalse();
            NameValueCollection headers = context.Request.Headers.SetReadOnly(false);
            ////foreach (string key in customHeaders.AllKeys)
            ////{
            ////    if (headers[key] != null && !allowDuplicateHeaders)
            ////    {
            ////        continue;
            ////    }

                    headers.Add(customHeaders);
            ////}
        }

        #endregion Methods
    }
}

#endif