namespace Oliviann.Data.Tests
{
    #region Usings

    using System;
    using Xunit;

    #endregion Usings

    [Trait("Category", "CI")]
    public class ADPTests
    {
        /// <summary>
        /// Verifies that matching providers do not throw an invalid operation
        /// exception.
        /// </summary>
        [Fact]
        public void CheckProviderInvalidTest_MatchingProviders()
        {
            var testProvider = DatabaseProvider.MicrosoftSqlServer;
            var correctProvider = DatabaseProvider.MicrosoftSqlServer;

            ADP.CheckProviderInvalid(testProvider, correctProvider);
        }

        /// <summary>
        /// Verifies that mismatching providers throws an invalid operation
        /// exception.
        /// </summary>
        [Fact]
        public void CheckProviderInvalidTest_MismatchingProviders()
        {
            var testProvider = DatabaseProvider.MicrosoftSqlServer;
            var correctProvider = DatabaseProvider.SQLite;

            Assert.Throws<InvalidOperationException>(() => ADP.CheckProviderInvalid(testProvider, correctProvider));
        }
    }
}