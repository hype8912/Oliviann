#if !NETSTANDARD1_3

namespace Oliviann.Tests.ServiceProcess
{
    #region Usings

    using System;
    using System.ServiceProcess;
    using Oliviann.ServiceProcess;
    using Xunit;

    #endregion Usings

    [Trait("Category", "CI")]
    public class ServiceControllerProxyTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void ServiceControllerProxyTest_InvalidServiceNames(string serviceName)
        {
            Assert.Throws<ArgumentNullException>(() => new ServiceControllerProxy(serviceName));
        }

        [Fact]
        public void ServiceControllerProxyTest_ValidServiceNames()
        {
            var proxy = new ServiceControllerProxy("BITS");
            proxy.Dispose();
        }

        [Fact]
        public void ServiceControllerProxyTest_NullService()
        {
            ServiceController controller = null;
            Assert.Throws<ArgumentNullException>(() => new ServiceControllerProxy(controller));
        }

        [Fact]
        public void ServiceControllerProxyTest_ValidService()
        {
            var controller = new ServiceController("BITS");
            var proxy = new ServiceControllerProxy(controller);
            proxy.Dispose();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void AsServiceTest_InvalidServiceNames(string serviceName)
        {
            bool result = ServiceControllerProxy.AsService(serviceName, controller => true);
            Assert.False(result);
        }

        [Fact]
        public void AsServiceTest_NullFunction()
        {
            bool result = ServiceControllerProxy.AsService("BITS", null);
            Assert.False(result);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void AsServiceTest_FunctionResult(bool expectedResult)
        {
            bool result = ServiceControllerProxy.AsService("BITS", c => expectedResult);
            Assert.Equal(expectedResult, result);
        }
    }
}

#endif