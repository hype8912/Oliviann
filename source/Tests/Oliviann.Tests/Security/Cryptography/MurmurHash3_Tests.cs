namespace Oliviann.Tests.Security.Cryptography
{
    #region Usings

    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
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
    public class MurmurHash3_Tests : IClassFixture<PathCleanupFixture>
    {
        #region Fields

        private PathCleanupFixture fixture;

        #endregion Fields

        #region Constructor/Destructor

        /// <summary>
        /// Initializes a new instance of the <see cref="MurmurHash3_Tests"/>
        /// class.
        /// </summary>
        /// <param name="fixture">The curent fixture.</param>
        public MurmurHash3_Tests(PathCleanupFixture fixture)
        {
            this.fixture = fixture;
        }

        #endregion Constructor/Destructor

        #region Files

        [Theory]
        [InlineData(@"INI\Large_Unicode.ini", "3129318080")]
        [InlineData(@"HashFiles\24-50-01-00-01.chg10.a50355-01.cgm4", "3550242720")]
        [InlineData(@"HashFiles\99.htm", "3754560846|3765742215")]
        [InlineData(@"HashFiles\99-10-01-01-03.chg11.c01044-01.cgm4", "1529804527")]
        [InlineData(@"HashFiles\C130OPT2_07_1.BIOLOG.CGM", "941739599")]
        [InlineData(@"HashFiles\C13002_12_0_Versioning_Summary.htm", "2869346852|1632429915")]
        [InlineData(@"HashFiles\PackageManifest.xml", "1596102292|3467473629")]
        [InlineData(@"HashFiles\style.xslt", "691261975|2341133539")]
        [InlineData(@"HashFiles\TAIL_NUMBER_AND_MSN.htm", "624738338|3317030903")]
        [InlineData(@"HashFiles\web.xml", "329938454|508421124")]
        [Trait("Category", "CI")]
        public void MurmurHash3Test_CalculateFileHashTest_File(string name, string expectedResult)
        {
            Trace.WriteLine("Testing file: " + name);
            string dir = Path.Combine(this.fixture.CurrentDirectory, "TestObjects");
            string filePath = Path.Combine(dir, name);
            Assert.True(File.Exists(filePath), "Specified Hash file does not exist. [{0}]".FormatWith(filePath));

            byte[] fileBytes = FileHelpers.ReadAsByteArray(filePath);
            string result = new MurmurHash3().Hash(fileBytes).ToString();

            Array.Clear(fileBytes, 0, fileBytes.Length);
            string[] expectedResults = expectedResult.Split('|');
            Assert.Contains(expectedResults, r => r == result);
        }

        [Theory]
        [InlineData(@"INI\Large_Unicode.ini", "3129318080")]
        [InlineData(@"HashFiles\24-50-01-00-01.chg10.a50355-01.cgm4", "3550242720")]
        [InlineData(@"HashFiles\99.htm", "3754560846|3765742215")]
        [InlineData(@"HashFiles\99-10-01-01-03.chg11.c01044-01.cgm4", "1529804527")]
        [InlineData(@"HashFiles\C130OPT2_07_1.BIOLOG.CGM", "941739599")]
        [InlineData(@"HashFiles\C13002_12_0_Versioning_Summary.htm", "2869346852|1632429915")]
        [InlineData(@"HashFiles\PackageManifest.xml", "1596102292|3467473629")]
        [InlineData(@"HashFiles\style.xslt", "691261975|2341133539")]
        [InlineData(@"HashFiles\TAIL_NUMBER_AND_MSN.htm", "624738338|3317030903")]
        [InlineData(@"HashFiles\web.xml", "329938454|508421124")]
        [Trait("Category", "Performance")]
        [Trait("Category", "SkipWhenLiveUnitTesting")]
        public void MurmurHash3Test_CalculateFileHashTest_File_50Times(string name, string expectedResult)
        {
            const int LoopMaxCount = 50;

            string dir = Path.Combine(this.fixture.CurrentDirectory, "TestObjects");
            string filePath = Path.Combine(dir, name);
            Assert.True(File.Exists(filePath), "Specified Hash file does not exist. [{0}]".FormatWith(filePath));
            string[] expectedResults = expectedResult.Split('|');

            Stopwatch sw = Stopwatch.StartNew();
            for (int count = 0; count < LoopMaxCount; count += 1)
            {
                byte[] fileBytes = FileHelpers.ReadAsByteArray(filePath);
                string result = new MurmurHash3().Hash(fileBytes).ToString();
                Array.Clear(fileBytes, 0, fileBytes.Length);
                Assert.Contains(expectedResults, r => r == result);
            }

            sw.Stop();
            long length = new FileInfo(filePath).Length;
            Trace.WriteLine("File: {0}\tLength:{1}\tElapsed: {2}ms".FormatWith(name, length, sw.ElapsedMilliseconds));
        }

        #endregion Files
    }
}