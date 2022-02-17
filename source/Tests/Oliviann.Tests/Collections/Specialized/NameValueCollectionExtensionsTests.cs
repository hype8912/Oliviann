namespace Oliviann.Tests.Collections.Specialized
{
    #region Usings

    using System.Collections.Specialized;
    using Oliviann.Collections.Specialized;
    using Xunit;

    #endregion Usings

    [Trait("Category", "CI")]
    public class NameValueCollectionExtensionsTests
    {
        #region SetReadOnly Tests

        /// <summary>
        /// Verifies a null input collection returns a null collection.
        /// </summary>
        [Fact]
        public void SetReadOnlyTest_NullCollection()
        {
            NameValueCollection col = null;
            NameValueCollection result = col.SetReadOnly(true);

            Assert.Null(result);
        }

        /// <summary>
        /// Verifies setting a not read-only collection to not read-only doesn't
        /// throw an exception.
        /// </summary>
        [Fact]
        public void SetReadOnlyTest_FalseCollection()
        {
            var col = new NameValueCollection();
            Assert.False(col.IsReadOnly());

            NameValueCollection result = col.SetReadOnly(false);
            Assert.False(result.IsReadOnly());
        }

        /// <summary>
        /// Verifies the specified collection is set to read only.
        /// </summary>
        [Fact]
        public void SetReadOnlyTest_TrueCollection()
        {
            var col = new NameValueCollection();
            Assert.False(col.IsReadOnly());

            NameValueCollection result = col.SetReadOnly(true);
            Assert.True(result.IsReadOnly());
        }

        #endregion SetReadOnly Tests

        #region IsReadOnly Tests

        /// <summary>
        /// Verifies a null read only collection returns a false value.
        /// </summary>
        [Fact]
        public void IsReadOnlyTest_NullCollection()
        {
            NameValueCollection nvc = null;
            bool result = nvc.IsReadOnly();

            Assert.False(result);
        }

        #endregion IsReadOnly Tests
    }
}