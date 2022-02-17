namespace Oliviann.Tests.Windows.SystemInformation
{
    #region Usings

    using Oliviann.Windows.Forms.SystemInformation;
    using Xunit;

    #endregion Usings

    [Trait("Category", "CI")]
    public class PowerStatusHelpersTests
    {
        [Fact]
        public void IsRunningOnBatteryTest_Execute()
        {
            // NOTE: Test was designed to be run on a connected workstation. If
            // you are running this test on a laptop running on battery this
            // test will fail.
            Assert.False(PowerStatusHelpers.IsRunningOnBattery(), "Computer is running on battery power.");
        }
    }
}