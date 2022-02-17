namespace Oliviann.Data.Tests.Enums
{
    #region Usings

    using Xunit;

    #endregion Usings

    [Trait("Category", "CI")]
    public class ProviderAttributeTests
    {
        #region cTor Tests

        /// <summary>
        /// Verifies creating a default instance the values are the same.
        /// </summary>
        [Fact]
        public void ProviderAttributeTest_DefaultInstance_NoName()
        {
            var att = new ProviderAttribute();
            Assert.True(att.Name == string.Empty, "Name was not correct.[{0}]".FormatWith(att.Name));

            int hash = att.GetHashCode();
            Assert.True(hash != 0, "Hash code is not correct.[{0}]".FormatWith(hash));
        }

        /// <summary>
        /// Verifies creating a default instance with a name the values are the
        /// same as set.
        /// </summary>
        [Fact]
        public void ProviderAttributeTest_DefaultInstance_WithName()
        {
            var att = new ProviderAttribute("Microsoft.ACE.OLEDB.12.0");
            Assert.True(att.Name == "Microsoft.ACE.OLEDB.12.0", "Name was not correct.[{0}]".FormatWith(att.Name));

            int hash = att.GetHashCode();
            Assert.True(hash != 0, "Hash code is not correct.[{0}]".FormatWith(hash));
        }

        #endregion cTor Tests

        #region Equals Tests

        /// <summary>
        /// Verifies comparing an instance to a wrong type object returns false
        /// and no exceptions.
        /// </summary>
        [Theory]
        [InlineData(null, "Microsoft.ACE.OLEDB.12.0")]
        [InlineData("", "Microsoft.ACE.OLEDB.12.0")]
        public void ProviderAttributeTest_EqualNonTypes(object value, string instanceName)
        {
            object testValue = value;
            var att = new ProviderAttribute(instanceName);
            bool result = att.Equals(testValue);

            Assert.False(result, "Result was found to be the same as test value.");
        }

        /// <summary>
        /// Verifies comparing an instance to a matching and non-matching instance
        /// </summary>
        [Theory]
        [InlineData("Microsoft.ACE.OLEDB.14.0", "Microsoft.ACE.OLEDB.12.0", false)]
        [InlineData("Microsoft.ACE.OLEDB.12.0", "Microsoft.ACE.OLEDB.12.0", true)]
        public void ProviderAttributeTest_EqualsInstances(string instance1, string instance2, bool expectedResult)
        {
            object testValue = new ProviderAttribute(instance1);
            var att = new ProviderAttribute(instance2);
            bool result = att.Equals(testValue);

            Assert.True(
                result == expectedResult,
                "Result was found to not be the same as test value.1[{0}] 2[{1}]".FormatWith(instance1, instance2));
        }

        /// <summary>
        /// Verifies comparing an instance to a string returns false and no
        /// exceptions.
        /// </summary>
        [Fact]
        public void ProviderAttributeTest_EqualsSameInstance()
        {
            var att = new ProviderAttribute("Microsoft.ACE.OLEDB.12.0");
            bool result = att.Equals(att);

            Assert.True(result, "Result was found to not be the same as test value.");
        }

        #endregion Equals Tests
    }
}