namespace Oliviann.Windows
{
    #region Usings

    using System;

    #endregion Usings

    /// <summary>
    /// Major reason flags. They indicate the general issue type.
    /// </summary>
    [Flags]
    public enum ShutdownReason : uint
    {
        #region MAJOR REASON FLAGS

        /// <summary>
        /// Application issue.
        /// </summary>
        MajorApplication = 0x00040000,

        /// <summary>
        /// Hardware issue.
        /// </summary>
        MajorHardware = 0x00010000,

        /// <summary>
        /// The InitiateSystemShutdown function was used instead of
        /// InitiateSystemShutdownEx.
        /// </summary>
        MajorLegacyApi = 0x00070000,

        /// <summary>
        /// Operating system issue.
        /// </summary>
        MajorOperatingSystem = 0x00020000,

        /// <summary>
        /// Other issue.
        /// </summary>
        MajorOther = 0x00000000,

        /// <summary>
        /// Power failure.
        /// </summary>
        MajorPower = 0x00060000,

        /// <summary>
        /// Software issue.
        /// </summary>
        MajorSoftware = 0x00030000,

        /// <summary>
        /// System failure.
        /// </summary>
        MajorSystem = 0x00050000,

        #endregion MAJOR REASON FLAGS

        #region MINOR REASON FLAGS

        /// <summary>
        /// Blue screen crash event.
        /// </summary>
        MinorBlueScreen = 0x0000000F,

        /// <summary>
        /// Cord unplugged.
        /// </summary>
        MinorCordUnplugged = 0x0000000b,

        /// <summary>
        /// Disk issue.
        /// </summary>
        MinorDisk = 0x00000007,

        /// <summary>
        /// Environment issue.
        /// </summary>
        MinorEnvironment = 0x0000000c,

        /// <summary>
        /// Hardware driver issue.
        /// </summary>
        MinorHardwareDriver = 0x0000000d,

        /// <summary>
        /// Hot fix.
        /// </summary>
        MinorHotfix = 0x00000011,

        /// <summary>
        /// Hot fix uninstallation.
        /// </summary>
        MinorHotfixUninstall = 0x00000017,

        /// <summary>
        /// Application unresponsive.
        /// </summary>
        MinorHung = 0x00000005,

        /// <summary>
        /// Application installation.
        /// </summary>
        MinorInstallation = 0x00000002,

        /// <summary>
        /// System maintenance.
        /// </summary>
        MinorMaintenance = 0x00000001,

        /// <summary>
        /// MMC issue.
        /// </summary>
        MinorMMC = 0x00000019,

        /// <summary>
        /// Network connectivity.
        /// </summary>
        MinorNetworkConnectivity = 0x00000014,

        /// <summary>
        /// Network card.
        /// </summary>
        MinorNetworkCard = 0x00000009,

        /// <summary>
        /// Other issue.
        /// </summary>
        MinorOther = 0x00000000,

        /// <summary>
        /// Other driver event.
        /// </summary>
        MinorOtherDriver = 0x0000000e,

        /// <summary>
        /// Power supply.
        /// </summary>
        MinorPowerSupply = 0x0000000a,

        /// <summary>
        /// Processor event.
        /// </summary>
        MinorProcessor = 0x00000008,

        /// <summary>
        /// Reconfigure system.
        /// </summary>
        MinorReconfig = 0x00000004,

        /// <summary>
        /// Security issue.
        /// </summary>
        MinorSecurity = 0x00000013,

        /// <summary>
        /// Security patch.
        /// </summary>
        MinorSecurityFix = 0x00000012,

        /// <summary>
        /// Security patch uninstallation.
        /// </summary>
        MinorSecurityFixUninstall = 0x00000018,

        /// <summary>
        /// Service pack.
        /// </summary>
        MinorServicePack = 0x00000010,

        /// <summary>
        /// Service pack uninstallation.
        /// </summary>
        MinorServicePackUninstall = 0x00000016,

        /// <summary>
        /// Terminal Services.
        /// </summary>
        MinorTermSrv = 0x00000020,

        /// <summary>
        /// System unstable.
        /// </summary>
        MinorUnstable = 0x00000006,

        /// <summary>
        /// System upgrade.
        /// </summary>
        MinorUpgrade = 0x00000003,

        /// <summary>
        /// WMI issue.
        /// </summary>
        MinorWMI = 0x00000015,

        #endregion MINOR REASON FLAGS

        /// <summary>
        /// The reason code is defined by the user.
        /// </summary>
        FlagUserDefined = 0x40000000,

        /// <summary>
        /// The shutdown was planned.
        /// </summary>
        FlagPlanned = 0x80000000
    }
}