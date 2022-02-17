namespace Oliviann.Tests.Configuration.Providers
{
    #region Usings

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Oliviann.Configuration.Providers;
    using Oliviann.Testing.Fixtures;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Xunit;
    using Assert = Xunit.Assert;

    #endregion Usings

    [Trait("Category", "CI")]
    [DeploymentItem(@"TestObjects\INI\AutoImports.ini")]
    public class IniConfigurationProviderTests : IClassFixture<PathCleanupFixture>
    {
        #region Fields

        private readonly string TestIniFile;

        #endregion Fields

        #region Constructor/Destructor

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="IniConfigurationProviderTests"/> class.
        /// </summary>
        public IniConfigurationProviderTests(PathCleanupFixture fixture)
        {
            this.TestIniFile = fixture.CurrentDirectory + @"\TestObjects\INI\AutoImports.ini";
        }

        #endregion Constructor/Destructor

        /// <summary>
        /// Verifies the provider returns a collection of keys.
        /// </summary>
        [Fact]
        public void IniTest_GetChildKeys()
        {
            var provider = new IniConfigurationProvider(this.TestIniFile);
            provider.Load();
            IEnumerable<string> result = provider.GetChildKeys();

            Assert.Equal(5, result.Count());
        }

        /// <summary>
        /// Verifies the provider will return the default provider name.
        /// </summary>
        [Fact]
        public void IniTest_DefaultName()
        {
            var provider = new IniConfigurationProvider(null);
            string result = provider.Name;

            Assert.Equal("IniFile", result);
        }

        /// <summary>
        /// Verifies the provider will return the default provider name.
        /// </summary>
        [Fact]
        public void IniTest_SetFileName()
        {
            var provider = new IniConfigurationProvider(this.TestIniFile);
            string result = provider.Name;

            Assert.Equal("IniFile:AutoImports", result);
        }

        /// <summary>
        /// Verifies the provider set with a null or empty key throws an
        /// exception.
        /// </summary>
        [Theory]
        [InlineData(null, "TACO1", typeof(ArgumentNullException))]
        [InlineData("", "TACO1", typeof(ArgumentNullException))]
        [InlineData("   :   ", "TACO1", typeof(NotImplementedException))]
        public void IniTest_SetInvalidInput(string key, string value, Type expectedException)
        {
            var provider = new IniConfigurationProvider(this.TestIniFile);
            provider.Load();

            Assert.Throws(expectedException, () => provider.Set(key, value));
        }

        /// <summary>
        /// Verifies the provider get a null or empty key throws an exception.
        /// </summary>
        [Theory]
        [InlineData(null, typeof(ArgumentNullException))]
        [InlineData("", typeof(ArgumentNullException))]
        [InlineData("   :   ", typeof(ArgumentException))]
        [InlineData("User", typeof(ArgumentException))]
        [InlineData("User:", typeof(ArgumentException))]
        [InlineData("User:    ", typeof(ArgumentException))]
        [InlineData("   :ghere", typeof(ArgumentException))]
        [InlineData(":ghere", typeof(ArgumentException))]
        public void IniTest_TryGetInvalidInput(string key, Type expectionType)
        {
            var provider = new IniConfigurationProvider(this.TestIniFile);
            provider.Load();

            Assert.Throws(expectionType, () => provider.TryGet(key, out string value));
        }

        /// <summary>
        /// Verifies the provider gets the correct value.
        /// </summary>
        [Theory]
        [InlineData("Email:To", true, "")]
        [InlineData("Demo:pimport", true, @"Z:\data\quill21_db\parse_data\c130demo\import")]
        [InlineData("User:TACO_BELL", false, null)]
        public void IniTest_TryGetGoodInput(string key, bool expectedFound, string expectedResult)
        {
            var provider = new IniConfigurationProvider(this.TestIniFile);
            provider.Load();

            bool result = provider.TryGet(key, out string value);

            Assert.Equal(expectedFound, result);
            Assert.Equal(expectedResult, value);
        }
    }
}