namespace Oliviann.Configuration
{
    #region Usings

    using System.Collections.Specialized;
    using System.Configuration;

    #endregion

    /// <summary>
    /// Represents a interface for implementing an instance of
    /// ConfigurationManager.
    /// </summary>
    public interface IConfigurationManager
    {
        /// <summary>Gets the
        /// <see cref="T:System.Configuration.AppSettingsSection"></see> data
        /// for the current application's default configuration.
        /// </summary>
        /// <returns>Returns a
        /// <see cref="System.Collections.Specialized.NameValueCollection"></see>
        /// object that contains the contents of the
        /// <see cref="System.Configuration.AppSettingsSection"></see> object
        /// for the current application's default configuration.
        /// </returns>
        /// <exception cref="T:System.Configuration.ConfigurationErrorsException">
        /// Could not retrieve a
        /// <see cref="System.Collections.Specialized.NameValueCollection"></see>
        /// object with the application settings data.</exception>
        NameValueCollection AppSettings { get; }

        /// <summary>Gets the
        /// <see cref="T:System.Configuration.ConnectionStringsSection"></see>
        /// data for the current application's default configuration.</summary>
        /// <returns>Returns a
        /// <see cref="System.Configuration.ConnectionStringSettingsCollection"></see>
        /// object that contains the contents of the
        /// <see cref="System.Configuration.ConnectionStringsSection"></see>
        /// object for the current application's default configuration.
        /// </returns>
        /// <exception cref="T:System.Configuration.ConfigurationErrorsException">
        /// Could not retrieve a
        /// <see cref="System.Configuration.ConnectionStringSettingsCollection"></see>
        /// object.</exception>
        ConnectionStringSettingsCollection ConnectionStrings { get; }

        /// <summary>Retrieves a specified configuration section for the current
        /// application's default configuration.</summary>
        /// <param name="sectionName">The configuration section path and name.
        /// </param>
        /// <returns>The specified
        /// <see cref="System.Configuration.ConfigurationSection"></see> object,
        /// or null if the section does not exist.</returns>
        /// <exception cref="T:System.Configuration.ConfigurationErrorsException">
        /// A configuration file could not be loaded.</exception>
        object GetSection(string sectionName);

        /// <summary>Opens the configuration file for the current application as
        /// a <see cref="T:System.Configuration.Configuration"></see> object.
        /// </summary>
        /// <param name="userLevel">The
        /// <see cref="T:System.Configuration.ConfigurationUserLevel"></see> for
        /// which you are opening the configuration.</param>
        /// <returns>A <see cref="System.Configuration.Configuration"></see>
        /// object.</returns>
        /// <exception cref="T:System.Configuration.ConfigurationErrorsException">
        /// A configuration file could not be loaded.</exception>
        System.Configuration.Configuration OpenExeConfiguration(ConfigurationUserLevel userLevel);

        /// <summary>Opens the specified client configuration file as a
        /// <see cref="T:System.Configuration.Configuration"></see> object.
        /// </summary>
        /// <param name="exePath">The path of the executable (exe) file.</param>
        /// <returns>A <see cref="System.Configuration.Configuration"></see>
        /// object.</returns>
        /// <exception cref="T:System.Configuration.ConfigurationErrorsException">
        /// A configuration file could not be loaded.</exception>
        System.Configuration.Configuration OpenExeConfiguration(string exePath);

        /// <summary>Opens the machine configuration file on the current
        /// computer as a
        /// <see cref="T:System.Configuration.Configuration"></see> object.
        /// </summary>
        /// <returns>A <see cref="System.Configuration.Configuration"></see>
        /// object.</returns>
        /// <exception cref="T:System.Configuration.ConfigurationErrorsException">
        /// A configuration file could not be loaded.</exception>
        System.Configuration.Configuration OpenMachineConfiguration();

        /// <summary>Opens the specified client configuration file as a
        /// <see cref="T:System.Configuration.Configuration"></see> object that
        /// uses the specified file mapping and user level.</summary>
        /// <param name="fileMap">An
        /// <see cref="T:System.Configuration.ExeConfigurationFileMap"></see>
        /// object that references configuration file to use instead of the
        /// application default configuration file.</param>
        /// <param name="userLevel">The
        /// <see cref="T:System.Configuration.ConfigurationUserLevel"></see>
        /// object for which you are opening the configuration.</param>
        /// <returns>The configuration object.</returns>
        /// <exception cref="T:System.Configuration.ConfigurationErrorsException">
        /// A configuration file could not be loaded.</exception>
        System.Configuration.Configuration OpenMappedExeConfiguration(
          ExeConfigurationFileMap fileMap,
          ConfigurationUserLevel userLevel);

#if !NET40
        /// <summary>Opens the specified client configuration file as a
        /// <see cref="T:System.Configuration.Configuration"></see> object that
        /// uses the specified file mapping, user level, and preload option.
        /// </summary>
        /// <param name="fileMap">An
        /// <see cref="T:System.Configuration.ExeConfigurationFileMap"></see>
        /// object that references the configuration file to use instead of the
        /// default application configuration file.</param>
        /// <param name="userLevel">The
        /// <see cref="T:System.Configuration.ConfigurationUserLevel"></see>
        /// object for which you are opening the configuration.</param>
        /// <param name="preLoad">true to preload all section groups and
        /// sections; otherwise, false.</param>
        /// <returns>The configuration object.</returns>
        /// <exception cref="T:System.Configuration.ConfigurationErrorsException">
        /// A configuration file could not be loaded.</exception>
        System.Configuration.Configuration OpenMappedExeConfiguration(
          ExeConfigurationFileMap fileMap,
          ConfigurationUserLevel userLevel,
          bool preLoad);
#endif

        /// <summary>Opens the machine configuration file as a
        /// <see cref="T:System.Configuration.Configuration"></see> object that
        /// uses the specified file mapping.</summary>
        /// <param name="fileMap">An
        /// <see cref="T:System.Configuration.ExeConfigurationFileMap"></see>
        /// object that references configuration file to use instead of the
        /// application default configuration file.</param>
        /// <returns>A <see cref="System.Configuration.Configuration"></see>
        /// object.</returns>
        /// <exception cref="T:System.Configuration.ConfigurationErrorsException">
        /// A configuration file could not be loaded.</exception>
        System.Configuration.Configuration OpenMappedMachineConfiguration(
          ConfigurationFileMap fileMap);

        /// <summary>Refreshes the named section so the next time that it is
        /// retrieved it will be re-read from disk.</summary>
        /// <param name="sectionName">The configuration section name or the
        /// configuration path and section name of the section to refresh.
        /// </param>
        void RefreshSection(string sectionName);
    }
}