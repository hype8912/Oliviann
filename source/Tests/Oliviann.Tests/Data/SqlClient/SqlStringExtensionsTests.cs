namespace Oliviann.Tests.Data.SqlClient
{
    #region Usings

    using System.Data.SqlTypes;
    using Oliviann.Data.SqlClient;
    using Xunit;

    #endregion Usings

    [Trait("Category", "CI")]
    public class SqlStringExtensionsTests
    {
        [Fact]
        public void ValueOrDefault_Null()
        {
            SqlString value = null;

            string result = value.ValueOrDefault();
            Assert.Null(result);
        }

        [Fact]
        public void ValueOrDefault_NullSqlString()
        {
            var value = new SqlString();
            Assert.True(value.IsNull, "Value was found not to be null.");

            string result = value.ValueOrDefault();
            Assert.Null(result);
        }

        [Fact]
        public void ValueOrDefault_NullSqlStringWithDefaultValue()
        {
            var value = new SqlString();
            Assert.True(value.IsNull, "Value was found not to be null.");

            string result = value.ValueOrDefault("HAHA");
            Assert.Equal("HAHA", result);
        }

        [Fact]
        public void ValueOrDefault_WithValue()
        {
            var value = new SqlString("Taco Bell");
            Assert.False(value.IsNull, "Value was found to be null.");

            string result = value.ValueOrDefault();
            Assert.Equal("Taco Bell", result);
        }

        [Fact]
        public void ValueOrDefault_WithValueAndDefaultValue()
        {
            var value = new SqlString("Taco Bell");
            Assert.False(value.IsNull, "Value was found to be null.");

            string result = value.ValueOrDefault("Pizza Hut");
            Assert.Equal("Taco Bell", result);
        }
    }
}