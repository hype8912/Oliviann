namespace Oliviann.Security.Cryptography
{
    #region Usings

    using System;
    using System.Diagnostics;
    using System.Security;
    using System.Security.Cryptography;
    using System.Text;

    #endregion Usings

    /// <summary>
    /// Represents a collection of commonly used extension methods for extending
    /// <see cref="SymmetricAlgorithm"/> providers.
    /// </summary>
    public static class SymmetricAlgorithmExtensions
    {
        #region Fields

        /// <summary>
        /// Escape character to not decrypt the string.
        /// </summary>
        private const char DecryptEscapeCharacter = '\'';

        #endregion Fields

        #region Methods

        /// <summary>
        /// Decrypts an encrypted string and returns it back as a
        /// <see cref="SecureString" />. Allows for escaped text by wrapping
        /// string with single quotes. Default text converter is
        /// <c>Base64String</c>.
        /// </summary>
        /// <param name="provider">The crypto service provider instance.</param>
        /// <param name="encryptedText">The encrypted text to be decrypted.
        /// </param>
        /// <returns>
        /// The decrypted string as a <see cref="SecureString" />.
        /// </returns>
        public static SecureString DecryptStringSecure(this SymmetricAlgorithm provider, string encryptedText) =>
            provider.DecryptStringSecure(encryptedText, Convert.FromBase64String);

        /// <summary>
        /// Decrypts an encrypted string and returns it back as a
        /// <see cref="SecureString" />. Allows for escaped text by wrapping
        /// string with single quotes.
        /// </summary>
        /// <param name="provider">The crypto service provider instance.</param>
        /// <param name="encryptedText">The encrypted text to be decrypted.
        /// </param>
        /// <param name="textConverter">The custom text converter.</param>
        /// <returns>
        /// The decrypted string as a <see cref="SecureString" />.
        /// </returns>
        public static SecureString DecryptStringSecure(
            this SymmetricAlgorithm provider,
            string encryptedText,
            Func<string, byte[]> textConverter)
        {
            byte[] decryptedData;
            if (encryptedText.StartsAndEndsWith(DecryptEscapeCharacter))
            {
                // Removes the escape characters at the start and end.
                decryptedData =
                    Encoding.Unicode.GetBytes(
                        encryptedText.RemoveFirstChar(DecryptEscapeCharacter).RemoveLastChar(DecryptEscapeCharacter));
            }
            else
            {
                ADP.CheckArgumentNull(provider, nameof(provider));
                ADP.CheckArgumentNull(textConverter, nameof(textConverter));

                byte[] encodedText = textConverter(encryptedText);
                decryptedData = Transformer(provider, encodedText, p => p.CreateDecryptor());
            }

            Debug.WriteLine("Decrypted String: " + Encoding.Unicode.GetString(decryptedData));
            SecureString encryptedString = Encoding.Unicode.GetString(decryptedData).ToSecureString();
            return encryptedString;
        }

        /// <summary>
        /// Encrypts a <see cref="SecureString" /> for security reasons and
        /// returns it as a string. Default text converter is
        /// <c>Base64String</c>.
        /// </summary>
        /// <param name="provider">The crypto service provider instance.</param>
        /// <param name="decryptedText">The decrypted text to be encrypted.
        /// </param>
        /// <returns>The encrypted string.</returns>
        public static string EncryptStringSecure(this SymmetricAlgorithm provider, SecureString decryptedText) =>
            provider.EncryptStringSecure(decryptedText, Convert.ToBase64String);

        /// <summary>
        /// Encrypts a <see cref="SecureString" /> for security reasons and
        /// returns it as a string.
        /// </summary>
        /// <param name="provider">The crypto service provider instance.</param>
        /// <param name="decryptedText">The decrypted text to be encrypted.
        /// </param>
        /// <param name="textConverter">The custom text converter.</param>
        /// <returns>The encrypted string.</returns>
        public static string EncryptStringSecure(
            this SymmetricAlgorithm provider,
            SecureString decryptedText,
            Func<byte[], string> textConverter)
        {
            ADP.CheckArgumentNull(provider, nameof(provider));

            byte[] encodedText = Encoding.Unicode.GetBytes(decryptedText.ToUnsecureString());
            byte[] encryptedText = Transformer(provider, encodedText, p => p.CreateEncryptor());

            return textConverter(encryptedText);
        }

        #endregion Methods

        #region Helper Methods

        /// <summary>
        /// Transforms the specified data using the specified provider.
        /// </summary>
        /// <param name="provider">The crypto service provider instance.</param>
        /// <param name="data">The data to be transformed.</param>
        /// <param name="cryptor">Function for retrieving the symmetric
        /// transformer object.</param>
        /// <returns>
        /// A byte array of the transformed text.
        /// </returns>
        private static byte[] Transformer(
            SymmetricAlgorithm provider,
            byte[] data,
            Func<SymmetricAlgorithm, ICryptoTransform> cryptor)
        {
            byte[] transformedData;
            using (ICryptoTransform ct = cryptor(provider))
            {
                transformedData = ct.TransformFinalBlock(data, 0, data.Length);
            }

            return transformedData;
        }

        #endregion Helper Methods
    }
}