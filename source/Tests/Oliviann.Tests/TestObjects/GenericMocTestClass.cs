namespace Oliviann.Tests.TestObjects
{
    #region Usings

    using System;
    using System.ComponentModel.Composition;

    #endregion Usings

    /// <summary>
    /// Represents a generic mock test class.
    /// </summary>
    [Serializable]
    [Export("GenericMocTest", typeof(IGenericMocTestClass))]
    public class GenericMocTestClass : IGenericMocTestClass
    {
        #region Fields

        private string _setOnlyProp;

        #endregion Fields

        #region Properties

        public string PropString { get; set; }

        public bool PropBool { get; set; }

        public int PropInt { get; set; }

        public string SetOnlyProp
        {
           set { this._setOnlyProp = value; }
        }

        #endregion Properties

        #region Methods

        public string GetSetOnlyPropValue()
        {
            return this._setOnlyProp;
        }

        #endregion Methods
    }
}