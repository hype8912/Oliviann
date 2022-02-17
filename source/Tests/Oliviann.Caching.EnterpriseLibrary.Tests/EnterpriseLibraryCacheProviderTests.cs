namespace Oliviann.Caching.EnterpriseLibrary.Tests
{
    #region Usings

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Oliviann.Caching.Providers;
    using Xunit;

    #endregion Usings

    [Trait("Category", "NOT_FINISHED")]
    public class EnterpriseLibraryCacheProviderTests
    {
        #region Add Tests

        /// <summary>
        /// Verifies adding a null entry returns false.
        /// </summary>
        [Fact]
        public void EnterpriseLibraryCacheAddTest_NullEntry()
        {
            ICacheProvider provider = new EnterpriseLibraryCacheProvider();

            bool result = provider.Add(null);
            Assert.False(result);
        }

        #endregion
    }
}