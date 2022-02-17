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
        #region CompileDelete Tests

        [Fact]
        public void CompileDeleteTest_Valid()
        {
            string expectedResult = "DELETE FROM [Books] WHERE [id] = @p0";

            var builder = new DbQueryStringBuilder<Book>(this.bk1, new SqlServerCompiler());
            Assert.Empty(builder.Parameters);

            string result = builder.CompileDelete();
            Assert.Equal(expectedResult, result);
            Assert.Equal(1, builder.Parameters.Count);

            VerifyParameter(builder.Parameters, "@p0", this.bk1.Id);
        }

        [Fact]
        public void CompileDeleteByKeyTest_NoAttributeKeys()
        {
            var builder = new DbQueryStringBuilder<Book2>(new Book2(), new SqlServerCompiler());
            Assert.Empty(builder.Parameters);

            Assert.Throws<KeyNotFoundException>(() => builder.CompileDelete());
        }

        [Fact]
        public void CompileDeleteTest_StringKeys()
        {
            string expectedResult = "DELETE FROM [Books] WHERE [title] = @p0 AND [year] = @p1";

            var builder = new DbQueryStringBuilder<Book>(this.bk1, new SqlServerCompiler());
            Assert.Empty(builder.Parameters);

            string result = builder.CompileDelete("title", "year");
            Assert.Equal(expectedResult, result);
            Assert.Equal(2, builder.Parameters.Count);

            VerifyParameter(builder.Parameters, "@p0", this.bk1.Title);
            VerifyParameter(builder.Parameters, "@p1", this.bk1.Year);
        }

        #endregion
    }
}