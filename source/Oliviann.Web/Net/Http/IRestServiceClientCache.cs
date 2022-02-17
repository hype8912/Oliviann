#if !NET35

namespace Oliviann.Net.Http
{
    #region Usings

    using System.Net.Http;

    #endregion Usings

    /// <summary>
    /// Represents an implementation of REST service client cache.
    /// </summary>
    internal interface IRestServiceClientCache
    {
        /// <summary>
        /// Validates and adds the specified result message to the cache
        /// </summary>
        /// <param name="response">The current response message.</param>
        /// <param name="result">The response message content result.</param>
        void ClientCacheSet(HttpResponseMessage response, object result);

        /// <summary>
        /// Gets the associated item from the cache if available.
        /// </summary>
        /// <typeparam name="T">The type of object to be returned.</typeparam>
        /// <param name="key">A unique identifier for the cache entry to get.
        /// </param>
        /// <returns>A reference to the cache entry that is identified by key,
        /// if the entry exists; otherwise, null.</returns>
        T GetResponseFromCache<T>(string key);
    }
}

#endif