namespace Oliviann.Tests.Timers
{
    #region Usings

    using System;
    using System.Timers;
    using Oliviann.Timers;
    using Xunit;

    #endregion Usings

    [Trait("Category", "CI")]
    public class TimerExtensionsTests
    {
        [Fact]
        public void TimerResetTest_Null()
        {
            Timer tmr = null;
            Assert.Throws<ArgumentNullException>(() => tmr.Reset());
        }

        [Fact]
        public void TimerResetTest_Work()
        {
            bool resultCalled = false;
            var tmr = new Timer(4000) { Enabled = true, AutoReset = false };
            tmr.Elapsed += (sender, args) => { resultCalled = true; };
            tmr.Start();

            System.Threading.Thread.Sleep(2000);
            Assert.False(resultCalled, "Timer elapsed before set amount of time.");

            tmr.Reset();
            System.Threading.Thread.Sleep(2000);
            Assert.False(resultCalled, "Timer elapsed early after reset.");

            System.Threading.Thread.Sleep(3000);
            Assert.True(resultCalled, "Timer did not elapse after specified time limit.");
        }
    }
}