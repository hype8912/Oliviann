namespace Oliviann.Data.ConnectionStrings
{
    #region Usings

    using System;

    #endregion

    /// <summary>
    /// Interface for creating a new database connection string.
    /// </summary>
    public interface IDbConnectionString
    {
        /// <summary>
        /// Gets or sets the database object.
        /// </summary>
        /// <value>The database object.</value>
        IDatabase DatabaseItem { get; set; }

        /// <summary>
        /// Gets a connection string for connecting to the specific database.
        /// </summary>
        /// <param name="decrypter">A function for taking in an encrypted string
        /// and returning a decrypted string.</param>
        /// <returns>
        /// A database connection string.
        /// </returns>
        string ConnectionString(Func<string, string> decrypter);
    }
}