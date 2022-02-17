namespace Oliviann.Data.Common.Tests
{
    #region Usings

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Reflection;
    using SqlKata.Compilers;
    using Xunit;

    #endregion

    /// <summary>
    /// Represents a
    /// </summary>
    [Trait("Category", "CI")]
    public partial class DbQueryStringBuilderTests
    {
        #region Fields

        private Book bk1 = new Book
        {
            Id = 1,
            Author = "Gambardella, Matthew",
            Description = "An in-depth look at creating applications with XML.",
            Title = "XML Developer's Guide",
            Year = 2000,
            Notes = "This book talks about XML stuff"
        };

        #endregion

        #region Constructor/Destructor

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="DbQueryStringBuilderTests"/> class.
        /// </summary>
        public DbQueryStringBuilderTests()
        {
        }

        #endregion

        #region GetFilteredParameters Tests

        [Fact]
        public void GetFilteredParametersTest_NullExcludedParametersWithValues()
        {
            var wrapper = new DbQueryStringBuilderWrapper<Book>(this.bk1, new SqlServerCompiler()) { ExcludedParameters = null };
            var result = wrapper.GetFilteredParameters(true);
            Assert.Equal(5, result.Count);

            VerifyColumn(result, -1, "id", true, this.bk1.Id);
            VerifyColumn(result, 1, "author", false, this.bk1.Author);
            VerifyColumn(result, -1, "description", false, this.bk1.Description);
            VerifyColumn(result, -1, "title", false, this.bk1.Title);
            VerifyColumn(result, -1, "year", false, this.bk1.Year);
        }

        [Fact]
        public void GetFilteredParametersTest_AllParamsExcluded()
        {
            var wrapper = new DbQueryStringBuilderWrapper<Book>(this.bk1, new SqlServerCompiler())
                { ExcludedParameters = new List<string> { "id", "author", "description", "title", "year" } };
            var result = wrapper.GetFilteredParameters(true);

            Assert.Empty(result);
        }

        [Fact]
        public void GetFilteredParametersTest_NoExcludedParametersNoValues()
        {
            var wrapper = new DbQueryStringBuilderWrapper<Book>(this.bk1, new SqlServerCompiler()) { ExcludedParameters = null };
            var result = wrapper.GetFilteredParameters(false);
            Assert.Equal(5, result.Count);

            VerifyColumn(result, -1, "id", true, null);
            VerifyColumn(result, 1, "author", false, null);
            VerifyColumn(result, -1, "description", false, null);
            VerifyColumn(result, -1, "title", false, null);
            VerifyColumn(result, -1, "year", false, null);
        }

        #endregion

        #region GetTypeData Tests

        [Fact]
        public void GetTypeDataTest_NoValuesNullStringKeys()
        {
            var wrapper = new DbQueryStringBuilderWrapper<Book>(this.bk1, new SqlServerCompiler());
            var result = wrapper.GetTypeData(false, null);

            Assert.Equal("Books", result.Name);
            Assert.Equal(5, result.Columns.Count);
        }

        [Fact]
        public void GetTypeDataTest_NoValuesEmptyStringKeys()
        {
            var wrapper = new DbQueryStringBuilderWrapper<Book>(this.bk1, new SqlServerCompiler());
            var result = wrapper.GetTypeData(false, new string[0]);

            Assert.Equal("Books", result.Name);
            Assert.Equal(5, result.Columns.Count);
        }

        [Fact]
        public void GetTypeDataTest_WithKeyColumnNames()
        {
            var wrapper = new DbQueryStringBuilderWrapper<Book>(this.bk1, new SqlServerCompiler());
            var result = wrapper.GetTypeData(true, new[] { "title", "year" });

            Assert.Equal("Books", result.Name);
            VerifyColumn(result.Columns, -1, "id", false, this.bk1.Id);
            VerifyColumn(result.Columns, 1, "author", false, this.bk1.Author);
            VerifyColumn(result.Columns, -1, "description", false, this.bk1.Description);
            VerifyColumn(result.Columns, -1, "title", true, this.bk1.Title);
            VerifyColumn(result.Columns, -1, "year", true, this.bk1.Year);
        }

        #endregion

        #region GetTableName Test

        [Fact]
        public void GetTableNameTest_NullMemberInfo()

        {
            var wrapper = new DbQueryStringBuilderWrapper<Book>(this.bk1, new SqlServerCompiler());
            Assert.Throws<ArgumentNullException>(() => wrapper.GetTableName(null));
        }

        [Fact]
        public void GetTableNameTest_NonAttributeClass()
        {
            var wrapper = new DbQueryStringBuilderWrapper<Book2>(null, new SqlServerCompiler());
            string result = wrapper.GetTableName(typeof(Book2));

            Assert.Equal("Book2", result);
        }

        [Fact]
        public void GetTableNameTest_TableAttributeClass()
        {
            var wrapper = new DbQueryStringBuilderWrapper<Book>(this.bk1, new SqlServerCompiler());
            string result = wrapper.GetTableName(typeof(Book));

            Assert.Equal("Books", result);
        }

        #endregion

        #region GetColumn Tests

        [Fact]
        public void GetColumnTest_NullPropertyName()
        {
            var wrapper = new DbQueryStringBuilderWrapper<Book>(this.bk1, new SqlServerCompiler());
            Assert.Throws<ArgumentNullException>(() => wrapper.GetColumn(null, new Attribute[0]));
        }

        [Fact]
        public void GetColumnTest_KeyAndColumnMemberInfo()
        {
            MemberInfo prop = typeof(Book).GetProperty("Id");
            var wrapper = new DbQueryStringBuilderWrapper<Book>(this.bk1, new SqlServerCompiler());
            ColumnData result = wrapper.GetColumn(prop.Name, prop.GetCustomAttributesCached(false));

            Assert.Equal(-1, result.Order);
            Assert.Equal("id", result.Name);
            Assert.True(result.IsPrimaryKey);
        }

        [Fact]
        public void GetColumnTest_ColumnMemberInfo()
        {
            MemberInfo prop = typeof(Book).GetProperty("Author");
            var wrapper = new DbQueryStringBuilderWrapper<Book>(this.bk1, new SqlServerCompiler());
            ColumnData result = wrapper.GetColumn(prop.Name, prop.GetCustomAttributesCached(false));

            Assert.Equal(1, result.Order);
            Assert.Equal("author", result.Name);
            Assert.False(result.IsPrimaryKey);
        }

        [Fact]
        public void GetColumnTest_NotMappedMemberInfo()
        {
            MemberInfo prop = typeof(Book).GetProperty("Notes");
            var wrapper = new DbQueryStringBuilderWrapper<Book>(this.bk1, new SqlServerCompiler());
            ColumnData result = wrapper.GetColumn(prop.Name, prop.GetCustomAttributesCached(false));

            Assert.Equal(-1, result.Order);
            Assert.Equal("Notes", result.Name);
            Assert.False(result.IsPrimaryKey);
        }

        #endregion

        #region Helper Methods

        private void VerifyColumn(IEnumerable<ColumnData> result, int order, string name, bool key, object value)
        {
            var column = result.First(c => c.Name == name);
            Assert.Equal(order, column.Order);
            Assert.Equal(key, column.IsPrimaryKey);
            Assert.Equal(value, column.Value);
        }

        private void VerifyParameter(IDictionary<string, object> result, string name, object value)
        {
            Assert.True(result.ContainsKey(name));
            Assert.Equal(value, result[name]);
        }

        #endregion

        #region Wrapper Class

        internal class DbQueryStringBuilderWrapper<T> : DbQueryStringBuilder<T>
        {
            public DbQueryStringBuilderWrapper(T entity, Compiler compilerType) : base(entity, compilerType)
            {
            }

            public TableData GetTypeData(bool includeValues, string[] keyColumnNames = null)
                => base.GetTypeData(includeValues, keyColumnNames);

            public List<ColumnData> GetFilteredParameters(bool includeValues)
                => base.GetFilteredParameters(includeValues);

            public string GetTableName(MemberInfo currentType) => base.GetTableName(currentType);

            public ColumnData GetColumn(string propertyName, Attribute[] attributes) => base.GetColumn(propertyName, attributes);
        }

        #endregion
    }
}