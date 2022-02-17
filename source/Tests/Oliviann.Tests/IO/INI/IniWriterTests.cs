namespace Oliviann.Tests.IO.INI
{
    #region Usings

    using System;
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
    public class IniWriterTests : IClassFixture<PathCleanupFixture>
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
        /// Initializes a new instance of the <see cref="IniWriterTests"/>
        /// class.
        /// </summary>
        /// <param name="fixture">The current fixture.</param>
        public IniWriterTests(PathCleanupFixture fixture)
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
        [InlineData("", typeof(ArgumentNullException))]
        [InlineData(@"F:\Hello\World\no_file.ini", typeof(FileNotFoundException))]
        public void IniWriter_cTorTest_Exceptions(string input, Type expectedExceptionType)
        {
            Assert.Throws(expectedExceptionType, () => new IniWriter(input));
        }

        [Fact]
        public void IniWriter_ctorTest_GoodFilePath()
        {
            var ini = new IniWriter(this.AutoImportsIniFilePath);

            Assert.Equal(this.AutoImportsIniFilePath, ini.FilePath);
            Assert.True(ini.AutoFlush);
            Assert.Equal(this.AutoImportsIniFilePath, ini.FilePath);

            ini.Dispose();
        }

        #endregion Constructor Tests

        #region WriteString Tests

        [Fact]
        public void WriteStringTest_ValidStringAnsi()
        {
            var writer = new IniWriter(this.NormalIniFilePath);
            int writeResult = writer.WriteString("WriteTest", "String1", "HelloOliviann");

            var reader = new IniReader(this.NormalIniFilePath);
            Assert.True(reader.SectionKeyExists("WriteTest", "String1"));
            string readResult = reader.ReadString("WriteTest", "String1", string.Empty);

            Assert.Equal("HelloOliviann", readResult);

            writer.Dispose();
            reader.Dispose();
        }

        [Fact]
        public void WriteStringTest_ValidStringUnicode()
        {
            var writer = new IniWriter(this.NormalIniFilePath);
            int writeResult = writer.WriteString("WriteTest", "String2", "I love tacos", CharSet.Unicode);

            var reader = new IniReader(this.NormalIniFilePath);
            Assert.True(reader.SectionKeyExists("WriteTest", "String2"));
            string readResult = reader.ReadString("WriteTest", "String2", string.Empty);

            Assert.Equal("I love tacos", readResult);

            writer.Dispose();
            reader.Dispose();
        }

        #endregion WriteString Tests

        #region WriteInt Test

        [Theory]
        [InlineData("Int1", 1)]
        [InlineData("Int2", int.MinValue)]
        [InlineData("Int3", 0)]
        [InlineData("Int4", int.MaxValue)]
        public void WriteIntTest_ValidInt(string keyName, int value)
        {
            var writer = new IniWriter(this.NormalIniFilePath);
            int writeResult = writer.WriteInt("WriteTest", keyName, value);

            var reader = new IniReader(this.NormalIniFilePath);
            Assert.True(reader.SectionKeyExists("WriteTest", keyName));
            int readResult = reader.ReadInteger("WriteTest", keyName, -17);

            Assert.Equal(value, readResult);

            writer.Dispose();
            reader.Dispose();
        }

        #endregion WriteInt Test

        #region DeleteKey Tests

        [Fact]
        public void DeleteKeyTest_NullWriter()
        {
            IniWriter writer = null;
            Assert.Throws<ArgumentNullException>(() => writer.DeleteKey("WriteTest", "Int1"));
        }

        [Fact]
        public void DeleteKeyTest_NullSection()
        {
            var writer = new IniWriter(this.NormalIniFilePath);
            Assert.Throws<ArgumentNullException>(() => writer.DeleteKey(null, "Int1"));

            writer.Dispose();
        }

        [Fact]
        public void DeleteKeyTest_NullKey()
        {
            var writer = new IniWriter(this.NormalIniFilePath);
            Assert.Throws<ArgumentNullException>(() => writer.DeleteKey("WriteTest", null));

            writer.Dispose();
        }

        [Fact]
        public void DeleteKeyTest_ValidString()
        {
            var writer = new IniWriter(this.NormalIniFilePath);
            int writeResult = writer.WriteString("WriteTest", "DeleteKey1", "Delete me now!");

            var reader = new IniReader(this.NormalIniFilePath);
            Assert.True(reader.SectionKeyExists("WriteTest", "DeleteKey1"));

            writer.DeleteKey("WriteTest", "DeleteKey1");
            Assert.False(reader.SectionKeyExists("WriteTest", "DeleteKey1"));

            writer.Dispose();
            reader.Dispose();
        }

        #endregion DeleteKey Tests

        #region DeleteSection Tests

        [Fact]
        public void DeleteSectionTest_NullWriter()
        {
            IniWriter writer = null;
            Assert.Throws<ArgumentNullException>(() => writer.DeleteSection("WriteTest"));
        }

        [Fact]
        public void DeleteSectionTest_NullSection()
        {
            var writer = new IniWriter(this.NormalIniFilePath);
            Assert.Throws<ArgumentNullException>(() => writer.DeleteSection(null));

            writer.Dispose();
        }

        [Fact]
        public void DeleteSectionTest_ValidSection()
        {
            var writer = new IniWriter(this.NormalIniFilePath);
            writer.WriteString("DeleteSectionTest", "Key1", "Taco");
            writer.WriteString("DeleteSectionTest", "Key2", string.Empty);
            writer.WriteString("DeleteSectionTest", "Key3", "Hallo Mellow");
            writer.WriteString("DeleteSectionTest", "Key4", "Pizzzzaaaaa!");

            var reader = new IniReader(this.NormalIniFilePath);
            var sections = reader.ReadSectionNames();
            Assert.Contains(sections, s => s == "DeleteSectionTest");

            writer.DeleteSection("DeleteSectionTest");
            sections = reader.ReadSectionNames();
            Assert.DoesNotContain(sections, s => s == "DeleteSectionTest");

            reader.Dispose();
            writer.Dispose();
        }

        #endregion DeleteSection Tests
    }
}