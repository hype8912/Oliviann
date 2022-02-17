#if !NETSTANDARD2_0 && !NETCOREAPP2_0 && !NET35

namespace Oliviann.ServiceModel.Web
{
    #region Usings

    using System.Net;
    using System.ServiceModel.Channels;
    using System.ServiceModel.Web;
    using System.Text;

    #endregion Usings

    /// <summary>
    /// Represents a collection of commonly used extension methods for extending
    /// <see cref="WebOperationContext"/>.
    /// </summary>
    public static class WebOperationContextExtensions
    {
        /// <summary>
        /// Creates a UTF8 xml text formatted message.
        /// </summary>
        /// <param name="context">The current Web operation context.</param>
        /// <param name="text">The XML formatted text to be written to the
        /// message.</param>
        /// <returns>A UTF8 xml text formatted message if the specified context
        /// is not null; otherwise, the result is null.</returns>
        public static Message CreateUTF8XmlResponse(this WebOperationContext context, string text)
        {
            return context?.CreateTextResponse(text, @"text/xml", Encoding.UTF8);
        }

        /// <summary>
        /// Creates a JSON text formatted message.
        /// </summary>
        /// <param name="context">>The current Web operation context.</param>
        /// <param name="text">The JSON formatted text to be written to the
        /// message.</param>
        /// <returns>A JSON text formatted message if the specified context is
        /// not null; otherwise, the result is null.</returns>
        public static Message CreateJsonResponse(this WebOperationContext context, string text)
        {
            return context.CreateTextResponseSafe(text, "application/json");
        }

        /// <summary>
        /// Creates a text formatted message safely.
        /// </summary>
        /// <param name="context">The current Web operation context.</param>
        /// <param name="text">The text to write to the message.</param>
        /// <param name="contentType">The content type of the message.</param>
        /// <returns>A text formatted message.</returns>
        public static Message CreateTextResponseSafe(this WebOperationContext context, string text, string contentType = null)
        {
            if (context == null)
            {
                return null;
            }

            return contentType.IsNullOrEmpty() ? context.CreateTextResponse(text) : context.CreateTextResponse(text, contentType);
        }

        /// <summary>
        /// Sets the specified web operation context's outgoing response status
        /// code and status description to the specified values by checking if
        /// the specified <paramref name="context"/> is null first.
        /// </summary>
        /// <param name="context">The current web operation context.</param>
        /// <param name="statusCode">The status code to be set.</param>
        /// <param name="statusDescription">Optional. The status description to
        /// be set.</param>
        public static void SetOutgoingResponseStatusSafe(
                                                         this WebOperationContext context,
                                                         HttpStatusCode statusCode,
                                                         string statusDescription = null)
        {
            if (context == null)
            {
                return;
            }

            context.OutgoingResponse.StatusCode = statusCode;
            if (!statusDescription.IsNullOrEmpty())
            {
                context.OutgoingResponse.StatusDescription = statusDescription;
            }
        }
    }
}

#endif