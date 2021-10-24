#if !NETSTANDARD1_3

namespace Oliviann.ServiceProcess
{
    #region Usings

    using System;
    using System.ServiceProcess;

    #endregion

    /// <summary>
    /// Represents a collection of commonly used extension methods for extending
    /// <see cref="IServiceController"/>.
    /// </summary>
    public static class IServiceControllerExtensions
    {
        #region Checks

        /// <summary>
        /// Determines whether the specified windows service is running.
        /// </summary>
        /// <param name="controller">The service controller instance.</param>
        /// <returns>
        /// True if the specified windows service is running; otherwise, false.
        /// </returns>
        public static bool IsRunning(this IServiceController controller)
        {
            return controller.IsValid() && controller.Status == ServiceControllerStatus.Running;
        }

        /// <summary>
        /// Determines whether the current service is a valid windows service.
        /// </summary>
        /// <param name="controller">The service controller instance.</param>
        /// <returns>
        /// True if the current service is a valid windows service;
        /// otherwise, false.
        /// </returns>
        public static bool IsValid(this IServiceController controller)
        {
            if (controller == null)
            {
                return false;
            }

            bool resultValue;
            try
            {
                _ = controller.Status;
                resultValue = true;
            }
            catch (Exception)
            {
                // Invalid service on this machine.
                resultValue = false;
            }

            return resultValue;
        }

        #endregion

        #region Restarts

        /// <summary>
        /// Restarts the specified windows service using the specified start up
        /// arguments.
        /// </summary>
        /// <param name="controller">The service controller instance.</param>
        /// <param name="statusTimeout">A <see cref="System.TimeSpan"/> object
        /// specifying the amount of time to wait for the service to reach the
        /// specified status.</param>
        /// <param name="arguments">The service start up arguments.</param>
        /// <returns>
        /// True if the specified windows service is running; otherwise, false.
        /// </returns>
        public static bool Restart(this IServiceController controller, TimeSpan statusTimeout = default, params string[] arguments)
        {
            TimeSpan timeout = statusTimeout == default(TimeSpan) ? TimeSpan.FromSeconds(30D) : statusTimeout;

            // Attempt to stop the service if it isn't already stopped.
            StopSafe(controller, timeout);

            // Attempt to start the service if it is stopped.
            return StartSafe(controller, timeout, arguments);
        }

        #endregion

        #region Starts

        /// <summary>
        /// Starts the specified windows service if it is currently stopped
        /// using the specified start up arguments.
        /// </summary>
        /// <param name="controller">The controller instance.</param>
        /// <param name="statusTimeout">Optional. A
        /// <see cref="System.TimeSpan"/> object specifying the amount of time
        /// to wait for the service to reach the specified status.</param>
        /// <param name="arguments">The service start up arguments.</param>
        /// <returns>
        /// True if the specified windows service is running; otherwise, false.
        /// </returns>
        public static bool StartSafe(
            this IServiceController controller,
            TimeSpan statusTimeout = default(TimeSpan),
            params string[] arguments)
        {
            if (controller == null || !controller.IsValid())
            {
                return false;
            }

            if (controller.Status == ServiceControllerStatus.Running ||
                controller.Status == ServiceControllerStatus.StartPending)
            {
                return true;
            }

            bool serviceStarted;

            try
            {
                // Attempt to start the service.
                controller.Start(arguments);

                // Wait for the status of the service to change.
                controller.WaitForStatusOpt(ServiceControllerStatus.Running, statusTimeout);

                // Display the current service status.
                controller.Refresh();
                serviceStarted = true;
            }
            catch (InvalidOperationException)
            {
                serviceStarted = false;
            }

            return serviceStarted;
        }

        #endregion

        #region Stops

        /// <summary>
        /// Stops the specified windows service if it is not currently stopped.
        /// </summary>
        /// <param name="controller">The controller instance.</param>
        /// <param name="statusTimeout">Optional. A
        /// <see cref="System.TimeSpan"/> object specifying the amount of time
        /// to wait for the service to reach the specified status.</param>
        /// <returns>
        /// True if the specified windows service is stopped; otherwise, false.
        /// </returns>
        public static bool StopSafe(this IServiceController controller, TimeSpan statusTimeout = default(TimeSpan))
        {
            if (controller == null || !controller.IsValid())
            {
                return false;
            }

            if (controller.Status == ServiceControllerStatus.Stopped ||
                controller.Status == ServiceControllerStatus.StopPending)
            {
                return true;
            }

            // Stop the service since the status is currently not stopped.
            bool serviceStopped = true;

            try
            {
                // Attempt to stop the service.
                controller.Stop();

                // Wait for the status of the service to change.
                controller.WaitForStatusOpt(ServiceControllerStatus.Stopped, statusTimeout);

                // Display the current service status.
                controller.Refresh();
            }
            catch (InvalidOperationException)
            {
                serviceStopped = false;
            }

            return serviceStopped;
        }

        #endregion

        #region Waits

        /// <summary>
        /// Waits for the service to reach the specified status until the
        /// optional timeout is reached or for infinity.
        /// </summary>
        /// <param name="controller">The service controller instance.</param>
        /// <param name="desiredStatus">The status to wait for.</param>
        /// <param name="timeout">Optional. The <see cref="System.TimeSpan"/>
        /// object specifying the amount of time to wait for the service to
        /// reach the specified status.</param>
        public static void WaitForStatusOpt(
            this IServiceController controller,
            ServiceControllerStatus desiredStatus,
            TimeSpan timeout = default(TimeSpan))
        {
            if (controller == null)
            {
                return;
            }

            if (timeout == default(TimeSpan))
            {
                timeout = TimeSpan.MaxValue;
            }

            controller.WaitForStatus(desiredStatus, timeout);
        }

        #endregion
    }
}

#endif