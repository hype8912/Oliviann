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
        public void CompileInsertTest_Valid()
        {
            string expectedResult = "INSERT INTO [Books] ([description], [id], [title], [year], [author]) VALUES (@p0, @p1, @p2, @p3, @p4)";

            var builder = new DbQueryStringBuilder<Book>(this.bk1, new SqlServerCompiler());
            Assert.Empty(builder.Parameters);

            string result = builder.CompileInsert();
            Assert.Equal(expectedResult, result);
            Assert.Equal(5, builder.Parameters.Count);

            VerifyParameter(builder.Parameters, "@p0", this.bk1.Description);
            VerifyParameter(builder.Parameters, "@p1", this.bk1.Id);
            VerifyParameter(builder.Parameters, "@p2", this.bk1.Title);
            VerifyParameter(builder.Parameters, "@p3", this.bk1.Year);
            VerifyParameter(builder.Parameters, "@p4", this.bk1.Author);
        }

        [Fact]
        public void CompileInsertTest_StringKeys()
        {
            var bk = new Book2
            {
                Id = this.bk1.Id,
                Author = this.bk1.Author,
                Description = this.bk1.Description,
                Title = this.bk1.Title,
                Year = this.bk1.Year
            };

            string expectedResult = "INSERT INTO [Book2] ([Author], [Description], [Id], [Notes], [Title], [Year]) VALUES (@p0, @p1, @p2, @p3, @p4, @p5)";

            var builder = new DbQueryStringBuilder<Book2>(bk, new SqlServerCompiler());
            Assert.Empty(builder.Parameters);

            string result = builder.CompileInsert();
            Assert.Equal(expectedResult, result);
            Assert.Equal(6, builder.Parameters.Count);

            VerifyParameter(builder.Parameters, "@p0", bk.Author);
            VerifyParameter(builder.Parameters, "@p1", bk.Description);
            VerifyParameter(builder.Parameters, "@p2", bk.Id);
            VerifyParameter(builder.Parameters, "@p3", bk.Notes);
            VerifyParameter(builder.Parameters, "@p4", bk.Title);
            VerifyParameter(builder.Parameters, "@p5", bk.Year);
        }

        #endregion
    }
}