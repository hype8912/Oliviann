namespace Oliviann.IO.Compression
{
    /// <summary>
    /// Defines an encryption method.
    /// </summary>
    public enum EncryptionMethod
    {
        /// <summary>
        /// Defines a basic zip encryption method.
        /// </summary>
        ZipCrypto,

        /// <summary>
        /// Defines 128-bit AES encryption method.
        /// </summary>
        AES128,

        /// <summary>
        /// Defines 192-bit AES encryption method.
        /// </summary>
        AES192,

        /// <summary>
        /// Defines 256-bit AES encryption method.
        /// </summary>
        AES256
    }
}