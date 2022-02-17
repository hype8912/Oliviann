#if !ExcludeMD5

namespace Oliviann.Tests.Security.Cryptography
{
    #region Usings

    using System.Diagnostics;
    using Oliviann.Security.Cryptography;
    using Xunit;

    #endregion Usings

    [Trait("Category", "CI")]
    public class MD5_Tests
    {
        [Theory]
        [InlineData(null, null)]
        [InlineData("", "D41D8CD98F00B204E9800998ECF8427E")]
        [InlineData("The quick brown fox jumps over the lazy dog", "9E107D9D372BB6826BD81D3542A419D6")]
        [InlineData("imis12345", "B0AC51D960FF72886947C2CBC9556766")]
        [InlineData("Oliviann$%^I23456789O", "BDFE8669D6EEDF0BEC88270A444FDC0C")]
        [InlineData("QWer!@34tyuiop098765", "FE327481CC7DA1E3E07D43E66E6CB3A7")]
        public void MD5ComputeHashTest_Strings(string input, string expectedResult)
        {
            string result = CryptoAlgorithms.ComputeHash(input, CryptoAlgorithms.HashType.MD5);
            Trace.WriteLine(input + " :: " + result);
            Assert.Equal(expectedResult, result);
        }
    }
}

#endif