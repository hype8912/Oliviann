namespace Oliviann.IO.Compression
{
    /// <summary>
    /// Represents the level of compression to be used.
    /// </summary>
    public enum CompressionLevel
    {
        /// <summary>
        /// Copy mode, no compression.
        /// </summary>
        None = 0,

        /// <summary>
        /// Fastest compression method.
        /// </summary>
        Fastest = 1,

        /// <summary>
        /// Fast compression method.
        /// </summary>
        Fast = 3,

        /// <summary>
        /// Normal compression method.
        /// </summary>
        Normal = 5,

        /// <summary>
        /// Maximum compression method.
        /// </summary>
        Maximum = 7,

        /// <summary>
        /// Ultra compression method.
        /// </summary>
        Ultra = 9
    }
}