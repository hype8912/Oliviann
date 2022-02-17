namespace Oliviann.Web.Mappers
{
    #region Usings

    using System.Collections.Generic;
    using System.IO;
    using Newtonsoft.Json;

    #endregion Usings

    /// <summary>
    /// Represents a JSON mapper class for mapping the values of the properties
    /// from a source class to the properties of a similar destination class
    /// using JSON serialization.
    /// </summary>
    /// <remarks>The JSON mapper was found to be approximately 2X to 3X times
    /// faster than using reflection to map the property values. Can use
    /// <see cref="P:DataMemberAttribute.Name"/> to map properties of different
    /// names. Recommend using TinyMapper as a long term solution for mapping
    /// objects. TinyMapper was found to be approximately 100X times faster than
    /// using the JSON Mapper while scaling much better on memory.</remarks>
    public static class JsonMapper
    {
        #region Methods

        /// <summary>
        /// Copies all the values of the properties of the source object to the
        /// properties in the destination object.
        /// </summary>
        /// <typeparam name="TIn">The type of object to be copied from.
        /// </typeparam>
        /// <typeparam name="TOut">The type of object to be copied to.
        /// </typeparam>
        /// <param name="source">The source object instance to be read.</param>
        /// <returns>A new <typeparamref name="TOut"/> instance with the
        /// matching property values.</returns>
        public static TOut PropertyMap<TIn, TOut>(TIn source)
        {
            if (source == null)
            {
                return default;
            }

            TOut result;
            var stream = new MemoryStream();
            using (var writer = new StreamWriter(stream))
            using (var reader = new StreamReader(stream))
            {
                var serializer = new JsonSerializer();
                serializer.Serialize(writer, source);
                writer.Flush();

                stream.Seek(0, SeekOrigin.Begin);
                result = (TOut)serializer.Deserialize(reader, typeof(TOut));
            }

            return result;
        }

        /// <summary>
        /// Copies all the values of the properties of the source collection
        /// objects to the properties of a new destination object.
        /// </summary>
        /// <typeparam name="TIn">The type of object to be copied from.
        /// </typeparam>
        /// <typeparam name="TOut">The type of object to be copied to.
        /// </typeparam>
        /// <param name="source">The source object collection for values to be
        /// copied from.</param>
        /// <returns>
        /// A new collection of <typeparamref name="TOut" /> objects with
        /// matching property values.
        /// </returns>
        public static IEnumerable<TOut> PropertyMap<TIn, TOut>(IEnumerable<TIn> source) =>
            PropertyMap<IEnumerable<TIn>, IEnumerable<TOut>>(source);

        #endregion Methods
    }
}