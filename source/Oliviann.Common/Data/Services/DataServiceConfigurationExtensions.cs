#if NET35_OR_GREATER

namespace Oliviann.Data.Services
{
    #region Usings

    using System.Data.Services;
    using System.Reflection;
    using Oliviann.Reflection;

    #endregion Usings

    /// <summary>
    /// Represents a collection of commonly used extension methods for extending
    /// data service configuration objects.
    /// </summary>
    public static class DataServiceConfigurationExtensions
    {
        /// <summary>
        /// Sets the access rules for the specified configuration using the type
        /// class provided. Properties in type class must use
        /// <see cref="EntitySetAccessRuleAttribute"/>s  for the property to be
        /// found.
        /// </summary>
        /// <typeparam name="T">Type that defines the data service.</typeparam>
        /// <param name="configuration">The data service configuration
        /// information.</param>
        public static void SetEntityAccessRules<T>(this IDataServiceConfiguration configuration) where T : class
        {
            PropertyInfo[] classProperties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo property in classProperties)
            {
                var attributeObject = property.GetCustomAttributeCached<EntitySetAccessRuleAttribute>();
                if (attributeObject != null)
                {
                    configuration.SetEntitySetAccessRule(property.Name, attributeObject.Rights);
                }
            }
        }
    }
}

#endif