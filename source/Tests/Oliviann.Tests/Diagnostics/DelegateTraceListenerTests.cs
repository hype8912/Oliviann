namespace Oliviann.Tests.Diagnostics
{
    #region Usings

    using System;
    using Oliviann.Diagnostics;
    using Xunit;

    #endregion Usings

    [Trait("Category", "CI")]
    public class DelegateTraceListenerTests
    {
        [Fact]
        public void TraceListenerWriteTest_NullMessageNullCategory()
        {
            string message = "123456";
            Action<string> act = s => message = s;

            var listener = new DelegateTraceListener(act, "Trace1");
            listener.Write(null, null);

            Assert.Equal("123456", message);
        }

        [Fact]
        public void TraceListenerWriteTest_NullMessage()
        {
            string message = "123456";
            Action<string> act = s => message = s;

            var listener = new DelegateTraceListener(act, "Trace1");
            listener.Write(null, "Trace1");

            Assert.Null(message);
        }

        [Fact]
        public void TraceListenerWriteTest_StringMessage()
        {
            string message = "123456";
            Action<string> act = s => message = s;

            var listener = new DelegateTraceListener(act, "Trace1");
            listener.Write("Here's a message", "Trace1");

            Assert.Equal("Here's a message", message);
        }

        [Fact]
        public void TraceListenerWriteLineTest_NullMessageNullCategory()
        {
            string message = "123456";
            Action<string> act = s => message = s;

            var listener = new DelegateTraceListener(act, "Trace1");
            listener.WriteLine(null, null);

            Assert.Equal("123456", message);
        }

        [Fact]
        public void TraceListenerWriteLineTest_NullMessage()
        {
            string message = "123456";
            Action<string> act = s => message = s;

            var listener = new DelegateTraceListener(act, "Trace1");
            listener.WriteLine(null, "Trace1");

            Assert.Equal(Environment.NewLine, message);
        }

        [Fact]
        public void TraceListenerWriteLineTest_StringMessage()
        {
            string message = "123456";
            Action<string> act = s => message = s;

            var listener = new DelegateTraceListener(act, "Trace1");
            listener.WriteLine("Here's a message", "Trace1");

            Assert.Equal("Here's a message\r\n", message);
        }
    }
}