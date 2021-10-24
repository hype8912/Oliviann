namespace Oliviann.ComponentModel.Composition
{
    #region Usings

    using System;
#if NET35|| NET40 || NET45 || NET46 || NET47 || NET48
    using System.ComponentModel.Composition;
#else
    using System.Composition;
#endif

    #endregion Usings

    /// <summary>
    /// Specifies that a type provides a particular export.
    /// </summary>
    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ExportServiceMetadataAttribute : ExportAttribute
    {
        #region Constructor/Destructor

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="ExportServiceMetadataAttribute"/> class.
        /// </summary>
        public ExportServiceMetadataAttribute() : this(null, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="ExportServiceMetadataAttribute"/> class.
        /// </summary>
        /// <param name="contractName">The contract name that is used to export
        /// the type or member marked with this attribute, or null or an empty
        /// string ("") to use the default contract name.</param>
        public ExportServiceMetadataAttribute(string contractName) : this(contractName, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="ExportServiceMetadataAttribute"/> class.
        /// </summary>
        /// <param name="contractType">The type to export.</param>
        public ExportServiceMetadataAttribute(Type contractType) : this(null, contractType)
        {
        }

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="ExportServiceMetadataAttribute"/> class.
        /// </summary>
        /// <param name="contractName">The contract name that is used to export
        /// the type or member marked with this attribute, or null or an empty
        /// string ("") to use the default contract name.</param>
        /// <param name="contractType">The type to export.</param>
        public ExportServiceMetadataAttribute(string contractName, Type contractType) : base(contractName, contractType)
        {
            this.LoadOrder = uint.MaxValue;
        }

        #endregion Constructor/Destructor

        #region Properties

        /// <summary>
        /// Gets or sets the name of the service host.
        /// </summary>
        /// <value>
        /// The name of the service host.
        /// </value>
        public string HostName { get; set; }

        /// <summary>
        /// Gets or sets the load order the service will be loaded in.
        /// </summary>
        /// <value>
        /// The load order the service will be loaded in.
        /// </value>
        public uint LoadOrder { get; set; }

        #endregion Properties
    }
}