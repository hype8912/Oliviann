#if NET35_OR_GREATER

namespace Oliviann.Data.Services
{
    #region Usings

    using System;
    using System.Data.Services;

    #endregion Usings

    /// <summary>
    /// Represents an attribute for properties to set entity access rights.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public sealed class EntitySetAccessRuleAttribute : Attribute
    {
        #region Properties

        /// <summary>
        /// Gets or sets the access rights to be granted to this resource,
        /// passed as an <see cref="EntitySetRights"/> value.
        /// </summary>
        /// <value>
        /// The access rights to be granted to this resource.
        /// </value>
        public EntitySetRights Rights { get; set; }

        #endregion Properties
    }
}

#endif