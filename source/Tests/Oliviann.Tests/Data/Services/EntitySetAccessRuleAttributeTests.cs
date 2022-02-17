namespace Oliviann.Tests.Data.Services
{
    #region Usings

    using System.Data.Services;
    using Oliviann.Data.Services;
    using Xunit;

    #endregion Usings

    [Trait("Category", "CI")]
    public class EntitySetAccessRuleAttributeTests
    {
        /// <summary>
        /// Verifies when creating a new instance that the default value for
        /// Rights is correct.
        /// </summary>
        [Fact]
        public void EntitySetAccessRuleAttribute_Defaults()
        {
            var att = new EntitySetAccessRuleAttribute();
            EntitySetRights rights = att.Rights;

            Assert.Equal(EntitySetRights.None, rights);
        }

        /// <summary>
        /// Verifies setting the Rights value is that same value when retrieved.
        /// </summary>
        [Fact]
        public void EntitySetAccessRuleAttribute_SetRights()
        {
            var att = new EntitySetAccessRuleAttribute { Rights = EntitySetRights.All };
            EntitySetRights rightsSet = att.Rights;
            Assert.Equal(EntitySetRights.All, rightsSet);
        }
    }
}