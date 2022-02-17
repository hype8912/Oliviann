namespace Oliviann.IO.Compression
{
    #region Usings

    using System;
    using IO;

    #endregion Usings

    /// <summary>
    /// Represents a collection of advanced options for the compressor engine.
    /// Not all options are used by every compressor engine.
    /// </summary>
    [Serializable]
    public class AdvancedFormatOptions
    {
        #region Constructor/Destructor

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="AdvancedFormatOptions"/> class.
        /// </summary>
        public AdvancedFormatOptions()
        {
            this.Level = CompressionLevel.Normal;
            this.EnableArchiveHeaderCompression = true;
            this.EnableMultithreading = true;
            this.FastBytesNumber = 32;
            this.ModelOrder = 8;
            this.NumberOfPasses = 1;
            this.SolidMode = new SolidBlockMode();
        }

        #endregion Constructor/Destructor

        #region Properties

        /// <summary>
        /// Gets or sets the compression level. Default value is Normal.
        /// </summary>
        /// <value>
        /// The compression level.
        /// </value>
        /// <example>
        /// Modes: Zip, GZip, BZip2, 7z, Xz
        /// Switch: <c>x=[0 | 1 | 3 | 5 | 7 | 9 ]</c>
        /// Default: 5 (Normal)
        /// </example>
        public CompressionLevel Level { get; set; }

        /// <summary>
        /// Gets or sets the size of the dictionary.
        /// </summary>
        /// <value>
        /// The size of the dictionary.
        /// </value>
        /// <example>
        /// Modes: Zip, BZip2, LZMA, LZMA2
        /// Switch: d={Size}[b|k|m]
        /// Default: Zip, BZip2 = 900000b
        ///          LZMA, LZMA2 = 24 (16 MB)
        /// Range: Zip, BZip = 100000b - 900000b
        /// </example>
        public FileSize DictionarySize { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to enable archive header
        /// compression. The default value is true.
        /// </summary>
        /// <value>
        /// true if to enable archive header compression; otherwise, false.
        /// </value>
        /// <example>
        /// Modes: 7z
        /// Switch: hc=[off | on]
        /// Default: on
        /// </example>
        public bool EnableArchiveHeaderCompression { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether enable archive header
        /// encryption. Default value is false.
        /// </summary>
        /// <value>
        /// true if enable archive header encryption; otherwise, false.
        /// </value>
        /// <example>
        /// Modes: 7z
        /// Switch: he=[off | on]
        /// Default: off
        /// </example>
        public bool EnableArchiveHeaderEncryption { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether enable or disable
        /// multithreading. Each thread in the multithread mode uses 32 MB of
        /// RAM for buffering. Default value is true.
        /// </summary>
        /// <value>
        /// <c>true</c> if to enable multithreading; otherwise, <c>false</c>.
        /// </value>
        /// <example>
        /// Mode: Zip, BZip2, 7z, Xz
        /// Switch: mt=[off | on | {N}]
        /// Default: on
        /// </example>
        public bool EnableMultithreading { get; set; }

        /// <summary>
        /// Gets or sets the number of threads to use when multithreading is
        /// enabled. Each thread in the multithread mode uses 32 MB of RAM for
        /// buffering.
        /// </summary>
        /// <value>
        /// The number of threads to use when multithreading is enabled.
        /// </value>
        /// <example>
        /// Mode: Zip, BZip2, 7z, Xz
        /// </example>
        public int? MultithreadThreads { get; set; }

        ////public bool EnableNonAsciiFileNames { get; set; }

        /// <summary>
        /// Gets or sets the type of the encryption to be used when a password
        /// has been provided. Default value is
        /// <see cref="EncryptionMethod.ZipCrypto"/>.
        /// </summary>
        /// <value>
        /// The type of the encryption.
        /// </value>
        /// <example>
        /// Mode: Zip, BZip2
        /// Switch: em={EncryptionMethodID}
        /// Default: ZipCrypto
        /// </example>
        public EncryptionMethod? EncryptionType { get; set; }

        /// <summary>
        /// Gets or sets the number of fast bytes for the specific compression
        /// engine. Also known as the word size in the 7zGui. Default value is 32.
        /// </summary>
        /// <value>
        /// The number of fast bytes.
        /// </value>
        /// <example>
        /// Modes: Zip, GZip, LZMA, LZMA2
        /// Switch: fb={NumFastBytes}
        /// Default: 32
        /// Range: Zip, GZip = 3 - 257 (258 Deflate64)
        ///        LZMA, LZMA2 = 5 - 273
        /// </example>
        public int FastBytesNumber { get; set; }

        /// <summary>
        /// Gets or sets the file chunk size. If you don't specify a chunk size,
        /// LZMA2 sets it to <see cref="DictionarySize"/> times 4. LZMA2 uses 1
        /// thread when <see cref="Level"/> is set to 1 or 3; and 2 threads for
        /// each chunk when <see cref="Level"/> is set to 5, 7, or 9.
        /// </summary>
        /// <value>
        /// The size of the file chunk.
        /// </value>
        /// <example>
        /// Mode: 7z:LZMA2
        /// Switch: c={Size}[b|k|m]
        /// Default: DictionarySize * 4
        /// </example>
        public FileSize FileChunkSize { get; set; }

        //// public FileSize MemorySize { get; set; }

        /// <summary>
        /// Gets or sets the compression method to be used for compressing an
        /// archive.
        /// </summary>
        /// <value>
        /// The compression method to be used for compressing an archive.
        /// </value>
        /// <example>
        /// Modes: Add, Update
        /// Switch: <c>m={MethodID}</c>
        /// </example>
        public CompressionMethod Method { get; set; }

        /// <summary>
        /// Gets or sets the model order for PPMd only. The size must be in the
        /// range from 2 to 16. The default value is 8.
        /// </summary>
        /// <value>
        /// The model order.
        /// </value>
        /// <example>
        /// Mode: Zip
        /// Switch: o={Size}
        /// Default: 8
        /// Range: 2 - 16
        /// </example>
        public int ModelOrder { get; set; }

        /// <summary>
        /// Gets or sets the number of passes. Default value is 1.
        /// </summary>
        /// <value>
        /// The number of passes.
        /// </value>
        /// <example>
        /// Mode: Zip, GZip, BZip2
        /// Switch: pass={NumPasses}
        /// Default: 1
        /// Range: Zip:Deflate, GZip = 1 -15
        ///        BZip2 = 1 - 10
        /// </example>
        public int NumberOfPasses { get; set; }

        /// <summary>
        /// Gets the solid block mode options.
        /// </summary>
        /// <value>
        /// The solid block mode options.
        /// </value>
        /// <example>
        /// Mode: 7z
        /// Switch: s=[off | on | [e] [{N}f] [{N}b | {N}k | {N}m | {N}g)]
        /// Default: Store   = 0 B
        ///          Fastest = 16 MB
        ///          Fast    = 128 MB
        ///          Normal  = 2 GB
        ///          Maximum = 4 GB
        ///          Ultra   = 4 GB
        /// </example>
        public SolidBlockMode SolidMode { get; private set; }

        ////public bool StoreFileTimestampInformation { get; set; }

        ////public bool StoreFileLastModifiedTimestamps { get; set; }

        ////public bool StoreFileLastAccessTimestamps { get; set; }

        ////public bool UseLocalCodePageFileNames { get; set; }

        #endregion Properties
    }
}