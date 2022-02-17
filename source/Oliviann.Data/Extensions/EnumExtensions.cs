namespace Oliviann.Data
{
    #region Usings

    using System;

    #endregion Usings

    /// <summary>
    /// Represents a collection of commonly used extension methods for extending
    /// enumerator objects.
    /// </summary>
    internal static class EnumExtensions
    {
        /// <summary>
        /// Gets the provider attribute for an enumeration field.
        /// </summary>
        /// <param name="value">The enumeration value.</param>
        /// <returns>The provider string if available.</returns>
        internal static string GetProviderAttribute(this Enum value) =>
            value.GetAttribute<ProviderAttribute>()?.Name ?? string.Empty;
    }
}