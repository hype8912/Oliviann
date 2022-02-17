namespace Oliviann.Data.Tests
{
    #region Usings

    using System.Collections.Generic;
    using System.Linq;
    using Xunit;

    #endregion Usings

    [Trait("Category", "CI")]
    public class IDatabaseExtensionsTests
    {
        /// <summary>
        /// Verifies a null collection returns a null object.
        /// </summary>
        [Fact]
        public void FilterByNameTest_NullCollection()
        {
            IEnumerable<IDatabase> dbs = null;
            IDatabase result = dbs.FilterByName("PMDB");

            Assert.Null(result);
        }

        /// <summary>
        /// Verifies an empty collection returns a null object.
        /// </summary>
        [Fact]
        public void FilterByNameTest_EmptyCollection()
        {
            IEnumerable<IDatabase> dbs = Enumerable.Empty<IDatabase>();
            IDatabase result = dbs.FilterByName("PMDB");

            Assert.Null(result);
        }

        /// <summary>
        /// Verifies an invalid database name string returns a null instance.
        /// </summary>
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("Taco Bell")]
        [InlineData("JKBD8y7348u02388rnd9sacs79ctoelrjs 8odsjg ne4q86u503416t om4lg")]
        public void FilterByNameTest_InvalidStrings(string input)
        {
            IEnumerable<IDatabase> dbs = new List<IDatabase>
                                             {
                                                 new InternalDatabase { Name = "PMDB" },
                                                 new InternalDatabase { Name = "ColorMat" },
                                                 new InternalDatabase { Name = "Stuxnet" }
                                             };
            IDatabase result = dbs.FilterByName(input);

            Assert.Null(result);
        }

        /// <summary>
        /// Verifies a valid string returns the correct object.
        /// </summary>
        /// <param name="input">The input.</param>
        [Theory]
        [InlineData("PMDB")]
        public void FilterByNameTest_ValidString(string input)
        {
            IEnumerable<IDatabase> dbs = new List<IDatabase>
                                             {
                                                 new InternalDatabase { Name = "PMDB" },
                                                 new InternalDatabase { Name = "ColorMat" },
                                                 new InternalDatabase { Name = "Stuxnet" }
                                             };
            IDatabase result = dbs.FilterByName(input);

            Assert.NotNull(result);
            Assert.Equal(input, result.Name);
        }
    }
}