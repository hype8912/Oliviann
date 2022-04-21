namespace Oliviann
{
    #region Usings

    using System;
#if !NET40
    using System.Threading.Tasks;
#endif
    using Oliviann.IO;

    #endregion Usings

    /// <summary>
    /// Represents a writer to the console.
    /// </summary>
    public class ConsoleWriter : IWriter
    {
        #region Methods

        /// <inheritdoc />
        public void Write(string value) => Console.Write(value);

        /// <inheritdoc />
        public void Write(string format, params object[] args) => Console.Write(format, args);

#if !NET40

        /// <inheritdoc />
        public Task WriteAsync(string value) => Console.Out.WriteLineAsync(value);

#endif

        /// <inheritdoc />
        public void WriteLine(string value) => Console.WriteLine(value);

        /// <inheritdoc />
        public void WriteLine(string format, params object[] args) => Console.WriteLine(format, args);

#if !NET40

        /// <inheritdoc />
        public Task WriteLineAsync(string value) => Console.Out.WriteLineAsync(value);

#endif

        #endregion
    }
}