namespace Oliviann.Tests.Security.Cryptography
{
    #region Usings

    using System.Diagnostics;
    using System.IO;
    using Oliviann.Security.Cryptography;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Testing.Fixtures;
    using Xunit;
    using Assert = Xunit.Assert;

    #endregion Usings

    [DeploymentItem(@"TestObjects\INI\Large_Unicode.ini")]
    [DeploymentItem(@"TestObjects\HashFiles\24-50-01-00-01.chg10.a50355-01.cgm4")]
    [DeploymentItem(@"TestObjects\HashFiles\99-10-01-01-03.chg11.c01044-01.cgm4")]
    [DeploymentItem(@"TestObjects\HashFiles\99.htm")]
    [DeploymentItem(@"TestObjects\HashFiles\C13002_12_0_Versioning_Summary.htm")]
    [DeploymentItem(@"TestObjects\HashFiles\C130OPT2_07_1.BIOLOG.CGM")]
    [DeploymentItem(@"TestObjects\HashFiles\PackageManifest.xml")]
    [DeploymentItem(@"TestObjects\HashFiles\style.xslt")]
    [DeploymentItem(@"TestObjects\HashFiles\TAIL_NUMBER_AND_MSN.htm")]
    [DeploymentItem(@"TestObjects\HashFiles\web.xml")]
    public class SHA1_Tests : IClassFixture<PathCleanupFixture>
    {
        #region Fields

        private PathCleanupFixture fixture;

        #endregion Fields

        #region Constructor/Destructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SHA1_Tests"/>
        /// class.
        /// </summary>
        /// <param name="fixture">The curent fixture.</param>
        public SHA1_Tests(PathCleanupFixture fixture)
        {
            this.fixture = fixture;
        }

        #endregion Constructor/Destructor

        #region Strings

        [Theory]
        [InlineData(null, null)]
        [InlineData("", "DA39A3EE5E6B4B0D3255BFEF95601890AFD80709")]
        [InlineData("The quick brown fox jumps over the lazy dog", "2FD4E1C67A2D28FCED849EE1BB76E7391B93EB12")]
        [InlineData("imis12345", "2AFB60EC238E4AC748C55FDD9F6E6CF813BEAB9C")]
        [InlineData("Oliviann$%^I23456789O", "CFACCAA3EA4E5D88D0605CAF62996083D72DA9E4")]
        [InlineData("QWer!@34tyuiop098765", "84C9D972D05BDAAB8392D846F2FA20D2DBA946B6")]
        [Trait("Category", "CI")]
        public void SHA1Test_Strings(string input, string expectedResult)
        {
            string result = CryptoAlgorithms.ComputeHash(input, CryptoAlgorithms.HashType.SHA1);
            Assert.Equal(expectedResult, result);
        }

        #endregion Strings

        #region Files

        [Theory]
        [InlineData(@"INI\Large_Unicode.ini", "2029FA1EF300622EFF465F2C89AEDE368780257D")]
        [InlineData(@"HashFiles\24-50-01-00-01.chg10.a50355-01.cgm4", "5680DA8AB0D8D59CAA7892855C96E2B2506F2E69")]
        [InlineData(@"HashFiles\99.htm", "688D5FB8953DB74B84C85E289756F95839DC21EF|39EC0F01B361B3D018984F54A3965ABBA20AAED9")]
        [InlineData(@"HashFiles\99-10-01-01-03.chg11.c01044-01.cgm4", "8423C93B29DE441D7BF243E5B9A92C25FEDADA74")]
        [InlineData(@"HashFiles\C130OPT2_07_1.BIOLOG.CGM", "8724CD50A5301180BC8D5897242B0B4A3ECD58D7")]
        [InlineData(@"HashFiles\C13002_12_0_Versioning_Summary.htm", "259EB8EB308E12AB797E493D046DB4F74856EF5E|DE72B599A4C4F6A190625284AC5C6E8C8FAF41C3")]
        [InlineData(@"HashFiles\PackageManifest.xml", "093FCF675FE6D9C79BC550E93E0C3BE3B7F90401|7242F188E841F3C2C39EC88B212B6ABC17046A1B")]
        [InlineData(@"HashFiles\style.xslt", "2029FA1EF300622EFF465F2C89AEDE368780257D|52A8ED21266EFDEF7796AC6FF2F64B93D9A1074A")]
        [InlineData(@"HashFiles\TAIL_NUMBER_AND_MSN.htm", "A68CDDE3F916C5005D3BD0C76E943604A67CD4A5|72EBD83E33654A0058746BF926DFE1850ABD3A7F")]
        [InlineData(@"HashFiles\web.xml", "7FE45A82BE463B15B19DDCFD33C3378D149F34C2|2B5B9C606153627A41B3DCD12E63234E874300FE")]
        [Trait("Category", "CI")]
        public void SHA1Cng_CalculateFileHash_File(string name, string expectedResult)
        {
            Trace.WriteLine("Testing file: " + name);
            string dir = Path.Combine(this.fixture.CurrentDirectory, "TestObjects");
            string filePath = Path.Combine(dir, name);
            Assert.True(File.Exists(filePath), "Specified Hash file does not exist. [{0}]".FormatWith(filePath));

            string result = CryptoAlgorithms.ComputeFileHash(filePath, CryptoAlgorithms.HashType.SHA1);
            string[] expectedResults = expectedResult.Split('|');
            Assert.Contains(expectedResults, r => r == result);
        }

        [Theory]
        [InlineData(@"INI\Large_Unicode.ini", "2029FA1EF300622EFF465F2C89AEDE368780257D")]
        [InlineData(@"HashFiles\24-50-01-00-01.chg10.a50355-01.cgm4", "5680DA8AB0D8D59CAA7892855C96E2B2506F2E69")]
        [InlineData(@"HashFiles\99.htm", "688D5FB8953DB74B84C85E289756F95839DC21EF|39EC0F01B361B3D018984F54A3965ABBA20AAED9")]
        [InlineData(@"HashFiles\99-10-01-01-03.chg11.c01044-01.cgm4", "8423C93B29DE441D7BF243E5B9A92C25FEDADA74")]
        [InlineData(@"HashFiles\C130OPT2_07_1.BIOLOG.CGM", "8724CD50A5301180BC8D5897242B0B4A3ECD58D7")]
        [InlineData(@"HashFiles\C13002_12_0_Versioning_Summary.htm", "259EB8EB308E12AB797E493D046DB4F74856EF5E|DE72B599A4C4F6A190625284AC5C6E8C8FAF41C3")]
        [InlineData(@"HashFiles\PackageManifest.xml", "093FCF675FE6D9C79BC550E93E0C3BE3B7F90401|7242F188E841F3C2C39EC88B212B6ABC17046A1B")]
        [InlineData(@"HashFiles\style.xslt", "2029FA1EF300622EFF465F2C89AEDE368780257D|52A8ED21266EFDEF7796AC6FF2F64B93D9A1074A")]
        [InlineData(@"HashFiles\TAIL_NUMBER_AND_MSN.htm", "A68CDDE3F916C5005D3BD0C76E943604A67CD4A5|72EBD83E33654A0058746BF926DFE1850ABD3A7F")]
        [InlineData(@"HashFiles\web.xml", "7FE45A82BE463B15B19DDCFD33C3378D149F34C2|2B5B9C606153627A41B3DCD12E63234E874300FE")]
        [Trait("Category", "Performance")]
        [Trait("Category", "SkipWhenLiveUnitTesting")]
        public void SHA1Cng_CalculateFileHash_File_50Times(string name, string expectedResult)
        {
            const int LoopMaxCount = 50;

            string dir = Path.Combine(this.fixture.CurrentDirectory, "TestObjects");
            string filePath = Path.Combine(dir, name);
            Assert.True(File.Exists(filePath), "Specified Hash file does not exist. [{0}]".FormatWith(filePath));
            string[] expectedResults = expectedResult.Split('|');

            Stopwatch sw = Stopwatch.StartNew();
            for (int count = 0; count < LoopMaxCount; count += 1)
            {
                string result = CryptoAlgorithms.ComputeFileHash(filePath, CryptoAlgorithms.HashType.SHA1);
                Assert.Contains(expectedResults, r => r == result);
            }

            sw.Stop();
            long length = new FileInfo(filePath).Length;
            Trace.WriteLine("File: {0}\tLength:{1}\tElapsed: {2}ms".FormatWith(name, length, sw.ElapsedMilliseconds));
        }

        #endregion Files
    }
}