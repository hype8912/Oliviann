namespace Oliviann.IO
{
    #region Usings

    using System.IO;

    #endregion

    /// <summary>
    /// Represents an implementation of an open file.
    /// </summary>
    public interface IFile
    {
        #region Properties

        /// <summary>
        /// Gets the path of the file to be opened.
        /// </summary>
        public string Path { get; }

        /// <summary>
        /// Opens a file on the specified path.
        /// </summary>
        /// <returns>A stream that provides access to the specified file.
        /// </returns>
        public Stream Open();

        #endregion
    }
}