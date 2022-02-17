﻿namespace Oliviann.Tests.Security.Cryptography
{
    #region Usings

    using Oliviann.Security.Cryptography;
    using Xunit;

    #endregion Usings

    [Trait("Category", "CI")]
    public class SHA_512_Tests
    {
        [Theory]
        [InlineData(null, null)]
        [InlineData("", "CF83E1357EEFB8BDF1542850D66D8007D620E4050B5715DC83F4A921D36CE9CE47D0D13C5D85F2B0FF8318D2877EEC2F63B931BD47417A81A538327AF927DA3E")]
        [InlineData("The quick brown fox jumps over the lazy dog", "07E547D9586F6A73F73FBAC0435ED76951218FB7D0C8D788A309D785436BBB642E93A252A954F23912547D1E8A3B5ED6E1BFD7097821233FA0538F3DB854FEE6")]
        [InlineData("imis12345", "BE221676D2FEEBC1E6CCF676C276FBF845BDB495856A80156E0C2013E8F2E2568A99012A0C5BEB39E7E62777D9ABC03614C497DF672DAB85AEEC21586D3574A2")]
        [InlineData("Oliviann$%^I23456789O", "C01066A6FA1E108C00EDDBA6133E1D1E9B4E9219D96E0D98ADD77B7495763D1B72E530E7D033BFF70F011987BF81101F8130205C5DF6A05627723730238B9A1D")]
        [InlineData("QWer!@34tyuiop098765", "6AAEEE5B0E1EEC45A5929B23C278076AEF4F6669FFC98569555198DEE46A0EEA5EB0E4304B81AA8A59D403C9BBCD2B4D2F99F32BA1E344C914D6591744756730")]
        public void SHA512Test_Strings(string input, string expectedResult)
        {
            string result = CryptoAlgorithms.ComputeHash(input, CryptoAlgorithms.HashType.SHA512);
            Assert.Equal(expectedResult, result);
        }
    }
}