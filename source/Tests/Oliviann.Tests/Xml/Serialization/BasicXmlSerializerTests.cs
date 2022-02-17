namespace Oliviann.Tests.Xml.Serialization
{
    #region Usings

    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Runtime.Serialization;
    using System.Xml;
    using Oliviann.IO;
    using Oliviann.Xml.Serialization;
    using Common.TestObjects.Xml;
    using Testing.Fixtures;
    using Xunit;
    using Assert = Xunit.Assert;

    #endregion Usings

    [Trait("Category", "CI")]
    public class BasicXmlSerializerTests : IClassFixture<PathCleanupFixture>
    {
        #region Fields

        private readonly string GoodXmlFilePath;

        private readonly string MissingXmlFilePath;

        private readonly PathCleanupFixture fixture;

        #endregion Fields

        #region Constructor/Destructor

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="BasicXmlSerializerTests"/> class.
        /// </summary>
        /// <param name="fixture">The current fixture.</param>
        public BasicXmlSerializerTests(PathCleanupFixture fixture)
        {
            this.fixture = fixture;
            this.GoodXmlFilePath = Path.Combine(this.fixture.CurrentDirectory, @"TestObjects\Books1.xml");
            if (!File.Exists(this.GoodXmlFilePath))
            {
                File.WriteAllText(this.GoodXmlFilePath, Common.TestObjects.Properties.Resources.Books1);
            }

            this.MissingXmlFilePath = Path.Combine(this.fixture.CurrentDirectory, "MissingFile.xml");
        }

        #endregion Constructor/Destructor

        #region File

        [Fact]
        public void DeserializeFileTest_NullPath()
        {
            bool actionCalled = false;
            Action<Catalog> act = c => actionCalled = true;

            IXmlSerializer<Catalog> serializer = new BasicXmlSerializer<Catalog>();
            Assert.Throws<ArgumentNullException>(() => serializer.DeserializeFile(null, act));
        }

        [Fact]
        public void DeserializeFileTest_MissingFilePath()
        {
            bool actionCalled = false;
            Action<Catalog> act = c => actionCalled = true;

            IXmlSerializer<Catalog> serializer = new BasicXmlSerializer<Catalog>();
            Assert.Throws<FileNotFoundException>(() => serializer.DeserializeFile(this.MissingXmlFilePath, act));
        }

        [Fact]
        public void DeserializeFileTest_NullAction()
        {
            IXmlSerializer<Catalog> serializer = new BasicXmlSerializer<Catalog>();
            Assert.Throws<SerializationException>(() => serializer.DeserializeFile(this.GoodXmlFilePath, null));
        }

        [Fact]
        public void DeserializeFileTest_GoodExecution()
        {
            bool actionCalled = false;
            Catalog cat = null;
            Action<Catalog> act = c =>
                {
                    actionCalled = true;
                    cat = c;
                };

            IXmlSerializer<Catalog> serializer = new BasicXmlSerializer<Catalog>();
            serializer.DeserializeFile(this.GoodXmlFilePath, act);

            Assert.True(actionCalled, "Action not executed.");
            Assert.NotNull(cat);
            Assert.Equal(4, cat.Books.Count);
        }

        [Fact]
        public void SerializeFileTest_NullPath()
        {
            IXmlSerializer<Catalog> serializer = new BasicXmlSerializer<Catalog>();
            Assert.Throws<ArgumentNullException>(() => serializer.SerializeFile(null, null));
        }

        [Fact]
        public void SerializeFileTest_EmptyPath()
        {
            IXmlSerializer<Catalog> serializer = new BasicXmlSerializer<Catalog>();
            Assert.Throws<SerializationException>(() => serializer.SerializeFile(string.Empty, null));
        }

        [Fact]
        public void SerializeFileTest_NullData()
        {
            string path = this.GoodXmlFilePath.Replace("Books1.xml", "Books1-1.xml");
            IXmlSerializer<Catalog> serializer = new BasicXmlSerializer<Catalog>();
            serializer.SerializeFile(path, null);

            Assert.True(File.Exists(path), "File does not exist.");
            string contents = FileHelpers.ReadContents(path);

            string expectedResult = @"<?xml version=""1.0"" encoding=""utf-8""?>
<catalog d1p1:nil=""true"" xmlns:d1p1=""http://www.w3.org/2001/XMLSchema-instance"" />";
            Assert.Equal(expectedResult, contents);
        }

        #endregion File

        #region Stream

        [Fact]
        public void SerializeStreamTest_NullData()
        {
            IXmlSerializer<Catalog> serializer = new BasicXmlSerializer<Catalog>();
            MemoryStream st = serializer.SerializeStream(null);
            Assert.NotNull(st);

            st.Position = 0;
            string result = st.ReadToEnd();
            st.DisposeSafe();

            string expectedResult =
                @"<?xml version=""1.0"" encoding=""utf-8""?><catalog p1:nil=""true"" xmlns:p1=""http://www.w3.org/2001/XMLSchema-instance"" />";
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void SerializeStreamTest_BadData()
        {
            Catalog cat = null;
            IXmlSerializer<Catalog> serializer = new BasicXmlSerializer<Catalog>();
            serializer.DeserializeFile(this.GoodXmlFilePath, c => cat = c);
            var settings = new XmlWriterSettings { ConformanceLevel = ConformanceLevel.Fragment };

            Assert.Throws<SerializationException>(() => serializer.SerializeStream(cat, settings));
        }

        [Fact]
        public void SerializeStreamTest_GoodData()
        {
            Catalog cat = null;
            IXmlSerializer<Catalog> serializer = new BasicXmlSerializer<Catalog>();
            serializer.DeserializeFile(this.GoodXmlFilePath, c => cat = c);

            MemoryStream st = serializer.SerializeStream(cat);
            Assert.NotNull(st);

            st.Position = 0;
            string result = st.ReadToEnd();
            st.DisposeSafe();

            string expectedResult =
                @"<?xml version=""1.0"" encoding=""utf-8""?><catalog><books><book id=""1""><author>Gambardella, Matthew</author><description>An in-depth look at creating applications with XML.</description><title>XML Developer's Guide</title><year>2000</year></book><book id=""2""><author>Ralls, Kim</author><description>A former architect battles corporate zombies, an evil sorceress, and her own childhood to become queen of the world.</description><title>Midnight Rain</title><year>2000</year></book><book id=""3""><author>Knorr, Stefan</author><description>An anthology of horror stories about roaches, centipedes, scorpions  and other insects.</description><title>Creepy Crawlies</title><year>2000</year></book><book id=""4""><author>Corets, Eva</author><description>In post-apocalypse England, the mysterious agent known only as Oberon helps to create a new life for the inhabitants of London. Sequel to Maeve Ascendant.</description><title>Oberon's Legacy</title><year>2001</year></book></books></catalog>";
            Assert.Equal(expectedResult, result);
        }

        #endregion Stream

        #region String

        [Fact]
        public void DeserializeStringTest_NullData()
        {
            Catalog cat = null;
            Action<Catalog> act = c =>
                {
                    cat = c;
                };

            IXmlSerializer<Catalog> serializer = new BasicXmlSerializer<Catalog>();
            Assert.Throws<SerializationException>(() => serializer.DeserializeString(null, act));
        }

        [Fact]
        public void DeserializeStringTest_EmptyData()
        {
            Catalog cat = null;
            Action<Catalog> act = c =>
                {
                    cat = c;
                };

            IXmlSerializer<Catalog> serializer = new BasicXmlSerializer<Catalog>();
            Assert.Throws<SerializationException>(() => serializer.DeserializeString(string.Empty, act));
        }

        [Fact]
        public void DeserializeStringTest_NullAction()
        {
            IXmlSerializer<Catalog> serializer = new BasicXmlSerializer<Catalog>();
            Action<object> act = null;
            Assert.Throws<ArgumentNullException>(() => serializer.DeserializeString(null, act));
        }

        [Fact]
        public void DeserializeStringTest_GoodExecution()
        {
            bool actionCalled = false;
            Catalog cat = null;
            Action<Catalog> act = c =>
                {
                    actionCalled = true;
                    cat = c;
                };

            string data = FileHelpers.ReadContents(this.GoodXmlFilePath);
            IXmlSerializer<Catalog> serializer = new BasicXmlSerializer<Catalog>();
            serializer.DeserializeString(data, act);

            Assert.True(actionCalled, "Action not executed.");
            Assert.NotNull(cat);
            Assert.Equal(4, cat.Books.Count);
        }

        [Fact]
        public void SerializeStringTest_NullData()
        {
            IXmlSerializer<Catalog> serializer = new BasicXmlSerializer<Catalog>();
            string result = serializer.SerializeString(null);

            Assert.False(result.IsNullOrEmpty(), "Serialized data is incorrect.");

            string expectedResult = @"<?xml version=""1.0"" encoding=""utf-8""?>
<catalog p1:nil=""true"" xmlns:p1=""http://www.w3.org/2001/XMLSchema-instance"" />";
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void SerializeStringTest_GoodData()
        {
            Catalog cat = null;

            IXmlSerializer<Catalog> serializer = new BasicXmlSerializer<Catalog>();
            serializer.DeserializeFile(this.GoodXmlFilePath, c => cat = c);
            Assert.NotNull(cat);

            string result = serializer.SerializeString(cat);
            Assert.False(result.IsNullOrEmpty(), "Serialized data is incorrect.");
            Assert.Contains("Ralls, Kim", result);
            Assert.Contains("Corets, Eva", result);
        }

        [Fact]
        public void SerializeStringTest_WithProcessingInstructions()
        {
            Catalog cat = null;

            IXmlSerializer<Catalog> serializer = new BasicXmlSerializer<Catalog>();
            serializer.DeserializeFile(this.GoodXmlFilePath, c => cat = c);
            Assert.NotNull(cat);

            var inst = new Dictionary<string, string>
                {
                    { "xml", "version='1.0' encoding='UTF-16'" }
                };
            string result = serializer.SerializeString(cat, inst);
            Trace.WriteLine(result);
            Assert.False(result.IsNullOrEmpty(), "Serialized data is incorrect.");
            Assert.Contains("encoding='UTF-16'", result);
            Assert.Contains("Ralls, Kim", result);
            Assert.Contains("Corets, Eva", result);
        }

        #endregion String
    }
}