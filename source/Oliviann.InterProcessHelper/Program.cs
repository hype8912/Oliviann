namespace Oliviann.InterProcessHelper
{
    #region usings

    using System;
    using Oliviann.InterProcessHelper.Commands;

    #endregion usings

    /// <summary>
    /// Main entry class for the application.
    /// </summary>
    internal static class Program
    {
        /// <summary>
        /// Mains the specified arguments.
        /// </summary>
        /// <param name="args">The command line arguments.</param>
        internal static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                Environment.Exit(1);
#if DEBUG
                // This is so the unit tests will exit like when run from command line.
                return;
#endif
            }

            ////System.Diagnostics.Debugger.Launch();
            Arguments.SetArguments(args);

            switch (Arguments.Command)
            {
                case @"LaunchIE":
                    LaunchInternetExplorer.Launch(Arguments.Data);
                    break;

                case @"LaunchProcess":
                    LaunchApplication.Launch(Arguments.Data);
                    break;

                default:
                    Environment.Exit(2);
                    break;
            }
        }
    }
}