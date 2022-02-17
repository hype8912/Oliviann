namespace Oliviann.Tests.Windows
{
    #region Usings

    using System;
    using System.Windows.Forms;
    using Oliviann.Windows.Forms;
    using Xunit;

    #endregion Usings

    [Trait("Category", "CI")]
    public class WindowWrapperTests
    {
        [Fact]
        public void WindowWrapper_ctorTest_DefaultHandle()
        {
            var ptr = default(IntPtr);
            IWin32Window wrapper = new WindowWrapper(ptr);

            IntPtr result = wrapper.Handle;
            Assert.Equal(ptr, result);
        }
    }
}