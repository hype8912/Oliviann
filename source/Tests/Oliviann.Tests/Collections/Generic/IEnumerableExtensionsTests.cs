namespace Oliviann.Tests.Collections.Generic
{
    #region Usings

    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Oliviann.Collections.Generic;
    using TestObjects;
    using Xunit;

    #endregion Usings

    [Trait("Category", "CI")]
    public class IEnumerableExtensionsTests
    {
        #region Add Tests

        [Fact]
        public void EnumerableAddTest_NullSource()
        {
            IEnumerable<string> col = null;
            Assert.Throws<ArgumentNullException>(() => col.Add(null));
        }

        [Fact]
        public void EnumerableAddTest_NullItem()
        {
            IEnumerable<string> col = Enumerable.Empty<string>();
            var result = col.Add(null);

            Assert.Single(result);
        }

        [Fact]
        public void EnumerableAddTest_SingleItem()
        {
            IEnumerable<string> col = Enumerable.Empty<string>();
            var result = col.Add("Hello");

            Assert.Single(result);
        }

        [Fact]
        public void EnumerableAddTest_MultipleItems()
        {
            IEnumerable<string> col = Enumerable.Empty<string>();
            var result1 = col.Add("Hello");
            var result2 = result1.Add("World");
            var result3 = result2.Add("Now");

            Assert.Equal(3, result3.Count());
        }

        #endregion Add Tests

        #region ForEach Tests

        [Fact]
        public void Test1_ForEach_NullSource()
        {
            Assert.Throws<ArgumentNullException>(() => IEnumerableExtensions.ForEach<string>(null, null));
        }

        [Fact]
        public void Test2_ForEach_NullAction()
        {
            Assert.Throws<ArgumentNullException>(() => IEnumerableExtensions.ForEach(new List<string>(), null));
        }

        [Fact]
        public void Test3_ForEach_EmptyList()
        {
            int count = 0;
            var lst = new List<string>();

            IEnumerableExtensions.ForEach(lst, s => count += 1);
            Assert.Equal(lst.Count, count);
        }

        [Fact]
        public void Test4_ForEach_StringList()
        {
            int count = 0;
            var lst = new List<string>(7)
                          {
                              "Hello", "Juan", "Happy", "Unit Testing", "World", "This", "Is"
                          };

            IEnumerableExtensions.ForEach(lst, s => count += 1);
            Assert.Equal(lst.Count, count);
        }

        #endregion ForEach Tests

        #region IsEmpty Tests

        [Fact]
        public void Test1_IsEmpty_Null()
        {
            bool returnedValue = IEnumerableExtensions.IsNullOrEmpty(null);
            Assert.True(returnedValue);
        }

        ////[Fact]
        ////public void Test1_IsEmptyT_Null()
        ////{
        ////    bool returnedValue = IEnumerableExtensions.IsNullOrEmpty<string>(null);
        ////    Assert.True(returnedValue);
        ////}

        [Fact]
        public void Test2_IsEmpty_Empty()
        {
            var lst = new ArrayList();
            bool returnedValue = lst.IsNullOrEmpty();

            Assert.True(returnedValue);
        }

        [Fact]
        public void Test2_IsEmptyT_Empty()
        {
            var lst = new List<string>();
            bool returnedValue = lst.IsNullOrEmpty();

            Assert.True(returnedValue);
        }

        [Fact]
        public void Test3_IsEmpty_Data()
        {
            var lst = new ArrayList { "Hello", "Juan", "Happy", "Unit Testing", "World", "This", "Is" };
            bool returnedValue = lst.IsNullOrEmpty();

            Assert.False(returnedValue);
        }

        [Fact]
        public void Test3_IsEmptyT_Data()
        {
            var lst = new List<string> { "Hello", "Juan", "Happy", "Unit Testing", "World", "This", "Is" };
            bool returnedValue = lst.IsNullOrEmpty();

            Assert.False(returnedValue);
        }

        #endregion IsEmpty Tests

        #region ToDataTable Tests

        /// <summary>
        /// Verifies an null enumerable source collection throws a argument null
        /// exception.
        /// </summary>
        [Fact]
        public void ToDataTableTest_NullSource()
        {
            IEnumerable<GenericMocTestClass> source = null;
            Assert.Throws<ArgumentNullException>(() => source.ToDataTable());
        }

        /// <summary>
        /// Verifies an empty enumerable source collection returns and empty
        /// data table.
        /// </summary>
        [Fact]
        public void ToDataTableTest_EmptySource()
        {
            IEnumerable<GenericMocTestClass> source = Enumerable.Empty<GenericMocTestClass>();
            DataTable result = source.ToDataTable();

            Assert.Equal(3, result.Columns.Count);
            Assert.Equal(0, result.Rows.Count);
        }

        /// <summary>
        /// Verifies a populated enumerable source collection of objects with no
        /// properties returns an empty data table.
        /// </summary>
        [Fact]
        public void ToDataTableTest_PopulatedSourceAndEmptyModels()
        {
            var items = new List<TestClassWithNoProperties>
            {
                new TestClassWithNoProperties(),
                new TestClassWithNoProperties(),
                new TestClassWithNoProperties()
            };

            DataTable result = items.ToDataTable();
            Assert.Empty(result.Columns);
            Assert.Equal(3, result.Rows.Count);
        }

        /// <summary>
        /// Verifies a populated enumerable source collection returns a
        /// populated data table.
        /// </summary>
        [Fact]
        public void ToDataTableTest_PopulatedSourceAndModels()
        {
            var items = new List<GenericMocTestClass>
            {
                new GenericMocTestClass { PropBool = true, PropInt = 99, PropString = "String1" },
                new GenericMocTestClass { PropInt = -10, PropString = "Taco Bell" },
                new GenericMocTestClass()
            };

            DataTable result = items.ToDataTable("Burger King");
            Assert.Equal(3, result.Columns.Count);
            Assert.Equal(3, result.Rows.Count);
            Assert.Equal("Burger King", result.TableName);
        }

        /// <summary>
        /// Verifies a populated enumerable source collection of models with
        /// nullable properties returns a populated data table.
        /// </summary>
        [Fact]
        public void ToDataTableTest_PopulatedSourceAndNullableProperties()
        {
            var items = new List<TestClassWithNullableProperties>
            {
                new TestClassWithNullableProperties { PropBool = true, PropInt = 99, PropDateTime = null },
                new TestClassWithNullableProperties { PropInt = -10, PropDateTime = DateTime.Now, PropBool = null },
                new TestClassWithNullableProperties { PropInt = null, PropDateTime = DateTime.Now, PropBool = false },
                new TestClassWithNullableProperties()
            };

            DataTable result = items.ToDataTable();
            Assert.Equal(3, result.Columns.Count);
            Assert.Equal(4, result.Rows.Count);
        }

        /// <summary>
        /// Verifies a populated enumerable source collection with an interface
        /// returns a populated data table.
        /// </summary>
        [Fact]
        public void ToDataTableTest_PopulatedSourceAndInterface()
        {
            var item = new List<IGenericMocTestClass>
            {
                new GenericMocTestClass { PropBool = true, PropInt = 99, PropString = "String1" },
                new GenericMocTestClass { PropInt = -10, PropString = "Taco Bell" },
                new GenericMocTestClass()
            };

            DataTable result = item.ToDataTable();
            Assert.Equal(3, result.Columns.Count);
            Assert.Equal(3, result.Rows.Count);
        }

        #endregion ToDataTable Tests

        #region ToListAsync Tests

        /// <summary>
        /// Verifies a null source throws an argument null exception.
        /// </summary>
        [Fact]
        public void ToListAsyncTest_NullSource()
        {
            IEnumerable<string> items = null;
            Assert.Throws<ArgumentNullException>(() => items.ToListAsync().Result);
        }

        /// <summary>
        /// Verifies the list count is correct and all the items are in the
        /// list.
        /// </summary>
        [Fact]
        public void ToListAsync_Values()
        {
            IEnumerable<Guid> items = Enumerable.Range(0, 1000).Select(i => Guid.NewGuid()).ToList();
            List<Guid> result = items.ToListAsync().Result;

            Assert.Equal(1000, result.Count);
            foreach (Guid item in items)
            {
                Assert.Contains(item, result);
            }
        }

        #endregion ToListAsync Tests
    }
}