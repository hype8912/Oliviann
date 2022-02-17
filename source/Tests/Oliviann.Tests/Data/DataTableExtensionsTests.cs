namespace Oliviann.Tests.Data
{
    #region Usings

    using System.Data;
    using Oliviann.Data;
    using Xunit;

    #endregion Usings

    [Trait("Category", "CI")]
    public class DataTableExtensionsTests
    {
        #region ColumnValuesToCollection Test

        /// <summary>
        /// Verifies an empty collection is return when a null table is passed
        /// in.
        /// </summary>
        [Fact]
        public void ColumnValuesToCollectionTest_NullTable()
        {
            DataTable table = null;
            var result = table.ColumnValuesToCollection<string>("Column3");

            Assert.NotNull(result);
            Assert.Empty(result);
        }

        /// <summary>
        /// Verifies an empty collection is return when an empty table is passed
        /// in.
        /// </summary>
        [Fact]
        public void ColumnValuesToCollectionTest_EmptyDataTable()
        {
            var table = new DataTable("Table1");
            table.Columns.Add("Column1", typeof(int));
            table.Columns.Add("Column2", typeof(string));
            table.Columns.Add("Column3", typeof(string));
            table.Columns.Add("Column4", typeof(string));

            var result = table.ColumnValuesToCollection<string>("Column3");

            Assert.NotNull(result);
            Assert.Empty(result);
        }

        /// <summary>
        /// Verifies an empty collection is return when an invalid column name
        /// is passed in.
        /// </summary>
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("Ninja1")]
        public void ColumnValuesToCollectionTest_InvalidColumnNames(string columnName)
        {
            var table = new DataTable("Table1");
            table.Columns.Add("Column1", typeof(int));
            table.Columns.Add("Column2", typeof(string));
            table.Columns.Add("Column3", typeof(string));
            table.Columns.Add("Column4", typeof(string));
            table.Rows.Add(1, "Bell", "Taco", "Fast Food");

            var result = table.ColumnValuesToCollection<string>(columnName);

            Assert.NotNull(result);
            Assert.Empty(result);
        }

        /// <summary>
        /// Verifies the correct collection data is returned when a valid column
        /// name is passed in.
        /// </summary>
        [Fact]
        public void ColumnValuesToCollectionTest_ValidData()
        {
            var table = new DataTable("Table1");
            table.Columns.Add("Column1", typeof(int));
            table.Columns.Add("Column2", typeof(string));
            table.Columns.Add("Column3", typeof(string));
            table.Columns.Add("Column4", typeof(string));
            table.Rows.Add(1, "Bell", "Taco", "Fast Food");
            table.Rows.Add(2, "King", "Burger", "Fast Food");
            table.Rows.Add(3, "Shake", "Steak &", "Fast Food");

            var result = table.ColumnValuesToCollection<string>("column2");

            Assert.NotNull(result);
            Assert.Equal(3, result.Count);
            Assert.Contains(result, val => val == "Bell");
            Assert.Contains(result, val => val == "King");
            Assert.Contains(result, val => val == "Shake");
        }

        #endregion

        #region SetColumnsReadOnly Tests

        /// <summary>
        /// Verifies a null data table returns a null value.
        /// </summary>
        [Fact]
        public void SetColumnsReadOnlyTest_NullTable()
        {
            DataTable table = null;
            DataTable result = table.SetColumnsReadOnly();

            Assert.Null(result);
        }

        /// <summary>
        /// Verifies a data table with no columns doesn't throw an error.
        /// </summary>
        [Fact]
        public void SetColumnsReadOnlyTest_TableWithNoColumns()
        {
            var table = new DataTable();
            DataTable result = table.SetColumnsReadOnly();

            Assert.NotNull(result);
            Assert.Empty(result.Columns);
        }

        /// <summary>
        /// Verifies a data table columns are set to read only.
        /// </summary>
        [Fact]
        public void SetColumnsReadOnlyTest_SetColumnsToReadOnly()
        {
            var table = new DataTable("People");
            table.Columns.Add("Id", typeof(int));
            table.Columns.Add("FirstName", typeof(string));
            table.Columns.Add("MiddleInitial", typeof(char));
            table.Columns.Add("LastName", typeof(string));
            table.Columns.Add("Comments", typeof(string));
            DataTable result = table.SetColumnsReadOnly();

            Assert.NotNull(result);
            foreach (DataColumn column in result.Columns)
            {
                Assert.True(column.ReadOnly);
            }
        }

        /// <summary>
        /// Verifies a data table columns are set to read only and back.
        /// </summary>
        [Fact]
        public void SetColumnsReadOnlyTest_SetColumnsToNotReadOnly()
        {
            var table = new DataTable("People");
            table.Columns.Add("Id", typeof(int));
            table.Columns.Add("FirstName", typeof(string));
            table.Columns.Add("MiddleInitial", typeof(char));
            table.Columns.Add("LastName", typeof(string));
            table.Columns.Add("Comments", typeof(string));
            DataTable result1 = table.SetColumnsReadOnly();

            Assert.NotNull(result1);
            foreach (DataColumn column in result1.Columns)
            {
                Assert.True(column.ReadOnly);
            }

            DataTable result2 = result1.SetColumnsReadOnly(false);
            Assert.NotNull(result2);
            foreach (DataColumn column in result2.Columns)
            {
                Assert.False(column.ReadOnly);
            }
        }

        #endregion

        #region ToCSV Test

        /// <summary>
        /// Verifies a null data table returns a null value.
        /// </summary>
        [Fact]
        public void ToCSVTests_NullTable()
        {
            DataTable table = null;
            string result = table.ToCSV();

            Assert.Empty(result);
        }

        /// <summary>
        /// Verifies an empty data table returns null.
        /// </summary>
        [Fact]
        public void ToCSVTests_EmptyTable()
        {
            var table = new DataTable("People");
            string result = table.ToCSV();

            Assert.Empty(result);
        }

        /// <summary>
        /// Verifies all the column headers are listed in the string.
        /// </summary>
        [Fact]
        public void ToCSVTests_TableWithHeadersOnly()
        {
            var table = new DataTable("People");
            table.Columns.Add("Id", typeof(int));
            table.Columns.Add("FirstName", typeof(string));
            table.Columns.Add("MiddleInitial", typeof(char));
            table.Columns.Add("LastName", typeof(string));
            table.Columns.Add("Comments", typeof(string));

            string result = table.ToCSV();

            Assert.Equal("Id,FirstName,MiddleInitial,LastName,Comments\r\n", result);
        }

        /// <summary>
        /// Verifies all the table data is converted correctly.
        /// </summary>
        [Fact]
        public void ToCSVTests_TableWithHeadersAndData()
        {
            var table = new DataTable("People");
            table.Columns.Add("Id", typeof(int));
            table.Columns.Add("FirstName", typeof(string));
            table.Columns.Add("MiddleInitial", typeof(char));
            table.Columns.Add("LastName", typeof(string));
            table.Columns.Add("Comments", typeof(string));

            table.Rows.Add(1, "Donald", 'J', "Trump", "This is some crazy stuff.");
            table.Rows.Add(2, "Juan", null, "Valdez", "Columbia, the great coffee house.");
            table.Rows.Add(3, "Bart", 'J', "Simpson", "Likes to say \"Ay, Caramba\".");

            string result = table.ToCSV();

            Assert.NotNull(result);
            Assert.Contains("Id,FirstName,MiddleInitial,LastName,Comments\r\n", result);
            Assert.Contains("1,Donald,J,Trump,This is some crazy stuff.\r\n", result);
            Assert.Contains("2,Juan,,Valdez,\"Columbia, the great coffee house.\"\r\n", result);
            Assert.Contains("3,Bart,J,Simpson,\"Likes to say \"\"Ay, Caramba\"\".\"\r\n", result);
        }

        #endregion ToCSV Test
    }
}