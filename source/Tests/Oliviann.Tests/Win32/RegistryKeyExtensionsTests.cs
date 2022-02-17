namespace Oliviann.Tests.Win32
{
    #region Usings

    using System;
    using Oliviann.Win32;
    using Microsoft.Win32;
    using Xunit;

    #endregion Usings

    [Trait("Category", "CI")]
    public class RegistryKeyExtensionsTests
    {
        /// <summary>
        /// Verifies an argument null exception is thrown when a null registry
        /// key is passed in.
        /// </summary>
        [Fact]
        public void RegistryKey_Null()
        {
            RegistryKey key = null;
            string subKeyPath = @"SOFTWARE\Microsoft\.NETFramework\v4.0.30319\SKUs\.NETFramework,Version=v4.0";
            Assert.Throws<ArgumentNullException>(() => RegistryKeyExtensions.SubKeyExists(key, subKeyPath));
        }

        /// <summary>
        /// Verifies a valid registry key path returns correctly.
        /// </summary>
        [Fact]
        public void Valid_RegistryKeyAndPath()
        {
            string subKeyPath = @"SOFTWARE\Microsoft\.NETFramework\v4.0.30319\SKUs\.NETFramework,Version=v4.0";
            bool result = Registry.LocalMachine.SubKeyExists(subKeyPath);

            Assert.True(result, "Registry key does not exist on machine.");
        }

        /// <summary>
        /// Verifies a invalid registry key path returns correctly.
        /// </summary>
        [Fact]
        public void Invalid_RegistryKeyAndPath()
        {
            string subKeyPath = @"SOFTWARE\Microsoft\.NETFramework\v4.0.30319\SKUs\.NETFramework,kdsnfbk";
            bool result = Registry.LocalMachine.SubKeyExists(subKeyPath);

            Assert.False(result, "Registry key exists on local machine when it shouldn't.");
        }

        /// <summary>
        /// Verifies ...
        /// </summary>
        [Fact]
        public void RegistryKey_NullPath()
        {
            string subKeyPath = null;
            Assert.Throws<ArgumentNullException>(() => Registry.LocalMachine.SubKeyExists(subKeyPath));
        }
    }
}