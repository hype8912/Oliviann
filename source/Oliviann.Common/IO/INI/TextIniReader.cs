namespace Oliviann.IO
{
    #region Usings

    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Runtime.InteropServices;
    using Oliviann.Collections.Generic;

    #endregion Usings

    /// <summary>
    /// Represents a managed only text based INI file reader.
    /// </summary>
    public class TextIniReader : IniReader
    {
        #region Fields

        /// <summary>
        /// The collection of comment characters.
        /// </summary>
        private static readonly char[] commentCharacters = { ';', '/', '#' };

        #endregion Fields

        #region Constructor/Destructor

        /// <summary>
        /// Initializes a new instance of the <see cref="TextIniReader"/> class.
        /// </summary>
        /// <param name="iniFilePath">The ini file path.</param>
        public TextIniReader(string iniFilePath) : base(iniFilePath)
        {
            this.ReadFile();
        }

        #endregion Constructor/Destructor

        #region Properties

        /// <summary>
        /// Gets the container object of the INI file.
        /// </summary>
        /// <value>The INI file container.</value>
        protected FileContainer Container { get; private set; }

        #endregion Properties

        /// <summary>
        /// Reads the INI value for a specific group and key.
        /// </summary>
        /// <param name="section">The INI group section.</param>
        /// <param name="keyName">Name of the key.</param>
        /// <param name="defaultValue">The default value to be returned if no
        /// section and key are found.</param>
        /// <param name="encoding">Not Used.</param>
        /// <returns>
        /// A string value read from the INI file using specified encoding.
        /// </returns>
        public override string ReadString(string section, string keyName, string defaultValue, CharSet encoding = CharSet.Ansi)
        {
            if (section.IsNullOrWhiteSpace())
            {
                return null;
            }

            Section sec = this.Container.Sections.FirstOrDefault(s => s.Name.EqualsOrdinalIgnoreCase(section));
            if (sec == null)
            {
                return defaultValue;
            }

            if (keyName.IsNullOrWhiteSpace())
            {
                return null;
            }

            KeyValuePair<string, string> pair = sec.Properties.FirstOrDefault(p => p.Key.EqualsOrdinalIgnoreCase(keyName));
            return pair.Value;
        }

        /// <summary>
        /// Retrieves the whole section inside the INI file.
        /// </summary>
        /// <param name="section">The INI group section.</param>
        /// <returns>An INI <see cref="Section"/> object containing all the
        /// properties.</returns>
        public new Section ReadSection(string section)
        {
            Section sec = this.Container.Sections.FirstOrDefault(s => s.Name.EqualsOrdinalIgnoreCase(section));
            return sec;
        }

        /// <summary>
        /// Retrieves all the section names inside the INI file.
        /// </summary>
        /// <returns>
        /// A collection of section names.
        /// </returns>
        public override IReadOnlyCollection<string> ReadSectionNames()
        {
            IEnumerable<string> items = this.Container.Sections.Select(s => s.Name);
            return items.ToListHelper();
        }

        /// <summary>
        /// Retrieves all the section key names for a specified section group.
        /// </summary>
        /// <param name="section">The INI group section.</param>
        /// <returns>A generic string list of section keys.</returns>
        public override IReadOnlyCollection<string> GetSectionKeyNames(string section)
        {
            Section sec = this.ReadSection(section);
            if (sec == null)
            {
                return CollectionHelpers.CreateReadOnlyCollection<string>();
            }

            IEnumerable<string> items = sec.Properties.Select(p => p.Key);
            return items.ToListHelper();
        }

        #region Loader

        /// <summary>
        /// Wraps the load ini file events to create a single entry point for
        /// loading the ini file into memory.
        /// </summary>
        /// <exception cref="FileNotFoundException">Exception thrown when the
        /// provided ini file is not found.</exception>
        protected void ReadFile()
        {
            string path = this.FilePath;
            if (path.Contains('$'))
            {
                path = this.FilePath.ConvertAdminFilePath();
                ADP.CheckFileNotFound(this.FilePath);
            }

            this.LoadFileDataToObjects(path);
        }

        /// <summary>
        /// Loads the file data to objects.
        /// </summary>
        /// <param name="iniFilePath">The INI file path.</param>
        protected void LoadFileDataToObjects(string iniFilePath)
        {
            FileStream fs = null;

            try
            {
                fs = new FileStream(iniFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                using (var reader = new StreamReader(fs))
                {
                    this.Container = new FileContainer();
                    bool isInvalidSection = false;
                    string currentSectionName = @"Default";

                    while (reader.Peek() != -1)
                    {
                        string line = reader.ReadLine();
                        if (line.IsNullOrWhiteSpace())
                        {
                            // Ignore any blank lines.
                            continue;
                        }

                        line = line.Trim();
                        if (!ProcessComments(ref line))
                        {
                            continue;
                        }

                        // Process valid Section group item.
                        int leftBracketIndex = line.IndexOf('[');
                        if (leftBracketIndex >= 0)
                        {
                            int rightBracketIndex = line.IndexOf(']', leftBracketIndex);
                            if (rightBracketIndex >= 0)
                            {
                                isInvalidSection = !this.ParseSectionName(line, leftBracketIndex, rightBracketIndex, out currentSectionName);
                                if (!isInvalidSection)
                                {
                                    this.Container.Sections.Add(new Section(currentSectionName));
                                }

                                continue;
                            }
                        }

                        // Process Key/Value item
                        if (!isInvalidSection && this.ParseKeyValuePair(line, out string key, out string value))
                        {
                            Section parent = this.ReadSectionOrAdd(currentSectionName);
                            parent.AddProperty(key, value);
                        }
                    }
                }
            }
            finally
            {
                fs.DisposeSafe();
            }
        }

        #endregion Loader

        #region Helpers

        /// <summary>
        /// Processes the comments for a single read line.
        /// </summary>
        /// <param name="line">A reference to the line string.</param>
        /// <returns>True if line should continue; otherwise, false to move to
        /// the next line.
        /// </returns>
        private static bool ProcessComments(ref string line)
        {
            // Processes line starts with comment characters.
            if (commentCharacters.Contains(line[0]))
            {
                return false;
            }

            // Processes in line comment characters.
            int index = line.IndexOf(';');
            if (index >= 0)
            {
                line = line.Substring(0, index).Trim();
            }

            return true;
        }

        /// <summary>
        /// Retrieves the whole section inside the INI file. A new section will
        /// be created and added to the container for the specified name if no
        /// section already exists.
        /// </summary>
        /// <param name="section">The INI group section.</param>
        /// <returns>An INI <see cref="Section"/> object containing all the
        /// properties.</returns>
        private Section ReadSectionOrAdd(string section)
        {
            int count = 0;
        start:
            Section sec = this.ReadSection(section);
            if (sec == null && count == 0)
            {
                count += 1;
                this.Container.Sections.Add(new Section(section));
                goto start;
            }

            return sec;
        }

        /// <summary>
        /// Parses out the name of the section from the text string.
        /// </summary>
        /// <param name="text">The text containing the section name.</param>
        /// <param name="leftBracketIndex">Index of the left bracket.</param>
        /// <param name="rightBracketIndex">Index of the right bracket.</param>
        /// <param name="sectionName">Name of the section.</param>
        /// <returns>True if the section was valid; otherwise, false.</returns>
        /// <remarks>We can do this with pointers but it only saves about 2ns.
        /// </remarks>
        private bool ParseSectionName(string text, int leftBracketIndex, int rightBracketIndex, out string sectionName)
        {
            for (int i = leftBracketIndex + 1; i < rightBracketIndex; i++)
            {
                if ((text[i] >= 48 && text[i] <= 57) ||
                    (text[i] >= 65 && text[i] <= 90) ||
                    (text[i] >= 97 && text[i] <= 122))
                {
                    continue;
                }

                sectionName = null;
                return false;
            }

            sectionName = text.Substring(leftBracketIndex + 1, rightBracketIndex - leftBracketIndex - 1);
            return true;
        }

        /// <summary>
        /// Parses out the key and value for a text string.
        /// </summary>
        /// <param name="text">The text containing the key and value.</param>
        /// <param name="key">The key retrieved from the text.</param>
        /// <param name="value">The value retrieved from the text.</param>
        /// <returns>True if the key contained valid characters and was able to
        /// parsed; otherwise, false.</returns>
        /// <remarks>We can do this with pointers but it only saves about 3ns.
        /// </remarks>
        private bool ParseKeyValuePair(string text, out string key, out string value)
        {
            int equalIndex = text.IndexOf('=');
            if (equalIndex < 1)
            {
                key = null;
                value = null;
                return false;
            }

            for (int i = 0; i < equalIndex; i++)
            {
                if ((text[i] >= 48 && text[i] <= 57) ||
                    (text[i] >= 65 && text[i] <= 90) ||
                    (text[i] >= 97 && text[i] <= 122))
                {
                    continue;
                }

                key = null;
                value = null;
                return false;
            }

            key = text.Substring(0, equalIndex);
            value = text.Substring(equalIndex + 1, text.Length - equalIndex - 1);
            return true;
        }

        #endregion Helpers
    }
}