namespace Oliviann.IO.Compression
{
    #region Usings

    using System.Linq;
    using System.Text;
    using Collections.Generic;

    #endregion Usings

    /// <summary>
    /// Represents a compression command line builder for an extract operation.
    /// </summary>
    internal class ExtractCLIBuilder : ICompressionCLIBuilder
    {
        #region Methods

        /// <summary>
        /// Determines whether the specified compression arguments are valid.
        /// </summary>
        /// <param name="arguments">The command arguments.</param>
        /// <returns>
        /// <c>true</c> if the specified compression arguments are valid;
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
            var builder = new StringBuilder("x");

            // -aoa, -aos, -aot, -aou
            if (arguments.OverwriteMode.HasValue)
            {
                string overWriteArg = CompressionFactory.GetOverwriteMode(arguments.OverwriteMode.Value);
                if (overWriteArg != null)
                {
                    builder.Append(" ").Append(overWriteArg);
                }
            }

            // -bb[0-3] Output log level
            if ((int)arguments.OutputLogLevel > 0)
            {
                builder.Append(" -bb").Append((int)arguments.OutputLogLevel);
            }

            // -p (Password)
            if (!arguments.Password.IsNullOrEmpty())
            {
                builder.Append(" -p").Append(arguments.Password);
            }

            // -ssc (Set Sensitive Case Mode)
            if (arguments.CaseSensitiveMode)
            {
                builder.Append(@" -ssc");
            }

            // -o (Output Directory)
            if (!arguments.OutputDirectoryPath.IsNullOrEmpty())
            {
                builder.Append(" -o\"").Append(arguments.OutputDirectoryPath).Append("\"");
            }

            // -t (Type of Archive)
            if (!arguments.ArchiveType.IsNullOrEmpty())
            {
                builder.Append(" -t").Append(arguments.ArchiveType);
            }

            // -y (Assume Yes on All Queries)
            if (arguments.AssumeYesOnAllQueries)
            {
                builder.Append(" -y");
            }

            // The archive file path to be extracted.
            builder.Append(" \"").Append(arguments.ArchiveFilePath).Append("\"");

            if (!arguments.IncludeFileNames.IsNullOrEmpty())
            {
                string extractFile = arguments.IncludeFileNames.First();
                if (extractFile.StartsWith('-', true))
                {
                    builder.Append(" --");
                }

                builder.Append(" ").Append(extractFile);
            }

            return builder.ToString();
        }

        #endregion Methods
    }
}