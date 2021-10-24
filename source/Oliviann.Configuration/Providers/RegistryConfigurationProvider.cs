namespace Oliviann.Configuration.Providers
{
    #region Usings

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Oliviann.Properties;
    using Microsoft.Win32;

    #endregion Usings

    /// <summary>
    /// Represents a <see cref="Registry"/> configuration provider.
    /// </summary>
    public class RegistryConfigurationProvider : ConfigurationProvider
    {
        #region Constructor/Destructor

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="RegistryConfigurationProvider"/> class.
        /// </summary>
        public RegistryConfigurationProvider() : base("Registry")
        {
        }

        #endregion Constructor/Destructor

        #region Methods

        /// <inheritdoc />
        public override IEnumerable<string> GetChildKeys()
        {
            // Not used.
            return Enumerable.Empty<string>();
        }

        /// <inheritdoc />
        public override void Load()
        {
            // Not used. We are going to go directly to the registry to retrieve
            // the values.
        }

        /// <inheritdoc />
        protected override void SetInt(string key, string value)
        {
            string[] splitKey = key.Split(StringSplitOptions.RemoveEmptyEntries, ':');
            if (splitKey.Length < 2 || splitKey[0].IsNullOrWhiteSpace() || splitKey[1].IsNullOrWhiteSpace())
            {
                throw new ArgumentException(Resources.ERR_ConfigRegistryIncorrectParms);
            }

            // NOTE: I'm commenting this out for now unless we have a need for
            // it but I think having the ability to set registry keys from an
            // application for a configuration is probably not a good idea.
            ////Registry.SetValue(splitKey[0], splitKey[1], value);
        }

        /// <inheritdoc />
        protected override bool TryGetInt<T>(string key, out T value)
        {
            string[] splitKey = key.Split(StringSplitOptions.RemoveEmptyEntries, ':');
            if (splitKey.Length < 2 || splitKey[0].IsNullOrWhiteSpace() || splitKey[1].IsNullOrWhiteSpace())
            {
                throw new ArgumentException(Resources.ERR_ConfigRegistryIncorrectParms);
            }

            try
            {
                Registry.GetValue(splitKey[0], splitKey[1], null).TryCast(out value);
                return value != null;
            }
            catch (Exception)
            {
                value = default(T);
                return false;
            }
        }

        #endregion Methods
    }
}