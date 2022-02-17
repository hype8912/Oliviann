namespace Oliviann.Tests.Extensions
{
    #region Usings

    using System;
    using System.Linq;
    using Xunit;

    #endregion Usings

    [Trait("Category", "CI")]
    public class ArrayExtensionsTests
    {
        #region Fields

        private const string[] NullStringArray = null;

        private static readonly string[] SampleArrayString1 = { "One", "Two", "Three", "Four", "Five" };

        private static readonly string[] SampleArrayString2 = { "One", "Two", null, "four", "Five", "Two", "Four" };

        #endregion Fields

        #region ArraySegement Tests

        [Fact]
        public void AsSegmentTest_NullArray()
        {
            Assert.Throws<ArgumentNullException>(() => NullStringArray.AsSegment());
        }

        [Fact]
        public void AsSegmentTest_StringArray()
        {
            ArraySegment<string> result = SampleArrayString1.AsSegment();
            Assert.Equal(SampleArrayString1, result);
        }

        [Fact]
        public void AsSegmentTest2_NullArray()
        {
            Assert.Throws<ArgumentNullException>(() => NullStringArray.AsSegment(1, 1));
        }

        [Fact]
        public void AsSegmentTest2_LowOffset()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => SampleArrayString1.AsSegment(-1, 1));
        }

        [Fact]
        public void AsSegmentTest2_HighOffset()
        {
            Assert.Throws<ArgumentException>(() => SampleArrayString1.AsSegment(20, 1));
        }

        [Fact]
        public void AsSegmentTest2_LowCount()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => SampleArrayString1.AsSegment(1, -20));
        }

        [Fact]
        public void AsSegmentTest2_ValidSegment()
        {
            ArraySegment<string> result = SampleArrayString2.AsSegment(1, 3);
            Assert.Equal(3, result.Count);

            string[] arrResult = result.ToArray();
            Assert.Equal(3, arrResult.Length);

            for (int i = 0; i < result.Count; i++)
            {
                string val1 = SampleArrayString2[i + 1];
                string val2 = arrResult[i];
                Assert.Equal(val1, val2);
            }
        }

        #endregion

        #region Contains Tests

        [Fact]
        public void ArrayContains_NullSource()
        {
            Assert.Throws<ArgumentNullException>(() => NullStringArray.Contains("Test"));
        }

        [Fact]
        public void ArrayContains_NullValue()
        {
            Assert.DoesNotContain(null, SampleArrayString1);

            Assert.Contains(null, SampleArrayString2);
        }

        [Theory]
        [InlineData(null, false)]
        [InlineData("One", true)]
        [InlineData("one", false)]
        [InlineData(" one", false)]
        [InlineData("one ", false)]
        [InlineData("Two", true)]
        [InlineData("Three", true)]
        [InlineData("Four", true)]
        [InlineData("Five", true)]
        public void ArrayContains_Values(string input, bool expectedResult)
        {
            bool result = SampleArrayString1.Contains(input);
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void ArrayFirstIndexOf_Null()
        {
            int result = NullStringArray.FirstIndexOf("Test");
            Assert.Equal(-1, result);
        }

        [Fact]
        public void ArrayFirstIndexOf_EmptyArray()
        {
            var tempArray = new string[0];
            int result = tempArray.FirstIndexOf("Test");
            Assert.Equal(-1, result);
        }

        [Theory]
        [InlineData("Two", 1)]
        [InlineData("two", -1)]
        [InlineData(null, 2)]
        public void ArrayFirstIndexOf_Values(string input, int expectedResult)
        {
            int result = SampleArrayString2.FirstIndexOf(input);
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void ArrayFirstIndexOf_Compare_Null()
        {
            int result = NullStringArray.FirstIndexOf("Test", StringComparison.Ordinal);
            Assert.Equal(-1, result);
        }

        [Fact]
        public void ArrayFirstIndexOf_Compare_EmptyArray()
        {
            var tempArray = new string[0];
            int result = tempArray.FirstIndexOf("Test", StringComparison.Ordinal);
            Assert.Equal(-1, result);
        }

        [Theory]
        [InlineData("Two", StringComparison.Ordinal, 1)]
        [InlineData("two", StringComparison.OrdinalIgnoreCase, 1)]
        [InlineData(null, StringComparison.OrdinalIgnoreCase, 2)]
        [InlineData("Four", StringComparison.OrdinalIgnoreCase, 3)]
        [InlineData("Four", StringComparison.Ordinal, 6)]
        [InlineData("Six", StringComparison.Ordinal, -1)]
        public void ArrayFirstIndexOf_Compare_Values(string input, StringComparison comparison, int expectedResult)
        {
            int result = SampleArrayString2.FirstIndexOf(input, comparison);
            Assert.Equal(expectedResult, result);
        }

        #endregion Contains Tests

        #region Converts

        #region UShortToString Tests

        [Theory]
        [InlineData(null)]
        [InlineData(new ushort[] { })]
        public void UShortArrayConvertToStringTest_InvalidValues(ushort[] input)
        {
            string result = input.ConvertToString();
            Assert.Empty(result);
        }

        [Theory]
        [InlineData(new ushort[] { 67, 77, 78 }, "CMN")]
        [InlineData(new ushort[] { 68, 69, 76 }, "DEL")]
        public void UShortArrayConvertToStringTest_ValidValues(ushort[] input, string expectedResult)
        {
            string result = input.ConvertToString();
            Assert.Equal(expectedResult, result);
        }

        #endregion UShortToString Tests

        #region ToHexString Tests

        [Fact]
        public void ToHexStringTest_Null()
        {
            byte[] nullArray = null;
            Assert.Throws<ArgumentNullException>(() => nullArray.ToHexString());
        }

        [Fact]
        public void ToHexStringTest_Strings()
        {
            var tempArray = new byte[] { 68, 111, 116, 32, 78, 101, 116, 32, 80, 101, 114, 108, 115 };
            string result = tempArray.ToHexString();

            Assert.Equal("446F74204E6574205065726C73", result);
        }

        [Fact]
        public void ToHexString2Test_Null()
        {
            byte[] nullArray = null;
            Assert.Throws<ArgumentNullException>(() => nullArray.ToHexString2());
        }

        [Fact]
        public void ToHexString2Test_Strings()
        {
            var tempArray = new byte[] { 68, 111, 116, 32, 78, 101, 116, 32, 80, 101, 114, 108, 115 };
            string result = tempArray.ToHexString2();

            Assert.Equal("446F74204E6574205065726C73", result);
        }

        #endregion ToHexString Tests

        #region ValueOrDefault Tests

        [Fact]
        public void ArrayValueOrDefault_NullArray()
        {
            string result = NullStringArray.ValueOrDefault(1);
            Assert.Null(result);
        }

        [Fact]
        public void ArrayValueOrDefault_EmptyLength()
        {
            var tempArray = new string[0];
            string result = tempArray.ValueOrDefault(1, "Test");
            Assert.Equal("Test", result);
        }

        [Fact]
        public void ArrayValueOrDefault_OverIndex()
        {
            string result = SampleArrayString1.ValueOrDefault(100, "Test");
            Assert.Equal("Test", result);
        }

        [Fact]
        public void ArrayValueOrDefault_Strings()
        {
            string result1 = SampleArrayString1.ValueOrDefault(1, "Test");
            Assert.Equal("Two", result1);

            string result2 = SampleArrayString2.ValueOrDefault(2, "Test");
            Assert.Null(result2);
        }

        #endregion ValueOrDefault Tests

        #endregion Converts
    }
}