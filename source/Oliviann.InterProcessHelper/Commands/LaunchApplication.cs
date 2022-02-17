namespace Oliviann.InterProcessHelper.Commands
{
    #region Usings

    using System;
    using System.Diagnostics;
    using Oliviann.Diagnostics;
    using Oliviann.IPC;

    #endregion Usings

    /// <summary>
    /// Represents a class for launching an application in a 32-Bit process.
    /// </summary>
    public static class LaunchApplication
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

            // Launch the process.
            ProcessProxy proc = null;

            try
            {
                proc = new ProcessProxy { StartInfo = { Arguments = processData.Data2, FileName = processData.Data1 } };

                proc.Start();
                proc.WaitForExit();
            }
            catch (Exception ex)
            {
                Arguments.Source.TraceEvent(
                    TraceEventType.Error,
                    6002,
                    $"An error occurred launching process: {processData.Data1 + processData.Data2}{Environment.NewLine}Error:{ex}");
            }
            finally
            {
                proc.DisposeSafe();
            }
        }
    }
}