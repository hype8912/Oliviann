namespace Oliviann.Testing.Tests.Fixtures
{
    #region Usings

    using System.IO;
    using System.Reflection;
    using Oliviann.Reflection;
    using Oliviann.Testing.Fixtures;
    using Xunit;

    #endregion Usings

    [Trait("Category", "CI")]
    public class PathCleanupFixtureTests
    {
        /// <summary>
        /// Verifies a default instance loads correctly.
        /// </summary>
        [Fact]
        public void PathCleanupFixtureTest_DefaultInstance()
        {
            var fixture = new PathCleanupFixture();

            string location = Assembly.GetExecutingAssembly().GetCurrentExecutingDirectory();
            Assert.Equal(fixture.CurrentDirectory, location);
            Assert.NotNull(fixture.DeletePaths);
            Assert.Empty(fixture.DeletePaths);
        }

        /// <summary>
        /// Verifies no exceptions are thrown and the files are deleted on
        /// location.
        /// </summary>
        [Fact]
        public void PathCleanupFixtureTest_DisposePaths()
        {
            string temppath = Path.GetTempPath();
            string dirPath = Path.Combine(temppath, StringHelpers.GenerateRandomString(10));
            Directory.CreateDirectory(dirPath);

            string filePath = Path.GetTempFileName();
            string missingPath = Path.Combine(temppath, StringHelpers.GenerateRandomString(10) + ".txt");

            var fixture = new PathCleanupFixture();
            fixture.DeletePaths.Add(filePath);
            fixture.DeletePaths.Add(missingPath);
            fixture.DeletePaths.Add(dirPath);

            Assert.Equal(3, fixture.DeletePaths.Count);
            fixture.Dispose();
        }
    }
}