namespace Oliviann.Tests.IO
{
    #region Usings

    using System;
    using System.Collections.Generic;
    using System.IO;
    using Oliviann.IO;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Testing.Fixtures;
    using Xunit;
    using Assert = Xunit.Assert;

    #endregion Usings

    [Trait("Category", "CI")]
    [DeploymentItem(@"TestObjects\INI\Large_Unicode.ini")]
    [DeploymentItem(@"TestObjects\EmptyFile.txt")]
    public class DirectoryHelpersTests : IClassFixture<PathCleanupFixture>
    {
        #region Fields

        private readonly PathCleanupFixture fixture;
        private readonly string WorkingDirectory;
        private readonly string CopySourceDirectory;

        #endregion Fields

        #region Constructor/Destructor

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="DirectoryHelpersTests"/>class.
        /// </summary>
        /// <param name="fixture">The curent fixture.</param>
        public DirectoryHelpersTests(PathCleanupFixture fixture)
        {
            this.fixture = fixture;
            this.WorkingDirectory = this.fixture.CurrentDirectory + @"\TestObjects\TestWorkingDir";
            this.CopySourceDirectory = this.WorkingDirectory + @"\CopySource";
            this.fixture.DeletePaths.Add(this.WorkingDirectory);
        }

        #endregion Constructor/Destructor

        #region Exists

        [Fact]
        public void DirectoryExistsEnumTest_NullDirs()
        {
            IEnumerable<string> items = null;
            Assert.Throws<ArgumentNullException>(() => DirectoryHelpers.Exists(items));
        }

        [Fact]
        public void DirectoryExistsEnumTest_EmptyDirs()
        {
            IEnumerable<string> items = new List<string>();
            bool result = DirectoryHelpers.Exists(items);

            Assert.True(result);
        }

        [Fact]
        public void DirectoryExistsEnumTest_AllGoodDirs()
        {
            IEnumerable<string> items = new List<string> { @"C:", @"C:\Windows", @"C:\Temp" };
            bool result = DirectoryHelpers.Exists(items);

            Assert.True(result);
        }

        [Fact]
        public void DirectoryExistsEnumTest_MissingDir()
        {
            IEnumerable<string> items = new List<string> { @"C:", @"C:\Windows", @"C:\Temp", @"C:\WhatChaMaCallIt" };
            bool result = DirectoryHelpers.Exists(items);

            Assert.False(result);
        }

        [Fact]
        public void DirectoryExistsEnumTest_TrashDir()
        {
            IEnumerable<string> items = new List<string> { @"C:", @"C:\Windows", @"C:\Temp", @"LJNJD($&*NFK_@mfkl3450j_*&^" };
            bool result = DirectoryHelpers.Exists(items);

            Assert.False(result);
        }

        [Fact]
        public void DirectoryExistsEnumTest_NullDir()
        {
            IEnumerable<string> items = new List<string> { @"C:", @"C:\Windows", @"C:\Temp", null };
            bool result = DirectoryHelpers.Exists(items);

            Assert.False(result);
        }

        [Fact]
        public void DirectoryExistsArrayTest_NullDirs()
        {
            string[] items = null;
            Assert.Throws<ArgumentNullException>(() => DirectoryHelpers.Exists(items));
        }

        [Fact]
        public void DirectoryExistsArrayTest_EmptyDirs()
        {
            var items = new string[0];
            bool result = DirectoryHelpers.Exists(items);

            Assert.True(result);
        }

        [Fact]
        public void DirectoryExistsArrayTest_AllGoodDirs()
        {
            var items = new[] { @"C:", @"C:\Windows", @"C:\Temp" };
            bool result = DirectoryHelpers.Exists(items);

            Assert.True(result);
        }

        [Fact]
        public void DirectoryExistsArrayTest_MissingDir()
        {
            var items = new[] { @"C:", @"C:\Windows", @"C:\Temp", @"C:\WhatChaMaCallIt" };
            bool result = DirectoryHelpers.Exists(items);

            Assert.False(result);
        }

        [Fact]
        public void DirectoryExistsArrayTest_TrashDir()
        {
            var items = new[] { @"C:", @"C:\Windows", @"C:\Temp", @"LJNJD($&*NFK_@mfkl3450j_*&^" };
            bool result = DirectoryHelpers.Exists(items);

            Assert.False(result);
        }

        [Fact]
        public void DirectoryExistsArrayTest_NullDir()
        {
            var items = new[] { @"C:", @"C:\Windows", @"C:\Temp", null };
            bool result = DirectoryHelpers.Exists(items);

            Assert.False(result);
        }

        #endregion Exists

        #region Move Tests

        [Fact]
        public void DirectoryMoveTest_NullSource()
        {
            Assert.Throws<ArgumentNullException>(() => DirectoryHelpers.Move(null, null));
        }

        [Fact]
        public void DirectoryMoveTest_EmptySource()
        {
            Assert.Throws<ArgumentNullException>(() => DirectoryHelpers.Move(string.Empty, null));
        }

        [Fact]
        public void DirectoryMoveTest_NullDestDir()
        {
            string fromPath = Path.GetTempPath();
            Assert.Throws<ArgumentNullException>(() => DirectoryHelpers.Move(fromPath, null));
        }

        [Fact]
        public void DirectoryMoveTest_EmptyDestDir()
        {
            string fromPath = Path.GetTempPath();
            Assert.Throws<ArgumentNullException>(() => DirectoryHelpers.Move(fromPath, string.Empty));
        }

        [Fact]
        public void DirectoryMoveTest_MissingSourceDir()
        {
            string fromPath = @"A:\MissingDir";
            string toPath = this.WorkingDirectory + @"\MissingSourceDir";
            Assert.Throws<DirectoryNotFoundException>(() => DirectoryHelpers.Move(fromPath, toPath, true));
        }

        [Fact]
        public void DirectoryMoveTest_SameSourceDir()
        {
            this.CreateWorkingDirectory();
            string fromPath = this.CopySourceDirectory;
            string toPath = this.CopySourceDirectory;
            Assert.Throws<IOException>(() => DirectoryHelpers.Move(fromPath, toPath, true));
        }

        [Fact]
        public void DirectoryMoveTest_DifferentDir()
        {
            this.CreateWorkingDirectory();
            string fromPath = this.CopySourceDirectory;
            string toPath = this.WorkingDirectory + @"\MoveDest";
            DirectoryHelpers.Move(fromPath, toPath, true);

            Assert.True(Directory.Exists(toPath));
            Assert.True(File.Exists(toPath + @"\Books1.xml"));
            Assert.True(File.Exists(toPath + @"\Large_Unicode.ini"));
            Assert.True(File.Exists(toPath + @"\EmptyFile.txt"));

            this.fixture.DeletePaths.Add(toPath);
        }

        /// <remarks>
        /// This test will fail if you are not connected to the Oliviann network
        /// and have permissions to the file share.
        /// </remarks>
        [Fact]
        public void DirectoryMoveTest_NetworkDir()
        {
            this.CreateWorkingDirectory();
            string fromPath = this.CopySourceDirectory;
#if NET40 || NET45
            string toPath = @"\\se\data\SOA_Share\Test\Net45" + @"\MoveDestTest";
#endif

#if NET46
            string toPath = @"\\se\data\SOA_Share\Test\Net46" + @"\MoveDestTest";
#endif

#if NET47
            string toPath = @"\\se\data\SOA_Share\Test\Net47" + @"\MoveDestTest";
#endif

            DirectoryHelpers.Move(fromPath, toPath, true);

            Assert.True(Directory.Exists(toPath));
            Assert.True(File.Exists(toPath + @"\Books1.xml"));
            Assert.True(File.Exists(toPath + @"\Large_Unicode.ini"));
            Assert.True(File.Exists(toPath + @"\EmptyFile.txt"));

            this.fixture.DeletePaths.Add(toPath);
        }

#endregion Move Tests

        #region Copy Tests

        [Fact]
        public void DirectoryCopyTest_NullSource()
        {
            Assert.Throws<ArgumentNullException>(() => DirectoryHelpers.Copy(null, null));
        }

        [Fact]
        public void DirectoryCopyTest_EmptySource()
        {
            Assert.Throws<ArgumentNullException>(() => DirectoryHelpers.Copy(string.Empty, null));
        }

        [Fact]
        public void DirectoryCopyTest_NullDestDir()
        {
            string fromPath = Path.GetTempPath();
            Assert.Throws<ArgumentNullException>(() => DirectoryHelpers.Copy(fromPath, null));
        }

        [Fact]
        public void DirectoryCopyTest_EmptyDestDir()
        {
            string fromPath = Path.GetTempPath();
            Assert.Throws<ArgumentNullException>(() => DirectoryHelpers.Copy(fromPath, string.Empty));
        }

        [Fact]
        public void DirectoryCopyTest_MissingSourceDir()
        {
            string fromPath = @"A:\MissingDir";
            string toPath = @"C:\Temp\Test";
            Assert.Throws<DirectoryNotFoundException>(() => DirectoryHelpers.Copy(fromPath, toPath, true));
        }

        [Fact]
        public void DirectoryCopyTest_SameSourceDir()
        {
            this.CreateWorkingDirectory();
            string fromPath = this.CopySourceDirectory;
            string toPath = this.CopySourceDirectory;
            Assert.Throws<IOException>(() => DirectoryHelpers.Copy(fromPath, toPath, true));
        }

        [Fact]
        public void DirectoryCopyTest_DifferentDir()
        {
            this.CreateWorkingDirectory();
            string fromPath = this.CopySourceDirectory;
            string toPath = this.WorkingDirectory + @"\CopyDest";
            DirectoryHelpers.Copy(fromPath, toPath, true);

            Assert.True(Directory.Exists(toPath));
            Assert.True(File.Exists(toPath + @"\Books1.xml"));
            Assert.True(File.Exists(toPath + @"\Large_Unicode.ini"));
            Assert.True(File.Exists(toPath + @"\EmptyFile.txt"));

            this.fixture.DeletePaths.Add(toPath);
        }

        #endregion Copy Tests

        #region Helpers

        private void CreateWorkingDirectory()
        {
            if (!Directory.Exists(this.WorkingDirectory))
            {
                Directory.CreateDirectory(this.WorkingDirectory);
                Assert.True(Directory.Exists(this.WorkingDirectory));
            }

            if (!Directory.Exists(this.CopySourceDirectory))
            {
                Directory.CreateDirectory(this.CopySourceDirectory);
                Assert.True(Directory.Exists(this.CopySourceDirectory));
            }

            File.WriteAllText(this.CopySourceDirectory + @"\Books1.xml", Common.TestObjects.Properties.Resources.Books1);
            File.Copy(this.fixture.CurrentDirectory + @"\TestObjects\INI\Large_Unicode.ini", this.CopySourceDirectory + @"\Large_Unicode.ini", true);
            File.Copy(this.fixture.CurrentDirectory + @"\TestObjects\EmptyFile.txt", this.CopySourceDirectory + @"\EmptyFile.txt", true);
        }

        #endregion Helpers
    }
}