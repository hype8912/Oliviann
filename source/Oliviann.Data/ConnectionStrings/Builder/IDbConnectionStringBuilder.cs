namespace Oliviann.Data.ConnectionStrings.Builder
{
    #region Usings

    using System;

    #endregion

    /// <summary>
    /// Represents a base interface for the connection string builder interfaces
    /// to implement.
    /// </summary>
    public interface IDbConnectionStringBuilder
    {
        /// <summary>
        /// Gets and creates a database connection string.
        /// </summary>
        /// <returns>A complete connection string based on the parameters.
        /// </returns>
        string ConnectionString { get; }

        /// <summary>
        /// Adds a custom attribute and value if the attribute is not available.
        /// If the attribute already exists, then the value of the attribute
        /// will be replaced with the new value.
        /// </summary>
        /// <param name="attribute">The attribute name.</param>
        /// <param name="value">The attribute value.</param>
        /// <remarks>
        /// If the attribute or the value are <c>null</c> or empty then no entry
        /// will be added to the connection string.
        /// </remarks>
        void CustomAttribute(string attribute, string value);

        /// <summary>
        /// Adds a custom attribute and value if the attribute is not already
        /// available. If the attribute already exists, then the value of the
        /// attribute will be replaced with the new value.
        /// </summary>
        /// <param name="attribute">The attribute string.</param>
        /// <param name="encryptedValue">The encrypted string value.</param>
        /// <param name="decrypter">The decrypter delegate that accepts an
        /// encrypted string and returns a decrypted string.</param>
        /// <remarks>
        /// If the attribute or the encrypted string are <c>null</c> or empty
        /// then no entry will be added to the connection string.
        /// </remarks>
        /// <exception cref="DbDataException">An error occurred while invoking
        /// the decrypter delegate.</exception>
        void CustomEncryptedAttribute(string attribute, string encryptedValue, Func<string, string> decrypter = null);

        /// <summary>
        /// Sets the password depending if the password is wrapped in single
        /// quotes determining if it needs to be decrypted first.
        /// </summary>
        /// <param name="password">The password string.</param>
        /// <param name="decrypter">The delegate for decrypting the password
        /// string.</param>
        /// <remarks>If the password string begins and ends with a single quote
        /// then the single quotes will be removed and the text set to the
        /// password value. If the decrypter value is <c>null</c> then the
        /// specified password will be set to the password value; otherwise the
        /// decrypter delegate will be called to execute returning back a
        /// decrypted string which will be then set to the password value.
        /// </remarks>
        void SetPassword(string password, Func<string, string> decrypter = null);
    }
}