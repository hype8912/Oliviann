namespace Oliviann.Tests.IO.INI
{
    #region Usings

    using Oliviann.IO;
    using Xunit;

    #endregion Usings

    [Trait("Category", "CI")]
    public class SectionTests
    {
        #region Constructor Tests

        [Fact]
        public void Section_ctorTest_NewEmpty()
        {
            var sc = new Section();

            Assert.Equal(string.Empty, sc.Name);
            Assert.NotNull(sc.Properties);
            Assert.Empty(sc.Properties);
        }

        [Fact]
        public void Section_ctorTest_NewNotEmpty()
        {
            var sc = new Section("User");

            Assert.Equal("User", sc.ToString());
            Assert.NotNull(sc.Properties);
            Assert.Empty(sc.Properties);
        }

        #endregion Constructor Tests

        #region AddProperty Tests

        [Fact]
        public void SectionAddPropertyTest_NullKey()
        {
            var sc = new Section("User");
            sc.AddProperty(null, "World");

            Assert.NotNull(sc.Properties);
            Assert.Single(sc.Properties);
        }

        [Fact]
        public void SectionAddPropertyTest_NullValue()
        {
            var sc = new Section("User");
            sc.AddProperty("Hello", null);

            Assert.NotNull(sc.Properties);
            Assert.Single(sc.Properties);
        }

        [Fact]
        public void SectionAddPropertyTest_AddMultiple()
        {
            var sc = new Section("User");
            sc.AddProperty("Hello", "World");
            sc.AddProperty("Taco", "Bell");
            sc.AddProperty("Milhouse", "Houten");

            Assert.NotNull(sc.Properties);
            Assert.Equal(3, sc.Properties.Count);
        }

        [Fact]
        public void SectionAddPropertyTest_AddDuplicate()
        {
            var sc = new Section("User");
            sc.AddProperty("Hello", "World");
            sc.AddProperty("Taco", "Bell");
            sc.AddProperty("Hello", "World");

            Assert.NotNull(sc.Properties);
            Assert.Equal(2, sc.Properties.Count);
        }

        [Fact]
        public void SectionAddPropertyTest_NewValue()
        {
            var sc = new Section("User");
            sc.AddProperty("Hello", "World");
            sc.AddProperty("Taco", "Bell");
            sc.AddProperty("Hello", "Planet");

            Assert.NotNull(sc.Properties);
            Assert.Equal(2, sc.Properties.Count);
            Assert.Equal("Planet", sc.GetPropertyValue("Hello"));
        }

        #endregion AddProperty Tests

        #region GetPropertyValue Tests

        [Fact]
        public void SectionGetPropertyValueTest_NullKey()
        {
            var sc = new Section("User");
            sc.AddProperty("Hello", "World");
            sc.AddProperty("Taco", "Bell");
            sc.AddProperty("Hello", "Planet");

            Assert.Equal(string.Empty, sc.GetPropertyValue(null));
        }

        [Fact]
        public void SectionGetPropertyValueTest_NonKey()
        {
            var sc = new Section("User");
            sc.AddProperty("Hello", "World");
            sc.AddProperty("Taco", "Bell");
            sc.AddProperty("Hello", "Planet");

            Assert.Equal(string.Empty, sc.GetPropertyValue("Valdez"));
        }

        [Fact]
        public void SectionGetPropertyValueTest_KnownKey()
        {
            var sc = new Section("User");
            sc.AddProperty("Hello", "World");
            sc.AddProperty("Taco", "Bell");
            sc.AddProperty("Hello", "Planet");

            Assert.Equal("Bell", sc.GetPropertyValue("Taco"));
            Assert.Equal(string.Empty, sc.GetPropertyValue("taco"));
        }

        #endregion GetPropertyValue Tests
    }
}