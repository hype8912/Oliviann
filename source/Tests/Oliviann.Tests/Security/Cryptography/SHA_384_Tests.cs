namespace Oliviann.Tests.Security.Cryptography
{
    #region Usings

    using Oliviann.Security.Cryptography;
    using Xunit;

    #endregion Usings

    [Trait("Category", "CI")]
    public class SHA_384_Tests
    {
        [Theory]
        [InlineData(null, null)]
        [InlineData("", "38B060A751AC96384CD9327EB1B1E36A21FDB71114BE07434C0CC7BF63F6E1DA274EDEBFE76F65FBD51AD2F14898B95B")]
        [InlineData("The quick brown fox jumps over the lazy dog", "CA737F1014A48F4C0B6DD43CB177B0AFD9E5169367544C494011E3317DBF9A509CB1E5DC1E85A941BBEE3D7F2AFBC9B1")]
        [InlineData("imis12345", "E4FEDF13EB4B8A0743497C8767B35D14FBE5EB31833B07E3F703A6616B29417C5697FEFB466DF1D32F31D111ACA80FCA")]
        [InlineData("Oliviann$%^I23456789O", "6705430162B266F423FF40AD74996CA33278D63FD6A63F24E52956A7434FCF5B0CDC2FA945415729970DA5244C6F3F65")]
        [InlineData("QWer!@34tyuiop098765", "EEAEB054A9627BE5040E360ADB9160FE21621E60F1DC7CC343588F4254F014CB60AA0063757C26A7E81613ED44269502")]
        public void SHA384Test_Strings(string input, string expectedResult)
        {
            string result = CryptoAlgorithms.ComputeHash(input, CryptoAlgorithms.HashType.SHA384);
            Assert.Equal(expectedResult, result);
        }
    }
}