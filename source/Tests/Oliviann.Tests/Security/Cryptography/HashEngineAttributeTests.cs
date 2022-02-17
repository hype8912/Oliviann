namespace Oliviann.Tests.Security.Cryptography
{
    #region Usings

    using Oliviann.Security.Cryptography;
    using Xunit;

    #endregion Usings

    [Trait("Category", "CI")]
    public class HashEngineAttributeTests
    {
        /// <summary>
        /// Verifies the hash type is set by the constructor.
        /// </summary>
        [Fact]
        public void HashEngineAttributeTest_Instance()
        {
            var att = new HashEngineAttribute(typeof(string));

            Assert.Equal(typeof(string), att.HashType);
        }
    }
}