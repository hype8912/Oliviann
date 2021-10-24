#if NETSTANDARD1_3
namespace Oliviann.ComponentModel
{
    #region Usings

    using System;

    #endregion

    /// <summary>
    /// Specifies a description for a property or event.
    /// </summary>
    [AttributeUsage(AttributeTargets.All)]
    public class DescriptionAttribute : Attribute
    {
        #region Fields

        /// <summary>
        /// Specifies the default value for the
        /// <see cref='DescriptionAttribute'/>, which is an empty string ("").
        /// This <see langword='static'/> field is read-only.
        /// </summary>
        public static readonly DescriptionAttribute Default = new DescriptionAttribute();

        #endregion

        #region Constructor/Destructor

        /// <summary>
        /// Initializes a new instance of the <see cref="DescriptionAttribute"/>
        /// class.
        /// </summary>
        public DescriptionAttribute() : this(string.Empty)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DescriptionAttribute"/>
        /// class.
        /// </summary>
        /// <param name="description">The description text.</param>
        public DescriptionAttribute(string description)
        {
            DescriptionValue = description;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the description stored in this attribute.
        /// </summary>
        public virtual string Description => DescriptionValue;

        /// <summary>
        /// Read/Write property that directly modifies the string stored in the
        /// description attribute. The default implementation of the
        /// <see cref="Description"/> property simply returns this value.
        /// </summary>
        protected string DescriptionValue { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Determines whether the specified object, is equal to this instance.
        /// </summary>
        /// <param name="obj">The object to compare with this instance.</param>
        /// <returns>
        /// True if the specified object is equal to this instance; otherwise,
        /// false.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (obj == this)
            {
                return true;
            }

            DescriptionAttribute other = obj as DescriptionAttribute;
            return other != null && other.Description == Description;
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing
        /// algorithms and data structures like a hash table.
        /// </returns>
        public override int GetHashCode() => Description?.GetHashCode() ?? 0;

        #endregion
    }
}
#endif