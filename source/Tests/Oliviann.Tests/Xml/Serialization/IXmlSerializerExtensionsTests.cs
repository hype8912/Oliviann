namespace Oliviann.Tests.Xml.Serialization
{
    #region Usings

    using System;
    using System.IO;
    using Oliviann.Common.TestObjects.Xml;
    using Oliviann.IO;
    using Oliviann.Testing.Fixtures;
    using Oliviann.Xml.Serialization;
    using Xunit;

    #endregion Usings

    [Trait("Category", "CI")]
    public class IXmlSerializerExtensionsTests : IClassFixture<PathCleanupFixture>
    {
        #region Fields

        private readonly string GoodXmlFilePath;

        private readonly PathCleanupFixture fixture;

        #endregion Fields

        #region Constructor/Destructor

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="BasicXmlSerializerTests"/> class.
        /// </summary>
        /// <param name="fixture">The current fixture.</param>
        public IXmlSerializerExtensionsTests(PathCleanupFixture fixture)
        {
            this.fixture = fixture;
            this.GoodXmlFilePath = Path.Combine(this.fixture.CurrentDirectory, @"TestObjects\Books1.xml");
            if (!File.Exists(this.GoodXmlFilePath))
            {
                File.WriteAllText(this.GoodXmlFilePath, Common.TestObjects.Properties.Resources.Books1);
            }
        }

        #endregion Constructor/Destructor

        #region File

        [Fact]
        public void DeserializeFileTest_NullSerializer()
        {
            IXmlSerializer<Catalog> serializer = null;
            Assert.Throws<ArgumentNullException>(() => serializer.DeserializeFile(this.GoodXmlFilePath));
        }

        [Fact]
        public void DeserializeFileTest_ValidSerializer()
        {
            IXmlSerializer<Catalog> serializer = new BasicXmlSerializer<Catalog>();
            Catalog result = serializer.DeserializeFile(this.GoodXmlFilePath);

            Assert.NotNull(result);
            Assert.Equal(4, result.Books.Count);
        }

        #endregion

        #region Stream

        [Fact]
        public void DeserializeStreamTest_NullSerializer()
        {
            IXmlSerializer<Catalog> serializer = null;
            Assert.Throws<ArgumentNullException>(() => serializer.DeserializeStream(null));
        }

        [Fact]
        public void DeserializeStreamTest_ValidSerializer()
        {
            IXmlSerializer<Catalog> serializer = new BasicXmlSerializer<Catalog>();
            Catalog result;
            using (var stream = new FileInfo(this.GoodXmlFilePath).Open(FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                result = serializer.DeserializeStream(stream);
            }

            Assert.NotNull(result);
            Assert.Equal(4, result.Books.Count);
        }

        #endregion

        #region String

        [Fact]
        public void DeserializeStringTest_NullSerializer()
        {
            IXmlSerializer<Catalog> serializer = null;
            Assert.Throws<ArgumentNullException>(() => serializer.DeserializeString(null));
        }

        [Fact]
        public void DeserializeStringTest_ValidSerializer()
        {
            string data = FileHelpers.ReadContents(this.GoodXmlFilePath);

            IXmlSerializer<Catalog> serializer = new BasicXmlSerializer<Catalog>();
            Catalog result = serializer.DeserializeString(data);

            Assert.NotNull(result);
            Assert.Equal(4, result.Books.Count);
        }

        #endregion
    }
}