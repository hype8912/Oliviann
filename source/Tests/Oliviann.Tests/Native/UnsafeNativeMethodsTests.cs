namespace Oliviann.Tests.Native
{
    #region Usings

    using System;
    using System.Diagnostics;
    using Oliviann.Native;
    using Xunit;

    #endregion Usings

    public class UnsafeNativeMethodsTests
    {
        #region CTL Code Tests

        [Fact]
        [Trait("Category", "CI")]
        public void CTL_Code_HighFunction()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => UnsafeNativeMethods.CTL_CODE(32, 4096, 0, 0));
        }

        [Fact]
        [Trait("Category", "CI")]
        public void CTL_Code_GoodDeviceType()
        {
            uint result = UnsafeNativeMethods.CTL_CODE(32, 32, 0, 0);
            Assert.Equal(2097280U, result);
        }

        #endregion CTL Code Tests

        #region SetProcessForeground Tests

        /// <summary>
        /// Verifies passing a null process name doesn't throw an exception.
        /// </summary>
        [Fact]
        [Trait("Category", "CI")]
        public void SetProcessForegroundTest_NullProcess()
        {
            const string ProcessName = null;
            UnsafeNativeMethods.SetProcessForeground(ProcessName);
        }

        /// <summary>
        /// Verifies passing a process name that doesn't exist doesn't throw an
        /// exception.
        /// </summary>
        [Fact]
        [Trait("Category", "CI")]
        public void SetProcessForegroundTest_BadProcessName()
        {
            const string ProcessName = "tacobell";
            UnsafeNativeMethods.SetProcessForeground(ProcessName);
        }

        /// <summary>
        /// Verifies passing a valid process name doesn't throw an exception.
        /// </summary>
        [Fact]
        [Trait("Category", "CI")]
        public void SetProcessForegroundTest_GoodProcessName()
        {
            const string ProcessName = "explorer";
            UnsafeNativeMethods.SetProcessForeground(ProcessName);
        }

        #endregion SetProcessForeground Tests

        #region FindWindow Tests

        /// <summary>
        /// Verifies a null class name and window name returns any random open
        /// window handle.
        /// </summary>
        [Fact]
        [Trait("Category", "CI")]
        public void FindWindowTest_NullClassName_NullWindowName()
        {
            string className = null;
            string windowName = null;

            IntPtr result = UnsafeNativeMethods.FindWindow(className, windowName);
            Trace.WriteLine("Result: " + result);
            Assert.NotEqual(IntPtr.Zero, result);
        }

        /// <summary>
        /// Verifies providing a class name still returns a window for Notepad
        /// id it's open.
        /// </summary>
        [Fact(Skip = "Ignore")]
        [Trait("Category", "Developer")]
        [Trait("Category", "SkipWhenLiveUnitTesting")]
        public void FindWindowTest_NullWindowName()
        {
            string className = "Notepad";
            string windowName = null;

            IntPtr result = UnsafeNativeMethods.FindWindow(className, windowName);
            Trace.WriteLine("Result: " + result);
            Assert.NotEqual(IntPtr.Zero, result);
        }

        /// <summary>
        /// Verifies providing a null class name still finds the specified
        /// windows name if it's open.
        /// </summary>
        [Fact(Skip = "Ignore")]
        [Trait("Category", "Developer")]
        [Trait("Category", "SkipWhenLiveUnitTesting")]
        public void FindWindowTest_NullClassName()
        {
            string className = null;
            string windowName = "Untitled - Notepad";

            IntPtr result = UnsafeNativeMethods.FindWindow(className, windowName);
            Trace.WriteLine("Result: " + result);
            Assert.NotEqual(IntPtr.Zero, result);
        }

        /// <summary>
        /// Verifies a valid window is found and returns a non-zero window
        /// pointer value.
        /// </summary>
        [Fact(Skip = "Ignore")]
        [Trait("Category", "Developer")]
        [Trait("Category", "SkipWhenLiveUnitTesting")]
        public void FindWindowTest_GoodNames()
        {
            string className = "Notepad";
            string windowName = "Untitled - Notepad";

            IntPtr result = UnsafeNativeMethods.FindWindow(className, windowName);
            Trace.WriteLine("Result: " + result);
            Assert.NotEqual(IntPtr.Zero, result);
        }

        /// <summary>
        /// Verifies that no pointer value is returned for a window that is not
        /// open.
        /// </summary>
        [Fact]
        [Trait("Category", "CI")]
        public void FindWindowTest_ValidNames_MissingWindow()
        {
            string className = "Notepad2";
            string windowName = "Untitled - Notepad2";

            IntPtr result = UnsafeNativeMethods.FindWindow(className, windowName);
            Trace.WriteLine("Result: " + result);
            Assert.Equal(IntPtr.Zero, result);
        }

        #endregion FindWindow Tests
    }
}