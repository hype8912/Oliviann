namespace Oliviann.Data.Tests
{
    #region Usings

    using Xunit;

    #endregion Usings

    [Trait("Category", "CI")]
    public class DbErrorTests
    {
        #region cTor Tests

        /// <summary>
        /// Verifies the values are correct when set in the constructor.
        /// </summary>
        [Theory]
        [InlineData("This is a message.", "some state", int.MinValue, "This is a message.", "some state", int.MinValue)]
        [InlineData(null, null, int.MaxValue, "", "", int.MaxValue)]
        [InlineData("", "", 99, "", "", 99)]
        public void DbErrorTest_cTor1_Values(
            string message,
            string state,
            int number,
            string expectedMessage,
            string expectedState,
            int expectedNumber)
        {
            var error = new DbError(message, state, number);

            Assert.True(error.Message == expectedMessage, "Message is incorrect.[{0}]".FormatWith(error.Message));
            Assert.True(error.State == expectedState, "State is incorrect.[{0}]".FormatWith(error.State));
            Assert.True(error.Number == expectedNumber, "Number value is incorrect.[{0}]".FormatWith(error.Number));
            Assert.True(
                error.Source == ".Net DbClient Data Provider",
                "Default source is incorrect.[{0}]".FormatWith(error.Source));
        }

        /// <summary>
        /// Verifies the values are correct when set in the constructor.
        /// </summary>
        [Theory]
        [InlineData("This is a source.", "This is a message.", "some state", int.MinValue, "This is a message.", "some state", int.MinValue, "This is a source.")]
        [InlineData(null, null, null, int.MaxValue, "", "", int.MaxValue, "")]
        public void DbErrorTest_cTor2_Values(
            string source,
            string message,
            string state,
            int number,
            string expectedMessage,
            string expectedState,
            int expectedNumber,
            string expectedSource)
        {
            var error = new DbError(source, message, state, number);

            Assert.True(error.Message == expectedMessage, "Message is incorrect.[{0}]".FormatWith(error.Message));
            Assert.True(error.State == expectedState, "State is incorrect.[{0}]".FormatWith(error.State));
            Assert.True(error.Number == expectedNumber, "Number value is incorrect.[{0}]".FormatWith(error.Number));
            Assert.True(error.Source == expectedSource, "Source is incorrect.[{0}]".FormatWith(error.Source));
        }

        #endregion cTor Tests

        #region ToString Tests

        /// <summary>
        /// Verifies the values the ToString values returns correctly with a set
        /// message.
        /// </summary>
        [Theory]
        [InlineData("This is a message.")]
        [InlineData(null)]
        [InlineData("")]
        public void DbErrorTest_ToString_Text(string message)
        {
            var error = new DbError(message, "some state", int.MinValue);
            string result = error.ToString();

            Assert.True(result == "Oliviann.Data.DbError: " + message, "ToString is incorrect.[{0}]".FormatWith(result));
        }

        #endregion ToString Tests

        #region SetSource Tests

        /// <summary>
        /// Verifies the source value returns correctly with a set source text.
        /// </summary>
        [Fact]
        public void DbError_SetSource_Text()
        {
            var error = new DbError("This is a message.", "some state", int.MinValue);
            Assert.True(error.Source == ".Net DbClient Data Provider", "Default source is incorrect.[{0}]".FormatWith(error.Source));

            string newsource = "Taco Bell";
            error.Source = newsource;
            Assert.True(error.Source == newsource, "New source is incorrect.[{0}]".FormatWith(error.Source));
        }

        /// <summary>
        /// Verifies the source value returns correctly with a set null text.
        /// </summary>
        [Fact]
        public void DbError_SetSource_Null()
        {
            var error = new DbError("Original Source", "This is a message.", "some state", int.MinValue);
            Assert.True(error.Source == "Original Source", "Default source is incorrect.[{0}]".FormatWith(error.Source));

            string newsource = null;
            error.Source = newsource;
            Assert.True(error.Source == string.Empty, "New source is incorrect.[{0}]".FormatWith(error.Source));
        }

        #endregion SetSource Tests
    }
}