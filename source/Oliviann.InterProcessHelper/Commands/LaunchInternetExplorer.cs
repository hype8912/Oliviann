namespace Oliviann.InterProcessHelper.Commands
{
    #region Usings

    using Oliviann.IPC;

    #endregion Usings

    /// <summary>
    /// Represents a class to launching the 32-Bit version of Internet Explorer.
    /// </summary>
    public static class LaunchInternetExplorer
    {
        /// <summary>
        /// Launches this instance.
        /// </summary>
        /// <param name="processData">The process data.</param>
        public static void Launch(InterProcessData processData)
        {
            if (processData == null || processData.Data1.IsNullOrEmpty())
            {
                return;
            }

            new Web.WebPost().ToInternetExplorer(processData.Data1, processData.Data2);
        }
    }
}