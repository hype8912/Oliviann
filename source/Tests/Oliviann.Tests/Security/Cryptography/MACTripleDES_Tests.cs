namespace Oliviann.Tests.Security.Cryptography
{
    #region Usings

    using Oliviann.Security.Cryptography;
    using Xunit;

    #endregion Usings

    [Trait("Category", "CI")]
    public class MACTripleDES_Tests
    {
        [Theory]
        [InlineData(null, null)]
        [InlineData("", "0000000000000000")]
        ////[InlineData("The quick brown fox jumps over the lazy dog", "C2F489981E8A5220")]
        ////[InlineData("imis12345", "31405E61931F7FDB")]
        ////[InlineData("Oliviann$%^I23456789O", "9CBB059A9DD3A9E8")]
        ////[InlineData("QWer!@34tyuiop098765", "F96A2C7F4763E791")]
        public void MACTripleDESTest_Strings(string input, string expectedResult)
        {
            string result = CryptoAlgorithms.ComputeHash(input, CryptoAlgorithms.HashType.MACTripleDES);
            Assert.Equal(expectedResult, result);
        }
    }
}