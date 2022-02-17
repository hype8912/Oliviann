namespace Oliviann.Tests.Collections.Generic
{
    #region Usings

    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using Oliviann.Collections.Generic;
    using Oliviann.Common.TestObjects.Xml;
    using Xunit;

    #endregion Usings

    [Trait("Category", "CI")]
    public class SortableBindingListTest
    {
        #region ctor Tests

        [Fact]
        public void SortableBindingList_ctorTest()
        {
            var bl = new SortableBindingList<string>();
            Assert.Empty(bl);
        }

        [Fact]
        public void SortableBindingList_ctorTest_NullEnumerator()
        {
            Assert.Throws<ArgumentNullException>(() => new SortableBindingList<string>(null));
        }

        [Fact]
        public void SortableBindingList_ctorTest_Enumerator()
        {
            IEnumerable<string> ie = new List<string> { "A", "B", "C" };
            var bl = new SortableBindingList<string>(ie);

            Assert.Equal(3, bl.Count);
        }

        [Fact]
        public void IBindingList_ctorTest()
        {
            IBindingList bl = new SortableBindingList<string>();

            Assert.Equal(0, bl.Count);
            Assert.Equal(ListSortDirection.Ascending, bl.SortDirection);
            Assert.False(bl.IsSorted);
            Assert.True(bl.SupportsSearching);
            Assert.True(bl.SupportsSorting);
            Assert.Null(bl.SortProperty);
        }

        #endregion ctor Tests

        #region Other Tests

        [Fact]
        public void SortableBindingListTest_SortNullPropertyName()
        {
            var lst = new SortableBindingList<Book>();
            lst.Add(new Book { Author = "NavPress", Id = 3 });
            lst.Add(new Book { Author = "Somebody", Id = 1 });
            lst.Add(new Book { Author = "SomebodyElse", Id = 2 });
            Assert.Equal(3, lst[0].Id);

            lst.Sort(null, ListSortDirection.Ascending);
            Assert.Equal(3, lst[0].Id);
        }

        [Fact]
        public void SortableBindingListTest_SortEmptyPropertyName()
        {
            var lst = new SortableBindingList<Book>();
            lst.Add(new Book { Author = "NavPress", Id = 3 });
            lst.Add(new Book { Author = "Somebody", Id = 1 });
            lst.Add(new Book { Author = "SomebodyElse", Id = 2 });
            Assert.Equal(3, lst[0].Id);

            lst.Sort(string.Empty, ListSortDirection.Ascending);
            Assert.Equal(3, lst[0].Id);
        }

        [Fact]
        public void SortableBindingListTest_SortWrongPropertyName()
        {
            var lst = new SortableBindingList<Book>();
            lst.Add(new Book { Author = "NavPress", Id = 3 });
            lst.Add(new Book { Author = "Somebody", Id = 1 });
            lst.Add(new Book { Author = "SomebodyElse", Id = 2 });
            Assert.Equal(3, lst[0].Id);

            Assert.Throws<InvalidOperationException>(() => lst.Sort("Taco", ListSortDirection.Ascending));
        }

        [Fact]
        public void SortableBindingListTest_SortObjectNoProperties()
        {
            var lst = new SortableBindingList<object>();
            Assert.Empty(lst);

            lst.Sort("Taco", ListSortDirection.Ascending);
        }

        [Fact]
        public void SortableBindingListTest_SortThreeUniqueDefinedTypeItemsAsc()
        {
            var lst = new SortableBindingList<Book>();
            lst.Add(new Book { Author = "NavPress", Id = 1, Title = "How to find the one", Year = 1996 });
            lst.Add(new Book { Author = "Somebody", Id = 3, Title = "A book title", Year = 2104 });
            lst.Add(new Book { Author = "Somebody", Id = 2, Title = "Another book title", Year = 2014 });

            Assert.Equal(3, lst.Count);

            lst.Sort("Id", ListSortDirection.Ascending);
            Assert.Equal(1, lst[0].Id);
        }

        [Fact]
        public void SortableBindingListTest_SortThreeUniqueDefinedTypeItemsDesc()
        {
            var lst = new SortableBindingList<Book>();
            lst.Add(new Book { Author = "NavPress", Id = 1, Title = "How to find the one", Year = 1996 });
            lst.Add(new Book { Author = "Somebody", Id = 3, Title = "A book title", Year = 2104 });
            lst.Add(new Book { Author = "Somebody", Id = 2, Title = "Another book title", Year = 2014 });

            Assert.Equal(3, lst.Count);

            lst.Sort("Id", ListSortDirection.Descending);
            Assert.Equal(3, lst[0].Id);
        }

        [Fact]
        public void SortableBindingListTest_SortStrings()
        {
            var lst = new SortableBindingList<MyClass>();
            lst.Add(new MyClass { Id = 3});
            lst.Add(new MyClass { Id = 1, Name = "Hello" });
            lst.Add(new MyClass { Id = 2, Name = "Nope" });
            lst.Add(new MyClass { Id = 7, Name = string.Empty });
            lst.Add(new MyClass { Id = 5 });

            Assert.Equal(5, lst.Count);

            lst.Sort("Name", ListSortDirection.Ascending);
            Assert.Equal(3, lst[0].Id);
        }

        [Fact]
        public void SortableBindingListTest_SortObjects()
        {
            var lst = new SortableBindingList<MyClass>();
            lst.Add(new MyClass { Id = 3, Value = new object() });
            lst.Add(new MyClass { Id = 1, Name = "Hello", Value = null });
            lst.Add(new MyClass { Id = 2, Name = "Nope", Value = null });
            lst.Add(new MyClass { Id = 7, Name = string.Empty, Value = new object() });
            lst.Add(new MyClass { Id = 5, Value = new object() });

            Assert.Equal(5, lst.Count);

            lst.Sort("Value", ListSortDirection.Ascending);
        }

        [Fact]
        public void SortableBindingListTest_SortBools()
        {
            var lst = new SortableBindingList<MyClass>();
            lst.Add(new MyClass { Id = 3, IsValid = true });
            lst.Add(new MyClass { Id = 1, Name = "Hello", IsValid = true });
            lst.Add(new MyClass { Id = 2, Name = "Nope", IsValid = false });
            lst.Add(new MyClass { Id = 7, Name = string.Empty, IsValid = true });
            lst.Add(new MyClass { Id = 5, IsValid = false });

            Assert.Equal(5, lst.Count);

            lst.Sort("IsValid", ListSortDirection.Ascending);
        }

        [Fact]
        public void SortableBindingListTest_SortEqual()
        {
            var lst = new SortableBindingList<MyClass>();
            lst.Add(new MyClass { Id = 3, Name = "Hello", IsValid = true });
            lst.Add(new MyClass { Id = 3, Name = "Nope", IsValid = false });
            lst.Add(new MyClass { Id = 7, Name = string.Empty, IsValid = true });
            lst.Add(new MyClass { Id = 5, IsValid = false });
            lst.Add(new MyClass { Id = 1, Name = "Hello", IsValid = true });

            Assert.Equal(5, lst.Count);

            lst.Sort("Id", ListSortDirection.Ascending);
            Assert.Equal(lst[2].Id, lst[1].Id);
        }

        #endregion Other Tests

        #region Helpers

        public class MyClass
        {
            public int Id { get; set; }

            public string Name { get; set; }

            public object Value { get; set; }

            public bool IsValid { get; set; }
        }

        #endregion Helpers
    }
}