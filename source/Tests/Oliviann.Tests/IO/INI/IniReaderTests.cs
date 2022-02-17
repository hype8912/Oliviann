namespace Oliviann.Tests.IO.INI
{
    #region Usings

    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Runtime.InteropServices;
    using Oliviann.IO;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Testing.Fixtures;
    using Xunit;
    using Assert = Xunit.Assert;

    #endregion Usings

    [Trait("Category", "CI")]
    [DeploymentItem(@"TestObjects\INI\AutoImports.ini")]
    [DeploymentItem(@"TestObjects\INI\BadSection.ini")]
    [DeploymentItem(@"TestObjects\INI\Multiline.ini")]
    [DeploymentItem(@"TestObjects\INI\Normal.ini")]
    public class IniReaderTests : IClassFixture<PathCleanupFixture>
    {
        #region Fields

        private readonly PathCleanupFixture fixture;
        private readonly string AutoImportsIniFilePath;
        private readonly string BadSectionIniFilePath;
        private readonly string MultilineIniFilePath;
        private readonly string NormalIniFilePath;

        #endregion Fields

        #region Constructor/Destructor

        /// <summary>
        /// Initializes a new instance of the <see cref="IniReaderTests"/>
        /// class.
        /// </summary>
        /// <param name="fixture">The current fixture.</param>
        public IniReaderTests(PathCleanupFixture fixture)
        {
            this.fixture = fixture;
            this.AutoImportsIniFilePath = this.fixture.CurrentDirectory + @"\TestObjects\INI\AutoImports.ini";
            this.BadSectionIniFilePath = this.fixture.CurrentDirectory + @"\TestObjects\INI\BadSection.ini";
            this.MultilineIniFilePath = this.fixture.CurrentDirectory + @"\TestObjects\INI\Multiline.ini";
            this.NormalIniFilePath = this.fixture.CurrentDirectory + @"\TestObjects\INI\Normal.ini";
        }

        #endregion Constructor/Destructor

        #region Constructor Tests

        [Theory]
        [InlineData(null, typeof(ArgumentNullException))]
        [InlineData("", typeof(FileNotFoundException))]
        [InlineData(@"F:\Hello\World\no_file.ini", typeof(FileNotFoundException))]
        public void IniReader_cTorTest_Exceptions(string input, Type expectedExceptionType)
        {
            Assert.Throws(expectedExceptionType, () => new IniReader(input));
        }

        [Fact]
        public void IniReader_ctorTest_GoodFilePath()
        {
            var ini = new IniReader(this.AutoImportsIniFilePath);
            Assert.Equal(this.AutoImportsIniFilePath, ini.FilePath);
            ini.Close();
        }

        #endregion Constructor Tests

        #region ReadInteger Tests

        [Theory]
        [InlineData(null, null, 0)]
        [InlineData(null, "Port", 0)]
        [InlineData("User", null, 0)]
        [InlineData("User", "ghere", 0)]
        [InlineData("Email", "Bcc", 0)]
        [InlineData("Email", "Port", 25)]
        public void ReadIntegerTest(string section, string key, int expectedResult)
        {
            var reader = new IniReader(this.AutoImportsIniFilePath);
            int result = reader.ReadInteger(section, key);
            reader.Close();

            Assert.Equal(expectedResult, result);
        }

        #endregion ReadInteger Tests

        #region SectionKeyExists Tests

        [Fact]
        public void SectionKeyExistsTest_MissingKey()
        {
            var reader = new IniReader(this.AutoImportsIniFilePath);
            bool result = reader.SectionKeyExists("Email", "glogs");

            Assert.False(result);
        }

        [Fact]
        public void SectionKeyExistsTest_MatchingKey()
        {
            var reader = new IniReader(this.AutoImportsIniFilePath);
            bool result = reader.SectionKeyExists("Email", "Server");

            Assert.True(result);
        }

        [Fact]
        public void SectionKeyExistsTest_NullReader()
        {
            IniReader reader = null;
            Assert.Throws<ArgumentNullException>(() => reader.SectionKeyExists("Email", "Server"));
        }

        #endregion SectionKeyExists Tests

        #region ReadString Tests

        [Theory]
        [InlineData(null, null, "User")]
        [InlineData(null, "Port", "User")]
        [InlineData("User", null, "ghere")]
        [InlineData("Email", "Port", "25")]
        [InlineData("Email", "Bcc", "")]
        [InlineData("Prod", "commpath", @"Z:\tools\scripts\PS_Scripts\Prod\Import_Graphics\C130Prod_Imports.bat")]
        [InlineData("Parse", "pimport", @"\\ietm-fwb-fil1\drs_c130prod\c130prod_server\data\quill21_db\parse_data\c130parse\import-\\ietm-fwb-fil1\drs_c130prod\c130prod_server\data\quill21_db\parse_data\c130parse\import-\\ietm-fwb-fil1\drs_c130prod\c130prod_server\data\quill21_db\parse_data\c130parse\import-\\ietm-fwb-fil1\drs_c130prod\c130prod_server\data\quill21_db\parse_data\c130parse\import")]
        public void ReadStringTests_Ansi(string section, string key, string expectedResult)
        {
            var reader = new IniReader(this.AutoImportsIniFilePath);
            string result = reader.ReadString(section, key, null);
            reader.Close();

            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData(null, null, "User")]
        [InlineData(null, "Port", "User")]
        [InlineData("User", null, "ghere")]
        [InlineData("Email", "Port", "25")]
        [InlineData("Email", "Bcc", "")]
        [InlineData("Prod", "commpath", @"Z:\tools\scripts\PS_Scripts\Prod\Import_Graphics\C130Prod_Imports.bat")]
        [InlineData("Parse", "pimport", @"\\ietm-fwb-fil1\drs_c130prod\c130prod_server\data\quill21_db\parse_data\c130parse\import-\\ietm-fwb-fil1\drs_c130prod\c130prod_server\data\quill21_db\parse_data\c130parse\import-\\ietm-fwb-fil1\drs_c130prod\c130prod_server\data\quill21_db\parse_data\c130parse\import-\\ietm-fwb-fil1\drs_c130prod\c130prod_server\data\quill21_db\parse_data\c130parse\import")]
        public void ReadStringTests_Unicode(string section, string key, string expectedResult)
        {
            var reader = new IniReader(this.AutoImportsIniFilePath);
            string result = reader.ReadString(section, key, null, CharSet.Unicode);
            reader.Close();

            Assert.Equal(expectedResult, result);
        }

        #endregion ReadString Tests

        #region ReadSection Tests

        /// <summary>
        /// Verifies retrieving a whole section returns the correct results.
        /// </summary>
        [Fact]
        public void ReadSectionTest_AutoImports_Email()
        {
            var reader = new IniReader(this.AutoImportsIniFilePath);
            IEnumerable<string> keysAndValues = reader.ReadSection("Email");
            reader.Close();

            Assert.NotNull(keysAndValues);
            Assert.Equal(9, keysAndValues.Count());

            string[] expectedKeys = { "To=", "Cc=", "Bcc=", "From=TECHNICALSERVICES@oliviann.com", "User=", "Pass=", "Port=25", "Server=relay.oliviann.com", "Debug=False" };
            Assert.False(keysAndValues.Except(expectedKeys).Any(), "A key is missing.");
        }

        /// <summary>
        /// Verifies retrieving a whole section with an empty string input
        /// returns the correct results.
        /// </summary>
        [Fact]
        public void ReadSectionTest_AutoImports_EmptyString()
        {
            var reader = new IniReader(this.AutoImportsIniFilePath);
            IEnumerable<string> keysAndValues = reader.ReadSection(string.Empty);
            reader.Close();

            Assert.NotNull(keysAndValues);
            Assert.Single(keysAndValues);

            string[] expectedKeys = { string.Empty };
            Assert.False(keysAndValues.Except(expectedKeys).Any(), "A key is missing.");
        }

        /// <summary>
        /// Verifies retrieving a whole section with a null input returns the
        /// correct results.
        /// </summary>
        [Fact]
        public void ReadSectionTest_AutoImports_Null()
        {
            var reader = new IniReader(this.AutoImportsIniFilePath);
            IEnumerable<string> keysAndValues = reader.ReadSection(null);
            reader.Close();

            Assert.NotNull(keysAndValues);
            Assert.Single(keysAndValues);

            string[] expectedKeys = { string.Empty };
            Assert.False(keysAndValues.Except(expectedKeys).Any(), "A key is missing.");
        }

        /// <summary>
        /// Verifies retrieving a whole section with an empty string input
        /// returns the correct results.
        /// </summary>
        [Fact]
        public void ReadSectionTest_AutoImports_UnknownSection()
        {
            var reader = new IniReader(this.AutoImportsIniFilePath);
            IEnumerable<string> keysAndValues = reader.ReadSection("TacoBell");
            reader.Close();

            Assert.NotNull(keysAndValues);
            Assert.Single(keysAndValues);

            string[] expectedKeys = { string.Empty };
            Assert.False(keysAndValues.Except(expectedKeys).Any(), "A key is missing.");
        }

        #endregion ReadSection Tests

        #region ReadSectionNames Tests

        /// <summary>
        /// Verifies all the section names are returned correctly.
        /// </summary>
        [Fact]
        public void ReadSectionNamesTest_AutoImports()
        {
            var reader = new IniReader(this.AutoImportsIniFilePath);
            IEnumerable<string> sections = reader.ReadSectionNames();
            reader.Close();

            Assert.NotNull(sections);
            Assert.Equal(5, sections.Count());

            string[] expectedSections = { "User", "Parse", "Prod", "Demo", "Email" };
            Assert.False(sections.Except(expectedSections).Any(), "A section is missing.");
        }

        #endregion ReadSectionNames Tests

        #region GetSectionKeyNames Tests

        /// <summary>
        /// Verifies all the keys in a section returned correctly.
        /// </summary>
        [Fact]
        public void GetSectionKeyNames_AutoImports_User()
        {
            var reader = new IniReader(this.AutoImportsIniFilePath);
            IEnumerable<string> keys = reader.GetSectionKeyNames("User");
            reader.Close();

            Assert.NotNull(keys);
            Assert.Equal(14, keys.Count());

            string[] expectedKeys = { "ghere", "phere", "gparse", "pparse", "gparsefail", "pparsefail", "gparsepass", "pparsepass", "gprod", "pprod", "gprodfail", "pprodfail", "gprodpass", "pprodpass" };
            Assert.False(keys.Except(expectedKeys).Any(), "A key is missing.");
        }

        /// <summary>
        /// Verifies all the keys in a section returned correctly.
        /// </summary>
        [Fact]
        public void GetSectionKeyNames_AutoImports_Email()
        {
            var reader = new IniReader(this.AutoImportsIniFilePath);
            IEnumerable<string> keys = reader.GetSectionKeyNames("Email");
            reader.Close();

            Assert.NotNull(keys);
            Assert.Equal(9, keys.Count());

            string[] expectedKeys = { "To", "Cc", "Bcc", "From", "User", "Pass", "Port", "Server", "Debug" };
            Assert.False(keys.Except(expectedKeys).Any(), "A key is missing.");
        }

        /// <summary>
        /// Verifies the keys returned when a null is passed in actually returns
        /// all the sections.
        /// </summary>
        [Fact]
        public void GetSectionKeyNames_AutoImports_Null()
        {
            var reader = new IniReader(this.AutoImportsIniFilePath);
            IEnumerable<string> keys = reader.GetSectionKeyNames(null);
            reader.Close();

            Assert.NotNull(keys);
            Assert.Equal(5, keys.Count());

            string[] expectedKeys = { "User", "Parse", "Prod", "Demo", "Email" };
            Assert.False(keys.Except(expectedKeys).Any(), "A key is missing.");
        }

        /// <summary>
        /// Verifies the keys returned when an empty string is passed in
        /// actually returns an empty string.
        /// </summary>
        [Fact]
        public void GetSectionKeyNames_AutoImports_EmptyString()
        {
            var reader = new IniReader(this.AutoImportsIniFilePath);
            IEnumerable<string> keys = reader.GetSectionKeyNames(string.Empty);
            reader.Close();

            Assert.NotNull(keys);
            Assert.Single(keys);

            string[] expectedKeys = { string.Empty };
            Assert.False(keys.Except(expectedKeys).Any(), "A key is missing.");
        }

        /// <summary>
        /// Verifies the keys returned when an unknown key is passed in actually
        /// returns an empty string.
        /// </summary>
        [Fact]
        public void GetSectionKeyNames_AutoImports_UnknownKey()
        {
            var reader = new IniReader(this.AutoImportsIniFilePath);
            IEnumerable<string> keys = reader.GetSectionKeyNames("TacoBell");
            reader.Close();

            Assert.NotNull(keys);
            Assert.Single(keys);

            string[] expectedKeys = { string.Empty };
            Assert.False(keys.Except(expectedKeys).Any(), "A key is missing.");
        }

        #endregion GetSectionKeyNames Tests
    }
}