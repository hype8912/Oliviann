namespace Oliviann.Tests.ComponentModel
{
    #region Usings

    using Oliviann.ComponentModel;
    using Xunit;

    #endregion Usings

    [Trait("Category", "CI")]
    public class EventArgs_T1_T2_T3_Tests
    {
        [Fact]
        public void EventArgsTTest_NullObjects()
        {
            var ea = new EventArgs<object, object, object>(null, null, null);

            Assert.Null(ea.Value1);
            Assert.Null(ea.Value2);
            Assert.Null(ea.Value3);
        }

        [Fact]
        public void EventArgsTTest_NullStrings()
        {
            var ea = new EventArgs<string, string, string>(null, null, null);

            Assert.Null(ea.Value1);
            Assert.Null(ea.Value2);
            Assert.Null(ea.Value3);
        }

        [Fact]
        public void EventArgsTTest_Strings()
        {
            var ea = new EventArgs<string, string, string>("Hello", "World", "Texas");

            Assert.Equal("Hello", ea.Value1);
            Assert.Equal("World", ea.Value2);
            Assert.Equal("Texas", ea.Value3);
        }
    }
}