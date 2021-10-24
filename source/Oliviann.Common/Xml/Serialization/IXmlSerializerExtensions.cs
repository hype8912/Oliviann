namespace Oliviann.Xml.Serialization
{
    #region Usings

    using System.IO;
    using System.Xml;

    #endregion

    /// <summary>
    /// Represents a collection of commonly used extension methods for extending
    /// <see cref="IXmlSerializer{T}"/> objects.
    /// </summary>
    public static class IXmlSerializerExtensions
    {
        /// <summary>
        /// Deserializes XML file data into type T for the specified
        /// <paramref name="filePath"/>.
        /// </summary>
        /// <typeparam name="T">The type object for serializing. Must be of type
        /// class.</typeparam>
        /// <param name="serializer">The serializer instance.</param>
        /// <param name="filePath">The file path of the XML file.</param>
        /// <returns>The result of the deserializer.</returns>
        public static T DeserializeFile<T>(this IXmlSerializer<T> serializer, string filePath) where T : class
        {
            ADP.CheckArgumentNull(serializer, nameof(serializer));

            T result = null;
            serializer.DeserializeFile(filePath, item => result = item);
            return result;
        }

        /// <summary>
        /// Deserializes an XML stream into type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type object for serializing. Must be of type
        /// class.</typeparam>
        /// <param name="serializer">The serializer instance.</param>
        /// <param name="dataStream">The data stream to be deserialized.</param>
        /// <returns>The result of the deserializer.</returns>
        public static T DeserializeStream<T>(this IXmlSerializer<T> serializer, Stream dataStream) where T : class
        {
            ADP.CheckArgumentNull(serializer, nameof(serializer));

            T result = null;
            serializer.DeserializeStream(dataStream, item => result = item);
            return result;
        }

        /// <summary>
        /// Deserializes the specified xml string of data into the correct type
        /// model.
        /// </summary>
        /// <typeparam name="T">The type object for serializing. Must be of type
        /// class.</typeparam>
        /// <param name="serializer">The serializer instance.</param>
        /// <param name="data">The xml data string.</param>
        /// <returns>The result of the deserializer.</returns>
        public static T DeserializeString<T>(this IXmlSerializer<T> serializer, string data) where T : class
        {
            ADP.CheckArgumentNull(serializer, nameof(serializer));

            T result = null;
            var settings = new XmlReaderSettings();
#if !NETSTANDARD1_3
            settings.XmlResolver = null;
#endif
            serializer.DeserializeString(data, item => result = item, settings);
            return result;
        }
    }
}