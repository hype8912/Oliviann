namespace Oliviann.Native
{
    #region Usings

    using System;

    #endregion Usings

    /// <summary>
    /// Resource usage type to be enumerated.
    /// </summary>
    [Flags]
    public enum ResourceUsage : uint
    {
        /// <summary>
        /// All resources.
        /// </summary>
        AllResources = 0,

        /// <summary>
        /// All connectable resources.
        /// </summary>
        Connectable = 0x1,

        /// <summary>
        /// All container resources.
        /// </summary>
        Container = 0x2,

        /// <summary>
        /// The resource is not a local device.
        /// </summary>
        NoLocalDevice = 0x4,

        /// <summary>
        /// The resource is a sibling. This value is not used by Windows.
        /// </summary>
        Sibling = 0x8,

        /// <summary>
        /// The resource must be attached. This value specifies that a function
        /// to enumerate resource this should fail if the caller is not
        /// authenticated, even if the network permits enumeration without
        /// authentication.
        /// </summary>
        Attached = 0x10,

        /// <summary>
        /// Setting this value is equivalent to setting Connectable, Container,
        /// and Attached.
        /// </summary>
        All = 0x13,

        /// <summary>
        /// Windows reserved resource.
        /// </summary>
        Reserved = 0x80000000u
    }
}