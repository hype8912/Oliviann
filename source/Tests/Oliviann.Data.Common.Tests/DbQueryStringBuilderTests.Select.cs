namespace Oliviann.Data.Common.Tests
{
    #region Usings

    using System;
    using SqlKata.Compilers;
    using Xunit;

    #endregion

    /// <summary>
    /// Represents a
    /// </summary>
    public partial class DbQueryStringBuilderTests
    {
        #region CompileSelect Tests

        [Fact]
        public void CompileSelectTest_SqlServer_Valid()
        {
            string expectedResult = "SELECT [description], [id], [title], [year], [author] FROM [Books]";

            var builder = new SqlQueryStringBuilder<Book>(this.bk1);
            Assert.Empty(builder.Parameters);

            string result = builder.CompileSelect();
            Assert.Equal(expectedResult, result);
            Assert.Equal(0, builder.Parameters.Count);
        }

        [Fact]
        public void CompileSelectTest_SqlServer_MissingMember()
        {
            var builder = new DbQueryStringBuilder<Book>(this.bk1, new SqlServerCompiler());
            builder.ExcludedParameters.AddRange(new [] { "id", "author", "description", "title", "year" });

            Assert.Throws<MissingMemberException>(() => builder.CompileSelect());
        }

        #endregion

        #region CompileSelectByKey Tests

        [Fact]
        public void CompileSelectByKeyTest_Valid()
        {
            string expectedResult = "SELECT [description], [id], [title], [year], [author] FROM [Books] WHERE [id] = @p0";

            var builder = new DbQueryStringBuilder<Book>(this.bk1, new SqlServerCompiler());
            Assert.Empty(builder.Parameters);

            string result = builder.CompileSelectByKey();
            Assert.Equal(expectedResult, result);
            Assert.Equal(1, builder.Parameters.Count);

            VerifyParameter(builder.Parameters, "@p0", this.bk1.Id);
        }

        [Fact]
        public void CompileSelectByKeyTest_NoAttributeKeys()
        {
            string expectedResult = "SELECT [Author], [Description], [Id], [Notes], [Title], [Year] FROM [Book2]";

            var builder = new DbQueryStringBuilder<Book2>(new Book2(), new SqlServerCompiler());
            Assert.Empty(builder.Parameters);

            string result = builder.CompileSelectByKey();
            Assert.Equal(expectedResult, result);
            Assert.Empty(builder.Parameters);
        }

        [Fact]
        public void CompileSelectByKeyTest_StringKeys()
        {
            string expectedResult = "SELECT [description], [id], [title], [year], [author] FROM [Books] WHERE [title] = @p0 AND [year] = @p1";

            var builder = new DbQueryStringBuilder<Book>(this.bk1, new SqlServerCompiler());
            Assert.Empty(builder.Parameters);

            string result = builder.CompileSelectByKey("title", "year");
            Assert.Equal(expectedResult, result);
            Assert.Equal(2, builder.Parameters.Count);

            VerifyParameter(builder.Parameters, "@p0", this.bk1.Title);
            VerifyParameter(builder.Parameters, "@p1", this.bk1.Year);
        }

        #endregion
    }
}