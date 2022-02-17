namespace Oliviann.Tests.Configuration
{
    #region Usings

    using System;
    using Oliviann.Configuration;
    using Oliviann.Configuration.Providers;
    using Xunit;

    #endregion Usings

    [Trait("Category", "CI")]
    public class IConfigurationExtensionsTests
    {
        #region Get Tests

        /// <summary>
        /// Verifies an argument exception is thrown for a null configuration.
        /// </summary>
        [Fact]
        public void IConfigurationTest_Get_NullConfiguration()
        {
            IConfiguration config = null;
            Assert.Throws<ArgumentNullException>(() => config.Get("Oliviann"));
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
        public void IConfigurationTest_GetValidKeys(string key, string providerName, string expectedResult)
        {
            var builder = new ConfigurationBuilder();
            builder.AddProvider(new ConfigurationManagerConfigurationProvider());
            builder.AddProvider(new EnvironmentVariablesConfigurationProvider());

            IConfiguration config = builder;
            string result = config.Get(key, providerName);

            Assert.Equal(expectedResult, result, true);
        }

        #endregion Get Tests

        #region GetAppSetting Tests

        /// <summary>
        /// Verifies an argument exception is thrown for a null configuration.
        /// </summary>
        [Fact]
        public void IConfigurationTest_GetAppSetting_NullConfiguration()
        {
            IConfiguration config = null;
            Assert.Throws<ArgumentNullException>(() => config.GetAppSetting("test.taco"));
        }

        [Theory]
        [InlineData("test.taco", null, "bell")]
        [InlineData("test.taco", "ConfigurationManager", "bell")]
        [InlineData("BlahBlah", null, null)]
        public void IConfigurationTest_GetAppSetting(string key, string providerName, string expectedValue)
        {
            var builder = new ConfigurationBuilder();
            builder.AddProvider(new ConfigurationManagerConfigurationProvider());
            builder.AddProvider(new EnvironmentVariablesConfigurationProvider());

            IConfiguration config = builder;
            string result = config.GetAppSetting(key, providerName);

            Assert.Equal(expectedValue, result);
        }

        #endregion GetAppSetting Tests

        #region GetConnectionString Tests

        /// <summary>
        /// Verifies an argument exception is thrown for a null configuration.
        /// </summary>
        [Fact]
        public void IConfigurationTest_GetConnectionString_NullConfiguration()
        {
            IConfiguration config = null;
            Assert.Throws<ArgumentNullException>(() => config.GetConnectionString("OracleConnectionString"));
        }

        [Theory]
        [InlineData("OracleConnectionString", null, "User Id=APP_ABC123;Password=P@$$w0rd;Data Source=//server.oliviann.com/database.olivianndb")]
        [InlineData("OracleConnectionString", "ConfigurationManager", "User Id=APP_ABC123;Password=P@$$w0rd;Data Source=//server.oliviann.com/database.olivianndb")]
        [InlineData("DbConnectionString", null, null)]
        public void IConfigurationTest_GetConnectionString(string key, string providerName, string expectedValue)
        {
            var builder = new ConfigurationBuilder();
            builder.AddProvider(new ConfigurationManagerConfigurationProvider());
            builder.AddProvider(new EnvironmentVariablesConfigurationProvider());

            IConfiguration config = builder;
            string result = config.GetConnectionString(key, providerName);

            Assert.Equal(expectedValue, result);
        }

        #endregion GetConnectionString Tests
    }
}