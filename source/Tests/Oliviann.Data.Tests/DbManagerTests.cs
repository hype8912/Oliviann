namespace Oliviann.Data.Tests
{
    #region Usings

    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
#if NETFRAMEWORK
    using System.Data.SqlServerCe;
#endif
    using Oliviann.Common.TestObjects.Xml;
    using Oliviann.Security;
    using FastMember;
    using Moq;
    using Xunit;

    #endregion Usings

    [Trait("Category", "CI")]
    public class DbManagerTests
    {
        #region Constructor Tests

        /// <summary>
        /// Verifies the default constructor sets the correct properties.
        /// </summary>
        [Fact]
        public void DbManagerTest_DefaultConstructor()
        {
            var manager = new DbManager();

            Assert.Equal(DatabaseProvider.MicrosoftSqlServer, manager.Provider);
            Assert.Empty(manager.ConnectionString.ToUnsecureString());
            manager.Dispose();
        }

        /// <summary>
        /// Verifies the default constructor sets the correct properties.
        /// </summary>
        [Fact]
        public void DbManagerTest_ProviderConstructor()
        {
            var manager = new DbManager(DatabaseProvider.GenericOleDb);

            Assert.Equal(DatabaseProvider.GenericOleDb, manager.Provider);
            Assert.Empty(manager.ConnectionString.ToUnsecureString());
        }

        /// <summary>
        /// Verifies the default constructor sets the correct properties.
        /// </summary>
        [Fact]
        public void DbManagerTest_ProviderAndConnectionStringConstructor()
        {
            string connString = "Data Source=Server;Initial Catalog=Database;User ID=UserName;Password=P@$$w0rd";
            var manager = new DbManager(DatabaseProvider.GenericOleDb, connString);

            Assert.Equal(DatabaseProvider.GenericOleDb, manager.Provider);
            Assert.Equal(connString, manager.ConnectionString.ToUnsecureString());
        }

#if NETFRAMEWORK

        /// <summary>
        /// Verifies the default constructor sets the correct properties.
        /// </summary>
        [Fact]
        public void DbManagerTest_FactoryConstructor()
        {
            var manager = new DbManager(new SqlCeProviderFactory());

            Assert.Equal(DatabaseProvider.None, manager.Provider);
            Assert.Null(manager.ConnectionString.ToUnsecureString());
        }

#endif

        #endregion

        #region Open Tests

        [Fact]
        public void DbManagerOpenTest_NullConnection()
        {
            var factory = new Mock<DbProviderFactory>();
            factory.Setup(f => f.CreateConnection()).Returns(() => null);

            var manager = new DbManager(factory.Object);
            Assert.Throws<DbDataException>(() => manager.Open());
        }

        [Fact]
        public void DbManagerOpenTest_OpenNewConnection()
        {
            ConnectionState state = ConnectionState.Closed;
            var conn = new Mock<DbConnection> { CallBase = true };
            conn.Setup(c => c.Open()).Callback(() => state = ConnectionState.Open);
            conn.Setup(c => c.State).Returns(() => state);
            conn.Setup(c => c.Close()).Callback(() => state = ConnectionState.Closed);

            var factory = new Mock<DbProviderFactory>();
            factory.Setup(f => f.CreateConnection()).Returns(() => conn.Object);

            var manager = new DbManager(factory.Object);
            manager.Open();
            Assert.NotNull(manager.Connection);
            Assert.Equal(ConnectionState.Open, state);
        }

        [Fact]
        public void DbManagerOpenTest_OpenAnOpenConnection()
        {
            ConnectionState state = ConnectionState.Closed;
            var conn = new Mock<DbConnection> { CallBase = true };
            conn.Setup(c => c.Open()).Callback(() => state = ConnectionState.Open);
            conn.Setup(c => c.State).Returns(() => state);
            conn.Setup(c => c.Close()).Callback(() => state = ConnectionState.Closed);

            var factory = new Mock<DbProviderFactory>();
            factory.Setup(f => f.CreateConnection()).Returns(() => conn.Object);

            var manager = new DbManager(factory.Object);
            manager.Open();
            Assert.NotNull(manager.Connection);
            Assert.Equal(ConnectionState.Open, state);

            manager.Open();
            Assert.NotNull(manager.Connection);
            Assert.Equal(ConnectionState.Open, state);
        }

        #endregion

        #region Close Tests

        [Fact]
        public void DbManagerCloseTest_NullConnection()
        {
            var manager = new DbManager();
            manager.Close();

            Assert.Null(manager.Connection);
        }

        [Fact]
        public void DbManagerCloseTest_CloseOpenConnection()
        {
            ConnectionState state = ConnectionState.Closed;
            var conn = new Mock<DbConnection> { CallBase = true };
            conn.Setup(c => c.Open()).Callback(() => state = ConnectionState.Open);
            conn.Setup(c => c.State).Returns(() => state);
            conn.Setup(c => c.Close()).Callback(() => state = ConnectionState.Closed);

            var factory = new Mock<DbProviderFactory>();
            factory.Setup(f => f.CreateConnection()).Returns(() => conn.Object);

            var manager = new DbManager(factory.Object);
            manager.Open();
            Assert.NotNull(manager.Connection);
            Assert.Equal(ConnectionState.Open, state);

            manager.Close();
            Assert.NotNull(manager.Connection);
            Assert.Equal(ConnectionState.Closed, state);
        }

        #endregion

        #region Parameters Tests

        [Fact]
        public void ClearParametersTest_NullCommand()
        {
            var manager = new DbManager();
            manager.ClearParameters();
        }

        [Fact]
        public void ClearParametersTest_ValidParameters()
        {
            var parm1 = new Mock<IDataParameter>();
            var parm2 = new Mock<IDataParameter>();

            var manager = new DbManager();
            manager.AddParameter(parm1.Object);
            manager.AddParameter(parm2.Object);
            Assert.Equal(2, manager.Parameters.Count);

            manager.ClearParameters();
            Assert.Empty(manager.Parameters);
        }

        [Fact]
        public void AddParameterTest_NullParameter()
        {
            var manager = new DbManager();
            manager.AddParameter(null);

            Assert.Empty(manager.Parameters);
        }

        [Fact]
        public void AddParameterTest_ValidParameter()
        {
            var parm = new Mock<IDataParameter>();

            var manager = new DbManager();
            manager.AddParameter(parm.Object);

            Assert.Equal(1, manager.Parameters.Count);
        }

        #endregion

        #region Transaction Tests

        [Fact]
        public void BeginTransactionTest_ValidTransaction()
        {
            var transaction = new Mock<DbTransaction>();

            ConnectionState state = ConnectionState.Closed;
            var conn = new Mock<DbConnection> { CallBase = true };
            conn.Setup(c => c.Open()).Callback(() => state = ConnectionState.Open);
            conn.Setup(c => c.State).Returns(() => state);
            conn.Setup(c => c.Close()).Callback(() => state = ConnectionState.Closed);
            conn.As<IDbConnection>().Setup(c => c.BeginTransaction(It.IsAny<IsolationLevel>())).Returns(() => transaction.Object);

            var factory = new Mock<DbProviderFactory>();
            factory.Setup(f => f.CreateConnection()).Returns(() => conn.Object);

            var manager = new DbManager(factory.Object);
            manager.BeginTransaction();
            Assert.NotNull(manager.Transaction);
        }

        [Fact]
        public void CommitTransactionTest_NullTransaction()
        {
            ConnectionState state = ConnectionState.Closed;
            var conn = new Mock<DbConnection> { CallBase = true };
            conn.Setup(c => c.Open()).Callback(() => state = ConnectionState.Open);
            conn.Setup(c => c.State).Returns(() => state);
            conn.Setup(c => c.Close()).Callback(() => state = ConnectionState.Closed);

            var factory = new Mock<DbProviderFactory>();
            factory.Setup(f => f.CreateConnection()).Returns(() => conn.Object);

            var manager = new DbManager(factory.Object);
            manager.CommitTransaction();
        }

        [Fact]
        public void CommitTransactionTest_ValidTransaction()
        {
            bool commitCalled = false;
            var transaction = new Mock<DbTransaction>();
            transaction.Setup(t => t.Commit()).Callback(() => commitCalled = true);

            ConnectionState state = ConnectionState.Closed;
            var conn = new Mock<DbConnection> { CallBase = true };
            conn.Setup(c => c.Open()).Callback(() => state = ConnectionState.Open);
            conn.Setup(c => c.State).Returns(() => state);
            conn.Setup(c => c.Close()).Callback(() => state = ConnectionState.Closed);
            conn.As<IDbConnection>().Setup(c => c.BeginTransaction(It.IsAny<IsolationLevel>())).Returns(() => transaction.Object);

            var factory = new Mock<DbProviderFactory>();
            factory.Setup(f => f.CreateConnection()).Returns(() => conn.Object);

            var manager = new DbManager(factory.Object);
            manager.BeginTransaction();
            Assert.NotNull(manager.Transaction);

            manager.CommitTransaction();
            Assert.True(commitCalled);
            Assert.Null(manager.Transaction);
        }

        [Fact]
        public void RollbackTransactionTest_NullTransaction()
        {
            ConnectionState state = ConnectionState.Closed;
            var conn = new Mock<DbConnection> { CallBase = true };
            conn.Setup(c => c.Open()).Callback(() => state = ConnectionState.Open);
            conn.Setup(c => c.State).Returns(() => state);
            conn.Setup(c => c.Close()).Callback(() => state = ConnectionState.Closed);

            var factory = new Mock<DbProviderFactory>();
            factory.Setup(f => f.CreateConnection()).Returns(() => conn.Object);

            var manager = new DbManager(factory.Object);
            manager.RollbackTransaction();
        }

        [Fact]
        public void RollbackTransactionTest_ValidTransaction()
        {
            bool rollbackCalled = false;
            var transaction = new Mock<DbTransaction>();
            transaction.Setup(t => t.Rollback()).Callback(() => rollbackCalled = true);

            ConnectionState state = ConnectionState.Closed;
            var conn = new Mock<DbConnection> { CallBase = true };
            conn.Setup(c => c.Open()).Callback(() => state = ConnectionState.Open);
            conn.Setup(c => c.State).Returns(() => state);
            conn.Setup(c => c.Close()).Callback(() => state = ConnectionState.Closed);
            conn.As<IDbConnection>().Setup(c => c.BeginTransaction(It.IsAny<IsolationLevel>())).Returns(() => transaction.Object);

            var factory = new Mock<DbProviderFactory>();
            factory.Setup(f => f.CreateConnection()).Returns(() => conn.Object);

            var manager = new DbManager(factory.Object);
            manager.BeginTransaction();
            Assert.NotNull(manager.Transaction);

            manager.RollbackTransaction();
            Assert.True(rollbackCalled);
            Assert.Null(manager.Transaction);
        }

        #endregion

        #region ExecuteNonQuery Tests

        [Fact]
        public void ExecuteNonQueryTest_NullCommand()
        {
            ConnectionState state = ConnectionState.Closed;
            var conn = new Mock<DbConnection> { CallBase = true };
            conn.Setup(c => c.Open()).Callback(() => state = ConnectionState.Open);
            conn.Setup(c => c.State).Returns(() => state);
            conn.Setup(c => c.Close()).Callback(() => state = ConnectionState.Closed);

            var factory = new Mock<DbProviderFactory>();
            factory.Setup(f => f.CreateConnection()).Returns(() => conn.Object);
            factory.Setup(f => f.CreateCommand()).Returns(() => null);

            var manager = new DbManager(factory.Object);
            Assert.Throws<DbDataException>(() => manager.ExecuteNonQuery(CommandType.Text, string.Empty));
        }

        [Fact]
        public void ExecuteNonQueryTest_CommandDelegateCalled()
        {
            ConnectionState state = ConnectionState.Closed;
            var conn = new Mock<DbConnection> { CallBase = true };
            conn.Setup(c => c.Open()).Callback(() => state = ConnectionState.Open);
            conn.Setup(c => c.State).Returns(() => state);
            conn.Setup(c => c.Close()).Callback(() => state = ConnectionState.Closed);

            var command = new Mock<DbCommand> { CallBase = true };
            command.Setup(c => c.ExecuteNonQuery()).Returns(() => 0);

            var factory = new Mock<DbProviderFactory>();
            factory.Setup(f => f.CreateConnection()).Returns(() => conn.Object);
            factory.Setup(f => f.CreateCommand()).Returns(() => command.Object);

            bool actionCalled = false;
            var manager = new DbManager(factory.Object);
            manager.ExecuteNonQuery(CommandType.Text, string.Empty, cmd => actionCalled = true);
            Assert.True(actionCalled);
        }

        [Fact]
        public void ExecuteNonQueryTest_ReturnedResult()
        {
            ConnectionState state = ConnectionState.Closed;
            var conn = new Mock<DbConnection> { CallBase = true };
            conn.Setup(c => c.Open()).Callback(() => state = ConnectionState.Open);
            conn.Setup(c => c.State).Returns(() => state);
            conn.Setup(c => c.Close()).Callback(() => state = ConnectionState.Closed);

            var command = new Mock<DbCommand> { CallBase = true };
            command.Setup(c => c.ExecuteNonQuery()).Returns(() => 1);

            var factory = new Mock<DbProviderFactory>();
            factory.Setup(f => f.CreateConnection()).Returns(() => conn.Object);
            factory.Setup(f => f.CreateCommand()).Returns(() => command.Object);

            var manager = new DbManager(factory.Object);
            int result = manager.ExecuteNonQuery(CommandType.Text, string.Empty);
            Assert.Equal(1, result);
        }

        #endregion

        #region ExecuteReader Tests

        [Fact]
        public void ExecuteReaderTest_NullCommand()
        {
            ConnectionState state = ConnectionState.Closed;
            var conn = new Mock<DbConnection> { CallBase = true };
            conn.Setup(c => c.Open()).Callback(() => state = ConnectionState.Open);
            conn.Setup(c => c.State).Returns(() => state);
            conn.Setup(c => c.Close()).Callback(() => state = ConnectionState.Closed);

            var factory = new Mock<DbProviderFactory>();
            factory.Setup(f => f.CreateConnection()).Returns(() => conn.Object);
            factory.Setup(f => f.CreateCommand()).Returns(() => null);

            var manager = new DbManager(factory.Object);
            Assert.Throws<DbDataException>(() => manager.ExecuteReader(CommandType.Text, string.Empty));
        }

        [Fact]
        public void ExecuteReaderTest_CommandDelegateCalled()
        {
            ConnectionState state = ConnectionState.Closed;
            var conn = new Mock<DbConnection> { CallBase = true };
            conn.Setup(c => c.Open()).Callback(() => state = ConnectionState.Open);
            conn.Setup(c => c.State).Returns(() => state);
            conn.Setup(c => c.Close()).Callback(() => state = ConnectionState.Closed);

            var reader = new Mock<IDataReader>();
            var command = new Mock<DbCommand> { CallBase = true };
            command.As<IDbCommand>().Setup(c => c.ExecuteReader()).Returns(() => reader.Object);

            var factory = new Mock<DbProviderFactory>();
            factory.Setup(f => f.CreateConnection()).Returns(() => conn.Object);
            factory.Setup(f => f.CreateCommand()).Returns(() => command.Object);

            bool actionCalled = false;
            var manager = new DbManager(factory.Object);
            manager.ExecuteReader(CommandType.Text, string.Empty, cmd => actionCalled = true);
            Assert.True(actionCalled);
        }

        [Fact]
        public void ExecuteReaderTest_ReturnBasicReader()
        {
            ConnectionState state = ConnectionState.Closed;
            var conn = new Mock<DbConnection> { CallBase = true };
            conn.Setup(c => c.Open()).Callback(() => state = ConnectionState.Open);
            conn.Setup(c => c.State).Returns(() => state);
            conn.Setup(c => c.Close()).Callback(() => state = ConnectionState.Closed);

            var reader = new Mock<IDataReader>();
            reader.SetupGet(r => r.IsClosed).Returns(() => false);

            var command = new Mock<DbCommand> { CallBase = true };
            command.As<IDbCommand>().Setup(c => c.ExecuteReader()).Returns(() => reader.Object);

            var factory = new Mock<DbProviderFactory>();
            factory.Setup(f => f.CreateConnection()).Returns(() => conn.Object);
            factory.Setup(f => f.CreateCommand()).Returns(() => command.Object);

            var manager = new DbManager(factory.Object);
            IDataReader result = manager.ExecuteReader(CommandType.Text, string.Empty);
            Assert.NotNull(result);
            Assert.NotNull(manager.DataReader);
        }

        #endregion

        #region CloseReader Tests

        [Fact]
        public void CloseReaderTest_NullReader()
        {
            var manager = new DbManager();
            Assert.Null(manager.DataReader);
            manager.CloseReader();
        }

        [Fact]
        public void CloseReaderTest_CloseClosedReader()
        {
            ConnectionState state = ConnectionState.Closed;
            var conn = new Mock<DbConnection> { CallBase = true };
            conn.Setup(c => c.Open()).Callback(() => state = ConnectionState.Open);
            conn.Setup(c => c.State).Returns(() => state);
            conn.Setup(c => c.Close()).Callback(() => state = ConnectionState.Closed);

            var reader = new Mock<IDataReader>();
            reader.SetupGet(r => r.IsClosed).Returns(() => true);

            var command = new Mock<DbCommand> { CallBase = true };
            command.As<IDbCommand>().Setup(c => c.ExecuteReader()).Returns(() => reader.Object);

            var factory = new Mock<DbProviderFactory>();
            factory.Setup(f => f.CreateConnection()).Returns(() => conn.Object);
            factory.Setup(f => f.CreateCommand()).Returns(() => command.Object);

            var manager = new DbManager(factory.Object);
            IDataReader result = manager.ExecuteReader(CommandType.Text, string.Empty);
            Assert.NotNull(result);

            manager.CloseReader();
        }

        [Fact]
        public void CloseReaderTest_CloseOpenReader()
        {
            ConnectionState state = ConnectionState.Closed;
            var conn = new Mock<DbConnection> { CallBase = true };
            conn.Setup(c => c.Open()).Callback(() => state = ConnectionState.Open);
            conn.Setup(c => c.State).Returns(() => state);
            conn.Setup(c => c.Close()).Callback(() => state = ConnectionState.Closed);

            bool readerCloseCalled = false;
            bool readerClosed = true;
            var reader = new Mock<IDataReader>();
            reader.SetupGet(r => r.IsClosed).Returns(() => readerClosed);
            reader.Setup(r => r.Close()).Callback(() =>
                {
                    readerCloseCalled = true;
                    readerClosed = true;
                });

            var command = new Mock<DbCommand> { CallBase = true };
            command.As<IDbCommand>().Setup(c => c.ExecuteReader()).Returns(() => reader.Object).Callback(() => readerClosed = false);

            var factory = new Mock<DbProviderFactory>();
            factory.Setup(f => f.CreateConnection()).Returns(() => conn.Object);
            factory.Setup(f => f.CreateCommand()).Returns(() => command.Object);

            var manager = new DbManager(factory.Object);
            IDataReader result = manager.ExecuteReader(CommandType.Text, string.Empty);
            Assert.NotNull(result);

            manager.CloseReader();
            Assert.True(readerCloseCalled);
            Assert.True(readerClosed);
        }

        #endregion

        #region ExecuteDataTable Tests

        [Fact]
        public void ExecuteDataTableTest_ReturnEmptyTable()
        {
            ConnectionState state = ConnectionState.Closed;
            var conn = new Mock<DbConnection> { CallBase = true };
            conn.Setup(c => c.Open()).Callback(() => state = ConnectionState.Open);
            conn.Setup(c => c.State).Returns(() => state);
            conn.Setup(c => c.Close()).Callback(() => state = ConnectionState.Closed);

            var items = new List<Book>();
            var reader = ObjectReader.Create(items, "Id", "Author", "Title");

            var command = new Mock<DbCommand> { CallBase = true };
            command.As<IDbCommand>().Setup(c => c.ExecuteReader()).Returns(() => reader);

            var factory = new Mock<DbProviderFactory>();
            factory.Setup(f => f.CreateConnection()).Returns(() => conn.Object);
            factory.Setup(f => f.CreateCommand()).Returns(() => command.Object);

            var manager = new DbManager(factory.Object);
            DataTable result = manager.ExecuteDataTable(CommandType.Text, string.Empty);
            Assert.NotNull(result);
            Assert.Equal(0, result.Rows.Count);
        }

        [Fact]
        public void ExecuteDataTableTest_ReturnRecords()
        {
            ConnectionState state = ConnectionState.Closed;
            var conn = new Mock<DbConnection> { CallBase = true };
            conn.Setup(c => c.Open()).Callback(() => state = ConnectionState.Open);
            conn.Setup(c => c.State).Returns(() => state);
            conn.Setup(c => c.Close()).Callback(() => state = ConnectionState.Closed);

            var items = new List<Book>
                            {
                                new Book { Id = 2, Author = "Valdez, Juan", Title = "How to make great Coffee" },
                                new Book { Id = 6, Author = "NavPress", Title = "How to find the one" }
                            };
            var reader = ObjectReader.Create(items, "Id", "Author", "Title");

            var command = new Mock<DbCommand> { CallBase = true };
            command.As<IDbCommand>().Setup(c => c.ExecuteReader()).Returns(() => reader);

            var factory = new Mock<DbProviderFactory>();
            factory.Setup(f => f.CreateConnection()).Returns(() => conn.Object);
            factory.Setup(f => f.CreateCommand()).Returns(() => command.Object);

            var manager = new DbManager(factory.Object);
            DataTable result = manager.ExecuteDataTable(CommandType.Text, string.Empty);
            Assert.NotNull(result);
            Assert.Equal(2, result.Rows.Count);
        }

        [Fact]
        public void ExecuteDataTableTest_NullReader()
        {
            ConnectionState state = ConnectionState.Closed;
            var conn = new Mock<DbConnection> { CallBase = true };
            conn.Setup(c => c.Open()).Callback(() => state = ConnectionState.Open);
            conn.Setup(c => c.State).Returns(() => state);
            conn.Setup(c => c.Close()).Callback(() => state = ConnectionState.Closed);

            var command = new Mock<DbCommand> { CallBase = true };
            command.As<IDbCommand>().Setup(c => c.ExecuteReader()).Returns(() => null);

            var factory = new Mock<DbProviderFactory>();
            factory.Setup(f => f.CreateConnection()).Returns(() => conn.Object);
            factory.Setup(f => f.CreateCommand()).Returns(() => command.Object);

            var manager = new DbManager(factory.Object);
            Assert.Throws<DbDataException>(() => manager.ExecuteDataTable(CommandType.Text, string.Empty));
        }

        #endregion

        #region ExecuteDataSet Tests

        [Fact]
        public void ExecuteDataSetTest_NullAdapter()
        {
            ConnectionState state = ConnectionState.Closed;
            var conn = new Mock<DbConnection> { CallBase = true };
            conn.Setup(c => c.Open()).Callback(() => state = ConnectionState.Open);
            conn.Setup(c => c.State).Returns(() => state);
            conn.Setup(c => c.Close()).Callback(() => state = ConnectionState.Closed);

            var command = new Mock<DbCommand> { CallBase = true };
            command.As<IDbCommand>().Setup(c => c.ExecuteReader()).Returns(() => null);

            var factory = new Mock<DbProviderFactory>();
            factory.Setup(f => f.CreateConnection()).Returns(() => conn.Object);
            factory.Setup(f => f.CreateCommand()).Returns(() => command.Object);
            factory.Setup(f => f.CreateDataAdapter()).Returns(() => null);

            var manager = new DbManager(factory.Object);
            Assert.Throws<DbDataException>(() => manager.ExecuteDataSet(CommandType.Text, string.Empty));
        }

        [Fact]
        public void ExecuteDataSetTest_ValidDataSet()
        {
            ConnectionState state = ConnectionState.Closed;
            var conn = new Mock<DbConnection> { CallBase = true };
            conn.Setup(c => c.Open()).Callback(() => state = ConnectionState.Open);
            conn.Setup(c => c.State).Returns(() => state);
            conn.Setup(c => c.Close()).Callback(() => state = ConnectionState.Closed);

            var command = new Mock<DbCommand> { CallBase = true };
            command.As<IDbCommand>().Setup(c => c.ExecuteReader()).Returns(() => null);

            var adapter = new Mock<DbDataAdapter> { CallBase = true };
            adapter.Setup(a => a.Fill(It.IsAny<DataSet>())).Returns(() => 1);

            var factory = new Mock<DbProviderFactory>();
            factory.Setup(f => f.CreateConnection()).Returns(() => conn.Object);
            factory.Setup(f => f.CreateCommand()).Returns(() => command.Object);
            factory.Setup(f => f.CreateDataAdapter()).Returns(() => adapter.Object);

            var manager = new DbManager(factory.Object);
            DataSet result = manager.ExecuteDataSet(CommandType.Text, string.Empty);

            Assert.NotNull(result);
        }

        #endregion
    }
}