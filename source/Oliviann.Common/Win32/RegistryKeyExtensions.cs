namespace Oliviann.Win32
{
    #region Usings

    using System;
    using Oliviann.Properties;
    using Microsoft.Win32;

    #endregion Usings

    /// <summary>
    /// Represents a collection of commonly used extension methods for extending
    /// <see cref="RegistryKey"/> objects.
    /// </summary>
    public static class RegistryKeyExtensions
    {
        /// <summary>
        /// Determines if the specified sub key name exists in specified
        /// registry key.
        /// </summary>
        /// <param name="key">The registry key instance.</param>
        /// <param name="name">The name or path of the sub key to open as
        /// read-only.</param>
        /// <returns>
        /// True if the specified key exists; otherwise, false.
        /// </returns>
        /// <exception cref="ArgumentNullException">Registry key cannot be null.
        /// </exception>
        public static bool SubKeyExists(this RegistryKey key, string name)
        {
            ADP.CheckArgumentNull(key, nameof(key), Resources.ERR_RegistryKeyNull);

            RegistryKey subKey = key.OpenSubKey(name);
            bool result = subKey != null;

            subKey.DisposeSafe();
            return result;
        }
    }
}