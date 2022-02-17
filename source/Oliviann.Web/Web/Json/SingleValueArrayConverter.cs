namespace Oliviann.Web.Json
{
    #region Usings

    using System;
    using System.Collections.Generic;
    using Newtonsoft.Json;

    #endregion

    /// <summary>
    /// Represents a converter to return a list for a single item array.
    /// </summary>
    /// <typeparam name="T">Object type to be converted.</typeparam>
    /// <seealso cref="Newtonsoft.Json.JsonConverter" />
    public class SingleValueArrayConverter<T> : JsonConverter
    {
        #region Methods

        /// <summary>
        /// Writes the JSON representation of the object.
        /// </summary>
        /// <param name="writer">The <see cref="T:Newtonsoft.Json.JsonWriter" />
        /// to write to.</param>
        /// <param name="value">The value being written.</param>
        /// <param name="serializer">The calling serializer.</param>
        /// <exception cref="System.NotImplementedException" />
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Reads the JSON representation of the object.
        /// </summary>
        /// <param name="reader">The <see cref="T:Newtonsoft.Json.JsonReader" />
        /// to read from.</param>
        /// <param name="objectType">Type of the object.</param>
        /// <param name="existingValue">The existing value of object being read.
        /// </param>
        /// <param name="serializer">The calling serializer.</param>
        /// <returns>
        /// The object value.
        /// </returns>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var returnValue = new object();
            if (reader.TokenType == JsonToken.StartObject)
            {
                T instance = (T)serializer.Deserialize(reader, typeof(T));
                returnValue = new List<T> { instance };
            }
            else if (reader.TokenType == JsonToken.StartArray)
            {
                returnValue = serializer.Deserialize(reader, objectType);
            }

            return returnValue;
        }

        /// <summary>
        /// Determines whether this instance can convert the specified object
        /// type.
        /// </summary>
        /// <param name="objectType">Type of the object.</param>
        /// <returns>
        /// True if this instance can convert the specified object type;
        /// otherwise, false.
        /// </returns>
        public override bool CanConvert(Type objectType) => true;

        #endregion
    }
}