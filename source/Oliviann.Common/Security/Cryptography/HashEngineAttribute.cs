namespace Oliviann.Security.Cryptography
{
    #region Usings

    using System;

    #endregion Usings

    /// <summary>
    /// Represents an attribute for a type of hashing algorithm.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class HashEngineAttribute : Attribute
    {
        #region Constructor/Destructor

        /// <summary>
        /// Initializes a new instance of the <see cref="HashEngineAttribute" />
        /// class.
        /// </summary>
        /// <param name="hashType">The type of hashing algorithm.</param>
        public HashEngineAttribute(Type hashType) => this.HashType = hashType;

        #endregion Constructor/Destructor

        #region Properties

        /// <summary>
        /// Gets or sets the type of the hashing algorithm.
        /// </summary>
        /// <value>
        /// The type of the hashing algorithm.
        /// </value>
        public Type HashType { get; set; }

        #endregion Properties
    }
}