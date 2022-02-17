namespace Oliviann.Tests.Data
{
    #region Usings

    using System.Data;
    using Oliviann.Data;
    using Moq;
    using Xunit;

    #endregion Usings

    [Trait("Category", "CI")]
    public class ConnectionExtensionsTests
    {
        [Fact]
        public void ConnectionCloseSafeTest_Null()
        {
            IDbConnection conn = null;
            ConnectionExtensions.CloseSafe(conn);
        }

        [Fact]
        public void ConnectionCloseSafeTest_ClosedConnection()
        {
            bool closedWasCalled = false;
            var mocConn = new Mock<IDbConnection>();
            mocConn.Setup(c => c.State).Returns(ConnectionState.Closed);
            mocConn.Setup(c => c.Close()).Callback(() => closedWasCalled = true);

            mocConn.Object.CloseSafe();
            Assert.False(closedWasCalled, "Closed was called.");
        }

        [Fact]
        public void ConnectionCloseSafeTest_OpenConnection()
        {
            bool closedWasCalled = false;
            var mocConn = new Mock<IDbConnection>();
            mocConn.Setup(c => c.State).Returns(ConnectionState.Open);
            mocConn.Setup(c => c.Close()).Callback(() => closedWasCalled = true);

            mocConn.Object.CloseSafe();
            Assert.True(closedWasCalled, "Closed was not called.");
        }
    }
}