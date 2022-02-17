namespace Oliviann.IO.Compression
{
    #region Usings

    using Properties;

    #endregion Usings

    /// <summary>
    /// Represents a compression factory instance.
    /// </summary>
    internal static class CompressionFactory
    {
        /// <summary>
        /// Gets the compression command line builder.
        /// </summary>
        /// <param name="commandType">Type of the compression command.</param>
        /// <returns>A command line builder instance.</returns>
        internal static ICompressionCLIBuilder GetCommandLineBuilder(CompressionCommand commandType)
        {
            switch (commandType)
            {
                case CompressionCommand.Add:
                    return new AddCLIBuilder();

                case CompressionCommand.Extract:
                case CompressionCommand.ExtractFullPaths:
                    return new ExtractCLIBuilder();

                case CompressionCommand.List:
                    return new ListCLIBuilder();

                case CompressionCommand.Test:
                    return new TestCLIBuilder();

                default:
                    return null;
            }
        }

        /// <summary>
        /// Gets the compression parser for parsing the output data.
        /// </summary>
        /// <param name="commandType">Type of the compression command.</param>
        /// <param name="arguments">The compression arguments to be passed to
        /// the parser.</param>
        /// <returns>
        /// A compression output parser instance.
        /// </returns>
        internal static ICompressionParser GetCompressionParser(CompressionCommand commandType, CompressionArguments arguments)
        {
            switch (commandType)
            {
                case CompressionCommand.Add:
                    return new AddParser { Arguments = arguments };

                case CompressionCommand.Extract:
                case CompressionCommand.ExtractFullPaths:
                    return new ExtractParser { Arguments = arguments };

                case CompressionCommand.List:
                    return new ListParser { Arguments = arguments };

                case CompressionCommand.Test:
                    return new TestParser { Arguments = arguments };

                default:
                    return null;
            }
        }

        /// <summary>
        /// Gets the exit code description.
        /// </summary>
        /// <param name="exitCode">The exit code.</param>
        /// <returns>A description for the specified exit code.</returns>
        internal static string GetExitCode(int exitCode)
        {
            switch (exitCode)
            {
                case 0:
                    return Resources.ExitCode0;

                case 1:
                    return Resources.ExitCode1;

                case 2:
                    return Resources.ExitCode2;

                case 7:
                    return Resources.ExitCode7;

                case 8:
                    return Resources.ExitCode8;

                case 255:
                    return Resources.ExitCode255;

                default:
                    return Resources.ExitCodeDefault;
            }
        }

        /// <summary>
        /// Gets the overwrite mode command line argument.
        /// </summary>
        /// <param name="mode">The overwrite mode.</param>
        /// <returns>The overwrite mode command line argument.</returns>
        internal static string GetOverwriteMode(CompressionOverwriteMode mode)
        {
            switch (mode)
            {
                case CompressionOverwriteMode.Overwrite:
                    return @"-aoa";

                case CompressionOverwriteMode.Skip:
                    return @"-aos";

                case CompressionOverwriteMode.RenameExtractedFile:
                    return @"-aou";

                case CompressionOverwriteMode.RenameExistingFile:
                    return @"-aot";

                default: return null;
            }
        }
    }
}