namespace Oliviann.Tests.ComponentModel
{
    #region Usings

    using System;
    using System.ComponentModel.Composition.Hosting;
    using System.Reflection;
    using Oliviann.ComponentModel;
    using TestObjects;
    using Xunit;

    #endregion Usings

    [Trait("Category", "CI")]
    public class CompositionExtensionsTests
    {
        [Fact]
        public void ContainsTests_NullContainer()
        {
            CompositionContainer container = null;
            Assert.Throws<ArgumentNullException>(() => container.Contains<IGenericMocTestClass>());
        }

        [Fact]
        public void ContainsTests_NullContract()
        {
            var container = this.GetContainer();
            bool result = container.Contains<IGenericMocTestClass>(null);

            Assert.False(result);
        }

        [Fact]
        public void ContainsTests_MissingContract()
        {
            var container = this.GetContainer();
            bool result = container.Contains<IGenericMocTestClass>("TacoBell");

            Assert.False(result);
        }

        [Fact]
        public void ContainsTests_MatchingContract()
        {
            var container = this.GetContainer();
            bool result = container.Contains<IGenericMocTestClass>("GenericMocTest");

            Assert.True(result);
        }

        #region Helpers

        private CompositionContainer GetContainer()
        {
            var catalog = new AggregateCatalog();
            catalog.Catalogs.Add(new AssemblyCatalog(Assembly.GetExecutingAssembly()));
            return new CompositionContainer(catalog);
        }

        #endregion Helpers
    }
}