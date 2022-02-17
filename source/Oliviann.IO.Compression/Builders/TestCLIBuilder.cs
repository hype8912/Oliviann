namespace Oliviann.IO.Compression
{
    #region Usings

    using System.Text;

    #endregion Usings

    /// <summary>
    /// Represents a compression command line builder for a test operation.
    /// </summary>
    internal class TestCLIBuilder : ICompressionCLIBuilder
    {
        #region Methods

        /// <summary>
        /// Determines whether the specified compression arguments are valid.
        /// </summary>
        /// <param name="arguments">The command arguments.</param>
        /// <returns>
        ///   <c>true</c> if the specified compression arguments are valid;
        /// otherwise, <c>false</c>.
        /// </returns>
        public bool IsValidArguments(CompressionArguments arguments)
        {
            return true;
        }

        /// <summary>
        /// Builds the command line arguments to be passed to the compression
        /// application.
        /// </summary>
        /// <param name="arguments">The command arguments.</param>
        /// <returns>
        /// A string of arguments to be passed to the compression
        /// application.
        /// </returns>
        public string BuildCommandArguments(CompressionArguments arguments)
        {
            var builder = new StringBuilder("t");

            // -p (Password)
            if (!arguments.Password.IsNullOrEmpty())
            {
                builder.Append(" -p").Append(arguments.Password);
            }

            // The archive file path to be tested.
            builder.Append(" \"").Append(arguments.ArchiveFilePath).Append("\"");

            return builder.ToString();
        }

        #endregion Methods
    }
}