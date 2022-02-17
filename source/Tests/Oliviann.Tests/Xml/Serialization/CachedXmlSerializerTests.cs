namespace Oliviann.Tests.Xml.Serialization
{
    #region Usings

    using System;
    using System.IO;
    using System.Runtime.Caching;
    using System.Runtime.Serialization;
    using System.Threading;
    using Oliviann.IO;
    using Oliviann.Xml.Serialization;
    using Common.TestObjects.Xml;
    using Testing.Fixtures;
    using Xunit;
    using Assert = Xunit.Assert;

    #endregion Usings

    [Trait("Category", "CI")]
    public class CachedXmlSerializerTests : IClassFixture<PathCleanupFixture>
    {
        #region Fields

        private readonly string GoodXmlFilePath;

        private readonly PathCleanupFixture fixture;

        #endregion Fields

        #region Constructor/Destructor

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="CachedXmlSerializerTests"/> class.
        /// </summary>
        /// <param name="fixture">The current fixture.</param>
        public CachedXmlSerializerTests(PathCleanupFixture fixture)
        {
            this.fixture = fixture;
            this.GoodXmlFilePath = Path.Combine(this.fixture.CurrentDirectory, @"TestObjects\Books1.xml");
            if (!File.Exists(this.GoodXmlFilePath))
            {
                File.WriteAllText(this.GoodXmlFilePath, Common.TestObjects.Properties.Resources.Books1);
            }
        }

        #endregion Constructor/Destructor

        [Fact]
        public void SerializeFileTest_NullPath()
        {
            var serializer = new CachedXmlSerializer<Catalog>();
            Assert.Throws<ArgumentNullException>(() => serializer.SerializeFile(null, null));
        }

        [Fact]
        public void SerializeFileTest_EmptyPath()
        {
            var serializer = new CachedXmlSerializer<Catalog>();
            Assert.Throws<SerializationException>(() => serializer.SerializeFile(string.Empty, null));
        }

        [Fact]
        public void SerializeFileTest_NullData()
        {
            string path = this.GoodXmlFilePath.Replace("Books1.xml", "Books2-1.xml");
            var serializer = new CachedXmlSerializer<Catalog>();
            serializer.SerializeFile(path, null);

            Assert.True(File.Exists(path), "File does not exist.");
            this.fixture.DeletePaths.Add(path);

            string contents = FileHelpers.ReadContents(path);
            Assert.True(
                contents == @"<?xml version=""1.0"" encoding=""utf-8""?>
<catalog xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xsi:nil=""true"" />",
                "Incorrect file output\n\tRESULT: [{0}]".FormatWith(contents));
        }

        [Fact]
        public void SerializeFileTest_DataWithPolicy()
        {
            bool itemRemovedCallbackExecuted = false;

            var policy = new CacheItemPolicy
                {
                    AbsoluteExpiration = ObjectCache.InfiniteAbsoluteExpiration,
                    SlidingExpiration = ObjectCache.NoSlidingExpiration,
                    Priority = CacheItemPriority.Default,
                    RemovedCallback = arguments => itemRemovedCallbackExecuted = true
                };

            Catalog cat = null;
            var serializer = new CachedXmlSerializer<Catalog>(policy);
            serializer.DeserializeFile(this.GoodXmlFilePath, c => cat = c);
            Assert.NotNull(cat);

            string path = this.GoodXmlFilePath.Replace("Books1.xml", "Books2-1.xml");
            serializer.SerializeFile(path, cat);

            Assert.True(File.Exists(path), "File does not exist.");
            this.fixture.DeletePaths.Add(path);

            string contents = FileHelpers.ReadContents(path);
            Assert.Contains("Knorr, Stefan", contents);
        }

        [Fact]
        public void SerializeFileTest_NullPolicy()
        {
            Catalog cat = null;
            var serializer = new CachedXmlSerializer<Catalog>(null);
            serializer.DeserializeFile(this.GoodXmlFilePath, c => cat = c);
            Assert.NotNull(cat);

            string path = this.GoodXmlFilePath.Replace("Books1.xml", "Books2-1.xml");
            serializer.SerializeFile(path, cat);

            Assert.True(File.Exists(path), "File does not exist.");
            this.fixture.DeletePaths.Add(path);

            string contents = FileHelpers.ReadContents(path);
            Assert.Contains("Knorr, Stefan", contents);
        }

        [Fact]
        public void RemoveSerializerTest_NullData()
        {
            string path = this.GoodXmlFilePath.Replace("Books1.xml", "Books2-2.xml");
            var serializer = new CachedXmlSerializer<Catalog>();
            serializer.SerializeFile(path, null);
            Assert.True(MemoryCache.Default.Contains(typeof(Catalog).FullName));

            Assert.True(File.Exists(path), "File does not exist.");
            this.fixture.DeletePaths.Add(path);

            string contents = FileHelpers.ReadContents(path);
            Assert.True(
                contents == @"<?xml version=""1.0"" encoding=""utf-8""?>
<catalog xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xsi:nil=""true"" />",
                "Incorrect file output\n\tRESULT: [{0}]".FormatWith(contents));

            serializer.RemoveSerializer();
            Thread.Sleep(1000);
            Assert.False(MemoryCache.Default.Contains(typeof(Catalog).FullName));
        }
    }
}