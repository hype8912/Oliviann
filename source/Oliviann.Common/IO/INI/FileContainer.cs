namespace Oliviann.IO
{
    #region Usings

    using System.Collections.Generic;

    #endregion Usings

    /// <summary>
    /// Defines the root container class for an INI file.
    /// </summary>
    public class FileContainer
    {
        #region Contsructor/Destructor

        /// <summary>
        /// Initializes a new instance of the <see cref="FileContainer"/> class.
        /// </summary>
        public FileContainer()
        {
            this.Sections = new List<Section>();
        }

        #endregion Contsructor/Destructor

        #region Properties

        /// <summary>
        /// Gets a list of INI sections.
        /// </summary>
        /// <value>A list of INI sections.</value>
        public List<Section> Sections { get; private set; }

        #endregion Properties
    }
}