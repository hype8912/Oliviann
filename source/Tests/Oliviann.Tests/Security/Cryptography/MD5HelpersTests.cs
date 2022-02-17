#if !ExcludeMD5

namespace Oliviann.Tests.Security.Cryptography
{
    #region Usings

    using System.Diagnostics;
    using System.IO;
    using System.Linq;
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
    public class MD5HelpersTests : IClassFixture<PathCleanupFixture>
    {
        #region Fields

        private PathCleanupFixture fixture;

        #endregion Fields

        #region Constructor/Destructor

        /// <summary>
        /// Initializes a new instance of the <see cref="MD5HelpersTests"/>
        /// class.
        /// </summary>
        /// <param name="fixture">The curent fixture.</param>
        public MD5HelpersTests(PathCleanupFixture fixture)
        {
            this.fixture = fixture;
        }

        #endregion Constructor/Destructor

        #region Strings

        [Theory]
        [InlineData(null, null)]
        [InlineData("", "D41D8CD98F00B204E9800998ECF8427E")]
        [InlineData("The quick brown fox jumps over the lazy dog", "9E107D9D372BB6826BD81D3542A419D6")]
        [InlineData("imis12345", "B0AC51D960FF72886947C2CBC9556766")]
        [InlineData("Oliviann$%^I23456789O", "BDFE8669D6EEDF0BEC88270A444FDC0C")]
        [InlineData("QWer!@34tyuiop098765", "FE327481CC7DA1E3E07D43E66E6CB3A7")]
        [Trait("Category", "CI")]
        public void MD5_CalculateStringHash_Strings(string input, string expectedResult)
        {
            string result = CryptoAlgorithms.ComputeHash(input, CryptoAlgorithms.HashType.MD5);
            Assert.Equal(expectedResult, result);
        }

        #endregion Strings

        #region Files

        [Theory]
        [InlineData(null, null)]
        [InlineData("", null)]
        [Trait("Category", "CI")]
        public void MD5_CalculateFileHash_Strings(string name, string expectedResult)
        {
            string result = CryptoAlgorithms.ComputeFileHash(name, CryptoAlgorithms.HashType.MD5);
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        [Trait("Category", "CI")]
        public void MD5_CalculateFileHash_NonExistFile()
        {
            Assert.Throws<FileNotFoundException>(
                () => CryptoAlgorithms.ComputeFileHash("C:\\TacoBell.txt", CryptoAlgorithms.HashType.MD5));
        }

        [Theory]
        [InlineData(@"INI\Large_Unicode.ini", "DF53EE9F166731B338137AA10F37BDCA")]
        [InlineData(@"HashFiles\24-50-01-00-01.chg10.a50355-01.cgm4", "15F98D31905715294C26E3256B085A6D")]
        [InlineData(@"HashFiles\99.htm", "DF53EE9F166731B338137AA10F37BDCA|E5E213A0BB37B8B3FD0E3F4A4EE044E6")]
        [InlineData(@"HashFiles\99-10-01-01-03.chg11.c01044-01.cgm4", "D92E0CEF7C0F8118872332429A9A7DED")]
        [InlineData(@"HashFiles\C130OPT2_07_1.BIOLOG.CGM", "256BB957F1DF81A9B6802BBBA90E67E9")]
        [InlineData(@"HashFiles\C13002_12_0_Versioning_Summary.htm", "389290262C956ACC15C7AAB0CCB13FCC|637BCE59943EE9963C91BCF14CE31660")]
        [InlineData(@"HashFiles\PackageManifest.xml", "FAF99E9FF855DF92B09753B995CCBADE|CA31404B3E1C3565B377B69B23C65EE4")]
        [InlineData(@"HashFiles\style.xslt", "1209B472935FC600093B26C70DDD2214|F8F71B387BDDA9B8DDF24EDBC050CD87")]
        [InlineData(@"HashFiles\TAIL_NUMBER_AND_MSN.htm", "BE88D9FE378A3203EFE8361B66C1B2EA|531B0A4E48D39A4DF150B056D6234D2E")]
        [InlineData(@"HashFiles\web.xml", "1C9BF9AFB4C4409F048232B5EB3B802C|D208EFAB7116771E7770794A2BE6B7D5")]
        [Trait("Category", "CI")]
        public void MD5_CalculateFileHash_Files(string name, string expectedResult)
        {
            Trace.WriteLine("Testing file: " + name);
            string dir = Path.Combine(this.fixture.CurrentDirectory, "TestObjects");
            string filePath = Path.Combine(dir, name);
            Assert.True(File.Exists(filePath), "Specified Hash file does not exist. [{0}]".FormatWith(filePath));

            string result = CryptoAlgorithms.ComputeFileHash(filePath, CryptoAlgorithms.HashType.MD5);

            string[] expectedResults = expectedResult.Split('|');
            Assert.Contains(expectedResults, r => r == result);
        }

        [Theory]
        [InlineData(@"INI\Large_Unicode.ini", "DF53EE9F166731B338137AA10F37BDCA")]
        [InlineData(@"HashFiles\24-50-01-00-01.chg10.a50355-01.cgm4", "15F98D31905715294C26E3256B085A6D")]
        [InlineData(@"HashFiles\99.htm", "DF53EE9F166731B338137AA10F37BDCA|E5E213A0BB37B8B3FD0E3F4A4EE044E6")]
        [InlineData(@"HashFiles\99-10-01-01-03.chg11.c01044-01.cgm4", "D92E0CEF7C0F8118872332429A9A7DED")]
        [InlineData(@"HashFiles\C130OPT2_07_1.BIOLOG.CGM", "256BB957F1DF81A9B6802BBBA90E67E9")]
        [InlineData(@"HashFiles\C13002_12_0_Versioning_Summary.htm", "389290262C956ACC15C7AAB0CCB13FCC|637BCE59943EE9963C91BCF14CE31660")]
        [InlineData(@"HashFiles\PackageManifest.xml", "FAF99E9FF855DF92B09753B995CCBADE|CA31404B3E1C3565B377B69B23C65EE4")]
        [InlineData(@"HashFiles\style.xslt", "1209B472935FC600093B26C70DDD2214|F8F71B387BDDA9B8DDF24EDBC050CD87")]
        [InlineData(@"HashFiles\TAIL_NUMBER_AND_MSN.htm", "BE88D9FE378A3203EFE8361B66C1B2EA|531B0A4E48D39A4DF150B056D6234D2E")]
        [InlineData(@"HashFiles\web.xml", "1C9BF9AFB4C4409F048232B5EB3B802C|D208EFAB7116771E7770794A2BE6B7D5")]
        [Trait("Category", "Performance")]
        [Trait("Category", "SkipWhenLiveUnitTesting")]
        public void MD5_CalculateFileHash_File_50Times(string name, string expectedResult)
        {
            const int LoopMaxCount = 50;

            string dir = Path.Combine(this.fixture.CurrentDirectory, "TestObjects");
            string filePath = Path.Combine(dir, name);
            Assert.True(File.Exists(filePath), "Specified Hash file does not exist. [{0}]".FormatWith(filePath));
            string[] expectedResults = expectedResult.Split('|');

            Stopwatch sw = Stopwatch.StartNew();
            for (int count = 0; count < LoopMaxCount; count += 1)
            {
                string result = CryptoAlgorithms.ComputeFileHash(filePath, CryptoAlgorithms.HashType.MD5);
                Assert.Contains(expectedResults, r => r == result);
            }

            sw.Stop();
            long length = new FileInfo(filePath).Length;
            Trace.WriteLine("File: {0}\tLength:{1}\tElapsed: {2}ms".FormatWith(name, length, sw.ElapsedMilliseconds));
        }

        #endregion Files
    }
}

#endif