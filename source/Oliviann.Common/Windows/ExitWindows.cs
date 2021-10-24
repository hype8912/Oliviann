namespace Oliviann.Windows
{
    #region Usings

    using System;

    #endregion Usings

    /// <summary>
    /// The shutdown type.
    /// </summary>
    [Flags]
    public enum ExitWindows : uint
    {
        /// <summary>
        /// Shuts down all processes running in the logon session of the process
        /// that called the ExitWindowsEx function. Then it logs the user off.
        /// </summary>
        LogOff = 0x00,

        /// <summary>
        /// Shuts down the system to a point at which it is safe to turn off the
        /// power.
        /// </summary>
        ShutDown = 0x01,

        /// <summary>
        /// Shuts down the system and then restarts the system.
        /// </summary>
        Reboot = 0x02,

        /// <summary>
        /// Shuts down the system and turns off the power.
        /// </summary>
        PowerOff = 0x08,

        /// <summary>
        /// Shuts down the system and then restarts it, as well as any
        /// applications that have been registered for restart using the
        /// RegisterApplicationRestart function.
        /// </summary>
        RestartApps = 0x40,

        /// <summary>
        /// This flag has no effect if terminal services is enabled. Otherwise,
        /// the system does not send the WM_QUERYENDSESSION message. This can
        /// cause applications to lose data. Therefore, you should only use this
        /// flag in an emergency.
        /// </summary>
        Force = 0x04,

        /// <summary>
        /// Forces processes to terminate if they do not respond to the
        /// WM_QUERYENDSESSION or WM_ENDSESSION message within the timeout
        /// interval.
        /// </summary>
        ForceIfHung = 0x10,
    }
}