namespace Oliviann.Tests.Configuration.Providers
{
    #region Usings

    using System;
    using System.Collections.Generic;
    using Oliviann.Configuration.Providers;
    using Xunit;

    #endregion Usings

    [Trait("Category", "CI")]
    public class RegistryConfigurationProviderTests
    {
        /// <summary>
        /// Verifies the provider returns a collection of keys.
        /// </summary>
        [Fact]
        public void RegistryTest_GetChildKeys()
        {
            var provider = new RegistryConfigurationProvider();
            IEnumerable<string> result = provider.GetChildKeys();

            Assert.Empty(result);
        }

        /// <summary>
        /// Verifies the provider will return the default provider name.
        /// </summary>
        [Fact]
        public void RegistryTest_DefaultName()
        {
            var provider = new RegistryConfigurationProvider();
            string result = provider.Name;

            Assert.Equal("Registry", result);
        }

        /// <summary>
        /// Verifies the provider load does not throw an exception.
        /// </summary>
        [Fact]
        public void RegistryTest_Load()
        {
            var provider = new RegistryConfigurationProvider();
            provider.Load();
        }

        /// <summary>
        /// Verifies the provider set with a null or empty key throws an
        /// exception.
        /// </summary>
        [Theory]
        [InlineData(null, "TACO1", typeof(ArgumentNullException))]
        [InlineData("", "TACO1", typeof(ArgumentNullException))]
        [InlineData("   :   ", "TACO1", typeof(ArgumentException))]
        [InlineData(@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\.txt", "TACO1", typeof(ArgumentException))]
        [InlineData(@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\.txt:", "TACO1", typeof(ArgumentException))]
        [InlineData(@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\.txt:    ", "TACO1", typeof(ArgumentException))]
        [InlineData(@"   :Content Type", "TACO1", typeof(ArgumentException))]
        [InlineData("Content Type", "TACO1", typeof(ArgumentException))]
        public void RegistryTest_SetInvalidInput(string key, string value, Type expectedException)
        {
            var provider = new RegistryConfigurationProvider();
            Assert.Throws(expectedException, () => provider.Set(key, value));
        }

        /// <summary>
        /// Verifies the provider get a null or empty key throws an exception.
        /// </summary>
        [Theory]
        [InlineData(null, typeof(ArgumentNullException))]
        [InlineData("", typeof(ArgumentNullException))]
        [InlineData("   :   ", typeof(ArgumentException))]
        [InlineData(@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\.txt", typeof(ArgumentException))]
        [InlineData(@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\.txt:", typeof(ArgumentException))]
        [InlineData(@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\.txt:    ", typeof(ArgumentException))]
        [InlineData(@"   :Content Type", typeof(ArgumentException))]
        [InlineData("Content Type", typeof(ArgumentException))]
        public void RegistryTest_TryGetInvalidInput(string key, Type expectionType)
        {
            var provider = new RegistryConfigurationProvider();
            Assert.Throws(expectionType, () => provider.TryGet(key, out string value));
        }

        /// <summary>
        /// Verifies the provider gets the correct value.
        /// </summary>
        [Theory]
        [InlineData(@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\.txt:Content Type", true, "text/plain")]
        [InlineData(@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\.txt:TACO_BELL", false, null)]
        public void RegistryTest_TryGet(string key, bool expectedFound, string expectedResult)
        {
            var provider = new RegistryConfigurationProvider();
            bool result = provider.TryGet(key, out string value);

            Assert.Equal(expectedFound, result);
            Assert.Equal(expectedResult, value);
        }
    }
}