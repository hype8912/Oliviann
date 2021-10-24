#if !NET35

namespace Oliviann.Runtime.Serialization
{
    #region Usings

    using System.IO;
    using System.Runtime.Serialization;

    #endregion Usings

    /// <summary>
    /// Represents a collection of commonly used extension methods for extending
    /// <see cref="DataContractSerializer"/>.
    /// </summary>
    public static class DataContractSerializerExtensions
    {
        #region Deserialize Methods

        /// <summary>
        /// Deserializes the specified string data using the specified
        /// serializer.
        /// </summary>
        /// <typeparam name="T">The type of data to be deserialized.</typeparam>
        /// <param name="serializer">The serializer to use to deserialize the
        /// data.</param>
        /// <param name="data">The serialized string data.</param>
        /// <returns>An instance of T matching the specified data.</returns>
        public static T DeserializeString<T>(this DataContractSerializer serializer, string data)
        {
            ADP.CheckArgumentNull(serializer, nameof(serializer));
            if (data == null)
            {
                return default(T);
            }

            using (var stream = new MemoryStream())
            using (var writer = new StreamWriter(stream))
            {
                writer.Write(data);
                writer.Flush();

                stream.Seek(0, SeekOrigin.Begin);

                var result = (T)serializer.ReadObject(stream);
                return result;
            }
        }

        /// <summary>
        /// Deserializes the specified byte data using the specified serializer.
        /// </summary>
        /// <typeparam name="T">The type of data to be deserialized.</typeparam>
        /// <param name="serializer">The serializer to use to deserialize the
        /// data.</param>
        /// <param name="data">The serialized byte data.</param>
        /// <returns>An instance of T matching the specified data.</returns>
        public static T DeserializeString<T>(this DataContractSerializer serializer, byte[] data)
        {
            ADP.CheckArgumentNull(serializer, nameof(serializer));
            if (data == null)
            {
                return default(T);
            }

            using (var stream = new MemoryStream(data))
            {
                stream.Seek(0, SeekOrigin.Begin);

                var result = (T)serializer.ReadObject(stream);
                return result;
            }
        }

        #endregion Deserialize Methods

        #region Serialize Methods

        /// <summary>
        /// Serializes the specified data using the specified serializer.
        /// </summary>
        /// <typeparam name="T">The type of object being serialized.</typeparam>
        /// <param name="serializer">The serializer to use to serialize the
        /// data.</param>
        /// <param name="serializableData">The data object to be serialized.
        /// </param>
        /// <returns>A string that matches the data to be serialized.</returns>
        public static string SerializeString<T>(this DataContractSerializer serializer, T serializableData)
        {
            ADP.CheckArgumentNull(serializer, nameof(serializer));

            using (var stream = new MemoryStream())
            {
                serializer.WriteObject(stream, serializableData);
                stream.Seek(0, SeekOrigin.Begin);

                using (var streamReader = new StreamReader(stream))
                {
                    string result = streamReader.ReadToEnd();
                    return result;
                }
            }
        }

        #endregion Serialize Methods
    }
}

#endif