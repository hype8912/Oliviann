#if !NETSTANDARD2_0 && !NETCOREAPP2_0 && !NET35

namespace Oliviann.ServiceModel.Web
{
    #region Usings

    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.ServiceModel.Channels;
    using System.ServiceModel.Web;
    using System.Web.Script.Serialization;
    using Oliviann.Xml.Serialization;

    #endregion Usings

    /// <summary>
    /// Represents a collection of commonly used helper methods for making
    /// working with REST web services using <see cref="WebOperationContext"/>
    /// easier.
    /// </summary>
    public static class WebOperationContextHelper
    {
        /// <summary>
        /// Allows you to create a simple XML message from a class by
        /// serializing it using the built-in XML serializer.
        /// </summary>
        /// <typeparam name="T">The type of be serialized.</typeparam>
        /// <param name="data">The data to be serialized into a message.</param>
        /// <param name="processorInstructions">Optional. The processor
        /// instructions to be passed to the xml serializer.</param>
        /// <returns>
        /// A message object for returning to the client if the current web
        /// operation context is not null; otherwise, null.
        /// </returns>
        public static Message CreateXmlMessage<T>(T data, IEnumerable<KeyValuePair<string, string>> processorInstructions = null)
            where T : class
        {
#if NET35
            string serializedString = new BasicXmlSerializer<T>().SerializeString(data, processorInstructions);
#else
            string serializedString = new CachedXmlSerializer<T>().SerializeString(data, processorInstructions);
#endif
            WebOperationContext context = WebOperationContext.Current;
            if (context == null)
            {
                Trace.TraceWarning("The current WebOperationContext is null.");
                return null;
            }

            return context.CreateUTF8XmlResponse(serializedString);
        }

        /// <summary>
        /// Allows you to create a simple JSON message from a class type by
        /// serializing it using either the built-in JSON serializer or a custom
        /// serializer.
        /// </summary>
        /// <typeparam name="T">The type of data to be serialized.</typeparam>
        /// <param name="data">The data to be serialized into a message.</param>
        /// <param name="jsonSerializer">Optional. The custom JSON serializer to
        /// serialize the specified input data. The default serializer will be
        /// used if not specified or null. The input <typeparamref name="T"/>
        /// will be the object passed in to this method. The string result from
        /// the delegate will be the serialized JSON string of the input data.
        /// </param>
        /// <returns>
        /// A message object for returning to the client if the current web
        /// operation context is not null; otherwise, null.
        /// </returns>
        public static Message CreateJsonMessage<T>(T data, Func<T, string> jsonSerializer = null) where T : class
        {
            if (jsonSerializer == null)
            {
                jsonSerializer = data2 => new JavaScriptSerializer().Serialize(data2);
            }

            string serializedString = jsonSerializer(data);
            WebOperationContext context = WebOperationContext.Current;
            if (context == null)
            {
                Trace.TraceWarning("The current WebOperationContext is null.");
                return null;
            }

            return context.CreateJsonResponse(text: serializedString);
        }

        /// <summary>
        /// Deserializes an XML message object into a specified type using the
        /// built-in XML serializer.
        /// </summary>
        /// <typeparam name="T">The type of message to be deserialized into.
        /// </typeparam>
        /// <param name="msg">The XML message object to be deserialized.</param>
        /// <returns>
        /// The deserialized output of the specified <paramref name="msg"/>
        /// object.
        /// </returns>
        public static T DeserializeXmlMessage<T>(Message msg) where T : class
        {
            ADP.CheckArgumentNull(msg, nameof(msg));

            T data = null;
#if NET35
            new BasicXmlSerializer<T>().DeserializeString(msg.ToString(), d => data = d);
#else
            new CachedXmlSerializer<T>().DeserializeString(msg.ToString(), d => data = d);
#endif
            return data;
        }
    }
}

#endif