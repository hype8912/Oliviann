#if !NETSTANDARD1_3

namespace Oliviann.ServiceProcess
{
    #region Usings

    using System.ServiceProcess;

    #endregion Usings

    /// <summary>
    /// Represents a collection of commonly used extension methods for extending
    /// <see cref="ServiceController"/>.
    /// </summary>
    public static class ServiceControllerExtensions
    {
        /// <summary>
        /// Converts the service controller instance to an IServiceController.
        /// </summary>
        /// <param name="controller">The controller instance.</param>
        /// <returns>A new implementation of IServiceController.</returns>
        public static IServiceController AsIServiceController(this ServiceController controller) =>
            new ServiceControllerProxy(controller);
    }
}

#endif