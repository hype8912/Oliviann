#if !NETSTANDARD1_3

namespace Oliviann.ServiceProcess
{
    #region Usings

    using System;
    using System.ServiceProcess;

    #endregion

    /// <summary>
    /// Represents a proxy class for a service controller.
    /// </summary>
    public class ServiceControllerProxy : IServiceController
    {
        #region Fields

        /// <summary>
        /// Place holder for the controller instance.
        /// </summary>
        private readonly ServiceController _controller;

        #endregion

        #region Constructor/Destructor

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="ServiceControllerProxy"/> class.
        /// </summary>
        /// <param name="serviceName">The name of the service.</param>
        public ServiceControllerProxy(string serviceName)
        {
            ADP.CheckArgumentNullOrEmpty(serviceName, nameof(serviceName));
            this._controller = new ServiceController(serviceName);
        }

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="ServiceControllerProxy"/> class.
        /// </summary>
        /// <param name="controller">The controller instance.</param>
        public ServiceControllerProxy(ServiceController controller)
        {
            ADP.CheckArgumentNull(controller, nameof(controller));
            this._controller = controller;
        }

        #endregion

        #region Properties

        /// <inheritdoc />
        public ServiceControllerStatus Status => this._controller.Status;

        #endregion

        #region Static Methods

        /// <summary>
        /// Executes a service function and returns a boolean value.
        /// </summary>
        /// <param name="serviceName">Name of the service.</param>
        /// <param name="serviceFunc">The service function.</param>
        /// <returns>A result representing the executed function.</returns>
        public static bool AsService(string serviceName, Func<IServiceController, bool> serviceFunc)
        {
            if (serviceName.IsNullOrEmpty() || serviceFunc == null)
            {
                return false;
            }

            using (var controller = new ServiceControllerProxy(serviceName))
            {
                return serviceFunc(controller);
            }
        }

        /// <summary>
        /// Determines whether the specified windows service is running.
        /// </summary>
        /// <param name="serviceName">Name of the service.</param>
        /// <returns>
        /// True if the specified windows service is running; otherwise, false.
        /// </returns>
        public static bool IsServiceRunning(string serviceName) => AsService(serviceName, c => c.IsRunning());

        /// <summary>
        /// Determines whether the specified windows service is valid service
        /// name on this machine.
        /// </summary>
        /// <param name="serviceName">Name of the service.</param>
        /// <returns>
        /// True if the specified windows service is a valid service
        /// name; otherwise, false.
        /// </returns>
        public static bool IsValidService(string serviceName) => AsService(serviceName, c => c.IsValid());

        /// <summary>
        /// Restarts the specified windows service using the specified start up
        /// arguments.
        /// </summary>
        /// <param name="serviceName">Name of the service.</param>
        /// <param name="statusTimeout">A <see cref="System.TimeSpan"/> object
        /// specifying the amount of time to wait for the service to reach the
        /// specified status.</param>
        /// <param name="arguments">The service start up arguments.</param>
        /// <returns>
        /// True if the specified windows service is running; otherwise, false.
        /// </returns>
        public static bool Restart(string serviceName, TimeSpan statusTimeout = default, params string[] arguments) =>
            AsService(serviceName, c => c.Restart(statusTimeout, arguments));

        /// <summary>
        /// Starts the specified windows service if it is currently stopped
        /// using the specified start up arguments.
        /// </summary>
        /// <param name="serviceName">Name of the service.</param>
        /// <param name="statusTimeout">Optional. A
        /// <see cref="System.TimeSpan"/> object specifying the amount of time
        /// to wait for the service to reach the specified status.</param>
        /// <param name="arguments">The service start up arguments.</param>
        /// <returns>
        /// True if the specified windows service is running; otherwise, false.
        /// </returns>
        public static bool Start(string serviceName, TimeSpan statusTimeout = default(TimeSpan), params string[] arguments)
            => AsService(serviceName, c => c.StartSafe(statusTimeout, arguments));

        /// <summary>
        /// Stops the specified windows service if it is not currently stopped.
        /// </summary>
        /// <param name="serviceName">Name of the service.</param>
        /// <param name="statusTimeout">Optional. A
        /// <see cref="System.TimeSpan"/> object specifying the amount of time
        /// to wait for the service to reach the specified status.</param>
        /// <returns>
        /// True if the specified windows service is stopped; otherwise, false.
        /// </returns>
        public static bool Stop(string serviceName, TimeSpan statusTimeout = default(TimeSpan)) =>
            AsService(serviceName, c => c.StopSafe(statusTimeout));

        #endregion

        #region Methods

        /// <inheritdoc />
        public void Dispose() => this._controller.DisposeSafe();

        /// <inheritdoc />
        public void Refresh() => this._controller.Refresh();

        /// <inheritdoc />
        public void Start() => this._controller.Start();

        /// <inheritdoc />
        public void Start(string[] args)  => this._controller.Start(args);

        /// <inheritdoc />
        public void Stop() => this._controller.Stop();

        /// <inheritdoc />
        public void WaitForStatus(ServiceControllerStatus desiredStatus, TimeSpan timeout) =>
            this._controller.WaitForStatus(desiredStatus, timeout);

        #endregion
    }
}

#endif