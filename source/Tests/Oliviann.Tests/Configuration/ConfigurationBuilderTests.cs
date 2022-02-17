namespace Oliviann.Tests.Configuration
{
    #region Usings

    using System;
    using Oliviann.Configuration;
    using Oliviann.Configuration.Providers;
    using Moq;
    using Xunit;

    #endregion Usings

    [Trait("Category", "CI")]
    public class ConfigurationBuilderTests
    {
        #region Add Providers Tests

        /// <summary>
        /// Verifies you can add a provider to the builder with out exception.
        /// </summary>
        [Fact]
        public void ConfigurationBuilderTest_AddProvider()
        {
            var builder = new ConfigurationBuilder();
            builder.AddProvider(new ConfigurationManagerConfigurationProvider());

            IConfiguration config = builder;
            Assert.NotNull(config);
        }

        /// <summary>
        /// Verifies no exception is thrown by updating a provider with the same
        /// name.
        /// </summary>
        [Fact]
        public void ConfigurationBuilderTest_UpdateProvider()
        {
            var builder = new ConfigurationBuilder();
            builder.AddProvider(new EnvironmentVariablesConfigurationProvider());

            string envResult = builder.Get<string>("windir");
            Assert.Equal(@"C:\WINDOWS", envResult, true);

            string expectedValue = "NoWhere";
            var mockProvider = new Mock<IConfigurationProvider>();
            mockProvider.Setup(p => p.Name).Returns("EnvironmentVariable");
            mockProvider.Setup(p => p.TryGet(It.IsAny<string>(), out expectedValue)).Returns(true);

            builder.AddProvider(mockProvider.Object);
            string moqResult = builder.Get<string>("windir");
            Assert.Equal(expectedValue, moqResult);
        }

        /// <summary>
        /// Verifies no exception is thrown by updating a provider with the same
        /// name.
        /// </summary>
        [Fact]
        public void ConfigurationBuilderTest_InvalidProviderName()
        {
            var builder = new ConfigurationBuilder();
            var mockProvider = new Mock<IConfigurationProvider>();
            mockProvider.Setup(p => p.Name).Returns(string.Empty);

            Assert.Throws<ArgumentException>(() => builder.AddProvider(mockProvider.Object));
        }

        #endregion Add Providers Tests

        #region Build Tests

        /// <summary>
        /// Verifies no exceptions are thrown when build is called with no
        /// providers.
        /// </summary>
        [Fact]
        public void ConfigurationBuilderTest_BuildNoProviders()
        {
            var builder = new ConfigurationBuilder();
            builder.Build();
        }

        /// <summary>
        /// Verifies no exceptions are thrown when build is called for the
        /// providers.
        /// </summary>
        [Fact]
        public void ConfigurationBuilderTest_BuildProviders()
        {
            var builder = new ConfigurationBuilder();
            builder.AddProvider(new ConfigurationManagerConfigurationProvider());
            builder.AddProvider(new EnvironmentVariablesConfigurationProvider());
            builder.Build();
        }

        #endregion Build Tests

        #region Get Tests

        /// <summary>
        /// Verifies the configuration doesn't throw an exception when trying
        /// to retrieve a value with no providers.
        /// </summary>
        [Fact]
        public void ConfigurationBuilderTest_GetWithNoProviders()
        {
            var builder = new ConfigurationBuilder();

            IConfiguration config = builder;
            string result = config.Get<string>("AppSettings:xunit.maxParallelThreads");

            Assert.Null(result);
        }

        /// <summary>
        /// Verifies the configuration retrieves the correct configuration
        /// value.
        /// </summary>
        [Fact]
        public void ConfigurationBuilderTest_GetWithOneProvider()
        {
            var builder = new ConfigurationBuilder();
            builder.AddProvider(new ConfigurationManagerConfigurationProvider());

            IConfiguration config = builder;
            string result = config.Get<string>("AppSettings:xunit.maxParallelThreads");

            Assert.Equal("1", result);
        }

        /// <summary>
        /// Verifies the configuration retrieves the correct configuration
        /// value.
        /// </summary>
        [Theory]
        [InlineData("AppSettings:xunit.maxParallelThreads", null, "1")]
        [InlineData("windir", null, @"C:\WINDOWS")]
        [InlineData("AppSettings:xunit.maxParallelThreads", "ConfigurationManager", "1")]
        [InlineData("windir", "EnvironmentVariable", @"C:\WINDOWS")]
        [InlineData("windir", "MissingProvvider", @"C:\WINDOWS")]
        public void ConfigurationBuilderTest_GetWithMultipleProvider(string key, string providerName, string expectedResult)
        {
            var builder = new ConfigurationBuilder();
            builder.AddProvider(new ConfigurationManagerConfigurationProvider());
            builder.AddProvider(new EnvironmentVariablesConfigurationProvider());

            IConfiguration config = builder;
            string result = config.Get<string>(key, providerName);

            Assert.Equal(expectedResult, result, true);
        }

        #endregion Get Tests
    }
}