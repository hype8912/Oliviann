namespace Oliviann.Native
{
    /// <summary>
    /// Determines how the resource will be displayed.
    /// </summary>
    public enum ResourceDisplayType : uint
    {
        /// <summary>
        /// The method used to display the object does not matter.
        /// </summary>
        Generic = 0x0,

        /// <summary>
        /// The object should be displayed as a domain.
        /// </summary>
        Domain = 0x1,

        /// <summary>
        /// The object should be displayed as a server.
        /// </summary>
        Server = 0x2,

        /// <summary>
        /// The object should be displayed as a share.
        /// </summary>
        Share = 0x3,

        /// <summary>
        /// The object should be displayed as a file.
        /// </summary>
        File = 0x4,

        /// <summary>
        /// The object should be displayed as a group.
        /// </summary>
        Group = 0x5,

        /// <summary>
        /// The object should be displayed as a network.
        /// </summary>
        Network = 0x6,

        /// <summary>
        /// The object should be displayed as a root.
        /// </summary>
        Root = 0x7,

        /// <summary>
        /// The object should be displayed as a admin share.
        /// </summary>
        ShareAdmin = 0x8,

        /// <summary>
        /// The object should be displayed as a directory.
        /// </summary>
        Directory = 0x9,

        /// <summary>
        /// The object should be displayed as a tree.
        /// </summary>
        Tree = 0xa,

        /// <summary>
        /// The object should be displayed as a NDS container.
        /// </summary>
        NDSContainer = 0xb
    }
}