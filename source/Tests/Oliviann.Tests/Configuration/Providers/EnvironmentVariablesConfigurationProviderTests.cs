namespace Oliviann.Tests.Configuration.Providers
{
    #region Usings

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Oliviann.Configuration.Providers;
    using Xunit;

    #endregion Usings

    [Trait("Category", "CI")]
    public class EnvironmentVariablesConfigurationProviderTests
    {
        /// <summary>
        /// Verifies the provider returns a collection of keys.
        /// </summary>
        [Fact]
        public void EnvironmentVariablesTest_GetChildKeys()
        {
            var provider = new EnvironmentVariablesConfigurationProvider();
            IEnumerable<string> result = provider.GetChildKeys();

            Assert.True(result.Count().IsGreaterThan(50));
        }

        [Fact]
        public void EnvironmentVariablesTest_GetChildKeysWithPrefix()
        {
            var provider = new EnvironmentVariablesConfigurationProvider("PROCESSOR_");
            IEnumerable<string> result = provider.GetChildKeys();

            Assert.True(result.Count().IsGreaterThan(2));
        }

        /// <summary>
        /// Verifies the provider will return the default provider name.
        /// </summary>
        [Fact]
        public void EnvironmentVariablesTest_DefaultName()
        {
            var provider = new EnvironmentVariablesConfigurationProvider();
            string result = provider.Name;

            Assert.Equal("EnvironmentVariable", result);
        }

        /// <summary>
        /// Verifies the provider will return the default provider name.
        /// </summary>
        [Fact]
        public void EnvironmentVariablesTest_NameWithPrefix()
        {
            var provider = new EnvironmentVariablesConfigurationProvider("IMIS");
            string result = provider.Name;

            Assert.Equal("EnvironmentVariable_IMIS", result);
        }

        /// <summary>
        /// Verifies the provider load does not throw an exception.
        /// </summary>
        [Fact]
        public void EnvironmentVariablesTest_Load()
        {
            var provider = new EnvironmentVariablesConfigurationProvider();
            provider.Load();
        }

        /// <summary>
        /// Verifies the provider set with a null or empty key throws an
        /// exception.
        /// </summary>
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void EnvironmentVariablesTest_SetNullKey(string key)
        {
            var provider = new EnvironmentVariablesConfigurationProvider();
            Assert.Throws<ArgumentNullException>(() => provider.Set(key, "Oliviann"));
        }

        /// <summary>
        /// Verifies the provider get a null or empty key throws an exception.
        /// </summary>
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void EnvironmentVariablesTest_TryGetNullKey(string key)
        {
            var provider = new EnvironmentVariablesConfigurationProvider();
            Assert.Throws<ArgumentNullException>(() => provider.TryGet(key, out string value));
        }

        /// <summary>
        /// Verifies the provider gets the correct value.
        /// </summary>
        [Theory]
        [InlineData("windir", true, @"C:\WINDOWS")]
        [InlineData("TACO_BELL", false, null)]
        public void EnvironmentVariablesTest_TryGet(string key, bool expectedFound, string expectedResult)
        {
            var provider = new EnvironmentVariablesConfigurationProvider();
            bool result = provider.TryGet(key, out string value);

            Assert.Equal(expectedFound, result);
            Assert.Equal(expectedResult, value, true);
        }

        /// <summary>
        /// Verifies the provider gets the correct value.
        /// </summary>
        [Theory]
        [InlineData("windir", true, @"C:\WINDOWS")]
        [InlineData("TACO_BELL", false, null)]
        public void EnvironmentVariablesTest_TryGetWithPrefix(string key, bool expectedFound, string expectedResult)
        {
            var provider = new EnvironmentVariablesConfigurationProvider("win");
            bool result = provider.TryGet(key, out string value);

            Assert.Equal(expectedFound, result);
            Assert.Equal(expectedResult, value, true);
        }
    }
}