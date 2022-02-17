namespace Oliviann.Tests.Security.Cryptography
{
    #region Usings

    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Text;
    using Oliviann.IO;
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
    public class MurmurHash2_Tests : IClassFixture<PathCleanupFixture>
    {
        #region Fields

        private PathCleanupFixture fixture;

        #endregion Fields

        #region Constructor/Destructor

        /// <summary>
        /// Initializes a new instance of the <see cref="MurmurHash2_Tests"/>
        /// class.
        /// </summary>
        /// <param name="fixture">The curent fixture.</param>
        public MurmurHash2_Tests(PathCleanupFixture fixture)
        {
            this.fixture = fixture;
        }

        #endregion Constructor/Destructor

        #region Strings

        [Fact]
        [Trait("Category", "CI")]
        public void MurmurHash2_ComputeHashTest_Null()
        {
            Assert.Throws<NullReferenceException>(() => new MurmurHash2().Hash(null));
        }

        [Theory]
        [InlineData("", "0")]
        [InlineData("The quick brown fox jumps over the lazy dog", "4230443545")]
        [InlineData("imis12345", "649494634")]
        [InlineData("Oliviann$%^I23456789O", "2307497254")]
        [InlineData("QWer!@34tyuiop098765", "741727471")]
        [InlineData("Hey", "3290967642")]
        [InlineData("2", "2829747241")]
        [InlineData("9", "2941011771")]
        [InlineData("47", "2846253068")]
        [Trait("Category", "CI")]
        public void MurmurHash2_ComputeHashTest_Strings(string input, string expectedResult)
        {
            byte[] keyBytes = Encoding.Default.GetBytes(input);
            uint result = new MurmurHash2().Hash(keyBytes);
            Assert.Equal(expectedResult, result.ToString());
        }

        [Fact]
        [Trait("Category", "CI")]
        public void MurmurHash2Unsafe_ComputeHashTest_Null()
        {
            byte[] array = null;
            Assert.Throws<NullReferenceException>(() => new MurmurHash2Unsafe().Hash(array));
        }

        [Theory]
        [InlineData("", "0")]
        [InlineData("The quick brown fox jumps over the lazy dog", "4230443545")]
        [InlineData("imis12345", "649494634")]
        [InlineData("Oliviann$%^I23456789O", "2307497254")]
        [InlineData("QWer!@34tyuiop098765", "741727471")]
        [InlineData("Hey", "3290967642")]
        [InlineData("2", "2829747241")]
        [InlineData("9", "2941011771")]
        [InlineData("47", "2846253068")]
        [Trait("Category", "CI")]
        public void MurmurHash2Unsafe_ComputeHashTest_Strings(string input, string expectedResult)
        {
            byte[] keyBytes = Encoding.Default.GetBytes(input);
            uint result = new MurmurHash2Unsafe().Hash(keyBytes);
            Assert.Equal(expectedResult, result.ToString());
        }

        #endregion Strings

        #region Files

        [Theory]
        [InlineData(@"INI\Large_Unicode.ini", "1040591493")]
        [InlineData(@"HashFiles\24-50-01-00-01.chg10.a50355-01.cgm4", "1800281461")]
        [InlineData(@"HashFiles\99.htm", "3571483177|4075393860")]
        [InlineData(@"HashFiles\99-10-01-01-03.chg11.c01044-01.cgm4", "1584504844")]
        [InlineData(@"HashFiles\C130OPT2_07_1.BIOLOG.CGM", "3743915101")]
        [InlineData(@"HashFiles\C13002_12_0_Versioning_Summary.htm", "2945069313|68352192")]
        [InlineData(@"HashFiles\PackageManifest.xml", "2718079962|4176907308")]
        [InlineData(@"HashFiles\style.xslt", "2011020245|890324373")]
        [InlineData(@"HashFiles\TAIL_NUMBER_AND_MSN.htm", "33765886|1500089472")]
        [InlineData(@"HashFiles\web.xml", "3293340125|3617925149")]
        [Trait("Category", "CI")]
        public void MurmurHash2_CalculateFileHash_File(string name, string expectedResult)
        {
            Trace.WriteLine("Testing file: " + name);
            string dir = Path.Combine(this.fixture.CurrentDirectory, "TestObjects");
            string filePath = Path.Combine(dir, name);
            Assert.True(File.Exists(filePath), "Specified Hash file does not exist. [{0}]".FormatWith(filePath));

            string result = new MurmurHash2().Hash(FileHelpers.ReadAsByteArray(filePath)).ToString();
            string[] expectedResults = expectedResult.Split('|');
            Assert.Contains(expectedResults, r => r == result);
        }

        [Theory]
        [InlineData(@"INI\Large_Unicode.ini", "1040591493")]
        [InlineData(@"HashFiles\24-50-01-00-01.chg10.a50355-01.cgm4", "1800281461")]
        [InlineData(@"HashFiles\99.htm", "3571483177|4075393860")]
        [InlineData(@"HashFiles\99-10-01-01-03.chg11.c01044-01.cgm4", "1584504844")]
        [InlineData(@"HashFiles\C130OPT2_07_1.BIOLOG.CGM", "3743915101")]
        [InlineData(@"HashFiles\C13002_12_0_Versioning_Summary.htm", "2945069313|68352192")]
        [InlineData(@"HashFiles\PackageManifest.xml", "2718079962|4176907308")]
        [InlineData(@"HashFiles\style.xslt", "2011020245|890324373")]
        [InlineData(@"HashFiles\TAIL_NUMBER_AND_MSN.htm", "33765886|1500089472")]
        [InlineData(@"HashFiles\web.xml", "3293340125|3617925149")]
        [Trait("Category", "Performance")]
        [Trait("Category", "SkipWhenLiveUnitTesting")]
        public void MurmurHash2_CalculateFileHash_File_50Times(string name, string expectedResult)
        {
            const int LoopMaxCount = 50;

            string dir = Path.Combine(this.fixture.CurrentDirectory, "TestObjects");
            string filePath = Path.Combine(dir, name);
            Assert.True(File.Exists(filePath), "Specified Hash file does not exist. [{0}]".FormatWith(filePath));
            string[] expectedResults = expectedResult.Split('|');

            Stopwatch sw = Stopwatch.StartNew();
            for (int count = 0; count < LoopMaxCount; count += 1)
            {
                string result = new MurmurHash2().Hash(FileHelpers.ReadAsByteArray(filePath)).ToString();
                Assert.Contains(expectedResults, r => r == result);
            }

            sw.Stop();
            long length = new FileInfo(filePath).Length;
            Trace.WriteLine("File: {0}\tLength:{1}\tElapsed: {2}ms".FormatWith(name, length, sw.ElapsedMilliseconds));
        }

        [Theory]
        [InlineData(@"INI\Large_Unicode.ini", "1040591493")]
        [InlineData(@"HashFiles\24-50-01-00-01.chg10.a50355-01.cgm4", "1800281461")]
        [InlineData(@"HashFiles\99.htm", "3571483177|4075393860")]
        [InlineData(@"HashFiles\99-10-01-01-03.chg11.c01044-01.cgm4", "1584504844")]
        [InlineData(@"HashFiles\C130OPT2_07_1.BIOLOG.CGM", "3743915101")]
        [InlineData(@"HashFiles\C13002_12_0_Versioning_Summary.htm", "2945069313|68352192")]
        [InlineData(@"HashFiles\PackageManifest.xml", "2718079962|4176907308")]
        [InlineData(@"HashFiles\style.xslt", "2011020245|890324373")]
        [InlineData(@"HashFiles\TAIL_NUMBER_AND_MSN.htm", "33765886|1500089472")]
        [InlineData(@"HashFiles\web.xml", "3293340125|3617925149")]
        [Trait("Category", "CI")]
        public void MurmurHash2Unsafe_CalculateFileHash_File(string name, string expectedResult)
        {
            Trace.WriteLine("Testing file: " + name);
            string dir = Path.Combine(this.fixture.CurrentDirectory, "TestObjects");
            string filePath = Path.Combine(dir, name);
            Assert.True(File.Exists(filePath), "Specified Hash file does not exist. [{0}]".FormatWith(filePath));

            string result = new MurmurHash2Unsafe().Hash(FileHelpers.ReadAsByteArray(filePath)).ToString();
            string[] expectedResults = expectedResult.Split('|');
            Assert.Contains(expectedResults, r => r == result);
        }

        [Theory]
        [InlineData(@"INI\Large_Unicode.ini", "1040591493")]
        [InlineData(@"HashFiles\24-50-01-00-01.chg10.a50355-01.cgm4", "1800281461")]
        [InlineData(@"HashFiles\99.htm", "3571483177|4075393860")]
        [InlineData(@"HashFiles\99-10-01-01-03.chg11.c01044-01.cgm4", "1584504844")]
        [InlineData(@"HashFiles\C130OPT2_07_1.BIOLOG.CGM", "3743915101")]
        [InlineData(@"HashFiles\C13002_12_0_Versioning_Summary.htm", "2945069313|68352192")]
        [InlineData(@"HashFiles\PackageManifest.xml", "2718079962|4176907308")]
        [InlineData(@"HashFiles\style.xslt", "2011020245|890324373")]
        [InlineData(@"HashFiles\TAIL_NUMBER_AND_MSN.htm", "33765886|1500089472")]
        [InlineData(@"HashFiles\web.xml", "3293340125|3617925149")]
        [Trait("Category", "Performance")]
        [Trait("Category", "SkipWhenLiveUnitTesting")]
        public void MurmurHash2Unsafe_CalculateFileHash_File_50Times(string name, string expectedResult)
        {
            const int LoopMaxCount = 50;

            string dir = Path.Combine(this.fixture.CurrentDirectory, "TestObjects");
            string filePath = Path.Combine(dir, name);
            Assert.True(File.Exists(filePath), "Specified Hash file does not exist. [{0}]".FormatWith(filePath));
            string[] expectedResults = expectedResult.Split('|');

            Stopwatch sw = Stopwatch.StartNew();
            for (int count = 0; count < LoopMaxCount; count += 1)
            {
                string result = new MurmurHash2Unsafe().Hash(FileHelpers.ReadAsByteArray(filePath)).ToString();
                Assert.Contains(expectedResults, r => r == result);
            }

            sw.Stop();
            long length = new FileInfo(filePath).Length;
            Trace.WriteLine("File: {0}\tLength:{1}\tElapsed: {2}ms".FormatWith(name, length, sw.ElapsedMilliseconds));
        }

        #endregion Files
    }
}