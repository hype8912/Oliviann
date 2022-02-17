namespace Oliviann.Tests.Extensions
{
    #region Usings

    using System;
    using Xunit;

    #endregion Usings

    [Trait("Category", "CI")]
    public class CharExtensionsTests
    {
        [Theory]
        [InlineData('A', 'A', true)]
        [InlineData('A', 'a', true)]
        [InlineData('a', 'A', true)]
        [InlineData('a', 'a', true)]
        [InlineData('-', '-', true)]
        [InlineData('-', 'B', false)]
        public void CharEqualsInvariantIgnoreCaseTest_Values(char inputA, char inputB, bool expectedResult)
        {
            bool result = inputA.EqualsInvariantIgnoreCase(inputB);
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void CharJoinTest_Null()
        {
            Assert.Throws<NullReferenceException>(() => ' '.Join(null));
        }

        [Fact]
        public void CharJoinTest_Strings()
        {
            string result = ' '.Join("Hello", "World!");
            Assert.Equal("Hello World!", result);
        }
    }
}