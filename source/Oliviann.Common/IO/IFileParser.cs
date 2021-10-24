namespace Oliviann.IO
{
    /// <summary>
    /// Represents an interface for implementing a generic file parser.
    /// </summary>
    /// <typeparam name="TOut">The output type of the result.</typeparam>
    public interface IFileParser<out TOut>
    {
        #region Properties

        /// <summary>
        /// Gets the fully qualified file path to the input file.
        /// </summary>
        /// <value>
        /// The fully qualified file path to the input file.
        /// </value>
        string FilePath { get; }

        /// <summary>
        /// Gets the parsed results.
        /// </summary>
        /// <value>
        /// The parsed results.
        /// </value>
        TOut Result { get; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Executes the parser to begin reading the input file.
        /// </summary>
        /// <returns>True if the operation completed successfully; otherwise,
        /// false.</returns>
        bool Parse();

        #endregion Methods
    }
}