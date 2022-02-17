namespace Oliviann.Tests.Extensions
{
    #region Usings

    using System;
    using System.ComponentModel;
    using Xunit;

    #endregion Usings

    [Trait("Category", "CI")]
    public class DelegateExtensionsTests
    {
        #region Raises

        [Fact]
        public void RaiseEvent_EventHandler_NullHandler()
        {
            EventHandler handler = null;
            handler.RaiseEvent(null, EventArgs.Empty);
        }

        [Fact]
        public void RaiseEvent_EventHandler_NullSender()
        {
            object sender = new object();
            EventArgs args = null;

            EventHandler handler = (s, e) => { sender = s; args = e; };
            handler.RaiseEvent(null, EventArgs.Empty);

            Assert.Null(sender);
            Assert.Equal(EventArgs.Empty, args);
        }

        [Fact]
        public void RaiseEvent_PropertyChangedEventHandler_NullHandler()
        {
            PropertyChangedEventHandler handler = null;
            handler.RaiseEvent(null, new PropertyChangedEventArgs(string.Empty));
        }

        [Fact]
        public void RaiseEvent_PropertyChangedEventHandler_NullSender()
        {
            object sender = new object();
            PropertyChangedEventArgs args = null;

            PropertyChangedEventHandler handler = (s, e) => { sender = s; args = e; };
            handler.RaiseEvent(null, new PropertyChangedEventArgs("Test"));

            Assert.Null(sender);
            Assert.Equal("Test", args.PropertyName);
        }

        [Fact]
        public void RaiseEvent_EventHandlerT_NullHandler()
        {
            EventHandler<EventArgs> handler = null;
            handler.RaiseEvent(null, EventArgs.Empty);
        }

        [Fact]
        public void RaiseEvent_EventHandlerT_NullSender()
        {
            object sender = new object();
            EventArgs args = null;

            EventHandler<EventArgs> handler = (s, e) => { sender = s; args = e; };
            handler.RaiseEvent(null, EventArgs.Empty);

            Assert.Null(sender);
            Assert.Equal(EventArgs.Empty, args);
        }

        [Fact]
        public void RaiseEvent_ActionT_NullHandler()
        {
            Action<object, string> handler = null;
            handler.RaiseEvent(null, string.Empty);
        }

        [Fact]
        public void RaiseEvent_ActionT_NullSender()
        {
            object sender = new object();
            string arg = null;

            Action<object, string> handler = (s, e) => { sender = s; arg = e; };
            handler.RaiseEvent(null, "Test");

            Assert.Null(sender);
            Assert.Equal("Test", arg);
        }

        #endregion Raises

        #region Invokes

        #region Action Tests

        [Fact]
        public void InvokeSafe_Action_Null()
        {
            Action act = null;
            act.InvokeSafe();
        }

        [Fact]
        public void InvokeSafe_Action_Invoked()
        {
            int invokedValue = 1;
            Action act = () => invokedValue = 2;
            act.InvokeSafe();

            Assert.Equal(2, invokedValue);
        }

        [Fact]
        public void InvokeSafeT_Action_Null()
        {
            Action<int> act = null;
            act.InvokeSafe(5);
        }

        [Fact]
        public void InvokeSafeT_Action_NullValue()
        {
            string invokedValue = string.Empty;
            Action<string> act = i => invokedValue = i;
            act.InvokeSafe(null);

            Assert.Null(invokedValue);
        }

        [Fact]
        public void InvokeSafeT_Action_Invoked()
        {
            int invokedValue = 1;
            Action<int> act = i => invokedValue = i;
            act.InvokeSafe(2);

            Assert.Equal(2, invokedValue);
        }

        [Fact]
        public void InvokeSafeT_ActionT_NullHandler()
        {
            Action<object, string> handler = null;
            handler.InvokeSafe(null, string.Empty);
        }

        [Fact]
        public void InvokeSafeT_ActionT_NullSender()
        {
            object sender = new object();
            string arg = null;

            Action<object, string> handler = (s, e) => { sender = s; arg = e; };
            handler.InvokeSafe(null, "Test");

            Assert.Null(sender);
            Assert.Equal("Test", arg);
        }

        #endregion Action Tests

        #region Func Tests

        [Fact]
        public void InvokeSafeT_Func_Null()
        {
            Func<int, bool> funct = null;
            bool result = funct.InvokeSafe(0);

            Assert.False(result);
        }

        [Fact]
        public void InvokeSafeT_Func_NullValue()
        {
            Func<string, bool> funct = s => s == string.Empty;
            bool result = funct.InvokeSafe(null);

            Assert.False(result);
        }

        [Fact]
        public void InvokeSafeT_Func_Invoked()
        {
            Func<int, bool> funct = i => i == 6;

            Assert.False(funct.InvokeSafe(1));
            Assert.True(funct.InvokeSafe(6));
        }

        [Fact]
        public void InvokeSafeT_Func2_Null()
        {
            Func<int, int, bool> funct = null;
            bool result = funct.InvokeSafe(0, 0);

            Assert.False(result);
        }

        [Fact]
        public void InvokeSafeT_Func2_NullEquals()
        {
            Func<string, string, bool> funct = (s1, s2) => s1 == s2;
            bool result = funct.InvokeSafe(null, null);

            Assert.True(funct.InvokeSafe(null, null));
            Assert.False(funct.InvokeSafe(null, string.Empty));
            Assert.False(funct.InvokeSafe(string.Empty, null));
        }

        [Fact]
        public void InvokeSafeT_Func2_Values()
        {
            int value1 = 99;
            int value2 = 2;

            Func<int, int, bool> funct = (i1, i2) =>
                {
                    value1 = i1;
                    value2 = i2;
                    return i1 == 6;
                };

            Assert.False(funct.InvokeSafe(10, 5));
            Assert.Equal(10 ,value1);
            Assert.Equal(5, value2);

            Assert.True(funct.InvokeSafe(6, 10));
            Assert.Equal(6, value1);
            Assert.Equal(10, value2);
        }

        #endregion Func Tests

        #endregion Invokes
    }
}