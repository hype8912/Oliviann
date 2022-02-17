#if NETFRAMEWORK

namespace Oliviann.Web.Tests.UI.WebControls
{
    #region Usings

    using System.Linq;
    using System.Web.UI.WebControls;
    using Collections.Generic;
    using Oliviann.Web.UI.WebControls;
    using Xunit;

    #endregion Usings

    [Trait("Category", "CI")]
    public class ListControlExtensionsTests
    {
        #region Concatenate Tests

        /// <summary>
        /// Verifies calling method with a null control returns an empty string.
        /// </summary>
        [Fact]
        public void ListControlConcatenateTest_NullControl()
        {
            ListControl ctrl = null;
            string result = ctrl.Concatenate();

            Assert.True(result == string.Empty, "Result string is not empty.[{0}]".FormatWith(result));
        }

        /// <summary>
        /// Verifies calling the method with an empty control returns an empty
        /// string.
        /// </summary>
        [Fact]
        public void ListControlConcatenateTest_EmptyControl()
        {
            ListControl ctrl = new ListBox();
            string result = ctrl.Concatenate();

            Assert.True(result == string.Empty, "Result is incorrect.[{0}]".FormatWith(result));
        }

        /// <summary>
        /// Verifies calling the method with a populated control and no items
        /// selected returns an empty string.
        /// </summary>
        [Fact]
        public void ListControlConcatenateTest_PopulatedControl_NoneSelected()
        {
            ListControl ctrl = new ListBox { Items = { "Pizza", "Hot Dog", "Taco", "Hamburger" } };
            string result = ctrl.Concatenate();

            Assert.True(result == string.Empty, "Result is incorrect.[{0}]".FormatWith(result));
        }

        /// <summary>
        /// Verifies calling the method with a populated control with selected
        /// items and a null separator returns the correct delimited string of
        /// items.
        /// </summary>
        [Fact]
        public void ListControlConcatenateTest_PopulatedControl_NullSeparator()
        {
            ListControl ctrl = new ListBox { Items = { "Pizza", "Hot Dog", "Taco", "Hamburger" } };
            ctrl.Items.Cast<ListItem>().ForEach(item => item.Selected = true);

            string result = ctrl.Concatenate(null);
            Assert.True(result == "Pizza,Hot Dog,Taco,Hamburger,", "Result is incorrect.[{0}]".FormatWith(result));
        }

        /// <summary>
        /// Verifies calling the method with a populated control with selected
        /// items and the default separator returns the correct delimited string
        /// of items.
        /// </summary>
        [Fact]
        public void ListControlConcatenateTest_PopulatedControl_DefaultSeparator()
        {
            ListControl ctrl = new ListBox { Items = { "Pizza", "Hot Dog", "Taco", "Hamburger" } };
            ctrl.Items.Cast<ListItem>().ForEach(item => item.Selected = true);

            string result = ctrl.Concatenate();
            Assert.True(result == "Pizza,Hot Dog,Taco,Hamburger,", "Result is incorrect.[{0}]".FormatWith(result));
        }

        /// <summary>
        /// Verifies calling the method with a populated control with selected
        /// items and a custom separator returns the correct delimited string
        /// of items.
        /// </summary>
        [Fact]
        public void ListControlConcatenateTest_PopulatedControl_CustomSeparator()
        {
            ListControl ctrl = new ListBox { Items = { "Pizza", "Hot Dog", "Taco", "Hamburger" } };
            ctrl.Items.Cast<ListItem>().ForEach(item => item.Selected = true);

            string result = ctrl.Concatenate("::");
            Assert.True(result == "Pizza::Hot Dog::Taco::Hamburger::", "Result is incorrect.[{0}]".FormatWith(result));
        }

        #endregion Concatenate Tests

        #region MyRegion

        /// <summary>
        /// Verifies an exception isn't thrown when a null control is passed in.
        /// </summary>
        [Fact]
        public void ListControlResetTest_NullControl()
        {
            ListControl ctrl = null;
            ctrl.Reset();
        }

        /// <summary>
        /// Verifies an exception isn't thrown when an empty control is passed
        /// in.
        /// </summary>
        [Fact]
        public void ListControlResetTest_EmptyControl()
        {
            ListControl ctrl = new ListBox();
            ctrl.Reset();
        }

        /// <summary>
        /// Verifies no items in the collection are selected when no items were
        /// selected at the start.
        /// </summary>
        [Fact]
        public void ListControlResetTest_NoSelectedItems()
        {
            ListControl ctrl = new ListBox { Items = { "Pizza", "Hot Dog", "Taco", "Hamburger" } };
            ctrl.Reset();

            ctrl.Items.Cast<ListItem>().ForEach(l => Assert.False(l.Selected));
        }

        /// <summary>
        /// Verifies no items in the collection are selected after items were
        /// selected.
        /// </summary>
        [Fact]
        public void ListControlResetTest_SelectedItems()
        {
            ListControl ctrl = new ListBox { Items = { "Pizza", "Hot Dog", "Taco", "Hamburger" } };
            ctrl.SelectByText("Taco");
            Assert.Contains(ctrl.Items.Cast<ListItem>(), l => l.Selected);

            ctrl.Reset();
            ctrl.Items.Cast<ListItem>().ForEach(l => Assert.False(l.Selected));
        }

        #endregion MyRegion

        #region SelectByText Tests

        /// <summary>
        /// Verifies passing in a null control returns false.
        /// </summary>
        [Fact]
        public void SelectByTextTest_NullControl()
        {
            ListControl ctrl = null;
            bool result = ctrl.SelectByText("Taco");

            Assert.False(result, "Item value was found and selected.");
        }

        /// <summary>
        /// Verifies passing in a control with no items returns false.
        /// </summary>
        [Fact]
        public void SelectByTextTest_NoItems()
        {
            ListControl ctrl = new ListBox();
            bool result = ctrl.SelectByText("Taco");

            Assert.False(result, "Item value was found and selected.");
        }

        /// <summary>
        /// Verifies passing in a control with no matching items returns false.
        /// </summary>
        [Fact]
        public void SelectByTextTest_NoMatchingItems()
        {
            ListControl ctrl = new ListBox { Items = { "Pizza", "Hot Dog", "Hamburger" } };
            bool result = ctrl.SelectByText("Taco");

            Assert.True(ctrl.Items.Count > 0, "Items collection count is 0.");
            Assert.False(result, "Item value was found and selected.");
        }

        /// <summary>
        /// Verifies passing in a control with a matching item returns true.
        /// </summary>
        [Fact]
        public void SelectByTextTest_MatchingItem()
        {
            ListControl ctrl = new ListBox { Items = { "Pizza", "Hot Dog", "Taco", "Hamburger" } };
            bool result = ctrl.SelectByText("Taco");

            Assert.True(ctrl.Items.Count > 0);
            Assert.True(result, "Item value was not found or selected.");
        }

        #endregion SelectByText Tests

        #region SelectByValue Tests

        /// <summary>
        /// Verifies passing in a null control returns false.
        /// </summary>
        [Fact]
        public void SelectByValueTest_NullControl()
        {
            ListControl ctrl = null;
            bool result = ctrl.SelectByValue("Taco");

            Assert.False(result, "Item value was found and selected.");
        }

        /// <summary>
        /// Verifies passing in a control with no items returns false.
        /// </summary>
        [Fact]
        public void SelectByValueTest_NoItems()
        {
            ListControl ctrl = new ListBox();
            bool result = ctrl.SelectByValue("Taco");

            Assert.False(result, "Item value was found and selected.");
        }

        /// <summary>
        /// Verifies passing in a control with no matching items returns false.
        /// </summary>
        [Fact]
        public void SelectByValueTest_NoMatchingItems()
        {
            ListControl ctrl = new ListBox { Items = { "Pizza", "Hot Dog", "Hamburger" } };
            bool result = ctrl.SelectByValue("Taco");

            Assert.True(ctrl.Items.Count > 0);
            Assert.False(result, "Item value was found and selected.");
        }

        /// <summary>
        /// Verifies passing in a control with a matching item returns true.
        /// </summary>
        [Fact]
        public void SelectByValueTest_MatchingItem()
        {
            ListControl ctrl = new ListBox { Items = { "Pizza", "Hot Dog", "Taco", "Hamburger" } };
            bool result = ctrl.SelectByValue("Taco");

            Assert.True(ctrl.Items.Count > 0);
            Assert.True(result, "Item value was not found or selected.");
        }

        #endregion SelectByValue Tests

        #region Select Tests

        /// <summary>
        /// Verifies the method returns false if the list item was null.
        /// </summary>
        [Fact]
        public void ListItemSelectTest_Null()
        {
            ListItem item = null;
            bool result = item.Select();

            Assert.False(result, "Return result was true.");
        }

        /// <summary>
        /// Verifies a false list item returns true and selected is set to true.
        /// </summary>
        [Fact]
        public void ListItemSelectTest_ValidItem_False()
        {
            var item = new ListItem("Taco", "Bell") { Selected = false };
            bool result = item.Select();

            Assert.True(result, "Return result was false.");
            Assert.True(item.Selected, "List item was not selected.");
        }

        /// <summary>
        /// Verifies a true list item returns true and selected is set to true.
        /// </summary>
        [Fact]
        public void ListItemSelectTest_ValidItem_True()
        {
            var item = new ListItem("Taco", "Bell") { Selected = true };
            bool result = item.Select();

            Assert.True(result, "Return result was false.");
            Assert.True(item.Selected, "List item was not selected.");
        }

        #endregion Select Tests
    }
}

#endif