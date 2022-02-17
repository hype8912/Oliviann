namespace Oliviann.IO.Compression
{
    #region Usings

    using System.Linq;
    using System.Text;

    #endregion Usings

    /// <summary>
    /// Represents a compression command line builder for a create operation.
    /// </summary>
    public class AddCLIBuilder : ICompressionCLIBuilder
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
            switch (arguments.Format)
            {
                case CompressionFormat.GZip:
                    return this.IsValidGZipArguments(arguments);

                case CompressionFormat.SevenZip:
                    return this.IsValid7zArguments(arguments);

                default:
                    return true;
            }
        }

        /// <summary>
        /// Builds the command line arguments to be passed to the compression
        /// application.
        /// </summary>
        /// <param name="arguments">The command arguments.</param>
        /// <returns>A string of arguments to be passed to the compression
        /// application.</returns>
        public string BuildCommandArguments(CompressionArguments arguments)
        {
            var builder = new StringBuilder("a");

            // -ssw (Compress files open for writing)
            if (arguments.IncludeOpenFiles)
            {
                builder.Append(@" -ssw");
            }

            // -p (Password)
            if (!arguments.Password.IsNullOrEmpty())
            {
                builder.Append(" -p").Append(arguments.Password);
            }

            // -t (Type of Archive)
            if (!arguments.ArchiveType.IsNullOrEmpty())
            {
                builder.Append(" -t").Append(arguments.ArchiveType);
            }

            // The archive file path to be created.
            builder.Append(" \"").Append(arguments.ArchiveFilePath).Append("\"");

            // The folder path to be archived.
            builder.Append(" \"").Append(arguments.IncludeFileNames.FirstOrDefault()).Append("\"");

            switch (arguments.Format)
            {
                case CompressionFormat.GZip:
                    this.BuildGZipCommandArguments(builder, arguments);
                    break;

                case CompressionFormat.SevenZip:
                    this.Build7zCommandArguments(builder, arguments);
                    break;
            }

            // -w (set Working directory)
            if (!arguments.WorkingDirectory.IsNullOrEmpty())
            {
                builder.Append(" -w").Append("\"").Append(arguments.WorkingDirectory).Append("\"");
            }

            return builder.ToString();
        }

        #endregion Methods

        #region Internals Validations

        /// <summary>
        /// Determines whether the specified arguments for creating a GZip
        /// compression package are valid.
        /// </summary>
        /// <param name="args">The compression arguments.</param>
        /// <returns>
        /// <c>true</c> if the specified arguments are valid; otherwise,
        /// <c>false</c>.
        /// </returns>
        private bool IsValidGZipArguments(CompressionArguments args)
        {
            if (args.AdvancedOptions.Level == CompressionLevel.None)
            {
                args.AdvancedOptions.Level = CompressionLevel.Fastest;
            }

            if (!args.AdvancedOptions.FastBytesNumber.IsBetweenOrEqual(3, 258))
            {
                // Sets it to the default value if it's out of the correct range.
                args.AdvancedOptions.FastBytesNumber = 32;
            }

            if (!args.AdvancedOptions.NumberOfPasses.IsBetweenOrEqual(1, 15))
            {
                args.AdvancedOptions.NumberOfPasses = 1;
            }

            return true;
        }

        /// <summary>
        /// Determines whether the specified arguments for creating a 7Zip
        /// compression package are valid.
        /// </summary>
        /// <param name="args">The compression arguments.</param>
        /// <returns>
        /// <c>true</c> if the specified arguments are valid; otherwise,
        /// <c>false</c>.
        /// </returns>
        private bool IsValid7zArguments(CompressionArguments args)
        {
            if (!args.AdvancedOptions.FastBytesNumber.IsBetweenOrEqual(5, 273) && args.AdvancedOptions.FastBytesNumber != 0)
            {
                args.AdvancedOptions.FastBytesNumber = 32;
            }

            return true;
        }

        #endregion Internals Validations

        #region Internal Builders

        /// <summary>
        /// Builds the command line arguments for the GZip compression engine.
        /// </summary>
        /// <param name="builder">The string builder instance.</param>
        /// <param name="args">The input compression arguments.</param>
        private void BuildGZipCommandArguments(StringBuilder builder, CompressionArguments args)
        {
            // -mx=
            if (args.AdvancedOptions.Level != CompressionLevel.Normal)
            {
                builder.Append(@" -mx=").Append((int)args.AdvancedOptions.Level);
            }

            // -mfb=
            if (args.AdvancedOptions.FastBytesNumber != 32)
            {
                builder.Append(@" -mfb=").Append(args.AdvancedOptions.FastBytesNumber);
            }

            // -mpass=
            if (args.AdvancedOptions.NumberOfPasses != 1)
            {
                builder.Append(@" -mpass=").Append(args.AdvancedOptions.NumberOfPasses);
            }
        }

        /// <summary>
        /// Builds the 7zip specific command arguments.
        /// </summary>
        /// <param name="builder">The string builder instance.</param>
        /// <param name="args">The input compression arguments.</param>
        private void Build7zCommandArguments(StringBuilder builder, CompressionArguments args)
        {
            // -mx=
            if (args.AdvancedOptions.Level != CompressionLevel.Normal)
            {
                builder.Append(@" -mx=").Append((int)args.AdvancedOptions.Level);
            }

            // -ms=
            if (args.AdvancedOptions.SolidMode.Enabled)
            {
                this.BuildSolidModeCommandArguments(builder, args);
            }
            else
            {
                builder.Append(" -ms=off");
            }

            // -mmt=
            if (!args.AdvancedOptions.EnableMultithreading)
            {
                builder.Append(@" -mmt=off");
            }
            else if (args.AdvancedOptions.MultithreadThreads.HasValue)
            {
                builder.Append(@" -mmt=").Append(args.AdvancedOptions.MultithreadThreads.Value);
            }

            // -mhc=
            if (!args.AdvancedOptions.EnableArchiveHeaderCompression)
            {
                builder.Append(@" -mhc=off");
            }

            // -mhe=
            if (args.AdvancedOptions.EnableArchiveHeaderEncryption)
            {
                builder.Append(@" -mhe=on");
            }

            switch (args.AdvancedOptions.Method)
            {
                case CompressionMethod.LZMA:
                case CompressionMethod.LZMA2:
                    this.BuildLZMACommandArguments(builder, args);
                    break;
            }
        }

        /// <summary>
        /// Builds the solid mode command arguments.
        /// </summary>
        /// <param name="builder">The string builder instance.</param>
        /// <param name="args">The input compression arguments.</param>
        private void BuildSolidModeCommandArguments(StringBuilder builder, CompressionArguments args)
        {
            if (args.AdvancedOptions.SolidMode.BlockFileCountLimit != 0 && args.AdvancedOptions.SolidMode.BlockSizeLimit.Bytes != 0)
            {
                builder.Append(" -ms=").Append(args.AdvancedOptions.SolidMode.BlockFileCountLimit).Append("f")
                    .Append(args.AdvancedOptions.SolidMode.BlockSizeLimit.Bytes).Append("b");
            }
            else if (args.AdvancedOptions.SolidMode.BlockFileCountLimit != 0)
            {
                builder.Append(" -ms=").Append(args.AdvancedOptions.SolidMode.BlockFileCountLimit).Append("f");
            }
            else if (args.AdvancedOptions.SolidMode.BlockSizeLimit.Bytes != 0)
            {
                builder.Append(" -ms=").Append(args.AdvancedOptions.SolidMode.BlockSizeLimit.Bytes).Append("b");
            }
        }

        /// <summary>
        /// Builds the LZMA specific command arguments.
        /// </summary>
        /// <param name="builder">The string builder instance.</param>
        /// <param name="args">The input compression arguments.</param>
        private void BuildLZMACommandArguments(StringBuilder builder, CompressionArguments args)
        {
            if (args.AdvancedOptions.DictionarySize.Bytes != 0 ||
                args.AdvancedOptions.FastBytesNumber != 32)
            {
                builder.Append(" -m0=LZMA");
            }

            // Set Dictionary size.
            if (args.AdvancedOptions.DictionarySize.Bytes != 0)
            {
                builder.Append(":d=");
                builder.Append(args.AdvancedOptions.DictionarySize.Bytes);
                builder.Append("b");
            }

            // Set number of Fast Bytes.
            if (args.AdvancedOptions.FastBytesNumber != 32)
            {
                builder.Append(@":fb=").Append(args.AdvancedOptions.FastBytesNumber);
            }
        }

        #endregion Internal Builders
    }
}