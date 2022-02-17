namespace Oliviann.Tests.Collections.Generic
{
    #region Usings

    using System;
    using Oliviann.Collections.Generic;
    using Xunit;

    #endregion Usings

    [Trait("Category", "CI")]
    public class IndexedLinkedListTests
    {
        #region ctor Tests

        [Fact]
        public void IndexedLinkedList_ctorTest()
        {
            var lst = new IndexedLinkedList<string>();

            Assert.Equal(0, lst.Count);
        }

        #endregion ctor Tests

        #region Add Tests

        [Fact]
        public void AddTest_Values()
        {
            var lst = new IndexedLinkedList<string>();
            Assert.Equal(0, lst.Count);

            lst.Add("Hello");
            lst.Add("World");
            lst.Add("Taco");

            Assert.Equal(3, lst.Count);
            string result = lst.First;
            Assert.Equal("Hello", result);
        }

        [Fact]
        public void AddTest_NullValue()
        {
            var lst = new IndexedLinkedList<string>();
            Assert.Equal(0, lst.Count);

            lst.Add("Hello");
            lst.Add("World");
            Assert.Throws<ArgumentNullException>(() => lst.Add(null));
        }

        [Fact]
        public void AddTest_DuplicateValue()
        {
            var lst = new IndexedLinkedList<string>();
            Assert.Equal(0, lst.Count);

            lst.Add("Hello");
            lst.Add("World");
            lst.Add("Hello");

            Assert.Equal(3, lst.Count);
            string result = lst.First;
            Assert.Equal("Hello", result);
        }

        #endregion Add Tests

        #region Clear Tests

        [Fact]
        public void ClearTest()
        {
            var lst = new IndexedLinkedList<string>();
            Assert.Equal(0, lst.Count);

            lst.Add("Hello");
            lst.Add("World");
            lst.Add("Taco");
            Assert.Equal(3, lst.Count);

            lst.Clear();
            Assert.Equal(0, lst.Count);
        }

        #endregion Clear Tests

        #region Remove Tests

        [Fact]
        public void RemoveFirstTest()
        {
            var lst = new IndexedLinkedList<string>();
            Assert.Equal(0, lst.Count);

            lst.Add("Hello");
            lst.Add("World");
            lst.Add("Taco");
            Assert.Equal(3, lst.Count);
            string result = lst.First;
            Assert.Equal("Hello", result);

            lst.RemoveFirst();
            Assert.Equal(2, lst.Count);
            result = lst.First;
            Assert.Equal("World", result);
        }

        [Fact]
        public void RemoveTest_Null()
        {
            var lst = new IndexedLinkedList<string>();
            Assert.Equal(0, lst.Count);

            lst.Add("Hello");
            lst.Add("World");
            lst.Add("Taco");
            Assert.Equal(3, lst.Count);

            Assert.Throws<ArgumentNullException>(() => lst.Remove(null));
        }

        [Fact]
        public void RemoveTest_MissingValue()
        {
            var lst = new IndexedLinkedList<string>();
            Assert.Equal(0, lst.Count);

            lst.Add("Hello");
            lst.Add("World");
            lst.Add("Taco");
            Assert.Equal(3, lst.Count);

            lst.Remove("Jumbo");
            Assert.Equal(3, lst.Count);
        }

        [Fact]
        public void RemoveTest_IncorrectCaseValue()
        {
            var lst = new IndexedLinkedList<string>();
            Assert.Equal(0, lst.Count);

            lst.Add("Hello");
            lst.Add("World");
            lst.Add("Taco");
            Assert.Equal(3, lst.Count);

            lst.Remove("hELLO");
            Assert.Equal(3, lst.Count);
        }

        [Fact]
        public void RemoveTest_Value()
        {
            var lst = new IndexedLinkedList<string>();
            Assert.Equal(0, lst.Count);

            lst.Add("Hello");
            lst.Add("World");
            lst.Add("Taco");
            Assert.Equal(3, lst.Count);

            lst.Remove("Hello");
            Assert.Equal(2, lst.Count);
        }

        #endregion Remove Tests
    }
}