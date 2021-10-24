namespace Oliviann.Reflection
{
    #region Usings

    using System;
    using System.IO;
    using System.Linq;
    using System.Reflection;

    #endregion Usings

    /// <summary>
    /// Represents a collection of commonly used extension methods for extending
    /// reflection.
    /// </summary>
    public static class ReflectionExtensions
    {
        /// <summary>
        /// Gets the specified assemblies
        /// <see cref="AssemblyInformationalVersionAttribute"/> value.
        /// </summary>
        /// <param name="assy">The assembly to retrieve information.</param>
        /// <returns>The specified assemblies AssemblyInformationalVersion
        /// value; otherwise, null.</returns>
        public static string GetInformationalVersion(this Assembly assy)
        {
            if (assy == null)
            {
                return null;
            }

#if NET40
            var att = Attribute.GetCustomAttribute(assy, typeof(AssemblyInformationalVersionAttribute)) as
                    AssemblyInformationalVersionAttribute;
            return att?.InformationalVersion;
#else
            CustomAttributeData attr = assy.CustomAttributes.FirstOrDefault(a => a.AttributeType == typeof(AssemblyInformationalVersionAttribute));
            return attr?.ConstructorArguments.FirstOrDefault().Value?.ToString();
#endif
        }

#if !NETSTANDARD1_3

        /// <summary>
        /// Gets the current executing directory for the specified assembly.
        /// </summary>
        /// <param name="assy">The assembly to retrieve information.</param>
        /// <returns>The executing directory path of the specified assembly.
        /// </returns>
        public static string GetCurrentExecutingDirectory(this Assembly assy)
        {
            ADP.CheckArgumentNull(assy, nameof(assy));

            var uri = new UriBuilder(assy.CodeBase);
            return Path.GetDirectoryName(Uri.UnescapeDataString(uri.Path));
        }

#endif
    }
}