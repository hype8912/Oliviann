namespace Oliviann.Tests.ServiceModel
{
    #region Usings

    using System;
    using System.Collections.Generic;
    using System.ServiceModel;
    using Oliviann.ServiceModel;
    using TestObjects;
    using Xunit;

    #endregion Usings

    [Trait("Category", "CI")]
    public class ChannelFactoryExtensionsTests
    {
        [Fact]
        public void ExecuteRequestTTest_NullFactory()
        {
            ChannelFactory<IGenericService> factory = null;
            factory.ExecuteRequest(null);
        }

        [Fact]
        public void ExecuteRequestTTest_NullAction()
        {
            var factory = new ChannelFactory<IGenericService>();
            factory.ExecuteRequest(null);
        }

        [Fact]
        public void ExecuteRequestTInTest_NullFactory()
        {
            ChannelFactory<IGenericService> factory = null;
            IEnumerable<string> items = null;
            factory.ExecuteRequest(null, items);
        }

        [Fact]
        public void ExecuteRequestTInTest_NullFunc()
        {
            var factory = new ChannelFactory<IGenericService>();
            Func<IGenericService, string[], bool> funct = null;
            IEnumerable<string> items = null;
            factory.ExecuteRequest(funct, items);
        }

        [Fact]
        public void ExecuteRequestTInTest_NullEnum()
        {
            var factory = new ChannelFactory<IGenericService>();
            Func<IGenericService, string[], bool> funct = (service, strings) =>
                {
                    string[] s = strings;
                    return false;
                };
            IEnumerable<string> items = null;
            factory.ExecuteRequest(funct, items);
        }

        [Fact]
        public void ExecuteRequestTResTest_NullFactory()
        {
            ChannelFactory<IGenericService> factory = null;
            Func<IGenericService, string> funct = null;
            string result = factory.ExecuteRequest(funct);

            Assert.Null(result);
        }

        [Fact]
        public void ExecuteRequestTResTest_NullFunct()
        {
            var factory = new ChannelFactory<IGenericService>();
            Func<IGenericService, string> funct = null;
            string result = factory.ExecuteRequest(funct);

            Assert.Null(result);
        }
    }
}