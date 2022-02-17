namespace Oliviann.Tests.Collections.Generic
{
    #region Usings

    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Linq;
    using Oliviann.Collections.Generic;
    using Xunit;

    #endregion Usings

    [Trait("Category", "CI")]
    public class OrderedDictionary_TKey_TVal_Tests
    {
        #region ctor Tests

        [Fact]
        public void ctor_NegativeCapacity()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new OrderedDictionary<string, string>(-26));
        }

        [Fact]
        public void ctor_Comparer()
        {
            var od = new OrderedDictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        }

        #endregion ctor Tests

        #region Add Tests

        [Fact]
        public void AddTest_NullKeyValue()
        {
            var od = new OrderedDictionary<string, string>();
            Assert.Empty(od);
            Assert.Equal(-1, od.IndexOfKey("foo"));

            Assert.False(od.TryGetValue("foo", out _));

            Assert.Throws<ArgumentNullException>(() => od.Add(null, null));
        }

        [Fact]
        public void AddTest_Values()
        {
            var od = new OrderedDictionary<string, string>();
            Assert.Empty(od);
            Assert.Equal(-1, od.IndexOfKey("foo"));
            Assert.False(od.TryGetValue("foo", out string outValue));

            od.Add("foo", "bar");
            Assert.Single(od);
            Assert.Equal(0, od.IndexOfKey("foo"));
            Assert.Equal("bar", od[0]);
            Assert.Equal("bar", od["foo"]);

            Assert.True(od.TryGetValue("foo", out outValue));
            Assert.Equal("bar", outValue);
            Assert.Equal("foo", od.GetItem(0).Key);
            Assert.Equal("bar", od.GetItem(0).Value);
        }

        [Fact]
        public void ICollectionTAddTest_Values()
        {
            ICollection<KeyValuePair<string, string>> od = new OrderedDictionary<string, string>();
            Assert.Equal(0, od.Count);

            od.Add(new KeyValuePair<string, string>("foo", "bar"));
            Assert.Equal(1, od.Count);
        }

        [Fact]
        public void IDictionaryTAddTest_Values()
        {
            IDictionary<string, string> od = new OrderedDictionary<string, string>();
            Assert.Equal(0, od.Count);
            Assert.False(od.TryGetValue("foo", out string outValue));

            od.Add("foo", "bar");
            Assert.Equal(1, od.Count);
            Assert.Equal("bar", od["foo"]);

            Assert.True(od.TryGetValue("foo", out outValue));
            Assert.Equal("bar", outValue);
        }

        [Fact]
        public void IDictionaryAddTest_Values()
        {
            IDictionary od = new OrderedDictionary<string, string>();
            Assert.Equal(0, od.Count);

            od.Add("foo", "bar");
            Assert.Equal(1, od.Count);
        }

        [Fact]
        public void IDictionaryAddTest_NullKey()
        {
            IDictionary od = new OrderedDictionary<string, string>();
            Assert.Equal(0, od.Count);

            Assert.Throws<ArgumentNullException>(() => od.Add(null, "bar"));
        }

        [Fact]
        public void IDictionaryAddTest_NullValue()
        {
            IDictionary od = new OrderedDictionary<string, string>();
            Assert.Equal(0, od.Count);

            od.Add("foo", null);
            Assert.Equal(1, od.Count);
        }

        [Fact]
        public void IDictionaryAddTest_NullStructType()
        {
            IDictionary od = new OrderedDictionary<string, int>();
            Assert.Equal(0, od.Count);

            Assert.Throws<ArgumentNullException>(() => od.Add("foo", null));
        }

        [Fact]
        public void IDictionaryAddTest_IncorrectKeyType()
        {
            IDictionary od = new OrderedDictionary<string, string>();
            Assert.Equal(0, od.Count);

            Assert.Throws<ArgumentException>(() => od.Add(6, "bar"));
        }

        [Fact]
        public void IDictionaryAddTest_IncorrectValueType()
        {
            IDictionary od = new OrderedDictionary<string, string>();
            Assert.Equal(0, od.Count);

            Assert.Throws<ArgumentException>(() => od.Add("foo", 6));
        }

        #endregion Add Tests

        #region Remove Tests

        [Fact]
        public void RemoveTest_NullKey()
        {
            var od = new OrderedDictionary<string, string>();
            Assert.Throws<ArgumentNullException>(() => od.Remove(null));
        }

        [Fact]
        public void RemoveTest_InvalidKey()
        {
            var od = new OrderedDictionary<string, string>();
            Assert.False(od.Remove("ABC"));
        }

        [Fact]
        public void RemoveTest_Values()
        {
            var od = new OrderedDictionary<string, string>();

            od.Add("foo", "bar");
            Assert.Single(od);

            od.Remove("foo");
            Assert.Empty(od);
        }

        [Fact]
        public void ICollectionTRemoveTest_Values()
        {
            ICollection<KeyValuePair<string, string>> od = new OrderedDictionary<string, string>();

            od.Add(new KeyValuePair<string, string>("foo", "bar"));
            Assert.Equal(1, od.Count);

            od.Remove(new KeyValuePair<string, string>("foo", null));
            Assert.Equal(0, od.Count);
        }

        [Fact]
        public void IDictionaryRemoveTest_Values()
        {
            IDictionary od = new OrderedDictionary<string, string>();

            od.Add("foo", "bar");
            Assert.Equal(1, od.Count);

            od.Remove("foo");
            Assert.Equal(0, od.Count);
        }

        [Fact]
        public void RemoveAtTest_LowIndex()
        {
            var od = new OrderedDictionary<string, string>();
            od.Add("ABC", "abc");
            Assert.Throws<ArgumentOutOfRangeException>(() => od.RemoveAt(-26));
        }

        [Fact]
        public void RemoveAtTest_HighIndex()
        {
            var od = new OrderedDictionary<string, string>();
            od.Add("ABC", "abc");
            Assert.Throws<ArgumentOutOfRangeException>(() => od.RemoveAt(100));
        }

        [Fact]
        public void RemoveAtTest_Values()
        {
            var od = new OrderedDictionary<string, string>();

            od.Add("foo", "bar");
            Assert.Single(od);

            od.RemoveAt(0);
            Assert.Empty(od);
        }

        [Fact]
        public void ClearTest_Values()
        {
            var od = this.GetAlphabetDictionary();
            Assert.Equal(26, od.Count);

            od.Clear();
            Assert.Empty(od);
        }

        #endregion Remove Tests

        #region Enumerable Tests

        [Fact]
        public void EnumeratorTest()
        {
            var alphabetDict = this.GetAlphabetDictionary();
            List<string> keys = alphabetDict.Keys.ToList();
            Assert.Equal(26, keys.Count);

            int i = 0;
            foreach (KeyValuePair<string, string> pair in alphabetDict)
            {
                Assert.Equal(pair.Value, alphabetDict[pair.Key]);
                i += 1;
            }
        }

        [Fact]
        public void IOrderedDictionaryEnumeratorTest()
        {
            var alphabetDict = this.GetAlphabetDictionary();
            IOrderedDictionary od = alphabetDict;
            List<string> keys = alphabetDict.Keys.ToList();
            Assert.Equal(26, keys.Count);

            int i = 0;
            foreach (KeyValuePair<string, string> pair in od)
            {
                Assert.Equal(pair.Value, (string)od[pair.Key]);
                i += 1;
            }
        }

        [Fact]
        public void IDictionaryEnumeratorTest()
        {
            var alphabetDict = this.GetAlphabetDictionary();
            IDictionary od = alphabetDict;
            List<string> keys = alphabetDict.Keys.ToList();
            Assert.Equal(26, keys.Count);

            int i = 0;
            foreach (KeyValuePair<string, string> pair in od)
            {
                Assert.Equal(pair.Value, (string)od[pair.Key]);
                i += 1;
            }
        }

        [Fact]
        public void IEnumerableEnumeratorTest()
        {
            var alphabetDict = this.GetAlphabetDictionary();
            IEnumerable od = alphabetDict;
            List<string> keys = alphabetDict.Keys.ToList();
            Assert.Equal(26, keys.Count);

            int i = 0;
            foreach (KeyValuePair<string, string> pair in od)
            {
                i += 1;
            }

            Assert.Equal(26, i);
        }

        #endregion Enumerable Tests

        #region Get Tests

        [Theory]
        [InlineData(-10)]
        [InlineData(100)]
        public void GetItemTest_IndexExceptions(int indexValue)
        {
            var od = this.GetAlphabetDictionary();
            Assert.Throws<IndexOutOfRangeException>(() => od.GetItem(indexValue));
        }

        [Fact]
        public void TryGetValueTest()
        {
            var alphabetDict = this.GetAlphabetDictionary();
            Assert.False(alphabetDict.TryGetValue("abc", out string result));
            Assert.Null(result);
            Assert.True(alphabetDict.TryGetValue("z", out result));
            Assert.Equal("Z", result);
        }

        [Fact]
        public void InvalidIndexTest()
        {
            var alphabetDict = this.GetAlphabetDictionary();
            Assert.Throws<ArgumentOutOfRangeException>(() => alphabetDict[100]);
        }

        [Fact]
        public void MissingKeyTest()
        {
            var alphabetDict = this.GetAlphabetDictionary();
            Assert.Throws<KeyNotFoundException>(() => alphabetDict["abc"]);
        }

        #endregion Get Tests

        #region Update Tests

        [Fact]
        public void UpdateExistingValueTest_LowIndex()
        {
            var alphabetDict = this.GetAlphabetDictionary();
            Assert.True(alphabetDict.ContainsKey("c"));
            Assert.Equal(2, alphabetDict.IndexOfKey("c"));
            Assert.Equal("C", alphabetDict[2]);

            Assert.Throws<ArgumentOutOfRangeException>(() => alphabetDict[-26] = "CCC");
        }

        [Fact]
        public void UpdateExistingValueTest_HighIndex()
        {
            var alphabetDict = this.GetAlphabetDictionary();
            Assert.True(alphabetDict.ContainsKey("c"));
            Assert.Equal(2, alphabetDict.IndexOfKey("c"));
            Assert.Equal("C", alphabetDict[2]);

            Assert.Throws<ArgumentOutOfRangeException>(() => alphabetDict[100] = "CCC");
        }

        [Fact]
        public void UpdateExistingValueTest_ByIndex()
        {
            var alphabetDict = this.GetAlphabetDictionary();
            Assert.True(alphabetDict.ContainsKey("c"));
            Assert.Equal(2, alphabetDict.IndexOfKey("c"));
            Assert.Equal("C", alphabetDict[2]);

            alphabetDict[2] = "CCC";
            Assert.True(alphabetDict.ContainsKey("c"));
            Assert.Equal(2, alphabetDict.IndexOfKey("c"));
            Assert.Equal("CCC", alphabetDict[2]);
        }

        [Fact]
        public void IOrderedDictionaryUpdateExistingValueTest_ByIndex()
        {
            IOrderedDictionary alphabetDict = this.GetAlphabetDictionary();
            Assert.Equal("C", (string)alphabetDict[2]);

            alphabetDict[2] = "CCC";
            Assert.Equal("CCC", (string)alphabetDict[2]);
        }

        [Fact]
        public void UpdateExistingValueTest_ByKey()
        {
            var alphabetDict = this.GetAlphabetDictionary();
            Assert.True(alphabetDict.ContainsKey("c"));
            Assert.Equal(2, alphabetDict.IndexOfKey("c"));
            Assert.Equal("C", alphabetDict[2]);

            alphabetDict["c"] = "CCC";
            Assert.True(alphabetDict.ContainsKey("c"));
            Assert.Equal(2, alphabetDict.IndexOfKey("c"));
            Assert.Equal("CCC", alphabetDict[2]);
        }

        [Fact]
        public void IOrderedDictionaryUpdateExistingValueTest_ByKey()
        {
            IOrderedDictionary alphabetDict = this.GetAlphabetDictionary();
            Assert.Equal("C", (string)alphabetDict["c"]);

            alphabetDict["c"] = "CCC";
            Assert.Equal("CCC", (string)alphabetDict[2]);
        }

        [Fact]
        public void UpdateExistingValueTest_MissingKey()
        {
            var alphabetDict = this.GetAlphabetDictionary();
            Assert.True(alphabetDict.ContainsKey("c"));
            Assert.Equal(2, alphabetDict.IndexOfKey("c"));
            Assert.Equal("C", alphabetDict[2]);

            alphabetDict["XyZZy"] = "CCC";
            Assert.True(alphabetDict.ContainsKey("XyZZy"));
            Assert.Equal(27, alphabetDict.Count);
        }

        #endregion Update Tests

        #region Insert Tests

        [Theory]
        [InlineData(-20)]
        [InlineData(100)]
        public void InsertTest_IndexExceptions(int inputIndex)
        {
            var od = new OrderedDictionary<string, string>();
            Assert.Throws<ArgumentOutOfRangeException>(() => od.Insert(inputIndex, "ABC", "abc"));
        }

        [Fact]
        public void InsertTest_Values()
        {
            var alphabetDict = this.GetAlphabetDictionary();
            Assert.True(alphabetDict.ContainsKey("c"));
            Assert.Equal(2, alphabetDict.IndexOfKey("c"));
            Assert.Equal("C", alphabetDict[2]);
            Assert.Equal(26, alphabetDict.Count);
            Assert.DoesNotContain(alphabetDict, p => p.Value == "ABC");

            alphabetDict.Insert(2, "abc", "ABC");
            Assert.True(alphabetDict.ContainsKey("c"));
            Assert.Equal(2, alphabetDict.IndexOfKey("abc"));
            Assert.Equal("ABC", alphabetDict[2]);
            Assert.Equal(27, alphabetDict.Count);
            Assert.Contains(alphabetDict, p => p.Value == "ABC");
        }

        [Fact]
        public void IOrderedDictionaryInsertTest_Values()
        {
            IOrderedDictionary alphabetDict = this.GetAlphabetDictionary();
            Assert.Equal("C", (string)alphabetDict[2]);
            Assert.Equal(26, alphabetDict.Count);

            alphabetDict.Insert(2, "abc", "ABC");
            Assert.Equal("ABC", (string)alphabetDict[2]);
            Assert.Equal(27, alphabetDict.Count);
        }

        #endregion Insert Tests

        #region Other Tests

        [Fact]
        public void OrderIsPreservedTest()
        {
            var alphabetDict = this.GetAlphabetDictionary();
            var alphabetList = this.GetAlphabetList();
            Assert.Equal(26, alphabetDict.Count);
            Assert.Equal(26, alphabetList.Count);

            List<string> keys = alphabetDict.Keys.ToList();
            List<string> values = alphabetDict.Values.ToList();

            for (int i = 0; i < 26; i += 1)
            {
                KeyValuePair<string, string> dictItem = alphabetDict.GetItem(i);
                KeyValuePair<string, string> listItem = alphabetList[i];
                string key = keys[i];
                string value = values[i];

                Assert.Equal(dictItem, listItem);
                Assert.Equal(listItem.Key, key);
                Assert.Equal(listItem.Value, value);
            }
        }

        [Fact]
        public void ValueComparerTest()
        {
            var alphabetDict = this.GetAlphabetDictionary();
            Assert.DoesNotContain(alphabetDict, p => p.Value == "a");
            Assert.Contains(alphabetDict, p => string.Equals(p.Value, "a", StringComparison.OrdinalIgnoreCase));
        }

        [Fact]
        public void IDictionaryIsTest()
        {
            IDictionary od = new OrderedDictionary<string, string>();
            Assert.False(od.IsSynchronized);
            Assert.False(od.IsFixedSize);
            Assert.False(od.IsReadOnly);
        }

        [Fact]
        public void SyncRootTest()
        {
            ICollection od = new OrderedDictionary<string, string>();
            var result = od.SyncRoot;
            Assert.NotNull(result);
        }

        [Fact]
        public void DictionaryKeysTest()
        {
            IDictionary od = this.GetAlphabetDictionary();
            ICollection result = od.Keys;
            Assert.Equal(26, result.Count);
        }

        [Fact]
        public void DictionaryValuesTest()
        {
            IDictionary od = this.GetAlphabetDictionary();
            ICollection result = od.Values;
            Assert.Equal(26, result.Count);
        }

        [Fact]
        public void ICollectionTContainsTest()
        {
            ICollection<KeyValuePair<string, string>> od = this.GetAlphabetDictionary();
            bool resultTrue = od.Contains(new KeyValuePair<string, string>("c", "C"));
            Assert.True(resultTrue);

            bool resultFalse = od.Contains(new KeyValuePair<string, string>("c", null));
            Assert.False(resultFalse);
        }

        [Fact]
        public void IDictionaryContainsTest()
        {
            IDictionary od = this.GetAlphabetDictionary();

            Assert.True(od.Contains("c"));
            Assert.False(od.Contains("C"));
        }

        [Fact]
        public void IndexOfTest_NullKey()
        {
            var od = this.GetAlphabetDictionary();
            Assert.Throws<ArgumentNullException>(() => od.IndexOfKey(null));
        }

        [Fact]
        public void IndexOfTest_WithComparer()
        {
            var od = this.GetAlphabetDictionary(StringComparer.OrdinalIgnoreCase);
            int resultUpper = od.IndexOfKey("C");
            int resultLower = od.IndexOfKey("c");

            Assert.Equal(resultUpper, resultLower);
        }

        [Fact]
        public void ICollectionCopyToTest()
        {
            ICollection od = this.GetAlphabetDictionary();
            var resultAr = new object[26];
            od.CopyTo(resultAr, 0);

            Assert.Equal(26, resultAr.Length);
        }

        [Fact]
        public void ICollectionTCopyToTest()
        {
            ICollection<KeyValuePair<string, string>> od = this.GetAlphabetDictionary();
            var resultAr = new KeyValuePair<string, string>[26];
            od.CopyTo(resultAr, 0);

            Assert.Equal(26, resultAr.Length);
        }

        #endregion Other Tests

        #region Helpers

        private OrderedDictionary<string, string> GetAlphabetDictionary(IEqualityComparer<string> comparer = null)
        {
            OrderedDictionary<string, string> alphabet = comparer == null ? new OrderedDictionary<string, string>() : new OrderedDictionary<string, string>(comparer);
            for (int a = Convert.ToInt32('a'); a <= Convert.ToInt32('z'); a += 1)
            {
                char c = Convert.ToChar(a);
                alphabet.Add(c.ToString(), c.ToString().ToUpper());
            }

            Assert.Equal(26, alphabet.Count);
            return alphabet;
        }

        private List<KeyValuePair<string, string>> GetAlphabetList()
        {
            var alphabet = new List<KeyValuePair<string, string>>();
            for (int a = Convert.ToInt32('a'); a <= Convert.ToInt32('z'); a += 1)
            {
                char c = Convert.ToChar(a);
                alphabet.Add(new KeyValuePair<string, string>(c.ToString(), c.ToString().ToUpper()));
            }

            Assert.Equal(26, alphabet.Count);
            return alphabet;
        }

        #endregion Helpers
    }
}