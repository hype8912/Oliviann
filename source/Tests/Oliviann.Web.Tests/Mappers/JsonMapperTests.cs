namespace Oliviann.Web.Tests.Mappers
{
    #region Usings

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Oliviann.Collections.Generic;
    using Oliviann.Common.TestObjects.Xml;
    using Oliviann.Web.Mappers;
    using Xunit;

    #endregion Usings

    [Trait("Category", "CI")]
    public class JsonMapperTests
    {
        #region Single Object Mapping Tests

        /// <summary>
        /// Verifies mapping a null source object to the same type object
        /// returns a null.
        /// </summary>
        [Fact]
        public void JsonMapperTest_SingleObjectNullSourceToSameType()
        {
            Book bk = null;

            Book result = JsonMapper.PropertyMap<Book, Book>(bk);
            Assert.Null(result);
        }

        /// <summary>
        /// Verifies mapping a populated simple source object of one type to a
        /// similar different type.
        /// </summary>
        [Fact]
        public void JsonMapperTest_SingleObjectSourceToDiffType()
        {
            // Arrange
            var book1 = new Book
                        {
                            Author = "Author Name",
                            Description = "Describe the book",
                            Id = 123,
                            Title = "Book Title",
                            Year = 2017
                        };

            Book2 book2 = JsonMapper.PropertyMap<Book, Book2>(book1);

            Assert.Equal(book1.Author, book2.Author);
            Assert.Equal(book1.Description, book2.Description);
            Assert.Equal(book1.Id, book2.Id);
            Assert.Equal(book1.Title, book2.Title);
            Assert.Equal(book1.Year, book2.Year);
        }

        /// <summary>
        /// Verifies an object with different serialization strings won't match
        /// values.
        /// </summary>
        [Fact]
        public void JsonMapperTest_SingleObjectSourceToNonMatchingType()
        {
            var book = new Book
                       {
                           Author = "author name",
                           Description = "describe the book",
                           Id = 123,
                           Title = "book title",
                           Year = 2017
                       };

            Novel novel = JsonMapper.PropertyMap<Book, Novel>(book);

            Assert.NotEqual(book.Author, novel.NovelAuthor);
            Assert.NotEqual(book.Description, novel.NovelDescription);
            Assert.NotEqual(book.Id, novel.NovelId);
            Assert.NotEqual(book.Title, novel.NovelTitle);
            Assert.NotEqual(book.Year.ToInt32(), novel.NovelYear);
            Assert.Null(novel.NovelAuthor);
            Assert.Null(novel.NovelDescription);
            Assert.Equal(0, novel.NovelId);
            Assert.Null(novel.NovelTitle);
            Assert.Equal(0, novel.NovelYear);
        }

        #endregion Single Object Mapping Tests

        #region Multi-Object Mapping Tests

        /// <summary>
        /// Verifies mapping a null source object to the same type object
        /// returns a null.
        /// </summary>
        [Fact]
        public void JsonMapperTest_MultiObjectNullSourceToSameType()
        {
            IEnumerable<Book> books = null;

            IEnumerable<Book> result = JsonMapper.PropertyMap<Book, Book>(books);
            Assert.Null(result);
        }

        /// <summary>
        /// Verifies mapping an empty collection returns an empty collection.
        /// </summary>
        [Fact]
        public void JsonMapperTest_MultiObjectEmptySourceToSameType()
        {
            IEnumerable<Book> books = Enumerable.Empty<Book>();
            IEnumerable<Book> result = JsonMapper.PropertyMap<Book, Book>(books);

            Assert.NotNull(result);
            Assert.Empty(result);
        }

        /// <summary>
        /// Verifies mapping a collection of objects of type to different type.
        /// </summary>
        [Fact]
        public void JsonMapperTest_MultiObjectSimpleSourceToDiffType()
        {
            IReadOnlyCollection<Book> books = this.GenerateBooks(100);
            IEnumerable<Book2> contracts = JsonMapper.PropertyMap<Book, Book2>(books);

            Assert.NotNull(contracts);
            Assert.Equal(books.Count, contracts.Count());
            Assert.NotNull(contracts.First());
        }

        /// <summary>
        /// Verifies mapping a populated collection of simple source objects of
        /// one type to a similar different type.
        /// </summary>
        [Fact]
        public void JsonMapperTest_MultiObjectSourceToDiffType()
        {
            IReadOnlyCollection<Book> books = this.GenerateBooks(2);
            IEnumerable<Book2> books2 = JsonMapper.PropertyMap<Book, Book2>(books);

            Assert.NotNull(books2);
            Assert.Equal(books.Count, books2.Count());

            Book book1 = books.OrderBy(b => b.Id).First();
            Book2 book2 = books2.OrderBy(b => b.Id).First();
            Assert.Equal(book1.Author, book2.Author);
            Assert.Equal(book1.Description, book2.Description);
            Assert.Equal(book1.Id, book2.Id);
            Assert.Equal(book1.Title, book2.Title);
            Assert.Equal(book1.Year, book2.Year);
        }

        #endregion Multi-Object Mapping Tests

        private IReadOnlyCollection<Book> GenerateBooks(int maxCount)
        {
            const string RandChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789 ";
            var rand = new Random(Guid.NewGuid().GetHashCode());

            Func<int, Book> bookGen =
                id =>
                    new Book
                    {
                        Author = StringHelpers.GenerateRandomString(rand.Next(5, 20), RandChars),
                        Id = id,
                        Description = StringHelpers.GenerateRandomString(rand.Next(10, 50), RandChars),
                        Title = StringHelpers.GenerateRandomString(rand.Next(10, 35), RandChars),
                        Year = rand.Next(1900, 2017).ToUInt32()
                    };

            var books = new List<Book>();
            Enumerable.Range(1, maxCount).ForEach(i => books.Add(bookGen(i)));
            return books;
        }
    }
}