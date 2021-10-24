namespace Oliviann.Caching
{
    /// <summary>
    /// Defines the target cache location.
    /// </summary>
    public enum CacheTarget
    {
        /// <summary>
        /// Use to store data scoped for the default level.
        /// </summary>
        Default,

        /// <summary>
        /// Use to store data scoped to the User level.
        /// </summary>
        User,

        /// <summary>
        /// Use to store data scoped to the application level.
        /// </summary>
        Application,

        /// <summary>
        /// Use to store data scoped to the Tenant level.
        /// </summary>
        Tenant,

        /// <summary>
        /// Use to store and share data across the entire Platform.
        /// </summary>
        Grid
    }
}