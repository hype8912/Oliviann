namespace Oliviann.Native
{
    /// <summary>
    /// Specifies a specific resource type.
    /// </summary>
    public enum ResourceType : uint
    {
        /// <summary>
        /// All resources.
        /// </summary>
        Any = 0x0,

        /// <summary>
        /// Disk resources.
        /// </summary>
        Disk = 0x1,

        /// <summary>
        /// Print resources.
        /// </summary>
        Print = 0x2,

        /// <summary>
        /// Reserved for operating system.
        /// </summary>
        Reserved = 0x8,

        /// <summary>
        /// Unknown resources.
        /// </summary>
        Unknown = 0xffffffffu
    }
}