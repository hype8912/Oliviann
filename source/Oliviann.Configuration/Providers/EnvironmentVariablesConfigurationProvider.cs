namespace Oliviann.Configuration.Providers
{
    #region Usings

    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    #endregion Usings

    /// <summary>
    /// Represents an environment variable configuration provider.
    /// </summary>
    public class EnvironmentVariablesConfigurationProvider : ConfigurationProvider
    {
        #region Fields

        /// <summary>
        /// The variable prefix;
        /// </summary>
        private readonly string prefix;

        #endregion Fields

        #region Constructor/Destructor

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="EnvironmentVariablesConfigurationProvider" /> class.
        /// </summary>
        public EnvironmentVariablesConfigurationProvider() : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="EnvironmentVariablesConfigurationProvider"/> class.
        /// </summary>
        /// <param name="prefix">The variables prefix.</param>
        public EnvironmentVariablesConfigurationProvider(string prefix) : base("EnvironmentVariable")
        {
            this.prefix = prefix ?? string.Empty;
            if (this.prefix.Length > 0)
            {
                this.Name = "EnvironmentVariable_" + this.prefix;
            }
        }

        #endregion Constructor/Destructor

        #region Methods

        /// <inheritdoc />
        public override IEnumerable<string> GetChildKeys()
        {
#pragma warning disable DE0006
            return Environment.GetEnvironmentVariables()
                .Cast<DictionaryEntry>()
                .Where(entry => ((string)entry.Key).StartsWith(this.prefix, StringComparison.OrdinalIgnoreCase))
                .Select(entry => (string)entry.Key);
#pragma warning restore DE0006
        }

        /// <inheritdoc />
        public override void Load()
        {
            // Not used. We are going to go directly to the environment
            // variables instead of loading them into memory.
        }

        /// <inheritdoc />
        protected override void SetInt(string key, string value)
        {
            // NOTE: I'm commenting this out for now unless we have a need for
            // it but I think having the ability to set environment variables
            // from an application for a configuration is probably not a good
            // idea.
            ////Environment.SetEnvironmentVariable(key, value);
        }

        /// <inheritdoc />
        protected override bool TryGetInt<T>(string key, out T value)
        {
            if (this.GetChildKeys().Contains(key))
            {
                Environment.GetEnvironmentVariable(key).TryCast(out value);
                return true;
            }

            value = default(T);
            return false;
        }

        #endregion Methods
    }
}