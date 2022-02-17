namespace Oliviann.IO.Compression
{
    /// <summary>
    /// Represents a interface for implementing a compression command line
    /// arguments builder.
    /// </summary>
    internal interface ICompressionCLIBuilder
    {
        /// <summary>
        /// Determines whether the specified compression arguments are valid.
        /// </summary>
        /// <param name="arguments">The command arguments.</param>
        /// <returns>
        /// <c>true</c> if the specified compression arguments are valid;
        /// otherwise, <c>false</c>.
        /// </returns>
        bool IsValidArguments(CompressionArguments arguments);

        /// <summary>
        /// Builds the command line arguments to be passed to the compression
        /// application.
        /// </summary>
        /// <param name="arguments">The command arguments.</param>
        /// <returns>A string of arguments to be passed to the compression
        /// application.</returns>
        string BuildCommandArguments(CompressionArguments arguments);
    }
}