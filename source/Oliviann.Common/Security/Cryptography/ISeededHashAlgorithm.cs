namespace Oliviann.Security.Cryptography
{
    /// <summary>
    /// Represents an interface for implementing a seeded hashing algorithm.
    /// </summary>
    public interface ISeededHashAlgorithm : IHashAlgorithm
    {
        /// <summary>
        /// Hashes the specified data using the specified hash
        /// <paramref name="seed"/> value.
        /// </summary>
        /// <param name="data">The byte array to be hashed.</param>
        /// <param name="seed">The value to seed the hashing algorithm.</param>
        /// <returns>
        /// The hash code value for the specified data.
        /// </returns>
        uint Hash(byte[] data, uint seed);
    }
}