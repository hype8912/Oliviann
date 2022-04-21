namespace Oliviann.Web.Api.Unity
{
    #region Usings

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Http.Dependencies;
    using global::Unity;
    using global::Unity.Exceptions;

    #endregion

    /// <summary>
    /// Represents a IoC resolver for Unity using Web API.
    /// </summary>
    public class UnityResolver : IDependencyResolver
    {
        #region Fields

        /// <summary>
        /// Private field for holding the container.
        /// </summary>
        private readonly IUnityContainer _unityContainer;

        #endregion

        #region Constructor/Destructor

        /// <summary>
        /// Initializes a new instance of the <see cref="UnityResolver"/> class.
        /// </summary>
        /// <param name="container">The container instance.</param>
        public UnityResolver(IUnityContainer container) => this._unityContainer = container;

        #endregion

        #region Methods

        /// <summary>
        /// Resolves singly registered services that support arbitrary object
        /// creation.
        /// </summary>
        /// <param name="serviceType">The type of the requested service or
        /// object.</param>
        /// <returns>The requested service or object.</returns>
        public object GetService(Type serviceType)
        {
            try
            {
                return this._unityContainer.Resolve(serviceType);
            }
            catch (ResolutionFailedException)
            {
                return null;
            }
        }

        /// <summary>
        /// Resolves multiply registered services.
        /// </summary>
        /// <param name="serviceType">The type of the requested services.
        /// </param>
        /// <returns>The requested services.</returns>
        public IEnumerable<object> GetServices(Type serviceType)
        {
            try
            {
                return this._unityContainer.ResolveAll(serviceType);
            }
            catch (ResolutionFailedException)
            {
                return Enumerable.Empty<object>();
            }
        }

        /// <summary>
        /// Starts a resolution scope.
        /// </summary>
        /// <returns>The dependency scope.</returns>
        public IDependencyScope BeginScope()
        {
            IUnityContainer child = this._unityContainer.CreateChildContainer();
            return new UnityResolver(child);
        }

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
        /// Disposes of the managed and unmanaged resources.
        /// </summary>
        /// <param name="disposing">True if being disposed by user managed code;
        /// otherwise, false if being disposed by the finalizer.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                this._unityContainer?.Dispose();
            }
        }

        #endregion
    }
}