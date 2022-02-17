namespace Oliviann.IO.Compression.Tests
{
    #region Usings

    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using Oliviann.IO;
    using Oliviann.IO.Compression;
    using Oliviann.Security.Cryptography;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Xunit;
    using Assert = Xunit.Assert;

    #endregion Usings

    /// <summary>
    /// Summary description for SevenZipCompressionTests
    /// </summary>
    [DeploymentItem(@"TestObjects\TestArchive.7z")]
    [DeploymentItem(@"TestObjects\TestArchive_NoPass.7z")]
    [DeploymentItem(@"TestObjects\IMIS_5_1.TEST.ipdu")]
    [DeploymentItem(@"7za.exe")]
    public class SevenZipCompressionTests : IClassFixture<SevenZipCompressionTestsFixture>
    {
        #region Fields

        private SevenZipCompressionTestsFixture fixture;

        #endregion Fields

        #region Constructor/Destructor

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="SevenZipCompressionTests"/> class.
        /// </summary>
        /// <param name="fixture">The current fixture.</param>
        public SevenZipCompressionTests(SevenZipCompressionTestsFixture fixture)
        {
            this.fixture = fixture;
        }

        #endregion Constructor/Destructor

        #region Read Tests

        [Fact]
        [Trait("Category", "Compression")]
        public void Test_7ZipCompression_ReadAllArchiveFiles_Small()
        {
            var instance = new SevenZipCompression2(this.fixture.TemporaryArchivePath_Small, CompressionCommand.List);
            instance.Arguments.Password = "1234567";
            instance.Arguments.TraceOutputData = true;

            IList<string> result = instance.ReadArchiveFileNames();
            Assert.Equal(7, result.Count);
        }

        [Fact]
        [Trait("Category", "Compression")]
        public void Test_7ZipCompression_ReadAllArchiveFiles_Small_NoPassword()
        {
            var instance = new SevenZipCompression2(this.fixture.TemporaryArchivePath_Small_NoPassword, CompressionCommand.List);
            instance.Arguments.Password = "1234567";
            instance.Arguments.TraceOutputData = true;

            IList<string> result = instance.ReadArchiveFileNames();
            Assert.Equal(7, result.Count);
        }

        [Fact]
        [Trait("Category", "Compression")]
        public void Test_7ZipCompression_ReadAllArchiveFiles_Medium()
        {
            var instance = new SevenZipCompression2(this.fixture.TemporaryArchivePath_Medium, CompressionCommand.List);
            instance.Arguments.Password = "Boeing$%^I23456789O";
            instance.Arguments.TraceOutputData = true;

            IList<string> result = instance.ReadArchiveFileNames();
            Assert.Equal(11, result.Count);
        }

        [Fact]
        [Trait("Category", "Compression")]
        public void Test_7ZipCompression_CheckArchivePasswordProtected_Small()
        {
            var instance = new SevenZipCompression2(this.fixture.TemporaryArchivePath_Small, CompressionCommand.Test);
            bool result = instance.IsArchivePasswordProtected();

            Assert.True(result, "Small archive was found to be not password protected.");
        }

        [Fact]
        [Trait("Category", "Compression")]
        public void Test_7ZipCompression_CheckArchivePasswordProtected_Small_NoPassword()
        {
            var instance = new SevenZipCompression2(this.fixture.TemporaryArchivePath_Small_NoPassword, CompressionCommand.Test);
            bool result = instance.IsArchivePasswordProtected();

            Assert.False(result, "Small archive was found to be password protected.");
        }

        [Fact]
        [Trait("Category", "Compression")]
        public void Test_7ZipCompression_CheckArchivePasswordProtected_Medium()
        {
            var instance = new SevenZipCompression2(this.fixture.TemporaryArchivePath_Medium, CompressionCommand.Test);
            bool result = instance.IsArchivePasswordProtected();

            Assert.True(result, "Medium archive was found to be not password protected.");
        }

        #endregion Read Tests

        #region Extract Tests

        [Fact]
        [Trait("Category", "Compression")]
        public void Test_7ZipCompression_ExtractCompleteArchiveToFolder_Small()
        {
            int randomValue = new SecureRandom().Next(1, int.MaxValue);
            string outputDir = Path.Combine(this.fixture.TemporaryDirPath, "ResultArchive_Small_" + randomValue);

            var instance = new SevenZipCompression2(this.fixture.TemporaryArchivePath_Small, CompressionCommand.ExtractFullPaths);
            instance.Arguments.Password = "1234567";
            instance.Arguments.TraceOutputData = true;
            instance.Arguments.OutputDirectoryPath = outputDir;
            instance.Arguments.OutputLogLevel = CompressionLogLevel.Info;
            instance.DecompressArchive();

            Assert.True(File.Exists(outputDir.AddPathSeparator() + @"RootFileA.txt"));
            Assert.True(File.Exists(outputDir.AddPathSeparator() + @"RootFileB.txt"));
            Assert.True(File.Exists(outputDir.AddPathSeparator() + @"FolderA\FolderAFileA.txt"));
            Assert.True(File.Exists(outputDir.AddPathSeparator() + @"FolderA\FolderAFileB.txt"));
            Assert.True(File.Exists(outputDir.AddPathSeparator() + @"FolderB\Compressed.7z"));
            Assert.True(File.Exists(outputDir.AddPathSeparator() + @"FolderB\FolderBFileA.txt"));
            Assert.True(File.Exists(outputDir.AddPathSeparator() + @"FolderB\FolderBFileB.txt"));
        }

        [Fact]
        [Trait("Category", "Performance")]
        public void Test_7ZipCompression_ExtractCompleteArchiveToFolder_Small_20Times()
        {
            this.RepeatActionWithTimer(20, this.Test_7ZipCompression_ExtractCompleteArchiveToFolder_Small);
        }

        [Fact]
        [Trait("Category", "Compression")]
        public void Test_7ZipCompression_ExtractCompleteArchiveToFolder_Small_NoPassword()
        {
            int randomValue = new SecureRandom().Next(1, int.MaxValue);
            string outputDir = Path.Combine(this.fixture.TemporaryDirPath, "ResultArchive_Small_NoPass_" + randomValue);

            var instance = new SevenZipCompression2(this.fixture.TemporaryArchivePath_Small_NoPassword, CompressionCommand.ExtractFullPaths);
            instance.Arguments.Password = "1234567";
            instance.Arguments.TraceOutputData = true;
            instance.Arguments.OutputDirectoryPath = outputDir;
            instance.Arguments.OutputLogLevel = CompressionLogLevel.Info;
            instance.DecompressArchive();

            Assert.True(File.Exists(outputDir.AddPathSeparator() + @"RootFileA.txt"));
            Assert.True(File.Exists(outputDir.AddPathSeparator() + @"RootFileB.txt"));
            Assert.True(File.Exists(outputDir.AddPathSeparator() + @"FolderA\FolderAFileA.txt"));
            Assert.True(File.Exists(outputDir.AddPathSeparator() + @"FolderA\FolderAFileB.txt"));
            Assert.True(File.Exists(outputDir.AddPathSeparator() + @"FolderB\Compressed.7z"));
            Assert.True(File.Exists(outputDir.AddPathSeparator() + @"FolderB\FolderBFileA.txt"));
            Assert.True(File.Exists(outputDir.AddPathSeparator() + @"FolderB\FolderBFileB.txt"));
        }

        [Fact]
        [Trait("Category", "Performance")]
        public void Test_7ZipCompression_ExtractCompleteArchiveToFolder_Small_NoPassword_20Times()
        {
            this.RepeatActionWithTimer(20, this.Test_7ZipCompression_ExtractCompleteArchiveToFolder_Small_NoPassword);
        }

        [Fact]
        [Trait("Category", "Compression")]
        public void Test_7ZipCompression_ExtractCompleteArchiveToFolder_Medium()
        {
            int randomValue = new SecureRandom().Next(1, int.MaxValue);
            string outputDir = Path.Combine(this.fixture.TemporaryDirPath, "ResultArchive_Medium_" + randomValue);

            var instance = new SevenZipCompression2(this.fixture.TemporaryArchivePath_Medium, CompressionCommand.ExtractFullPaths);
            instance.Arguments.Password = "Boeing$%^I23456789O";
            instance.Arguments.TraceOutputData = true;
            instance.Arguments.OutputDirectoryPath = outputDir;
            instance.Arguments.OutputLogLevel = CompressionLogLevel.Info;
            instance.DecompressArchive();

            Assert.True(File.Exists(outputDir.AddPathSeparator() + @"WIW\Database\snapshot.idbss"));
            Assert.True(File.Exists(outputDir.AddPathSeparator() + @"Documents\C130_INCORPORATION\C130_INCORPORATION.7z"));
            Assert.True(File.Exists(outputDir.AddPathSeparator() + @"Documents\TitlePage\titlepage.htm"));
        }

        [Fact]
        [Trait("Category", "Performance")]
        public void Test_7ZipCompression_ExtractCompleteArchiveToFolder_Medium_20Times()
        {
            this.RepeatActionWithTimer(20, this.Test_7ZipCompression_ExtractCompleteArchiveToFolder_Medium);
        }

        [Fact]
        [Trait("Category", "Compression")]
        public void Test_7ZipCompression_ExtractSingleFileToFolder_Small()
        {
            int randomValue = new SecureRandom().Next(1, int.MaxValue);
            string outputDir = Path.Combine(this.fixture.TemporaryDirPath, "ResultSingle_Small_" + randomValue);

            var instance = new SevenZipCompression2(this.fixture.TemporaryArchivePath_Small, CompressionCommand.ExtractFullPaths);
            instance.Arguments.Password = "1234567";
            instance.Arguments.TraceOutputData = true;
            instance.Arguments.OutputDirectoryPath = outputDir;
            instance.Arguments.OutputLogLevel = CompressionLogLevel.Info;
            instance.DecompressFile("RootFileB.txt");

            Assert.True(File.Exists(outputDir.AddPathSeparator() + @"RootFileB.txt"));
        }

        [Fact]
        [Trait("Category", "Performance")]
        public void Test_7ZipCompression_ExtractSingleFileToFolder_Small_20Times()
        {
            this.RepeatActionWithTimer(20, this.Test_7ZipCompression_ExtractSingleFileToFolder_Small);
        }

        [Fact]
        [Trait("Category", "Compression")]
        public void Test_7ZipCompression_ExtractSingleFileToFolder_Small_NoPassword()
        {
            int randomValue = new SecureRandom().Next(1, int.MaxValue);
            string outputDir = Path.Combine(this.fixture.TemporaryDirPath, "ResultSingle_Small_NoPass_" + randomValue);

            var instance = new SevenZipCompression2(this.fixture.TemporaryArchivePath_Small_NoPassword, CompressionCommand.ExtractFullPaths);
            instance.Arguments.Password = "1234567";
            instance.Arguments.TraceOutputData = true;
            instance.Arguments.OutputDirectoryPath = outputDir;
            instance.Arguments.OutputLogLevel = CompressionLogLevel.Info;
            instance.DecompressFile("RootFileB.txt");

            Assert.True(File.Exists(outputDir.AddPathSeparator() + @"RootFileB.txt"));
        }

        [Fact]
        [Trait("Category", "Performance")]
        public void Test_7ZipCompression_ExtractSingleFileToFolder_Small_NoPassword_20Times()
        {
            this.RepeatActionWithTimer(20, this.Test_7ZipCompression_ExtractSingleFileToFolder_Small_NoPassword);
        }

        [Fact]
        [Trait("Category", "Compression")]
        public void Test_7ZipCompression_ExtractSingleFileToFolder_Medium()
        {
            int randomValue = new SecureRandom().Next(1, int.MaxValue);
            string outputDir = Path.Combine(this.fixture.TemporaryDirPath, "ResultSingle_Medium_" + randomValue);

            var instance = new SevenZipCompression2(this.fixture.TemporaryArchivePath_Medium, CompressionCommand.ExtractFullPaths);
            instance.Arguments.Password = "Boeing$%^I23456789O";
            instance.Arguments.TraceOutputData = true;
            instance.Arguments.OutputDirectoryPath = outputDir;
            instance.Arguments.OutputLogLevel = CompressionLogLevel.Info;
            instance.DecompressFile("PackageManifest.xml");

            Assert.True(File.Exists(outputDir.AddPathSeparator() + "PackageManifest.xml"));
        }

        [Fact]
        [Trait("Category", "Performance")]
        public void Test_7ZipCompression_ExtractSingleFileToFolder_Medium_20Times()
        {
            this.RepeatActionWithTimer(20, this.Test_7ZipCompression_ExtractSingleFileToFolder_Medium);
        }

        #endregion Extract Tests

        #region Compress Tests

        [Fact]
        [Trait("Category", "Compression")]
        public void Test_7ZipCompression_CompressSingleFolder_Small()
        {
            // Extracts the package to prepare for compressing.
            const string Password = "1234567";

            int randomValue = new SecureRandom().Next(1, int.MaxValue);
            string extractOutputDir = Path.Combine(this.fixture.TemporaryDirPath, "ResultArchive_Small_" + randomValue);

            var extractInstance = new SevenZipCompression2(this.fixture.TemporaryArchivePath_Small, CompressionCommand.ExtractFullPaths);
            extractInstance.Arguments.Password = Password;
            extractInstance.Arguments.TraceOutputData = true;
            extractInstance.Arguments.OutputDirectoryPath = extractOutputDir;
            extractInstance.DecompressArchive();

            Assert.True(File.Exists(extractOutputDir.AddPathSeparator() + @"RootFileA.txt"), "RootFileA.txt missing.");
            Assert.True(File.Exists(extractOutputDir.AddPathSeparator() + @"RootFileB.txt"), "RootFileB.txt missing.");
            Assert.True(File.Exists(extractOutputDir.AddPathSeparator() + @"FolderA\FolderAFileA.txt"), "FolderAFileA.txt missing.");
            Assert.True(File.Exists(extractOutputDir.AddPathSeparator() + @"FolderA\FolderAFileB.txt"), "FolderAFileB.txt missing.");
            Assert.True(File.Exists(extractOutputDir.AddPathSeparator() + @"FolderB\Compressed.7z"), "Compressed.7z missing.");
            Assert.True(File.Exists(extractOutputDir.AddPathSeparator() + @"FolderB\FolderBFileA.txt"), "FolderBFileA.txt missing.");
            Assert.True(File.Exists(extractOutputDir.AddPathSeparator() + @"FolderB\FolderBFileB.txt"), "FolderBFileB.txt missing.");

            // Begin Compressing the extracted data back into a new package.
            const string FileName = "SmallArchive.7z";

            var compressInstance = new SevenZipCompression2(this.fixture.TemporaryDirPath.AddPathSeparator() + FileName, CompressionCommand.Add);
            compressInstance.Arguments.Password = "1234567";
            compressInstance.Arguments.TraceOutputData = true;
            compressInstance.Arguments.Format = CompressionFormat.SevenZip;
            compressInstance.Arguments.IncludeFileNames = new List<string> { extractOutputDir.AddPathSeparator() + "*" };
            compressInstance.CompressArchive();

            // Verify the file size is above a certain length.
            var fi = new FileInfo(this.fixture.TemporaryDirPath.AddPathSeparator() + FileName);
            Assert.True(fi.Exists, "Archive file was not created.");
            Assert.True(fi.Length > 400L, "File size was too small. [{0}]".FormatWith(fi.Length));

            // Verify all the files made it into the archive.
            var listInstance = new SevenZipCompression2(this.fixture.TemporaryDirPath.AddPathSeparator() + FileName, CompressionCommand.List);
            listInstance.Arguments.Password = Password;
            listInstance.Arguments.TraceOutputData = true;

            IList<string> result = listInstance.ReadArchiveFileNames();
            Assert.Equal(7, result.Count);
        }

        [Fact]
        [Trait("Category", "Compression")]
        public void Test_7ZipCompression_CompressSingleFolder_Medium_Store()
        {
            // Extracts the package to prepare for compressing.
            const string Password = "Boeing$%^I23456789O";

            int randomValue = new SecureRandom().Next(1, int.MaxValue);
            string extractOutputDir = Path.Combine(this.fixture.TemporaryDirPath, "ResultArchive_Medium_" + randomValue);

            var extractInstance = new SevenZipCompression2(this.fixture.TemporaryArchivePath_Medium, CompressionCommand.ExtractFullPaths);
            extractInstance.Arguments.Password = Password;
            extractInstance.Arguments.TraceOutputData = true;
            extractInstance.Arguments.OutputDirectoryPath = extractOutputDir;
            extractInstance.DecompressArchive();

            Assert.True(File.Exists(extractOutputDir.AddPathSeparator() + @"WIW\Database\snapshot.idbss"));
            Assert.True(File.Exists(extractOutputDir.AddPathSeparator() + @"Documents\C130_INCORPORATION\C130_INCORPORATION.7z"));
            Assert.True(File.Exists(extractOutputDir.AddPathSeparator() + @"Documents\TitlePage\titlepage.htm"));

            // Begin Compressing the extracted data back into a new package.
            const string FileName = @"MediumArchive.ipdu";

            var compressInstance = new SevenZipCompression2(this.fixture.TemporaryDirPath.AddPathSeparator() + FileName, CompressionCommand.Add);
            compressInstance.Arguments.Password = Password;
            compressInstance.Arguments.TraceOutputData = true;
            compressInstance.Arguments.Format = CompressionFormat.SevenZip;
            compressInstance.Arguments.ArchiveType = "7z";
            compressInstance.Arguments.AdvancedOptions.Level = CompressionLevel.None;

            compressInstance.Arguments.IncludeFileNames = new List<string> { extractOutputDir.AddPathSeparator() + "*" };
            compressInstance.CompressArchive();

            // Verify the file size is above a certain length.
            var fi = new FileInfo(this.fixture.TemporaryDirPath.AddPathSeparator() + FileName);
            Assert.True(fi.Exists, "Archive file was not created.");
            Assert.True(fi.Length > 346000000L);

            // Verify the file is password protected.
            Assert.True(compressInstance.IsArchivePasswordProtected(), "Archive was not created with a password.");

            // Verify all the files made it into the archive.
            var listInstance = new SevenZipCompression2(this.fixture.TemporaryDirPath.AddPathSeparator() + FileName, CompressionCommand.List);
            listInstance.Arguments.Password = Password;
            listInstance.Arguments.TraceOutputData = true;

            IList<string> result = listInstance.ReadArchiveFileNames();
            Assert.Equal(11, result.Count);
        }

        [Fact]
        [Trait("Category", "Compression")]
        public void Test_7ZipCompression_CompressSingleFolder_Medium_Normal()
        {
            // Extracts the package to prepare for compressing.
            const string Password = "Boeing$%^I23456789O";

            int randomValue = new SecureRandom().Next(1, int.MaxValue);
            string extractOutputDir = Path.Combine(this.fixture.TemporaryDirPath, "ResultArchive_Medium_" + randomValue);

            var extractInstance = new SevenZipCompression2(this.fixture.TemporaryArchivePath_Medium, CompressionCommand.ExtractFullPaths);
            extractInstance.Arguments.Password = Password;
            extractInstance.Arguments.TraceOutputData = true;
            extractInstance.Arguments.OutputDirectoryPath = extractOutputDir;
            extractInstance.DecompressArchive();

            Assert.True(File.Exists(extractOutputDir.AddPathSeparator() + @"WIW\Database\snapshot.idbss"));
            Assert.True(File.Exists(extractOutputDir.AddPathSeparator() + @"Documents\C130_INCORPORATION\C130_INCORPORATION.7z"));
            Assert.True(File.Exists(extractOutputDir.AddPathSeparator() + @"Documents\TitlePage\titlepage.htm"));

            // Begin Compressing the extracted data back into a new package.
            const string FileName = @"MediumArchive_Normal.ipdu";

            var compressInstance = new SevenZipCompression2(this.fixture.TemporaryDirPath.AddPathSeparator() + FileName, CompressionCommand.Add);
            ////compressInstance.Arguments.Password = "Boeing$%^I23456789O";
            compressInstance.Arguments.TraceOutputData = true;
            compressInstance.Arguments.Format = CompressionFormat.SevenZip;
            compressInstance.Arguments.ArchiveType = "7z";
            compressInstance.Arguments.AdvancedOptions.Level = CompressionLevel.Normal;
            compressInstance.Arguments.AdvancedOptions.SolidMode.BlockSizeLimit = FileSize.FromGigabytes(64D);
            compressInstance.Arguments.AdvancedOptions.EnableMultithreading = true;
            compressInstance.Arguments.AdvancedOptions.MultithreadThreads = 2;
            compressInstance.Arguments.AdvancedOptions.DictionarySize = FileSize.FromMegabytes(128D);
            compressInstance.Arguments.AdvancedOptions.FastBytesNumber = 273;
            compressInstance.Arguments.AdvancedOptions.Method = CompressionMethod.LZMA;

            compressInstance.Arguments.IncludeFileNames = new List<string> { extractOutputDir.AddPathSeparator() + "*" };
            compressInstance.CompressArchive();

            // Verify the file size is above a certain length.
            var fi = new FileInfo(this.fixture.TemporaryDirPath.AddPathSeparator() + FileName);
            Assert.True(fi.Exists, "Archive file was not created.");
            Assert.True(fi.Length > 346000000L);

            // Verify all the files made it into the archive.
            var listInstance = new SevenZipCompression2(this.fixture.TemporaryDirPath.AddPathSeparator() + FileName, CompressionCommand.List);
            listInstance.Arguments.Password = Password;
            listInstance.Arguments.TraceOutputData = true;

            IList<string> result = listInstance.ReadArchiveFileNames();
            Assert.Equal(11, result.Count);
        }

        #endregion Compress Tests

        #region Helpers

        /// <summary>
        /// Repeats the action the specified number of times with an output of
        /// iteration time and total elapsed time.
        /// </summary>
        /// <param name="repeatCount">The number of time to be repeated.</param>
        /// <param name="act">The action to be repeated.</param>
        private void RepeatActionWithTimer(int repeatCount, Action act)
        {
            Stopwatch totalStopwatch = Stopwatch.StartNew();

            for (int count = 1; count <= repeatCount; count += 1)
            {
                Stopwatch iterationStopwatch = Stopwatch.StartNew();
                act.Invoke();

                iterationStopwatch.Stop();
                Trace.WriteLine("Iteration Execution Time(ms): " + iterationStopwatch.ElapsedMilliseconds);
            }

            totalStopwatch.Stop();
            Trace.WriteLine("Total Execution Time(ms): " + totalStopwatch.ElapsedMilliseconds);
        }

        #endregion Helpers
    }
}