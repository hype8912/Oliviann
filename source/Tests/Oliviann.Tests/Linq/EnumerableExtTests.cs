namespace Oliviann.Tests.Linq
{
    #region Usings

    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Oliviann.Linq;
    using TestObjects;
    using Xunit;

    #endregion Usings

    [Trait("Category", "CI")]
    public class EnumerableExtTests
    {
        #region Count Tests

        [Fact]
        public void IEnumerableCountTest_Null()
        {
            IEnumerable items = null;
            Assert.Throws<ArgumentNullException>(() => items.Count());
        }

        [Fact]
        public void IEnumerableCountTest_Collection()
        {
            ICollection items = this.CreatePopulateStringList();
            int result = items.Count();

            Assert.Equal(20, result);
        }

        [Fact]
        public void IEnumerableCountTest_Queryable()
        {
            IQueryable items = this.CreatePopulateStringList().AsQueryable();
            int result = items.Count();

            Assert.Equal(20, result);
        }

        #endregion Count Tests

        #region Distinct Tests

        [Fact]
        public void IEnumerableDistinctTest_NullSource()
        {
            Assert.Throws<ArgumentNullException>(() => EnumerableExt.Distinct<string, string>(null, null).ToList());
        }

        [Fact]
        public void IEnumerableDistinctTest_NullSelector()
        {
            Assert.Throws<ArgumentNullException>(
                () => EnumerableExt.Distinct<string, string>(this.CreatePopulateStringList(), null).ToList());
        }

        [Fact]
        public void IEnumerableDistinctTest_Items()
        {
            var items = new List<GenericMocTestClass>
                {
                    new GenericMocTestClass { PropString = "One" },
                    new GenericMocTestClass { PropString = "Two" },
                    new GenericMocTestClass { PropString = "Three" },
                    new GenericMocTestClass { PropString = "One" },
                    new GenericMocTestClass { PropString = "Two" },
                    new GenericMocTestClass { PropString = "Three" },
                };

            var result = items.Distinct(g => g.PropString).ToList();
            Assert.Equal(3, result.Count);
        }

        [Fact]
        public void IEnumerableDistinctKeyTest_NullSource()
        {
            Assert.Throws<ArgumentNullException>(() => EnumerableExt.DistinctKey<string, string>(null, null).ToList());
        }

        [Fact]
        public void IEnumerableDistinctKeyTest_NullSelector()
        {
            Assert.Throws<ArgumentNullException>(
                () => EnumerableExt.DistinctKey<string, string>(this.CreatePopulateStringList(), null).ToList());
        }

        [Fact]
        public void IEnumerableDistinctKeyTest_Items()
        {
            var items = new List<GenericMocTestClass>
                {
                    new GenericMocTestClass { PropString = "One" },
                    new GenericMocTestClass { PropString = "Two" },
                    new GenericMocTestClass { PropString = "Three" },
                    new GenericMocTestClass { PropString = "One" },
                    new GenericMocTestClass { PropString = "Two" },
                    new GenericMocTestClass { PropString = "Three" },
                };

            var result = items.DistinctKey(g => g.PropString).ToList();
            Assert.Equal(3, result.Count);
            Assert.Equal(1, result.Count(a => a == "One"));
            Assert.Equal(1, result.Count(a => a == "Two"));
            Assert.Equal(1, result.Count(a => a == "Three"));
        }

        #endregion Distinct Tests

        #region Duplicate Tests

        [Fact]
        public void IEnumerableDuplicatesTest_NullCollection()
        {
            IEnumerable<string> source = null;
            Assert.Throws<ArgumentNullException>(() => source.Duplicates());
        }

        [Fact]
        public void IEnumerableDuplicatesTest_EmptyCollection()
        {
            IEnumerable<string> source = Enumerable.Empty<string>();

            IEnumerable<string> result = source.Duplicates();
            Assert.Empty(result);
        }

        [Fact]
        public void IEnumerableDuplicatesTest_NonDuplicateCollection()
        {
            IEnumerable<string> source = new List<string> { "Yellow", "Blue", "Purple", "Red", "Pink", "Black", "White" };

            IEnumerable<string> result = source.Duplicates();
            Assert.Empty(result);
        }

        [Fact]
        public void IEnumerableDuplicatesTest_DuplicateCollection()
        {
            IEnumerable<string> source = new List<string> { "Yellow", "Blue", "Purple", "Blue", "Red", "Pink", "Black", "White", "Red", "Red" };

            IEnumerable<string> result = source.Duplicates();
            Assert.Equal(2, result.Count());
            Assert.Contains("Blue", source);
            Assert.Contains("Red", source);
        }

        #endregion

        #region Except Tests

        [Fact]
        public void IEnumerableExceptTest_NullFirst()
        {
            Assert.Throws<ArgumentNullException>(
                () => EnumerableExt.Except(null, new List<GenericMocTestClass>(), (a, i) => a.PropString == i.PropString));
        }

        [Fact]
        public void IEnumerableExceptTest_NullSecond()
        {
            Assert.Throws<ArgumentNullException>(
                () => EnumerableExt.Except(new List<GenericMocTestClass>(), null, (a, i) => a.PropString == i.PropString));
        }

        [Fact]
        public void IEnumerableExceptTest_NullComparer()
        {
            Assert.Throws<ArgumentNullException>(
                () => EnumerableExt.Except(new List<GenericMocTestClass>(), new List<GenericMocTestClass>(), null));
        }

        [Fact]
        public void IEnumerableExceptTest_Comparer()
        {
            var items1 = new List<GenericMocTestClass>
                {
                    new GenericMocTestClass { PropString = "One" },
                    new GenericMocTestClass { PropString = "Two" },
                    new GenericMocTestClass { PropString = "Three" },
                    new GenericMocTestClass { PropString = "One" },
                    new GenericMocTestClass { PropString = "Two" },
                    new GenericMocTestClass { PropString = "Three" },
                };

            var items2 = new List<GenericMocTestClass>
                {
                    new GenericMocTestClass { PropString = "Three" },
                };

            IEnumerable<GenericMocTestClass> result = items1.Except(items2, (i1, i2) => i1.PropString == i2.PropString);
            Assert.Equal(2, result.Count());
        }

        #endregion Except Tests

        #region OfTypeSafe Tests

        [Fact]
        public void OfTypeSafeTest_Null()
        {
            IList items = null;

            var result = items.OfTypeSafe<string>();
            Assert.Empty(result);
        }

        [Fact]
        public void OfTypeSafeTest_Types()
        {
            IList items = this.CreatePopulateStringList();

            var result1 = items.OfTypeSafe<string>();
            Assert.Equal(20, result1.Count());

            var result2 = items.OfTypeSafe<int>();
            Assert.Empty(result2);
        }

        #endregion OfTypeSafe Tests

        #region ToReadOnlyCollection Tests

        [Fact]
        public void ToReadOnlyCollectionTest_Null()
        {
            IEnumerable<string> items = null;
            Assert.Throws<ArgumentNullException>(() => items.ToReadOnlyCollection());
        }

        [Fact]
        public void ToReadOnlyCollectionTest_Items()
        {
            IEnumerable<string> items = this.CreatePopulateStringList();
            IList<string> result = items.ToReadOnlyCollection();

            Assert.Equal(20, result.Count);
            Assert.True(result.IsReadOnly, "List read-only property was not set to read only.");
        }

        #endregion ToReadOnlyCollection Tests

        #region Union Tests

        [Fact]
        public void IEnumerableUnionTest_NullFirst()
        {
            Assert.Throws<ArgumentNullException>(
                () => EnumerableExt.Union(null, new List<GenericMocTestClass>(), (a, i) => a.PropString == i.PropString));
        }

        [Fact]
        public void IEnumerableUnionTest_NullSecond()
        {
            Assert.Throws<ArgumentNullException>(
                () => EnumerableExt.Union(new List<GenericMocTestClass>(), null, (a, i) => a.PropString == i.PropString));
        }

        [Fact]
        public void IEnumerableUnionTest_NullComparer()
        {
            Assert.Throws<ArgumentNullException>(
                () => EnumerableExt.Union(new List<GenericMocTestClass>(), new List<GenericMocTestClass>(), null));
        }

        [Fact]
        public void IEnumerableUnionTest_Comparer()
        {
            var items1 = new List<GenericMocTestClass>
                {
                    new GenericMocTestClass { PropString = "One" },
                    new GenericMocTestClass { PropString = "Two" },
                    new GenericMocTestClass { PropString = "Three" },
                };

            var items2 = new List<GenericMocTestClass>
                {
                    new GenericMocTestClass { PropString = "Four" },
                    new GenericMocTestClass { PropString = "Five" },
                    new GenericMocTestClass { PropString = "Six" },
                };

            IEnumerable<GenericMocTestClass> result = items1.Union(items2, (i1, i2) => i1.PropString == i2.PropString);
            Assert.Equal(6, result.Count());
        }

        #endregion Union Tests

        #region Helpers

        private List<string> CreatePopulateStringList()
        {
            return new List<string>
                {
                    "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Eleven", "Twelve",
                    "Thirteen", "Fourteen", "Fifthteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen", "Twenty"
                };
        }

        #endregion Helpers
    }
}