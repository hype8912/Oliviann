namespace Oliviann.Testing.Fixtures
{
    #region Usings

    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using Oliviann.Reflection;

    #endregion Usings

    /// <summary>
    /// Represents a class fixture for cleaning up file and directory paths once
    /// all the tests have completed.
    /// </summary>
    public class PathCleanupFixture : IDisposable
    {
        #region Constructor/Destructor

        /// <summary>
        /// Initializes a new instance of the <see cref="PathCleanupFixture"/>
        /// class.
        /// </summary>
        public PathCleanupFixture()
        {
            this.CurrentDirectory = Assembly.GetExecutingAssembly().GetCurrentExecutingDirectory();
            this.DeletePaths = new List<string>();
        }

        #endregion Constructor/Destructor

        #region Properties

        /// <summary>
        /// Gets the current directory of this assembly.
        /// </summary>
        /// <value>
        /// The current directory of this assembly.
        /// </value>
        public string CurrentDirectory { get; }

        /// <summary>
        /// Gets the collection of paths to be deleted once the tests have
        /// completed. Can include file and directory paths.
        /// </summary>
        /// <value>
        /// The collection of paths to be deleted.
        /// </value>
        public List<string> DeletePaths { get; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Performs application-defined tasks associated with freeing,
        /// releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            foreach (string deletePath in this.DeletePaths.Where(p => !p.IsNullOrEmpty()))
            {
                try
                {
                    if (deletePath.IsDirectory())
                    {
                        Directory.Delete(deletePath, true);
                    }
                    else
                    {
                        File.Delete(deletePath);
                    }
                }
                catch
                {
                    // Ignore exception.
                }
            }
        }

        #endregion Methods
    }
}