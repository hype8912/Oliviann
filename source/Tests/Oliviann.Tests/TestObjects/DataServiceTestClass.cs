namespace Oliviann.Tests.TestObjects
{
    #region Usings

    using System.Data.Services;
    using Oliviann.Data.Services;

    #endregion Usings

    /// <summary>
    /// Represents a test class for testing the
    /// <see cref="Oliviann.Data.Services.DataServiceConfigurationExtensions.SetEntityAccessRules{T}"/>
    /// method.
    /// </summary>
    public class DataServiceTestClass
    {
        #region Properties

        [EntitySetAccessRule(Rights = EntitySetRights.AllRead)]
        public string StringProperty { get; set; }

        public int IntProperty { get; set; }

        [EntitySetAccessRule(Rights = EntitySetRights.AllWrite)]
        public object ObjectProperty { get; set; }

        #endregion Properties
    }
}