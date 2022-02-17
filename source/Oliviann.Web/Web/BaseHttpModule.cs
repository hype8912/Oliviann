#if NETFRAMEWORK

namespace Oliviann.Web
{
    #region Usings

    using System;
    using System.Web;

    #endregion Usings

    /// <summary>
    /// Represents a base http module for implementing a custom HttpModule.
    /// </summary>
    public abstract class BaseHttpModule : IHttpModule
    {
        #region Public Methods

        /// <summary>
        /// Initializes a module and prepares it to handle requests.
        /// </summary>
        /// <param name="context">An <see cref="T:System.Web.HttpApplication" />
        /// that provides access to the methods, properties, and events common
        /// to all application objects within an ASP.NET application</param>
        public void Init(HttpApplication context)
        {
            context.BeginRequest +=
                (s, e) => this.OnBeginRequest(new HttpContextWrapper(((HttpApplication)s).Context), e);
            context.PreRequestHandlerExecute +=
                (s, e) => this.OnPreRequestHandlerExecute(new HttpContextWrapper(((HttpApplication)s).Context), e);
            context.PostReleaseRequestState +=
                (s, e) => this.OnPostReleaseRequestState(new HttpContextWrapper(((HttpApplication)s).Context), e);
        }

        /// <summary>
        /// Disposes of the resources (other than memory) used by the module
        /// that implements <see cref="T:System.Web.IHttpModule" />.
        /// </summary>
        public void Dispose()
        {
            // Nothing to dispose;
        }

        #endregion Public Methods

        #region Virtual Methods

        /// <summary>
        /// Called when the begin request event is raised.
        /// </summary>
        /// <param name="context">The http base context.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the
        /// event data.</param>
        public virtual void OnBeginRequest(HttpContextBase context, EventArgs e)
        {
        }

        /// <summary>
        /// Called when the pre-request handler execute event is raised.
        /// </summary>
        /// <param name="context">The http base context.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the
        /// event data.</param>
        public virtual void OnPreRequestHandlerExecute(HttpContextBase context, EventArgs e)
        {
        }

        /// <summary>
        /// Called when the post release request state event is raised.
        /// </summary>
        /// <param name="context">The http base context.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the
        /// event data.</param>
        public virtual void OnPostReleaseRequestState(HttpContextBase context, EventArgs e)
        {
        }

        #endregion Virtual Methods
    }
}

#endif