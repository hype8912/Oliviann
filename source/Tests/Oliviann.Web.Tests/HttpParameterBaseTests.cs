#if NETFRAMEWORK

namespace Oliviann.Web.Tests
{
    #region Usings

    using System;
    using System.Collections.Specialized;
    using Xunit;

    #endregion Usings

    [Trait("Category", "CI")]
    public class HttpParameterBaseTests
    {
        #region GetCustomHeaders Tests

#if DEBUG

        /// <summary>
        /// Verifies an empty collection is returned even if the header string
        /// is null, empty, or whitespace.
        /// </summary>
        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("              ")]
        public void GetCustomHeadersTest_NullHeaderString(string customHeadersString)
        {
            NameValueCollection result = HttpParameterBase.GetCustomParameters(customHeadersString, null);

            Assert.NotNull(result);
            Assert.Empty(result);
        }

        /// <summary>
        /// Verifies a valid header string is parsed correctly and gets values
        /// correctly.
        /// </summary>
        [Fact]
        public void GetCustomHeadersTest_ValidHeadersString()
        {
            Func<string, string> del = key => key == "boeingbemsid" ? "1545853" : "cn=102610,ou=People,O=Boeing,c=US";
            NameValueCollection result = HttpParameterBase.GetCustomParameters("boeingbemsid;manager", del);

            Assert.NotNull(result);
            Assert.Equal("1545853", result["boeingbemsid"]);
            Assert.Equal("cn=102610,ou=People,O=Boeing,c=US", result["manager"]);
        }

        /// <summary>
        /// Verifies the exception message is inserted into the header value.
        /// </summary>
        [Fact]
        public void GetCustomHeadersTest_ThrowsException()
        {
            Func<string, string> del = key =>
            {
                if (key == "boeingbemsid")
                {
                    return "1545853";
                }

                throw new Exception("Null reference exception man!");
            };
            NameValueCollection result = HttpParameterBase.GetCustomParameters("boeingbemsid;manager", del);

            Assert.NotNull(result);
            Assert.Equal("1545853", result["boeingbemsid"]);
            Assert.Equal("Null reference exception man!", result["manager"]);
        }

#endif

        #endregion GetCustomHeaders Tests
    }
}

#endif