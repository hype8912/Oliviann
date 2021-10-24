#if !NETSTANDARD1_3

namespace Oliviann.Runtime.Serialization.Formatters.Binary
{
    #region Usings

    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;

    #endregion Usings

    /// <summary>
    /// Represents a collection of commonly used extension methods for extending
    /// <see cref="BinaryFormatter"/>.
    /// </summary>
    public static class BinaryFormatterExtensions
    {
        /// <summary>
        /// Deserializes the specified stream into a typed graph.
        /// </summary>
        /// <typeparam name="T">The type of object to be returned.</typeparam>
        /// <param name="formatter">The formatter instance.</param>
        /// <param name="serializationStream">The stream from which to
        /// deserialize the object graph.</param>
        /// <returns>
        /// If the <paramref name="formatter"/> is null, the the default value
        /// of T; otherwise, the top (root) of the typed graph.
        /// </returns>
        public static T Deserialize<T>(this BinaryFormatter formatter, Stream serializationStream)
        {
            return (T)formatter?.Deserialize(serializationStream);
        }
    }
}

#endif