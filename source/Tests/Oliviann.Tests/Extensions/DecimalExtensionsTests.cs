namespace Oliviann.Tests.Extensions
{
    #region Usings

    using System.Collections.Generic;
    using Xunit;

    #endregion Usings

    [Trait("Category", "CI")]
    public class DecimalExtensionsTests
    {
        #region Celsius And Fahrenheit Tests

        public static IEnumerable<object[]> CelsiusTestValues =>
            new List<object[]>
                {
                    new object[] { default(double), -17.777777777777779 },
                    new object[] { -273, -169.44444444444446 },
                    new object[] { 0, -17.777777777777779 },
                    new object[] { 32, 0.0 },
                    new object[] { 100, 37.777777777777779 },
                    new object[] { 212, 100.0 },
                    new object[] { 401.73, 205.40555555555557 },
                };

        /// <summary>
        /// Verifies converting a set of Fahrenheit values to Celsius values
        /// correctly.
        /// </summary>
        [Theory]
        [MemberData(nameof(CelsiusTestValues))]
        public void ToCelsiusTest_Values(double fahrenheitValue, double expectedValue)
        {
            double result = fahrenheitValue.ToCelsius();
            Assert.Equal(result, expectedValue);
        }

        public static IEnumerable<object[]> FahrenheitTestValues =>
            new List<object[]>
                {
                    new object[] { default(double), 32.0 },
                    new object[] { -169, -272.2 },
                    new object[] { -17.7, 0.14000000000000057 },
                    new object[] { 0.0, 32.0 },
                    new object[] { 37.7, 99.860000000000014 },
                    new object[] { 100.0, 212.0 },
                    new object[] { 205.4, 401.72 },
                };

        /// <summary>
        /// Verifies converting a set of Celsius values to Fahrenheit values
        /// correctly.
        /// </summary>
        [Theory]
        [MemberData(nameof(FahrenheitTestValues))]
        public void ToFahrenheitTest_Values(double celsiusValue, double expectedValue)
        {
            double result = celsiusValue.ToFahrenheit();
            Assert.Equal(result, expectedValue);
        }

        #endregion Celsius And Fahrenheit Tests
    }
}