namespace Oliviann.Tests.Windows.Forms
{
    #region Usings

    using System.Windows.Forms;
    using Oliviann.Windows.Forms;
    using Xunit;

    #endregion Usings

    [Trait("Category", "Developer")]
    [Trait("Category", "SkipWhenLiveUnitTesting")]
    public class ComputerBrowserDialogTest
    {
        [Fact(Skip = "Ignore")]
        public void Test_ComputerBrowserDialog_Yes()
        {
            var d = new ComputerBrowserDialog();
            var dResult = d.ShowDialog();

            const string ShouldBeValue = @"ietm-fwb-bl8.se.nos.boeing.com";
            string returnedValue = d.SelectedComputer;

            Assert.Equal(DialogResult.OK, dResult);
            Assert.Equal(ShouldBeValue, returnedValue);
            d.DisposeSafe();
        }
    }
}