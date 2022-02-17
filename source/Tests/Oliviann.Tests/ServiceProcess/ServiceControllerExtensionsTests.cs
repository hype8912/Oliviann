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
    [Trait("Category", "Services")]
    public class ServiceControllerExtensionsTests
    {
        [Fact]
        public void AsIServiceControllerTest_NullController()
        {
            ServiceController controller = null;
            Assert.Throws<ArgumentNullException>(() => controller.AsIServiceController());
        }

        [Fact]
        public void AsIServiceControllerTest_ValidController()
        {
            var controller = new ServiceController("BITS");
            IServiceController proxy = controller.AsIServiceController();

            Assert.NotNull(proxy);
            proxy.Dispose();
        }
    }
}

#endif