namespace Oliviann.Tests.Extensions
{
    #region Usings

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Xunit;

    #endregion Usings

    [Trait("Category", "CI")]
    public class TypeExtensionsTests
    {
        public static IEnumerable<object[]> TestTypes =>
            new[]
                {
                    new object[] { typeof(string), false }, new object[] { typeof(int), false },
                    new object[] { typeof(int?), true }
                };

        /// <summary>
        /// Verifies a nullable type returns the correct result.
        /// </summary>
        [Theory]
        [MemberData(nameof(TestTypes))]
        public void IsNullableTest_Values(Type input, bool expectedResult)
        {
            bool result = input.IsNullable();
            Assert.Equal(expectedResult, result);
        }
    }
}