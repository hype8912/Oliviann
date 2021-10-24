namespace Oliviann.IO
{
    #region Usings

#if !NET40
    using System.Threading.Tasks;
#endif

    #endregion Usings

    /// <summary>
    /// Represents an implementation for a writer.
    /// </summary>
    public interface IWriter
    {
        /// <summary>
        /// Writes the string to the target.
        /// </summary>
        /// <param name="value">The string to write.</param>
        void Write(string value);

        /// <summary>
        /// Writes out a formatted string to the target
        /// </summary>
        /// <param name="format">A composite format string.</param>
        /// <param name="args">An object array that contains zero or more
        /// objects to format and write.</param>
        void Write(string format, params object[] args);

#if !NET40

        /// <summary>
        /// Asynchronously writes a string to the target.
        /// </summary>
        /// <param name="value">The string to write.</param>
        /// <returns>A task that represents the asynchronous write operation.
        /// </returns>
        Task WriteAsync(string value);

#endif

        /// <summary>
        /// Writes a string to the target, followed by a line terminator.
        /// </summary>
        /// <param name="value">The string to write. If value is null, only the
        /// line terminator is written.</param>
        void WriteLine(string value);

        /// <summary>
        /// Writes out a formatted string and a new line to the target.
        /// </summary>
        /// <param name="format">A composite format string.</param>
        /// <param name="args">An object array that contains zero or more
        /// objects to format and write.</param>
        void WriteLine(string format, params object[] args);

#if !NET40

        /// <summary>
        /// Asynchronously writes a string to the target, followed by a line
        /// terminator.
        /// </summary>
        /// <param name="value">The string to write. If value is null, only the
        /// line terminator is written.</param>
        /// <returns>A task that represents the asynchronous write operation.
        /// </returns>
        Task WriteLineAsync(string value);

#endif
    }
}