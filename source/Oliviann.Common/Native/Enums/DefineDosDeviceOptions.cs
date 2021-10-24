namespace Oliviann.Native
{
    #region Usings

    using System;

    #endregion Usings

    /// <summary>
    /// Defines the controllable aspects of the dos options when defining a DOS
    /// device.
    /// </summary>
    [Flags]
    public enum DefineDosDeviceOptions : uint
    {
        /// <summary>
        /// If this value is specified along with DDD_REMOVE_DEFINITION, the
        /// function will use an exact match to determine which mapping to
        /// remove. Use this value to ensure that you do not delete something
        /// that you did not define.
        /// </summary>
        ExactMatchOnRemove = 0x00000004,

        /// <summary>
        /// Do not broadcast the Windows message setting changed message. By
        /// default, this message is broadcast to notify the shell and
        /// applications of the change.
        /// </summary>
        NoBroadcastSystem = 0x00000008,

        /// <summary>
        /// Defines using the target path string as is. Otherwise, it is
        /// converted from an MS-DOS path to a path.
        /// </summary>
        RawTargetPath = 0x00000001,

        /// <summary>
        /// Removes the specified definition for the specified device. To
        /// determine which definition to remove, the function walks the list of
        /// mappings for the device, looking for a match of the target path
        /// against a prefix of each mapping associated with this device. The
        /// first mapping that matches is the one removed, and then the function
        /// returns. If the target path is <c>null</c>, the function will remove
        /// the first mapping associated with the device and pop the most recent
        /// one pushed. If there is nothing left to pop, the device name will be
        /// removed. If this value is not specified, the string pointed to by
        /// the target path parameter will become the new mapping for this
        /// device.
        /// </summary>
        RemoveDefinition = 0x00000002
    }
}