#if !NET35

namespace Oliviann.Net.Http
{
    #region Usings

    using System;
    using System.Net.Http;

    #endregion Usings

    /// <summary>
    /// Represents a collection of commonly used extension methods for extending
    /// <see cref="HttpRequestMessage"/>.
    /// </summary>
    public static class HttpRequestMessageExtensions
    {
        /// <summary>
        /// Determines whether the specified request is local.
        /// </summary>
        /// <param name="request">The current request.</param>
        /// <returns>
        /// True if the specified request is local; otherwise, false.
        /// </returns>
        public static bool IsLocal(this HttpRequestMessage request)
        {
            return request.Properties["MS_IsLocal"] is Lazy<bool> localFlag && localFlag.Value;
        }
    }
}

#endif