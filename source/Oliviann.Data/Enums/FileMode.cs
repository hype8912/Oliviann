namespace Oliviann.Data
{
    #region Usings

    using System.ComponentModel;

    #endregion Usings

    /// <summary>
    /// Modes for opening and communicating with the database.
    /// </summary>
    public enum FileMode
    {
        /// <summary>
        /// Allows multiple processes to open and modify the database. This is
        /// the default setting if the mode property is not specified.
        /// </summary>
        [Description("Read Write")]
        Read_Write,

        /// <summary>
        /// Allows you to open a read-only copy of the database.
        /// </summary>
        [Description("Read Only")]
        Read_Only,

        /// <summary>
        /// Does not allow other processes from opening or modifying the
        /// database until the database connection is closed.
        /// </summary>
        [Description("Exclusive")]
        Exclusive,

        /// <summary>
        /// Allows other processes to read, but not modify, while the database
        /// connection is still open.
        /// </summary>
        [Description("Shared Read")]
        SharedRead
    }
}