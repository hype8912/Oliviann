namespace Oliviann.Tests.IO
{
    #region Usings

    using System;
    using System.Diagnostics;
    using System.IO;
    using Oliviann.IO;
    using Testing.Fixtures;
    using Xunit;

    #endregion Usings

    [Trait("Category", "CI")]
    public class DirectoryInfoExtensionsTests : IClassFixture<PathCleanupFixture>
    {
        #region Fields

        private PathCleanupFixture fixture;

        #endregion Fields

        #region Constructors/Destructor

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="DirectoryInfoExtensionsTests"/> class.
        /// </summary>
        /// <param name="fixture">The fixture.</param>
        public DirectoryInfoExtensionsTests(PathCleanupFixture fixture)
        {
            this.fixture = fixture;
        }

        #endregion Constructors/Destructor

        /// <summary>
        /// Verifies an ArgumentNull exception is thrown.
        /// </summary>
        [Fact]
        public void SizeTest_NullObject()
        {
            DirectoryInfo info = null;
            Assert.Throws<ArgumentNullException>(() => info.Size());
        }

        /// <summary>
        /// Verifies an empty directory return a zero size.
        /// </summary>
        [Fact]
        public void SizeTest_EmptyDirectory()
        {
            string emptyDir = Path.Combine(this.fixture.CurrentDirectory, "EmptyDir");
            Directory.CreateDirectory(emptyDir);

            long total = new DirectoryInfo(emptyDir).Size();
            Assert.Equal(0, total);

            this.fixture.DeletePaths.Add(emptyDir);
        }

        /// <summary>
        /// Verifies a directory only calculates the size of the current top
        /// directory and no sub-directories.
        /// </summary>
        [Fact]
        public void SizeTest_TopDirectoryOnly()
        {
            string directoryPath = Path.GetTempPath();

            var dir = new DirectoryInfo(directoryPath);
            long topDirectoryTotal = dir.Size(false);
            long subDirectoryTotal = dir.Size();

            Trace.WriteLine("Top Directory Size: " + topDirectoryTotal);
            Trace.WriteLine("Sub Directory Size: " + subDirectoryTotal);
            Assert.True(topDirectoryTotal > 0, "Total is incorrect.[{0}]".FormatWith(topDirectoryTotal));
            Assert.True(
                topDirectoryTotal < subDirectoryTotal,
                "Top directory total is equal to or larger than the sub-directory total.");
        }

        /// <summary>
        /// Verifies a directory calculates the size of the top-level files and
        /// all the sub-directories files.
        /// </summary>
        [Fact]
        public void SizeTest_AllSubDirectories_WithParsePoints()
        {
            string directoryPath = Environment.GetEnvironmentVariable("USERPROFILE");
            long total = new DirectoryInfo(directoryPath).Size();

            Trace.WriteLine("Directory Size: " + total);
            Assert.True(total > 0, "Total is incorrect.[{0}]".FormatWith(total));
        }
    }
}