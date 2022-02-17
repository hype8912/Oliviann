namespace Oliviann.Web.Tests.Json
{
    #region Usings

    using System;
    using System.IO;
    using Oliviann.Common.TestObjects.Xml;
    using Oliviann.Web.Json;
    using Oliviann.Web.Tests.TestObjects;
    using Newtonsoft.Json;
    using Xunit;

    #endregion Usings

    [Trait("Category", "CI")]
    public class SingleValueArrayConverterTests
    {
        #region Fields

        private const string WellFormedJson = "{ id : 1, customer : 'Joe Black', items : [ { id : 1, description:'One', unit_price: 1.00, quantity: 1}, { id : 2, description:'Two', unit_price: 2.00, quantity: 2}, { id : 3, description:'Three', unit_price: 3.00, quantity: 3} ] }";

        private const string PoorlyFormedJson = "{ id : 1, customer : 'Joe Black', items : { id : 1, description:'One', unit_price: 1.00, quantity: 1} }";

        #endregion Fields

        /// <summary>
        /// Verifies calling the write json method throws a not implemented
        /// exception.
        /// </summary>
        [Fact]
        public void SingleValueArrayConverterTest_WriteJson()
        {
            var newBook = new SingleValueArrayConverter<Book2>();

            var writer = new StreamWriter("./test.txt");
            var jsonWriter = new JsonTextWriter(writer);
            var book = new Book2 { Title = "New Book" };
            var serializer = new JsonSerializer();

            Assert.Throws<NotImplementedException>(() => newBook.WriteJson(jsonWriter, book, serializer));
        }

        /// <summary>
        /// Verifies a json string with good form deserializes correctly.
        /// </summary>
        [Fact]
        public void SingleValueArrayConverterTest_ReadJson_WellForm()
        {
            var settings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore, DateFormatHandling = DateFormatHandling.IsoDateFormat };
            Order order = JsonConvert.DeserializeObject<Order>(WellFormedJson, settings);

            Assert.NotNull(order);
            Assert.NotNull(order.Items);
            Assert.Equal(3, order.Items.Count);
        }

        /// <summary>
        /// Verifies a json string with bad form deserializes correctly.
        /// </summary>
        [Fact]
        public void SingleValueArrayConverterTest_ReadJson_BadFormWithItems()
        {
            var settings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore, DateFormatHandling = DateFormatHandling.IsoDateFormat };
            Order order = JsonConvert.DeserializeObject<Order>(PoorlyFormedJson, settings);

            Assert.NotNull(order);
            Assert.NotNull(order.Items);
            Assert.Single(order.Items);
        }

        /// <summary>
        /// Verifies can convert method always returns true.
        /// </summary>
        [Fact]
        public void SingleValueArrayConverterTest_CanConvert()
        {
            var newBook = new SingleValueArrayConverter<Book2>();
            bool result = newBook.CanConvert(typeof(Book2));

            Assert.True(result);
        }
    }
}