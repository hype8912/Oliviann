namespace Oliviann.IO.Compression
{
    /// <summary>
    /// Defines an additional compression method for the compression format.
    /// </summary>
    public enum CompressionMethod
    {
        /// <summary>
        /// LZ-based algorithm. Improved and optimized version of LZ77
        /// algorithm.
        /// </summary>
        LZMA,

        /// <summary>
        /// LZMA-based algorithm. Improved version of LZMA.
        /// </summary>
        LZMA2,

        /// <summary>
        /// Dmitry Shkarin's PPMdH with small changes.
        /// </summary>
        PPMd,

        /// <summary>
        /// Standard BWT algorithm.
        /// </summary>
        BZip2,

        /// <summary>
        /// LZ + Huffman. Standard LZ77-based algorithm.
        /// </summary>
        Deflate,

        /// <summary>
        /// LZ + Huffman. Standard LZ77-based algorithm.
        /// </summary>
        Deflate64,

        /// <summary>
        /// No compression.
        /// </summary>
        Copy
    }
}