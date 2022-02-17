namespace Oliviann.Tests.Text
{
    #region Usings

    using Oliviann.Text;
    using Xunit;

    #endregion Usings

    [Trait("Category", "CI")]
    public class SearchStringTokenizerTests
    {
        [Theory]
        [InlineData(null, 0)]
        [InlineData("", 0)]
        [InlineData("One", 1)]
        [InlineData("One Two", 2)]
        [InlineData("One Two Three Four", 4)]
        [InlineData(" One Two", 2)]
        [InlineData("One Two ", 2)]
        [InlineData(" One Two  ", 2)]
        public void SearchTokenizerTest_NoParmsNoQuotes(string input, int expectedCount)
        {
            this.PerformTest(input, expectedCount);
        }

        [Theory]
        [InlineData("\"taco\"", 1)]
        [InlineData("\"taco bell\"", 1)]
        [InlineData("one \"taco\"", 2)]
        [InlineData("one \"taco bell\"", 2)]
        [InlineData("one \"taco\" two", 3)]
        [InlineData("one \"taco bell\" two", 3)]
        [InlineData("one \"taco\" two \"wheel barrow\"", 4)]
        [InlineData("one \"taco\" two \"wheel barrow\" three", 5)]
        [InlineData("one two \"taco\"", 3)]
        [InlineData("one two \"taco bell\"", 3)]
        [InlineData("one two \"taco bell\" three four", 5)]
        [InlineData("one\"taco\"three", 3)]
        [InlineData("one\"taco bell\"three", 3)]
        [InlineData("\"taco bell\"\"wheel barrow\"", 2)]
        [InlineData("\"taco bell\" one \"wheel barrow\"", 3)]
        [InlineData("\"taco bell\" one \"wheel barrow\" two", 4)]
        public void SearchTokenizer_NoParmsWithQuotes(string input, int expectedCount)
        {
            this.PerformTest(input, expectedCount);
        }

        [Theory]
        [InlineData("name:valdez", 1)]
        [InlineData("name :valdez", 2)]
        [InlineData("name: valdez", 1)]
        [InlineData("name:valdez juan", 2)]
        [InlineData("name:valdez,juan", 1)]
        [InlineData("one name:valdez", 2)]
        [InlineData("one two name:valdez", 3)]
        [InlineData("  one two name:valdez", 3)]
        [InlineData("one two name:valdez  ", 3)]
        [InlineData("one two name:valdez juan", 4)]
        [InlineData("first:juan last:valdez", 2)]
        [InlineData("last:val-dez juan", 2)]
        public void SearchTokenizer_ParamsNoQuotes(string input, int expectedCount)
        {
            this.PerformTest(input, expectedCount);
        }

        [Theory]
        [InlineData("name:\"valdez juan\"", 1)]
        [InlineData("name:\"valdez\"", 1)]
        [InlineData(" name:\"valdez\"", 1)]
        [InlineData("name:\"valdez\" ", 1)]
        [InlineData("one name:\"valdez\"", 2)]
        [InlineData("one \"taco bell\" name:\"valdez\"", 3)]
        [InlineData("one \"taco bell\" two name:\"valdez\"", 4)]
        public void SearchTokenizer_ParamsAndQuotes(string input, int expectedCount)
        {
            this.PerformTest(input, expectedCount);
        }

        #region Helper Methods

        private void PerformTest(string input, int expectedCount)
        {
            var parser = new SearchStringTokenizer(input);
            parser.Parse();
            int result = parser.Tokens.Count;

            Assert.Equal(expectedCount, result);
        }

        #endregion Helper Methods
    }
}