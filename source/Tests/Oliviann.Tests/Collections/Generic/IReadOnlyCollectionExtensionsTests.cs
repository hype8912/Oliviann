namespace Oliviann.Tests.Collections.Generic
{
    #region Usings

    using System.Collections.Generic;
    using Oliviann.Collections.Generic;
    using Xunit;

    #endregion Usings

    [Trait("Category", "CI")]
    public class IReadOnlyCollectionExtensionsTests
    {
        /// <summary>
        /// Verifies a null collection returns true.
        /// </summary>
        [Fact]
        public void IReadOnlyCollection_IsNullOrEmptyTest_NullCollection()
        {
            IReadOnlyCollection<string> col = null;
            bool result = col.IsNullOrEmpty();

            Assert.True(result);
        }

        /// <summary>
        /// Verifies an empty collection returns true.
        /// </summary>
        [Fact]
        public void IReadOnlyCollection_IsNullOrEmptyTest_EmptyCollection()
        {
            IReadOnlyCollection<string> col = new List<string>();
            bool result = col.IsNullOrEmpty();

            Assert.True(result);
        }

        /// <summary>
        /// Verifies a populated collection return false.
        /// </summary>
        [Fact]
        public void IReadOnlyCollection_IsNullOrEmptyTest_PopulatedCollection()
        {
            IReadOnlyCollection<string> col = new List<string> { "Taco", "Oliviann", "Pizza" };
            bool result = col.IsNullOrEmpty();

            Assert.False(result);
        }
    }
}