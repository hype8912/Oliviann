namespace Oliviann.Tests.IO.INI
{
    #region Usings

    using Oliviann.IO;
    using Xunit;

    #endregion Usings

    [Trait("Category", "CI")]
    public class FileContainerTests
    {
        [Fact]
        public void FileContainer_ctorTest_New()
        {
            var fc = new FileContainer();

            Assert.NotNull(fc.Sections);
            Assert.Empty(fc.Sections);
        }
    }
}