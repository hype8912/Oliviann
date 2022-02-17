namespace Oliviann.Tests.Data.Services
{
    #region Usings

    using System;
    using System.Collections.Generic;
    using System.Data.Services;
    using Oliviann.Data.Services;
    using Moq;
    using TestObjects;
    using Xunit;

    #endregion Usings

    [Trait("Category", "CI")]
    public class DataServiceConfigurationExtensionsTests
    {
        #region SetEntityAccessRules

        /// <summary>
        /// Verifies a null reference exception is thrown when a null
        /// configuration is passed in.
        /// </summary>
        [Fact]
        public void SetEntityAccessRulesTest_NullConfiguration()
        {
            IDataServiceConfiguration config = null;
            Assert.Throws<NullReferenceException>(() => config.SetEntityAccessRules<DataServiceTestClass>());
        }

        /// <summary>
        /// Verifies an exception isn't thrown when an object with no properties
        /// is set as the type.
        /// </summary>
        [Fact]
        public void SetEntityAccessRulesTest_NoPropertyEntity()
        {
            var properties = new Dictionary<string, EntitySetRights>();
            var mocConfig = new Mock<IDataServiceConfiguration>();
            mocConfig.Setup(c => c.SetEntitySetAccessRule(It.IsAny<string>(), It.IsAny<EntitySetRights>()))
                .Callback<string, EntitySetRights>((name, rights) => properties.Add(name, rights));

            mocConfig.Object.SetEntityAccessRules<object>();
            Assert.Empty(properties);
        }

        /// <summary>
        /// Verifies the correct properties right were set to the rights in the
        /// attribute.
        /// </summary>
        [Fact]
        public void SetEntityAccessRulesTest_SetPropertyEntities()
        {
            var properties = new Dictionary<string, EntitySetRights>();
            var mocConfig = new Mock<IDataServiceConfiguration>();
            mocConfig.Setup(c => c.SetEntitySetAccessRule(It.IsAny<string>(), It.IsAny<EntitySetRights>()))
                .Callback<string, EntitySetRights>((name, rights) => properties.Add(name, rights));

            mocConfig.Object.SetEntityAccessRules<DataServiceTestClass>();
            Assert.Equal(2, properties.Count);
            Assert.Equal(EntitySetRights.AllRead, properties["StringProperty"]);
            Assert.Equal(EntitySetRights.AllWrite, properties["ObjectProperty"]);
        }

        #endregion SetEntityAccessRules
    }
}