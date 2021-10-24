namespace Oliviann.Windows.Forms.SystemInformation
{
    #region Usings

    using System.Windows.Forms;

    #endregion Usings

    /// <summary>
    /// Represents a helper class for working with
    /// <see cref="System.Windows.Forms.SystemInformation.PowerStatus"/>.
    /// </summary>
    public static class PowerStatusHelpers
    {
        /// <summary>
        /// Determines whether the machine is running on battery power.
        /// </summary>
        /// <returns>
        /// <c>true</c> if this machine is running on battery power; otherwise,
        /// <c>false</c>.
        /// </returns>
        public static bool IsRunningOnBattery() => SystemInformation.PowerStatus.PowerLineStatus == PowerLineStatus.Offline;
    }
}