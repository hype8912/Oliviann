namespace Oliviann.Data
{
    #region Usings

    using System;
    using Oliviann.Properties;

    #endregion Usings

    /// <summary>
    /// Represents a static automatic data processing class.
    /// </summary>
    internal static class ADP
    {
        #region Check

        /// <summary>
        /// Checks the provider type to be valid.
        /// </summary>
        /// <param name="provider">The current provider type.</param>
        /// <param name="correctProvider">The correct provider type.</param>
        /// <exception cref="InvalidOperationException">The specified database
        /// provider is the incorrect provider.</exception>
        internal static void CheckProviderInvalid(DatabaseProvider provider, DatabaseProvider correctProvider)
        {
            if (provider != correctProvider)
            {
                throw Oliviann.ADP.InvalidOperation(Resources.ERR_DB_InvalidProvider.FormatWith(correctProvider));
            }
        }

        #endregion Check
    }
}