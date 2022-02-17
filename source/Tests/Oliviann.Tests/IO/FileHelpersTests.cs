namespace Oliviann.Tests.IO
{
    #region Usings

    using System;
    using System.Collections.Generic;
    using System.IO;
    using Oliviann.IO;
    using Oliviann.Testing.Fixtures;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Xunit;
    using Assert = Xunit.Assert;

    #endregion Usings

    [Trait("Category", "CI")]
    [DeploymentItem(@"TestObjects\INI\Large_Unicode.ini")]
    public class FileHelpersTests : IClassFixture<PathCleanupFixture>
    {
        #region Fields

        private readonly PathCleanupFixture fixture;

        private readonly Dictionary<string, string> filePaths = new Dictionary<string, string>();

        #endregion Fields

        #region Constructor/Destructor

        /// <summary>
        /// Initializes a new instance of the <see cref="FileHelpersTests"/>
        /// class.
        /// </summary>
        /// <param name="fixture">The current fixture.</param>
        public FileHelpersTests(PathCleanupFixture fixture)
        {
            this.fixture = fixture;
            this.filePaths.Add("EmptyFilePath", this.fixture.CurrentDirectory + @"\TestObjects\EmptyFile.txt");
            File.WriteAllText(this.filePaths["EmptyFilePath"], string.Empty);

            this.filePaths.Add("GoodXmlFilePath", this.fixture.CurrentDirectory + @"\TestObjects\Books1.xml");
            File.WriteAllText(this.filePaths["GoodXmlFilePath"], Common.TestObjects.Properties.Resources.Books1);

            this.filePaths.Add("MissingXmlFilePath", this.fixture.CurrentDirectory + @"\TestObjects\MissingFile.xml");
            this.filePaths.Add("LargeFilePath", this.fixture.CurrentDirectory + @"\TestObjects\INI\Large_Unicode.ini");
        }

        #endregion Constructor/Destructor

        #region ContainsInvalidFileNameChars Tests

        [Theory]
        [InlineData(null, typeof(ArgumentNullException))]
        [InlineData("", typeof(ArgumentNullException))]
        public void ContainsInvalidFileNameChars_Exceptions(string input, Type expectedExceptionType)
        {
            Assert.Throws(expectedExceptionType, () => FileHelpers.ContainsInvalidFileNameChars(input));
        }

        /// <summary>
        /// Verifies the collection of valid and invalid files names return the
        /// expected result.
        /// </summary>
        [Theory]
        [InlineData("MyFileName.xml", false)]
        [InlineData("My File Name.xml", false)]
        [InlineData("My_File_Name.xml", false)]
        [InlineData("My_File_Name.xml.orig", false)]
        [InlineData("My^File&Name'.xml", false)]
        [InlineData("My@File{Name}.xml", false)]
        [InlineData("My[File]Name,.xml", false)]
        [InlineData("My$File=Name!.xml", false)]
        [InlineData("My-File#Name(.xml", false)]
        [InlineData("My)File%Name.xml", false)]
        [InlineData("My+File~Name.xml", false)]
        [InlineData("My\"FileName.xml", true)]
        [InlineData("MyFile<Name.xml", true)]
        [InlineData("MyFile>Name.xml", true)]
        [InlineData("MyFile|Name.xml", true)]
        [InlineData("MyFile\\Name.xml", true)]
        [InlineData("MyFile/Name.xml", true)]
        [InlineData("MyFile\rName.xml", true)]
        [InlineData("MyFile\nName.xml", true)]
        public void ContainsInvalidFileNameChars_FileNames(string input, bool expectedResult)
        {
            bool result = FileHelpers.ContainsInvalidFileNameChars(input);
            Assert.Equal(expectedResult, result);
        }

        #endregion ContainsInvalidFileNameChars Tests

        #region Exists Tests

        [Fact]
        public void FilesExistEnumTest_NullFiles()
        {
            IEnumerable<string> items = null;
            Assert.Throws<ArgumentNullException>(() => FileHelpers.Exists(items));
        }

        [Fact]
        public void FilesExistEnumTest_EmptyFiles()
        {
            IEnumerable<string> items = new List<string>();
            bool result = FileHelpers.Exists(items);

            Assert.True(result);
        }

        [Fact]
        public void FilesExistEnumTest_AllGoodFiles()
        {
            IEnumerable<string> items = new List<string> { this.filePaths["GoodXmlFilePath"], this.filePaths["EmptyFilePath"], this.filePaths["LargeFilePath"] };
            bool result = FileHelpers.Exists(items);

            Assert.True(result);
        }

        [Fact]
        public void FilesExistEnumTest_MissingFile()
        {
            IEnumerable<string> items = new List<string> { this.filePaths["GoodXmlFilePath"], this.filePaths["EmptyFilePath"], this.filePaths["LargeFilePath"], this.filePaths["MissingXmlFilePath"] };
            bool result = FileHelpers.Exists(items);

            Assert.False(result);
        }

        [Fact]
        public void FilesExistEnumTest_TrashFile()
        {
            IEnumerable<string> items = new List<string> { this.filePaths["GoodXmlFilePath"], this.filePaths["EmptyFilePath"], this.filePaths["LargeFilePath"], "Khwef08238*&^T8byiuf" };
            bool result = FileHelpers.Exists(items);

            Assert.False(result);
        }

        [Fact]
        public void FilesExistEnumTest_NullFile()
        {
            IEnumerable<string> items = new List<string> { this.filePaths["GoodXmlFilePath"], this.filePaths["EmptyFilePath"], this.filePaths["LargeFilePath"], null };
            bool result = FileHelpers.Exists(items);

            Assert.False(result);
        }

        [Fact]
        public void FilesExistArrayTest_NullFiles()
        {
            string[] items = null;
            Assert.Throws<ArgumentNullException>(() => FileHelpers.Exists(items));
        }

        [Fact]
        public void FilesExistArrayTest_EmptyFiles()
        {
            var items = new string[0];
            bool result = FileHelpers.Exists(items);

            Assert.True(result);
        }

        [Fact]
        public void FilesExistArrayTest_AllGoodFiles()
        {
            var items = new[] { this.filePaths["GoodXmlFilePath"], this.filePaths["EmptyFilePath"], this.filePaths["LargeFilePath"] };
            bool result = FileHelpers.Exists(items);

            Assert.True(result);
        }

        [Fact]
        public void FilesExistArrayTest_MissingFile()
        {
            var items = new[] { this.filePaths["GoodXmlFilePath"], this.filePaths["EmptyFilePath"], this.filePaths["LargeFilePath"], this.filePaths["MissingXmlFilePath"] };
            bool result = FileHelpers.Exists(items);

            Assert.False(result);
        }

        [Fact]
        public void FilesExistArrayTest_TrashFile()
        {
            var items = new[] { this.filePaths["GoodXmlFilePath"], this.filePaths["EmptyFilePath"], this.filePaths["LargeFilePath"], "Khwef08238*&^T8byiuf" };
            bool result = FileHelpers.Exists(items);

            Assert.False(result);
        }

        [Fact]
        public void FilesExistArrayTest_NullFile()
        {
            var items = new[] { this.filePaths["GoodXmlFilePath"], this.filePaths["EmptyFilePath"], this.filePaths["LargeFilePath"], null };
            bool result = FileHelpers.Exists(items);

            Assert.False(result);
        }

        #endregion Exists Tests

        #region IsFileLocked Tests

        [Theory]
        [InlineData(null, false)]
        [InlineData("", false)]
        [InlineData("ljsdgnlkMI&T8934u58nihk/rgjknb340nvkhf", false)]
        public void IsFileLockedTest_Strings(string input, bool expectedResult)
        {
            bool result = FileHelpers.IsFileLocked(input);
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void IsFileLockedTest_MissingPath()
        {
            bool result = FileHelpers.IsFileLocked(this.filePaths["MissingXmlFilePath"]);
            Assert.False(result);
        }

        [Fact]
        public void IsFileLockedTest_GoodPath()
        {
            bool result = FileHelpers.IsFileLocked(this.filePaths["GoodXmlFilePath"]);
            Assert.False(result);
        }

        [Fact]
        public void IsFileLockedTest_SimulatedLockedFile()
        {
            string path = this.filePaths["GoodXmlFilePath"].Replace("Books1.xml", "Books_Copy2.xml");
            File.Copy(this.filePaths["GoodXmlFilePath"], path);
            Assert.True(File.Exists(path), "File did not copy.");
            this.fixture.DeletePaths.Add(path);

            var info = new FileInfo(path);
            FileStream stream = info.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            bool result1 = FileHelpers.IsFileLocked(path);
            stream.Close();

            Assert.True(result1, "File was not locked.");
            Assert.False(FileHelpers.IsFileLocked(path), "File was found to be locked.");
        }

        #endregion IsFileLocked Tests

        #region Move Tests

        [Theory]
        [InlineData(null, typeof(ArgumentNullException))]
        [InlineData("", typeof(ArgumentNullException))]
        public void FileMoveTest_Exceptions(string input, Type expectedExceptionType)
        {
            Assert.Throws(expectedExceptionType, () => FileHelpers.Move(input, null));
        }

        [Fact]
        public void FileMoveTest_NullDestFile()
        {
            string toPath = Path.GetTempPath() + @"\Books_MoveCopy.xml";
            File.Copy(this.filePaths["GoodXmlFilePath"], toPath, true);
            Assert.True(File.Exists(toPath));

            Assert.Throws<ArgumentNullException>(() => FileHelpers.Move(toPath, null));
        }

        [Fact]
        public void FileMoveTest_EmptyDestFile()
        {
            string toPath = Path.GetTempPath() + @"\Books_MoveCopy.xml";
            File.Copy(this.filePaths["GoodXmlFilePath"], toPath, true);
            Assert.True(File.Exists(toPath));

            Assert.Throws<ArgumentNullException>(() => FileHelpers.Move(toPath, string.Empty));
        }

        [Fact]
        public void FileMoveTest_MissingSourceFile()
        {
            string newPath = Path.GetTempPath() + @"\Books_MoveCopy2.xml";
            Assert.Throws<FileNotFoundException>(() => FileHelpers.Move(this.filePaths["MissingXmlFilePath"], newPath, true));
        }

        [Fact]
        public void FileMoveTest_SameDir()
        {
            string toPath = Path.GetTempPath() + @"\Books_MoveCopy.xml";
            File.Copy(this.filePaths["GoodXmlFilePath"], toPath, true);
            Assert.True(File.Exists(toPath));

            this.fixture.DeletePaths.Add(toPath);

            string newPath = Path.GetTempPath() + @"\Books_MoveCopy1.xml";
            FileHelpers.Move(toPath, newPath, true);
            Assert.True(File.Exists(newPath));

            this.fixture.DeletePaths.Add(newPath);
        }

        [Fact]
        public void FileMoveTest_DifferentDir()
        {
            string toPath = Path.GetTempPath() + @"\Books_MoveCopy.xml";
            File.Copy(this.filePaths["GoodXmlFilePath"], toPath, true);
            Assert.True(File.Exists(toPath));

            string newPath = Path.GetTempPath() + @"\MoveTest\Books_MoveCopy1.xml";
            FileHelpers.Move(toPath, newPath, true);
            Assert.True(File.Exists(newPath));

            this.fixture.DeletePaths.Add(Path.GetTempPath() + @"\MoveTest");
        }

        /// <remarks>
        /// This test will fail if you are not connected to the Oliviann network
        /// and have permissions to the file share.
        /// </remarks>
        [Fact]
        public void FileMoveTest_NetworkDir()
        {
            string toPath = Path.GetTempPath() + @"\Books_MoveCopy.xml";
            File.Copy(this.filePaths["GoodXmlFilePath"], toPath, true);
            Assert.True(File.Exists(toPath));

            string newPath = @"\\se\data\SOA_Share\Test\Books_MoveCopyTest1.xml";
            FileHelpers.Move(toPath, newPath, true);
            Assert.True(File.Exists(newPath));

            this.fixture.DeletePaths.Add(newPath);
        }

        #endregion Move Tests

        #region ReadAsByteArray Tests

        [Fact]
        public void ReadAsByteArrayTest_Exceptions()
        {
            Assert.Throws<ArgumentNullException>(() => FileHelpers.ReadAsByteArray(null));
        }

        [Fact]
        public void ReadAsByteArrayTest_EmptyPath()
        {
            byte[] result = FileHelpers.ReadAsByteArray(string.Empty);
            Assert.Null(result);
        }

        [Fact]
        public void ReadAsByteArrayTest_MissingPath()
        {
            byte[] result = FileHelpers.ReadAsByteArray(this.filePaths["MissingXmlFilePath"]);
            Assert.Null(result);
        }

        [Fact]
        public void ReadAsByteArrayTest_FilePaths()
        {
            byte[] result = FileHelpers.ReadAsByteArray(this.filePaths["EmptyFilePath"]);

            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public void ReadAsByteArrayTest_SmallFilePath()
        {
            byte[] result = FileHelpers.ReadAsByteArray(this.filePaths["GoodXmlFilePath"]);

            Assert.NotNull(result);
            Assert.InRange(result.Length, 1150, 1178);
        }

#if !TFSBuildAgent

        [Fact]
        public void ReadAsByteArrayTest_LargeFilePath()
        {
            byte[] result = FileHelpers.ReadAsByteArray(this.filePaths["LargeFilePath"]);

            Assert.NotNull(result);
            Assert.Equal(1406108, result.Length);
        }

#endif

        #endregion ReadAsByteArray Tests

        #region ReadContents Tests

        [Fact]
        public void ReadContentsTest_NullPath()
        {
            Assert.Throws<ArgumentNullException>(() => FileHelpers.ReadContents(null));
        }

        [Fact]
        public void ReadContentsTest_EmptyPath()
        {
            string result = FileHelpers.ReadContents(string.Empty);
        }

        [Fact]
        public void ReadContentsTest_MissingPath()
        {
            string result = FileHelpers.ReadContents(this.filePaths["MissingXmlFilePath"]);
            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public void ReadContentsTest_ValidPath()
        {
            string result = FileHelpers.ReadContents(this.filePaths["GoodXmlFilePath"]);
            Assert.False(result.IsNullOrEmpty());
            Assert.Contains("Knorr, Stefan", result);
            Assert.Contains("<title>Oberon's Legacy</title>", result);
        }

        [Fact]
        public void ReadContentsTest_InvalidFileName()
        {
            Assert.Throws<ArgumentException>(() => FileHelpers.ReadContents(this.filePaths["GoodXmlFilePath"].Replace("Books", "Books|")));
        }

        #endregion ReadContents Tests

        #region ToBase64String Tests

        /// <summary>
        /// Verifies calling the method with an invalid path throws the correct
        /// exception.
        /// </summary>
        [Theory]
        [InlineData(null, typeof(ArgumentNullException))]
        [InlineData("", typeof(ArgumentNullException))]
        [InlineData(@"C:\Temp", typeof(FileNotFoundException))]
        [InlineData(@"Q:\kafekwaf\gnkf\fbksf.fth", typeof(FileNotFoundException))]
        public void ToBase64StringTest_InvalidPaths(string filePath, Type expectedResult)
        {
            Assert.Throws(expectedResult, () => FileHelpers.ToBase64String(filePath));
        }

        /// <summary>
        /// Verifies calling the method with an empty file path returns and
        /// empty string.
        /// </summary>
        [Fact]
        public void ToBase64StringTest_EmptyFile()
        {
            string result = FileHelpers.ToBase64String(this.filePaths["EmptyFilePath"]);
            Assert.Equal(string.Empty, result);
        }

#if NOT_WORKING
        /// <summary>
        /// Verifies calling the method with a good file path returns the
        /// expected result.
        /// </summary>
        [Fact]
        public void ToBase64StringTest_GoodFile()
        {
            const string ExpectedResult =
                "77u/PD94bWwgdmVyc2lvbj0iMS4wIiBlbmNvZGluZz0idXRmLTgiID8+DQo8Y2F0YWxvZz4NCiAgPGJvb2tzPg0KICAgIDxib29rIGlkPSIwMSI+DQogICAgICA8YXV0aG9yPkdhbWJhcmRlbGxhLCBNYXR0aGV3PC9hdXRob3I+DQogICAgICA8ZGVzY3JpcHRpb24+QW4gaW4tZGVwdGggbG9vayBhdCBjcmVhdGluZyBhcHBsaWNhdGlvbnMgd2l0aCBYTUwuPC9kZXNjcmlwdGlvbj4NCiAgICAgIDx0aXRsZT5YTUwgRGV2ZWxvcGVyJ3MgR3VpZGU8L3RpdGxlPg0KICAgICAgPHllYXI+MjAwMDwveWVhcj4NCiAgICA8L2Jvb2s+DQogICAgPGJvb2sgaWQ9IjAyIj4NCiAgICAgIDxhdXRob3I+UmFsbHMsIEtpbTwvYXV0aG9yPg0KICAgICAgPGRlc2NyaXB0aW9uPkEgZm9ybWVyIGFyY2hpdGVjdCBiYXR0bGVzIGNvcnBvcmF0ZSB6b21iaWVzLCBhbiBldmlsIHNvcmNlcmVzcywgYW5kIGhlciBvd24gY2hpbGRob29kIHRvIGJlY29tZSBxdWVlbiBvZiB0aGUgd29ybGQuPC9kZXNjcmlwdGlvbj4NCiAgICAgIDx0aXRsZT5NaWRuaWdodCBSYWluPC90aXRsZT4NCiAgICAgIDx5ZWFyPjIwMDA8L3llYXI+DQogICAgPC9ib29rPg0KICAgIDxib29rIGlkPSIwMyI+DQogICAgICA8YXV0aG9yPktub3JyLCBTdGVmYW48L2F1dGhvcj4NCiAgICAgIDxkZXNjcmlwdGlvbj5BbiBhbnRob2xvZ3kgb2YgaG9ycm9yIHN0b3JpZXMgYWJvdXQgcm9hY2hlcywgY2VudGlwZWRlcywgc2NvcnBpb25zICBhbmQgb3RoZXIgaW5zZWN0cy48L2Rlc2NyaXB0aW9uPg0KICAgICAgPHRpdGxlPkNyZWVweSBDcmF3bGllczwvdGl0bGU+DQogICAgICA8eWVhcj4yMDAwPC95ZWFyPg0KICAgIDwvYm9vaz4NCiAgICA8Ym9vayBpZD0iMDQiPg0KICAgICAgPGF1dGhvcj5Db3JldHMsIEV2YTwvYXV0aG9yPg0KICAgICAgPGRlc2NyaXB0aW9uPkluIHBvc3QtYXBvY2FseXBzZSBFbmdsYW5kLCB0aGUgbXlzdGVyaW91cyBhZ2VudCBrbm93biBvbmx5IGFzIE9iZXJvbiBoZWxwcyB0byBjcmVhdGUgYSBuZXcgbGlmZSBmb3IgdGhlIGluaGFiaXRhbnRzIG9mIExvbmRvbi4gU2VxdWVsIHRvIE1hZXZlIEFzY2VuZGFudC48L2Rlc2NyaXB0aW9uPg0KICAgICAgPHRpdGxlPk9iZXJvbidzIExlZ2FjeTwvdGl0bGU+DQogICAgICA8eWVhcj4yMDAxPC95ZWFyPg0KICAgIDwvYm9vaz4NCiAgPC9ib29rcz4NCjwvY2F0YWxvZz4=";
            string result = FileHelpers.ToBase64String(this.filePaths["GoodXmlFilePath"]);
            Assert.Equal(ExpectedResult, result);
        }
#endif

        #endregion ToBase64String Tests

        #region FromBase64String Tests

        /// <summary>
        /// Verifies calling the method with an invalid file path throws the
        /// correct exception.
        /// </summary>
        [Theory]
        [InlineData(null, typeof(ArgumentNullException))]
        [InlineData("", typeof(ArgumentNullException))]
        [InlineData(@"C:\Temp", typeof(UnauthorizedAccessException))]
        [InlineData(@"Q:\kafekwaf\gnkf\fbksf.fth", typeof(DirectoryNotFoundException))]
        public void FromBase64StringTest_InvalidPaths(string filePath, Type expectedResult)
        {
            const string Data =
                "77u/PD94bWwgdmVyc2lvbj0iMS4wIiBlbmNvZGluZz0idXRmLTgiID8+DQo8Y2F0YWxvZz4NCiAgPGJvb2tzPg0KICAgIDxib29rIGlkPSIwMSI+DQogICAgICA8YXV0aG9yPkdhbWJhcmRlbGxhLCBNYXR0aGV3PC9hdXRob3I+DQogICAgICA8ZGVzY3JpcHRpb24+QW4gaW4tZGVwdGggbG9vayBhdCBjcmVhdGluZyBhcHBsaWNhdGlvbnMgd2l0aCBYTUwuPC9kZXNjcmlwdGlvbj4NCiAgICAgIDx0aXRsZT5YTUwgRGV2ZWxvcGVyJ3MgR3VpZGU8L3RpdGxlPg0KICAgICAgPHllYXI+MjAwMDwveWVhcj4NCiAgICA8L2Jvb2s+DQogICAgPGJvb2sgaWQ9IjAyIj4NCiAgICAgIDxhdXRob3I+UmFsbHMsIEtpbTwvYXV0aG9yPg0KICAgICAgPGRlc2NyaXB0aW9uPkEgZm9ybWVyIGFyY2hpdGVjdCBiYXR0bGVzIGNvcnBvcmF0ZSB6b21iaWVzLCBhbiBldmlsIHNvcmNlcmVzcywgYW5kIGhlciBvd24gY2hpbGRob29kIHRvIGJlY29tZSBxdWVlbiBvZiB0aGUgd29ybGQuPC9kZXNjcmlwdGlvbj4NCiAgICAgIDx0aXRsZT5NaWRuaWdodCBSYWluPC90aXRsZT4NCiAgICAgIDx5ZWFyPjIwMDA8L3llYXI+DQogICAgPC9ib29rPg0KICAgIDxib29rIGlkPSIwMyI+DQogICAgICA8YXV0aG9yPktub3JyLCBTdGVmYW48L2F1dGhvcj4NCiAgICAgIDxkZXNjcmlwdGlvbj5BbiBhbnRob2xvZ3kgb2YgaG9ycm9yIHN0b3JpZXMgYWJvdXQgcm9hY2hlcywgY2VudGlwZWRlcywgc2NvcnBpb25zICBhbmQgb3RoZXIgaW5zZWN0cy48L2Rlc2NyaXB0aW9uPg0KICAgICAgPHRpdGxlPkNyZWVweSBDcmF3bGllczwvdGl0bGU+DQogICAgICA8eWVhcj4yMDAwPC95ZWFyPg0KICAgIDwvYm9vaz4NCiAgICA8Ym9vayBpZD0iMDQiPg0KICAgICAgPGF1dGhvcj5Db3JldHMsIEV2YTwvYXV0aG9yPg0KICAgICAgPGRlc2NyaXB0aW9uPkluIHBvc3QtYXBvY2FseXBzZSBFbmdsYW5kLCB0aGUgbXlzdGVyaW91cyBhZ2VudCBrbm93biBvbmx5IGFzIE9iZXJvbiBoZWxwcyB0byBjcmVhdGUgYSBuZXcgbGlmZSBmb3IgdGhlIGluaGFiaXRhbnRzIG9mIExvbmRvbi4gU2VxdWVsIHRvIE1hZXZlIEFzY2VuZGFudC48L2Rlc2NyaXB0aW9uPg0KICAgICAgPHRpdGxlPk9iZXJvbidzIExlZ2FjeTwvdGl0bGU+DQogICAgICA8eWVhcj4yMDAxPC95ZWFyPg0KICAgIDwvYm9vaz4NCiAgPC9ib29rcz4NCjwvY2F0YWxvZz4=";
            Assert.Throws(expectedResult, () => FileHelpers.FromBase64String(filePath, Data));
        }

        /// <summary>
        /// Verifies writing good data to a good file path does not throw any
        /// exceptions.
        /// </summary>
        [Fact]
        public void FromBase64StringTest_GoodDataAndFilePath()
        {
            const string Data =
                "77u/PD94bWwgdmVyc2lvbj0iMS4wIiBlbmNvZGluZz0idXRmLTgiID8+DQo8Y2F0YWxvZz4NCiAgPGJvb2tzPg0KICAgIDxib29rIGlkPSIwMSI+DQogICAgICA8YXV0aG9yPkdhbWJhcmRlbGxhLCBNYXR0aGV3PC9hdXRob3I+DQogICAgICA8ZGVzY3JpcHRpb24+QW4gaW4tZGVwdGggbG9vayBhdCBjcmVhdGluZyBhcHBsaWNhdGlvbnMgd2l0aCBYTUwuPC9kZXNjcmlwdGlvbj4NCiAgICAgIDx0aXRsZT5YTUwgRGV2ZWxvcGVyJ3MgR3VpZGU8L3RpdGxlPg0KICAgICAgPHllYXI+MjAwMDwveWVhcj4NCiAgICA8L2Jvb2s+DQogICAgPGJvb2sgaWQ9IjAyIj4NCiAgICAgIDxhdXRob3I+UmFsbHMsIEtpbTwvYXV0aG9yPg0KICAgICAgPGRlc2NyaXB0aW9uPkEgZm9ybWVyIGFyY2hpdGVjdCBiYXR0bGVzIGNvcnBvcmF0ZSB6b21iaWVzLCBhbiBldmlsIHNvcmNlcmVzcywgYW5kIGhlciBvd24gY2hpbGRob29kIHRvIGJlY29tZSBxdWVlbiBvZiB0aGUgd29ybGQuPC9kZXNjcmlwdGlvbj4NCiAgICAgIDx0aXRsZT5NaWRuaWdodCBSYWluPC90aXRsZT4NCiAgICAgIDx5ZWFyPjIwMDA8L3llYXI+DQogICAgPC9ib29rPg0KICAgIDxib29rIGlkPSIwMyI+DQogICAgICA8YXV0aG9yPktub3JyLCBTdGVmYW48L2F1dGhvcj4NCiAgICAgIDxkZXNjcmlwdGlvbj5BbiBhbnRob2xvZ3kgb2YgaG9ycm9yIHN0b3JpZXMgYWJvdXQgcm9hY2hlcywgY2VudGlwZWRlcywgc2NvcnBpb25zICBhbmQgb3RoZXIgaW5zZWN0cy48L2Rlc2NyaXB0aW9uPg0KICAgICAgPHRpdGxlPkNyZWVweSBDcmF3bGllczwvdGl0bGU+DQogICAgICA8eWVhcj4yMDAwPC95ZWFyPg0KICAgIDwvYm9vaz4NCiAgICA8Ym9vayBpZD0iMDQiPg0KICAgICAgPGF1dGhvcj5Db3JldHMsIEV2YTwvYXV0aG9yPg0KICAgICAgPGRlc2NyaXB0aW9uPkluIHBvc3QtYXBvY2FseXBzZSBFbmdsYW5kLCB0aGUgbXlzdGVyaW91cyBhZ2VudCBrbm93biBvbmx5IGFzIE9iZXJvbiBoZWxwcyB0byBjcmVhdGUgYSBuZXcgbGlmZSBmb3IgdGhlIGluaGFiaXRhbnRzIG9mIExvbmRvbi4gU2VxdWVsIHRvIE1hZXZlIEFzY2VuZGFudC48L2Rlc2NyaXB0aW9uPg0KICAgICAgPHRpdGxlPk9iZXJvbidzIExlZ2FjeTwvdGl0bGU+DQogICAgICA8eWVhcj4yMDAxPC95ZWFyPg0KICAgIDwvYm9vaz4NCiAgPC9ib29rcz4NCjwvY2F0YWxvZz4=";
            string path = this.filePaths["GoodXmlFilePath"].Replace("Books1.xml", "Books_CopyF64Data.xml");
            FileHelpers.FromBase64String(path, Data);
            Assert.True(File.Exists(path));

            this.fixture.DeletePaths.Add(path);
        }

        /// <summary>
        /// Verifies writing empty data to a good file path does not throw any
        /// exceptions.
        /// </summary>
        [Fact]
        public void FromBase64StringTest_NullDataAndGoodFilePath()
        {
            string path = this.filePaths["GoodXmlFilePath"].Replace("Books1.xml", "Books_CopyF64Empty.xml");
            FileHelpers.FromBase64String(path, null);
            Assert.True(File.Exists(path));

            this.fixture.DeletePaths.Add(path);
        }

        /// <summary>
        /// Verifies writing base 64 string data to a file is the exact same
        /// when it's read back in from the file.
        /// </summary>
        [Fact]
        public void FromToBase64StringTest_RoundTrip()
        {
            const string Data =
                "77u/PD94bWwgdmVyc2lvbj0iMS4wIiBlbmNvZGluZz0idXRmLTgiID8+DQo8Y2F0YWxvZz4NCiAgPGJvb2tzPg0KICAgIDxib29rIGlkPSIwMSI+DQogICAgICA8YXV0aG9yPkdhbWJhcmRlbGxhLCBNYXR0aGV3PC9hdXRob3I+DQogICAgICA8ZGVzY3JpcHRpb24+QW4gaW4tZGVwdGggbG9vayBhdCBjcmVhdGluZyBhcHBsaWNhdGlvbnMgd2l0aCBYTUwuPC9kZXNjcmlwdGlvbj4NCiAgICAgIDx0aXRsZT5YTUwgRGV2ZWxvcGVyJ3MgR3VpZGU8L3RpdGxlPg0KICAgICAgPHllYXI+MjAwMDwveWVhcj4NCiAgICA8L2Jvb2s+DQogICAgPGJvb2sgaWQ9IjAyIj4NCiAgICAgIDxhdXRob3I+UmFsbHMsIEtpbTwvYXV0aG9yPg0KICAgICAgPGRlc2NyaXB0aW9uPkEgZm9ybWVyIGFyY2hpdGVjdCBiYXR0bGVzIGNvcnBvcmF0ZSB6b21iaWVzLCBhbiBldmlsIHNvcmNlcmVzcywgYW5kIGhlciBvd24gY2hpbGRob29kIHRvIGJlY29tZSBxdWVlbiBvZiB0aGUgd29ybGQuPC9kZXNjcmlwdGlvbj4NCiAgICAgIDx0aXRsZT5NaWRuaWdodCBSYWluPC90aXRsZT4NCiAgICAgIDx5ZWFyPjIwMDA8L3llYXI+DQogICAgPC9ib29rPg0KICAgIDxib29rIGlkPSIwMyI+DQogICAgICA8YXV0aG9yPktub3JyLCBTdGVmYW48L2F1dGhvcj4NCiAgICAgIDxkZXNjcmlwdGlvbj5BbiBhbnRob2xvZ3kgb2YgaG9ycm9yIHN0b3JpZXMgYWJvdXQgcm9hY2hlcywgY2VudGlwZWRlcywgc2NvcnBpb25zICBhbmQgb3RoZXIgaW5zZWN0cy48L2Rlc2NyaXB0aW9uPg0KICAgICAgPHRpdGxlPkNyZWVweSBDcmF3bGllczwvdGl0bGU+DQogICAgICA8eWVhcj4yMDAwPC95ZWFyPg0KICAgIDwvYm9vaz4NCiAgICA8Ym9vayBpZD0iMDQiPg0KICAgICAgPGF1dGhvcj5Db3JldHMsIEV2YTwvYXV0aG9yPg0KICAgICAgPGRlc2NyaXB0aW9uPkluIHBvc3QtYXBvY2FseXBzZSBFbmdsYW5kLCB0aGUgbXlzdGVyaW91cyBhZ2VudCBrbm93biBvbmx5IGFzIE9iZXJvbiBoZWxwcyB0byBjcmVhdGUgYSBuZXcgbGlmZSBmb3IgdGhlIGluaGFiaXRhbnRzIG9mIExvbmRvbi4gU2VxdWVsIHRvIE1hZXZlIEFzY2VuZGFudC48L2Rlc2NyaXB0aW9uPg0KICAgICAgPHRpdGxlPk9iZXJvbidzIExlZ2FjeTwvdGl0bGU+DQogICAgICA8eWVhcj4yMDAxPC95ZWFyPg0KICAgIDwvYm9vaz4NCiAgPC9ib29rcz4NCjwvY2F0YWxvZz4=";
            string path = this.filePaths["GoodXmlFilePath"].Replace("Books1.xml", "Books_CopyF64Round.xml");
            FileHelpers.FromBase64String(path, Data);
            Assert.True(File.Exists(path));

            this.fixture.DeletePaths.Add(path);

            string result = FileHelpers.ToBase64String(path);
            Assert.Equal(Data, result);
        }

        #endregion FromBase64String Tests
    }
}