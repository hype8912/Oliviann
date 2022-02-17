namespace Oliviann.Tests.Security
{
    #region Usings

    using System;
    using System.Security;
    using Oliviann.Security;
    using Xunit;

    #endregion Usings

    [Trait("Category", "CI")]
    public class SecureStringExtensionsTests
    {
        #region IsNullOrEmpty Tests

        [Fact]
        public void SecureStringIsNullOrEmptyTest_Null()
        {
            SecureString originalValue = null;
            bool returnedValue = originalValue.IsNullOrEmpty();

            Assert.True(returnedValue);
        }

        [Theory]
        [InlineData("", true)]
        [InlineData(@"QWERTYUIOP{}|ASDFGHJKL:ZXCVBNM<>?~!@#$%^&*()_+`1234567890-=qwertyuiop[]\asdfghjkl;'zxcvbnm,./", false)]
        public void SecureStringIsNullOrEmptyTest_Strings(string input, bool expectedResult)
        {
            SecureString inputString = input.ToSecureString();
            bool result = inputString.IsNullOrEmpty();

            Assert.Equal(expectedResult, result);
        }

        #endregion IsNullOrEmpty Tests

        #region ToUnsecureString Tests

        [Fact]
        public void ToUnsecureStringTest_Null()
        {
            string result = SecureStringExtensions.ToUnsecureString(null);
            Assert.Null(result);
        }

        [Theory]
        [InlineData("")]
        [InlineData("QWERTYUIOP{}|ASDFGHJKL:ZXCVBNM<>?~!@#$%^&*()_+`1234567890-=qwertyuiop[]\\asdfghjkl;'zxcvbnm,./")]
        [InlineData("Oliviann$%^&I23456789O")]
        [InlineData("QWer!@34tyuiop098765")]
        [InlineData("Qwertyu#10iop0987654")]
        [InlineData("QAws!@45wsedrf098765")]
        [InlineData(@"y8T5?oi7""nyC5!K@86E#")]
        [InlineData("2jPD!hK#yiR3quOBjtAH")]
        public void ToSecureString_ToUnsecureString_RoundTrip(string input)
        {
            SecureString secureValue = input.ToSecureString();
            string result = secureValue.ToUnsecureString();

            Assert.Equal(input, result);
        }

        #endregion ToUnsecureString Tests

        #region ToSecureString Tests

        //// See IsNullOrEmpty Methods for additional tests using ToSecureString.

        [Fact]
        public void ToSecureStringTest_Null()
        {
            Assert.Throws<ArgumentNullException>(() => SecureStringExtensions.ToSecureString(null));
        }

        #endregion ToSecureString Tests
    }
}