namespace Oliviann.Native
{
    #region Usings

    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.InteropServices;

    #endregion Usings

    /// <summary>
    /// Contains information about a network resource.
    /// </summary>
    [SuppressMessage("Microsoft.StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "This is a native code class.")]
    [StructLayout(LayoutKind.Sequential)]
    public class NetResource
    {
        /// <summary>
        /// The scope of the enumeration.
        /// </summary>
        public ResourceScope Scope;

        /// <summary>
        /// The type of resource.
        /// </summary>
        public ResourceType Type;

        /// <summary>
        /// The display options for the network object in a network browsing
        /// user interface.
        /// </summary>
        public ResourceDisplayType DisplayType;

        /// <summary>
        /// A set of bit flags describing how the resource can be used.
        /// </summary>
        public ResourceUsage Usage;

        /// <summary>
        /// A pointer to a NULL-terminated character string that specifies the
        /// name of a local device.
        /// </summary>
        public string LocalName;

        /// <summary>
        /// A pointer to a NULL-terminated character string that specifies the
        /// remote network name.
        /// </summary>
        public string RemoteName;

        /// <summary>
        /// A pointer to a NULL-terminated string that contains a comment
        /// supplied by the network provider.
        /// </summary>
        public string Comment;

        /// <summary>
        /// A pointer to a NULL-terminated string that contains the name of the
        /// provider that owns the resource. This member can be NULL if the
        /// provider name is unknown. To retrieve the provider name, you can
        /// call the <c>WNetGetProviderName</c> function.
        /// </summary>
        public string Provider;
    }
}