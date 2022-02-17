#if NETFRAMEWORK

namespace Oliviann.Web.Tests.UI.WebControls
{
    #region Usings

    using System.Linq;
    using System.Web.UI.WebControls;
    using Oliviann.Web.UI.WebControls;
    using Xunit;

    #endregion Usings

    [Trait("Category", "CI")]
    public class ListItemCollectionExtensionsTests
    {
        #region SortByText Tests

        /// <summary>
        /// Verifies that no exception is thrown when a null collection is
        /// passed and that the collection remains null.
        /// </summary>
        [Fact]
        public void SortByTextTest_NullCollection()
        {
            ListItemCollection items = null;
            items.SortByText();

            Assert.Null(items);
        }

        /// <summary>
        /// Verifies and empty collection doesn't thrown an exception.
        /// </summary>
        [Fact]
        public void SortByTextTest_EmptyCollection()
        {
            var items = new ListItemCollection();
            items.SortByText();

            Assert.Empty(items);
        }

        /// <summary>
        /// A list item collection for performing tests.
        /// </summary>
        private ListItemCollection ValidTestItemsCollection =
            new ListItemCollection
            {
                new ListItem("Reason for Change", "1"),
                new ListItem("Description of Change", "2"),
                new ListItem("Change Instructions"),
                new ListItem("Retest Instructions", "4"),
            };

        /// <summary>
        /// Verifies a populated collection is re-populated in ascending order.
        /// </summary>
        [Fact]
        public void SortByTextTest_ValidItemsAscending()
        {
            var items = new ListItemCollection();
            items.AddRange(this.ValidTestItemsCollection.Cast<ListItem>().ToArray());
            items.SortByText();

            Assert.NotNull(items);
            Assert.Equal(4, items.Count);
            Assert.Equal("Change Instructions", items[0].Text);
            Assert.Equal("Description of Change", items[1].Text);
            Assert.Equal("Reason for Change", items[2].Text);
            Assert.Equal("Retest Instructions", items[3].Text);
        }

        /// <summary>
        /// Verifies a populated collection is re-populated in descending order.
        /// </summary>
        [Fact]
        public void SortByTextTest_ValidItemsDescending()
        {
            var items = new ListItemCollection();
            items.AddRange(this.ValidTestItemsCollection.Cast<ListItem>().ToArray());
            items.SortByText(true);

            Assert.NotNull(items);
            Assert.Equal(4, items.Count);
            Assert.Equal("Change Instructions", items[3].Text);
            Assert.Equal("Description of Change", items[2].Text);
            Assert.Equal("Reason for Change", items[1].Text);
            Assert.Equal("Retest Instructions", items[0].Text);
        }

        #endregion SortByText Tests
    }
}

#endif