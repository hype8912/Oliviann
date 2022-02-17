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
    public class ListItemExtenstionsTests
    {
        /// <summary>
        /// Verifies setting the tool tip with a null list item does not throw
        /// an exception.
        /// </summary>
        [Fact]
        public void ListItemSetToolTipTest_NullItem()
        {
            ListItem item = null;
            item.SetToolTip("sdjfjnkd");
        }

        [Theory]
        [InlineData(null, "ItemText")]
        [InlineData("", "")]
        [InlineData("Show me the tacos!", "Show me the tacos!")]
        public void ListItemSetToolTip_TextValues(string text, string expectedResult)
        {
            var item = new ListItem("ItemText", "ItemValue");
            item.SetToolTip(text);

            Assert.True(
                item.Attributes.Keys.Cast<string>().Any(key => key == "title"),
                "Title attribute not found in collection.");

            string result = item.Attributes["title"];
            Assert.Equal(expectedResult, result);
        }
    }
}

#endif