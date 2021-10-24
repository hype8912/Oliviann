namespace Oliviann
{
    #region Usings

    using System.Runtime.InteropServices;

    #endregion

    /// <summary>
    /// Represents a class for providing environment and platform information.
    /// </summary>
    public static class EnvironmentFolders
    {
        /// <summary>
        /// Specifies enumerated constants used to retrieve directory paths to
        /// system special folders missing in the .NET Environment.SpecialFolder
        /// enumeration.
        /// </summary>
        [ComVisible(true)]
        public enum SpecialFolder
        {
            /// <summary>
            /// Internet Explorer folder.
            /// </summary>
            InternetExplorer = 1,

            /// <summary>
            /// Control Panel folder.
            /// </summary>
            ControlPanel = 3,

            /// <summary>
            /// Printers folder.
            /// </summary>
            Printers = 4,

            /// <summary>
            /// Recycle bin folder for system drive.
            /// </summary>
            RecycleBin = 10,

            /// <summary>
            /// Network folder.
            /// </summary>
            NetworkNeighborhood = 18,
        }
    }
}