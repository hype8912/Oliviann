namespace Oliviann.Security.Cryptography
{
    #region Usings

    using System;
    using System.Security;
    using System.Security.Cryptography;

    #endregion Usings

    /// <summary>
    /// Represents a collection of commonly used extension methods for extending
    /// <see cref="Aes"/> providers.
    /// </summary>
    public static class AesExtensions
    {
        #region Methods

        /// <summary>
        /// Decrypts an encrypted string and returns it back as a
        /// <see cref="SecureString" /> using FIPS compliant encryption. Allows
        /// for escaped text by wrapping string with single quotes. Default text
        /// converter is <c>Base64String</c>.
        /// </summary>
        /// <param name="provider">The crypto service provider instance.</param>
        /// <param name="encryptedText">The encrypted text to be decrypted.
        /// </param>
        /// <returns>
        /// The decrypted string as a <see cref="SecureString" />.
        /// </returns>
        [Obsolete("Use SymmetricAlgorithmExtensions.DecryptStringSecure instead.", true)]
        public static SecureString DecryptStringSecure(this Aes provider, string encryptedText) =>
            provider.DecryptStringSecure(encryptedText, Convert.FromBase64String);

        /// <summary>
        /// Decrypts an encrypted string and returns it back as a
        /// <see cref="SecureString" /> using FIPS compliant encryption. Allows
        /// for escaped text by wrapping string with single quotes.
        /// </summary>
        /// <param name="provider">The crypto service provider instance.</param>
        /// <param name="encryptedText">The encrypted text to be decrypted.
        /// </param>
        /// <param name="textConverter">The custom text converter.</param>
        /// <returns>
        /// The decrypted string as a <see cref="SecureString" />.
        /// </returns>
        [Obsolete("Use SymmetricAlgorithmExtensions.DecryptStringSecure instead.", true)]
        public static SecureString DecryptStringSecure(
            this Aes provider,
            string encryptedText,
            Func<string, byte[]> textConverter) =>
            SymmetricAlgorithmExtensions.DecryptStringSecure(provider, encryptedText, textConverter);

        /// <summary>
        /// Encrypts a <see cref="SecureString" /> for security reasons and
        /// returns it as a string. Uses FIPS compliant AES encryption. Default
        /// text converter is <c>Base64String</c>.
        /// </summary>
        /// <param name="provider">The crypto service provider instance.</param>
        /// <param name="decryptedText">The decrypted text to be encrypted.
        /// </param>
        /// <returns>The encrypted string.</returns>
        [Obsolete("Use SymmetricAlgorithmExtensions.EncryptStringSecure instead.", true)]
        public static string EncryptStringSecure(this Aes provider, SecureString decryptedText) =>
            provider.EncryptStringSecure(decryptedText, Convert.ToBase64String);

        /// <summary>
        /// Encrypts a <see cref="SecureString" /> for security reasons and
        /// returns it as a string. Uses FIPS compliant AES encryption.
        /// </summary>
        /// <param name="provider">The crypto service provider instance.</param>
        /// <param name="decryptedText">The decrypted text to be encrypted.
        /// </param>
        /// <param name="textConverter">The custom text converter.</param>
        /// <returns>The encrypted string.</returns>
        [Obsolete("Use SymmetricAlgorithmExtensions.EncryptStringSecure instead.", true)]
        public static string EncryptStringSecure(
            this Aes provider,
            SecureString decryptedText,
            Func<byte[], string> textConverter) =>
            SymmetricAlgorithmExtensions.EncryptStringSecure(provider, decryptedText, textConverter);

        #endregion Methods
    }
}