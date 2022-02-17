namespace Oliviann.Data.Tests.Base
{
    #region Usings

    using System;
    using System.Data.Odbc;
    using System.Data.OleDb;
#if NETFRAMEWORK
    using System.Data.OracleClient;
#endif
    using System.Data.SqlClient;
    using System.Data.SqlTypes;
    using Oliviann.Data.Base;
    using Xunit;

    #endregion Usings

    [Trait("Category", "CI")]
    public class DbManagerFactoryTests
    {
        #region CreateTypeConnection Tests

        [Theory]
        [InlineData(DatabaseProvider.GenericOdbc, typeof(OdbcConnection))]
        [InlineData(DatabaseProvider.GenericOleDb, typeof(OleDbConnection))]
        [InlineData(DatabaseProvider.MicrosoftAccess, typeof(OleDbConnection))]
        [InlineData(DatabaseProvider.MicrosoftAccess12, typeof(OleDbConnection))]
        [InlineData(DatabaseProvider.MicrosoftAccess14, typeof(OleDbConnection))]
        [InlineData(DatabaseProvider.MicrosoftAccess15, typeof(OleDbConnection))]
        [InlineData(DatabaseProvider.MicrosoftExcel, typeof(OleDbConnection))]
        [InlineData(DatabaseProvider.MicrosoftExcel12, typeof(OleDbConnection))]
        [InlineData(DatabaseProvider.MicrosoftExcel14, typeof(OleDbConnection))]
        [InlineData(DatabaseProvider.MicrosoftExcel15, typeof(OleDbConnection))]
        [InlineData(DatabaseProvider.MicrosoftSqlServer, typeof(SqlConnection))]
#if NETFRAMEWORK
        [InlineData(DatabaseProvider.Oracle, typeof(OracleConnection))]
#endif
        public void CreateTypeConnectionTest_ProviderTypes(DatabaseProvider input, Type expectedResult)
        {
            var result = DbManagerFactory.CreateConnection(input);
            Assert.Equal(expectedResult, result.GetType());
        }

        [Theory]
        [InlineData(DatabaseProvider.SQLite)]
        public void CreateTypeConnectionTest_UnknownProvider(DatabaseProvider input)
        {
            var result = DbManagerFactory.CreateConnection(input);
            Assert.Null(result);
        }

        #endregion CreateTypeConnection Tests

        #region CreateCommand Tests

        [Theory]
        [InlineData(DatabaseProvider.GenericOdbc, typeof(OdbcCommand))]
        [InlineData(DatabaseProvider.GenericOleDb, typeof(OleDbCommand))]
        [InlineData(DatabaseProvider.MicrosoftAccess, typeof(OleDbCommand))]
        [InlineData(DatabaseProvider.MicrosoftAccess12, typeof(OleDbCommand))]
        [InlineData(DatabaseProvider.MicrosoftAccess14, typeof(OleDbCommand))]
        [InlineData(DatabaseProvider.MicrosoftAccess15, typeof(OleDbCommand))]
        [InlineData(DatabaseProvider.MicrosoftExcel, typeof(OleDbCommand))]
        [InlineData(DatabaseProvider.MicrosoftExcel12, typeof(OleDbCommand))]
        [InlineData(DatabaseProvider.MicrosoftExcel14, typeof(OleDbCommand))]
        [InlineData(DatabaseProvider.MicrosoftExcel15, typeof(OleDbCommand))]
        [InlineData(DatabaseProvider.MicrosoftSqlServer, typeof(SqlCommand))]
#if NETFRAMEWORK
        [InlineData(DatabaseProvider.Oracle, typeof(OracleCommand))]
#endif
        public void CreateCommandTest_ProviderTypes(DatabaseProvider input, Type expectedResult)
        {
            var result = DbManagerFactory.CreateCommand(input);

            Assert.NotNull(result);
            Assert.Equal(expectedResult, result.GetType());
        }

        [Theory]
        [InlineData(DatabaseProvider.SQLite)]
        public void CreateCommandTest_UnknownProvider(DatabaseProvider input)
        {
            var result = DbManagerFactory.CreateCommand(input);
            Assert.Null(result);
        }

        #endregion CreateCommand Tests

        #region CreateDataAdapter Tests

        [Theory]
        [InlineData(DatabaseProvider.GenericOdbc, typeof(OdbcDataAdapter))]
        [InlineData(DatabaseProvider.GenericOleDb, typeof(OleDbDataAdapter))]
        [InlineData(DatabaseProvider.MicrosoftAccess, typeof(OleDbDataAdapter))]
        [InlineData(DatabaseProvider.MicrosoftAccess12, typeof(OleDbDataAdapter))]
        [InlineData(DatabaseProvider.MicrosoftAccess14, typeof(OleDbDataAdapter))]
        [InlineData(DatabaseProvider.MicrosoftAccess15, typeof(OleDbDataAdapter))]
        [InlineData(DatabaseProvider.MicrosoftExcel, typeof(OleDbDataAdapter))]
        [InlineData(DatabaseProvider.MicrosoftExcel12, typeof(OleDbDataAdapter))]
        [InlineData(DatabaseProvider.MicrosoftExcel14, typeof(OleDbDataAdapter))]
        [InlineData(DatabaseProvider.MicrosoftExcel15, typeof(OleDbDataAdapter))]
        [InlineData(DatabaseProvider.MicrosoftSqlServer, typeof(SqlDataAdapter))]
#if NETFRAMEWORK
        [InlineData(DatabaseProvider.Oracle, typeof(OracleDataAdapter))]
#endif
        public void CreateDataAdapterTest_ProviderTypes(DatabaseProvider input, Type expectedResult)
        {
            var result = DbManagerFactory.CreateDataAdapter(input);
            Assert.Equal(expectedResult, result.GetType());
        }

        [Theory]
        [InlineData(DatabaseProvider.SQLite)]
        public void CreateDataAdapterTest_UnknownProvider(DatabaseProvider input)
        {
            var result = DbManagerFactory.CreateDataAdapter(input);
            Assert.Null(result);
        }

        #endregion CreateDataAdapter Tests

        #region CreateParameter Tests

        [Theory]
        [InlineData(DatabaseProvider.GenericOdbc, typeof(OdbcParameter))]
        [InlineData(DatabaseProvider.GenericOleDb, typeof(OleDbParameter))]
        [InlineData(DatabaseProvider.MicrosoftAccess, typeof(OleDbParameter))]
        [InlineData(DatabaseProvider.MicrosoftAccess12, typeof(OleDbParameter))]
        [InlineData(DatabaseProvider.MicrosoftAccess14, typeof(OleDbParameter))]
        [InlineData(DatabaseProvider.MicrosoftAccess15, typeof(OleDbParameter))]
        [InlineData(DatabaseProvider.MicrosoftExcel, typeof(OleDbParameter))]
        [InlineData(DatabaseProvider.MicrosoftExcel12, typeof(OleDbParameter))]
        [InlineData(DatabaseProvider.MicrosoftExcel14, typeof(OleDbParameter))]
        [InlineData(DatabaseProvider.MicrosoftExcel15, typeof(OleDbParameter))]
        [InlineData(DatabaseProvider.MicrosoftSqlServer, typeof(SqlParameter))]
#if NETFRAMEWORK
        [InlineData(DatabaseProvider.Oracle, typeof(OracleParameter))]
#endif
        public void CreateParameterTest_ProviderTypes(DatabaseProvider input, Type expectedResult)
        {
            var result = DbManagerFactory.CreateParameter(input);
            Assert.Equal(expectedResult, result.GetType());
        }

        [Theory]
        [InlineData(DatabaseProvider.SQLite)]
        public void CreateParameterTest_UnknownProvider(DatabaseProvider input)
        {
            var result = DbManagerFactory.CreateParameter(input);
            Assert.Null(result);
        }

        #endregion CreateParameter Tests

        #region GetDbDateTime Tests

        [Theory]
        [InlineData(DatabaseProvider.MicrosoftAccess, 0, 0D)]
        [InlineData(DatabaseProvider.MicrosoftAccess, 636087692879600456, 42619.607499537036)]
        [InlineData(DatabaseProvider.MicrosoftAccess, 3155378975999999999L, 2958465.9999999884)]
        [InlineData(DatabaseProvider.MicrosoftAccess12, 636087692879600456, 42619.607499537036)]
        [InlineData(DatabaseProvider.MicrosoftAccess14, 636087692879600456, 42619.607499537036)]
        [InlineData(DatabaseProvider.MicrosoftAccess15, 636087692879600456, 42619.607499537036)]
        public void GetDbDateTimeTest_Access(DatabaseProvider provider, long ticks, double expectedResult)
        {
            var time = new DateTime(ticks);
            var result = (double)DbManagerFactory.GetDbDateTime(provider, time);

            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData(DatabaseProvider.MicrosoftSqlServer, 0, 552877920000000000)]
        [InlineData(DatabaseProvider.MicrosoftSqlServer, 636087692879600456, 636087692879600456)]
        [InlineData(DatabaseProvider.MicrosoftSqlServer, 3155378975999999999, 3155378975999999999)]
        public void GetDbDateTimeTest_SqlServer(DatabaseProvider provider, long ticks, long expectedResultTicks)
        {
            var time = new DateTime(ticks);
            var expectedResult = new DateTime(expectedResultTicks);
            var result = (SqlDateTime)DbManagerFactory.GetDbDateTime(provider, time);

            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData(DatabaseProvider.GenericOdbc, 0)]
        [InlineData(DatabaseProvider.GenericOleDb, 636087692879600456)]
        [InlineData(DatabaseProvider.MicrosoftExcel, 3155378975999999999)]
        [InlineData(DatabaseProvider.MicrosoftExcel12, 0)]
        [InlineData(DatabaseProvider.MicrosoftExcel14, 636087692879600456)]
        [InlineData(DatabaseProvider.MicrosoftExcel15, 3155378975999999999)]
#if NETFRAMEWORK
        [InlineData(DatabaseProvider.Oracle, 0)]
#endif
        [InlineData(DatabaseProvider.SQLite, 636087692879600456)]
        public void GetDbDateTimeTest_OtherProviders(DatabaseProvider provider, long ticks)
        {
            var inputDt = new DateTime(ticks);
            var expectedDt = new DateTime(ticks);
            var result = (DateTime)DbManagerFactory.GetDbDateTime(provider, inputDt);

            Assert.Equal(expectedDt, result);
        }

        #endregion GetDbDateTime Tests
    }
}