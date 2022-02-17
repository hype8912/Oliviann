namespace Oliviann.Tests.Collections
{
    #region Usings

    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using Oliviann.Collections;
    using Xunit;

    #endregion Usings

    [Trait("Category", "CI")]
    public class CollectionExtensionsTests
    {
        #region AddRange Tests

        [Fact]
        public void CollectionAddRangeArrayTest_NullCollectionNullValues()
        {
            ICollection<string> col = null;
            string[] items = null;

            Assert.Throws<ArgumentNullException>(() => col.AddRange(items));
        }

        [Fact]
        public void CollectionAddRangeArrayTest_NullCollection()
        {
            ICollection<string> col = null;
            var items = new[] { "Three", "Four", "Five", "Six", "Seven" };

            Assert.Throws<ArgumentNullException>(() => col.AddRange(items));
        }

        [Fact]
        public void CollectionAddRangeArrayTest_NullValues()
        {
            ICollection<string> col = new List<string> { "One", "Two" };
            string[] items = null;

            col.AddRange(items);
            Assert.Equal(2, col.Count);
        }

        [Fact]
        public void CollectionAddRangeArrayTest_MultipleItems()
        {
            ICollection<string> col = new List<string> { "One", "Two" };
            var items = new[] { "Three", "Four", "Five", "Six", "Seven" };

            col.AddRange(items);
            Assert.Equal(7, col.Count);
        }

        [Fact]
        public void CollectionAddRangeArrayTest_DuplicateItems()
        {
            ICollection<string> col = new List<string> { "One", "Two" };
            var items = new[] { "Three", "Four", "One", "Six", "Two" };

            col.AddRange(items);
            Assert.Equal(7, col.Count);
        }

        [Fact]
        public void CollectionAddRangeEnumTest_NullCollectionNullValues()
        {
            ICollection<string> col = null;
            IEnumerable<string> items = null;

            Assert.Throws<ArgumentNullException>(() => col.AddRange(items));
        }

        [Fact]
        public void CollectionAddRangeEnumTest_NullCollection()
        {
            ICollection<string> col = null;
            IEnumerable<string> items = new List<string> { "Three", "Four", "Five", "Six", "Seven" };

            Assert.Throws<ArgumentNullException>(() => col.AddRange(items));
        }

        [Fact]
        public void CollectionAddRangeEnumTest_NullValues()
        {
            ICollection<string> col = new List<string> { "One", "Two" };
            IEnumerable<string> items = null;

            col.AddRange(items);
            Assert.Equal(2, col.Count);
        }

        [Fact]
        public void CollectionAddRangeEnumTest_MultipleItems()
        {
            ICollection<string> col = new List<string> { "One", "Two" };
            IEnumerable<string> items = new List<string> { "Three", "Four", "Five", "Six", "Seven" };

            col.AddRange(items);
            Assert.Equal(7, col.Count);
        }

        [Fact]
        public void CollectionAddRangeEnumTest_DuplicateItems()
        {
            ICollection<string> col = new List<string> { "One", "Two" };
            IEnumerable<string> items = new List<string> { "Three", "Four", "One", "Six", "Two" };

            col.AddRange(items);
            Assert.Equal(7, col.Count);
        }

        [Fact]
        public void CollectionAddRangeEnumTest_DuplicateItems_IgnoreDuplicates()
        {
            ICollection<string> col = new List<string> { "One", "Two" };
            IEnumerable<string> items = new List<string> { "Three", "Four", "One", "Six", "Two" };

            col.AddRange(items, true);
            Assert.Equal(5, col.Count);
        }

        #endregion AddRange Tests

        #region CopyTo Tests

        [Fact]
        public void CollectionCopyToTest_NullSourceNullTarget()
        {
            ICollection<string> source = null;
            ICollection<string> target = null;

            Assert.Throws<ArgumentNullException>(() => source.CopyTo(target));
        }

        [Fact]
        public void CollectionCopyToTest_NullSource()
        {
            ICollection<string> source = null;
            ICollection<string> target = new List<string> { "One", "Two" };

            source.CopyTo(target);
            Assert.Equal(0, target.Count);
        }

        [Fact]
        public void CollectionCopyToTest_Values()
        {
            ICollection<string> source = new List<string> { "Three", "Four", "One", "Six", "Two" };
            ICollection<string> target = new List<string> { "One", "Two" };

            source.CopyTo(target);
            Assert.Equal(5, source.Count);
            Assert.Equal(5, target.Count);
        }

        #endregion CopyTo Tests

        #region New Tests

        [Fact]
        public void KeyValuePairNewTest_NullValue()
        {
            var pair = new KeyValuePair<string, string>("Hello", "World");
            var result = pair.New(null);

            Assert.Null(result.Value);
        }

        [Fact]
        public void KeyValuePairNewTest_Value()
        {
            var pair = new KeyValuePair<string, string>("Hello", "World");
            KeyValuePair<string, string> result = pair.New("Earth");

            Assert.Equal("Earth", result.Value);
        }

        #endregion New Tests

        #region RemoveAll Tests

        [Fact]
        public void CollectionRemovalAllTest_NullCollectionNullMatch()
        {
            ICollection<string> source = null;
            Func<string, bool> act = null;

            Assert.Throws<ArgumentNullException>(() => source.RemoveAll(act));
        }

        [Fact]
        public void CollectionRemovalAllTest_NullCollection()
        {
            ICollection<string> source = null;
            Func<string, bool> act = s => s == "One";

            Assert.Throws<ArgumentNullException>(() => source.RemoveAll(act));
        }

        [Fact]
        public void CollectionRemovalAllTest_NullMatch()
        {
            ICollection<string> source = new List<string> { "Three", "Four", "One", "Six", "Two" };
            Func<string, bool> act = null;

            int result = source.RemoveAll(act);
            Assert.Equal(0, result);
            Assert.Equal(5, source.Count);
        }

        [Fact]
        public void CollectionRemovalAllTest_NoMatch()
        {
            ICollection<string> source = new List<string> { "Three", "Four", "One", "Six", "Two" };
            Func<string, bool> act = s => s == "Twelve";

            int result = source.RemoveAll(act);
            Assert.Equal(0, result);
            Assert.Equal(5, source.Count);
        }

        [Fact]
        public void CollectionRemovalAllTest_Match()
        {
            ICollection<string> source = new List<string> { "Three", "Four", "One", "Six", "Two" };
            Func<string, bool> act = s => s == "One";

            int result = source.RemoveAll(act);
            Assert.Equal(1, result);
            Assert.Equal(4, source.Count);
        }

        #endregion RemoveAll Tests

        #region ToArray Tests

        [Fact]
        public void StringCollectionToArrayTest_NullCollection()
        {
            StringCollection col = null;
            Assert.Throws<ArgumentNullException>(() => col.ToArray());
        }

        [Fact]
        public void StringCollectionToArrayTest_EmptyCollection()
        {
            var col = new StringCollection();
            string[] result = col.ToArray();

            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public void StringCollectionToArrayTest_FilledCollection()
        {
            var col = new StringCollection { "One", "Two", "Three", "Four" };
            string[] result = col.ToArray();

            Assert.NotNull(result);
            Assert.Equal(4, result.Length);
        }

        #endregion ToArray Tests
    }
}