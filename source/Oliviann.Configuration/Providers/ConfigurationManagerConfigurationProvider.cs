#if NETFRAMEWORK

namespace Oliviann.Configuration.Providers
{
    #region Usings

    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using Oliviann.Properties;

    #endregion Usings

    /// <summary>
    /// Represents a <see cref="ConfigurationManager"/> configuration provider.
    /// </summary>
    public class ConfigurationManagerConfigurationProvider : ConfigurationProvider
    {
        #region Fields

        /// <summary>
        /// The application settings prefix.
        /// </summary>
        private const string AppSettingsPrefix = "AppSettings:";

        /// <summary>
        /// The connection strings prefix.
        /// </summary>
        private const string ConnectionStringsPrefix = "ConnectionStrings:";

        #endregion

        #region Constructor/Destructor

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="ConfigurationManagerConfigurationProvider"/> class.
        /// </summary>
        public ConfigurationManagerConfigurationProvider() : base("ConfigurationManager")
        {
        }

        #endregion Constructor/Destructor

        #region Methods

        /// <inheritdoc />
        public override IEnumerable<string> GetChildKeys()
        {
            // Add all the application settings.
            List<string> items = ConfigurationManager.AppSettings.AllKeys.Select(key => AppSettingsPrefix + key).ToList();
            items.AddRange(
                ConfigurationManager.ConnectionStrings.Cast<ConnectionStringSettings>()
                    .Select(connString => ConnectionStringsPrefix + connString.Name));

            return items;
        }

        /// <inheritdoc />
        public override void Load()
        {
            // Not used. We are going to go direct to the Configuration Manager
            // instead of loading it into memory.
        }

        /// <inheritdoc />
        protected override void SetInt(string key, string value)
        {
            if (key.StartsWith(AppSettingsPrefix, StringComparison.OrdinalIgnoreCase) && key.Length > 12)
            {
                string tempKey = key.Remove(0, 12);
                ConfigurationManager.AppSettings[tempKey] = value;
                return;
            }

            if (key.StartsWith(ConnectionStringsPrefix, StringComparison.OrdinalIgnoreCase) && key.Length > 18)
            {
                // NOTE: Currently this doesn't work so I am commenting it out
                // and may later remove this feature.
                ////string tempKey = key.Remove(0, 18);
                ////ConfigurationManager.ConnectionStrings.Remove(tempKey);
                ////ConfigurationManager.ConnectionStrings.Add(new ConnectionStringSettings(tempKey, value));
                return;
            }

            throw new ArgumentException(Resources.ERR_ConfigKeyPrefix);
        }

        /// <inheritdoc />
        protected override bool TryGetInt<T>(string key, out T value)
        {
            if (key.StartsWith(AppSettingsPrefix, StringComparison.OrdinalIgnoreCase) &&
                this.GetChildKeys().Any(k => k.EqualsOrdinalIgnoreCase(key)))
            {
                string tempKey = key.Remove(0, 12);
                ConfigurationManager.AppSettings[tempKey].TryCast(out value);
                return true;
            }

            if (key.StartsWith(ConnectionStringsPrefix, StringComparison.OrdinalIgnoreCase) &&
                this.GetChildKeys().Any(k => k.EqualsOrdinalIgnoreCase(key)))
            {
                string tempKey = key.Remove(0, 18);
                ConfigurationManager.ConnectionStrings[tempKey].ConnectionString.TryCast(out value);
                return true;
            }

            value = default(T);
            return false;
        }

        #endregion Methods
    }
}

#endif