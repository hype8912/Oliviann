namespace Oliviann.Tests.Extensions
{
    #region Usings

    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Text.RegularExpressions;
    using Oliviann.Tests.TestObjects;
    using Xunit;

    #endregion Usings

    /// <summary>
    /// Represents a collection of Unit Tests for testing the String Extensions
    /// static class.
    /// </summary>
    [Trait("Category", "CI")]
    public class StringExtensionsTests
    {
        #region Fields

        private const string SampleString1 = "If this is your first visit, be sure to check out the FAQ by clicking the link above.-You may have to register before you can post: click the register link above to proceed.-To start viewing messages, select the forum that you want to visit from the selection below.";

        #endregion Fields

        #region Cleaners

        #region Right Tests

        [Theory]
        [InlineData(null, 5, "")]
        [InlineData("Hello World!", -5, "")]
        [InlineData("Hello World!", 5, "orld!")]
        [InlineData("Hello World!", 20, "Hello World!")]
        public void StringLastTest_Strings(string input, int length, string expectedResult)
        {
            string result = input.Right(length);
            Assert.Equal(expectedResult, result);
        }

        #endregion Right Tests

        #region ReplaceAll Tests

        public static IEnumerable<object[]> ReplaceAllTestStrings
            =>
                new[]
                    {
                        new object[] { "Hello World!", null, "Taco", "Hello World!" },
                        new object[] { null, null, null, null },
                        new object[] { "Hello World!", new string[0], "Taco", "Hello World!" },
                        new object[] { "Hello World!", new[] { "ll", "rl" }, "LL", "HeLLo WoLLd!" },
                        new object[] { "Hello World!", new[] { "ll", "rl" }, null, "Heo Wod!" },
                        new object[] { "Hello World!", new[] { "ll", "rl" }, string.Empty, "Heo Wod!" }
                    };

        [Theory]
        [MemberData(nameof(ReplaceAllTestStrings))]
        public void ReplaceAllTest_Strings(string input, string[] oldStrings, string newString, string expectedResult)
        {
            string result = input.ReplaceAll(oldStrings, newString);
            Assert.Equal(expectedResult, result);
        }

        public static IEnumerable<object[]> ReplaceAllTestStrings2
            =>
                new[]
                {
                    new object[] { "Hello World!", null, "Hello World!" },
                    new object[] { null, null, null },
                    new object[] { null, new Dictionary<string, string> { { "ll", "LL" } }, null },
                    new object[] { "Hello World!", new Dictionary<string, string> { { "ll", "LL" } }, "HeLLo World!" },
                    new object[] { "Hello World!", new Dictionary<string, string>(), "Hello World!" },
                    new object[] { "Hello World!", new Dictionary<string, string> { { "ll", "LL" }, { "rl", "RL" } }, "HeLLo WoRLd!" },
                    new object[] { "Hello World!", new Dictionary<string, string> { { "ll", "LL" }, { "rl", "RL" }, { "x1", "AB" } }, "HeLLo WoRLd!" },
                    new object[] { "Hello World!", new Dictionary<string, string> { { "ll", "LL" }, { "rl", "RL" }, { " ", string.Empty } }, "HeLLoWoRLd!" },
                };

        [Theory]
        [MemberData(nameof(ReplaceAllTestStrings2))]
        public void ReplaceAllTest_StringsValues(string input, IDictionary<string, string> strings, string expectedResult)
        {
            string result = input.ReplaceAll(strings);
            Assert.Equal(expectedResult, result);
        }

        #endregion

        #region RemoveFirstChar Tests

        [Theory]
        [InlineData("//ABC12/3abc*&#", '/', "/ABC12/3abc*&#")]
        [InlineData("A/BC123abc*&#", ' ', "A/BC123abc*&#")]
        [InlineData("A/BC123abc*&#", 'C', "A/BC123abc*&#")]
        [InlineData(null, 'C', null)]
        [InlineData("", 'C', "")]
        public void RemoveFirstCharTest_Strings(string input, char value, string expectedResult)
        {
            string result = input.RemoveFirstChar(value);
            Assert.Equal(expectedResult, result);
        }

        #endregion RemoveFirstChar Tests

        #region RemoveLastChar Tests

        [Theory]
        [InlineData("//ABC1#23abc*&#", '#', "//ABC1#23abc*&")]
        [InlineData("A/BC1#23abc*&##", '#', "A/BC1#23abc*&#")]
        [InlineData("A/BC1#23abc*&##", ' ', "A/BC1#23abc*&##")]
        [InlineData("A/BC1#23abc*&##", '2', "A/BC1#23abc*&##")]
        [InlineData(null, '2', null)]
        [InlineData("", ' ', "")]
        public void RemoveLastCharTest_Strings(string input, char value, string expectedResult)
        {
            string result = input.RemoveLastChar(value);
            Assert.Equal(expectedResult, result);
        }

        #endregion RemoveLastChar Tests

        #region RemoveSpaces Test

        /// <summary>
        /// Verifies the spaces are removed correctly from a string.
        /// </summary>
        [Theory]
        [InlineData(null, null)]
        [InlineData("C:\\Windows\\", "C:\\Windows\\")]
        [InlineData("", "")]
        [InlineData("Tacos Burritos", "TacosBurritos")]
        [InlineData("      Tacos    Burritos     ", "TacosBurritos")]
        [InlineData(" T a c o s B u r r 1 t o $ ", "TacosBurr1to$")]
        [InlineData("Tacos\tBurritos", "Tacos\tBurritos")]
        [InlineData("Tacos Burritos\r\nAre Good", "TacosBurritos\r\nAreGood")]
        public void RemoveSpacesTest_Strings(string input, string expectedResult)
        {
            string result = input.RemoveSpaces();
            Assert.Equal(expectedResult, result);
        }

        #endregion

        #region RemoveChar Tests

        /// <summary>
        /// Verifies the correct characters are removed correctly from a string.
        /// </summary>
        [Theory]
        [InlineData(null, ' ', null)]
        [InlineData("C:\\Windows\\", ' ', "C:\\Windows\\")]
        [InlineData("", ' ', "")]
        [InlineData("Tacos Burritos", ' ', "TacosBurritos")]
        [InlineData("      Tacos    Burritos     ", ' ', "TacosBurritos")]
        [InlineData(" T a c o s B u r r 1 t o $ ", ' ', "TacosBurr1to$")]
        [InlineData("Tacos\tBurritos", ' ', "Tacos\tBurritos")]
        [InlineData("Tacos Burritos\r\nAre Good", ' ', "TacosBurritos\r\nAreGood")]
        [InlineData("Tacos Burritos\r\nAre Good", 't', "Tacos Burrios\r\nAre Good")]
        [InlineData("Tacos Burritos\r\nAre Good", 'r', "Tacos Buitos\r\nAe Good")]
        [InlineData("Tacos Burritos\r\nAre Good", 'B', "Tacos urritos\r\nAre Good")]
        [InlineData("Tacos Burritos\tAre Good", '\t', "Tacos BurritosAre Good")]
        public void RemoveCharTest_Strings(string input, char character, string expectedResult)
        {
            string result = input.RemoveChar(character);
            Assert.Equal(expectedResult, result);
        }

        #endregion

        #region Truncate Tests

        [Theory]
        [InlineData(null, 10, null)]
        [InlineData("Taco Bell", -6, "Taco Bell")]
        [InlineData("Taco Bell", 100, "Taco Bell")]
        [InlineData("Taco Bell", 4, "Taco...")]
        [InlineData("Taco", 4, "Taco")]
        public void TruncateStringTest_Strings(string input, int trimLength, string expectedResult)
        {
            string result = input.Truncate(trimLength);
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData(null, 10, null, null)]
        [InlineData("Taco Bell", -6, null, "Taco Bell")]
        [InlineData("Taco Bell", 100, null, "Taco Bell")]
        [InlineData("Taco Bell", 4, null, "Taco...")]
        [InlineData("Taco Bell", 4, "", "Taco")]
        [InlineData("Taco Bell", 4, "_Bell", "Taco_Bell")]
        [InlineData("Taco", 4, null, "Taco")]
        public void TruncateStringTest_StringsWithSuffix(string input, int trimLength, string suffix, string expectedResult)
        {
            string result = input.Truncate(trimLength, suffix);
            Assert.Equal(expectedResult, result);
        }

        #endregion Truncate Tests

        #region AddPathSeparator Tests

        [Theory]
        [InlineData(null, "\\")]
        [InlineData("C:\\Windows\\", "C:\\Windows\\")]
        [InlineData("C:\\Windows", "C:\\Windows\\")]
        [InlineData("", "\\")]
        [InlineData("http://oliviann.com/", "http://oliviann.com/")]
        [InlineData("http://oliviann.com", "http://oliviann.com/")]
        public void AddPathSeparatorTest_Strings(string input, string expectedResult)
        {
            string result = input.AddPathSeparator();
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData(null, "\\")]
        [InlineData("C:\\Windows\\", "C:\\Windows\\")]
        [InlineData("C:\\Windows", "C:\\Windows\\")]
        [InlineData("", "")]
        [InlineData("http://oliviann.com/", "http://oliviann.com/")]
        [InlineData("http://oliviann.com", "http://oliviann.com/")]
        public void AddPathSeparatorIfNotEmpty_Strings(string input, string expectedResult)
        {
            string result = input.AddPathSeparatorIfNotEmpty();
            Assert.Equal(expectedResult, result);
        }

        #endregion AddPathSeparator Tests

        #region UppercaseFirstChar Tests

        [Theory]
        [InlineData(null, "")]
        [InlineData("TgKLkEoJ|y|O", "TgKLkEoJ|y|O")]
        [InlineData("tu`fZ7:`M@)R", "Tu`fZ7:`M@)R")]
        [InlineData("P/:@0Z7[^Ydi", "P/:@0Z7[^Ydi")]
        [InlineData("x&Pkq@--9@*1", "X&Pkq@--9@*1")]
        [InlineData("Q@Yp4DE0o~^k", "Q@Yp4DE0o~^k")]
        [InlineData("D*v8)BgoQZyY", "D*v8)BgoQZyY")]
        [InlineData("e0hkBS1vJLp1", "E0hkBS1vJLp1")]
        [InlineData("ARc/T#J]B1fu", "ARc/T#J]B1fu")]
        [InlineData("MC_7gX9,P&\\C", "MC_7gX9,P&\\C")]
        [InlineData("p6zWWfq}X88s", "P6zWWfq}X88s")]
        [InlineData("rwUnP2i(V3R9", "RwUnP2i(V3R9")]
        [InlineData("6}O`oq9LTYOj", "6}O`oq9LTYOj")]
        [InlineData("", "")]
        public void UppercaseFirstCharTest_Strings(string input, string expectedResult)
        {
            string result = input.UppercaseFirstChar();
            Assert.Equal(expectedResult, result);
        }

        #endregion UppercaseFirstChar Tests

        #region ToTitleCaseTests

        [Theory]
        [InlineData(null, null)]
        [InlineData("", "")]
        [InlineData("this is a lower case string", "This Is A Lower Case String")]
        [InlineData("THIS IS A UPPER CASE STRING", "THIS IS A UPPER CASE STRING")]
        [InlineData("THIS is A mixed CASE string", "THIS Is A Mixed CASE String")]
        [InlineData("This is a String with an IETM acronym.", "This Is A String With An IETM Acronym.")]
        public void ToTitleCaseTest_Strings(string input, string expectedResult)
        {
            string result = input.ToTitleCase();
            Assert.Equal(expectedResult, result);
        }

        #endregion ToTitleCaseTests

        #region ToUpperInvariantSafe Tests

        [Theory]
        [InlineData(null, "")]
        [InlineData("", "")]
        [InlineData("Hello World!", "HELLO WORLD!")]
        public void ToUpperInvariantSafeTest_Strings(string input, string expectedResult)
        {
            string result = input.ToUpperInvariantSafe();
            Assert.Equal(expectedResult, result);
        }

        #endregion ToUpperInvariantSafe Tests

        #region Reverse Tests

        [Theory]
        [InlineData(null, null)]
        [InlineData("", "")]
        [InlineData("Taco Bell is delicious!", "!suoiciled si lleB ocaT")]
        public void ReverseStringTest_Strings(string input, string expectedResult)
        {
            string result = input.Reverse();
            Assert.Equal(expectedResult, result);
        }

        #endregion Reverse Tests

        #region Split Tests

        [Fact]
        public void StringSplitTest_Null()
        {
            Assert.Throws<NullReferenceException>(() => TestValues.NullString.Split(StringSplitOptions.None, ' '));
        }

        [Fact]
        public void StringSplitTest_EmptyString()
        {
            string[] result = string.Empty.Split(StringSplitOptions.None, ' ');
            Assert.Single(result);
        }

        [Fact]
        public void StringSplitTest_False()
        {
            string[] result = "Hello".Split(StringSplitOptions.None, ' ');

            Assert.Single(result);
            Assert.Equal("Hello", result[0]);
        }

        [Fact]
        public void StringSplitTest_True()
        {
            string[] result = "Hello World!".Split(StringSplitOptions.None, ' ');

            Assert.Equal(2, result.Length);
            Assert.Equal("Hello", result[0]);
            Assert.Equal("World!", result[1]);
        }

        #endregion Split Tests

        #region Trim Tests

        /// <summary>
        /// Verifies calling .NET to trim a null string throws a null reference exception.
        /// </summary>
        [Fact]
        public void StringTrimTest_NullString()
        {
            Assert.Throws<NullReferenceException>(() => TestValues.NullString.Trim());
        }

        [Theory]
        [InlineData(null, null)]
        [InlineData("     Hello World", "Hello World")]
        [InlineData("     ", "")]
        [InlineData("", "")]
        [InlineData("Hello World     ", "Hello World")]
        [InlineData("     Hello World     ", "Hello World")]
        public void StringTrimSafeTest_Strings(string input, string expectedResult)
        {
            string result = input.TrimSafe();
            Assert.Equal(expectedResult, result);
        }

        #endregion Trim Tests

        #endregion Cleaners

        #region Validations

        #region Contains Tests

        [Fact]
        public void ContainsTest_Null()
        {
            Assert.Throws<NullReferenceException>(() => TestValues.NullString.Contains(' '));
        }

        [Theory]
        [InlineData('l', true)]
        [InlineData(' ', true)]
        [InlineData('W', true)]
        [InlineData('!', true)]
        [InlineData('w', false)]
        public void ContainsTest_Strings(char value, bool expectedResult)
        {
            bool result = "Hello World!".Contains(value);
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData(null, " ", typeof(NullReferenceException))]
        [InlineData("Hello", null, typeof(ArgumentNullException))]
        public void ContainsCaseTest_Exceptions(string input, string value, Type expectedException)
        {
            Assert.Throws(expectedException, () => input.Contains(value, true));
        }

        [Theory]
        [InlineData("ld", false, true)]
        [InlineData(" ", true, true)]
        [InlineData("w", true, true)]
        [InlineData("!", false, true)]
        [InlineData("w", false, false)]
        public void ContainsCaseTest_Strings(string value, bool ignoreCase, bool expectedResult)
        {
            bool result = "Hello World!".Contains(value, ignoreCase);
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData(null, " ", typeof(NullReferenceException))]
        [InlineData("Hello", null, typeof(ArgumentNullException))]
        public void ContainsComparisonTest_Exceptions(string input, string value, Type expectedException)
        {
            Assert.Throws(expectedException, () => input.Contains(value, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Verifies the test string contains the specified characters and
        /// returns the correct result.
        /// </summary>
        [Theory]
        [InlineData("ld", StringComparison.Ordinal, true)]
        [InlineData(" ", StringComparison.OrdinalIgnoreCase, true)]
        [InlineData("w", StringComparison.OrdinalIgnoreCase, true)]
        [InlineData("!", StringComparison.Ordinal, true)]
        [InlineData("w", StringComparison.Ordinal, false)]
        public void ContainsComparisonTest_Strings(string value, StringComparison comparison, bool expectedResult)
        {
            bool result = "Hello World!".Contains(value, comparison);
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData(null, new[] { 'c', 'd' }, typeof(NullReferenceException))]
        [InlineData("Hello", null, typeof(ArgumentNullException))]
        public void ContainsAnyCharsTest_InvalidInputs(string input, char[] inputValues, Type expectedExceptionType)
        {
            Assert.Throws(expectedExceptionType, () => input.ContainsAny(inputValues));
        }

        [Theory]
        [InlineData(new[] { 'l', 'x' }, true)]
        [InlineData(new[] { 'X', ' ' }, true)]
        [InlineData(new[] { 'W', 'o' }, true)]
        [InlineData(new[] { '!', 'H' }, true)]
        [InlineData(new[] { 'w', 'O', 'h' }, false)]
        public void ContainsAnyCharsTest_Strings(char[] inputs, bool expectedResult)
        {
            bool result = "Hello World!".ContainsAny(inputs);
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData(null, new[] { "c", "d" }, typeof(NullReferenceException))]
        [InlineData("Hello", null, typeof(ArgumentNullException))]
        public void ContainsAnyStringsTest_InvalidInputs(string input, string[] inputValues, Type expectedExceptionType)
        {
            Assert.Throws(expectedExceptionType, () => input.ContainsAny(inputValues));
        }

        [Theory]
        [InlineData(new[] { "ld", "lo" }, true)]
        [InlineData(new[] { " ", "XX" }, true)]
        [InlineData(new[] { "w", "O" }, false)]
        [InlineData(new[] { "!", "X" }, true)]
        [InlineData(new[] { "w", "O", "h" }, false)]
        public void ContainsAnyStringsTest_Strings(string[] inputs, bool expectedResult)
        {
            bool result = "Hello World!".ContainsAny(inputs);
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData(null, new[] { "c", "d" }, typeof(NullReferenceException))]
        [InlineData("Hello", null, typeof(ArgumentNullException))]
        public void ContainsAnyCaseStringsTest_InvalidInputs(string input, string[] inputValues, Type expectedExceptionType)
        {
            Assert.Throws(expectedExceptionType, () => input.ContainsAny(true, inputValues));
        }

        [Theory]
        [InlineData(true, new[] { "ld", "lo" }, true)]
        [InlineData(true, new[] { " ", "XX" }, true)]
        [InlineData(true, new[] { "w", "O" }, true)]
        [InlineData(false, new[] { "!", "X" }, true)]
        [InlineData(false, new[] { "w", "O", "h" }, false)]
        public void ContainsAnyCaseStringsTest_Strings(bool ignoreCase, string[] inputValues, bool expectedResult)
        {
            bool result = "Hello World!".ContainsAny(ignoreCase, inputValues);
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData(null, new[] { "c", "d" }, typeof(NullReferenceException))]
        [InlineData("Hello", null, typeof(ArgumentNullException))]
        public void ContainsAnyCaseStringsIEnumerableTest_InvalidInputs(string input, IEnumerable<string> inputValues, Type expectedExceptionType)
        {
            Assert.Throws(expectedExceptionType, () => input.ContainsAny(true, inputValues));
        }

        [Theory]
        [InlineData(true, new[] { "ld", "lo" }, true)]
        [InlineData(true, new[] { " ", "XX" }, true)]
        [InlineData(true, new[] { "w", "O" }, true)]
        [InlineData(false, new[] { "!", "X" }, true)]
        [InlineData(false, new[] { "w", "O", "h" }, false)]
        public void ContainsAnyCaseStringsIEnumerableTest_Strings(bool ignoreCase, IEnumerable<string> inputValues, bool expectedResult)
        {
            bool result = "Hello World!".ContainsAny(ignoreCase, inputValues);
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData(null, new[] { 'c', 'd' }, typeof(NullReferenceException))]
        [InlineData("Hello", null, typeof(ArgumentNullException))]
        public void ContainsAllCharsTest_InvalidInputs(string input, char[] inputValues, Type expectedExceptionType)
        {
            Assert.Throws(expectedExceptionType, () => input.ContainsAll(inputValues));
        }

        [Theory]
        [InlineData(new[] { 'l', 'x' }, false)]
        [InlineData(new[] { 'X', ' ' }, false)]
        [InlineData(new[] { 'W', 'o' }, true)]
        [InlineData(new[] { '!', 'H' }, true)]
        [InlineData(new[] { 'w', 'O', 'h' }, false)]
        public void ContainsAllCharsTest_Strings(char[] inputValues, bool expectedResult)
        {
            bool result = "Hello World!".ContainsAll(inputValues);
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData(null, new[] { "c", "d" }, typeof(NullReferenceException))]
        [InlineData("Hello", null, typeof(ArgumentNullException))]
        public void ContainsAllStringsTest_InvalidInputs(string input, string[] inputValues, Type expectedExceptionType)
        {
            Assert.Throws(expectedExceptionType, () => input.ContainsAll(inputValues));
        }

        [Theory]
        [InlineData(new[] { "ld", "lo" }, true)]
        [InlineData(new[] { " ", "XX" }, false)]
        [InlineData(new[] { "w", "O" }, false)]
        [InlineData(new[] { "!", "X" }, false)]
        [InlineData(new[] { "w", "O", "h" }, false)]
        public void ContainsAllStringsTest_Strings(string[] inputValues, bool expectedResult)
        {
            bool result = "Hello World!".ContainsAll(inputValues);
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData(true, new[] { "ld", "lo" }, true)]
        [InlineData(true, new[] { " ", "Ld!" }, true)]
        [InlineData(true, new[] { "w", "O" }, true)]
        [InlineData(false, new[] { "!", "X" }, false)]
        [InlineData(false, new[] { "w", "O", "h" }, false)]
        public void ContainsAllCaseStringsTest_Strings(bool ignoreCase, string[] inputValues, bool expectedResult)
        {
            bool result = "Hello World!".ContainsAll(ignoreCase, inputValues);
            Assert.Equal(expectedResult, result);
        }

        #endregion Contains Tests

        #region Equals Tests

        [Theory]
        [InlineData(TestValues.NullString, TestValues.NullString, true)]
        [InlineData(TestValues.NullString, "Taco Bell", false)]
        [InlineData("Taco Bell", TestValues.NullString, false)]
        [InlineData("Taco Bell", "Taco Bell", true)]
        [InlineData("Taco Bell", "taco bell", true)]
        [InlineData("taco bell", "Taco Bell", true)]
        public void EqualsCurrentCultureIgnoreCaseTest_Strings(string value1, string value2, bool expectedResult)
        {
            bool result = value1.EqualsCurrentCultureIgnoreCase(value2);
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData(TestValues.NullString, TestValues.NullString, true)]
        [InlineData(TestValues.NullString, "Taco Bell", false)]
        [InlineData("Taco Bell", TestValues.NullString, false)]
        [InlineData("Taco Bell", "Taco Bell", true)]
        [InlineData("Taco Bell", "taco bell", false)]
        [InlineData("taco bell", "Taco Bell", false)]
        public void EqualsInvariantTest_Strings(string value1, string value2, bool expectedResult)
        {
            bool result = value1.EqualsInvariant(value2);
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData(TestValues.NullString, TestValues.NullString, true)]
        [InlineData(TestValues.NullString, "Taco Bell", false)]
        [InlineData("Taco Bell", TestValues.NullString, false)]
        [InlineData("Taco Bell", "Taco Bell", true)]
        [InlineData("Taco Bell", "taco bell", true)]
        [InlineData("taco bell", "Taco Bell", true)]
        public void EqualsInvariantIgnoreCaseTest_Strings(string value1, string value2, bool expectedResult)
        {
            bool result = value1.EqualsInvariantIgnoreCase(value2);
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData(TestValues.NullString, TestValues.NullString, true)]
        [InlineData(TestValues.NullString, "Taco Bell", false)]
        [InlineData("Taco Bell", TestValues.NullString, false)]
        [InlineData("Taco Bell", "Taco Bell", true)]
        [InlineData("Taco Bell", "taco bell", false)]
        [InlineData("taco bell", "Taco Bell", false)]
        public void EqualsOrdinalTest_Strings(string value1, string value2, bool expectedResult)
        {
            bool result = value1.EqualsOrdinal(value2);
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData(TestValues.NullString, TestValues.NullString, true)]
        [InlineData(TestValues.NullString, "Taco Bell", false)]
        [InlineData("Taco Bell", TestValues.NullString, false)]
        [InlineData("Taco Bell", "Taco Bell", true)]
        [InlineData("Taco Bell", "taco bell", true)]
        [InlineData("taco bell", "Taco Bell", true)]
        public void EqualsOrdinalIgnoreCaseTest_Strings(string value1, string value2, bool expectedResult)
        {
            bool result = value1.EqualsOrdinalIgnoreCase(value2);
            Assert.Equal(expectedResult, result);
        }

        #endregion Equals Tests

        #region EndsWith Tests

        [Theory]
        [InlineData(null, ' ', false)]
        [InlineData("", ' ', false)]
        [InlineData("Hello World!", '!', true)]
        [InlineData("Hello World!", 'o', false)]
        public void StringEndsWithTest_DefaultCaseStrings(string input, char value, bool expectedResult)
        {
            bool result = input.EndsWith(value);
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData("Hello World", 'D', true)]
        [InlineData("Hello WorlD", 'd', true)]
        public void StringEndsWithTest_IgnoreCaseStrings(string input, char value, bool expectedResult)
        {
            bool result = input.EndsWith(value, true);
            Assert.Equal(expectedResult, result);
        }

        #endregion EndsWith Tests

        #region IsAlphaCharacters Tests

        [Theory]
        [InlineData(null, false)]
        [InlineData("", false)]
        [InlineData("TacoBell", true)]
        [InlineData("Taco Bell", false)]
        [InlineData("TacoBe11", false)]
        public void StringIsAlphaCharsTest_Strings(string input, bool expectedResult)
        {
            bool result = input.IsAlphaCharacters();
            Assert.Equal(expectedResult, result);
        }

        #endregion IsAlphaCharacters Tests

        #region IsAplhaWhiteSpaceCharacters Tests

        [Theory]
        [InlineData(null, false)]
        [InlineData("", true)]
        [InlineData("TacoBell", true)]
        [InlineData("HelloWorld!", false)]
        [InlineData("Hello World1", false)]
        public void StringIsAlphaWhiteSpaceCharsTest_Strings(string input, bool expectedResult)
        {
            bool result = input.IsAlphaWhiteSpaceCharacters();
            Assert.Equal(expectedResult, result);
        }

        #endregion IsAplhaWhiteSpaceCharacters Tests

        #region IsDirectory Tests

        [Theory]
        [InlineData(null, typeof(ArgumentNullException))]
        [InlineData("", typeof(ArgumentException))]
        [InlineData(",sadtgngq,e24-0 86-t 24068", typeof(FileNotFoundException))]
        public void StringIsDirectoryTest_Exceptions(string input, Type expectedException)
        {
            Assert.Throws(expectedException, () => input.IsDirectory());
        }

        [Theory]
        [InlineData("C:\\Windows\\regedit.exe", false)]
        [InlineData("C:\\Windows\\", true)]
        [InlineData("C:\\Windows", true)]
        [InlineData("C:\\", true)]
        [InlineData("C:", true)]
        public void StringIsDirectoryTest_Strings(string input, bool expectedResult)
        {
            bool result = input.IsDirectory();
            Assert.Equal(expectedResult, result);
        }

        #endregion IsDirectory Tests

        #region IsLength Tests

        [Theory]
        [InlineData(null, 0, false)]
        [InlineData("", 0, true)]
        [InlineData("Hello", 5, true)]
        [InlineData("Taco bell", 9, true)]
        [InlineData("", 2, false)]
        [InlineData("Hello", -6, false)]
        [InlineData("Taco bell", 8, false)]
        public void StringIsLengthTest_Strings(string input, int length, bool expectedResult)
        {
            bool result = input.IsLength(length);
            Assert.Equal(expectedResult, result);
        }

        #endregion IsLength Tests

        #region IsMatch Tests

        [Fact]
        public void StringIsMatchTest_Null()
        {
            Assert.Throws<ArgumentNullException>(() => TestValues.NullString.IsMatch("[A-Z]"));
        }

        [Theory]
        [InlineData("", false)]
        [InlineData("Hello World!", true)]
        public void StringIsMatchTest_Strings(string input, bool expectedResult)
        {
            bool result = input.IsMatch("[A-Z]");
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData("Hello World!", RegexOptions.RightToLeft, true)]
        public void StringIsMatchTest_StringsWithOptions(string input, RegexOptions options, bool expectedResult)
        {
            bool result = input.IsMatch("[A-Z]", options);
            Assert.Equal(expectedResult, result);
        }

        #endregion IsMatch Tests

        #region IsNullOrEmpty Tests

        [Theory]
        [InlineData(null, true)]
        [InlineData("", true)]
        [InlineData(@"QWERTYUIOP{}|ASDFGHJKL:ZXCVBNM<>?~!@#$%^&*()_+`1234567890-=qwertyuiop[]\asdfghjkl;'zxcvbnm,./", false)]
        public void StringIsNullOrEmptyTest_Strings(string input, bool expectedResult)
        {
            bool result = input.IsNullOrEmpty();
            Assert.Equal(expectedResult, result);
        }

        #endregion IsNullOrEmpty Tests

        #region IsNullOrWhiteSpace Tests

        [Theory]
        [InlineData(null, true)]
        [InlineData("", true)]
        [InlineData(@"QWERTYUIOP{}|ASDFGHJKL:ZXCVBNM<>?~!@#$%^&*()_+`1234567890-=qwertyuiop[]\asdfghjkl;'zxcvbnm,./", false)]
        [InlineData("      ", true)]
        public void StringIsNullOrWhiteSpaceTest_Strings(string input, bool expectedResult)
        {
            bool result = input.IsNullOrWhiteSpace();
            Assert.Equal(expectedResult, result);
        }

        #endregion IsNullOrWhiteSpace Tests

        #region IsValidEmailAddress Tests

        [Theory]
        [InlineData(null, false)]
        [InlineData("niceandsimple@example.com", true)]
        [InlineData("very.common@example.com", true)]
        [InlineData("a.little.lengthy.but.fine@dept.example.com", true)]
        [InlineData("disposable.style.email.with+symbol@example.com", true)]
        [InlineData("user@[IPv6:2001:db8:1ff::a0b:dbd0]", false)] // This is a valid string but we find it as invalid.
        [InlineData("postbox@com", false)] // This is a valid string for a top level email address.
        [InlineData("Abc.example.com", false)]
        [InlineData("", false)]
        [InlineData("A@b@c@example.com", false)]
        [InlineData("just\"not\"right@example.com", false)]
        [InlineData("this is\"not\\allowed@example.com", false)]
        [InlineData("this\\ still\\\"not\\\\allowed@example.com", false)]
        public void StringIsValidEmailAddressTest_Strings(string input, bool expectedResult)
        {
            bool result = input.IsValidEmailAddress();
            Assert.Equal(expectedResult, result);
        }

        #endregion IsValidEmailAddress Tests

        #region IsValidInteger Tests

        [Theory]
        [InlineData(null, 0, false)]
        [InlineData("", 0, false)]
        [InlineData("Hello World!", 0, false)]
        [InlineData("12.6", 0, false)]
        [InlineData("12", 12, true)]
        [InlineData("-5", 0, false)]
        [InlineData("-5", -10, true)]
        [InlineData("-5.63", -10, false)]
        public void StringIsValidIntegerTest_Strings(string input, int minimumValue, bool expectedResult)
        {
            bool result = input.IsValidInteger(minimumValue);
            Assert.Equal(expectedResult, result);
        }

        #endregion IsValidInteger Tests

        #region StartsWith Tests

        [Theory]
        [InlineData(null, 'A', false)]
        [InlineData("", 'A', false)]
        [InlineData("Hello World!", 'H', true)]
        [InlineData("Hello World!", 'o', false)]
        public void StringStartsWithTest_Strings(string input, char value, bool expectedResult)
        {
            bool result = input.StartsWith(value);
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData(null, 'A', false)]
        [InlineData("", 'A', false)]
        [InlineData("Hello World", 'H', true)]
        [InlineData("Hello World", 'h', true)]
        [InlineData("Hello World!", 'o', false)]
        public void StringStartsWithTest_StringsIgnoreCase(string input, char value, bool expectedResult)
        {
            bool result = input.StartsWith(value, true);
            Assert.Equal(expectedResult, result);
        }

        #endregion StartsWith Tests

        #region StartsAndEndsWith Tests

        [Theory]
        [InlineData(null, 'A', false)]
        [InlineData("", 'A', false)]
        [InlineData("Hello WorldH", 'H', true)]
        [InlineData("Hello World!", 'o', false)]
        public void StringStartsAndEndsWithTest_StringsWithChar(string input, char value, bool expectedResult)
        {
            bool result = input.StartsAndEndsWith(value);
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData(null, "AB", false)]
        [InlineData("", "AB", false)]
        [InlineData("Hello World! He", "He", true)]
        [InlineData("Hello World!", "He", false)]
        public void StringStartsAndEndsWithTest_Strings(string input, string value, bool expectedResult)
        {
            bool result = input.StartsAndEndsWith(value);
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData(null, "AB", false)]
        [InlineData("", "AB", false)]
        [InlineData("Hello World! He", "He", true)]
        [InlineData("Hello World!", "He", false)]
        public void StringStartsAndEndsWithTest_StringsWithCulture(string input, string value, bool expectedResult)
        {
            bool result = input.StartsAndEndsWith(value, StringComparison.CurrentCulture);
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData("Hello WorldH", 'H', true)]
        [InlineData("Hello Worldh", 'H', true)]
        [InlineData("hello Worldh", 'H', true)]
        [InlineData("hello WorldH", 'H', true)]
        [InlineData("Hello WorldH", 'h', true)]
        [InlineData("Hello Worldh", 'h', true)]
        [InlineData("hello Worldh", 'h', true)]
        [InlineData("hello WorldH", 'h', true)]
        public void StringStartsAndEndsWithTest_StringsIgnoreCase(string input, char value, bool expectedResult)
        {
            bool result = input.StartsAndEndsWith(value, true);
            Assert.Equal(expectedResult, result);
        }

        #endregion StartsAndEndsWith Tests

        #region ValidateCapitalLetters Tests

        [Theory]
        [InlineData(null, 0, false)]
        [InlineData("", 5, false)]
        [InlineData("Hello World!", -5, false)]
        [InlineData("859-ABsCf-ewqr~hg", 4, false)]
        [InlineData("", 0, true)]
        [InlineData("hello world!", 0, true)]
        [InlineData("859-ABsCf-ewqr~hg", 3, true)]
        [InlineData("Hello World!", 1, true)]
        public void StringValidateCapitalCharsTest_Strings(string input, int count, bool expectedResult)
        {
            bool result = input.ValidateCapitalLetters(count);
            Assert.Equal(expectedResult, result);
        }

        #endregion ValidateCapitalLetters Tests

        #region ValidateDashes Tests

        [Theory]
        [InlineData(null, 0, false)]
        [InlineData("", 2, false)]
        [InlineData("859ABsCfewqr~hg", 3, false)]
        [InlineData("", 0, true)]
        [InlineData("859-ABsCf-ewqr~hg", 2, true)]
        public void StringValidateDashesTest_Strings(string input, int count, bool expectedResult)
        {
            bool result = input.ValidateDashes(count);
            Assert.Equal(expectedResult, result);
        }

        #endregion ValidateDashes Tests

        #region ValidateNumbers Tests

        [Theory]
        [InlineData(null, 5, false)]
        [InlineData("", 1, false)]
        [InlineData("", 5, false)]
        [InlineData("Hello World!", -5, false)]
        [InlineData("859-ABsCf-ewqr~hg", 4, false)]
        [InlineData("", 0, true)]
        [InlineData("Hello World!", 0, true)]
        [InlineData("859-ABsCf-ewqr~hg", 2, true)]
        public void StringValidateNumbersTest_Strings(string input, int count, bool expectedResult)
        {
            bool result = input.ValidateNumbers(count);
            Assert.Equal(expectedResult, result);
        }

        #endregion ValidateNumbers Tests

        #endregion Validations

        #region Html

        [Theory]
        [InlineData(null, null)]
        [InlineData("This is a <Test String>.", "This is a &lt;Test String&gt;.")]
        [InlineData("     ", "     ")]
        [InlineData("", "")]
        [InlineData("<div>I need encoding</div>", "&lt;div&gt;I need encoding&lt;/div&gt;")]
        public void StringEncodeHtmlTest_Strings(string input, string expectedResult)
        {
            string result = input.EncodeHtml();
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData(null, null)]
        [InlineData("This is a &lt;Test String&gt;.", "This is a <Test String>.")]
        [InlineData("     ", "     ")]
        [InlineData("", "")]
        [InlineData("&lt;div&gt;I need encoding&lt;/div&gt;", "<div>I need encoding</div>")]
        public void StringDecodeHtmlTest_Strings(string input, string expectedResult)
        {
            string result = input.DecodeHtml();
            Assert.Equal(expectedResult, result);
        }

        #endregion Html

        #region Converts

        #region ConvertAdminFilePath Tests

        [Theory]
        [InlineData("\\\\ABC-123\\C$\\Windows\\Server", "C:\\Windows\\Server")]
        [InlineData("\\\\ABC-123\\IMIS\\Windows\\Server", "\\\\ABC-123\\IMIS\\Windows\\Server")]
        [InlineData("", "")]
        public void ConvertAdminFilePathTest_Strings(string input, string expectedResult)
        {
            string result = input.ConvertAdminFilePath();
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void ConvertAdminFilePathTest_Null()
        {
            Assert.Throws<NullReferenceException>(() => TestValues.NullString.ConvertAdminFilePath());
        }

        #endregion ConvertAdminFilePath Tests

        #region ToBoolean Tests

        [Theory]
        [InlineData(null, false)]
        [InlineData("FaLsE", false)]
        [InlineData("nO", false)]
        [InlineData("YeS", true)]
        [InlineData("tRuE", true)]
        [InlineData("", false)]
        [InlineData(@"nKFn^%O)32454263", false)]
        [InlineData("                  ", false)]
        [InlineData("NO", false)]
        [InlineData("yes", true)]
        [InlineData("YES", true)]
        public void ToBooleanTest_Strings(string input, bool expectedResult)
        {
            bool result = input.ToBoolean();
            Assert.Equal(expectedResult, result);
        }

        #endregion ToBoolean Tests

        #region ToBooleanMatchFalse Tests

        [Theory]
        [InlineData(null, true)]
        [InlineData("FaLsE", false)]
        [InlineData("nO", false)]
        [InlineData("YeS", true)]
        [InlineData("tRuE", true)]
        [InlineData("", true)]
        [InlineData(@"nKFn^%O)32454263", true)]
        [InlineData("                  ", true)]
        [InlineData("NO", false)]
        [InlineData("yes", true)]
        [InlineData("YES", true)]
        public void ToBooleanMatchFalseTest_Strings(string input, bool expectedResult)
        {
            bool result = input.ToBooleanMatchFalse();
            Assert.Equal(expectedResult, result);
        }

        #endregion ToBooleanMatchFalse Tests

        #region ToEnum Tests

        [Theory]
        [InlineData(null, typeof(ArgumentNullException))]
        [InlineData("", typeof(ArgumentException))]
        [InlineData("Ninja", typeof(ArgumentException))]
        public void ToEnumTest_Exceptions(string input, Type expectedException)
        {
            Assert.Throws(expectedException, () => input.ToEnum<DataOperation>());
        }

        [Theory]
        [InlineData("Read", false, DataOperation.Read)]
        [InlineData("rEad", true, DataOperation.Read)]
        public void ToEnumTest_Strings(string input, bool ignoreCase, DataOperation expectedResult)
        {
            DataOperation result = input.ToEnum<DataOperation>(ignoreCase);
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData(null, false, DataOperation.Delete)]
        [InlineData("", false, DataOperation.Delete)]
        [InlineData("Hello", false, DataOperation.Delete)]
        [InlineData("read", false, DataOperation.Delete)]
        [InlineData("rEad", true, DataOperation.Read)]
        [InlineData("Read", false, DataOperation.Read)]
        public void ToEnumWithDefaultTest_Strings(string input, bool ignoreCase, DataOperation expectedResult)
        {
            DataOperation result = input.ToEnum(DataOperation.Delete, ignoreCase);
            Assert.Equal(expectedResult, result);
        }

        #endregion ToEnum Tests

        #region ToTimeSpan Tests

        [Theory]
        [InlineData(null, typeof(FormatException))]
        [InlineData("", typeof(FormatException))]
        [InlineData("akgn4565486ytobldf900", typeof(FormatException))]
        [InlineData("101216", typeof(FormatException))]
        public void ToTimeSpanTest_Exceptions(string input, Type expectedException)
        {
            Assert.Throws(expectedException, () => input.ToTimeSpan());
        }

        [Theory]
        [InlineData("02,10,45", 2, 10, 45)]
        [InlineData("02,66,99", 2, 66, 99)]
        public void ToTimeSpanTest_Strings(string input, int hours, int minutes, int seconds)
        {
            TimeSpan result = input.ToTimeSpan();
            var expectedResult = new TimeSpan(hours, minutes, seconds);

            Assert.Equal(expectedResult, result);
        }

        #endregion ToTimeSpan Tests

        #region ToUriString Tests

        [Theory]
        [InlineData(null, typeof(ArgumentNullException))]
        [InlineData("", typeof(UriFormatException))]
        [InlineData("Taco Bell", typeof(UriFormatException))]
        public void ToUriStringTest_Exceptions(string input, Type expectedException)
        {
            Assert.Throws(expectedException, input.ToUriString);
        }

        [Theory]
        [InlineData("C:\\Windows\\regedit.exe", "file:///C:/Windows/regedit.exe")]
        [InlineData("http://www.oliviann.com/merchandise.html", "http://www.oliviann.com/merchandise.html")]
        public void ToUriStringTest_Strings(string input, string expectedResult)
        {
            string result = input.ToUriString();
            Assert.Equal(expectedResult, result);
        }

        #endregion ToUriString Tests

        #region ValueOrDefault Tests

        [Theory]
        [InlineData(null, null)]
        [InlineData("", null)]
        [InlineData("Hello", "Hello")]
        public void ValueOrDefaultTest_Strings(string input, string expectedResult)
        {
            string result = input.ValueOrDefault();
            Assert.Equal(expectedResult, result);
        }

        #endregion ValueOrDefault Tests

        #endregion Converts

        #region Format Tests

        [Fact]
        public void StringFormatWithTest_NullString()
        {
            Assert.Throws<ArgumentNullException>(() => TestValues.NullString.FormatWith("Hello"));
        }

        [Fact]
        public void StringFormatWithTest_NullStringWithCultureInfo()
        {
            Assert.Throws<ArgumentNullException>(() => TestValues.NullString.FormatWith(CultureInfo.CurrentCulture, "Hello"));
        }

        [Fact]
        public void StringFormatWithTest_BadFormat()
        {
            Assert.Throws<FormatException>(() => "{0} {1}!".FormatWith(CultureInfo.CurrentCulture, "Hello"));
        }

        [Fact]
        public void StringFormatWithTest_NullArgs()
        {
            object[] temp = null;
            Assert.Throws<ArgumentNullException>(() => "String: {0}".FormatWith(temp));
        }

        [Fact]
        public void StringFormatWithTest_NullArgsWithCultureInfo()
        {
            object[] temp = null;
            Assert.Throws<ArgumentNullException>(() => "String: {0}".FormatWith(CultureInfo.CurrentCulture, temp));
        }

        [Fact]
        public void StringFormatWithTest_NullProvider()
        {
            IFormatProvider provider = null;
            string result = "String: {0}".FormatWith(provider, "Hello");

            Assert.Equal("String: Hello", result);
        }

        [Fact]
        public void StringFormatWithTest_Strings()
        {
            string result0 = "{0} {1}!".FormatWith("Hello", "World");
            Assert.Equal("Hello World!", result0);

            string result1 = "{0}!".FormatWith("Hello");
            Assert.Equal("Hello!", result1);

            string result2 = "{0}!".FormatWith("Hello", "World");
            Assert.Equal("Hello!", result2);

            // Mixed variables.
            string result3 = "{{ \"value\": {0} , \"type\"=\"{1}\" }}".FormatWith("Bell", 'S');
            Assert.Equal("{ \"value\": Bell , \"type\"=\"S\" }", result3);
        }

        [Fact]
        public void StringFormatWithTest_StringsWithCultureInfo()
        {
            string result0 = "{0} {1}!".FormatWith(CultureInfo.CurrentCulture, "Hello", "World");
            Assert.Equal("Hello World!", result0);

            string result1 = "{0}!".FormatWith(CultureInfo.CurrentCulture, "Hello");
            Assert.Equal("Hello!", result1);

            string result3 = "{0}!".FormatWith(CultureInfo.CurrentCulture, "Hello", "World");
            Assert.Equal("Hello!", result3);
        }

        [Theory]
        [InlineData(null, null, null)]
        [InlineData(null, "11", "1111")]
        [InlineData("Text", null, "Text")]
        [InlineData("Text", "11", "11Text11")]
        [InlineData("Text", "<p>", "<p>Text<p>")]
        [InlineData("Text", "\"", "\"Text\"")]
        public void StringWrapWithTest_Strings(string input, string appendText, string expectedResult)
        {
            string result = input.WrapWith(appendText);
            Assert.Equal(expectedResult, result);
        }

        #endregion Format Tests

        #region Join Tests

        [Fact]
        public void StringJoinObjectsTest_NullText()
        {
            string result = TestValues.NullString.Join(new object[2]);
            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public void StringJoinObjectsTest_NullValue()
        {
            object[] values = null;
            Assert.Throws<ArgumentNullException>(() => string.Empty.Join(values));
        }

        [Fact]
        public void StringJoinObjectsTest_Strings()
        {
            string result = " ".Join(new object[] { "Hello", "World" });
            Assert.Equal("Hello World", result);
        }

        [Fact]
        public void StringJoinStringsTest_NullText()
        {
            string result = TestValues.NullString.Join(new string[2]);
            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public void StringJoinStringsTest_NullValue()
        {
            string[] values = null;
            Assert.Throws<ArgumentNullException>(() => string.Empty.Join(values));
        }

        [Fact]
        public void StringJoinStringsTest_Strings()
        {
            string result = " ".Join(new[] { "Hello", "World" });
            Assert.Equal("Hello World", result);
        }

        [Fact]
        public void StringJoinCollectionTest_NullText()
        {
            IEnumerable<string> values = new List<string> { "Hello", "World" };
            string result = values.Join(null);

            Assert.Equal("HelloWorld", result);
        }

        [Fact]
        public void StringJoinCollectionTest_NullValue()
        {
            IEnumerable<string> values = null;
            Assert.Throws<ArgumentNullException>(() => values.Join(string.Empty));
        }

        [Fact]
        public void StringJoinCollectionTest_Strings()
        {
            IEnumerable<string> values = new List<string> { "Hello", "World" };
            string result = values.Join(" ");

            Assert.Equal("Hello World", result);
        }

        #endregion Join Tests

        #region SubstringSafe Tests

        [Theory]
        [InlineData(null, 1, 10, "")]
        [InlineData("", 9, 10, "")]
        [InlineData("", 1, 10, "")]
        [InlineData("Abc123", 1, 10, "bc123")]
        [InlineData("Abc123", -1, 5, "Abc12")]
        [InlineData(SampleString1, 5, 8, "is is yo")]
        [InlineData("Abc123", 1, -10, "")]
        [InlineData("Abc123", 0, 999999999, "Abc123")]
        [InlineData("Abc123", 3, 999999999, "123")]
        public void StringSubstringSafeTest_Values(string text, int startIndex, int length, string expectedResult)
        {
            string result = text.SubstringSafe(startIndex, length);
            Assert.Equal(expectedResult, result);
        }

        #endregion SubstringSafe Tests

        #region IsDigitsOnly Tests

        [Theory]
        [InlineData(null, false)]
        [InlineData("", false)]
        [InlineData("             ", false)]
        [InlineData("Abc123%*()[}", false)]
        [InlineData("5463.021", false)]
        [InlineData("89456", true)]
        [InlineData("Abc\r\nH38\t", false)]
        public void IsDigitsOnlyTest_Values(string input, bool expectedResult)
        {
            bool result = input.IsDigitsOnly();
            Assert.Equal(expectedResult, result);
        }

        #endregion
    }
}