namespace Oliviann.Tests
{
    #region Usings

    using System;
    using Moq;
    using Xunit;

    #endregion Usings

    [Trait("Category", "CI")]
    public class BaseUriBuilderTests
    {
        [Fact]
        public void UriBuilderTest_Constructor()
        {
            var moc = new Mock<BaseUriBuilder> { CallBase = true };
            string result = moc.Object.ToString();

            Assert.Equal(":-1/", result);
        }

        [Fact]
        public void UriBuilderTest_HostUrl()
        {
            var moc = new Mock<BaseUriBuilder> { CallBase = true };
            moc.Object.HostUrl = "oliviann.com";
            string result = moc.Object.ToString();

            Assert.Equal("oliviann.com:-1/", result);
        }

        [Fact]
        public void UriBuilderTest_HostPath()
        {
            var moc = new Mock<BaseUriBuilder> { CallBase = true };
            moc.Object.HostUrl = "oliviann.com";
            moc.Object.Path = "item";
            string result = moc.Object.ToString();

            Assert.Equal("oliviann.com:-1/item", result);
        }

        [Theory]
        [InlineData(-25, "oliviann.com:-1/item")]
        [InlineData(80, "oliviann.com:80/item")]
        [InlineData(99999, "oliviann.com:-1/item")]
        public void UriBuilderTest_Ports(int portNumber, string expectedResult)
        {
            var moc = new Mock<BaseUriBuilder> { CallBase = true };
            moc.Object.HostUrl = "oliviann.com";
            moc.Object.Path = "item";
            moc.Object.Port = portNumber;
            string result = moc.Object.ToString();

            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void UriBuilderTest_CustomParameter_NullParameter()
        {
            var moc = new Mock<BaseUriBuilder> { CallBase = true };
            moc.Object.HostUrl = "oliviann.com";
            moc.Object.Path = "ietm";
            moc.Object.Port = 12025;

            Assert.Throws<ArgumentNullException>(() => moc.Object.CustomParameter(null, "ABC123"));
        }

        [Fact]
        public void UriBuilderTest_CustomParameter_EmptyParameter()
        {
            var moc = new Mock<BaseUriBuilder> { CallBase = true };
            moc.Object.HostUrl = "oliviann.com";
            moc.Object.Path = "ietm";
            moc.Object.Port = 12025;
            moc.Object.CustomParameter(string.Empty, "ABC123");
            string result = moc.Object.ToString();

            Assert.Equal("oliviann.com:12025/ietm?=ABC123", result);
        }

        [Fact]
        public void UriBuilderTest_CustomParameter_SingleParameter()
        {
            var moc = new Mock<BaseUriBuilder> { CallBase = true };
            moc.Object.HostUrl = "oliviann.com";
            moc.Object.Path = "item";
            moc.Object.Port = 12025;
            moc.Object.CustomParameter("Param1", "ABC123");
            string result = moc.Object.ToString();

            Assert.Equal("oliviann.com:12025/item?Param1=ABC123", result);
        }

        [Fact]
        public void UriBuilderTest_CustomParameter_MultipleParameters()
        {
            var moc = new Mock<BaseUriBuilder> { CallBase = true };
            moc.Object.HostUrl = "oliviann.com";
            moc.Object.Path = "item";
            moc.Object.Port = 12025;
            moc.Object.CustomParameter("Param1", "ABC123");
            moc.Object.CustomParameter("Param2", "153webfdysfdg");
            moc.Object.CustomParameter("Param3", "taco bell");
            string result = moc.Object.ToString();

            Assert.Equal("oliviann.com:12025/item?Param1=ABC123&Param2=153webfdysfdg&Param3=taco bell", result);
        }

        [Fact]
        public void UriBuilderTest_CustomParameter_ChangeParameter()
        {
            var moc = new Mock<BaseUriBuilder> { CallBase = true };
            moc.Object.HostUrl = "oliviann.com";
            moc.Object.Path = "item";
            moc.Object.Port = 12025;
            moc.Object.CustomParameter("Param1", "ABC123");

            string result = moc.Object.ToString();
            Assert.Equal("oliviann.com:12025/item?Param1=ABC123", result);

            moc.Object.CustomParameter("Param1", "DEF987");
            result = moc.Object.ToString();
            Assert.Equal("oliviann.com:12025/item?Param1=DEF987", result);
        }
    }
}