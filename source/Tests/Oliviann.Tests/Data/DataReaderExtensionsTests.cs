namespace Oliviann.Tests.Data
{
    #region Usings

    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Oliviann.Collections.Generic;
    using Oliviann.Data;
    using Xunit;

    #endregion Usings

    [Trait("Category", "CI")]
    public class DataReaderExtensionsTests
    {
        #region AsEnumerable Tests

        [Fact]
        public void DataReaderAsEnumerableTest_Null()
        {
            IDataReader reader = null;
            IEnumerable<IDataRecord> records = DataReaderExtensions.AsEnumerable(reader);
            Assert.Throws<ArgumentNullException>(() => records.ForEach(r => { }));
        }

        [Fact]
        public void DataReaderAsEnumerableTest_DataTable_WithData()
        {
            DataTable dt = this.CreateAndLoadDataTable("Temp1");
            DataTableReader reader = dt.CreateDataReader();

            int count = reader.AsEnumerable().Count();
            dt.DisposeSafe();

            Assert.Equal(3, count);
        }

        #endregion AsEnumerable Tests

        #region HasRows Tests

        [Fact]
        public void DataReaderHasRowsTest_Null()
        {
            IDataReader reader = null;
            Assert.Throws<NullReferenceException>(() => DataReaderExtensions.HasRows(reader));
        }

        [Fact]
        public void DataReaderHasRowsTest_DataTable_NoData()
        {
            DataTable dt = this.CreateAndLoadDataTable("Temp2", 0);
            DataTableReader reader = dt.CreateDataReader();

            bool result = reader.HasRows();
            dt.DisposeSafe();

            Assert.False(result, "Data Reader contains Rows.");
        }

        [Fact]
        public void DataReaderHasRowsTest_DataTable_WithData()
        {
            DataTable dt = this.CreateAndLoadDataTable("Temp3", 10);
            DataTableReader reader = dt.CreateDataReader();

            bool result = reader.HasRows();
            dt.DisposeSafe();

            Assert.True(result, "Data Reader does not contain any Rows.");
        }

        #endregion HasRows Tests

        #region Select Tests

        [Fact]
        public void DataReaderSelectTest_Null()
        {
            IDataReader reader = null;
            IEnumerable<string> records = reader.Select(dataReader => dataReader[0].ToString());
            Assert.Throws<NullReferenceException>(() => records.ForEach(s => { }));
        }

        [Fact]
        public void DataReaderSelectTest_DataTable_NoData()
        {
            DataTable dt = this.CreateAndLoadDataTable("Temp5", 0);
            DataTableReader reader = dt.CreateDataReader();

            IEnumerable<string> records = reader.Select(dataReader => dataReader[0].ToString());
            dt.DisposeSafe();

            Assert.False(records.Any(), "Data Reader contains data.");
        }

        [Fact]
        public void DataReaderSelectTest_DataTable_WithData()
        {
            DataTable dt = this.CreateAndLoadDataTable("Temp6", 10);
            DataTableReader reader = dt.CreateDataReader();

            IEnumerable<string> records = reader.Select(dataReader => dataReader[0].ToString());
            dt.DisposeSafe();

            Assert.Equal(10, records.Count());
        }

        #endregion Select Tests

        #region Helpers

        private DataTable CreateAndLoadDataTable(string tableName, int numberOfRecords = 3)
        {
            var table = new DataTable(tableName);

            // Add Columns.
            table.Columns.Add("ID", typeof(int));
            table.Columns.Add("Name", typeof(string));

            // Add Rows.
            for (int i = 1; i <= numberOfRecords; i += 1)
            {
                table.Rows.Add(i, Guid.NewGuid().ToString());
            }

            return table;
        }

        #endregion Helpers
    }
}