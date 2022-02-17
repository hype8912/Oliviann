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
    public class ConfigurationManagerConfigurationProviderTests
    {
        /// <summary>
        /// Verifies the correct number of keys are returned from the provider.
        /// </summary>
        [Fact]
        public void ConfigurationManagerTest_GetChildKeys()
        {
            var provider = new ConfigurationManagerConfigurationProvider();
            IEnumerable<string> result = provider.GetChildKeys();

            Assert.Equal(6, result.Count());
        }

        /// <summary>
        /// Verifies the provider will return the provided provider name.
        /// </summary>
        [Fact]
        public void ConfigurationManagerTest_Name()
        {
            var provider = new ConfigurationManagerConfigurationProvider();
            string result = provider.Name;

            Assert.Equal("ConfigurationManager", result);
        }

        /// <summary>
        /// Verifies the provider load does not throw an exception.
        /// </summary>
        [Fact]
        public void ConfigurationManagerTest_Load()
        {
            var provider = new ConfigurationManagerConfigurationProvider();
            provider.Load();
        }

        /// <summary>
        /// Verifies the provider set with a null or empty key throws an
        /// exception.
        /// </summary>
        [Theory]
        [InlineData(null, typeof(ArgumentNullException))]
        [InlineData("", typeof(ArgumentNullException))]
        [InlineData("AppSetings", typeof(ArgumentException))]
        [InlineData("App", typeof(ArgumentException))]
        [InlineData("AppSettings:", typeof(ArgumentException))]
        [InlineData("ConnectionStrings:", typeof(ArgumentException))]
        public void ConfigurationManagerTest_SetInvalidKeys(string key, Type exceptionType)
        {
            var provider = new ConfigurationManagerConfigurationProvider();
            Assert.Throws(exceptionType, () => provider.Set(key, "Oliviann"));
        }

        [Theory]
        [InlineData("AppSettings:Test11", "Value11")]
        ////[InlineData("ConnectionStrings:TestDb", @"Data Source=SERVER\DBASE;Initial Catalog=DbName;User Id=DbUser;Password=DbPwd;")]
        public void ConfigurationManagerTests_SetValidKeys(string key, string value)
        {
            var provider = new ConfigurationManagerConfigurationProvider();
            provider.Set(key, value);

            bool result = provider.TryGet(key, out string resultValue);
            Assert.True(result);
            Assert.Equal(value, resultValue);
        }

        /// <summary>
        /// Verifies the provider get a null or empty key throws an exception.
        /// </summary>
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void ConfigurationManagerTest_TryGetNullKey(string key)
        {
            var provider = new ConfigurationManagerConfigurationProvider();
            Assert.Throws<ArgumentNullException>(() => provider.TryGet(key, out string value));
        }

        /// <summary>
        /// Verifies the provider get a null or empty key throws an exception.
        /// </summary>
        [Theory]
        [InlineData("AppSettings:xunit.maxParallelThreads", "1", true)]
        [InlineData("appsettings:xunit.parallelizeTestCollections", "false", true)]
        [InlineData("APPSETTINGS:xunit.methodDisplay", "method", true)]
        [InlineData("connectionstrings:LocalSqlServer", "data source=.\\SQLEXPRESS;Integrated Security=SSPI;AttachDBFilename=|DataDirectory|aspnetdb.mdf;User Instance=true", true)]
        [InlineData("ConnectionStrings:OraAspNetConString", null, false)]
        [InlineData("ConnectionStrings:Oliviann", null, false)]
        [InlineData("Oliviann", null, false)]
        [InlineData("ConnectionStrings:OracleConnectionString", "User Id=APP_ABC123;Password=P@$$w0rd;Data Source=//server.oliviann.com/database.olivianndb", true)]
        public void ConfigurationManagerTest_TryGetValues(string key, string expectedValue, bool expectedResult)
        {
            var provider = new ConfigurationManagerConfigurationProvider();
            bool result = provider.TryGet(key, out string value);
            Assert.Equal(expectedResult, result);
            Assert.Equal(expectedValue, value);
        }
    }
}