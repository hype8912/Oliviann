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
    public class MemoryConfigurationProviderTests
    {
        #region Fields

        private IDictionary<string, string> TestValues =
            new Dictionary<string, string>
            {
                { "Oliviann", "Airplanes" },
                { "Taco", "Bell" },
                { "Multi", "Target" },
                { "Ned", "Flanders" }
            };

        #endregion Fields

        /// <summary>
        /// Verifies the correct number of keys are returned from the provider.
        /// </summary>
        [Fact]
        public void MemoryTest_GetChildKeys()
        {
            var provider = new MemoryConfigurationProvider("MemorySettings", this.TestValues);
            IEnumerable<string> result = provider.GetChildKeys();

            Assert.Equal(4, result.Count());
        }

        /// <summary>
        /// Verifies the provider will return the provided provider name.
        /// </summary>
        [Fact]
        public void MemoryTest_Name()
        {
            var provider = new MemoryConfigurationProvider("MemorySettings", this.TestValues);
            string result = provider.Name;

            Assert.Equal("MemorySettings", result);
        }

        /// <summary>
        /// Verifies the provider load does not throw an exception.
        /// </summary>
        [Fact]
        public void MemoryTest_Load()
        {
            var provider = new MemoryConfigurationProvider("MemorySettings", this.TestValues);
            provider.Load();
        }

        /// <summary>
        /// Verifies the provider set with a null or empty key throws an
        /// exception.
        /// </summary>
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void MemoryTest_SetNullKey(string key)
        {
            var provider = new MemoryConfigurationProvider("MemorySettings", this.TestValues);
            Assert.Throws<ArgumentNullException>(() => provider.Set(key, "Oliviann"));
        }

        /// <summary>
        /// Verifies the provider set with a null or empty key throws an
        /// exception.
        /// </summary>
        [Theory]
        [InlineData("Juan", "Valdez", "Valdez")]
        [InlineData("Oliviann", "F-15C", "F-15C")]
        public void MemoryTest_SetValues(string key, string inputValue, string expectedValue)
        {
            var provider = new MemoryConfigurationProvider("MemorySettings", this.TestValues);
            provider.Set(key, inputValue);

            bool result = provider.TryGet(key, out string value);
            Assert.True(result);
            Assert.Equal(expectedValue, value);
        }

        /// <summary>
        /// Verifies the provider get a null or empty key throws an exception.
        /// </summary>
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void MemoryTest_TryGetNullKey(string key)
        {
            var provider = new MemoryConfigurationProvider("MemorySettings", this.TestValues);
            Assert.Throws<ArgumentNullException>(() => provider.TryGet(key, out string value));
        }

        /// <summary>
        /// Verifies the provider get a null or empty key throws an exception.
        /// </summary>
        [Theory]
        [InlineData("Oliviann", "Airplanes", true)]
        [InlineData("Taco", "Bell", true)]
        [InlineData("Multi", "Target", true)]
        [InlineData("Ned", "Flanders", true)]
        [InlineData("Holy", null, false)]
        public void MemoryTest_TryGetValues(string key, string expectedValue, bool expectedResult)
        {
            var provider = new MemoryConfigurationProvider("MemorySettings", this.TestValues);
            bool result = provider.TryGet(key, out string value);
            Assert.Equal(expectedResult, result);
            Assert.Equal(expectedValue, value);
        }
    }
}