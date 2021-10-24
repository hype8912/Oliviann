#if NETSTANDARD1_3
namespace Oliviann
{
    #region Usings

    using System;

    #endregion

    /// <summary>
    /// Represents that a field of a serializable class should not be
    /// serialized.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, Inherited = false)]
    public sealed class NonSerializedAttribute : Attribute
    {
        #region Constructor/Destructor

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="NonSerializedAttribute"/> class.
        /// </summary>
        public NonSerializedAttribute()  {  }

        #endregion
    }
}
#endif