namespace Oliviann.Data.Common.Tests
{
    #region Usings

    using System;
    using System.Collections.Generic;
    using SqlKata.Compilers;
    using Xunit;

    #endregion

    /// <summary>
    /// Represents a
    /// </summary>
    public partial class DbQueryStringBuilderTests
    {
        #region CompileUpdate Tests

        [Fact]
        public void CompileUpdateTest_Valid()
        {
            string expectedResult = "UPDATE [Books] SET [description] = @p0, [title] = @p1, [year] = @p2, [author] = @p3 WHERE [id] = @p4";

            var builder = new DbQueryStringBuilder<Book>(this.bk1, new SqlServerCompiler());
            Assert.Empty(builder.Parameters);

            string result = builder.CompileUpdate();
            Assert.Equal(expectedResult, result);
            Assert.Equal(5, builder.Parameters.Count);

            VerifyParameter(builder.Parameters, "@p0", this.bk1.Description);
            VerifyParameter(builder.Parameters, "@p1", this.bk1.Title);
            VerifyParameter(builder.Parameters, "@p2", this.bk1.Year);
            VerifyParameter(builder.Parameters, "@p3", this.bk1.Author);
            VerifyParameter(builder.Parameters, "@p4", this.bk1.Id);
        }

        [Fact]
        public void CompileUpdateByKeyTest_NoAttributeKeys()
        {
            var builder = new DbQueryStringBuilder<Book2>(new Book2(), new SqlServerCompiler());
            Assert.Empty(builder.Parameters);

            Assert.Throws<KeyNotFoundException>(() => builder.CompileUpdate());
        }

        [Fact]
        public void CompileUpdateTest_StringKeys()
        {
            string expectedResult = "UPDATE [Books] SET [description] = @p0, [id] = @p1, [author] = @p2 WHERE [title] = @p3 AND [year] = @p4";

            var builder = new DbQueryStringBuilder<Book>(this.bk1, new SqlServerCompiler());
            Assert.Empty(builder.Parameters);

            string result = builder.CompileUpdate("title", "year");
            Assert.Equal(expectedResult, result);
            Assert.Equal(5, builder.Parameters.Count);

            VerifyParameter(builder.Parameters, "@p0", this.bk1.Description);
            VerifyParameter(builder.Parameters, "@p1", this.bk1.Id);
            VerifyParameter(builder.Parameters, "@p2", this.bk1.Author);
            VerifyParameter(builder.Parameters, "@p3", this.bk1.Title);
            VerifyParameter(builder.Parameters, "@p4", this.bk1.Year);
        }

        #endregion
    }
}