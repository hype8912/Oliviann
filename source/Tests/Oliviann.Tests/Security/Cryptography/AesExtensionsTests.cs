namespace Oliviann.Tests.Security.Cryptography
{
    #region Usings

    using System;
    using System.Security;
    using System.Security.Cryptography;
    using System.Text;
    using Oliviann.Security;
    using Oliviann.Security.Cryptography;
    using Xunit;

    #endregion Usings

    [Trait("Category", "CI")]
    public class AesExtensionsTests
    {
        #region Encrypt Tests

        /// <summary>
        /// Verifies a null provider throws an argument null exception.
        /// </summary>
        [Fact]
        public void EncryptStringSecureTest_NullProvider()
        {
            AesCryptoServiceProvider provider = null;
            SecureString input = "HelloWorld".ToSecureString();
            Assert.Throws<ArgumentNullException>(() => provider.EncryptStringSecure(input));
        }

        /// <summary>
        /// Verifies a valid string is encrypted correctly.
        /// </summary>
        [Theory]
        [InlineData("HelloWorld", "imFtJMMkfDcnYVtd1sKf4xpAxQmW0TrM3Um9s2qEolk=")]
        public void EncryptStringSecureTest_ValidString(string inputString, string expectedResult)
        {
            AesCryptoServiceProvider provider = this.CreateProvider();
            SecureString input = inputString.ToSecureString();

            string result = provider.EncryptStringSecure(input);
            Assert.Equal(expectedResult, result);

            provider.Dispose();
        }

        #endregion Encrypt Tests

        #region Decrypt Tests

        /// <summary>
        /// Verifies a null provider throws a argument null exception.
        /// </summary>
        [Fact]
        public void DecryptStringSecureTest_NullProvider()
        {
            AesCryptoServiceProvider provider = null;
            Assert.Throws<ArgumentNullException>(() => provider.DecryptStringSecure("HelloWorld"));
        }

        /// <summary>
        /// Verifies a string is decrypted correctly.
        /// </summary>
        [Theory]
        [InlineData("imFtJMMkfDcnYVtd1sKf4xpAxQmW0TrM3Um9s2qEolk=", "HelloWorld")]
        [InlineData("'I like tacos!'", "I like tacos!")]
        public void DecryptStringSecureTest_ValidStrings(string inputString, string expectedResult)
        {
            AesCryptoServiceProvider provider = this.CreateProvider();
            string result = provider.DecryptStringSecure(inputString).ToUnsecureString();

            Assert.Equal(expectedResult, result);
            provider.Dispose();
        }

        #endregion Decrypt Tests

        #region Helper Methods

        private AesCryptoServiceProvider CreateProvider()
        {
            return new AesCryptoServiceProvider
                   {
                       IV = Encoding.UTF8.GetBytes("F#1yD4(Pr!svPFOQ"),
                       Key = Encoding.UTF8.GetBytes("Kb1a*wLz4PNwCh#2X{g!Qnx3gpJx6Gg0"),
                       Mode = CipherMode.CBC,
                       Padding = PaddingMode.PKCS7
                   };
        }

        #endregion Helper Methods
    }
}