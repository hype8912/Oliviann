namespace Oliviann.IO
{
    #region Usings

    using System;
    using System.Diagnostics;
    using System.IO;
#if !NETSTANDARD1_3
    using System.Runtime.Serialization.Formatters.Binary;
#endif
    using Oliviann.Properties;

    #endregion Usings

    /// <summary>
    /// Represents a collection of commonly used extension methods for extending
    /// byte streams.
    /// </summary>
    public static class StreamExtensions
    {
#if NET35
        /// <summary>Reads the bytes from the current stream and writes them to
        /// another stream.</summary>
        /// <param name="source">The stream of which the contents will be copied
        /// from.</param>
        /// <param name="target">The stream to which the contents of the
        /// current stream will be copied.</param>
        /// <remarks>This method was added in .NET Framework 4.0.</remarks>
        public static void CopyTo(this Stream source, Stream target)
        {
            ADP.CheckArgumentNull(source, nameof(source));
            ADP.CheckArgumentNull(target, nameof(target));

            if (!source.CanRead)
            {
                throw new NotSupportedException(Resources.ERR_StreamNotReadable);
            }

            if (!source.CanWrite)
            {
                throw new NotSupportedException(Resources.ERR_StreamNotWritable);
            }

            byte[] buffer = new byte[32768];
            int read;
            while ((read = source.Read(buffer, 0, buffer.Length)) > 0)
            {
                target.Write(buffer, 0, read);
            }
        }
#endif

        /// <summary>
        /// Reads the stream from the current position to the end of the stream.
        /// </summary>
        /// <param name="stream">The byte stream to read from.</param>
        /// <returns>The rest of the stream as a string, from the current
        /// position to the end. If the current position is at the end of the
        /// stream, returns the empty string("").</returns>
        public static string ReadToEnd(this Stream stream)
        {
            using (var reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }

        /// <summary>
        /// Writes the text to the specified stream.
        /// </summary>
        /// <param name="stream">The stream to write to.</param>
        /// <param name="text">The string to write to the stream. If value is
        /// null, nothing is written.</param>
        /// <param name="autoClose">True if the stream writer and stream should
        /// be disposed when done writing. Default value is true.</param>
        /// <returns>The <see cref="StreamWriter"/> instance if auto close is
        /// set to false; otherwise, null.</returns>
        public static StreamWriter Write(this Stream stream, string text, bool autoClose = true)
        {
            StreamWriter writer = null;

            try
            {
                writer = new StreamWriter(stream);
                writer.Write(text);
                writer.Flush();
            }
            finally
            {
                if (autoClose && writer != null)
                {
                    writer.Dispose();
                    writer = null;
                }
            }

            return writer;
        }

        /// <summary>
        /// Converts the specified stream to a byte array.
        /// </summary>
        /// <param name="stream">The stream to be read from.
        /// </param>
        /// <param name="closeStream">Optional. True will close the specified
        /// stream when done. False will leave the stream open. Default value is
        /// false.</param>
        /// <returns>A byte array representing the specified stream if
        /// successful; otherwise, null.
        /// </returns>
        public static byte[] ToArray(this Stream stream, bool closeStream = false)
        {
            if (stream == null)
            {
                return null;
            }

            byte[] result;
            if (stream is MemoryStream tempStream)
            {
                result = tempStream.ToArray();
            }
            else
            {
                using (var memStream = new MemoryStream())
                {
                    stream.CopyTo(memStream);
                    result = memStream.ToArray();
                }
            }

            if (closeStream)
            {
                stream.Dispose();
            }

            return result;
        }

#if !NETSTANDARD1_3

        /// <summary>
        /// Converts the specified <paramref name="item"/> to a stream.
        /// </summary>
        /// <param name="item">The object to be converted to a
        /// <see cref="Stream"/>.</param>
        /// <returns>A <see cref="MemoryStream"/> for the specified object.
        /// </returns>
        /// <remarks>Keep in mind that this method is returning an instance of a
        /// stream so the calling method is responsible for disposing of the
        /// stream object when done. This method uses a
        /// <see cref="BinaryFormatter"/> to serialize the object before writing
        /// in to a stream. To read the object back from the stream it will have
        /// to be deserialized first.</remarks>
        public static MemoryStream ToStream(this object item)
        {
            if (item == null)
            {
                return null;
            }

            var memStream = new MemoryStream();

            try
            {
                memStream.Position = 0;
                new BinaryFormatter().Serialize(memStream, item);
                memStream.Flush();
            }
            catch (Exception ex)
            {
               Trace.TraceError(@"An error occurred converting object to a Stream: " + ex.Message);
            }

            return memStream;
        }

#endif
    }
}