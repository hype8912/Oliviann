namespace Oliviann.Configuration.Providers
{
    #region Usings

    using System;
    using System.Collections.Generic;
    using System.IO;
    using Oliviann.IO;
    using Oliviann.Properties;

    #endregion Usings

    /// <summary>
    /// Represents an INI file configuration provider.
    /// </summary>
    public class IniConfigurationProvider : ConfigurationProvider
    {
        #region Fields

        /// <summary>
        /// The INI file path.
        /// </summary>
        private readonly string iniFilePath;

        /// <summary>
        /// The INI text reader.
        /// </summary>
        private TextIniReader iniReader;

        #endregion Fields

        #region Constructor/Destructor

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="IniConfigurationProvider" /> class.
        /// </summary>
        /// <param name="filePath">The INI file path.</param>
        public IniConfigurationProvider(string filePath) : base("IniFile")
        {
            this.iniFilePath = filePath;
            if (!this.iniFilePath.IsNullOrWhiteSpace())
            {
                this.Name = "IniFile:" + Path.GetFileNameWithoutExtension(this.iniFilePath);
            }
        }

        #endregion Constructor/Destructor

        #region Methods

        /// <inheritdoc />
        public override IEnumerable<string> GetChildKeys()
        {
            return this.iniReader.ReadSectionNames();
        }

        /// <inheritdoc />
        public override void Load()
        {
            ADP.CheckArgumentNullOrEmpty(this.iniFilePath, "filePath");
            this.iniReader = new TextIniReader(this.iniFilePath);
        }

        /// <inheritdoc />
        protected override void SetInt(string key, string value)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        protected override bool TryGetInt<T>(string key, out T value)
        {
            string[] splitKey = key.Split(StringSplitOptions.RemoveEmptyEntries, ':');
            if (splitKey.Length < 2 || splitKey[0].IsNullOrWhiteSpace() || splitKey[1].IsNullOrWhiteSpace())
            {
                throw new ArgumentException(Resources.ERR_ConfigIniIncorrectParms);
            }

            this.iniReader.ReadString(splitKey[0], splitKey[1], null).TryCast(out value);
            return value != null;
        }

        #endregion Methods
    }
}