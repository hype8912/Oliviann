namespace Oliviann.Tests.Linq.Dynamic
{
    #region Usings

    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Oliviann.Linq.Dynamic;
    using TestObjects;
    using Xunit;

    #endregion Usings

    [Trait("Category", "CI")]
    public class IQueryableExtensionsTests
    {
        #region OrderBy Tests

        [Fact]
        public void OrderByTest_NullSource()
        {
            IQueryable<string> q = null;
            Assert.Throws<ArgumentNullException>(() => q.OrderBy("One"));
        }

        [Fact]
        public void OrderByTest_NullProperty()
        {
            IQueryable<GenericMocTestClass> q = this.GetFilledList();
            Assert.Throws<ArgumentNullException>(() => q.OrderBy(null));
        }

        [Fact]
        public void OrderByTest_StringProp()
        {
            IQueryable<GenericMocTestClass> q = this.GetFilledList();
            var result = q.OrderBy("PropString");

            Assert.Equal(10, result.Count());
            Assert.Equal("Eight", result.First().PropString);
            Assert.Equal("Two", result.Last().PropString);
        }

        [Fact]
        public void OrderByTest_IntProp()
        {
            IQueryable<GenericMocTestClass> q = this.GetFilledList();
            var result = q.OrderBy("PropInt");

            Assert.Equal(10, result.Count());
            Assert.Equal(1, result.First().PropInt);
            Assert.Equal(10, result.Last().PropInt);
        }

        #endregion OrderBy Tests

        #region OrderByDescending Tests

        [Fact]
        public void OrderByDescendingTest_NullSource()
        {
            IQueryable<string> q = null;
            Assert.Throws<ArgumentNullException>(() => q.OrderByDescending("One"));
        }

        [Fact]
        public void OrderByDescendingTest_NullProperty()
        {
            IQueryable<GenericMocTestClass> q = this.GetFilledList();
            Assert.Throws<ArgumentNullException>(() => q.OrderByDescending(null));
        }

        [Fact]
        public void OrderByDescendingTest_StringProp()
        {
            IQueryable<GenericMocTestClass> q = this.GetFilledList();
            var result = q.OrderByDescending("PropString");

            Assert.Equal(10, result.Count());
            Assert.Equal("Two", result.First().PropString);
            Assert.Equal("Eight", result.Last().PropString);
        }

        [Fact]
        public void OrderByDescendingTest_IntProp()
        {
            IQueryable<GenericMocTestClass> q = this.GetFilledList();
            var result = q.OrderByDescending("PropInt");

            Assert.Equal(10, result.Count());
            Assert.Equal(10, result.First().PropInt);
            Assert.Equal(1, result.Last().PropInt);
        }

        #endregion OrderByDescending Tests

        #region ThenBy Tests

        [Fact]
        public void ThenByTest_NullSource()
        {
            IOrderedQueryable<string> q = null;
            Assert.Throws<ArgumentNullException>(() => q.ThenBy("One"));
        }

        [Fact]
        public void ThenByTest_NullProperty()
        {
            IOrderedQueryable<GenericMocTestClass> q = this.GetFilledList2();
            Assert.Throws<ArgumentNullException>(() => q.ThenBy(null));
        }

        [Fact]
        public void ThenByTest_StringProp()
        {
            IOrderedQueryable<GenericMocTestClass> q = this.GetFilledList2();
            var result = q.ThenBy("PropString");

            Assert.Equal(10, result.Count());
            Assert.Equal("Three", result.Last().PropString);
            Assert.Equal("Eight", result.First().PropString);
        }

        #endregion ThenBy Tests

        #region ThenByDescending Tests

        [Fact]
        public void ThenByDescendingTest_NullSource()
        {
            IOrderedQueryable<string> q = null;
            Assert.Throws<ArgumentNullException>(() => q.ThenByDescending("One"));
        }

        [Fact]
        public void ThenByDescendingTest_NullProperty()
        {
            IOrderedQueryable<GenericMocTestClass> q = this.GetFilledList2();
            Assert.Throws<ArgumentNullException>(() => q.ThenByDescending(null));
        }

        [Fact]
        public void ThenByDescendingTest_StringProp()
        {
            IOrderedQueryable<GenericMocTestClass> q = this.GetFilledList2();
            var result = q.ThenByDescending("PropString");

            Assert.Equal(10, result.Count());
            Assert.Equal("Two", result.First().PropString);
            Assert.Equal("Five", result.Last().PropString);
        }

        #endregion ThenByDescending Tests

        #region ToList Tests

        [Fact]
        public void IQueryableToListTest_NullQuery()
        {
            IQueryable q = null;
            Assert.Throws<ArgumentNullException>(() => q.ToList());
        }

        [Fact]
        public void IQueryableToListTest_EmptyQuery()
        {
            IQueryable q = new List<string>().AsQueryable();
            IList result = q.ToList();

            Assert.Equal(0, result.Count);
        }

        [Fact]
        public void IQueryableToListTest_Values()
        {
            var lst = new List<string> { "One", "Two", "Three", "Four", "Five" };
            IQueryable q = lst.AsQueryable();
            IList result = q.ToList();

            Assert.Equal(5, result.Count);
        }

        #endregion ToList Tests

        #region Helpers

        private IQueryable<GenericMocTestClass> GetFilledList()
        {
            var lst = new List<GenericMocTestClass>
                {
                    new GenericMocTestClass { PropString = "One", PropInt = 1 },
                    new GenericMocTestClass { PropString = "Two", PropInt = 2 },
                    new GenericMocTestClass { PropString = "Three", PropInt = 3 },
                    new GenericMocTestClass { PropString = "Ten", PropInt = 10 },
                    new GenericMocTestClass { PropString = "Four", PropInt = 4 },
                    new GenericMocTestClass { PropString = "Eight", PropInt = 8 },
                    new GenericMocTestClass { PropString = "Six", PropInt = 6 },
                    new GenericMocTestClass { PropString = "Seven", PropInt = 7 },
                    new GenericMocTestClass { PropString = "Nine", PropInt = 9 },
                    new GenericMocTestClass { PropString = "Five", PropInt = 5 },
                };

            return lst.AsQueryable();
        }

        private IOrderedQueryable<GenericMocTestClass> GetFilledList2()
        {
            var lst = new List<GenericMocTestClass>
                {
                    new GenericMocTestClass { PropString = "One", PropInt = 1, PropBool = true },
                    new GenericMocTestClass { PropString = "Two", PropInt = 2, PropBool = false },
                    new GenericMocTestClass { PropString = "Three", PropInt = 3, PropBool = true },
                    new GenericMocTestClass { PropString = "Ten", PropInt = 10, PropBool = false },
                    new GenericMocTestClass { PropString = "Four", PropInt = 4, PropBool = false },
                    new GenericMocTestClass { PropString = "Eight", PropInt = 8, PropBool = false },
                    new GenericMocTestClass { PropString = "Six", PropInt = 6, PropBool = false },
                    new GenericMocTestClass { PropString = "Seven", PropInt = 7, PropBool = true },
                    new GenericMocTestClass { PropString = "Nine", PropInt = 9, PropBool = true },
                    new GenericMocTestClass { PropString = "Five", PropInt = 5, PropBool = true },
                };

            return lst.AsQueryable().OrderBy("PropBool");
        }

        #endregion Helpers
    }
}