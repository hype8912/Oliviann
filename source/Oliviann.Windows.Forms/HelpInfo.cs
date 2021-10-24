namespace Oliviann.Windows.Forms
{
    #region Usings

    using System.Windows.Forms;

    #endregion Usings

    /// <summary>
    /// Represents an internal help file information model.
    /// </summary>
    internal class HelpInfo
    {
        #region Fields

        /// <summary>
        /// The shared navigator selection.
        /// </summary>
        private readonly HelpNavigator navigator;

        #endregion Fields

        #region Constructor/Destructor

        /// <summary>
        /// Initializes a new instance of the <see cref="HelpInfo" /> class.
        /// </summary>
        /// <param name="helpFilePath">The path and name of the Help file.
        /// </param>
        public HelpInfo(string helpFilePath)
        {
            this.HelpFilePath = helpFilePath;
            this.Keyword = string.Empty;
            this.navigator = HelpNavigator.TableOfContents;
            this.Param = null;
            this.Option = 1;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HelpInfo" /> class.
        /// </summary>
        /// <param name="helpFilePath">The path and name of the Help file.
        /// </param>
        /// <param name="keyword">The Help file topic keyword.</param>
        public HelpInfo(string helpFilePath, string keyword)
        {
            this.HelpFilePath = helpFilePath;
            this.Keyword = keyword;
            this.navigator = HelpNavigator.TableOfContents;
            this.Param = null;
            this.Option = 2;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HelpInfo" /> class.
        /// </summary>
        /// <param name="helpFilePath">The path and name of the Help file.
        /// </param>
        /// <param name="navigator">One of the <see cref="HelpNavigator"/>
        /// values.</param>
        public HelpInfo(string helpFilePath, HelpNavigator navigator)
        {
            this.HelpFilePath = helpFilePath;
            this.Keyword = string.Empty;
            this.navigator = navigator;
            this.Param = null;
            this.Option = 3;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HelpInfo" /> class.
        /// </summary>
        /// <param name="helpFilePath">The path and name of the Help file.
        /// </param>
        /// <param name="navigator">One of the <see cref="HelpNavigator"/>
        /// values.</param>
        /// <param name="param">The numeric ID of the Help topic.</param>
        public HelpInfo(string helpFilePath, HelpNavigator navigator, object param)
        {
            this.HelpFilePath = helpFilePath;
            this.Keyword = string.Empty;
            this.navigator = navigator;
            this.Param = param;
            this.Option = 4;
        }

        #endregion Constructor/Destructor

        #region Properties

        /// <summary>
        /// Gets the path and name of the Help file.
        /// </summary>
        /// <value>
        /// The path and name of the Help file.
        /// </value>
        public string HelpFilePath { get; }

        /// <summary>
        /// Gets the Help file topic keyword.
        /// </summary>
        /// <value>
        /// The Help file topic keyword.
        /// </value>
        public string Keyword { get; }

        /// <summary>
        /// Gets the creation option number.
        /// </summary>
        /// <value>
        /// The creation option number.
        /// </value>
        public int Option { get; }

        /// <summary>
        /// Gets the numeric ID of the Help topic.
        /// </summary>
        /// <value>
        /// The numeric ID of the Help topic.
        /// </value>
        public object Param { get; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this
        /// instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return $"{{HelpFilePath={this.HelpFilePath}, keyword ={this.Keyword}, navigator={this.navigator}}}";
        }

        #endregion Methods
    }
}