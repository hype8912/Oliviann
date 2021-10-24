namespace Oliviann.IPC
{
    #region Usings

    using System;

    #endregion Usings

    /// <summary>
    /// Represents an inter process communication data object.
    /// </summary>
    [Serializable]
    public class InterProcessData
    {
        #region Fields

        /// <summary>
        /// Gets or sets the first data string.
        /// </summary>
        /// <value>
        /// The first data string.
        /// </value>
        public string Data1 { get; set; }

        /// <summary>
        /// Gets or sets the second data string.
        /// </summary>
        /// <value>
        /// The second data string.
        /// </value>
        public string Data2 { get; set; }

        #endregion Fields
    }
}