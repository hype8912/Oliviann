#if !NETSTANDARD2_0 && !NETCOREAPP2_0 && !NET35

namespace Oliviann.ServiceModel.Channels
{
    #region Usings

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.ServiceModel.Channels;
    using Oliviann.ServiceModel.Web;

    #endregion Usings

    /// <summary>
    /// Represents a collection of commonly used extension methods for extending
    /// <see cref="Message"/>.
    /// </summary>
    public static class MessageExtensions
    {
        /// <summary>
        /// Deserializes an XML message object into a single POCO.
        /// </summary>
        /// <typeparam name="TIn">The type of collection inside the message.
        /// </typeparam>
        /// <typeparam name="TOut">The type of message to be serialized into.
        /// </typeparam>
        /// <param name="msg">The XML message object to be deserialized.</param>
        /// <param name="getter">The function delegate for getting the object
        /// from the deserialized message object.</param>
        /// <returns>A single object of <typeparamref name="TOut"/> from the
        /// message.</returns>
        public static TOut DeserializeXmlMessage<TIn, TOut>(this Message msg, Func<TIn, TOut> getter)
            where TIn : class
            where TOut : class
        {
            var deserializedMessage = WebOperationContextHelper.DeserializeXmlMessage<TIn>(msg);
            if (deserializedMessage == null || getter == null)
            {
                return default;
            }

            TOut getterResult = getter(deserializedMessage);
            return getterResult;
        }

        /// <summary>
        /// Deserializes an XML message object into a collection of objects.
        /// </summary>
        /// <typeparam name="TIn">The type of collection inside the message.
        /// </typeparam>
        /// <typeparam name="TOut">The type of message to be serialized into.
        /// </typeparam>
        /// <param name="msg">The XML message object to be deserialized.</param>
        /// <param name="getter">The function delegate for getting the
        /// collection from the deserialized message object.</param>
        /// <returns>A collection of <typeparamref name="TOut"/> objects from
        /// the message.</returns>
        public static IEnumerable<TOut> DeserializeXmlMessage<TIn, TOut>(this Message msg, Func<TIn, IEnumerable<TOut>> getter)
            where TIn : class
            where TOut : class
        {
            TIn deserializedMessage = WebOperationContextHelper.DeserializeXmlMessage<TIn>(msg);
            if (deserializedMessage == null || getter == null)
            {
                return Enumerable.Empty<TOut>();
            }

            IEnumerable<TOut> getterResult = getter(deserializedMessage);
            return getterResult ?? Enumerable.Empty<TOut>();
        }
    }
}

#endif