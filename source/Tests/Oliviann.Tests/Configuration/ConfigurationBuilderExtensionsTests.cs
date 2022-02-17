namespace Oliviann.Tests.Configuration
{
    #region Usings

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Oliviann.Configuration;

    using Xunit;

    #endregion Usings

    [Trait("Category", "CI")]
    public class ConfigurationBuilderExtensionsTests
    {
        #region Configuration Manager Tests

        /// <summary>
        /// Verifies an argument exception is thrown for a null builder.
        /// </summary>
        [Fact]
        public void AddConfigurationManagerTest_NullBuilder()
        {
            ConfigurationBuilder builder = null;
            Assert.Throws<ArgumentNullException>(() => builder.AddConfigurationManager());
        }

        /// <summary>
        /// Verifies a configuration manager provider is added to the builder.
        /// </summary>
        [Fact]
        public void AddConfigurationManagerTest_ValidBuilder()
        {
            var builder = new ConfigurationBuilder().AddConfigurationManager();
            string result = builder.Get("AppSettings:xunit.maxParallelThreads");

            Assert.Equal("1", result);
        }

        #endregion Configuration Manager Tests

        #region Environment Variables Tests

        /// <summary>
        /// Verifies an argument exception is thrown for a null builder.
        /// </summary>
        [Fact]
        public void AddEnvironmentVariablesTest_NullBuilder()
        {
            ConfigurationBuilder builder = null;
            Assert.Throws<ArgumentNullException>(() => builder.AddEnvironmentVariables());
        }

        /// <summary>
        /// Verifies a environment variables provider is added to the builder.
        /// </summary>
        [Fact]
        public void AddEnvironmentVariablesTest_ValidBuilder()
        {
            var builder = new ConfigurationBuilder().AddEnvironmentVariables();
            string result = builder.Get("windir");

            Assert.Equal(@"C:\WINDOWS", result, true);
        }

        #endregion Environment Variables Tests

        #region Registry Tests

        /// <summary>
        /// Verifies an argument exception is thrown for a null builder.
        /// </summary>
        [Fact]
        public void AddRegistryTest_NullBuilder()
        {
            ConfigurationBuilder builder = null;
            Assert.Throws<ArgumentNullException>(() => builder.AddRegistry());
        }

        /// <summary>
        /// Verifies a registry provider is added to the builder.
        /// </summary>
        [Fact]
        public void AddRegistryTest_ValidBuilder()
        {
            var builder = new ConfigurationBuilder().AddRegistry();
            string result = builder.Get(@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\.txt:Content Type");

            Assert.Equal("text/plain", result);
        }

        #endregion Registry Tests

        #region In-Memory Collection Tests

        /// <summary>
        /// Verifies an argument exception is thrown for a null builder.
        /// </summary>
        [Fact]
        public void AddInMemoryCollectionTest_NullBuilder()
        {
            ConfigurationBuilder builder = null;
            Assert.Throws<ArgumentNullException>(
                () => builder.AddInMemoryCollection(
                    "TestMemory",
                    new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)));
        }

        /// <summary>
        /// Verifies a in-memory collection provider is added to the builder.
        /// </summary>
        [Fact]
        public void AddInMemoryCollectionTest_ValidBuilder()
        {
            var testValues = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase) { { "Oliviann", "Airplanes" } };

            var builder = new ConfigurationBuilder().AddInMemoryCollection("MemorySettings2", testValues);
            string result = builder.Get("Oliviann");

            Assert.Equal("Airplanes", result);
        }

        #endregion In-Memory Collection Tests
    }
}