namespace Oliviann.Tests.Runtime.Serialization
{
    #region Usings

    using System;
    using System.Runtime.Serialization;
    using System.Text;
    using Oliviann.Runtime.Serialization;
    using Common.TestObjects.Xml;
    using Xunit;

    #endregion Usings

    [Trait("Category", "CI")]
    public class DataContractSerializerExtensionsTests
    {
        #region DeserializeString Tests

        /// <summary>
        /// Verifies an argument null exception is thrown when a null serializer
        /// is passed in.
        /// </summary>
        [Fact]
        public void DeserializeStringTest_StringNullSerializer()
        {
            DataContractSerializer serializer = null;
            string data =
                "<Book xmlns=\"http://schemas.datacontract.org/2004/07/Oliviann.Common.TestObjects.Xml\" xmlns:i=\"http://www.w3.org/2001/XMLSchema-instance\"><author>NavPress</author><description i:nil=\"true\"/><id>1</id><title>How to find the one</title><year>1996</year></Book>";

            Assert.Throws<ArgumentNullException>(() => serializer.DeserializeString<Book>(data));
        }

        /// <summary>
        /// Verifies a null value is returned when a null object is
        /// deserialized.
        /// </summary>
        [Fact]
        public void DeserializeStringTest_NullStringData()
        {
            var serializer = new DataContractSerializer(typeof(Book));
            string data = null;

            Book result = serializer.DeserializeString<Book>(data);

            Assert.Null(result);
        }

        /// <summary>
        /// Verifies an object is deserialized correctly.
        /// </summary>
        [Fact]
        public void DeserializeStringTest_ValidStringData()
        {
            var serializer = new DataContractSerializer(typeof(Book));
            var bk = new Book { Author = "NavPress", Id = 1, Title = "How to find the one", Year = 1996 };
            string bkStr = serializer.SerializeString(bk);

            Assert.NotNull(bkStr);

            Book result = serializer.DeserializeString<Book>(bkStr);

            Assert.Equal(bk.Author, result.Author);
            Assert.Equal(bk.Id, result.Id);
            Assert.Equal(bk.Title, result.Title);
            Assert.Equal(bk.Year, result.Year);
            Assert.Equal(bk.Description, result.Description);
        }

        /// <summary>
        /// Verifies an argument null exception is thrown when a null serializer
        /// is passed in.
        /// </summary>
        [Fact]
        public void DeserializeStringTest_NullSerializer()
        {
            DataContractSerializer serializer = null;
            byte[] data =
                Encoding.UTF8.GetBytes(
                    "<Book xmlns=\"http://schemas.datacontract.org/2004/07/Oliviann.Common.TestObjects.Xml\" xmlns:i=\"http://www.w3.org/2001/XMLSchema-instance\"><author>NavPress</author><description i:nil=\"true\"/><id>1</id><title>How to find the one</title><year>1996</year></Book>");

            Assert.Throws<ArgumentNullException>(() => serializer.DeserializeString<Book>(data));
        }

        /// <summary>
        /// Verifies a null value is returned when a null object is
        /// deserialized.
        /// </summary>
        [Fact]
        public void DeserializeStringTest_NullByteData()
        {
            var serializer = new DataContractSerializer(typeof(Book));
            byte[] data = null;

            Book result = serializer.DeserializeString<Book>(data);

            Assert.Null(result);
        }

        /// <summary>
        /// Verifies an object is deserialized correctly.
        /// </summary>
        [Fact]
        public void DeserializeStringTest_ValidByteData()
        {
            var serializer = new DataContractSerializer(typeof(Book));
            var bk = new Book { Author = "NavPress", Id = 1, Title = "How to find the one", Year = 1996 };
            string bkStr = serializer.SerializeString(bk);

            Assert.NotNull(bkStr);

            Book result = serializer.DeserializeString<Book>(Encoding.UTF8.GetBytes(bkStr));

            Assert.Equal(bk.Author, result.Author);
            Assert.Equal(bk.Id, result.Id);
            Assert.Equal(bk.Title, result.Title);
            Assert.Equal(bk.Year, result.Year);
            Assert.Equal(bk.Description, result.Description);
        }

        #endregion DeserializeString Tests

        #region SerializeString Tests

        /// <summary>
        /// Verifies an argument null exception is thrown when a null serializer
        /// is passed in.
        /// </summary>
        [Fact]
        public void SerializeStringTest_NullSerializer()
        {
            DataContractSerializer serializer = null;
            var bk = new Book { Author = "NavPress", Id = 1, Title = "How to find the one", Year = 1996 };

            Assert.Throws<ArgumentNullException>(() => serializer.SerializeString(bk));
        }

        /// <summary>
        /// Verifies an empty string is returned when a null object is
        /// serialized.
        /// </summary>
        [Fact]
        public void SerializeStringTest_NullData()
        {
            var serializer = new DataContractSerializer(typeof(Book));
            Book bk = null;

            string result = serializer.SerializeString(bk);
            Assert.Equal(
                "<Book i:nil=\"true\" xmlns=\"http://schemas.datacontract.org/2004/07/Oliviann.Common.TestObjects.Xml\" xmlns:i=\"http://www.w3.org/2001/XMLSchema-instance\"/>",
                result);
        }

        /// <summary>
        /// Verifies a object is serialized correctly.
        /// </summary>
        [Fact]
        public void SerializeStringTest_ValidObject()
        {
            var serializer = new DataContractSerializer(typeof(Book));
            var bk = new Book { Author = "NavPress", Id = 1, Title = "How to find the one", Year = 1996 };

            string result = serializer.SerializeString(bk);

            Assert.Equal(
                "<Book xmlns=\"http://schemas.datacontract.org/2004/07/Oliviann.Common.TestObjects.Xml\" xmlns:i=\"http://www.w3.org/2001/XMLSchema-instance\"><author>NavPress</author><description i:nil=\"true\"/><id>1</id><title>How to find the one</title><year>1996</year></Book>",
                result);
        }

        #endregion SerializeString Tests
    }
}