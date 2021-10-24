namespace Oliviann.Collections.Specialized
{
    #region Usings

    using System;
    using System.Collections.Specialized;
    using System.Reflection;
    using Oliviann.Reflection;

    #endregion Usings

    /// <summary>
    /// Represents a collection of commonly used extension methods for extending
    /// <see cref="NameValueCollection"/>.
    /// </summary>
    public static class NameValueCollectionExtensions
    {
        /// <summary>
        /// Sets the collection read only property to the specified value.
        /// </summary>
        /// <param name="collection">The collection to be set.</param>
        /// <param name="value">True if the collection is to be read-only;
        /// otherwise, false.</param>
        /// <returns>The collection after the property has been set.</returns>
        public static NameValueCollection SetReadOnly(this NameValueCollection collection, bool value)
        {
            if (collection != null)
            {
                PropertyInfo property = GetIsReadOnlyInfo();
                property?.SetValue(collection, value);
            }

            return collection;
        }

        /// <summary>
        /// Determines whether the specified collection is read-only.
        /// </summary>
        /// <param name="collection">The collection to be checked.</param>
        /// <returns>
        /// True if the collection is read-only; otherwise, false.
        /// </returns>
        public static bool IsReadOnly(this NameValueCollection collection)
        {
            if (collection == null)
            {
                return false;
            }

            PropertyInfo property = GetIsReadOnlyInfo();
            var result = property != null && (bool)property.GetValue(collection);
            return result;
        }

        /// <summary>
        /// Gets the property information for the .
        /// </summary>
        /// <returns>The property info object for "IsReadOnly".</returns>
        private static PropertyInfo GetIsReadOnlyInfo()
        {
            return typeof(NameValueCollection).GetProperty(
                "IsReadOnly",
                BindingFlags.Instance | BindingFlags.IgnoreCase | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);
        }
    }
}