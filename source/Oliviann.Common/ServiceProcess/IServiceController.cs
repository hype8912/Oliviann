#if !NETSTANDARD1_3

namespace Oliviann.ServiceProcess
{
    #region Usings

    using System;
    using System.ServiceProcess;

    #endregion

    /// <summary>
    /// Represents an interface for implementing a service controller proxy.
    /// </summary>
    public interface IServiceController : IDisposable
    {
        /// <inheritdoc cref="ServiceController.Status"/>
        ServiceControllerStatus Status { get; }

        /// <inheritdoc cref="ServiceController.Refresh"/>
        void Refresh();

        /// <inheritdoc cref="ServiceController.Start()"/>
        void Start();

        /// <inheritdoc cref="ServiceController.Start(string[])"/>
        void Start(string[] args);

        /// <inheritdoc cref="ServiceController.Stop"/>
        void Stop();

        /// <inheritdoc cref="ServiceController.WaitForStatus(ServiceControllerStatus, TimeSpan)"/>
        void WaitForStatus(ServiceControllerStatus desiredStatus, TimeSpan timeout);
    }
}

#endif