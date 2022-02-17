namespace Oliviann.Data.DatabaseManagement.Shrink
{
    #region Usings

    using System;

    #endregion

    /// <summary>
    /// Implements shrinking the database.
    /// </summary>
    public interface IDbShrink
    {
        /// <summary>
        /// Gets the database object.
        /// </summary>
        /// <value>The database object.</value>
        IDatabase Database { get; }

        /// <summary>
        /// Shrinks the specified database.
        /// </summary>
        /// <param name="decrypter">A function for taking in an encrypted string
        /// and returning a decrypted string.</param>
        /// <returns>
        ///   <c>True</c> if the database shrinks successfully; otherwise,
        /// <c>false</c>.
        /// </returns>
        bool ShrinkDatabase(Func<string, string> decrypter);
    }
}