namespace Oliviann.Native
{
    /// <summary>
    /// Scope of the resource enumeration.
    /// </summary>
    public enum ResourceScope : uint
    {
        /// <summary>
        /// Enumerate currently connected resources.
        /// </summary>
        Connected = 0x1,

        /// <summary>
        /// Enumerate all resources on the network.
        /// </summary>
        GlobalNet = 0x2,

        /// <summary>
        /// Enumerate remembered (persistent) connections.
        /// </summary>
        Remembered = 0x3,

        /// <summary>
        /// Enumerate recently viewed resources.
        /// </summary>
        Recent = 0x4,

        /// <summary>
        /// Enumerate only resources in the network context of the caller.
        /// Specify this value for a Network Neighborhood view.
        /// </summary>
        Context = 0x5
    }
}