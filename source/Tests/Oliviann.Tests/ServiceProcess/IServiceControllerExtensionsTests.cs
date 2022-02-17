#if !NETSTANDARD1_3

namespace Oliviann.Tests.ServiceProcess
{
    #region Usings

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.ServiceProcess;
    using System.Text;

    using Oliviann.ServiceProcess;

    using Moq;

    using Xunit;

    #endregion Usings

    [Trait("Category", "CI")]
    public class IServiceControllerExtensionsTests
    {
        #region IsRunning And IsValid Tests

        [Fact]
        public void IsRunningTest_NullController()
        {
            IServiceController controller = null;
            bool result = controller.IsRunning();

            Assert.False(result);
        }

        [Fact]
        public void IsRunningTest_InvalidController()
        {
            var mocController = new Mock<IServiceController>();
            mocController.SetupGet(c => c.Status).Throws(new Exception());

            bool result = mocController.Object.IsRunning();
            Assert.False(result);
        }

        [Theory]
        [InlineData(ServiceControllerStatus.ContinuePending, false)]
        [InlineData(ServiceControllerStatus.PausePending, false)]
        [InlineData(ServiceControllerStatus.Paused, false)]
        [InlineData(ServiceControllerStatus.Running, true)]
        [InlineData(ServiceControllerStatus.StartPending, false)]
        [InlineData(ServiceControllerStatus.StopPending, false)]
        [InlineData(ServiceControllerStatus.Stopped, false)]
        public void IsRunningTest_Status(ServiceControllerStatus status, bool expectedResult)
        {
            var mocController = new Mock<IServiceController>();
            mocController.SetupGet(c => c.Status).Returns(status);

            bool result = mocController.Object.IsRunning();
            Assert.Equal(expectedResult, result);
        }

        #endregion

        #region Restart Tests

        [Fact]
        public void RestartTest_NullController()
        {
            IServiceController controller = null;
            bool result = controller.Restart();

            Assert.False(result);
        }

        [Fact]
        public void RestartTest_InvalidController()
        {
            var mocController = new Mock<IServiceController>();
            mocController.SetupGet(c => c.Status).Throws(new Exception());

            bool result = mocController.Object.Restart();
            Assert.False(result);
        }

        [Theory]
        [InlineData(ServiceControllerStatus.Stopped)]
        [InlineData(ServiceControllerStatus.StopPending)]
        public void RestartTest_NoArguments(ServiceControllerStatus status)
        {
            var mocController = new Mock<IServiceController>();
            mocController.SetupGet(c => c.Status).Returns(status);
            mocController.Setup(c => c.Start(It.IsAny<string[]>()));
            mocController.Setup(c => c.WaitForStatus(It.Is<ServiceControllerStatus>(s => s == ServiceControllerStatus.Running), It.IsAny<TimeSpan>()));
            mocController.Setup(c => c.Refresh());

            bool result = mocController.Object.Restart();
            Assert.True(result);
        }

        [Theory]
        [InlineData(ServiceControllerStatus.Stopped)]
        [InlineData(ServiceControllerStatus.StopPending)]
        public void RestartTest_WithTimeout(ServiceControllerStatus status)
        {
            var rand = new Random().Next(1000, 30000);
            var span = TimeSpan.FromMilliseconds(rand);
            var mocController = new Mock<IServiceController>();
            mocController.SetupGet(c => c.Status).Returns(status);
            mocController.Setup(c => c.Start(It.IsAny<string[]>()));
            mocController.Setup(c => c.WaitForStatus(It.Is<ServiceControllerStatus>(s => s == ServiceControllerStatus.Running), It.Is<TimeSpan>(t => t == span)));
            mocController.Setup(c => c.Refresh());

            bool result = mocController.Object.Restart(span);
            Assert.True(result);
        }

        #endregion

        #region Start Tests

        [Fact]
        public void StartSafeTest_NullController()
        {
            IServiceController controller = null;
            bool result = controller.StartSafe();

            Assert.False(result);
        }

        [Fact]
        public void StartSafeTest_InvalidController()
        {
            var mocController = new Mock<IServiceController>();
            mocController.SetupGet(c => c.Status).Throws(new Exception());

            bool result = mocController.Object.StartSafe();
            Assert.False(result);
        }

        [Theory]
        [InlineData(ServiceControllerStatus.Running)]
        [InlineData(ServiceControllerStatus.StartPending)]
        public void StartSafeTest_NoArguments(ServiceControllerStatus status)
        {
            var mocController = new Mock<IServiceController>();
            mocController.SetupGet(c => c.Status).Returns(status);

            bool result = mocController.Object.StartSafe();
            Assert.True(result);
        }

        [Theory]
        [InlineData(ServiceControllerStatus.ContinuePending)]
        [InlineData(ServiceControllerStatus.PausePending)]
        [InlineData(ServiceControllerStatus.Paused)]
        [InlineData(ServiceControllerStatus.StopPending)]
        [InlineData(ServiceControllerStatus.Stopped)]
        public void StartSafeTest_NoArguments2(ServiceControllerStatus status)
        {
            var mocController = new Mock<IServiceController>();
            mocController.SetupGet(c => c.Status).Returns(status);
            mocController.Setup(c => c.Start(It.IsAny<string[]>()));
            mocController.Setup(c => c.WaitForStatus(It.Is<ServiceControllerStatus>(s => s == ServiceControllerStatus.Running), It.IsAny<TimeSpan>()));
            mocController.Setup(c => c.Refresh());

            bool result = mocController.Object.StartSafe();
            Assert.True(result);
        }

        [Theory]
        [InlineData(ServiceControllerStatus.ContinuePending)]
        [InlineData(ServiceControllerStatus.PausePending)]
        [InlineData(ServiceControllerStatus.Paused)]
        [InlineData(ServiceControllerStatus.StopPending)]
        [InlineData(ServiceControllerStatus.Stopped)]
        public void StartSafeTest_Failed(ServiceControllerStatus status)
        {
            var mocController = new Mock<IServiceController>();
            mocController.SetupGet(c => c.Status).Returns(status);
            mocController.Setup(c => c.Start(It.IsAny<string[]>())).Throws(new InvalidOperationException());
            mocController.Setup(c => c.WaitForStatus(It.Is<ServiceControllerStatus>(s => s == ServiceControllerStatus.Running), It.IsAny<TimeSpan>()));
            mocController.Setup(c => c.Refresh());

            bool result = mocController.Object.StartSafe();
            Assert.False(result);
        }

        #endregion

        #region Stop Tests

        [Fact]
        public void StopSafeTest_NullController()
        {
            IServiceController controller = null;
            bool result = controller.StopSafe();

            Assert.False(result);
        }

        [Fact]
        public void StopSafeTest_InvalidController()
        {
            var mocController = new Mock<IServiceController>();
            mocController.SetupGet(c => c.Status).Throws(new Exception());

            bool result = mocController.Object.StopSafe();
            Assert.False(result);
        }

        [Theory]
        [InlineData(ServiceControllerStatus.Stopped)]
        [InlineData(ServiceControllerStatus.StopPending)]
        public void StopSafeTest_NoArguments(ServiceControllerStatus status)
        {
            var mocController = new Mock<IServiceController>();
            mocController.SetupGet(c => c.Status).Returns(status);

            bool result = mocController.Object.StopSafe();
            Assert.True(result);
        }

        [Theory]
        [InlineData(ServiceControllerStatus.ContinuePending)]
        [InlineData(ServiceControllerStatus.PausePending)]
        [InlineData(ServiceControllerStatus.Paused)]
        [InlineData(ServiceControllerStatus.StopPending)]
        [InlineData(ServiceControllerStatus.Stopped)]
        public void StopSafeTest_NoArguments2(ServiceControllerStatus status)
        {
            var mocController = new Mock<IServiceController>();
            mocController.SetupGet(c => c.Status).Returns(status);
            mocController.Setup(c => c.Start(It.IsAny<string[]>()));
            mocController.Setup(c => c.WaitForStatus(It.Is<ServiceControllerStatus>(s => s == ServiceControllerStatus.Running), It.IsAny<TimeSpan>()));
            mocController.Setup(c => c.Refresh());

            bool result = mocController.Object.StopSafe();
            Assert.True(result);
        }

        [Theory]
        [InlineData(ServiceControllerStatus.ContinuePending)]
        [InlineData(ServiceControllerStatus.PausePending)]
        [InlineData(ServiceControllerStatus.Paused)]
        [InlineData(ServiceControllerStatus.Running)]
        [InlineData(ServiceControllerStatus.StartPending)]
        public void StopSafeTest_Failed(ServiceControllerStatus status)
        {
            var mocController = new Mock<IServiceController>();
            mocController.SetupGet(c => c.Status).Returns(status);
            mocController.Setup(c => c.Stop()).Throws(new InvalidOperationException());
            mocController.Setup(c => c.WaitForStatus(It.Is<ServiceControllerStatus>(s => s == ServiceControllerStatus.Running), It.IsAny<TimeSpan>()));
            mocController.Setup(c => c.Refresh());

            bool result = mocController.Object.StopSafe();
            Assert.False(result);
        }

        #endregion

        #region Wait Tests

        [Fact]
        public void WaitForStatusOptTest_NullController()
        {
            IServiceController controller = null;
            controller.WaitForStatusOpt(ServiceControllerStatus.ContinuePending);
        }

        [Theory]
        [InlineData(ServiceControllerStatus.ContinuePending)]
        [InlineData(ServiceControllerStatus.PausePending)]
        [InlineData(ServiceControllerStatus.Paused)]
        [InlineData(ServiceControllerStatus.Running)]
        [InlineData(ServiceControllerStatus.StartPending)]
        [InlineData(ServiceControllerStatus.StopPending)]
        [InlineData(ServiceControllerStatus.Stopped)]
        public void WaitForStatusOptTest_DefaultTimeout(ServiceControllerStatus status)
        {
            var mocController = new Mock<IServiceController>();
            mocController.Setup(
                c => c.WaitForStatus(
                    It.Is<ServiceControllerStatus>(s => s == status), It.Is<TimeSpan>(t => t == TimeSpan.MaxValue)));

            mocController.Object.WaitForStatusOpt(status);
        }

        [Theory]
        [InlineData(ServiceControllerStatus.ContinuePending)]
        [InlineData(ServiceControllerStatus.PausePending)]
        [InlineData(ServiceControllerStatus.Paused)]
        [InlineData(ServiceControllerStatus.Running)]
        [InlineData(ServiceControllerStatus.StartPending)]
        [InlineData(ServiceControllerStatus.StopPending)]
        [InlineData(ServiceControllerStatus.Stopped)]
        public void WaitForStatusOptTest_SetTimeout(ServiceControllerStatus status)
        {
            var rand = new Random().Next(1000, 30000);
            var span = TimeSpan.FromMilliseconds(rand);
            var mocController = new Mock<IServiceController>();
            mocController.Setup(
                c => c.WaitForStatus(
                    It.Is<ServiceControllerStatus>(s => s == status), It.Is<TimeSpan>(t => t == span)));

            mocController.Object.WaitForStatusOpt(status, span);
        }

        #endregion
    }
}

#endif