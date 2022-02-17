namespace Oliviann.Tests.Runtime.Serialization.Formatters.Binary
{
    #region Usings

    using System;
    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;
    using Oliviann.Runtime.Serialization.Formatters.Binary;
    using Common.TestObjects.Xml;
    using Xunit;

    #endregion Usings

    [Trait("Category", "CI")]
    public class BinaryFormatterExtensionsTests
    {
        /// <summary>
        /// Verifies calling the extension method with a null formatter returns
        /// the default type value.
        /// </summary>
        [Fact]
        public void Deserialize_NullFormatter()
        {
            MemoryStream stream = this.CreateSerializedStream();
            BinaryFormatter formatter = null;
            Book result = formatter.Deserialize<Book>(stream);
            stream.Dispose();

            Assert.Null(result);
        }

        /// <summary>
        /// Verifies deserializing a stream back to an object has the correct
        /// values.
        /// </summary>
        [Fact]
        public void Deserialize_Formatter()
        {
            MemoryStream stream = this.CreateSerializedStream();
            var formatter = new BinaryFormatter();
            Book result = formatter.Deserialize<Book>(stream);
            stream.Dispose();

            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("Gambardella, Matthew", result.Author);
            Assert.StartsWith("An in-depth look at cr", result.Description);
            Assert.Equal("XML Developer's Guide", result.Title);
            Assert.Equal(2000U, result.Year);
        }

        /// <summary>
        /// Verifies calling the method with a null stream throws a argument
        /// null exception.
        /// </summary>
        [Fact]
        public void Deserialize_NullStream()
        {
            MemoryStream stream = null;
            var formatter = new BinaryFormatter();
            Assert.Throws<ArgumentNullException>(() => formatter.Deserialize<Book>(stream));
        }

        #region Helper Methods

        private MemoryStream CreateSerializedStream()
        {
            var book = new Book
            {
                Id = 1,
                Author = "Gambardella, Matthew",
                Description = "An in-depth look at creating applications with XML.",
                Title = "XML Developer's Guide",
                Year = 2000
            };

            var stream = new MemoryStream();
            new BinaryFormatter().Serialize(stream, book);
            stream.Position = 0;
            return stream;
        }

        #endregion Helper Methods
    }
}