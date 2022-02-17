namespace Oliviann.Tests.IO
{
    #region Usings

    using System;
    using System.IO;
    using Oliviann.IO;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Testing.Fixtures;
    using Xunit;
    using Assert = Xunit.Assert;

    #endregion Usings

    [Trait("Category", "CI")]
    [DeploymentItem(@"TestObjects\INI\AutoImports.ini")]
    public class FileInfoExtensionsTests : IClassFixture<PathCleanupFixture>
    {
        #region Fields

        private readonly string GoodFilePath;

        private readonly PathCleanupFixture fixture;

        #endregion Fields

        #region Constructor/Destructor

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="FileInfoExtensionsTests"/> class.
        /// </summary>
        /// <param name="fixture">The current fixture.</param>
        public FileInfoExtensionsTests(PathCleanupFixture fixture)
        {
            this.fixture = fixture;
            this.GoodFilePath = Path.Combine(this.fixture.CurrentDirectory, @"TestObjects\INI\AutoImports.ini");
        }

        #endregion Constructor/Destructor

        #region IsFileLocked Tests

        [Fact]
        public void IsFileLockedTest_NullInfo()
        {
            FileInfo info = null;
            Assert.Throws<ArgumentNullException>(() => info.IsFileLocked());
        }

        [Fact]
        public void IsFileLockedTest_NonLockedFile()
        {
            string path = this.GoodFilePath.Replace("AutoImports.ini", "AutoImports_Copy12.ini");
            File.Copy(this.GoodFilePath, path, true);
            Assert.True(File.Exists(path), "File did not copy.");

            var info = new FileInfo(path);
            bool result = info.IsFileLocked();

            Assert.False(result, "File was found to be locked.");
            this.fixture.DeletePaths.Add(path);
        }

        [Fact]
        public void IsFileLockedTest_SimulatedLockedFile()
        {
            string path = this.GoodFilePath.Replace("AutoImports.ini", "AutoImports_Copy.ini");
            File.Copy(this.GoodFilePath, path);
            Assert.True(File.Exists(path), "File did not copy.");
            this.fixture.DeletePaths.Add(path);

            var info = new FileInfo(path);
            FileStream stream = info.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            bool result1 = info.IsFileLocked();
            stream.Close();

            Assert.True(result1, "File was not locked.");
            Assert.False(info.IsFileLocked(), "File was found to be locked.");
        }

        #endregion IsFileLocked Tests

        #region DeleteReadOnlyFile Tests

        [Fact]
        public void DeleteReadOnlyFileTest_NullInfo()
        {
            FileInfo info = null;
            info.DeleteReadOnlyFile();
        }

        [Fact]
        public void DeleteReadOnlyFileTest_NonReadonlyFile()
        {
            string path = this.GoodFilePath.Replace("AutoImports.ini", "AutoImports_Copy1.ini");
            File.Copy(this.GoodFilePath, path, true);
            Assert.True(File.Exists(path), "File did not copy.");

            var info = new FileInfo(path);
            info.DeleteReadOnlyFile();
            Assert.False(File.Exists(path), "File did not get deleted.");
        }

        [Fact]
        public void DeleteReadOnlyFileTest_ReadonlyFile()
        {
            string path = this.GoodFilePath.Replace("AutoImports.ini", "AutoImports_Copy2.ini");
            File.Copy(this.GoodFilePath, path, true);
            Assert.True(File.Exists(path), "File did not copy.");

            File.SetAttributes(path, FileAttributes.ReadOnly);

            var info = new FileInfo(path);
            info.DeleteReadOnlyFile();
            Assert.False(File.Exists(path), "File did not get deleted.");
        }

        #endregion DeleteReadOnlyFile Tests

        #region NameWithoutExtension Tests

        [Fact]
        public void NameWithoutExtensionTest_NullInfo()
        {
            FileInfo info = null;
            Assert.Throws<ArgumentNullException>(() => info.NameWithoutExtension());
        }

        [Fact]
        public void NameWithoutExtensionTest_GoodInfo()
        {
            var info = new FileInfo(this.GoodFilePath);
            string result = info.NameWithoutExtension();

            Assert.Equal("AutoImports", result);
        }

        #endregion NameWithoutExtension Tests
    }
}