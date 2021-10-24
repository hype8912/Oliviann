namespace Oliviann.Configuration
{
    #region Usings

    using System.Collections.Specialized;
    using System.Configuration;

    #endregion

    /// <summary>
    /// Represents a configuration manager wrapper.
    /// </summary>
    public class ConfigurationManagerWrapper : IConfigurationManager
    {
        #region Properties

        /// <inheritdoc />
        public NameValueCollection AppSettings => ConfigurationManager.AppSettings;

        /// <inheritdoc />
        public ConnectionStringSettingsCollection ConnectionStrings => ConfigurationManager.ConnectionStrings;

        #endregion

        #region Methods

        /// <inheritdoc />
        public object GetSection(string sectionName) => ConfigurationManager.GetSection(sectionName);

        /// <inheritdoc />
        public Configuration OpenExeConfiguration(ConfigurationUserLevel userLevel) =>
            ConfigurationManager.OpenExeConfiguration(userLevel);

        /// <inheritdoc />
        public Configuration OpenExeConfiguration(string exePath) => ConfigurationManager.OpenExeConfiguration(exePath);

        /// <inheritdoc />
        public Configuration OpenMachineConfiguration() => ConfigurationManager.OpenMachineConfiguration();

        /// <inheritdoc />
        public Configuration OpenMappedExeConfiguration(ExeConfigurationFileMap fileMap, ConfigurationUserLevel userLevel) =>
            ConfigurationManager.OpenMappedExeConfiguration(fileMap, userLevel);

#if !NET40
        /// <inheritdoc />
        public Configuration OpenMappedExeConfiguration(
            ExeConfigurationFileMap fileMap,
            ConfigurationUserLevel userLevel,
            bool preLoad) =>
            ConfigurationManager.OpenMappedExeConfiguration(fileMap, userLevel, preLoad);
#endif

        /// <inheritdoc />
        public Configuration OpenMappedMachineConfiguration(ConfigurationFileMap fileMap) =>
            ConfigurationManager.OpenMappedMachineConfiguration(fileMap);

        /// <inheritdoc />
        public void RefreshSection(string sectionName) => ConfigurationManager.RefreshSection(sectionName);

        #endregion
    }
}