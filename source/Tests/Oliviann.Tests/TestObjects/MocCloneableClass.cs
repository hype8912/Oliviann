namespace Oliviann.Tests.TestObjects
{
    #region Usings

    using System;

    #endregion Usings

    /// <summary>
    /// Represents a generic mock cloneable class.
    /// </summary>
    [Serializable]
    public class MocCloneableClass : ICloneable
    {
        #region Constructor/Destructor

        public MocCloneableClass()
        {
            this.PropTestClass = new GenericMocTestClass();
        }

        #endregion Constructor/Destructor

        #region Properties

        public GenericMocTestClass PropTestClass { get; private set; }

        public int PropInt { get; set; }

        #endregion Properties

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}