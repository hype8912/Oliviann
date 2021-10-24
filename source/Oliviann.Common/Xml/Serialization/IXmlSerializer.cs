namespace Oliviann.Xml.Serialization
{
    #region Usings

    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Xml;

    #endregion

    /// <summary>
    /// Represents an implementation of an Xml serializer for a specified type.
    /// </summary>
    /// <typeparam name="T">The type object for serializing. Must be of type
    /// class.</typeparam>
    public interface IXmlSerializer<T> where T : class
    {
        /// <summary>
        /// Deserializes XML file data into type T for the specified
        /// <paramref name="filePath"/>.
        /// </summary>
        /// <param name="filePath">The file path of the XML file.</param>
        /// <param name="actionHandler">The action handler for assigning the
        /// serialized object to the delegate object.</param>
        void DeserializeFile(string filePath, Action<T> actionHandler);

        /// <summary>
        /// Serializes the model data for type T to the specified
        /// <paramref name="filePath"/>.
        /// </summary>
        /// <param name="filePath">The file path of the XML file.</param>
        /// <param name="serializableData">The serializable data model object.
        /// </param>
        void SerializeFile(string filePath, T serializableData);

        /// <summary>
        /// Deserializes an XML stream into type <typeparamref name="T"/>.
        /// </summary>
        /// <param name="dataStream">The data stream to be deserialized.</param>
        /// <param name="actionHandler">The action handler for assigning the
        /// serialized object to the delegate object.</param>
        void DeserializeStream(Stream dataStream, Action<T> actionHandler);

        /// <summary>
        /// Serializes the model data for <typeparamref name="T"/> to a stream.
        /// </summary>
        /// <param name="serializableData">The serializable data model object.
        /// </param>
        /// <param name="settings">The XML writer settings. Optional.</param>
        /// <returns>A stream of serialized xml data.</returns>
        MemoryStream SerializeStream(T serializableData, XmlWriterSettings settings = null);

        /// <summary>
        /// Deserializes the specified xml string of data into the correct type
        /// model.
        /// </summary>
        /// <param name="data">The xml data string.</param>
        /// <param name="actionHandler">The action handler for assigning and
        /// performing any fixes for closure.</param>
        /// <param name="settings">Optional. The xml reader settings to be used.
        /// </param>
        void DeserializeString(string data, Action<T> actionHandler, XmlReaderSettings settings = null);

        /// <summary>
        /// Serializes the specified type model of data into an xml data string.
        /// </summary>
        /// <param name="serializableData">The serializable data.</param>
        /// <param name="processorInstructions">The processor instructions to be
        /// included in the xml.</param>
        /// <returns>
        /// A string of serialized xml data.
        /// </returns>
        string SerializeString(T serializableData, IEnumerable<KeyValuePair<string, string>> processorInstructions = null);
    }
}