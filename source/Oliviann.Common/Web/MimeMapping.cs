#if NETFRAMEWORK

namespace Oliviann.Web
{
    #region Usings

    using System;
    using System.Collections.Generic;

#if NET40 || NET35
    using System.Reflection;
    using System.Web;
#endif

    #endregion Usings

    /// <summary>
    /// Maps document extensions to content MIME types.
    /// </summary>
    public static class MimeMapping
    {
        #region Fields

        /// <summary>
        /// The dictionary of common MIME types.
        /// </summary>
        private static readonly Dictionary<string, string> mappings;

#if NET35 || NET40

        /// <summary>
        /// The MIME mapping method information from reflection for NET 35 or NET 40.
        /// </summary>
        private static MethodInfo mimeMappingMethodNet40;

#endif

        #endregion Fields

        #region Constructor

        /// <summary>
        /// Initializes the <see cref="MimeMapping"/> class.
        /// </summary>
        static MimeMapping()
        {
            mappings = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            PopulateMappings();

#if NET35 || NET40
            SetNet40MimeMapMethodInfo();
#endif
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// Gets the MIME mapping for the specified file name.
        /// </summary>
        /// <param name="fileExtension">The file extension that is used to
        /// determine the MIME type.</param>
        /// <returns>The MIME mapping for the specified file name.</returns>
        public static string GetMimeMapping(string fileExtension)
        {
            ADP.CheckArgumentNull(fileExtension, nameof(fileExtension));
            if (mappings.TryGetValue(fileExtension, out string result))
            {
                return result;
            }

#if NET35 || NET40
            return (string)mimeMappingMethodNet40.Invoke(null, new object[] { fileExtension });
#else
            return System.Web.MimeMapping.GetMimeMapping(fileExtension);
#endif
        }

        /// <summary>
        /// Populates all the mappings to the dictionary.
        /// </summary>
        private static void PopulateMappings()
        {
            mappings.Add(".json", "application/json");
        }

#if NET35 || NET40

        /// <summary>
        /// Sets the NET 40 MIME map method information from reflection.
        /// </summary>
        private static void SetNet40MimeMapMethodInfo()
        {
            Assembly assy = Assembly.GetAssembly(typeof(HttpApplication));
            Type mimeMappingType = assy.GetType("System.Web.MimeMapping");
            mimeMappingMethodNet40 = mimeMappingType.GetMethod(
                "GetMimeMapping",
                BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic
                | BindingFlags.FlattenHierarchy);
        }

#endif

        #endregion Methods
    }
}

#endif