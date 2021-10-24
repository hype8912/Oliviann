namespace Oliviann.Xml.Serialization
{
    #region Usings

    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Runtime.Serialization;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;
    using Oliviann.Collections.Generic;
    using Oliviann.Properties;

    #endregion Usings

    /// <summary>
    /// Represents a basic class for serializing data to/from a file or string.
    /// </summary>
    /// <typeparam name="T">The type object for serializing. Must be of type
    /// class.</typeparam>
    public class BasicXmlSerializer<T> : IXmlSerializer<T> where T : class
    {
        #region File

        /// <summary>
        /// Deserializes XML file data into type T for the specified
        /// <paramref name="filePath"/>.
        /// </summary>
        /// <param name="filePath">The file path of the XML file.</param>
        /// <param name="actionHandler">The action handler for assigning the
        /// serialized object to the delegate object.</param>
        /// <exception cref="FileNotFoundException">The specified file path
        /// cannot be found.</exception>
        /// <exception cref="SerializationException">An error occurred
        /// deserializing file. See inner exception for more details.
        /// </exception>
        public void DeserializeFile(string filePath, Action<T> actionHandler)
        {
            ADP.CheckArgumentNull(filePath, nameof(filePath));
            ADP.CheckFileNotFound(filePath, string.Empty);
            Stream fileInfoStream = null;

            try
            {
                fileInfoStream = new FileInfo(filePath).Open(FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                this.DeserializeStream(fileInfoStream, actionHandler);
            }
            catch (Exception inner)
            {
                var ex = new SerializationException(Resources.ERR_DeserializingInnerEx.FormatWith("file"), inner);
                ex.Data.Add("FilePath", filePath);
                Trace.TraceError(ex.ToString());
                throw ex;
            }
            finally
            {
                fileInfoStream.DisposeSafe();
            }
        }

        /// <summary>
        /// Serializes the model data for type T to the specified
        /// <paramref name="filePath"/>.
        /// </summary>
        /// <param name="filePath">The file path of the XML file.</param>
        /// <param name="serializableData">The serializable data model object.
        /// </param>
        /// <exception cref="SerializationException">An error occurred
        /// serializing file. See inner exception for more details.
        /// </exception>
        public void SerializeFile(string filePath, T serializableData)
        {
            ADP.CheckArgumentNull(filePath, nameof(filePath));
            FileStream stream = null;
            StreamWriter writer = null;
            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);

            try
            {
                stream = File.Open(filePath, FileMode.Create);
                writer = new StreamWriter(stream) { AutoFlush = false };
                this.GetSerializer().Serialize(writer, serializableData, namespaces);
                writer.Flush();
            }
            catch (Exception inner)
            {
                var ex = new SerializationException(Resources.ERR_SerializingInnerEx.FormatWith("file"), inner);
                ex.Data.Add("FilePath", filePath);
                Trace.TraceError(ex.ToString());
                throw ex;
            }
            finally
            {
                writer.DisposeSafe();
                stream.DisposeSafe();
            }
        }

        #endregion File

        #region Stream

        /// <summary>
        /// Deserializes an XML stream into type <typeparamref name="T"/>.
        /// </summary>
        /// <param name="dataStream">The data stream to be deserialized.</param>
        /// <param name="actionHandler">The action handler for assigning the
        /// serialized object to the delegate object.</param>
        /// <exception cref="ArgumentNullException">The data stream object
        /// cannot be null.</exception>
        public void DeserializeStream(Stream dataStream, Action<T> actionHandler)
        {
            ADP.CheckArgumentNull(dataStream, nameof(dataStream));
            ADP.CheckArgumentNull(actionHandler, nameof(actionHandler));

            XmlSerializer serializer = this.GetSerializer();
            actionHandler((T)serializer.Deserialize(dataStream));
        }

        /// <summary>
        /// Serializes the model data for <typeparamref name="T"/> to a stream.
        /// </summary>
        /// <param name="serializableData">The serializable data model object.
        /// </param>
        /// <param name="settings">The XML writer settings. Optional.</param>
        /// <returns>A stream of serialized xml data.</returns>
        /// <exception cref="SerializationException">An error occurred
        /// serializing stream. See inner exception for more details.
        /// </exception>
        public MemoryStream SerializeStream(T serializableData, XmlWriterSettings settings = null)
        {
            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);

            var stream = new MemoryStream();
            XmlWriter writer = null;

            try
            {
                writer = XmlWriter.Create(stream, settings);
                this.GetSerializer().Serialize(writer, serializableData, namespaces);
            }
            catch (Exception inner)
            {
                var ex = new SerializationException(Resources.ERR_SerializingInnerEx.FormatWith("stream"), inner);
                Trace.TraceError(ex.ToString());
                throw ex;
            }
            finally
            {
                writer.DisposeSafe();
            }

            return stream;
        }

        #endregion Stream

        #region String

        /// <summary>
        /// Deserializes the specified xml string of data into the correct type
        /// model.
        /// </summary>
        /// <param name="data">The xml data string.</param>
        /// <param name="actionHandler">The action handler for assigning and
        /// performing any fixes for closure.</param>
        /// <param name="settings">Optional. The xml reader settings to be used.
        /// </param>
        /// <remarks>
        /// If calling this method in an application scanned by Veracode, then
        /// you need to pass a <see cref="XmlReaderSettings"/> instance with the
        /// Xml Resolver set to null.
        /// </remarks>
        public void DeserializeString(string data, Action<T> actionHandler, XmlReaderSettings settings = null)
        {
            ADP.CheckArgumentNull(actionHandler, nameof(actionHandler));
            StringReader txtReader = null;
            XmlReader reader = null;

            try
            {
                XmlSerializer serializer = this.GetSerializer();

                txtReader = new StringReader(data);
                reader  = XmlReader.Create(txtReader, settings);

                serializer.Deserialize(reader).TryCast(out T result);
                actionHandler(result);
            }
            catch (Exception inner)
            {
                var ex = new SerializationException(Resources.ERR_DeserializingInnerEx.FormatWith("string"), inner);
                Trace.TraceError(ex.ToString());
                throw ex;
            }
            finally
            {
                reader.DisposeSafe();
                txtReader.DisposeSafe();
            }
        }

        /// <summary>
        /// Serializes the specified type model of data into an xml data string.
        /// </summary>
        /// <param name="serializableData">The serializable data.</param>
        /// <param name="processorInstructions">The processor instructions to be
        /// included in the xml.</param>
        /// <returns>
        /// A string of serialized xml data.
        /// </returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope",
            Justification = "Everything is in a try/finally block so it is being caught and disposed correctly.")]
        public string SerializeString(T serializableData, IEnumerable<KeyValuePair<string, string>> processorInstructions = null)
        {
            var utf8 = new UTF8Encoding(false);
            string data;

            var nameSpaces = new XmlSerializerNamespaces();
            nameSpaces.Add(string.Empty, string.Empty);

            var settings = new XmlWriterSettings
                               {
                                   OmitXmlDeclaration = false,
                                   Encoding = utf8,
                                   Indent = true
                               };

            MemoryStream stream = null;

            try
            {
                stream = new MemoryStream(500);
                using (XmlWriter writer = XmlWriter.Create(stream, settings))
                {
                    if (!processorInstructions.IsNullOrEmpty())
                    {
                        foreach (KeyValuePair<string, string> pair in processorInstructions)
                        {
                            writer.WriteProcessingInstruction(pair.Key, pair.Value);
                        }
                    }

                    this.GetSerializer().Serialize(writer, serializableData, nameSpaces);
                }

                data = utf8.GetString(stream.ToArray());
            }
            finally
            {
                stream.DisposeSafe();
            }

            return data;
        }

        #endregion String

        #region Helpers

        /// <summary>
        /// Gets the a serializer object reference for the type T.
        /// </summary>
        /// <returns>An XML serializer instance for the type T.</returns>
        protected virtual XmlSerializer GetSerializer()
        {
            var serializer = new XmlSerializer(typeof(T));
            return serializer;
        }

        #endregion Helpers
    }
}