namespace Oliviann.Data
{
    #region Usings

    using System;

    #endregion Usings

    /// <summary>
    /// Specifies the name of the database provider.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public sealed class ProviderAttribute : Attribute
    {
        #region Constructor/Deconstructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ProviderAttribute"/>
        /// class.
        /// </summary>
        public ProviderAttribute() : this(string.Empty)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProviderAttribute"/>
        /// class.
        /// </summary>
        /// <param name="name">The provider name.</param>
        public ProviderAttribute(string name)
        {
            this.Name = name;
        }

        #endregion Constructor/Deconstructor

        #region Properties

        /// <summary>
        /// Gets the providers name.
        /// </summary>
        /// <value>The provider name.</value>
        public string Name { get; }

        /// <summary>
        /// Gets or sets the type of the database provider factory.
        /// </summary>
        /// <value>
        /// The type of the database provider factory.
        /// </value>
        public Type DbProviderFactoryType { get; set; }

        #endregion Properties

        #region Overrides

        /// <summary>
        /// Determines whether the specified <see cref="object"/> is equal to
        /// this instance.
        /// </summary>
        /// <param name="obj">The <see cref="object"/> to compare with this
        /// instance.</param>
        /// <returns>
        /// True if the specified <see cref="object"/> is equal to this
        /// instance; otherwise, false.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (obj == this)
            {
                return true;
            }

            return obj is ProviderAttribute attribute && attribute.Name.EqualsOrdinal(this.Name);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing
        /// algorithms and data structures like a hash table.
        /// </returns>
        public override int GetHashCode() => this.Name.GetHashCode();

        #endregion Overrides
    }
}