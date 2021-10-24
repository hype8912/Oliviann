namespace Oliviann.Security.Cryptography
{
    /// <summary>
    /// Represents an interface for implementing a hashing algorithm.
    /// </summary>
    public interface IHashAlgorithm
    {
        /// <summary>
        /// Hashes the specified data.
        /// </summary>
        /// <param name="data">The byte array to be hashed.</param>
        /// <returns>The hash code value for the specified data.</returns>
        uint Hash(byte[] data);
    }
}