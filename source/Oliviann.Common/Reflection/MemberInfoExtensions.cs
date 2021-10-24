namespace Oliviann.Reflection
{
    #region Usings

    using System;
    using System.Collections;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    #endregion

    /// <summary>
    /// Represents a collection of commonly used extension methods for extending
    /// MemberInfo types.
    /// </summary>
    public static class MemberInfoExtensions
    {
        #region Fields

        /// <summary>
        /// Cache for storing the attributes for a specific type.
        /// </summary>
        private static readonly IDictionary<string, Attribute[]> attributesCache =
            new ConcurrentDictionary<string, Attribute[]>(StringComparer.Ordinal);

        #endregion

        /// <summary>
        /// Determines if an attribute type is defined on the specified member.
        /// </summary>
        /// <typeparam name="T">The type of attribute.</typeparam>
        /// <param name="member">The element to search.</param>
        /// <param name="inherit">Optional. True specifies to also search the
        /// ancestors of <paramref name="member" /> for the attribute type.
        /// </param>
        /// <returns>True if the specified attribute is applied to
        /// <paramref name="member" />; otherwise, false.</returns>
        public static bool AttributeIsDefined<T>(this MemberInfo member, bool inherit = true) where T : Attribute
        {
#if NETSTANDARD1_3
            return member.CustomAttributes.Any(a => a.AttributeType == typeof(T));
#else
            return member != null && Attribute.IsDefined(member, typeof(T), inherit);
#endif
        }

        /// <summary>Returns a custom attribute applied to this member. Will
        /// search the cache first and then query the member if not found in the
        /// cache.</summary>
        /// <param name="member">The member for which to retrieve attributes
        /// for.</param>
        /// <param name="inherit">True to search this member's inheritance chain
        /// to find the attributes; otherwise, false.</param>
        /// <returns>A custom attribute applied to this member if found, or
        /// null.</returns>
        public static T GetCustomAttributeCached<T>(this MemberInfo member, bool inherit = true) where T : Attribute
            => (T)member.GetCustomAttributeCached(typeof(T), inherit);

        /// <summary>Returns a custom attribute applied to this member. Will
        /// search the cache first and then query the member if not found in the
        /// cache.</summary>
        /// <param name="member">The member for which to retrieve attributes
        /// for.</param>
        /// <param name="attributeType">The type of attribute to retrieve.
        /// </param>
        /// <param name="inherit">True to search this member's inheritance chain
        /// to find the attributes; otherwise, false.</param>
        /// <returns>A custom attribute applied to this member if found, or
        /// null.</returns>
        public static Attribute GetCustomAttributeCached(this MemberInfo member, Type attributeType, bool inherit)
        {
            if (member == null)
            {
                return null;
            }

            return GetCustomAttributesCached(member, inherit).FirstOrDefault(attr => attr.GetType() == attributeType);
        }

        /// <summary>Returns an array of custom attributes applied to this
        /// member matching the specified type. Will search the cache first and
        /// then query the member if not found in the cache.</summary>
        /// <param name="member">The member for which to retrieve attributes
        /// for.</param>
        /// <param name="attributeType">The type of attribute to retrieve.
        /// </param>
        /// <param name="inherit">True to search this member's inheritance chain
        /// to find the attributes; otherwise, false.</param>
        /// <returns>An array of custom attributes applied to this member, or an
        /// array with zero elements if no attributes have been applied.
        /// </returns>
        public static Attribute[] GetCustomAttributesCached(this MemberInfo member, Type attributeType, bool inherit) =>
            GetCustomAttributesCached(member, inherit).Where(attr => attr.GetType() == attributeType).ToArray();

        /// <summary>Returns an array of custom attributes applied to this
        /// member. Will search the cache first and then query the member if not
        /// found in the cache.</summary>
        /// <param name="member">The member for which to retrieve attributes
        /// for.</param>
        /// <param name="inherit">True to search this member's inheritance chain
        /// to find the attributes; otherwise, false.</param>
        /// <returns>An array of custom attributes applied to this member, or an
        /// array with zero elements if no attributes have been applied.
        /// </returns>
        public static Attribute[] GetCustomAttributesCached(this MemberInfo member, bool inherit)
        {
            if (member == null)
            {
#if NET35 || NET40 || NET45
                return ArrayHelpers.Empty<Attribute>();
#else
                return Array.Empty<Attribute>();
#endif
            }

            string key = GetCacheKey(member, inherit);
            if (attributesCache.TryGetValue(key, out Attribute[] attributes))
            {
                return attributes;
            }

            attributes = (Attribute[])member.GetCustomAttributes(typeof(Attribute), inherit);
            attributesCache.Add(key, attributes);
            return attributes;
        }

        /// <summary>
        /// Indicates whether one or more attributes of the specified type or of
        /// its derived types is applied to this member.
        /// </summary>
        /// <typeparam name="T">The type of custom attribute to search for. The
        /// search includes derived types.</typeparam>
        /// <param name="member">The member for which to retrieve attributes
        /// for.</param>
        /// <param name="inherit">True to search this member's inheritance chain0
        /// to find the attributes; otherwise, false.</param>
        /// <returns>True if the specified attribute is applied to member;
        /// otherwise, false.</returns>
        public static bool IsDefinedCached<T>(this MemberInfo member, bool inherit = true) where T : Attribute
        {
            return member != null && GetCustomAttributesCached(member, inherit).Any(attr => attr.GetType() == typeof(T));
        }

// Methods were added to do this functionality in .NET 4.5.
#if NET35 || NET40

        /// <summary>Retrieves a custom attribute of a specified type that is
        /// applied to a specified member.</summary>
        /// <param name="element">The member to inspect.</param>
        /// <typeparam name="T">The type of attribute to search for.</typeparam>
        /// <returns>A custom attribute that matches the specified type, or null
        /// if no such attribute is found.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="element" /> is null.</exception>
        /// <exception cref="T:System.NotSupportedException">
        /// <paramref name="element" /> is not a constructor, method, property,
        /// event, type, or field. </exception>
        /// <exception cref="T:System.Reflection.AmbiguousMatchException">More
        /// than one of the requested attributes was found. </exception>
        /// <exception cref="T:System.TypeLoadException">A custom attribute type
        /// cannot be loaded. </exception>
        public static T GetCustomAttribute<T>(this MemberInfo element) where T : Attribute
        {
            return (T)Attribute.GetCustomAttribute(element, typeof(T));
        }

        /// <summary>
        /// Returns the value of the property for non-indexed properties.
        /// </summary>
        /// <param name="property">The property instance.</param>
        /// <param name="obj">The object whose property value will be returned.
        /// </param>
        /// <returns>The property value for the <paramref name="obj"/>
        /// parameter.</returns>
        public static object GetValue(this PropertyInfo property, object obj) => property.GetValue(obj, null);

        /// <summary>
        /// Sets the value of the property for non-indexed properties.
        /// </summary>
        /// <param name="property">The property instance.</param>
        /// <param name="obj">The object whose property value will be set.
        /// </param>
        /// <param name="value">The new value for this property.</param>
        public static void SetValue(this PropertyInfo property, object obj, object value) => property.SetValue(obj, value, null);

#endif

        #region Helper Methods

        /// <summary>
        /// Gets the unique cache key based on the specified member.
        /// </summary>
        /// <param name="member">The current member information.</param>
        /// <param name="inherit">True to search this member's inheritance chain
        /// to find the attributes; otherwise, false.</param>
        /// <returns>A string that represents the specified member.</returns>
        private static string GetCacheKey(MemberInfo member, bool inherit)
        {
#if NETSTANDARD1_3
            Type currentType = member.DeclaringType;
#else
            Type currentType = member.ReflectedType;
#endif

            return currentType == null ? member.Name : currentType.FullName + "." + member.Name + "-" + inherit;
        }

        #endregion
    }
}