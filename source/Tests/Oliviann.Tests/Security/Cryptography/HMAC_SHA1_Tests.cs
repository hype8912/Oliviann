namespace Oliviann.Tests.Security.Cryptography
{
    #region Usings

    using Oliviann.Security.Cryptography;
    using Xunit;

    #endregion Usings

    [Trait("Category", "CI")]
    public class HMAC_SHA1_Tests
    {
        [Fact]
        public void HMAC_SHA1ComputeHashTest_NullEncoding()
        {
            string result = CryptoAlgorithms.ComputeHash(null, CryptoAlgorithms.HashType.HMACSHA1, null);
            Assert.Null(result);
        }

        [Theory]
        [InlineData(null, null)]
        [InlineData("", "414FA58BBA398D0CC8591BC6A7BA32B565E78891")]
        [InlineData("The quick brown fox jumps over the lazy dog", "31AC099E83CDC1DF731BD222ECA800A819508749")]
        [InlineData("imis12345", "403922758D464FD437D5959F900FBFECD5597D05")]
        [InlineData("Oliviann$%^I23456789O", "D3328DC582DAA4128FAECA7CA34587F86753A85C")]
        [InlineData("QWer!@34tyuiop098765", "E8F33FEDC9642E628B749E28F94FD7B29B59678D")]
        public void HMAC_SHA1ComputeHashTest_Strings(string input, string expectedResult)
        {
            string result = CryptoAlgorithms.ComputeHash(input, CryptoAlgorithms.HashType.HMACSHA1);
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData(null, null, true)]
        [InlineData(null, "imis12345", false)]
        [InlineData("B0AC51D960FF72886947C2CBC9556766", null, false)]
        [InlineData("414FA58BBA398D0CC8591BC6A7BA32B565E78891", "", true)]
        [InlineData("31AC099E83CDC1DF731BD222ECA800A819508749", "The quick brown fox jumps over the lazy dog", true)]
        [InlineData("403922758D464FD437D5959F900FBFECD5597D05", "imis12345", true)]
        [InlineData("D3328DC582DAA4128FAECA7CA34587F86753A85C", "Oliviann$%^I23456789O", true)]
        [InlineData("E8F33FEDC9642E628B749E28F94FD7B29B59678D", "QWer!@34tyuiop098765", true)]
        public void HMAC_SHA1CompareHashesTest_Strings(string inputHash, string inputText, bool expectedResult)
        {
            bool result = CryptoAlgorithms.CompareHashes(inputHash, inputText, CryptoAlgorithms.HashType.HMACSHA1);
            Assert.Equal(expectedResult, result);
        }
    }
}