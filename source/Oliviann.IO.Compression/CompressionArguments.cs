namespace Oliviann.IO.Compression
{
    #region Usings

    using System;
    using System.Collections.Generic;

    #endregion Usings

    /// <summary>
    /// Represents a class for defining all the arguments required for the
    /// compression engine.
    /// </summary>
    [Serializable]
    public class CompressionArguments
    {
        #region Constructor/Destructor

        /// <summary>
        /// Initializes a new instance of the <see cref="CompressionArguments"/>
        /// class.
        /// </summary>
        public CompressionArguments()
        {
            this.AssumeYesOnAllQueries = true;
            this.AdvancedOptions = new AdvancedFormatOptions();
            this.Format = CompressionFormat.SevenZip;
        }

        #endregion Constructor/Destructor

        #region Properties

        /// <summary>
        /// Gets or sets the file path location to the 7-zip application.
        /// </summary>
        /// <value>
        /// The file path location to the 7-zip application.
        /// </value>
        public string ApplicationFilePath { get; set; }

        /// <summary>
        /// Gets the archive file path.
        /// </summary>
        /// <value>
        /// The archive file path.
        /// </value>
        public string ArchiveFilePath { get; internal set; }

        /// <summary>
        /// Gets or sets the type of the archive if 7zip cannot automatically
        /// recognize the type. It can be: *, 7z, split, zip, <c>gzip</c>, bzip2,
        /// tar, ..., or combination of them, like: <c>mbr.vhd</c>
        /// </summary>
        /// <value>
        /// The type of the archive.
        /// </value>
        /// <example>
        /// Modes: Add, Delete, Extract, List, Test, Update, Extract Full Paths
        /// Switch: -t{archive_type}
        /// </example>
        public string ArchiveType { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to assume yes on all normal
        /// user displayed queries. Default value is <c>true</c>.
        /// </summary>
        /// <value>
        /// <c>true</c> if to assume yes on all normal user displayed queries.;
        /// otherwise, <c>false</c>.
        /// </value>
        /// <example>
        /// Modes: Extract, Extract Full Paths
        /// Switch: <c>-y</c>
        /// </example>
        public bool AssumeYesOnAllQueries { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether sensitive case mode for
        /// filenames is enabled.
        /// </summary>
        /// <value>
        /// <c>true</c> if case sensitive mode is enabled; otherwise,
        /// <c>false</c>.
        /// </value>
        public bool CaseSensitiveMode { get; set; }

        /// <summary>
        /// Gets the compression command.
        /// </summary>
        /// <value>
        /// The compression command.
        /// </value>
        public CompressionCommand Command { get; internal set; }

        /// <summary>
        /// Gets or sets a value indicating whether disable the parsing of
        /// archive names.
        /// </summary>
        /// <value>
        /// <c>true</c> if to disable the parsing of archive names; otherwise,
        /// <c>false</c>.
        /// </value>
        /// <example>
        /// Modes: Extract, List, Test, Extract Full Paths
        /// Switch: <c>-an</c>
        /// </example>
        public bool DisableArchiveNameParsing { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to encrypt the archive
        /// headers when a password has been specified. This also encrypts the
        /// filenames.
        /// </summary>
        /// <value>
        /// <c>true</c> if to encrypt the archive headers when a password has
        /// been specified; otherwise, <c>false</c>.
        /// </value>
        /// <example>
        /// Modes: Add, Delete, Extract, Test, Update, Extract Full Paths
        /// Switch: <c>-mhe</c>
        /// </example>
        public bool EncryptArchiveHeaders { get; set; }

        /// <summary>
        /// Gets or sets the archives to be excluded from the operation.
        /// </summary>
        /// <value>
        /// The archives to be excluded from the operation.
        /// </value>
        /// <example>
        /// Modes: Extract, List, Test, Extract Full Paths
        /// Switch: <c>-ax[LIST FILE | WILDCARD]</c>
        /// </example>
        public IEnumerable<string> ExcludeArchiveFileNames { get; set; }

        /// <summary>
        /// Gets or sets a collection of filenames or wildcarded names that must
        /// be excluded from the operation.
        /// </summary>
        /// <value>
        /// A collection of filenames or wildcarded names that must be excluded
        /// from the operation.
        /// </value>
        /// <example>
        /// Modes: Add, Delete, Extract, List, Test, Update, Extract Full Paths
        /// Switch: <c>-x[LIST FILE | WILDCARD]</c>
        /// </example>
        public IEnumerable<string> ExcludeFileNames { get; set; }

        /// <summary>
        /// Gets or sets the additional include archive filenames and wildcards.
        /// </summary>
        /// <value>
        /// The additional include archive filenames and wildcards.
        /// </value>
        /// <example>
        /// Modes: All
        /// Switch: <c>-ai</c>
        /// </example>
        public IEnumerable<string> IncludeArchiveFileNames { get; set; }

        /// <summary>
        /// Gets or sets a collection of filenames or wildcarded names that must
        /// be included in the operation.
        /// </summary>
        /// <value>
        /// A collection of filenames or wildcarded names that must be included
        /// in the operation.
        /// </value>
        /// <example>
        /// Modes: Add, Delete, Extract, List, Test, Update, Extract Full Paths
        /// Switch: <c>-i[LIST FILE | WILDCARD]</c>
        /// </example>
        public IEnumerable<string> IncludeFileNames { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether include files that are open
        /// for writing by another application.
        /// </summary>
        /// <value>
        /// <c>true</c> if to include files that are open for writing by another
        /// application; otherwise, <c>false</c>.
        /// </value>
        /// <example>
        /// Modes: Add, Update
        /// Switch: <c>-ssw</c>
        /// </example>
        public bool IncludeOpenFiles { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether recurse subdirectories.
        /// </summary>
        /// <value>
        /// <c>true</c> if to recurse subdirectories; otherwise, <c>false</c>.
        /// </value>
        /// <example>
        /// Modes: Add, Delete, Extract, List, Test, Update, Extract Full Paths
        /// Switch: <c>-r[- | 0]</c>
        /// </example>
        public bool IncludeSubdirectories { get; set; }

        /// <summary>
        /// Gets or sets the format for creating an archive. The default value is 7z.
        /// </summary>
        /// <value>
        /// The compression format to be used.
        /// </value>
        public CompressionFormat Format { get; set; }

        /// <summary>
        /// Gets the collection of advanced compression method options to be
        /// passed directly to the compressor.
        /// </summary>
        /// <value>
        /// The collection of advanced compression method options.
        /// </value>
        public AdvancedFormatOptions AdvancedOptions { get; private set; }

        /// <summary>
        /// Gets or sets the destination directory where the file are to be
        /// extracted.
        /// </summary>
        /// <value>
        /// The destination directory where the file are to be extracted.
        /// </value>
        /// <example>
        /// Modes: Extract, Extract Full Paths
        /// Switch: <c>-o[STRING]</c>
        /// </example>
        public string OutputDirectoryPath { get; set; }

        /// <summary>
        /// Gets or sets the output log level.
        /// </summary>
        /// <value>
        /// The output log level.
        /// </value>
        /// <example>
        /// Modes: 0 (disabled), 1, 2, 3
        /// Switch: <c>-bb[0-3]</c>
        /// </example>
        public CompressionLogLevel OutputLogLevel { get; set; }

        /// <summary>
        /// Gets or sets a value specifying the mode to be used to overwrite
        /// files already present on disk.
        /// </summary>
        /// <value>
        /// A value specifying the mode to be used to overwrite files already
        /// present on disk
        /// </value>
        /// <example>
        /// Modes: Extract, Extract Full Paths
        /// </example>
        public CompressionOverwriteMode? OverwriteMode { get; set; }

        /// <summary>
        /// Gets or sets the archive password.
        /// </summary>
        /// <value>
        /// The archive password.
        /// </value>
        /// <example>
        /// Modes: Add, Delete, Extract, Test, Update, Extract Full Paths
        /// Switch: <c>-p[STRING]</c>
        /// </example>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to output every line
        /// received by the output parser to Trace. Should be used for debugging
        /// problems only. Default value is <c>false</c>.
        /// </summary>
        /// <value>
        /// <c>true</c> if output all trace data; otherwise, <c>false</c>.
        /// </value>
        public bool TraceOutputData { get; set; }

        /// <summary>
        /// Gets or sets the working directory for the temporary base archive
        /// directory path. If not assigned, then the Windows temporary
        /// directory will be used.
        /// </summary>
        /// <value>
        /// The working directory for the temporary base archive.
        /// </value>
        /// <example>
        /// Modes: Add, Delete, Update
        /// Switch: <c>-w[STRING]</c>
        /// </example>
        /// <remarks>
        /// By default, 7-Zip builds a new base archive file in the same
        /// directory as the old base archive file. By specifying this value,
        /// you can set the working directory where the temporary base archive
        /// file will be built. After the temporary base archive file is built,
        /// it is copied over the original archive; then, the temporary file is
        /// deleted.
        /// </remarks>
        public string WorkingDirectory { get; set; }

        #endregion Properties
    }
}