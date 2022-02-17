namespace Oliviann.Tests.TestObjects
{
    #region Usings

    using System;

    #endregion Usings

    /// <summary>
    /// Represents a
    /// </summary>
    public class TestClassWithNullableProperties
    {
        #region Constructor/Destructor

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="TestClassWithNullableProperties"/> class.
        /// </summary>
        public TestClassWithNullableProperties()
        {
        }

        #endregion Constructor/Destructor

        #region Properties

        public int? PropInt { get; set; }

        public DateTime? PropDateTime { get; set; }

        public bool? PropBool { get; set; }

        #endregion Properties
    }
}