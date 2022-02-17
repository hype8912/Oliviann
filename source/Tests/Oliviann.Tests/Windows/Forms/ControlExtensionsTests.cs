namespace Oliviann.Tests.Windows.Forms
{
    #region Usings

    using System;
    using System.Windows.Forms;
    using System.Windows.Forms.Fakes;
    using Oliviann.Windows.Forms;
    using Microsoft.QualityTools.Testing.Fakes;
    using Xunit;

    #endregion Usings

    [Trait("Category", "CI")]
    public class ControlExtensionsTests
    {
        #region Disable Tests

        [Fact]
        public void ControlDisableTest_Null()
        {
            Control ctrl = null;
            ControlExtensions.Disable(ctrl);
        }

        [Fact]
        public void ControlDisableTest_Enabled()
        {
            var ctrl = new Control
                {
                    Enabled = true
                };

            Assert.True(ctrl.Enabled, "Control is disabled.");

            ctrl.Disable();
            Assert.False(ctrl.Enabled, "Control is enabled.");

            ctrl.Dispose();
        }

        #endregion Disable Tests

        #region Enable Tests

        [Fact]
        public void ControlEnableTest_Null()
        {
            Control ctrl = null;
            ControlExtensions.Enable(ctrl);
        }

        [Fact]
        public void ControlEnableTest_Disabled()
        {
            var ctrl = new Control
                {
                    Enabled = false
                };

            Assert.False(ctrl.Enabled, "Control is enabled.");

            ctrl.Enable();
            Assert.True(ctrl.Enabled, "Control is disabled.");

            ctrl.Dispose();
        }

        #endregion Enable Tests

        #region InvokeIfRequired Tests

        [Fact]
        public void InvokeIfRequiredTest1_NullControl()
        {
            Control ctrl = null;
            ControlExtensions.InvokeIfRequired(ctrl, () => { string c = null; });
        }

        [Fact]
        public void InvokeIfRequiredTest1_NullAction()
        {
            var ctrl = new Control();
            Action act = null;
            ControlExtensions.InvokeIfRequired(ctrl, act);
        }

        [Fact]
        public void InvokeIfRequiredTest1_ControlDisposed()
        {
            using (ShimsContext.Create())
            {
                bool getCalled = false;
                bool actCalled = false;
                var shimCtrl = new ShimControl
                {
                    IsDisposedGet = () =>
                    {
                        getCalled = true;
                        return true;
                    }
                };

                Control ctrl = shimCtrl.Instance;
                Action act = () => actCalled = true;
                ctrl.InvokeIfRequired(act);

                Assert.True(getCalled, "IsDisposedGet not called.");
                Assert.False(actCalled, "Action was called.");
            }
        }

        [Fact]
        public void InvokeIfRequiredTest1_ExecuteNoRequired()
        {
            bool actCalled = false;

            var ctrl = new Control();
            Action act = () => actCalled = true;
            ctrl.InvokeIfRequired(act);

            Assert.True(actCalled, "Action was called.");
        }

        [Fact]
        public void InvokeIfRequiredTest1_ExecuteIsRequired()
        {
            using (ShimsContext.Create())
            {
                bool invokeCalled = false;
                var shimCtrl = new ShimControl
                {
                    InvokeRequiredGet = () => true,
                    IsDisposedGet = () => false,
                    BeginInvokeDelegate = del =>
                    {
                        invokeCalled = true;
                        return null;
                    }
                };

                var ctrl = shimCtrl.Instance;
                Action act = () => { };
                ctrl.InvokeIfRequired(act);

                Assert.True(invokeCalled, "Invoke Action was not called.");
            }
        }

        [Fact]
        public void InvokeIfRequiredTest2_NullControl()
        {
            Control ctrl = null;
            ControlExtensions.InvokeIfRequired(ctrl, ctrl2 => { });
        }

        [Fact]
        public void InvokeIfRequiredTest2_NullAction()
        {
            var ctrl = new Control();
            Action<Control> act = null;
            ControlExtensions.InvokeIfRequired(ctrl, act);
        }

        [Fact]
        public void InvokeIfRequiredTest2_ControlDisposed()
        {
            using (ShimsContext.Create())
            {
                bool getCalled = false;
                bool actCalled = false;
                var shimCtrl = new ShimControl
                {
                    IsDisposedGet = () =>
                    {
                        getCalled = true;
                        return true;
                    }
                };

                var ctrl = shimCtrl.Instance;
                Action<Control> act = ctrl2 => actCalled = true;
                ctrl.InvokeIfRequired(act);

                Assert.True(getCalled, "IsDisposedGet not called.");
                Assert.False(actCalled, "Action was called.");
            }
        }

        [Fact]
        public void InvokeIfRequiredTest2_ExecuteNoRequired()
        {
            bool actCalled = false;

            var ctrl = new Control();
            Action<Control> act = ctrl2 => actCalled = true;
            ctrl.InvokeIfRequired(act);

            Assert.True(actCalled, "Action was called.");
        }

        [Fact]
        public void InvokeIfRequiredTest2_ExecuteIsRequired()
        {
            using (ShimsContext.Create())
            {
                bool invokeCalled = false;
                var shimCtrl = new ShimControl
                {
                    InvokeRequiredGet = () => true,
                    IsDisposedGet = () => false,
                    BeginInvokeDelegateObjectArray = (del, parms) =>
                    {
                        invokeCalled = true;
                        return null;
                    }
                };

                var ctrl = shimCtrl.Instance;
                Action<Control> act = ctrl2 => { };
                ctrl.InvokeIfRequired(act);

                Assert.True(invokeCalled, "Invoke Action was not called.");
            }
        }

        #endregion InvokeIfRequired Tests

        #region ClearCueText Tests

        [Fact]
        public void ClearCueTextTest_NullControl()
        {
            Control ctrl = null;
            ControlExtensions.ClearCueText(ctrl);
        }

        #endregion ClearCueText Tests

        #region SetCueText Tests

        [Fact]
        public void SetCueTextTest_NullControl()
        {
            Control ctrl = null;
            ControlExtensions.SetCueText(ctrl, "Hello");
        }

        #endregion SetCueText Tests

        #region SetToolTip Tests

        /// <summary>
        /// Verifies an exception is thrown when called with a null control.
        /// </summary>
        [Fact]
        public void SetToolTipTest_NullControl()
        {
            Control ctrl = null;
            Assert.Throws<ArgumentNullException>(() => ctrl.SetToolTip("Hello"));
        }

        /// <summary>
        /// Verifies the same control and text is passed to the control to set
        /// the tool tip as is passed in.
        /// </summary>
        [Fact]
        public void SetToolTipTest_GoodTest()
        {
            using (ShimsContext.Create())
            {
                string textPassed = null;
                Control controlPassed = null;

                ShimToolTip.AllInstances.SetToolTipControlString = (instance, ctrl, caption) =>
                    {
                        textPassed = caption;
                        controlPassed = ctrl;
                    };

                Control inputControl = new TextBox { Name = "TxtInput" };
                inputControl.SetToolTip("Tooltip Text");

                Assert.Equal("Tooltip Text", textPassed);
                Assert.Equal(inputControl.Name, controlPassed.Name);
            }
        }

        #endregion SetToolTip Tests
    }
}