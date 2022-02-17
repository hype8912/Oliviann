namespace Oliviann.Tests.Security.Cryptography
{
    #region Usings

    using Oliviann.Security.Cryptography;
    using Xunit;

    #endregion Usings

    [Trait("Category", "CI")]
    public class SHA_256_Tests
    {
        [Theory]
        [InlineData(null, null)]
        [InlineData("", "E3B0C44298FC1C149AFBF4C8996FB92427AE41E4649B934CA495991B7852B855")]
        [InlineData("The quick brown fox jumps over the lazy dog", "D7A8FBB307D7809469CA9ABCB0082E4F8D5651E46D3CDB762D02D0BF37C9E592")]
        [InlineData("imis12345", "B0952EA75DE0BCCD3E6CC078C0935A006209DFB087334D1FAB5200B4F548A286")]
        [InlineData("Oliviann$%^I23456789O", "48B1C8C9DA3E3759623E6516E7C95F974AE1B9181E8E2C2FAB134518EB48EF69")]
        [InlineData("QWer!@34tyuiop098765", "82AED61DC9ECF60AFEFAB4F9AE123E89A11B5BC4C106BC4894AF46D6A410CA31")]
        public void SHA256Test_Strings(string input, string expectedResult)
        {
            string result = CryptoAlgorithms.ComputeHash(input, CryptoAlgorithms.HashType.SHA256);
            Assert.Equal(expectedResult, result);
        }
    }
}