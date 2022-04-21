namespace Oliviann.IO
{
    #region Usings

    using System;
    using System.IO;

    #endregion

    /// <summary>
    /// Represents access to a temporary file.
    /// </summary>
    public sealed class TemporaryFile : IFile, IDisposable
    {
        #region Constructor/Destructor

        /// <summary>
        /// Initializes a new instance of the <see cref="TemporaryFile"/>
        /// class.
        /// </summary>
        /// <param name="path">The path of where to create the temprorary file.
        /// </param>
        private TemporaryFile(string path)
        {
            this.Path = path;
        }

        #endregion

        #region Properties

        /// <inheritdoc />
        public string Path { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Creates a new temporary file.
        /// </summary>
        /// <returns>A temporary file instance.</returns>
        public static IFile Create()
        {
            return new TemporaryFile(System.IO.Path.GetTempFileName());
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        public void Dispose()
        {
            if (File.Exists(this.Path))
            {
                File.Delete(this.Path);
            }

            GC.SuppressFinalize(this);
        }

        /// <inheritdoc />
        public Stream Open()
        {
            return File.Open(this.Path, FileMode.OpenOrCreate, FileAccess.ReadWrite);
        }

        #endregion
    }
}