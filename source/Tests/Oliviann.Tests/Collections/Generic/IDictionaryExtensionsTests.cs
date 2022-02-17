namespace Oliviann.Tests.Collections.Generic
{
    #region Usings

    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using Oliviann.Collections.Generic;
    using Xunit;

    #endregion Usings

    [Trait("Category", "CI")]
    public class IDictionaryExtensionsTests
    {
        //#region AddNotContains

        ///// <summary>
        ///// Verifies no exception are thrown when called with a null collection.
        ///// </summary>
        //[Fact]
        //public void DictionaryAddNotContainsTest_NullCollectionNullKeyNullValue()
        //{
        //    IDictionary<string, string> col = null;
        //    col.AddNotContains(null, null);
        //}

        //[Fact]
        //public void DictionaryAddNotContainsTest_NullKeyNullValue()
        //{
        //    IDictionary<string, string> col = new Dictionary<string, string>();
        //    col.AddNotContains(null, null);

        //    Assert.Equal(0, col.Count);
        //}

        //[Fact]
        //public void DictionaryAddNotContainsTest_NullValue()
        //{
        //    IDictionary<string, string> col = new Dictionary<string, string>();
        //    col.AddNotContains("Hello", null);

        //    Assert.Equal(1, col.Count);
        //    Assert.Null(col["Hello"]);
        //}

        //[Fact]
        //public void DictionaryAddNotContainsTest_DuplicateKey()
        //{
        //    IDictionary<string, string> col = new Dictionary<string, string>();
        //    col.AddNotContains("Hello", null);
        //    Assert.Equal(1, col.Count);
        //    Assert.Null(col["Hello"]);

        //    col.AddNotContains("Hello", "World");
        //    Assert.Equal(1, col.Count);
        //    Assert.Null(col["Hello"]);
        //}

        //#endregion AddNotContains

        #region AddOrUpdate Tests

        [Fact]
        public void DictionaryAddOrUpdateTest_NullCollection()
        {
            Dictionary<string, string> col = null;
            col.AddOrUpdate(null, null);
        }

        [Fact]
        public void DictionaryAddOrUpdateTest_NullKey()
        {
            var col = new Dictionary<string, string>();
            col.AddOrUpdate(null, null);

            Assert.Empty(col);
        }

        [Fact]
        public void DictionaryAddOrUpdateTest_AddKeyNullValue()
        {
            var col = new Dictionary<string, string>();
            col.AddOrUpdate("Hello", null);

            Assert.Single(col);
            Assert.Null(col["Hello"]);
        }

        [Fact]
        public void DictionaryAddOrUpdateTest_AddAndUpdate()
        {
            var col = new Dictionary<string, string>();
            col.AddOrUpdate("Hello", null);
            Assert.Single(col);

            col.AddOrUpdate("Hello", "World");
            Assert.Single(col);
            Assert.Equal("World", col["Hello"]);
        }

        #endregion AddOrUpdate Tests

        #region TryAdd Tests

        [Fact]
        public void TryAddTest_NullDictionary()
        {
            IDictionary<string, string> col = null;
            bool result = col.TryAdd("Red", "Blue");

            Assert.False(result);
        }

        [Fact]
        public void TryAddTest_AddEntryNullKey()
        {
            IDictionary<string, string> col = null;
            bool result = col.TryAdd(null, "Blue");

            Assert.False(result);
        }

        [Fact]
        public void TryAddTest_AddEntryNullValue()
        {
            var col = new Dictionary<string, string>();
            bool result = col.TryAdd("Red", null);

            Assert.True(result);
            Assert.Single(col);
            Assert.Null(col["Red"]);
        }

        [Fact]
        public void TryAddTest_AddValidEntry()
        {
            var col = new Dictionary<string, string>();
            bool result = col.TryAdd("Red", "Blue");

            Assert.True(result);
            Assert.Single(col);
            Assert.Equal("Blue", col["Red"]);
        }

        [Fact]
        public void TryAddTest_AddDuplicateEntry()
        {
            var col = new Dictionary<string, string> { { "Red", "Blue" } };
            bool result = col.TryAdd("Red", "Yellow");

            Assert.False(result);
            Assert.Single(col);
            Assert.Equal("Blue", col["Red"]);
        }

        #endregion

        #region TryGetValueOrDefault Tests

        [Fact]
        public void DictionaryTryGetValueOrDefaultTest_NullCollectionNullKeyNullValue()
        {
            IDictionary<string, string> col = null;
            string result = col.TryGetValueOrDefault(null, null);

            Assert.Null(result);
        }

        [Fact]
        public void DictionaryTryGetValueOrDefaultTest_NullKeyNullValue()
        {
            IDictionary<string, string> col = new Dictionary<string, string>();
            string result = col.TryGetValueOrDefault(null, null);

            Assert.Null(result);
        }

        [Fact]
        public void DictionaryTryGetValueOrDefaultTest_NullValue()
        {
            IDictionary<string, string> col = new Dictionary<string, string>();
            string result = col.TryGetValueOrDefault("Hello", null);

            Assert.Null(result);
        }

        [Fact]
        public void DictionaryTryGetValueOrDefaultTest_MatchingKey()
        {
            IDictionary<string, string> col = new Dictionary<string, string>
                {
                    { "Hello", "World" },
                    { "Taco", "Bell" },
                };

            string result = col.TryGetValueOrDefault("Hello", null);
            Assert.Equal("World", result);
        }

        [Fact]
        public void DictionaryTryGetValueOrDefaultTest_NonMatchingKey()
        {
            IDictionary<string, string> col = new Dictionary<string, string>
                {
                    { "Hello", "World" },
                    { "Taco", "Bell" },
                };

            string result = col.TryGetValueOrDefault("World", "123fdgn^%");
            Assert.Equal("123fdgn^%", result);
        }

        #endregion TryGetValueOrDefault Tests

        #region ToNameValueCollection Tests

        /// <summary>
        /// Verifies converting a null dictionary to a name value collection
        /// throws an argument null exception.
        /// </summary>
        [Fact]
        public void ToNameValueCollectionTest_NullDictionary()
        {
            IDictionary<string, string> dict = null;
            Assert.Throws<ArgumentNullException>(() => dict.ToNameValueCollection());
        }

        /// <summary>
        /// Verifies converting an empty dictionary to a name value collection
        /// returns a new and empty name value collection.
        /// </summary>
        [Fact]
        public void ToNameValueCollectionTest_EmptyDictionary()
        {
            IDictionary<string, string> dict = new Dictionary<string, string>();
            NameValueCollection result = dict.ToNameValueCollection();

            Assert.NotNull(result);
            Assert.Empty(result);
        }

        /// <summary>
        /// Verifies converting a populated dictionary to a name value
        /// collection converted correctly.
        /// </summary>
        [Fact]
        public void ToNameValueCollectionTest_FilledDictionary()
        {
            IDictionary<string, string> dict = new Dictionary<string, string>
                                                   {
                                                       { "Hello", "World" },
                                                       { "Taco", "Bell" },
                                                       { "Nelson", null }
                                                   };

            NameValueCollection result = dict.ToNameValueCollection();
            Assert.NotNull(result);
            Assert.Equal(3, result.Count);

            Assert.Equal("World", result["Hello"]);
            Assert.Equal("Bell", result["Taco"]);
            Assert.Null(result["Nelson"]);
        }

        #endregion ToNameValueCollection Tests
    }
}